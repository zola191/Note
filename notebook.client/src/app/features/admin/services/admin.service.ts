import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { UserInfoModel } from '../models/user-info.models';
import { roleModels } from '../../account/models/roleModels.model';
import { UserModelRequest } from '../models/update-user-info.model';
import { NotebookRequest } from '../../notebook/models/notebook-request.model';
import { Notebook } from '../../notebook/models/notebook.model';
import { NoteChangeLog } from '../models/note-change-log';
import { LogFileByPeriod } from '../models/log-file-by-period';

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

  getCurrentNotebook(id: string): Observable<Notebook> {
    return this.http.get<Notebook>(
      `${environment.apiBaseUrl}/api/admin/getCurrentNote/${id}`
    );
  }

  updateNotebook(
    id: string,
    updateNotebookRequest: NotebookRequest
  ): Observable<Notebook> {
    return this.http.put<Notebook>(
      `${environment.apiBaseUrl}/api/admin/updateCurrentNote/${id}`,
      updateNotebookRequest
    );
  }

  deleteNotebook(id: string): Observable<Notebook> {
    return this.http.delete<Notebook>(
      `${environment.apiBaseUrl}/api/admin/deleteCurrentNote/${id}`
    );
  }

  getChangeLogs(email: string): Observable<NoteChangeLog[]> {
    return this.http.get<NoteChangeLog[]>(
      `${environment.apiBaseUrl}/api/admin/getChangeLogs/${email}`
    );
  }

  getExcelFileLogs(data: LogFileByPeriod): Observable<Blob> {
    return this.http.post(
      `${environment.apiBaseUrl}/api/admin/getExcelFileLogs`,
      data,
      {
        responseType: 'blob',
      }
    );
  }
}
