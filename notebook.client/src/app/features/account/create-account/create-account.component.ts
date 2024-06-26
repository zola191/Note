import { Component, OnDestroy } from '@angular/core';
import { AccountRequest } from '../models/account-request.model';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-create-account',
  templateUrl: './create-account.component.html',
  styleUrl: './create-account.component.css',
})
export class CreateAccountComponent implements OnDestroy {
  model: AccountRequest;
  private addAccountSubscription?: Subscription;

  constructor(
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.model = {
      email: '',
      password: '',
    };
  }

  onFormSubmit() {
    this.addAccountSubscription = this.authService
      .createAccount(this.model)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/account/login');
        },
        error: (response) => {
          this.snackBar.open(
            'Пользователь с таким логином уже существует',
            'close',
            {
              duration: 3000,
              panelClass: ['snackbar-1'],
            }
          );
        },
      });
  }

  redirectToLoginPage() {
    this.router.navigateByUrl('/account/login');
  }

  ngOnDestroy(): void {
    this.addAccountSubscription?.unsubscribe();
  }
}
