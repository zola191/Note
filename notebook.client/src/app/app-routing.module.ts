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
import { CabinetComponent } from './features/account/cabinet/cabinet.component';
import { AdminComponent } from './features/admin/admin/admin.component';
import { roleGuard } from './features/account/guards/role.guard';
import { CurrentUserComponent } from './features/admin/current-user/current-user.component';
import { AdminEditNotebookComponent } from './features/admin/admin-edit-notebook/admin-edit-notebook.component';

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
    path: 'user/login',
    component: LoginAccountComponent,
  },
  {
    path: 'user/create',
    component: CreateAccountComponent,
  },
  {
    path: 'user/restore',
    component: RestoreAccountComponent,
  },
  {
    path: 'user/restore/:id',
    component: ChangePasswordComponent,
  },
  {
    path: 'user/cabinet',
    component: CabinetComponent,
  },
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [roleGuard],
  },
  {
    path: 'admin/info/:email',
    component: CurrentUserComponent,
    canActivate: [roleGuard],
  },
  {
    path: 'admin/edit-notebook/:email/:id',
    component: AdminEditNotebookComponent,
    canActivate: [roleGuard],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
