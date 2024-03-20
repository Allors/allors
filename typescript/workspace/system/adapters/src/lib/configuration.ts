import { IdGenerator } from './database/database-connection';
declare module '@allors/workspace-system-domain' {
  interface Configuration {
    idGenerator: IdGenerator;
  }
}
