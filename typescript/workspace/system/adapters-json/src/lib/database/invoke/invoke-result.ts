import { IInvokeResult, ISession } from '@allors/workspace-system-domain';
import { InvokeResponse } from '@allors/database-system-protocol-json';
import { Result } from '../result';

export class InvokeResult extends Result implements IInvokeResult {
  constructor(session: ISession, invokeResponse: InvokeResponse) {
    super(session, invokeResponse);
  }
}
