import { Component, OnInit } from '@angular/core';
import { AdminService } from '../services/admin.service';
import { UserInfoModel } from '../models/user-info.models';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css'],
})
export class AdminComponent implements OnInit {
  users$?: Observable<UserInfoModel[]>;
  users?: UserInfoModel[];
  filteredUserList: UserInfoModel[] = [];

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.users$ = this.adminService.getAllUsers();
    this.users$.subscribe({
      next: (data: UserInfoModel[]) => {
        this.users = data;
        this.filteredUserList = this.users;
      },
    });
  }

  filterResults(text: string) {
    if (!text) {
      this.filteredUserList = this.users || [];
      return;
    }

    this.filteredUserList =
      this.users?.filter((user) =>
        user.email.toLowerCase().includes(text.toLowerCase())
      ) || [];
  }
}
