import { createAction, createActionGroup, props } from '@ngrx/store';
import { AccountResponse } from '../models/account-response.model';

export const loadAccount = createAction('[Todo] Load Todos');

export const loadAccountSuccess = createAction(
  '[Account] Load account Success',
  props<{ account: AccountResponse }>()
);

export const loadAccountFailure = createAction(
  '[Account] Load account Failure',
  props<{ error: string }>()
);
