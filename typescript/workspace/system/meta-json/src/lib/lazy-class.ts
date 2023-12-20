import { ObjectTypeData } from '@allors/database/system/protocol-json';
import { RelationType, RoleType } from '@allors/workspace/system/meta';

import { Lookup } from './utils/lookup';
import { InternalMetaPopulation } from './internal/internal-meta-population';
import { InternalClass } from './internal/internal-class';
import { InternalComposite } from './internal/internal-composite';

import { LazyComposite } from './lazy-composite';

export class LazyClass extends LazyComposite implements InternalClass {
  readonly kind = 'Class';
  override readonly _ = {};
  isInterface = false;
  isClass = true;
  classes: Set<InternalClass>;

  overriddenRequiredRoleTypes: RoleType[];

  requiredRoleTypes: Set<RoleType>;

  constructor(metaPopulation: InternalMetaPopulation, data: ObjectTypeData) {
    super(metaPopulation, data);
    this.classes = new Set([this]);
  }

  deriveOverridden(lookup: Lookup): void {
    this.overriddenRequiredRoleTypes = lookup.or.has(this.tag)
      ? [...lookup.or.get(this.tag)].map(
          (v) =>
            (this.metaPopulation.metaObjectByTag.get(v) as RelationType)
              .roleType
        )
      : [];

    this.requiredRoleTypes = new Set(
      [...this.roleTypes]
        .filter((v) => v.isRequired)
        .concat(this.overriddenRequiredRoleTypes)
    );
  }

  isAssignableFrom(objectType: InternalComposite): boolean {
    return this.tag === objectType?.tag;
  }

  deriveRelationEndTypeByPropertyName() {
    this.relationEndTypeByPropertyName = new Map();

    for (const roleType of this.roleTypes) {
      this.relationEndTypeByPropertyName.set(roleType.name, roleType);
    }

    for (const associationType of this.associationTypes) {
      this.relationEndTypeByPropertyName.set(
        associationType.name,
        associationType
      );
    }
  }
}
