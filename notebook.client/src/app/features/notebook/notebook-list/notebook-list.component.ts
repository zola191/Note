import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { NotebookService } from '../services/notebook.service';
import { Notebook } from '../models/notebook.model';

@Component({
  selector: 'app-notebook-list',
  templateUrl: './notebook-list.component.html',
  styleUrl: './notebook-list.component.css',
})
export class NotebookListComponent implements OnInit {
  notebooks$?: Observable<Notebook[]>;

  constructor(private notebookService: NotebookService) {}

  ngOnInit(): void {
    this.notebooks$ = this.notebookService.getAllNotebooks();
  }
}
