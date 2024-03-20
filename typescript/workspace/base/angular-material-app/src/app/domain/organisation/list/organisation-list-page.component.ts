import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Sort } from '@angular/material/sort';
import { PageEvent } from '@angular/material/paginator';
import { M } from '@allors/workspace-default-meta';
import { Organization } from '@allors/workspace-default-domain';
import {
  ContextService,
  CreateService,
  FilterService,
  MetaService,
  Table,
  TableRow,
} from '@allors/workspace-base-angular-foundation';
import {
  Action,
  Filter,
  FilterField,
  MediaService,
  RefreshService,
} from '@allors/workspace-base-angular-foundation';
import {
  AllorsListPageComponent,
  NavigationService,
} from '@allors/workspace-base-angular-application';
import {
  DeleteActionService,
  MethodActionService,
  OverviewActionService,
  SorterService,
} from '@allors/workspace-base-angular-material-application';

interface Row extends TableRow {
  object: Organization;
  name: string;
  country: string;
  owner: string;
}

@Component({
  templateUrl: './organisation-list-page.component.html',
  providers: [ContextService],
})
export class OrganizationListPageComponent
  extends AllorsListPageComponent
  implements OnInit, OnDestroy
{
  public override title = 'Organizations';

  table: Table<Row>;

  delete: Action;

  private subscription: Subscription;
  filter: Filter;
  override m: M;

  constructor(
    @Self() allors: ContextService,
    titleService: Title,
    public createService: CreateService,
    public refreshService: RefreshService,
    public overviewService: OverviewActionService,
    public deleteService: DeleteActionService,
    public methodService: MethodActionService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    public filterService: FilterService,
    public sorterService: SorterService,
    metaService: MetaService
  ) {
    super(allors, metaService, titleService);
    this.objectType = this.m.Organization;

    this.delete = deleteService.delete();
    this.delete.result.subscribe(() => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [{ name: 'name', sort: true }, 'country', 'owner'],
      actions: [overviewService.overview(), this.delete],
      defaultAction: overviewService.overview(),
      pageSize: 50,
      initialSort: 'name',
    });
  }

  public ngOnInit(): void {
    const m = this.m;
    const { pullBuilder: pull } = m;

    this.filter = this.filterService.filter(m.Organization);

    this.subscription = combineLatest([
      this.refreshService.refresh$,
      this.filter.fields$,
      this.table.sort$,
      this.table.pager$,
    ])
      .pipe(
        scan(
          (
            [previousRefresh, previousFilterFields],
            [refresh, filterFields, sort, pageEvent]
          ) => {
            pageEvent =
              previousRefresh !== refresh ||
              filterFields !== previousFilterFields
                ? {
                    ...pageEvent,
                    pageIndex: 0,
                  }
                : pageEvent;

            if (pageEvent.pageIndex === 0) {
              this.table.pageIndex = 0;
            }

            return [refresh, filterFields, sort, pageEvent];
          }
        ),
        switchMap(
          ([, filterFields, sort, pageEvent]: [
            Date,
            FilterField[],
            Sort,
            PageEvent
          ]) => {
            const pulls = [
              pull.Organization({
                predicate: this.filter.definition.predicate,
                sorting: sort
                  ? this.sorterService.sorter(m.Organization)?.create(sort)
                  : null,
                include: {
                  Owner: {},
                  Country: {},
                },
                arguments: this.filter.parameters(filterFields),
                skip: pageEvent.pageIndex * pageEvent.pageSize,
                take: pageEvent.pageSize,
              }),
            ];

            return this.allors.context.pull(pulls);
          }
        )
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        const organisations = loaded.collection<Organization>(m.Organization);
        this.table.data = organisations?.map((v) => {
          return {
            object: v,
            name: v.Name,
            country: v.Country?.Key ?? null,
            owner: v.Owner?.UserName ?? null,
          } as Row;
        });
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
