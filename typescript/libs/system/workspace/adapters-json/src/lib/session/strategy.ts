import {
  Session,
  Strategy as SystemStrategy,
} from '@allors/system/workspace/adapters';
import { Class } from '@allors/system/workspace/meta';
import { DatabaseState } from './originstate/database-origin-state';
import { DatabaseRecord } from '../database/database-record';

export class Strategy extends SystemStrategy {
  constructor(
    public override session: Session,
    public override cls: Class,
    public override id: number
  ) {
    super(session, cls, id);

    this.DatabaseState = new DatabaseState(
      this.object,
      session.workspace.database.getRecord(this.id)
    );
  }

  static fromDatabaseRecord(session: Session, databaseRecord: DatabaseRecord) {
    return new Strategy(session, databaseRecord.cls, databaseRecord.id);
  }
}
