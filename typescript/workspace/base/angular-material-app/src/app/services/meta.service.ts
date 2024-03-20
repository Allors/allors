import {
  MetaService,
  WorkspaceService,
} from '@allors/workspace-base-angular-foundation';
import { M } from '@allors/workspace-default-meta';
import {
  AssociationType,
  Composite,
  pluralize,
  RelationEndType,
  RoleType,
} from '@allors/workspace-system-meta';
import { Injectable } from '@angular/core';

@Injectable()
export class AppMetaService implements MetaService {
  singularNameByObject: Map<Composite | RelationEndType, string>;
  pluralNameByObject: Map<Composite | RelationEndType, string>;

  constructor(workspaceService: WorkspaceService) {
    const m = workspaceService.workspace.configuration.metaPopulation as M;

    this.singularNameByObject = new Map<Composite | RelationEndType, string>([
      [m.Organization, 'Company'],
      [m.Person.OrganizationsWhereShareholder, 'ShareholderCompany'],
    ]);

    this.pluralNameByObject = new Map<Composite | RelationEndType, string>([
      [m.Organization, 'Companies'],
    ]);
  }

  singularName(metaObject: Composite | RelationEndType): string {
    return this.singularNameByObject.get(metaObject) ?? metaObject.singularName;
  }

  pluralName(metaObject: Composite | RelationEndType): string {
    return (
      this.pluralNameByObject.get(metaObject) ??
      pluralize(this.singularNameByObject.get(metaObject)) ??
      metaObject.pluralName
    );
  }
}
