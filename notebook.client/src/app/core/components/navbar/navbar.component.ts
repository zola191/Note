import { Component, OnInit } from '@angular/core';
import { User } from '../../../features/account/models/user.model';
import { UserService } from '../../../features/account/services/user.service';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { roleModels } from '../../../features/account/models/roleModels.model';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { clearUserEmail } from '../../../features/account/userStore/user-actions';
import { UserState } from '../../../features/account/userStore/user-store';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit {
  user?: User;
  email$: Observable<string | null>;
  constructor(
    private userService: UserService,
    private router: Router,
    private cookieService: CookieService,
    private store: Store<{ user: UserState }>
  ) {
    this.email$ = this.store.pipe(select((state) => state.user.email));
  }

  ngOnInit(): void {}

  isAuth(): boolean {
    let existingUser = this.userService.getUser();
    if (existingUser !== null && existingUser !== undefined) {
      this.user = existingUser;
      return true;
    }
    return false;
  }

  isAdmin(): boolean {
    const userRoles = this.cookieService.get('roleModels');
    if (userRoles) {
      const userRoleArray = JSON.parse(userRoles) as roleModels[];
      // console.log(userRoleArray);
      const isAdmin = userRoleArray.some((f) => f.roleName === 'Admin');
      if (isAdmin) {
        return true;
      } else {
        return false;
      }
    }
    return false;
  }

  onLogout(): void {
    this.store.dispatch(clearUserEmail());
    this.userService.logout();
    this.router.navigateByUrl('/');
  }
}
