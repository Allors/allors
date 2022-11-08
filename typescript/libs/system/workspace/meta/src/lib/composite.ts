import { AssociationType } from './association-type';
import { Class } from './class';
import { Interface } from './interface';
import { MethodType } from './method-type';
import { ObjectType } from './object-type';
import { RelationEndType } from './relation-end-type';
import { RoleType } from './role-type';

export interface Composite extends ObjectType {
  directSupertypes: Set<Interface>;
  directAssociationTypes: Set<AssociationType>;
  directRoleTypes: Set<RoleType>;
  directMethodTypes: Set<MethodType>;
  relationEndTypeByPropertyName: Map<string, RelationEndType>;

  supertypes: Set<Interface>;
  classes: Set<Class>;
  associationTypes: Set<AssociationType>;
  roleTypes: Set<RoleType>;
  methodTypes: Set<MethodType>;

  databaseOriginRoleTypes: Set<RoleType>;

  isAssignableFrom(objectType: Composite): boolean;
}
