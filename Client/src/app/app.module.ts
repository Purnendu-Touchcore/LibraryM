import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA  } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { BookComponent } from './book/book.component';
import { RecordComponent } from './record/record.component';
import { LoginComponent } from './login/login.component';
import { StoreModule } from '@ngrx/store';
import {EffectsModule} from '@ngrx/effects';
import {AuthEffects} from './reducers/auth.effects';
import { AuthGuard } from './reducers/auth.guard';
import { reducers } from './reducers';
import { MatSelectModule} from '@angular/material/select';
import {MatDialogModule} from '@angular/material/dialog';
import {StoreDevtoolsModule} from '@ngrx/store-devtools';
import { environment } from 'src/environments/environment';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { EditBookDialogComponent } from './edit-book-dialog/edit-book-dialog.component';
import {MatInputModule} from '@angular/material/input';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {MatTableModule} from '@angular/material/table';
import {MatSortModule} from '@angular/material/sort';
import {MatMenuModule} from '@angular/material/menu';;
import { HttpErrorInterceptor } from './http-error.interceptor';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import {MatCardModule} from '@angular/material/card';
import {MatTooltipModule} from '@angular/material/tooltip';
import { BookDetailsComponent } from './book-details/book-details.component';
import { ToastrModule } from 'ngx-toastr';
import { MatPaginatorModule } from '@angular/material/paginator';
import {NgxPaginationModule} from 'ngx-pagination'; 
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    BookComponent,
    RecordComponent,
    LoginComponent,
    EditBookDialogComponent,
    BookDetailsComponent,
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    MatCardModule,
    HttpClientModule,
    FormsModule,
    NgxPaginationModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
    MatSelectModule,
    MatDialogModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatSortModule,
    MatTableModule,
    BsDropdownModule,
    ReactiveFormsModule,
    MatPaginatorModule,
    
    StoreModule.forRoot(reducers),
    EffectsModule.forRoot([AuthEffects]),
    ToastrModule.forRoot({
      timeOut: 1500,
      positionClass: 'toast-bottom-right'
    }),
    StoreDevtoolsModule.instrument({maxAge: 25, logOnly: environment.production}),
    RouterModule.forRoot([
      { path: '', component: LoginComponent, pathMatch: 'full'  },
      { path: 'records', component: RecordComponent, canActivate: [AuthGuard]},
      { path: 'books', component: BookComponent, canActivate: [AuthGuard] },
      {path : "book/:id", component: BookDetailsComponent, canActivate: [AuthGuard]},
      {
        path: '**',
        redirectTo: '/'
      }
    ]),
    BrowserAnimationsModule
  ],
  providers: [
    AuthGuard,
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true },
    { provide: LocationStrategy, useClass: HashLocationStrategy }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
