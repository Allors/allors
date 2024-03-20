import { RelationType } from '@allors/workspace-system-meta';
import { IObject } from '../iobject';

export interface IDiff {
  relationType: RelationType;

  assocation: IObject;
}
