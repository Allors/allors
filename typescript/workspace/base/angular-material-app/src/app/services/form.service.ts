import { M, tags } from '@allors/workspace/default/meta';
import { Composite } from '@allors/workspace/system/meta';
import {
  FormService,
  WorkspaceService,
} from '@allors/workspace/base/angular/foundation';

import { CountryFormComponent } from '../domain/country/form/country-form.component';
import { EmploymentFormComponent } from '../domain/employment/form/employment-form.component';
import { OrganizationFormComponent } from '../domain/organisation/form/organisation-form.component';

import { Injectable } from '@angular/core';
import { PersonFormComponent } from '../domain/person/form/person-form.component';

@Injectable()
export class AppFormService implements FormService {
  m: M;

  constructor(workspaceService: WorkspaceService) {
    this.m = workspaceService.workspace.configuration.metaPopulation as M;
  }

  createForm(composite: Composite) {
    return this.editForm(composite);
  }

  editForm(composite: Composite) {
    switch (composite.tag) {
      // Objects
      case tags.Country:
        return CountryFormComponent;

      case tags.Organization:
        return OrganizationFormComponent;

      case tags.Person:
        return PersonFormComponent;

      // Relationships
      case tags.Employment:
        return EmploymentFormComponent;
    }

    return null;
  }
}

export const components: any[] = [
  CountryFormComponent,
  PersonFormComponent,
  OrganizationFormComponent,
  EmploymentFormComponent,
];
