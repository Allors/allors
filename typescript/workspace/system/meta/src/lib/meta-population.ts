import { AssociationType } from './association-type';
import { Class } from './class';
import { Interface } from './interface';
import { MetaObject } from './meta-object';
import { MethodType } from './method-type';
import { RoleType } from './role-type';
import { Unit } from './unit';
import { Composite } from './composite';

export interface MetaPopulation {
  readonly kind: 'MetaPopulation';
  metaObjectByTag: Map<string, MetaObject>;
  objectTypeByUppercaseName: Map<string, MetaObject>;
  units: Set<Unit>;
  interfaces: Set<Interface>;
  classes: Set<Class>;
  composites: Set<Composite>;
  associationTypes: Set<AssociationType>;
  roleTypes: Set<RoleType>;
  methodTypes: Set<MethodType>;
}
