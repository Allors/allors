import { M } from '@allors/workspace-default-meta';
import { Composite, RelationType } from '@allors/workspace-system-meta';
import { WorkspaceService } from '@allors/workspace-base-angular-foundation';

import { Injectable } from '@angular/core';
import { IconService } from '@allors/workspace-base-angular-material-application';

@Injectable()
export class AppIconService implements IconService {
  iconByComposite: Map<Composite | RelationType, string>;

  constructor(workspaceService: WorkspaceService) {
    const m = workspaceService.workspace.configuration.metaPopulation as M;

    this.iconByComposite = new Map();
    this.iconByComposite.set(m.Country, 'public');
    this.iconByComposite.set(m.Organization, 'domain');
    this.iconByComposite.set(m.Person, 'person');
  }

  icon(meta: Composite | RelationType): string {
    return this.iconByComposite.get(meta);
  }
}
