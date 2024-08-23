import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { UserInfoModel } from '../models/user-info.models';
import { roleModels } from '../../account/models/roleModels.model';
import { UserModelRequest } from '../models/update-user-info.model';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  constructor(private http: HttpClient) {}

  getAllUsers(): Observable<UserInfoModel[]> {
    return this.http.get<UserInfoModel[]>(
      `${environment.apiBaseUrl}/api/Admin/all`
    );
  }

  getCurrentUser(email: string): Observable<UserInfoModel> {
    return this.http.get<UserInfoModel>(
      `${environment.apiBaseUrl}/api/Admin/${email}`
    );
  }

  updateUser(updateUserInfoRequest: UserModelRequest) {
    return this.http.post<UserInfoModel>(
      `${environment.apiBaseUrl}/api/Admin/updateUser`,
      updateUserInfoRequest
    );
  }

  getAvailableRoles(): Observable<roleModels[]> {
    return this.http.get<roleModels[]>(
      `${environment.apiBaseUrl}/api/Admin/roles`
    );
  }
}
