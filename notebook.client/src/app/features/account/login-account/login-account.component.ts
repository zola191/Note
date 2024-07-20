import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { Subscription } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CookieService } from 'ngx-cookie-service';
import { LoginAccountRequest } from '../models/loginRequest.model';
import { CredentialResponse, PromptMomentNotification } from 'google-one-tap';
import { LoginWithGoogleRequest } from '../models/loginWithGoogleRequest';

@Component({
  selector: 'app-login-account',
  templateUrl: './login-account.component.html',
  styleUrl: './login-account.component.css',
})
export class LoginAccountComponent implements OnInit {
  loginWithGoogleRequest: LoginWithGoogleRequest;
  model: LoginAccountRequest;
  private addAccountSubscription?: Subscription;

  constructor(
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar,
    private cookie: CookieService
  ) {
    this.model = {
      email: '',
      password: '',
    };
    this.loginWithGoogleRequest = {
      credential: '',
    };
  }
  ngOnInit(): void {
    //@ts-ignore
    window.onGoogleLibraryLoad = () => {
      //@ts-ignore
      google.accounts.id.initialize({
        client_id: '',
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
    this.loginWithGoogleRequest.credential = response.credential;
    this.authService.loginWithGoogle(this.loginWithGoogleRequest).subscribe({
      next: (res) => {
        this.cookie.set('email', res.email);
        this.cookie.set('token', res.token);
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
        this.snackBar.open('Неправильный логин или пароль', 'close', {
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
}
