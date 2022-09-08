import { M, tags } from '@allors/default/workspace/meta';
import { Composite } from '@allors/system/workspace/meta';
import {
  FormService,
  WorkspaceService,
} from '@allors/base/workspace/angular/foundation';

import { CountryFormComponent } from '../domain/country/form/country-form.component';
import { EmploymentFormComponent } from '../domain/employment/form/employment-form.component';
import { OrganizationFormComponent } from '../domain/organization/form/organization-form.component';

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
      case tags.Country:
        return CountryFormComponent;

      case tags.Organization:
        return OrganizationFormComponent;

      case tags.Person:
        return PersonFormComponent;

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
