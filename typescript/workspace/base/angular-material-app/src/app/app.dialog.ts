import {
  AllorsMaterialDynamicCreateComponent as Create,
  AllorsMaterialDynamicEditComponent as Edit,
} from '@allors/workspace-base-angular-material-application';
import { tags } from '@allors/workspace-default-meta';

import { PersonCreateDialogComponent } from './domain/person/create/person-create-dialog.component';

export const dialogs = {
  create: {
    [tags.Country]: Create,
    [tags.Employment]: Create,
    [tags.Organization]: Create,
    [tags.Person]: PersonCreateDialogComponent,
  },
  edit: {
    [tags.Country]: Edit,
    [tags.Employment]: Edit,
  },
};

export const components: any[] = [PersonCreateDialogComponent];
