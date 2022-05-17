import { state } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { Observable } from 'rxjs';
import { login } from './reducers/auth.actions';
import { isLoggedIn } from './reducers/auth.selectors';
import jwt_decode from 'jwt-decode';
import { User } from './login/user'
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'], 
})
export class AppComponent implements OnInit {

  title = 'app';
  token: string | null = null

  isLoggedIn$: Observable<boolean>;

  constructor(private store: Store) {

  }

  getUrl()
  {
    return "url('./../assets/Library.jpg')";
  }

  ngOnInit(): void {

    this.token = localStorage.getItem("token");

    if (this.token) {
      const userData: any = jwt_decode<any>(this.token);
      const user: User = {
        id: userData['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid'] as number,
        email: userData['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] as string,
        roleName: userData['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] as string,
      } as User
      this.store.dispatch(login({ user }));
    }

    this.isLoggedIn$ = this.store
      .pipe(
        select(isLoggedIn)
      );

  }

}
