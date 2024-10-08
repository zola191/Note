import { Component, EventEmitter, Output } from '@angular/core';
import { UserService } from '../services/user.service';
import { Subscription } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AccountRestoreRequest } from '../models/account-restore.model';

@Component({
  selector: 'app-restore-account',
  templateUrl: './restore-account.component.html',
  styleUrl: './restore-account.component.css',
})
export class RestoreAccountComponent {
  @Output() sendData: EventEmitter<string> = new EventEmitter<string>();
  private restoreAccountSubscription?: Subscription;
  model: AccountRestoreRequest;
  constructor(private userService: UserService, private snackBar: MatSnackBar) {
    this.model = {
      email: '',
    };
  }

  onFormSubmit() {
    this.restoreAccountSubscription = this.userService
      .restore(this.model)
      .subscribe({
        next: (response) => {
          this.snackBar.open(
            'Запрос на смену пароля направлен на указанную почту',
            'close',
            {
              duration: 3000,
              panelClass: ['snackbar-1'],
            }
          );
        },
        error: (error) => {
          this.snackBar.open(
            'Пользователь с такой почтой отсутствует',
            'close',
            {
              duration: 3000,
              panelClass: ['snackbar-1'],
            }
          );
        },
      });
  }

  sendDataToRestorePassword() {
    this.sendData.emit(this.model.email);
  }

  ngOnDestroy(): void {
    this.restoreAccountSubscription?.unsubscribe();
  }
}
