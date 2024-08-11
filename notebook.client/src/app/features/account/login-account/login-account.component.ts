import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { Observable, Subscription } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CookieService } from 'ngx-cookie-service';
import { LoginRequest } from '../models/loginRequest.model';
import { CredentialResponse } from 'google-one-tap';
import { LoginWithGoogleRequest } from '../models/loginWithGoogleRequest';
import { GoogleAuthService } from '../services/google-auth.service';
import { AppState } from '../store/store';
import { Store } from '@ngrx/store';
import { AccountResponse } from '../models/account-response.model';
import * as AccountActions from '../store/actions';

@Component({
  selector: 'app-login-account',
  templateUrl: './login-account.component.html',
  styleUrl: './login-account.component.css',
})
export class LoginAccountComponent implements OnInit {
  loginWithGoogleRequest: LoginWithGoogleRequest;
  model: LoginRequest;
  account$: Observable<AccountResponse>;
  isLoading$: Observable<boolean>;
  private addAccountSubscription?: Subscription;

  constructor(
    private authService: UserService,
    private googleAuthService: GoogleAuthService,
    private router: Router,
    private snackBar: MatSnackBar,
    private cookie: CookieService,
    private store: Store<AppState>
  ) {
    this.model = {
      email: '',
      password: '',
    };
    this.loginWithGoogleRequest = {
      credential: '',
    };
    this.account$ = this.store.select((state) => state.account.account);
    this.isLoading$ = this.store.select((state) => state.account.loading);
    this.loadAccount();
  }
  ngOnInit(): void {
    //@ts-ignore
    window.onGoogleLibraryLoad = () => {
      //@ts-ignore
      google.accounts.id.initialize({
        client_id:
          '394585500781-b061fjd4rb101nopvi5el26s8vf22s95.apps.googleusercontent.com',
        callback: this.handleCredentialsResponse.bind(this),
        auto_select: false,
        cancel_on_tap_outside: true,
      });
      //@ts-ignore
      google.accounts.id.renderButton(
        //@ts-ignore
        document.getElementById('google-btn'),
        {
          theme: 'outline',
          size: 'large',
          width: '100%',
        }
      );
      //@ts-ignore
      google.accounts.id.prompt((notification: PromtMomentNotification) => {});
    };
  }

  handleCredentialsResponse(response: CredentialResponse) {
    this.cookie.set('token', response.credential);
    console.log(response.credential);

    this.loginWithGoogleRequest.credential = response.credential;
    this.authService.loginWithGoogle(this.loginWithGoogleRequest).subscribe({
      next: (res) => {
        this.cookie.set('email', res.email);
        this.router.navigateByUrl(`/notebook/notebook-list`);
      },
      error: (res) => {
        this.snackBar.open('Неправильный логин или пароль', 'close', {
          duration: 3000,
          panelClass: ['snackbar-1'],
        });
      },
    });
  }

  onFormSubmit() {
    this.addAccountSubscription = this.authService.login(this.model).subscribe({
      next: (response) => {
        this.cookie.set('email', response.email);
        this.cookie.set('token', response.token);
        this.router.navigateByUrl(`/notebook/notebook-list`);
      },
      error: (response) => {
        console.log(console.error);
        this.snackBar.open(response.error, 'close', {
          duration: 3000,
          panelClass: ['snackbar-1'],
        });
      },
    });
  }

  redirectToRegisterPage() {
    this.router.navigateByUrl('/account/create');
  }

  ngOnDestroy(): void {
    this.addAccountSubscription?.unsubscribe();
  }

  externalLogin = () => {
    this.googleAuthService.signInWithGoogle();
    this.googleAuthService.extAuthChanged.subscribe((user) => {
      // const externalAuth: ExternalAuthDto = {
      //   provider: user.provider,
      //   idToken: user.idToken,
      // };
      // this.validateExternalAuth(externalAuth);
    });
  };

  loadAccount() {
    this.store.dispatch(AccountActions.loadAccount());
  }
}
