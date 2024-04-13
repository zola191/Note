import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { NotebookService } from '../services/notebook.service';
import { Notebook } from '../models/notebook.model';
import { NotebookRequest } from '../models/notebook-request.model';

@Component({
  selector: 'app-edit-notebook',
  templateUrl: './edit-notebook.component.html',
  styleUrl: './edit-notebook.component.css',
})
export class EditNotebookComponent implements OnInit, OnDestroy {
  id: string | null = null;
  paramsSubscription?: Subscription;
  notebook?: Notebook;
  updateNoteSubscription?: Subscription;

  constructor(
    private route: ActivatedRoute,
    private notebookService: NotebookService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.paramsSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        if (this.id) {
          //get the data from the API for this notebook Id
          this.notebookService.getNotebookById(this.id).subscribe({
            next: (response) => {
              this.notebook = response;
            },
          });
        }
      },
    });
  }

  onFormSubmit(): void {
    const updateNoteRequest: NotebookRequest = {
      firstName: this.notebook?.firstName ?? '',
      middleName: this.notebook?.middleName ?? '',
      lastName: this.notebook?.lastName ?? '',
      phoneNumber: this.notebook?.phoneNumber ?? '',
      birthDay: this.notebook?.birthDay ?? new Date(),
      country: this.notebook?.country ?? '',
      organization: this.notebook?.organization ?? '',
      position: this.notebook?.position ?? '',
      other: this.notebook?.other ?? '',
    };

    //pass this object to service
    if (this.id) {
      this.updateNoteSubscription = this.notebookService
        .updateNotebook(this.id, updateNoteRequest)
        .subscribe({
          next: (response) => {
            this.router.navigateByUrl('/notebook/notebook-list');
          },
        });
    }
  }

  onCancel(): void {
    this.router.navigateByUrl('/notebook/notebook-list');
  }

  onDelete(): void {
    if (this.id) {
      this.notebookService.deleteNotebook(this.id).subscribe({
        next: (response) => {
          this.router.navigateByUrl('/notebook/notebook-list');
        },
      });
    }
  }

  ngOnDestroy(): void {
    this.paramsSubscription?.unsubscribe();
    this.updateNoteSubscription?.unsubscribe();
  }
}
