import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { createRequest } from '../models/account-createRequest.model';
import { environment } from '../../../../environments/environment.development';
import { AccountResponse } from '../models/account-response.model';
import { CookieService } from 'ngx-cookie-service';
import { User } from '../models/user.model';
import { AccountRestoreRequest } from '../models/account-restore.model';
import { RestoreAccountResponse } from '../models/account-restore-response.model';
import { AccountChangePassword } from '../models/account-change-password-request.model';
import { LoginRequest } from '../models/loginRequest.model';
import { jwtDecode } from 'jwt-decode';
import { LoginWithGoogleRequest } from '../models/loginWithGoogleRequest';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  $user = new BehaviorSubject<User | undefined>(undefined);

  constructor(private http: HttpClient, private cookie: CookieService) {}

  createAccount(request: createRequest): Observable<AccountResponse> {
    return this.http.post<AccountResponse>(
      `${environment.apiBaseUrl}/api/user/create`,
      request
    );
  }

  login(request: LoginRequest): Observable<AccountResponse> {
    return this.http.post<AccountResponse>(
      `${environment.apiBaseUrl}/api/user/login`,
      request
    );
  }

  getUser(): User | undefined {
    const token = this.cookie.get('token');
    const email = this.cookie.get('email');
    const roles = this.cookie.get('roleModels');
    if (token) {
      // const user: User = JSON.parse(atob(token.split('.')[1])) as User;
      const user: User = {
        email: email,
        token: token,
        roles: JSON.parse(roles),
      };
      // console.log(user.roles);
      return user;
    }
    return undefined;
  }

  user(): Observable<User | undefined> {
    return this.$user.asObservable();
  }

  logout(): void {
    // this.cookie.delete('email');
    // this.cookie.delete('token');
    this.cookie.deleteAll();
    this.$user.next(undefined);
  }

  restore(request: AccountRestoreRequest): Observable<RestoreAccountResponse> {
    return this.http.post<RestoreAccountResponse>(
      `${environment.apiBaseUrl}/api/user/restore`,
      request
    );
  }

  changePassword(
    request: AccountChangePassword
  ): Observable<AccountChangePassword> {
    return this.http.post<AccountChangePassword>(
      `${environment.apiBaseUrl}/api/user/changepassword`,
      request
    );
  }

  isExpiredToken(): boolean {
    const user = this.getUser();
    if (user) {
      let jwt = jwtDecode(user.token);
      let aud = jwt.exp! * 1000;
      let time = Date.now();

      if (time > aud) {
        console.log('Токен просрочен');
        return true;
      }
      return false;
    }
    return true;
  }

  loginWithGoogle(
    request: LoginWithGoogleRequest
  ): Observable<AccountResponse> {
    return this.http.post<AccountResponse>(
      `${environment.apiBaseUrl}/api/user/loginWithGoogle`,
      request
    );
  }
}
