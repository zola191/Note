import { Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { createRequest } from '../models/account-createRequest.model';

@Component({
  selector: 'app-create-account',
  templateUrl: './create-account.component.html',
  styleUrl: './create-account.component.css',
})
export class CreateAccountComponent implements OnDestroy {
  form: FormGroup;
  model: createRequest;
  private addAccountSubscription?: Subscription;

  constructor(
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.model = {
      email: '',
      password: '',
      confirmPassword: '',
    };

    this.form = new FormGroup(
      {
        email: new FormControl('', [Validators.required, Validators.email]),
        password: new FormControl('', [Validators.required]),
        confirmPassword: new FormControl('', [Validators.required]),
      },
      {
        validators: this.passwordMatchValidator,
      }
    );
  }

  passwordMatchValidator(control: AbstractControl) {
    return control.get('password')?.value ===
      control.get('confirmPassword')?.value
      ? null
      : { mismatch: true };
  }

  onFormSubmit() {
    this.addAccountSubscription = this.authService
      .createAccount(this.model)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/account/login');
        },
        error: (error) => {
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
