import { Component, Input, OnInit, Output } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { AccountRequest } from '../models/account-request.model';
import { DataService } from '../services/data.service.service';
import { getEmail } from '../state/actions';
import { AppState } from '../state/app.state';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css',
})
export class ChangePasswordComponent implements OnInit {
  @Input() email: string = '';
  form: FormGroup;
  model: AccountRequest;
  constructor(
    private dataService: DataService,
    private store: Store<AppState>
  ) {
    this.model = {
      email: 'test',
      password: '',
      confirmPassword: '',
    };

    this.form = new FormGroup(
      {
        password: new FormControl('', [Validators.required]),
        confirmPassword: new FormControl('', [Validators.required]),
      },
      {
        validators: this.passwordMatchValidator,
      }
    );
  }

  getEmail(): string {
    return this.store.dispatch(getEmail());
  }

  ngOnInit(): void {
    this.dataService.dataTransferObservable.subscribe((str) => {
      this.model.email = str.email;
    });
  }

  passwordMatchValidator(control: AbstractControl) {
    return control.get('password')?.value ===
      control.get('confirmPassword')?.value
      ? null
      : { mismatch: true };
  }

  receiveEmail($event: string) {
    this.model.email = $event;
  }

  onFormSubmit() {}
}
