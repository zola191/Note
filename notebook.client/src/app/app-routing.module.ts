import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotebookListComponent } from './features/notebook/notebook-list/notebook-list.component';
import { AddNotebookComponent } from './features/notebook/add-notebook/add-notebook.component';
import { EditNotebookComponent } from './features/notebook/edit-notebook/edit-notebook.component';
import { CreateAccountComponent } from './features/account/create-account/create-account.component';
import { LoginAccountComponent } from './features/account/login-account/login-account.component';
import { RestoreAccountComponent } from './features/account/restore-account/restore-account.component';
import { ChangePasswordComponent } from './features/account/change-password/change-password.component';
import { WelcomeComponent } from './core/components/welcome/welcome.component';

const routes: Routes = [
  {
    path: 'notebook/welcome',
    component: WelcomeComponent,
  },
  {
    path: 'notebook/notebook-list',
    component: NotebookListComponent,
  },
  {
    path: 'notebook/add-notebook',
    component: AddNotebookComponent,
  },
  {
    path: 'notebook/edit-notebook/:id',
    component: EditNotebookComponent,
  },
  {
    path: 'account/login',
    component: LoginAccountComponent,
  },
  {
    path: 'account/create',
    component: CreateAccountComponent,
  },
  {
    path: 'account/restore',
    component: RestoreAccountComponent,
  },
  {
    path: 'account/restore/:id',
    component: ChangePasswordComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
