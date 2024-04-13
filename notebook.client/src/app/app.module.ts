import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './core/components/navbar/navbar.component';
import { AddNotebookComponent } from './features/notebook/add-notebook/add-notebook.component';
import { EditNotebookComponent } from './features/notebook/edit-notebook/edit-notebook.component';
import { NotebookListComponent } from './features/notebook/notebook-list/notebook-list.component';
import { FormsModule } from '@angular/forms';
import { CreateAccountComponent } from './features/account/create-account/create-account.component';
import { LoginAccountComponent } from './features/account/login-account/login-account.component';

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
  imports: [BrowserModule, HttpClientModule, AppRoutingModule, FormsModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
