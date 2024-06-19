import { HttpClient } from '@angular/common/http';
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
      `${environment.apiBaseUrl}/api/notebook/create`,
      model
    );
  }

  getAll(): Observable<Notebook[]> {
    return this.http.get<Notebook[]>(
      `${environment.apiBaseUrl}/api/notebook/all`
    );
  }

  getById(id: string): Observable<Notebook> {
    return this.http.get<Notebook>(
      `${environment.apiBaseUrl}/api/notebook/${id}`
    );
  }

  update(
    id: string,
    updateNotebookRequest: NotebookRequest
  ): Observable<Notebook> {
    return this.http.put<Notebook>(
      `${environment.apiBaseUrl}/api/notebook/${id}`,
      updateNotebookRequest
    );
  }

  delete(id: string): Observable<Notebook> {
    return this.http.delete<Notebook>(
      `${environment.apiBaseUrl}/api/notebook/${id}`
    );
  }
}
