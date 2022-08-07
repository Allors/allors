import { Component } from '@angular/core';
import { Organization } from '@allors/default/workspace/domain';
import {
  RefreshService,
  SharedPullService,
  WorkspaceService,
} from '@allors/base/workspace/angular/foundation';
import {
  AllorsViewSummaryPanelComponent,
  NavigationService,
  PanelService,
  ScopedService,
} from '@allors/base/workspace/angular/application';
import { IPullResult, Pull } from '@allors/system/workspace/domain';
import { M } from '@allors/default/workspace/meta';

@Component({
  selector: 'organization-summary-panel',
  templateUrl: './organization-summary-panel.component.html',
})
export class OrganizationSummaryPanelComponent extends AllorsViewSummaryPanelComponent {
  m: M;

  organization: Organization;
  contactKindsText: string;

  constructor(
    scopedService: ScopedService,
    panelService: PanelService,
    sharedPullService: SharedPullService,
    workspaceService: WorkspaceService,
    refreshService: RefreshService,
    public navigation: NavigationService
  ) {
    super(scopedService, panelService, sharedPullService, refreshService);
    this.m = workspaceService.workspace.configuration.metaPopulation as M;
  }

  onPreSharedPull(pulls: Pull[], scope?: string) {
    const {
      m: { pullBuilder: p },
    } = this;

    const id = this.scoped.id;

    pulls.push(
      p.Organization({
        name: scope,
        objectId: id,
        include: {
          Country: {},
        },
      })
    );
  }

  onPostSharedPull(pullResult: IPullResult, scope?: string) {
    this.organization = pullResult.object<Organization>(scope);
  }
}
