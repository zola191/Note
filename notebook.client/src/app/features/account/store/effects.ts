import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { UserService } from '../services/user.service';
import * as AccountActions from './actions';
import { LoginRequest } from '../models/loginRequest.model';

@Injectable()
export class AccountEffects {
  loginModel: LoginRequest = {
    email: '',
    password: '',
  };

  loadAccount$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AccountActions.loadAccount),
      mergeMap(() =>
        this.userService.login(this.loginModel).pipe(
          map((account) => AccountActions.loadAccountSuccess({ account })),
          catchError((error) =>
            of(AccountActions.loadAccountFailure({ error: error.message }))
          )
        )
      )
    )
  );
  constructor(private actions$: Actions, private userService: UserService) {}
}
