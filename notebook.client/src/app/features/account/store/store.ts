import { Action, ActionReducer } from '@ngrx/store';
import { AccountEffects } from './effects';
import { AccountReducer, AccountState } from './reducer';

export interface AppState {
  account: AccountState;
}

export interface AppStore {
  account: ActionReducer<AccountState, Action>;
}

export const AccountStore: AppStore = {
  account: AccountReducer,
};

export const appEffects = [AccountEffects];
