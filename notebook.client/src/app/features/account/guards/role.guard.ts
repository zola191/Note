import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { roleModels } from '../models/roleModels.model';

export const roleGuard: CanActivateFn = (route, state) => {
  const cookieService = inject(CookieService);
  const userRoles = cookieService.get('roleModels');
  if (userRoles) {
    const userRoleArray = JSON.parse(userRoles) as roleModels[];
    const isAdmin = userRoleArray.some((f) => f.roleName === 'Admin');
    if (isAdmin) {
      return true;
    } else {
      return false;
    }
  }
  return false;
};
