import { createAction, props } from '@ngrx/store';

export const setUserEmail = createAction(
  '[User] Set Email',
  props<{ email: string }>()
);
export const clearUserEmail = createAction('[User] Clear Email');
