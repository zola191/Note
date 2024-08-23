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
  ngOnInit(): void {}

  onFormSubmit() {
    this.addAccountSubscription = this.authService.login(this.model).subscribe({
      next: (response) => {
        // console.log(response);
        // console.log(response.email);
        // console.log(response.roleModels);

        this.cookie.set('email', response.email);
        this.cookie.set('token', response.token);
        this.cookie.set('roleModels', JSON.stringify(response.roleModels));

        // const roles = response.roleModels?.map((role) => role.roleName);

        // if (roles && roles.length > 0) {
        //   this.cookie.set('roleModels', JSON.stringify(roles));
        // }
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

  loadAccount() {
    this.store.dispatch(AccountActions.loadAccount());
  }
}
