import { roleModels } from './roleModels.model';

export interface AccountResponse {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  token: string;
  roleModels: roleModels[];
}
