import { IChangeSet, IObject } from '@allors/workspace-system-domain';
import {
  AssociationType,
  RelationEndType,
  RoleType,
} from '@allors/workspace-system-meta';

import { MapMap } from '../collections/map-map';
import { frozenEmptySet } from '../collections/frozen-empty-set';
import { IRange, Ranges } from '../collections/ranges/ranges';

import { Session } from './session';

export class ChangeSet implements IChangeSet {
  associationsByRoleType: Map<RoleType, Set<IObject>>;
  rolesByAssociationType: Map<AssociationType, Set<IObject>>;

  private ranges: Ranges<IObject>;

  public constructor(public session: Session, public created: Set<IObject>) {
    this.associationsByRoleType = new Map();
    this.rolesByAssociationType = new Map();
    this.created ??= frozenEmptySet as Set<IObject>;

    this.ranges = this.session.ranges;
  }

  public addSessionStateChanges(
    sessionStateChangeSet: MapMap<RelationEndType, IObject, unknown>
  ) {
    for (const [relationEndType, map] of sessionStateChangeSet.mapMap) {
      const strategies = new Set<IObject>();

      for (const [strategy] of map) {
        strategies.add(strategy);
      }

      if (relationEndType.isAssociationType) {
        this.rolesByAssociationType.set(
          relationEndType as AssociationType,
          strategies
        );
      } else if (relationEndType.isRoleType) {
        this.associationsByRoleType.set(
          relationEndType as RoleType,
          strategies
        );
      } else {
        throw new Error(
          `RelationEndType ${relationEndType.name} is not supported`
        );
      }
    }
  }

  public diffUnit(
    association: IObject,
    roleType: RoleType,
    current: unknown,
    previous: unknown
  ) {
    if (current !== previous) {
      this.addAssociationByRoleType(roleType, association);
    }
  }

  public diffCompositeObjectRecord(
    association: IObject,
    roleType: RoleType,
    currentRole: IObject,
    previousRoleId: number
  ) {
    if (currentRole?.id === previousRoleId) {
      return;
    }

    if (previousRoleId != null) {
      const previousRole = (this.session as Session).getObject(previousRoleId);
      if (previousRole) {
        this.addRoleByAssociationType(
          roleType.associationType,
          previousRole
        );
      }
    }

    if (currentRole != null) {
      this.addRoleByAssociationType(roleType.associationType, currentRole);
    }

    this.addAssociationByRoleType(roleType, association);
  }

  public diffCompositeRecordRecord(
    association: IObject,
    roleType: RoleType,
    currentRoleId: number,
    previousRoleId: number
  ) {
    if (currentRoleId === previousRoleId) {
      return;
    }

    if (previousRoleId != null) {
      this.addRoleByAssociationType(
        roleType.associationType,
        (this.session as Session).getObject(previousRoleId)
      );
    }

    if (currentRoleId != null) {
      this.addRoleByAssociationType(
        roleType.associationType,
        (this.session as Session).getObject(currentRoleId)
      );
    }

    this.addAssociationByRoleType(roleType, association);
  }

  public diffCompositeObjectObject(
    association: IObject,
    roleType: RoleType,
    currentRole: IObject,
    previousRole: IObject
  ) {
    if (currentRole === previousRole) {
      return;
    }

    if (previousRole != null) {
      this.addRoleByAssociationType(roleType.associationType, previousRole);
    }

    if (currentRole != null) {
      this.addRoleByAssociationType(roleType.associationType, currentRole);
    }

    this.addAssociationByRoleType(roleType, association);
  }

  public diffCompositeObjectsRecords(
    association: IObject,
    roleType: RoleType,
    currentRoles: IRange<IObject>,
    previousRoleIds: IRange<number>
  ) {
    const previousRoles: IRange<IObject> = previousRoleIds?.map((v) =>
      (this.session as Session).getObject(v)
    );
    this.diffCompositeObjectsObjects(
      association,
      roleType,
      currentRoles,
      previousRoles
    );
  }

  public diffCompositeRecordsRecords(
    association: IObject,
    roleType: RoleType,
    currentRoleIds: IRange<number>,
    previousRoleIds: IRange<number>
  ) {
    const currentRoles: IRange<IObject> = currentRoleIds?.map((v) =>
      (this.session as Session).getObject(v)
    );
    const previousRoles: IRange<IObject> = previousRoleIds?.map((v) =>
      (this.session as Session).getObject(v)
    );
    this.diffCompositeObjectsObjects(
      association,
      roleType,
      currentRoles,
      previousRoles
    );
  }

  public diffCompositeObjectsObjects(
    association: IObject,
    roleType: RoleType,
    currentRoles: IRange<IObject>,
    previousRoles: IRange<IObject>
  ) {
    let hasChange = false;

    for (const v of this.ranges.enumerate(previousRoles)) {
      if (!this.ranges.has(currentRoles, v)) {
        this.addRoleByAssociationType(roleType.associationType, v);
        hasChange = true;
      }
    }

    for (const v of this.ranges.enumerate(currentRoles)) {
      if (!this.ranges.has(previousRoles, v)) {
        this.addRoleByAssociationType(roleType.associationType, v);
        hasChange = true;
      }
    }

    if (hasChange) {
      this.addAssociationByRoleType(roleType, association);
    }
  }

  private addAssociationByRoleType(roleType: RoleType, association: IObject) {
    if (association == null) {
      // TODO: Investigate
      return;
    }

    let associations = this.associationsByRoleType.get(roleType);
    if (!associations) {
      associations = new Set();
      this.associationsByRoleType.set(roleType, associations);
    }

    associations.add(association);
  }

  private addRoleByAssociationType(
    associationType: AssociationType,
    role: IObject
  ) {
    if (role == null) {
      // TODO: Investigate
      return;
    }

    let roles = this.rolesByAssociationType.get(associationType);
    if (!roles) {
      roles = new Set();
      this.rolesByAssociationType.set(associationType, roles);
    }

    roles.add(role);
  }
}
