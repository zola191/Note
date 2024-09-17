import {
  HTTP_INTERCEPTORS,
  provideHttpClient,
  withInterceptorsFromDi,
} from '@angular/common/http';
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
import { RestoreAccountComponent } from './features/account/restore-account/restore-account.component';
import { ChangePasswordComponent } from './features/account/change-password/change-password.component';
import { NgxsModule } from '@ngxs/store';
import { MatDialogModule } from '@angular/material/dialog';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { MatButtonModule } from '@angular/material/button';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { provideNativeDateAdapter } from '@angular/material/core';
import { WelcomeComponent } from './core/components/welcome/welcome.component';
import { CabinetComponent } from './features/account/cabinet/cabinet.component';
import { provideOAuthClient } from 'angular-oauth2-oidc';
import {
  GoogleLoginProvider,
  SocialAuthServiceConfig,
} from '@abacritt/angularx-social-login';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { StoreModule } from '@ngrx/store';
import { AlertModalComponent } from './features/notebook/alert-modal/alert-modal.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AdminComponent } from './features/admin/admin/admin.component';
import { CurrentUserComponent } from './features/admin/current-user/current-user.component';
import { AdminEditNotebookComponent } from './features/admin/admin-edit-notebook/admin-edit-notebook.component';
import { userReducer } from './features/account/userStore/user-reducer';
import { NoteChangeLogComponent } from './features/admin/note-change-log/note-change-log.component';
import { LogFormatterPipe } from './features/admin/pipes/logFormatterPipe';
import { ExportChangeLogComponent } from './features/admin/export-change-log/export-change-log.component';
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS, MomentDateAdapter } from '@angular/material-moment-adapter';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_DATE_LOCALE,
} from '@angular/material/core';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    AddNotebookComponent,
    EditNotebookComponent,
    NotebookListComponent,
    CreateAccountComponent,
    LoginAccountComponent,
    RestoreAccountComponent,
    ChangePasswordComponent,
    WelcomeComponent,
    CabinetComponent,
    AlertModalComponent,
    AdminComponent,
    CurrentUserComponent,
    AdminEditNotebookComponent,
    NoteChangeLogComponent,
    LogFormatterPipe,
    ExportChangeLogComponent,
  ],
  bootstrap: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    MatSnackBarModule,
    FormsModule,
    ReactiveFormsModule,
    NgxsModule,
    MatDialogModule,
    BrowserModule,
    MatDialogModule,
    MatButtonModule,
    NgbModule,
    CommonModule,
    MatDatepickerModule,
    MatInputModule,
    MatFormFieldModule,
    MatToolbarModule,
    MatProgressBarModule,
    StoreModule.forRoot({ user: userReducer }),
    BrowserAnimationsModule,
    MatProgressSpinnerModule,
  ],
  providers: [
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE],
    },
    { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } },
    provideAnimations(),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        lang: 'en',
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider('clientId'),
          },
        ],
        onError: (err) => {
          console.error(err);
        },
      } as SocialAuthServiceConfig,
    },
    provideAnimationsAsync(),
    provideHttpClient(withInterceptorsFromDi()),
    provideNativeDateAdapter(),
    provideOAuthClient(),
  ],
})
export class AppModule {}
