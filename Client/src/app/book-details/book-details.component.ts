import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { ToastrService } from 'ngx-toastr';
import { Book } from '../book/book';
import { BookService } from '../book/book.service';
import { Record } from '../record/record';
import { RecordService } from '../record/record.service';
import { currentUser } from '../reducers/auth.selectors';

@Component({
  selector: 'app-book-details',
  templateUrl: './book-details.component.html',
  styleUrls: ['./book-details.component.css'],
  providers: [RecordService, BookService],
})
export class BookDetailsComponent implements OnInit {

  id : string;
  book : Book;
  user: any;
  allBookStatus: Record[] = [];

  constructor(
    private recordService: RecordService, 
    private bookService: BookService, 
    private activatedRoute : ActivatedRoute,
    private store: Store, 
    private toastr: ToastrService,
    ) { }

  ngOnInit(): void {
    this.store.pipe(select(currentUser)).subscribe(user => {this.user = user

    });
    this.id = this.activatedRoute.snapshot.paramMap.get('id')!;

    this.getBook(this.id);  
  }

  reload(): void {
    this.ngOnInit()
  }

  getBook(id: string){      
    if(this.user && this.user.role=='Student' )
      this.recordService.getStatus(this.user.id).subscribe(records => { this.allBookStatus = records; });

    this.bookService.getBook(id).subscribe(book => {
      this.book = book;
    }, err => console.log(err));
  }

    handlebookRequest() {
    const newRecord: Record = {
      bookId: this.book.id,
      bookName: this.book.name,
      userId: this.user.id as number,
      email: this.user.email as string,
      status: 'Requested',
      issuedDate: '',
      dueDate: '',
      returnDate: '',
      fineAmount: 0
    } as Record
    this.recordService.addRecord(newRecord).subscribe(id => {
      if(id){
        this.recordService.getStatus(this.user.id).subscribe(records => { this.allBookStatus = records; });
        this.toastr.success('Request sent successfully');
      }
    });
  }

  GetBookStatus() {
    let r: any = this.allBookStatus.filter(bookStatus => bookStatus.bookId == this.book.id) 
    if(r.length > 0){
      return r[0].status
    }
    else {
      return 'Request'
    }
  }
}
