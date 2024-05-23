import { AppState } from './app.state';
import { createSelector } from '@ngrx/store';

export const selectEmailState = (state: AppState) => state.email;

export const selectEmail = createSelector(
  selectEmailState,
  (state) => state.email
);
