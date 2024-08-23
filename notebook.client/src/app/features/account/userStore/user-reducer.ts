import { createReducer, on } from '@ngrx/store';
import { UserState } from './user-store';
import { clearUserEmail, setUserEmail } from './user-actions';

export const initialState: UserState = {
  email: null,
};

export const userReducer = createReducer(
  initialState,
  on(setUserEmail, (state, { email }) => ({ ...state, email })),
  on(clearUserEmail, (state) => ({ ...state, email: null }))
);
