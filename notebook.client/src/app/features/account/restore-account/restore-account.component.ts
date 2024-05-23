import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Observable, Subscription } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AccountRestoreRequest } from '../models/account-restore.model';
import { DataService } from '../services/data.service.service';
import { patchState, signalState } from '@ngrx/signals';
import { Store } from '@ngrx/store';
import { AppState } from '../state/app.state';
import { selectEmail } from '../state/selector';
import { setEmail } from '../state/actions';

@Component({
  selector: 'app-restore-account',
  templateUrl: './restore-account.component.html',
  styleUrl: './restore-account.component.css',
})
export class RestoreAccountComponent {
  @Output() sendData: EventEmitter<string> = new EventEmitter<string>();
  private restoreAccountSubscription?: Subscription;
  model: AccountRestoreRequest;
  email$: Observable<string>;
  constructor(
    private authService: AuthService,
    private snackBar: MatSnackBar,
    private dataService: DataService,
    private store: Store<AppState>
  ) {
    this.model = {
      email: '',
    };
    this.email$ = this.store.select(selectEmail);
  }

  setEmail(email: string) {
    this.store.dispatch(setEmail({ newEmail: email }));
  }

  onFormSubmit() {
    this.restoreAccountSubscription = this.authService
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
          this.dataService.push(response.accountModel);
          this.setEmail(response.accountModel.email);
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
