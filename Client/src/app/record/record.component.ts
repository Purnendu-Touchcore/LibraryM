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
export class RecordComponent implements OnInit, AfterViewInit  {
  public records: Record[] = [];
  editRecord: Record | undefined;
  returnDate: string | undefined;
  user: any;
  statusFillter: string = 'All'
  isLoading = false;
  totalRows = 0;
  pageSize = 10;
  currentPage = 0;

  displayColumns: string[] = ["id", "email", "bookName", "status", "issuedDate", "dueDate", "returnDate", "fineAmount",'edit', 'delete'];
  dataSource = new MatTableDataSource(this.records);

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;



  constructor(private store: Store, private recordService: RecordService, private bookService: BookService,) {
  }

  ngOnInit() {
    this.store.pipe(select(currentUser)).subscribe(currentUser => {
      this.user = currentUser
      if(this.user.role != "Admin"){
        this.displayColumns.splice(7)
        this.displayColumns.splice(1,1)
      }
      this.getRecords()
    })
  }
  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator; 
  }

  pageChanged($event: PageEvent){
    this.pageSize = $event.pageSize;
    this.currentPage = $event.pageIndex;
    //this.getRecordsByPage()
  }
  getRecordsByPage(){
    this.recordService.getRecordsByPage(this.pageSize, this.currentPage).subscribe(records => {
      this.records = records;
      this.totalRows = records[0].maxRows;
      this.dataSource.data = this.records;
    });
  }

  getRecords(): void {
    if (this.user)
      if (this.user.role == 'Admin') {
        this.recordService.getRecords().subscribe(records => {
          this.records = records;
          this.dataSource.data = records;
        });
      }
      else {
        this.recordService.getUserRecords(this.user.id).subscribe(records => {
          this.records = records
          this.dataSource.data = records;
        });
      }

  }

  applyFilter(s: string) {
    this.statusFillter = s;
    if (s == 'All') {
      this.getRecords()
    } else {
      
      if (this.user.role == 'Admin') {
        console.log(s)
        this.recordService.getRecords().subscribe(records => { 
          this.records = records.filter(r => r.status == s)
          this.dataSource.data = this.records
        });
      }
      else {
        this.recordService.getUserRecords(this.user.id).subscribe(records => { 
          this.records = records.filter(r => r.status == s) 
          this.dataSource.data = this.records
        });
      }
    }
  }

  delete(record: Record): void {
    this.records = this.records.filter(h => h !== record);
    console.log(record.id)
    this.recordService
      .deleteRecord(record.id)
      .subscribe((res:any)=>{
        this.getRecords()
      });
  }

  // search(searchTerm: string) {

  //   if (searchTerm) {
  //     this.recordService
  //       .searchRecords(searchTerm)
  //       .subscribe(record => this.records = [record]);
  //   } else {
  //     this.getRecords();
  //   }
  // }

  editStatus(status: string) {

    if (this.editRecord && this.editRecord.status !== status) {

      var id = this.editRecord.id;
      let d = new Date();
      this.editRecord.status = status
      //current date with format yyyy-mm-dd
      this.editRecord.issuedDate = d.toISOString().split('T')[0]; 
      d.setDate(d.getDate() + 21);
      this.editRecord.dueDate = d.toISOString().split('T')[0]


      const indexOfRecord = this.records.findIndex(h => h.id === id);
      if (indexOfRecord > -1) {
        this.records[indexOfRecord] = this.editRecord;
        this.recordService
          .updateRecord(this.editRecord)
          .subscribe();
        this.bookService.changeBookQnt(this.editRecord.bookId, -1).subscribe();
      }
      this.editRecord = undefined;
    }
  }

  updateRecordOrActivateForm(record: Record){
    //Activate form to edit return Date 
    if(this.returnDate === undefined)
      this.editRecord = record
    else
    //Update return date
      this.editReturnedDate()
  }

  editReturnedDate() {
    if (this.editRecord 
      && this.returnDate !== undefined
      && this.editRecord.returnDate !== this.returnDate
      ) {

      var id = this.editRecord.id;
      let isd = new Date(this.editRecord.issuedDate);
      let rd = new Date(this.returnDate);
      let dd = new Date(this.editRecord.dueDate);

      const indexOfRecord = this.records.findIndex(h => h.id === id);
      if (indexOfRecord > -1 && +rd >= +isd) {

        this.editRecord.returnDate = this.returnDate;
        this.editRecord.status = "Returned"
        if (+rd > +dd) {
          this.editRecord.fineAmount = 100
        }
        this.records[indexOfRecord] = this.editRecord;
        this.recordService
          .updateRecord(this.editRecord)
          .subscribe();
        this.bookService.changeBookQnt(this.editRecord.bookId, 1).subscribe();
      }
      this.editRecord = undefined;
      this.returnDate = undefined;
    }
  }



}


