import { AccountResponse } from './account-response.model';

export interface RestoreAccountResponse {
  token: string;
  accountModel: AccountResponse;
}
