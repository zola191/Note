import { roleModels } from '../../account/models/roleModels.model';
import { Notebook } from '../../notebook/models/notebook.model';

export interface UserInfoModel {
  email: string;
  firstName: string;
  lastName: string;
  notes: Notebook[];
  roleModels: roleModels[];
}
