import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { AccountRestoreRequest } from '../models/account-restore.model';

type EmailState = { email: string };

@Injectable({
  providedIn: 'root',
})
export class DataService {
  private dataTransferSubject = new Subject<AccountRestoreRequest>();
  dataTransferObservable = this.dataTransferSubject.asObservable();

  constructor() {}

  push(obj: AccountRestoreRequest) {
    this.dataTransferSubject.next(obj);
  }
  // state = patchState(this.state, { email: obj.email });
}