import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { NotebookService } from '../services/notebook.service';
import { Notebook } from '../models/notebook.model';
import { MatDialog } from '@angular/material/dialog';
import { AddNotebookComponent } from '../add-notebook/add-notebook.component';

@Component({
  selector: 'app-notebook-list',
  templateUrl: './notebook-list.component.html',
  styleUrl: './notebook-list.component.css',
})
export class NotebookListComponent implements OnInit {
  notebooks$?: Observable<Notebook[]>;

  constructor(
    private notebookService: NotebookService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.notebooks$ = this.notebookService.getAllNotebooks();
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(AddNotebookComponent, {
      height: '800px',
      width: '600px',
    });

    dialogRef.afterClosed().subscribe((result) => {
      console.log('The dialog was closed');
    });
  }
}
