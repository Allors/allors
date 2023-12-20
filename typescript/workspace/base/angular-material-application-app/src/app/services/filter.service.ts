import {
  Filter,
  FilterDefinition,
  FilterService,
  WorkspaceService,
} from '@allors/workspace/base/angular/foundation';
import { M, tags } from '@allors/workspace/default/meta';
import { Composite } from '@allors/workspace/system/meta';
import { Injectable } from '@angular/core';

@Injectable()
export class AppFilterService implements FilterService {
  m: M;

  countryFilter: Filter;
  organisationFilter: Filter;
  personFilter: Filter;

  constructor(workspaceService: WorkspaceService) {
    this.m = workspaceService.workspace.configuration.metaPopulation as M;
  }

  filter(composite: Composite): Filter {
    const { m } = this;

    switch (composite.tag) {
      case tags.Country:
        return (this.countryFilter ??= new Filter(
          new FilterDefinition({
            kind: 'And',
            operands: [
              {
                kind: 'Like',
                roleType: m.Country.IsoCode,
                parameter: 'isoCode',
              },
            ],
          })
        ));

      case tags.Organization:
        return (this.organisationFilter ??= new Filter(
          new FilterDefinition({
            kind: 'And',
            operands: [
              {
                kind: 'Like',
                roleType: m.Organization.Name,
                parameter: 'name',
              },
              {
                kind: 'Like',
                roleType: m.Country.IsoCode,
                parameter: 'country',
              },
            ],
          })
        ));

      case tags.Person:
        return (this.personFilter ??= new Filter(
          new FilterDefinition({
            kind: 'And',
            operands: [
              {
                kind: 'Like',
                roleType: m.Person.FirstName,
                parameter: 'firstName',
              },
              {
                kind: 'Like',
                roleType: m.Person.LastName,
                parameter: 'lastName',
              },
              {
                kind: 'Like',
                roleType: m.Person.UserEmail,
                parameter: 'email',
              },
            ],
          })
        ));
    }

    return null;
  }
}
