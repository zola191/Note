import { Component, ElementRef, Inject, ViewChild } from '@angular/core';
import { NotebookRequest } from '../models/notebook-request.model';
import { Subscription } from 'rxjs';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { NotebookService } from '../services/notebook.service';

// @ts-ignore
const $: any = window['$'];
@Component({
  selector: 'app-add-notebook',
  templateUrl: './add-notebook.component.html',
  styleUrl: './add-notebook.component.css',
})
export class AddNotebookComponent {
  @ViewChild('modal') modal?: ElementRef;

  model: NotebookRequest;

  private addNotebookSubscription?: Subscription;

  constructor(
    public dialogRef: MatDialogRef<AddNotebookComponent>,

    private notebookService: NotebookService,
    private router: Router
  ) {
    this.model = {
      firstName: '',
      middleName: '',
      lastName: '',
      phoneNumber: '',
      country: '',
      birthDay: new Date(),
      organization: '',
      position: '',
      other: '',
    };
  }

  onFormSubmit() {
    this.addNotebookSubscription = this.notebookService
      .addNotebook(this.model)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/notebook/notebook-list');
        },
      });
  }

  ngOnDestroy(): void {
    this.addNotebookSubscription?.unsubscribe;
  }
}
