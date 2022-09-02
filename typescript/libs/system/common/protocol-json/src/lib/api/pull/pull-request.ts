import { Pull } from '../../data/pull';
import { Request } from '../request';

export interface PullRequest extends Request {
  /** List of Pulls */
  l?: Pull[];
}
