import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Book } from './book';
import { environment } from 'src/environments/environment';


@Injectable()
export class BookService {
  booksUrl = environment.baseUrl+'/book';  // URL to web api

  constructor(private http: HttpClient) {}

  getBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(this.booksUrl)
  }
  getBooksByPage(page: number): Observable<Book[]> {
    return this.http.get<Book[]>(`${this.booksUrl}/page/${page}`)
  }
  
  getBook(id: string): Observable<Book> {
    return this.http.get<Book>(`${this.booksUrl}/${id}`)
  }

  searchBooks(searchTerm: string): Observable<Book[]> {
    searchTerm = searchTerm.trim();

    const url = `${this.booksUrl}/search/${searchTerm}`;
    console.log(url)
    return this.http.get<Book[]>(url)
  }


  addBook(book: Book): Observable<number> {
    return this.http.post<number>(this.booksUrl, book)
  }

  deleteBook(id: number): Observable<unknown> {
    const url = `${this.booksUrl}/${id}`; 
    return this.http.delete(url)
  }

  updateBook(book: Book): Observable<any> {

    return this.http.put<any>(`${this.booksUrl}/${book.id}`, book)
  }

  changeBookQnt(bookId: number, amount: number): Observable<unknown>{

    return this.http.get(`${this.booksUrl}/${bookId}/${amount}`)
  }
}
