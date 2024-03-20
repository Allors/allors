import { Injectable } from '@angular/core';
import { RoleType } from '@allors/workspace-system-meta';
import {
  RefreshService,
  EditDialogService,
} from '@allors/workspace-base-angular-foundation';
import { EditAction } from './edit-action';

@Injectable({
  providedIn: 'root',
})
export class EditActionService {
  constructor(
    private editDialogService: EditDialogService,
    private refreshService: RefreshService
  ) {}

  edit(roleType?: RoleType) {
    return new EditAction(
      this.editDialogService,
      this.refreshService,
      roleType
    );
  }
}
