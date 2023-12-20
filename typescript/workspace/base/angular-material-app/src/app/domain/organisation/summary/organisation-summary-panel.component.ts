import { Component } from '@angular/core';
import { Organisation } from '@allors/workspace/default/domain';
import {
  RefreshService,
  SharedPullService,
  WorkspaceService,
} from '@allors/workspace/base/angular-foundation';
import {
  AllorsViewSummaryPanelComponent,
  NavigationService,
  PanelService,
  ScopedService,
} from '@allors/workspace/base/angular-application';
import { IPullResult, Pull } from '@allors/workspace/system/domain';
import { M } from '@allors/workspace/default/meta';

@Component({
  selector: 'organisation-summary-panel',
  templateUrl: './organisation-summary-panel.component.html',
})
export class OrganisationSummaryPanelComponent extends AllorsViewSummaryPanelComponent {
  m: M;

  organisation: Organisation;
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
      p.Organisation({
        name: scope,
        objectId: id,
        include: {
          Country: {},
        },
      })
    );
  }

  onPostSharedPull(pullResult: IPullResult, scope?: string) {
    this.organisation = pullResult.object<Organisation>(scope);
  }
}
