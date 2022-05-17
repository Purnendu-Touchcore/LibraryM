import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import jwt_decode from 'jwt-decode';
import { User } from './user';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable()
export class UserService {
  usersUrl = environment.baseUrl+'/user';  // URL to web api

  constructor(private http: HttpClient) { }


  signIn(user: User): Observable<User> {
    return this.http.post<User>(`${this.usersUrl}/login`, user)
  }


  register(user: User): Observable<any> {
    return this.http.post<any>(`${this.usersUrl}/register`, user)
  }


}
