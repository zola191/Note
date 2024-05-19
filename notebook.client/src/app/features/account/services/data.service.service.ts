import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { AccountRestoreRequest } from '../models/account-restore.model';
import { PartialStateUpdater, patchState, signalState } from '@ngrx/signals';
import { select } from '@ngrx/store';
type EmailState = { email: string };

@Injectable({
  providedIn: 'root',
})
export class DataService {
  state = signalState<EmailState>({
    email: '',
  });

  private dataTransferSubject = new Subject<AccountRestoreRequest>();
  dataTransferObservable = this.dataTransferSubject.asObservable();

  constructor() {}

  push(obj: AccountRestoreRequest) {
    this.dataTransferSubject.next(obj);
  }
  // state = patchState(this.state, { email: obj.email });

  saveEmail(newEmail: string): PartialStateUpdater<{ email: String }> {
    return (state) => ({ email: { ...state.email, newEmail } });
  }
}
