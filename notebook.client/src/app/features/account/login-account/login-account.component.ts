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
import { Store } from '@ngrx/store';
import { AccountResponse } from '../models/account-response.model';
import { setUserEmail } from '../userStore/user-actions';

@Component({
  selector: 'app-login-account',
  templateUrl: './login-account.component.html',
  styleUrl: './login-account.component.css',
})
export class LoginAccountComponent implements OnInit {
  loginWithGoogleRequest: LoginWithGoogleRequest;
  model: LoginRequest;
  private addAccountSubscription?: Subscription;

  constructor(
    private authService: UserService,
    private googleAuthService: GoogleAuthService,
    private router: Router,
    private snackBar: MatSnackBar,
    private cookie: CookieService,
    private userStore: Store
  ) {
    this.model = {
      email: '',
      password: '',
    };
    this.loginWithGoogleRequest = {
      credential: '',
    };
  }
  ngOnInit(): void {}

  onFormSubmit() {
    this.addAccountSubscription = this.authService.login(this.model).subscribe({
      next: (response) => {
        this.cookie.set('email', response.email);
        this.cookie.set('token', response.token);
        this.cookie.set('roleModels', JSON.stringify(response.roleModels));
        this.userStore.dispatch(setUserEmail({ email: response.email }));

        console.log(response.email);
        const isAdmin = response.roleModels.some((f) => f.roleName === 'Admin');
        if (isAdmin) {
          this.router.navigateByUrl(`admin`);
        } else {
          this.router.navigateByUrl(`/notebook/notebook-list`);
        }
      },
      error: (response) => {
        console.log(response);
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
}
