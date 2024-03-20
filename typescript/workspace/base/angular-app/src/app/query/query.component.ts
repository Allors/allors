import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription } from 'rxjs';

import {
  ContextService,
  WorkspaceService,
} from '@allors/workspace-base-angular-foundation';
import { IPullResult, Pull } from '@allors/workspace-system-domain';
import { M } from '@allors/workspace-default-meta';
import { Organization } from '@allors/workspace-default-domain';

@Component({
  templateUrl: './query.component.html',
  providers: [ContextService],
})
export class QueryComponent implements OnInit, OnDestroy {
  public organisations: Organization[];

  public organisationCount: number;
  public skip = 1;
  public take = 2;

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    private workspaceService: WorkspaceService,
    private title: Title
  ) {
    this.allors.context.name = this.constructor.name;
  }

  public ngOnInit() {
    this.title.setTitle('Query');
    this.query();
  }

  public query() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const { context } = this.allors;
    const m = context.configuration.metaPopulation as M;
    const { pullBuilder: p } = m;

    const pulls: Pull[] = [
      p.Organization({
        sorting: [{ roleType: m.Organization.Name }],
        select: {
          include: {
            Owner: {},
          },
        },
        skip: this.skip || 0,
        take: this.take || 10,
      }),
    ];

    this.subscription = context.pull(pulls).subscribe(
      (result: IPullResult) => {
        this.organisations = result.collection<Organization>(m.Organization);
      },
      (error) => {
        alert(error);
      }
    );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
