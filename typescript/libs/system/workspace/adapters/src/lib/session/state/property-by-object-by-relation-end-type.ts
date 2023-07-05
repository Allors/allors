import { IObject } from '@allors/system/workspace/domain';
import { RelationEndType } from '@allors/system/workspace/meta';
import { MapMap } from '../../collections/map-map';
import { IRange, Ranges } from '../../collections/ranges/ranges';

export class RelationEndByObjectByRelationEndType {
  private propertyByObjectByRelationEndType: MapMap<
    RelationEndType,
    IObject,
    unknown
  >;

  private changedRelationEndByObjectByRelationEndType: MapMap<
    RelationEndType,
    IObject,
    unknown
  >;

  public constructor(private ranges: Ranges<IObject>) {
    this.propertyByObjectByRelationEndType = new MapMap();
    this.changedRelationEndByObjectByRelationEndType = new MapMap();
  }

  public get(object: IObject, relationEndType: RelationEndType): unknown {
    if (this.changedRelationEndByObjectByRelationEndType.has(relationEndType, object)) {
      return this.changedRelationEndByObjectByRelationEndType.get(
        relationEndType,
        object
      );
    }

    return this.propertyByObjectByRelationEndType.get(relationEndType, object);
  }

  public set(object: IObject, relationEndType: RelationEndType, newValue: unknown) {
    const originalValue = this.propertyByObjectByRelationEndType.get(
      relationEndType,
      object
    ) as IRange<IObject>;

    if (
      relationEndType.isOne
        ? newValue === originalValue
        : this.ranges.equals(newValue as IObject[], originalValue)
    ) {
      this.changedRelationEndByObjectByRelationEndType.remove(relationEndType, object);
    } else {
      this.changedRelationEndByObjectByRelationEndType.set(
        relationEndType,
        object,
        newValue
      );
    }
  }

  public checkpoint(): MapMap<RelationEndType, IObject, unknown> {
    try {
      const changeSet = this.changedRelationEndByObjectByRelationEndType;

      changeSet.mapMap.forEach((changedMap, relationEndType) => {
        let propertyByObject =
          this.propertyByObjectByRelationEndType.mapMap.get(relationEndType);

        changedMap.forEach((changedProperty, object) => {
          if (changedProperty == null) {
            propertyByObject?.delete(object);
          } else {
            if (propertyByObject == null) {
              propertyByObject = new Map();
              this.propertyByObjectByRelationEndType.mapMap.set(
                relationEndType,
                propertyByObject
              );
            }

            propertyByObject.set(object, changedProperty);
          }
        });

        if (propertyByObject?.size === 0) {
          this.propertyByObjectByRelationEndType.mapMap.delete(relationEndType);
        }
      });

      return changeSet;
    } finally {
      this.changedRelationEndByObjectByRelationEndType = new MapMap();
    }
  }
}
