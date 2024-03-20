import { RelationType } from '@allors/workspace-system-meta';
import { IObject } from './iobject';

export interface Role {
  object: IObject;

  relationType: RelationType;
}
