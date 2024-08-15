import { HttpClient, HttpEvent, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NotebookRequest } from '../models/notebook-request.model';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { Notebook } from '../models/notebook.model';

@Injectable({
  providedIn: 'root',
})
export class NotebookService {
  constructor(private http: HttpClient) {}

  create(model: NotebookRequest): Observable<void> {
    return this.http.post<void>(
      `${environment.apiBaseUrl}/api/note/create`,
      model
    );
  }

  getAll(): Observable<Notebook[]> {
    return this.http.get<Notebook[]>(`${environment.apiBaseUrl}/api/note/all`);
  }

  getById(id: string): Observable<Notebook> {
    return this.http.get<Notebook>(`${environment.apiBaseUrl}/api/note/${id}`);
  }

  update(
    id: string,
    updateNotebookRequest: NotebookRequest
  ): Observable<Notebook> {
    return this.http.put<Notebook>(
      `${environment.apiBaseUrl}/api/note/${id}`,
      updateNotebookRequest
    );
  }

  delete(id: string): Observable<Notebook> {
    return this.http.delete<Notebook>(
      `${environment.apiBaseUrl}/api/note/${id}`
    );
  }

  upload(file: File): Observable<Notebook[]> {
    const formData: FormData = new FormData();

    formData.append('file', file);
    return this.http.post<Notebook[]>(
      `${environment.apiBaseUrl}/api/fileManager/UploadFromExcel`,
      formData
    );
  }

  getFiles(): Observable<any> {
    return this.http.get(`${environment.apiBaseUrl}/api/fileManager/files`);
  }
}
