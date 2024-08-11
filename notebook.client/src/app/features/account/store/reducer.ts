import { createReducer, on } from '@ngrx/store';
import { AccountResponse } from '../models/account-response.model';
import * as AccountActions from './actions';

export const accountStateFeatureKey = 'account';

export interface AccountState {
  account: AccountResponse;
  loading: boolean;
  error: string;
}
export const initialState: AccountState = {
  account: {
    email: '',
    firstName: '',
    id: '',
    lastName: '',
    token: '',
  },
  loading: false,
  error: '',
};
export const AccountReducer = createReducer(
  initialState,

  on(AccountActions.loadAccount, (state) => ({
    ...state,
    loading: true,
  })),

  on(AccountActions.loadAccountSuccess, (state, { account }) => ({
    ...state,
    account,
    loading: false,
  })),

  on(AccountActions.loadAccountFailure, (state, { error }) => ({
    ...state,
    error,
    loading: false,
  }))
);
