import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import {
  Context,
  WorkspaceService,
} from '@allors/workspace/base/angular-foundation';
import { MethodType } from '@allors/workspace/system/meta';
import {
  Action,
  RefreshService,
  ErrorService,
} from '@allors/workspace/base/angular-foundation';
import { MethodAction } from './method-action';
import { MethodConfig } from './method-config';

@Injectable({
  providedIn: 'root',
})
export class MethodActionService {
  constructor(
    private workspaceService: WorkspaceService,
    private refreshService: RefreshService,
    private errorService: ErrorService,
    private snackBar: MatSnackBar
  ) {}

  create(methodType: MethodType, config?: MethodConfig): Action {
    return new MethodAction(
      this.workspaceService,
      this.refreshService,
      this.snackBar,
      this.errorService,
      methodType,
      config
    );
  }
}
