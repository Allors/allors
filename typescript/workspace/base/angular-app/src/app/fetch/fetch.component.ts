import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

import {
  ContextService,
  WorkspaceService,
} from '@allors/workspace/base/angular/foundation';
import { IPullResult, Pull } from '@allors/workspace/system/domain';
import { Organization } from '@allors/workspace/default/domain';
import { M } from '@allors/workspace/default/meta';

@Component({
  templateUrl: './fetch.component.html',
  providers: [ContextService],
})
export class FetchComponent implements OnInit, OnDestroy {
  public organisation: Organization;
  public organisations: Organization[];

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    private workspaceService: WorkspaceService,
    private title: Title,
    private route: ActivatedRoute
  ) {
    this.allors.context.name = this.constructor.name;
  }

  public ngOnInit() {
    this.title.setTitle('Fetch');
    this.fetch();
  }

  public fetch() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const { context } = this.allors;
    const m = context.configuration.metaPopulation as M;
    const { pullBuilder: p } = m;

    const id = this.route.snapshot.paramMap.get('id');

    const pulls: Pull[] = [
      p.Organization({
        objectId: id,
        results: [
          {},
          {
            select: {
              Owner: {
                OrganizationsWhereOwner: {
                  include: {
                    Owner: {},
                  },
                },
              },
            },
          },
        ],
      }),
    ];

    this.subscription = context.pull(pulls).subscribe(
      (result: IPullResult) => {
        this.organisation = result.object<Organization>(m.Organization);
        this.organisations = result.collection<Organization>(
          m.Person.OrganizationsWhereOwner
        );
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
