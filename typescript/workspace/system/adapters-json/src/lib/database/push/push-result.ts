import { PushResponse } from '@allors/database-system-protocol-json';
import { IPushResult, ISession } from '@allors/workspace-system-domain';
import { Result } from '../result';

export class PushResult extends Result implements IPushResult {
  constructor(session: ISession, response: PushResponse) {
    super(session, response);
  }
}
