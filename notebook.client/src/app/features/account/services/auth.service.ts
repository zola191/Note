import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { AccountRequest } from '../models/account-request.model';
import { environment } from '../../../../environments/environment.development';
import { AccountResponse } from '../models/account-response.model';
import { CookieService } from 'ngx-cookie-service';
import { User } from '../models/user.model';
import { AccountRestoreRequest } from '../models/account-restore.model';
import { RestoreAccountResponse } from '../models/account-restore-response.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  // private readonly TOKEN_NAME = 'tasty-cookies';
  $user = new BehaviorSubject<User | undefined>(undefined);
  // get token() {
  //   return localStorage.getItem(this.TOKEN_NAME);
  // }

  constructor(private http: HttpClient, private cookie: CookieService) {}

  createAccount(request: AccountRequest): Observable<AccountResponse> {
    return this.http.post<AccountResponse>(
      `${environment.apiBaseUrl}/api/account/create`,
      request
    );
  }

  login(request: AccountRequest): Observable<AccountResponse> {
    return this.http.post<AccountResponse>(
      `${environment.apiBaseUrl}/api/account/login`,
      request
    );
  }

  setUser(user: User): void {
    this.$user.next(user);
    localStorage.setItem('user-email', user.email);
  }

  getUser(): User | undefined {
    const email = localStorage.getItem('user-email');
    if (email) {
      const user: User = {
        email: email,
      };
      return user;
    }
    return undefined;
  }

  user(): Observable<User | undefined> {
    return this.$user.asObservable();
  }

  logout(): void {
    localStorage.clear();
    this.cookie.delete('Authorization', '/');
    this.$user.next(undefined);
  }

  restore(request: AccountRestoreRequest): Observable<RestoreAccountResponse> {
    return this.http.post<RestoreAccountResponse>(
      `${environment.apiBaseUrl}/api/account/restore`,
      request
    );
  }
}
