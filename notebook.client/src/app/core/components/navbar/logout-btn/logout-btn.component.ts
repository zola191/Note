import { Component, input, Input } from '@angular/core';
import { UserService } from '../../../../features/account/services/user.service';
import { Router } from '@angular/router';
import { User } from '../../../../features/account/models/user.model';

@Component({
  selector: 'app-logout-btn',
  templateUrl: './logout-btn.component.html',
  styleUrl: './logout-btn.component.css',
})
export class LogoutBtnComponent {
  @Input() user: User | undefined;

  constructor(
    private readonly userService: UserService,
    private readonly router: Router
  ) {}
  onLogout(): void {
    this.userService.logout();
    this.router.navigateByUrl('/account/login').then(() => {
      window.location.reload();
    });
  }
}
