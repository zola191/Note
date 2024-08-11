import { Component, Inject } from '@angular/core';
import { ErrorModel } from '../models/notebook-error.model';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-alert-modal',
  templateUrl: './alert-modal.component.html',
  styleUrl: './alert-modal.component.css',
})
export class AlertModalComponent {
  // errors?: ErrorModel[];

  constructor(@Inject(MAT_DIALOG_DATA) public data: ErrorModel[]) {}
}
