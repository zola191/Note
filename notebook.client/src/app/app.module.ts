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
import { provideStore, StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { UserService } from './features/account/services/user.service';
import { AccountReducer } from './features/account/store/reducer';
import { AlertModalComponent } from './features/notebook/alert-modal/alert-modal.component';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
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
    StoreModule.forRoot({ account: AccountReducer }),
    EffectsModule.forRoot([]),
    MatProgressSpinnerModule
  ],
  providers: [
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
