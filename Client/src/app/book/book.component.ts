import { Statement } from '@angular/compiler';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import {  currentUser } from '../reducers/auth.selectors';
import { Book } from './book';
import { BookService } from './book.service';
import { Record } from './../record/record';
import { RecordService } from '../record/record.service';
import { User } from '../login/user';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { EditBookDialogComponent } from './../edit-book-dialog/edit-book-dialog.component';
import { Router } from '@angular/router';



@Component({
  selector: 'app-book',
  providers: [BookService, RecordService],
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.css'],  
})
export class BookComponent implements OnInit {

  public books: Book[];
  p: number = 1;
  editBook: Book | undefined;
  bookName: string = '';
  bookQnt: number = 0;
  bookAQnt: number = 0;
  user: any;
  allBookStatus: Record[] = [];

  constructor(
    private store: Store, 
    private bookService: BookService, 
    private recordService: RecordService, 
    private dialog: MatDialog, 
    private router: Router) { }
    
    total: number = 0;
  

  ngOnInit() {
    this.store.pipe(select(currentUser)).subscribe(user => this.user = user);

    this.getBooksByPageNo(this.p)

  }

  getBooksByPageNo(event: number){
    this.p = event;
    
    this.bookService.getBooksByPage(this.p).subscribe(books => {
      this.books = books;
      this.total = books[0].maxRows;
      if(this.user.role !== 'Admin'){
        this.books = this.books.filter(b => b.totalQuantity !== 0)
      }
    });
  }

  getBooks(): void {
    this.bookService.getBooks()
      .subscribe(books => {
        this.books = books;
        if(this.user.role != 'Admin'){
          
          this.books = this.books.filter(b => b.totalQuantity !== 0)
        }
      });
  }
  viewBookDetails(id: number) {
    this.router.navigateByUrl(`/book/${id}`);
  }

  add(): void {

    const dialogConfig = this.defaultDialogConfig();

    dialogConfig.data = {
      dialogTitle: "Add Book",
      mode: 'create'
    };

    this.dialog.open(EditBookDialogComponent, dialogConfig)
    .afterClosed()
    .subscribe(() =>  this.getBooks());
  }



  delete(book: Book): void {
    this.books = this.books.filter(h => h !== book);
    this.bookService
      .deleteBook(book.id)
      .subscribe();
  }

  search(searchTerm: string) {

    if (searchTerm) {
      this.bookService
        .searchBooks(searchTerm)
        .subscribe(books => this.books = books);
    } else {
      this.getBooks();
    }
  }

  openEditDialog(book: any) {
    const dialogConfig = this.defaultDialogConfig();

    dialogConfig.data = {
      dialogTitle: "Edit Book",
      book,
      mode: 'update'
    };

    this.dialog.open(EditBookDialogComponent, dialogConfig)
      .afterClosed()
      .subscribe(() =>  this.getBooks());
  }

  defaultDialogConfig() {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = '400px';

    return dialogConfig;
  }

}


