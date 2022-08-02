import { IObject } from '@allors/system/workspace/domain';
import { DatabaseState } from '../state/database-state';

export class ChangeSetTracker {
  created: Set<IObject>;
  databaseOriginStates: Set<DatabaseState>;

  public onCreated(object: IObject) {
    (this.created ??= new Set()).add(object);
  }

  public onDatabaseChanged(state: DatabaseState) {
    (this.databaseOriginStates ??= new Set()).add(state);
  }

  onDelete(object: IObject) {
    this.created?.delete(object);
  }
}
