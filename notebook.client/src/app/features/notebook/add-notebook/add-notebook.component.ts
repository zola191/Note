import { Component } from '@angular/core';
import { NotebookRequest } from '../models/notebook-request.model';
import { Subscription } from 'rxjs';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { NotebookService } from '../services/notebook.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { dateValidator } from '../validators/dateValidator';
@Component({
  selector: 'app-add-notebook',
  templateUrl: './add-notebook.component.html',
  styleUrl: './add-notebook.component.css',
})
export class AddNotebookComponent {
  model: NotebookRequest;
  form: FormGroup;
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

    this.form = new FormGroup({
      firstName: new FormControl('', [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(25),
      ]),
      middleName: new FormControl('', [
        Validators.minLength(4),
        Validators.maxLength(25),
      ]),
      lastName: new FormControl('', [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(25),
      ]),
      phoneNumber: new FormControl('', [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(25),
        Validators.pattern(/(?:\+|\d)[\d\-\(\) ]{9,}\d/g),
      ]),
      country: new FormControl('', [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(25),
      ]),
      birthDay: new FormControl('', [dateValidator()]),
      organization: new FormControl('', [
        Validators.minLength(4),
        Validators.maxLength(25),
      ]),
      position: new FormControl('', [
        Validators.minLength(4),
        Validators.maxLength(25),
      ]),
    });
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
