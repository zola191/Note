import { Component, OnDestroy } from '@angular/core';
import { NotebookRequest } from '../models/notebook-request.model';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { NotebookService } from '../services/notebook.service';

@Component({
  selector: 'app-add-notebook',
  templateUrl: './add-notebook.component.html',
  styleUrl: './add-notebook.component.css',
})
export class AddNotebookComponent implements OnDestroy {
  model: NotebookRequest;
  private addNotebookSubscription?: Subscription;

  constructor(
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
