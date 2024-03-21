import { RelationTypeData } from '@allors/database-system-protocol-json';
import {
  pluralize,
  Multiplicity,
  RoleType,
  AssociationType,
  ObjectType,
  Unit,
} from '@allors/workspace-system-meta';

import { Lookup } from './utils/lookup';
import { InternalComposite } from './internal/internal-composite';

import { LazyAssociationType } from './lazy-association-type';
import { InternalMetaPopulation } from './internal/internal-meta-population';

export class LazyRoleType implements RoleType {
  readonly kind = 'RoleType';
  readonly _ = {};
  isRoleType = true;
  isAssociationType = false;
  isMethodType = false;

  metaPopulation: InternalMetaPopulation;
  tag: string;
  objectType: ObjectType;
  multiplicity: Multiplicity;
  isOne: boolean;
  isMany: boolean;
  name: string;
  singularName: string;
  isDerived: boolean;
  isRequired: boolean;
  size?: number;
  precision?: number;
  scale?: number;
  mediaType?: string;

  associationType: AssociationType;

  private _pluralName?: string;

  constructor(
    associationObjectType: InternalComposite,
    data: RelationTypeData,
    lookup: Lookup
  ) {
    this.metaPopulation =
      associationObjectType.metaPopulation as InternalMetaPopulation;

    const [roleTag, associationTag, r] = data;
    this.objectType = this.metaPopulation.metaObjectByTag.get(
      r
    ) as ObjectType;

    this.tag = roleTag;

    this.multiplicity = this.objectType.isUnit
      ? Multiplicity.OneToOne
      : lookup.m.get(this.tag) ?? Multiplicity.ManyToOne;
    this.isDerived = lookup.d.has(this.tag);

    this.isOne = (this.multiplicity & 1) == 0;
    this.isMany = !this.isOne;

    this.isDerived = lookup.d.has(this.tag);
    this.isRequired = lookup.r.has(this.tag);
    this.mediaType = lookup.t.get(this.tag);

    const [, , , v0, v1, v2, v3] = data;

    this.singularName =
      (!Number.isInteger(v0) ? (v0 as string) : undefined) ??
      this.objectType.singularName;
    this._pluralName = !Number.isInteger(v1) ? (v1 as string) : undefined;

    if (this.objectType.isUnit) {
      const unit = this.objectType as Unit;
      if (unit.isString || unit.isBinary || unit.isDecimal) {
        let sizeOrScale = undefined;
        let precision = undefined;

        if (Number.isInteger(v0)) {
          sizeOrScale = v0 as number;
          precision = v1 as number;
        } else if (Number.isInteger(v1)) {
          sizeOrScale = v1 as number;
          precision = v2 as number;
        } else {
          sizeOrScale = v2 as number;
          precision = v3 as number;
        }

        if (unit.isString || unit.isBinary) {
          this.size = sizeOrScale;
        }
        if (unit.isDecimal) {
          this.scale = sizeOrScale;
          this.precision = precision;
        }
      }
    }

    this.name = this.isOne ? this.singularName : this.pluralName;

    this.metaPopulation.onNew(this);

    this.associationType = new LazyAssociationType(
      this,
      associationTag,
      associationObjectType,
      this.multiplicity
    );

    if (this.objectType.isComposite) {
      (this.objectType as InternalComposite).onNewAssociationType(
        this.associationType
      );
    }

    (this.associationType.objectType as InternalComposite).onNewRoleType(this);
  }

  get pluralName() {
    return (this._pluralName ??= pluralize(this.singularName));
  }
}
