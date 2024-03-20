import {
  Configuration,
  ISession,
  IWorkspace,
} from '@allors/workspace-system-domain';

import { Ranges } from '../collections/ranges/ranges';
import { DatabaseConnection } from '../database/database-connection';

export abstract class Workspace implements IWorkspace {
  configuration: Configuration;

  readonly ranges: Ranges<number>;

  constructor(public database: DatabaseConnection) {
    this.ranges = database.ranges;

    this.configuration = database.configuration;
  }

  abstract createSession(): ISession;
}
