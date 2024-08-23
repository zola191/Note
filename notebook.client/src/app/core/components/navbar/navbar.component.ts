import { Component, OnInit } from '@angular/core';
import { User } from '../../../features/account/models/user.model';
import { UserService } from '../../../features/account/services/user.service';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { roleModels } from '../../../features/account/models/roleModels.model';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit {
  user?: User;

  constructor(
    private userService: UserService,
    private router: Router,
    private cookieService: CookieService
  ) {}

  ngOnInit(): void {
    // this.authService.user().subscribe({
    //   next: (response) => {
    //     this.user = response;
    //     // this.user = this.authService.getUser();
    //   },
    // });
  }

  isAuth(): boolean {
    let existingUser = this.userService.getUser();
    if (existingUser !== null && existingUser !== undefined) {
      this.user = existingUser;
      return true;
    }
    return false;

    // return (
    //   this.authService.getUser() !== null &&
    //   this.authService.getUser() !== undefined
    // );
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
    this.userService.logout();
    this.router.navigateByUrl('/');
  }
}
