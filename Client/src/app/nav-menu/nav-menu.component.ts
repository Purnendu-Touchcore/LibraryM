import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { User } from '../login/user';
import { logout } from '../reducers/auth.actions';
import { currentUser } from '../reducers/auth.selectors';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  isExpanded = false;
  user: any;
  constructor(private store: Store, private router: Router) { }

  ngOnInit() {
    this.store.pipe(select(currentUser)).subscribe(user => this.user = user);
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    this.store.dispatch(logout());
  }
}
