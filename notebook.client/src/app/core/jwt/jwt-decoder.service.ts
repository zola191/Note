import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class JwtDecoderService {
  constructor() {}

  public decodeToken(token: string) {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => {
          return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        })
        .join('')
    );
    let x = JSON.parse(jsonPayload);
    let y = JSON.stringify(x);
    return y;
    return JSON.parse(jsonPayload);
  }
}
