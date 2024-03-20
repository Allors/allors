import { Observable, throwError } from 'rxjs';
import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { IObject } from '@allors/workspace-system-domain';
import {
  EditRequest,
  EditDialogService,
} from '@allors/workspace-base-angular-foundation';

@Injectable()
export class AllorsMaterialEditDialogService extends EditDialogService {
  editControlByObjectTypeTag: { [id: string]: any };

  constructor(public dialog: MatDialog) {
    super();
  }

  canEdit(object: IObject): boolean {
    return !!this.editControlByObjectTypeTag[object.strategy.cls.tag];
  }

  edit(request: EditRequest): Observable<IObject> {
    const component = this.editControlByObjectTypeTag[request.objectType.tag];
    if (component) {
      const dialogRef = this.dialog.open(component, {
        data: request,
        minWidth: '80vw',
        maxHeight: '90vh',
      });
      return dialogRef.afterClosed();
    }

    return throwError(() => new Error('Missing component'));
  }
}
