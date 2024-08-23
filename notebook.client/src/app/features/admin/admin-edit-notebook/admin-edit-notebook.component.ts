import { Component, OnInit } from '@angular/core';
import { Notebook } from '../../notebook/models/notebook.model';
import { NotebookRequest } from '../../notebook/models/notebook-request.model';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { AdminService } from '../services/admin.service';

@Component({
  selector: 'app-admin-edit-notebook',
  templateUrl: './admin-edit-notebook.component.html',
  styleUrl: './admin-edit-notebook.component.css',
})
export class AdminEditNotebookComponent implements OnInit {
  id: string | null = null;
  email: string | null = null;
  paramsSubscription?: Subscription;
  notebook?: Notebook;
  updateNoteSubscription?: Subscription;

  constructor(
    private route: ActivatedRoute,
    private adminService: AdminService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.paramsSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');
        this.email = params.get('email');
        console.log(this.email);
        if (this.id) {
          this.adminService.getCurrentNotebook(this.id).subscribe({
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

    if (this.id) {
      this.updateNoteSubscription = this.adminService
        .updateNotebook(this.id, updateNoteRequest)
        .subscribe({
          next: (response) => {
            this.router.navigateByUrl(`/admin/info/${this.email}`);
          },
        });
    }
  }

  onCancel(): void {
    this.router.navigateByUrl(`/admin/info/${this.email}`);
  }

  onDelete(): void {
    if (this.id) {
      this.adminService.deleteNotebook(this.id).subscribe({
        next: (response) => {
          this.router.navigateByUrl(`/admin/info/${this.email}`);
        },
      });
    }
  }
}
