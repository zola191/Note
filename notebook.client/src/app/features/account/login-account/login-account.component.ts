import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { Subscription } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CookieService } from 'ngx-cookie-service';
import { loginRequest } from '../models/account-loginRequest.mode';
import { jwtDecode } from 'jwt-decode';
import { CredentialResponse, PromptMomentNotification } from 'google-one-tap';

@Component({
  selector: 'app-login-account',
  templateUrl: './login-account.component.html',
  styleUrl: './login-account.component.css',
})
export class LoginAccountComponent implements OnInit {
  model: loginRequest;
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
    this.authService.loginWithGoogle(response.credential).subscribe({
      next: (res) => {
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
