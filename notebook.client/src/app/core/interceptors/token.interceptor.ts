import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HTTP_INTERCEPTORS,
} from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { AuthService } from '../../features/account/services/auth.service';
import { Router } from '@angular/router';
@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(
    private cookie: CookieService,
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (this.authService.isExpiredToken()) {
      console.log('я работаю');

      // this.cookie.delete('email');
      // this.cookie.delete('token');

      // this.authService.logout();
      // this.router.navigateByUrl('account/login');

      //   return throwError('Токен просрочен');
      // } else {
      //   return next.handle(request);
    }
    return next.handle(request);
    // return next.handle(request).pipe(
    //   catchError((error) => {
    //     if (error.status === 401) {
    //       // Token expired, redirect to login page
    //       //   this.router.navigate(['/login']);
    //       console.log('Привет!');
    //     }
    //     return throwError(error);
    //   })
    // );
  }
}
