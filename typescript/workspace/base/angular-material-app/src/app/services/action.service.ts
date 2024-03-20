import { Injectable } from '@angular/core';
import {
  Action,
  ActionService,
  WorkspaceService,
} from '@allors/workspace-base-angular-foundation';
import { M } from '@allors/workspace-default-meta';
import { Composite } from '@allors/workspace-system-meta';
import { MethodActionService } from '@allors/workspace-base-angular-material-application';

@Injectable()
export class AppActionService implements ActionService {
  actionByObjectType: Map<Composite, Action[]>;

  constructor(
    workspaceService: WorkspaceService,
    methodActionService: MethodActionService
  ) {
    const m = workspaceService.workspace.configuration.metaPopulation as M;

    this.actionByObjectType = new Map<Composite, Action[]>([
      [
        m.Organization,
        [
          methodActionService.create(m.Organization.ToggleCanWrite),
          methodActionService.create(m.Organization.JustDoIt),
        ],
      ],
      [m.Person, []],
    ]);
  }

  action(objectType: Composite): Action[] {
    return this.actionByObjectType.get(objectType) ?? [];
  }
}
