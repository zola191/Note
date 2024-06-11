import { Component, OnInit, TemplateRef } from '@angular/core';
import { Observable } from 'rxjs';
import { NotebookService } from '../services/notebook.service';
import { Notebook } from '../models/notebook.model';
import { MatDialog } from '@angular/material/dialog';
import { AddNotebookComponent } from '../add-notebook/add-notebook.component';
import { ModalService } from '../services/modal.service';

@Component({
  selector: 'app-notebook-list',
  templateUrl: './notebook-list.component.html',
  styleUrl: './notebook-list.component.css',
})
export class NotebookListComponent implements OnInit {
  notebooks$?: Observable<Notebook[]>;

  constructor(
    private notebookService: NotebookService,
    private modal: MatDialog,
    private modalService: ModalService
  ) {}

  ngOnInit(): void {
    this.notebooks$ = this.notebookService.getAllNotebooks();
  }

  openModal(modal: TemplateRef<any>) {
    this.modalService
      .open(modal, { size: 'lg', title: 'add notebook' })
      .subscribe((action) => {
        console.log('modalAction', action);
      });
  }
}
