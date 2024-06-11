import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnDestroy,
  Output,
  input,
  output,
} from '@angular/core';
import { NotebookRequest } from '../models/notebook-request.model';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { NotebookService } from '../services/notebook.service';

@Component({
  selector: 'app-add-notebook',
  templateUrl: './add-notebook.component.html',
  styleUrl: './add-notebook.component.css',
})
export class AddNotebookComponent {
  @Input() size? = 'md';
  @Input() title? = 'modal title';

  @Output() closeEvent = new EventEmitter();
  @Output() submitEvent = new EventEmitter();

  model: NotebookRequest;
  private addNotebookSubscription?: Subscription;

  constructor(
    private notebookService: NotebookService,
    private router: Router,
    private elementRef: ElementRef
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

  close(): void {
    this.elementRef.nativeElement.remove();
    this.closeEvent.emit();
  }

  submit(): void {
    this.elementRef.nativeElement.remove();
    this.submitEvent.emit();
  }

  // onFormSubmit() {
  //   this.addNotebookSubscription = this.notebookService
  //     .addNotebook(this.model)
  //     .subscribe({
  //       next: (response) => {
  //         this.router.navigateByUrl('/notebook/notebook-list');
  //       },
  //     });
  // }

  // ngOnDestroy(): void {
  //   this.addNotebookSubscription?.unsubscribe;
  // }
}
