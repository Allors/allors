import { Procedure } from '../../data/procedure';
import { Pull } from '../../data/pull';
import { Request } from '../request';

export interface PullRequest extends Request {
  /** List of Pulls */
  l?: Pull[];

  /** Procedure */
  p?: Procedure;
}
