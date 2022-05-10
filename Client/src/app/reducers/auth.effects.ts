import {Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {login, logout} from './auth.actions';
import {tap} from 'rxjs/operators';
import {Router} from '@angular/router';

@Injectable()
export class AuthEffects {

    logout$ = createEffect(() =>
        this.actions$
            .pipe(
                ofType(logout),
                tap(action => {
                    this.router.navigateByUrl('/');
                    localStorage.removeItem('token');
                })
            )
    , {dispatch: false});


    constructor(private actions$: Actions,
                private router: Router) {

    }

}
