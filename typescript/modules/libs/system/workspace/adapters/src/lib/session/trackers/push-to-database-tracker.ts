import { IObject } from '@allors/system/workspace/domain';
import { DatabaseState } from '../state/database-state';

export class PushToDatabaseTracker {
  created: Set<IObject>;

  changed: Set<DatabaseState>;

  onCreated(strategy: IObject) {
    (this.created ??= new Set<IObject>()).add(strategy);
  }

  onChanged(state: DatabaseState) {
    if (!state.object.strategy.isNew) {
      (this.changed ??= new Set<DatabaseState>()).add(state);
    }
  }

  onDelete(object: IObject) {
    this.created?.delete(object);
  }
}
