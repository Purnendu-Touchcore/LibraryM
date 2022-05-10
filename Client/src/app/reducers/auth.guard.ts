import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { select, Store } from '@ngrx/store';
import { isLoggedIn } from './auth.selectors';
import { tap } from 'rxjs/operators';
import jwt_decode from 'jwt-decode';
import { logout } from './auth.actions';

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(
        private store: Store,
        private router: Router) {

    }
    token: string | null = null
    isLoggedin: boolean = false;

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Observable<boolean>  {
        this.token = localStorage.getItem("token");
        
        return this.store.pipe(select(isLoggedIn),
            tap(loggedIn => {
                if(loggedIn && this.token && jwt_decode<any>(this.token).exp <= +new Date() ){
                return false
                }
                else{
                    this.store.dispatch(logout());
                    return true
                }
            })
        )
    }

}