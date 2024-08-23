import { Component, OnInit } from '@angular/core';
import { AdminService } from '../services/admin.service';
import { UserInfoModel } from '../models/user-info.models';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { roleModels } from '../../account/models/roleModels.model';
import { UserModelRequest } from '../models/update-user-info.model';

@Component({
  selector: 'app-current-user',
  templateUrl: './current-user.component.html',
  styleUrl: './current-user.component.css',
})
export class CurrentUserComponent implements OnInit {
  availableRoles: roleModels[] = [];
  isEditing: boolean = false;
  email: string | null = null;
  paramsSubscription?: Subscription;
  user?: UserInfoModel;
  errorMessage: string = '';

  updatedUser: UserModelRequest = {
    email: '',
    firstName: '',
    lastName: '',
    roleModels: [],
  };

  constructor(
    private adminService: AdminService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.paramsSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.email = params.get('email');
        if (this.email) {
          this.adminService.getCurrentUser(this.email).subscribe({
            next: (response) => {
              this.user = response;
              this.initializeUpdatedUser();
              // console.log(this.user);
              // console.log(this.user.roleModels);
              // console.log(this.updatedUser.roleModels);
            },
          });
        }
      },
    });

    this.adminService.getAvailableRoles().subscribe((roles) => {
      this.availableRoles = roles;
    });
  }

  initializeUpdatedUser() {
    if (this.user) {
      this.updatedUser = {
        email: this.user.email,
        firstName: this.user.firstName,
        lastName: this.user.lastName,
        roleModels: this.user.roleModels.map((role) => ({ ...role })),
      };
    }
    // console.log(this.updatedUser.roleModels);
  }

  toggleEditMode() {
    this.isEditing = !this.isEditing;
    if (this.isEditing) {
      this.initializeUpdatedUser();
    } else {
      this.errorMessage = '';
    }
  }

  saveChanges() {
    if (!this.user) {
      return;
    }

    if (this.user.roleModels.length === 0) {
      this.errorMessage = 'Пожалуйста, выберите хотя бы одну роль.';
      return;
    }

    console.log(this.updatedUser);
    this.adminService.updateUser(this.updatedUser).subscribe({
      next: (response) => {
        this.user = response;
        this.isEditing = false;
        console.log('Данные пользователя успешно обновлены', response);
      },
    });

    // this.adminService.updateUser(this.updatedUser).subscribe((response) => {
    //   this.user = response;
    //   console.log('Данные пользователя успешно обновлены', response);
    //   this.isEditing = false;
    // });
  }
}
