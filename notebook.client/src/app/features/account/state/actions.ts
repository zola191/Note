import { createAction, props } from '@ngrx/store';

export const getEmail = createAction(
  '[email] Get Email',
  props<{ email: string }>
);

export const setEmail = createAction(
  '[email] Set Email',
  props<{ newEmail: string }>()
);
