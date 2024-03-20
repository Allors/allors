import { RelationTypeData } from '@allors/database-system-protocol-json';
import {
  AssociationType,
  ObjectType,
  RelationType,
  RoleType,
} from '@allors/workspace-system-meta';

import { Lookup } from './utils/lookup';
import { InternalComposite } from './internal/internal-composite';
import { InternalMetaPopulation } from './internal/internal-meta-population';

import { LazyRoleType } from './lazy-role-type';

export class LazyRelationType implements RelationType {
  readonly kind = 'RelationType';
  readonly _ = {};
  metaPopulation: InternalMetaPopulation;

  tag: string;

  associationType: AssociationType;
  roleType: RoleType;

  constructor(
    associationObjectType: InternalComposite,
    data: RelationTypeData,
    lookup: Lookup
  ) {
    this.metaPopulation =
      associationObjectType.metaPopulation as InternalMetaPopulation;

    const [roleTag, associationTag, r] = data;
    const roleObjectType = this.metaPopulation.metaObjectByTag.get(
      r
    ) as ObjectType;

    this.tag = roleTag;
    
    this.metaPopulation.onNew(this);

    this.roleType = new LazyRoleType(
      this,
      roleTag,
      associationTag,
      associationObjectType,
      roleObjectType,
      data,
      lookup
    );
    this.associationType = this.roleType.associationType;

    if (this.roleType.objectType.isComposite) {
      (this.roleType.objectType as InternalComposite).onNewAssociationType(
        this.associationType
      );
    }

    (this.associationType.objectType as InternalComposite).onNewRoleType(
      this.roleType
    );
  }
}
