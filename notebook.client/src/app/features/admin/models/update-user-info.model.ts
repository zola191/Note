import { roleModels } from '../../account/models/roleModels.model';

export interface UserModelRequest {
  email: string;
  firstName: string;
  lastName: string;
  roleModels: roleModels[];
}
