import { Component, OnInit } from '@angular/core';
import { User } from '../../../features/account/models/user.model';
import { AuthService } from '../../../features/account/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit {
  user?: User;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.authService.user().subscribe({
      next: (response) => {
        this.user = response;
        // this.user = this.authService.getUser();
      },
    });
  }

  isAuth(): boolean {
    return (
      this.authService.getUser() !== null &&
      this.authService.getUser() !== undefined
    );
  }

  onLogout(): void {
    this.authService.logout();
    this.router.navigateByUrl('/');
  }
}
