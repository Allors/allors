import {
  IDiff,
  IObject,
  IStrategy,
  IUnit,
} from '@allors/workspace/system/domain';
import {
  AssociationType,
  Class,
  Composite,
  MethodType,
  RoleType,
  UnitTags,
} from '@allors/workspace/system/meta';

import { DatabaseState } from './state/database-state';
import { frozenEmptyArray } from '../collections/frozen-empty-array';
import { isNewId, Session } from './session';

export abstract class Strategy implements IStrategy {
  DatabaseState: DatabaseState;

  rangeId: number;
  private _object: IObject;

  constructor(public session: Session, public cls: Class, public id: number) {
    this.rangeId = id;
  }

  get isDeleted() {
    return this.id === 0;
  }

  get version(): number {
    return this.DatabaseState.version;
  }

  get isNew(): boolean {
    return isNewId(this.id);
  }

  get object(): IObject {
    return (this._object ??=
      this.session.workspace.database.configuration.objectFactory.create(this));
  }

  delete(): void {
    if (!this.isNew) {
      throw new Error('Existing database objects can not be deleted');
    }

    for (const roleType of this.cls.roleTypes) {
      if (roleType.objectType.isUnit) {
        this.setUnitRole(roleType, null);
      } else if (roleType.isOne) {
        this.setCompositeRole(roleType, null);
      } else {
        this.setCompositesRole(roleType, null);
      }
    }

    for (const associationType of this.cls.associationTypes) {
      const roleType = associationType.roleType;
      if (associationType.isOne) {
        const association = this.getCompositeAssociation(associationType);
        if (roleType.isOne) {
          association?.strategy.setCompositeRole(roleType, null);
        } else {
          association?.strategy.removeCompositesRole(roleType, this.object);
        }
      } else {
        const association = this.getCompositesAssociation(associationType);
        if (roleType.isOne) {
          association?.forEach((v) =>
            v.strategy.setCompositeRole(roleType, null)
          );
        } else {
          association?.forEach((v) =>
            v.strategy.removeCompositesRole(roleType, this.object)
          );
        }
      }
    }

    this.session.onDelete(this);

    this.id = 0;
  }

  reset(): void {
    this.DatabaseState?.reset();
  }

  diff(): IDiff[] {
    const diffs: IDiff[] = [];
    this.DatabaseState.diff(diffs);
    return diffs;
  }

  get hasChanges(): boolean {
    return this.DatabaseState?.hasChanges;
  }

  hasChanged(roleType: RoleType): boolean {
    return this.canRead(roleType)
      ? this.DatabaseState?.hasChanged(roleType) ?? false
      : false;
  }

  restoreRole(roleType: RoleType) {
    return this.canRead(roleType)
      ? this.DatabaseState?.restoreRole(roleType)
      : false;
  }

  existRole(roleType: RoleType): boolean {
    if (roleType.objectType.isUnit) {
      return this.getUnitRole(roleType) != null;
    }

    if (roleType.isOne) {
      return this.getCompositeRole(roleType) != null;
    }

    return this.getCompositesRole(roleType)?.length > 0;
  }

  getRole(roleType: RoleType): unknown {
    if (roleType == null) {
      throw new Error('Argument null');
    }

    if (roleType.objectType.isUnit) {
      return this.getUnitRole(roleType);
    }

    if (roleType.isOne) {
      return this.getCompositeRole(roleType);
    }

    return this.getCompositesRole(roleType);
  }

  getUnitRole(roleType: RoleType): IUnit {
    return (
      (this.canRead(roleType)
        ? this.DatabaseState?.getUnitRole(roleType)
        : null) ?? null
    );
  }

  getCompositeRole<T extends IObject>(
    roleType: RoleType,
    skipMissing?: boolean
  ): T {
    return this.canRead(roleType)
      ? (this.DatabaseState?.getCompositeRole(roleType, skipMissing) as T) ??
          null
      : null;
  }

  getCompositesRole<T extends IObject>(
    roleType: RoleType,
    skipMissing?: boolean
  ): T[] {
    return this.canRead(roleType)
      ? (this.DatabaseState?.getCompositesRole(roleType, skipMissing) as T[]) ??
          (frozenEmptyArray as T[])
      : (frozenEmptyArray as T[]);
  }

  setRole(roleType: RoleType, value: unknown) {
    if (roleType.objectType.isUnit) {
      this.setUnitRole(roleType, value as IUnit);
    } else if (roleType.isOne) {
      this.setCompositeRole(roleType, value as any);
    } else {
      this.setCompositesRole(roleType, value as any);
    }
  }

  setUnitRole(roleType: RoleType, value: IUnit) {
    this.assertUnit(roleType, value);

    if (this.canWrite(roleType)) {
      this.DatabaseState?.setUnitRole(roleType, value);
    }
  }

  setCompositeRole<T extends IObject>(roleType: RoleType, value: T) {
    this.assertComposite(value);

    if (value != null) {
      this.assertSameType(roleType, value);
      this.assertSameSession(value);
    }

    if (roleType.isMany) {
      throw new Error('Wrong multiplicity');
    }

    if (this.canWrite(roleType)) {
      this.DatabaseState?.setCompositeRole(roleType, value);
    }
  }

  setCompositesRole(roleType: RoleType, role: ReadonlyArray<IObject>) {
    this.assertComposites(role);

    if (this.canWrite(roleType)) {
      this.DatabaseState?.setCompositesRole(
        roleType,
        this.session.ranges.importFrom(role)
      );
    }
  }

  addCompositesRole<T extends IObject>(roleType: RoleType, value: T) {
    if (value == null) {
      return;
    }

    this.assertComposite(value);

    this.assertSameType(roleType, value);

    if (roleType.isOne) {
      throw new Error('wrong multiplicity');
    }

    if (this.canWrite(roleType)) {
      this.DatabaseState.addCompositesRole(roleType, value);
    }
  }

  removeCompositesRole<T extends IObject>(roleType: RoleType, value: T) {
    if (value == null) {
      return;
    }

    this.assertComposite(value);

    if (this.canWrite(roleType)) {
      this.DatabaseState.removeCompositesRole(roleType, value);
    }
  }

  removeRole(roleType: RoleType) {
    if (roleType.objectType.isUnit) {
      this.setUnitRole(roleType, null);
    } else if (roleType.isOne) {
      this.setCompositeRole(roleType, null);
    } else {
      this.setCompositesRole(roleType, null);
    }
  }

  getCompositeAssociation<T extends IObject>(
    associationType: AssociationType
  ): T {
    return (
      (this.session.getCompositeAssociation(
        this.object,
        associationType
      ) as T) ?? null
    );
  }

  getCompositesAssociation<T extends IObject>(
    associationType: AssociationType
  ): T[] {
    return this.session.getCompositesAssociation(
      this.object,
      associationType
    ) as T[];
  }

  canRead(roleType: RoleType): boolean {
    return this.DatabaseState?.canRead(roleType) ?? true;
  }

  canWrite(roleType: RoleType): boolean {
    return this.DatabaseState?.canWrite(roleType) ?? true;
  }

  canExecute(methodType: MethodType): boolean {
    return this.DatabaseState?.canExecute(methodType);
  }

  isCompositeAssociationForRole(roleType: RoleType, forRole: IObject): boolean {
    return this.DatabaseState?.isAssociationForRole(roleType, forRole) ?? false;
  }

  isCompositesAssociationForRole(
    roleType: RoleType,
    forRole: IObject
  ): boolean {
    return this.DatabaseState?.isAssociationForRole(roleType, forRole) ?? false;
  }

  onDatabasePushNewId(newId: number) {
    this.id = newId;
  }

  onDatabasePushed() {
    this.DatabaseState.onPushed();
  }

  assertSameType(roleType: RoleType, value: IObject) {
    const composite = roleType.objectType as Composite;
    if (!composite.isAssignableFrom(value.strategy.cls)) {
      throw new Error(
        `Types do not match: ${composite} and ${value.strategy.cls}`
      );
    }
  }

  assertSameSession(value: IObject) {
    if (this.session != value.strategy.session) {
      throw new Error('Sessions do not match');
    }
  }

  assertUnit(roleType: RoleType, value: unknown) {
    if (value == null) {
      return;
    }

    let error = false;

    switch (roleType.objectType.tag) {
      case UnitTags.Binary:
      case UnitTags.Decimal:
      case UnitTags.String:
      case UnitTags.Unique:
        error = typeof value !== 'string';
        break;
      case UnitTags.Boolean:
        error = typeof value !== 'boolean';
        break;
      case UnitTags.DateTime:
        error = !(value instanceof Date);
        break;
      case UnitTags.Float:
        error = isNaN(value as number);
        break;
      case UnitTags.Integer:
        error = !Number.isInteger(value as number);
        break;
    }

    if (error) {
      throw new Error(`value is not a ${roleType.objectType.singularName}`);
    }
  }

  assertComposite(value: IObject) {
    if (value == null) {
      return;
    }

    if (this.session != value.strategy.session) {
      throw new Error('Strategy is from a different session');
    }
  }

  assertComposites(inputs: ReadonlyArray<IObject>) {
    if (inputs == null) {
      return;
    }

    inputs.forEach((v) => this.assertComposite(v));
  }

  toString() {
    return JSON.stringify(this);
  }

  toJSON() {
    return {
      id: this.id,
    };
  }
}
