import { AfterViewInit, Component, OnInit, ViewChild, } from '@angular/core';
import { Record } from './record';
import { RecordService } from './record.service';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { currentUser } from '../reducers/auth.selectors';
import { User } from '../login/user';
import { BookService } from '../book/book.service';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-record',
  providers: [RecordService, BookService],
  templateUrl: './record.component.html',
  styleUrls: ['./record.component.css'],
})
export class RecordComponent implements OnInit, AfterViewInit {
  public records: Record[] = [];
  editRecord: Record | undefined;
  returnDate: Date | null = null;
  user: any;
  statusFillter: string = 'All'
  isLoading = false;
  totalRows = 0;
  pageSize = 10;
  pageNumber = 0;
  email: string | null = null;

  displayColumns: string[] = ["id", "email", "bookName", "status", "issuedDate", "dueDate", "returnDate", "fineAmount", 'edit', 'delete'];
  dataSource = new MatTableDataSource(this.records);

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;



  constructor(private store: Store, private recordService: RecordService, private bookService: BookService,) {
  }

  ngOnInit() {
    this.store.pipe(select(currentUser)).subscribe(currentUser => {
      this.user = currentUser
      if (this.user && this.user.roleName != "Admin") {
        // remove email, edit and delete column from table
        this.displayColumns.splice(7)
        this.displayColumns.splice(1, 1)
      }
      this.getRecords()
    })
  }
  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
  }

  pageChanged($event: PageEvent) {
    this.pageSize = $event.pageSize;
    this.pageNumber = $event.pageIndex;
    if((this.email !== null && this.email !== undefined && this.email !== '') || this.user.roleName != 'Admin'){
      let email = this.user.roleName != 'Admin' ? this.user.email : this.email;
      this.recordService
      .getUserRecordsByPage(this.statusFillter, email, this.pageSize, this.pageNumber + 1)
      .subscribe(records => {
        this.records = records
        this.dataSource.data = this.records
        this.totalRows = this.dataSource.data[0].maxRows;
      });
    } else {
      this.getRecords()
    }
    
    this.paginator.length = this.totalRows;
  }


  getRecords(): void {
    
    if (this.user){
    this.isLoading = true;
      if (this.user.roleName == 'Admin') {
        this.recordService.getRecordsByPage(this.statusFillter, this.pageSize, this.pageNumber + 1).subscribe(records => {
          this.records = records;
          this.dataSource.data = records;
          this.isLoading = false;
          this.totalRows = this.dataSource.data[0].maxRows;
        });
      }
      else {
        this.recordService.getUserRecordsByPage(this.statusFillter, this.user.email, this.pageSize, this.pageNumber + 1).subscribe(records => {
          this.records = records
          this.dataSource.data = records;
          this.isLoading = false;
          this.totalRows = this.dataSource.data[0].maxRows;
        });
      }
    }
  }

  applyFilter(s: string) {
    this.statusFillter = s;
    if (s == 'All') {
      this.getRecords()
    } else {

      if (this.user.roleName == 'Admin') {
        this.recordService.getRecordsByPage(s, this.pageSize, this.pageNumber + 1).subscribe(records => {
          this.records = records
          this.dataSource.data = this.records
          this.totalRows = this.dataSource.data[0].maxRows;
        });
      }
      else {
        this.recordService.getUserRecordsByPage(s, this.user.email, this.pageSize, this.pageNumber + 1).subscribe(records => {
          this.records = records
          this.dataSource.data = this.records
          this.totalRows = this.dataSource.data[0].maxRows;
        });
      }
    }
  }

  delete(record: Record): void {
    this.records = this.records.filter(h => h !== record);
    console.log(record.id)
    this.recordService
      .deleteRecord(record.id)
      .subscribe((res: any) => {
        this.getRecords()
      });
  }

  search() {
    if (this.email !== null) {
      this.totalRows = 0;
      this.pageSize = 10;
      this.pageNumber = 0;
      this.email = this.email.trim();
      this.recordService
      .getUserRecordsByPage(this.statusFillter, this.email, this.pageSize, this.pageNumber + 1)
      .subscribe(records => {
        this.records = records
        this.dataSource.data = this.records
        this.totalRows = this.dataSource.data[0].maxRows;
      });
    } else {
      this.getRecords();
    }
  }

  editStatus(status: string) {

    if (this.editRecord && this.editRecord.status !== status) {

      //issuedDate is the current date with format yyyy-mm-dd      
      let d = new Date()
      this.editRecord.issuedDate = d
      let dd = new Date()
      dd.setDate(d.getDate() + 21)
      this.editRecord.dueDate = dd
      this.editRecord.status = status
      var id = this.editRecord.id

      const indexOfRecord = this.records.findIndex(h => h.id === id);

      this.records[indexOfRecord] = this.editRecord;

      this.recordService.updateRecord(this.editRecord).subscribe();

      this.bookService.changeBookQnt(this.editRecord.bookId, -1).subscribe();

      this.editRecord = undefined;
    }
  }

  updateRecordOrActivateForm(record: Record) {
    //Activate form to edit return Date 
    if (this.returnDate === null)
    {
      this.editRecord = record
    }
    else{
      //Update return date
      this.editRecord = record
      this.editReturnedDate()      
    }

  }

  editReturnedDate() {
    if (this.editRecord
      && this.returnDate !== null
      && this.editRecord.returnDate !== this.returnDate
    ) {

      var id = this.editRecord.id;

      let isd = this.editRecord.issuedDate?.toISOString().split('T')[0];
      let rd = this.returnDate?.toString().split('T')[0];
      let dd = this.editRecord.dueDate?.toISOString().split('T')[0];

      const indexOfRecord = this.records.findIndex(h => h.id === id);


      if (rd && isd && dd && indexOfRecord > -1 && rd >= isd) {
        
        this.editRecord.returnDate = this.returnDate;
        this.editRecord.status = "Returned"
        if (rd > dd) {
          this.editRecord.fineAmount = 100
        }
        this.records[indexOfRecord] = this.editRecord;
        
        this.recordService.updateRecord(this.editRecord).subscribe();
        this.bookService.changeBookQnt(this.editRecord.bookId, 1).subscribe();
      }
      this.editRecord = undefined;
      this.returnDate = null;
    }
  }

}


