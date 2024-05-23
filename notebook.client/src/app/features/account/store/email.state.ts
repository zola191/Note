import {
  createAction,
  createReducer,
  createSelector,
  on,
  props,
} from '@ngrx/store';
import { emailState } from '../state/emailState';
export interface AppState {
  email: emailState;
}
export const getEmail = createAction('[email] Get Email');

export const setEmail = createAction(
  '[email] Set Email',
  props<{ newEmail: string }>()
);

export const initialEmailState: emailState = {
  email: '',
};

export const emailReducer = createReducer(
  initialEmailState,
  on(setEmail, (state, { newEmail }) => ({
    ...state,
    email: newEmail,
  })),
  on(getEmail, (state) => ({
    ...state,
    email: state.email,
  }))
);

export const selectEmailState = (state: AppState) => state.email;

export const selectEmail = createSelector(
  selectEmailState,
  (state) => state.email
);
