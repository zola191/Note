import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { Observable } from 'rxjs';
import { select, Store } from '@ngrx/store';
import { UserState } from '../userStore/user-store';

@Component({
  selector: 'app-cabinet',
  templateUrl: './cabinet.component.html',
  styleUrl: './cabinet.component.css',
})
export class CabinetComponent implements OnInit {
  email$: Observable<string | null>;

  constructor(
    private readonly userService: UserService,
    private store: Store<{ user: UserState }>
  ) {
    this.email$ = this.store.pipe(select((state) => state.user.email));
  }

  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }
}
