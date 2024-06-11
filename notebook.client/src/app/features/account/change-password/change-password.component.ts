import { Component, Input, OnInit, Output } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { AccountRequest } from '../models/account-request.model';
import { AuthService } from '../services/auth.service';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AccountChangePassword } from '../models/account-change-password-request.model';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css',
})
export class ChangePasswordComponent implements OnInit {
  form: FormGroup;
  model: AccountChangePassword;
  private addAccountSubscription?: Subscription;

  constructor(
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar,
    private activatedRoute: ActivatedRoute,
    private route: ActivatedRoute
  ) {
    this.model = {
      token: '',
      password: '',
      confirmPassword: '',
    };

    this.form = new FormGroup(
      {
        password: new FormControl('', [Validators.required]),
        confirmPassword: new FormControl('', [Validators.required]),
      },
      {
        validators: this.passwordMatchValidator,
      }
    );
  }

  ngOnInit(): void {}

  passwordMatchValidator(control: AbstractControl) {
    return control.get('password')?.value ===
      control.get('confirmPassword')?.value
      ? null
      : { mismatch: true };
  }

  onFormSubmit() {
    let url = this.route.snapshot.url;
    let token = url[url.length - 1].path;
    this.model.token = token;

    this.route.url.subscribe(([url]) => {
      const { path, parameters } = url;
      console.log(path);
      console.log(parameters);
    });

    this.addAccountSubscription = this.authService
      .changePassword(this.model)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/account/login');
          this.snackBar.open('Пароль успешно заменен', 'close', {
            duration: 3000,
            panelClass: ['snackbar-1'],
          });
        },
        error: (error) => {
          this.snackBar.open(
            'Ошибка при смене пароля, повторите запрос по смене пароля',
            'close',
            {
              duration: 3000,
              panelClass: ['snackbar-1'],
            }
          );
        },
      });
  }
}
