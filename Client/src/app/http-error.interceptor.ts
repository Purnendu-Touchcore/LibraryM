import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Store } from '@ngrx/store';
import { logout } from './reducers/auth.actions';
import jwt_decode from 'jwt-decode';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

    constructor(private toastr: ToastrService, private store: Store) {

    }
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // add auth header with jwt if user is logged in
        const token = localStorage.getItem("token");

        if (token) {
            request = request.clone({
                setHeaders: {
                    'Ocp-Apim-Subscription-Key': '54e5c1223d2a47b59a9481cb50ae719d',
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`
                }
            });
        } else {
            request = request.clone({
                setHeaders: {
                    'Ocp-Apim-Subscription-Key': '54e5c1223d2a47b59a9481cb50ae719d',
                    'Content-Type': 'application/json'
                }
            });
        }
        return next.handle(request)
            .pipe<any>(
                catchError((error: HttpErrorResponse) => {
                    console.log(error)
                    if (error) {
                       
                        if (error.error.message) {
                            this.toastr.error(error.error.message);
                        }
                        else {

                            if (token && jwt_decode<any>(token).exp < Date.now() / 1000) {
                                this.toastr.error('Your session has expired. Please login again.');
                            }
                            else {
                                this.toastr.error('Something went wrong.');
                            }
                            this.store.dispatch(logout());
                        }
                    }
                    return throwError(error);
                })
            );
    }
}
