import { createReducer, on } from '@ngrx/store';
import { emailState } from './emailState';
import * as EmailActions from './actions';

export const initialEmailState: emailState = {
  email: '',
};

export const emailReducer = createReducer(
  initialEmailState,
  on(EmailActions.setEmail, (state, { newEmail }) => ({
    ...state,
    email: newEmail,
  })),
  on(EmailActions.getEmail, (state) => ({
    ...state,
    email: state.email,
  }))
);
