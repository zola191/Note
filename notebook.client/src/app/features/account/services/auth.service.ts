import { HttpClient, HttpHeaders } from '@angular/common/http';
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
import { loginRequest } from '../models/account-loginRequest.mode';
import { JwtDecoderService } from '../../../core/jwt/jwt-decoder.service';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  // private readonly TOKEN_NAME = 'tasty-cookies';
  $user = new BehaviorSubject<User | undefined>(undefined);
  // get token() {
  //   return localStorage.getItem(this.TOKEN_NAME);
  // }

  constructor(
    private http: HttpClient,
    private cookie: CookieService,
    private jwtService: JwtDecoderService
  ) {}

  createAccount(request: createRequest): Observable<AccountResponse> {
    return this.http.post<AccountResponse>(
      `${environment.apiBaseUrl}/api/account/create`,
      request
    );
  }

  login(request: loginRequest): Observable<AccountResponse> {
    return this.http.post<AccountResponse>(
      `${environment.apiBaseUrl}/api/account/login`,
      request
    );
  }

  // setUser(user: User): void {
  //   let jwt = jwtDecode(this.cookie.get('token'));
  //   let aud = jwt.exp! * 1000;
  //   let time = Date.now();

  //   if (time > aud) {
  //     return;
  //   }

  //   // this.$user.next(user);
  //   this.cookie.set('token', user.token);
  //   this.cookie.set('email', user.email);

  //   // localStorage.setItem('user-email', user.email);
  //   // localStorage.setItem(
  //   //   'jwt1',
  //   //   this.jwtService.decodeToken(this.cookie.get('token'))
  //   // );
  //   // let result = JSON.stringify(jwtDecode(this.cookie.get('token')));

  //   // localStorage.setItem('jwt2', result);
  //   // localStorage.setItem('time', time.toString());
  //   // localStorage.setItem('aud', aud.toString());
  // }

  getUser(): User | undefined {
    const token = this.cookie.get('token');
    const email = this.cookie.get('email');

    if (token) {
      const user: User = {
        email: email,
        token: token,
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

  changePassword(
    request: AccountChangePassword
  ): Observable<AccountChangePassword> {
    return this.http.post<AccountChangePassword>(
      `${environment.apiBaseUrl}/api/account/changepassword`,
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
}
