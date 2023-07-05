import {
  IChangeSet,
  IInvokeResult,
  InvokeOptions,
  IObject,
  IPullResult,
  IPushResult,
  ISession,
  Method,
  Pull,
} from '@allors/system/workspace/domain';
import {
  AssociationType,
  Class,
  Composite,
} from '@allors/system/workspace/meta';

import { Workspace } from '../workspace/workspace';
import { ObjectBase } from '../object-base';
import { DefaultObjectRanges } from '../collections/ranges/default-object-ranges';
import { Ranges } from '../collections/ranges/ranges';

import { Strategy } from './strategy';
import { ChangeSetTracker } from './trackers/change-set-tracker';
import { PushToDatabaseTracker } from './trackers/push-to-database-tracker';
import { ChangeSet } from './change-set';

export function isNewId(id: number): boolean {
  return id < 0;
}

export abstract class Session implements ISession {
  context: string;

  changeSetTracker: ChangeSetTracker;

  pushToDatabaseTracker: PushToDatabaseTracker;

  readonly ranges: Ranges<IObject>;

  protected objectByWorkspaceId: Map<number, IObject>;

  private objectsByClass: Map<Class, Set<IObject>>;

  constructor(public workspace: Workspace) {
    this.ranges = new DefaultObjectRanges();

    this.objectByWorkspaceId = new Map();
    this.objectsByClass = new Map();

    this.changeSetTracker = new ChangeSetTracker();
    this.pushToDatabaseTracker = new PushToDatabaseTracker();
  }

  get hasChanges(): boolean {
    // TODO: Optimize
    for (const [, object] of this.objectByWorkspaceId) {
      if (object.strategy.hasChanges) {
        return true;
      }
    }

    return false;
  }

  reset(): void {
    const changeSet = this.checkpoint();
    const objects: Set<IObject> = new Set(changeSet.created);

    for (const roles of changeSet.rolesByAssociationType.values()) {
      if (roles != null) {
        for (const role of roles) {
          objects.add(role);
        }
      }
    }

    for (const associations of changeSet.associationsByRoleType.values()) {
      if (associations != null) {
        for (const association of associations) {
          objects.add(association);
        }
      }
    }

    for (const object of objects) {
      object.strategy.reset();
    }
  }

  abstract create<T extends IObject>(cls: Composite): T;

  checkpoint(): IChangeSet {
    const changeSet = new ChangeSet(this, this.changeSetTracker.created);
    if (this.changeSetTracker.databaseOriginStates != null) {
      for (const databaseOriginState of this.changeSetTracker
        .databaseOriginStates) {
        databaseOriginState.checkpoint(changeSet);
      }
    }

    this.changeSetTracker.created = null;
    this.changeSetTracker.databaseOriginStates = null;

    return changeSet;
  }

  instantiate<T extends IObject>(id: number): T;
  instantiate<T extends IObject>(ids: number[]): T[];
  instantiate<T extends IObject>(objectType: Composite): T[];
  instantiate<T extends IObject>(obj: T): T[];
  instantiate<T extends IObject>(args: unknown): unknown {
    if (typeof args === 'number') {
      return (this.getObject(args) as unknown as T) ?? null;
    }

    if (args instanceof ObjectBase) {
      return (this.getObject(args.id) as unknown as T) ?? null;
    }

    if (Array.isArray(args)) {
      return args
        .map((v) => this.getObject(v))
        .filter((v) => v != null) as unknown as T[];
    }

    if (args && args['classes']) {
      const all: T[] = [];

      for (const cls of (args as Composite).classes) {
        if (this.objectsByClass.has(cls)) {
          const strategies = this.objectsByClass.get(cls);
          strategies.forEach((v) => {
            all.push(v as T);
          });
        }
      }

      return all;
    }

    return null;
  }

  abstract onDelete(strategy: Strategy);

  public getObject(id: number): IObject {
    if (id == 0) {
      return null;
    }

    if (this.objectByWorkspaceId.has(id)) {
      return this.objectByWorkspaceId.get(id);
    }

    return this.instantiateDatabaseStrategy(id);
  }

  public getCompositeAssociation(
    role: IObject,
    associationType: AssociationType
  ): IObject {
    const roleType = associationType.roleType;

    for (const cls of (associationType.objectType as Composite).classes) {
      const associations = this.objectsByClass.get(cls);
      if (associations != null) {
        for (const association of associations) {
          if (!association.strategy.canRead(roleType)) {
            continue;
          }

          if (
            (association.strategy as Strategy).isCompositeAssociationForRole(
              roleType,
              role
            )
          ) {
            return association;
          }
        }
      }
    }

    return null;
  }

  public getCompositesAssociation(
    role: IObject,
    associationType: AssociationType
  ): IObject[] {
    const roleType = associationType.roleType;

    const results: IObject[] = [];

    for (const cls of (associationType.objectType as Composite).classes) {
      const associations = this.objectsByClass.get(cls);
      if (associations != null) {
        for (const association of associations) {
          if (!association.strategy.canRead(roleType)) {
            continue;
          }

          if (
            (association.strategy as Strategy).isCompositesAssociationForRole(
              roleType,
              role
            )
          ) {
            results.push(association);
          }
        }
      }
    }

    return results;
  }

  protected addObject(object: IObject) {
    this.objectByWorkspaceId.set(object.id, object);
    let objects = this.objectsByClass.get(object.strategy.cls);
    if (objects == null) {
      objects = new Set();
      this.objectsByClass.set(object.strategy.cls, objects);
    }

    objects.add(object);

    let strategies = this.objectsByClass.get(object.strategy.cls);
    if (strategies == null) {
      strategies = new Set();
      this.objectsByClass.set(object.strategy.cls, strategies);
    }

    strategies.add(object);
  }

  protected removeObject(object: IObject) {
    this.objectByWorkspaceId.delete(object.id);
    this.objectsByClass.get(object.strategy.cls)?.delete(object);

    const strategies = this.objectsByClass.get(object.strategy.cls);
    strategies?.delete(object);
  }

  onDatabasePushResponseNew(workspaceId: number, databaseId: number) {
    const object = this.objectByWorkspaceId.get(workspaceId);
    this.pushToDatabaseTracker.created?.delete(object);
    (object.strategy as Strategy).onDatabasePushNewId(databaseId);
    this.addObject(object);
    (object.strategy as Strategy).onDatabasePushed();
  }

  abstract invoke(
    methodOrMethods: Method | Method[],
    options?: InvokeOptions
  ): Promise<IInvokeResult>;

  abstract pull(pulls: Pull | Pull[]): Promise<IPullResult>;

  abstract push(): Promise<IPushResult>;

  abstract instantiateDatabaseStrategy(id: number): IObject;
}
