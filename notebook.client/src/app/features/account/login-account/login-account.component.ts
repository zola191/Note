import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { Subscription } from 'rxjs';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarModule,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import { CookieService } from 'ngx-cookie-service';
import { loginRequest } from '../models/account-loginRequest.mode';

@Component({
  selector: 'app-login-account',
  templateUrl: './login-account.component.html',
  styleUrl: './login-account.component.css',
})
export class LoginAccountComponent {
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

  onFormSubmit() {
    this.addAccountSubscription = this.authService.login(this.model).subscribe({
      next: (response) => {
        console.log(response.token);
        this.cookie.set('token', response.token);
        this.router.navigateByUrl(`/notebook/notebook-list`);

        this.authService.setUser({
          email: response.email,
        });
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
