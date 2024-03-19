import {
  AssociationType,
  Multiplicity,
  RelationType,
  RoleType,
} from '@allors/workspace/system/meta';

import { InternalComposite } from './internal/internal-composite';
import { InternalMetaPopulation } from './internal/internal-meta-population';

export class LazyAssociationType implements AssociationType {
  metaPopulation: InternalMetaPopulation;
  
  readonly kind = 'AssociationType';
  readonly _ = {};
  isRoleType = false;
  isAssociationType = true;
  isMethodType = false;

  relationType: RelationType;
  operandTag: string;
  isOne: boolean;
  isMany: boolean;
  name: string;
  singularName: string;

  private _pluralName?: string;

  constructor(
    public roleType: RoleType,
    public tag: string,
    public objectType: InternalComposite,
    multiplicity: Multiplicity
  ) {
    this.metaPopulation = roleType.metaPopulation as InternalMetaPopulation;

    this.relationType = roleType.relationType;
    this.operandTag = this.tag;
    this.isOne = (multiplicity & 2) == 0;
    this.isMany = !this.isOne;
    this.singularName =
      this.objectType.singularName + 'Where' + this.roleType.singularName;
    this.name = this.isOne ? this.singularName : this.pluralName;
  }

  get pluralName() {
    return (this._pluralName ??=
      this.objectType.pluralName + 'Where' + this.roleType.singularName);
  }
}
