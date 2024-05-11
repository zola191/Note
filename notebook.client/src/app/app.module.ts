import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './core/components/navbar/navbar.component';
import { AddNotebookComponent } from './features/notebook/add-notebook/add-notebook.component';
import { EditNotebookComponent } from './features/notebook/edit-notebook/edit-notebook.component';
import { NotebookListComponent } from './features/notebook/notebook-list/notebook-list.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CreateAccountComponent } from './features/account/create-account/create-account.component';
import { LoginAccountComponent } from './features/account/login-account/login-account.component';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { provideAnimations } from '@angular/platform-browser/animations';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    AddNotebookComponent,
    EditNotebookComponent,
    NotebookListComponent,
    CreateAccountComponent,
    LoginAccountComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    MatSnackBarModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [
    provideAnimations(),
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
