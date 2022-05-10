import { Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { User } from './user';
import { UserService } from './user.service';
import { Store, select } from '@ngrx/store';
import {
  login,
} from '../reducers/auth.actions';
import { Router } from '@angular/router';
import { isLoggedIn } from '../reducers/auth.selectors';
import { Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  providers: [UserService],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {


  isLoggedIn$: Observable<boolean>;
  constructor(private toastr: ToastrService, private fb: FormBuilder, private userService: UserService, private store: Store, private router: Router) { }
  user: User | null = null;
  errorMessage: string;
  successMessage: string;
  showLoginForm = true;
  form = this.fb.group({
    userName: '',
    email: '',
    password: '',
  });

  ngOnInit(): void {
    this.errorMessage = '';
    this.store.select(isLoggedIn).subscribe((LoggedIn) => {
      if (LoggedIn) this.router.navigateByUrl('/books');
    })

  }

  signIn() {
    
    this.userService
      .signIn({
        email: this.form.value.email,
        password: this.form.value.password
      } as User)
      .subscribe(user => {
        if (user.email === null) return
        this.user = user;
        this.store.dispatch(login({ user }));
        this.router.navigateByUrl('/books');
        localStorage.setItem('token', user.token)
      }
      );
  }

  register() {
    this.successMessage = '';
    this.errorMessage = '';

    this.userService
      .register({
        name: this.form.value.userName,
        email: this.form.value.email,
        role: 'Student',
        password: this.form.value.password
      } as User)
      .subscribe(
        
        res => {this.toastr.success(res.message)
        this.changeForm()
        },
      );

  }

  changeForm() {
    this.showLoginForm = !this.showLoginForm;
    this.form = this.fb.group({
      userName: '',
      email: '',
      password: '',
    });
    this.successMessage = '';
    this.errorMessage = '';
  }
}
