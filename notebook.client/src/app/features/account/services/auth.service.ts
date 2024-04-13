import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountRequest } from '../models/account-request.model';
import { environment } from '../../../../environments/environment.development';
import { AccountResponse } from '../models/account-response.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient) {}

  login(request: AccountRequest): Observable<AccountResponse> {
    return this.http.post<AccountResponse>(
      `${environment.apiBaseUrl}/api/account/login`,
      request
    );
  }

  createAccount(request: AccountRequest): Observable<void> {
    return this.http.post<void>(
      `${environment.apiBaseUrl}/api/account/create`,
      request
    );
  }
}
