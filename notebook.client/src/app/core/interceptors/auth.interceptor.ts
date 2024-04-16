import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HTTP_INTERCEPTORS,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private cookie: CookieService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // const localToken = localStorage.getItem('token');
    const token = this.cookie.get('token');
    const modifiedReq = req.clone({
      headers: req.headers.append('auth', 'bearer' + token),
    });
    return next.handle(modifiedReq);
  }
}

// export const AuthInterceptorProvider = {
//   provide: HTTP_INTERCEPTORS,
//   userClass: AuthInterceptor,
//   multi: true,
// };
