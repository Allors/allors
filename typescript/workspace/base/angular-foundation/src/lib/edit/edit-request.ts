import { Composite } from '@allors/workspace/system/meta';
import { IObject } from '@allors/workspace/system/domain';

export interface EditRequest {
  readonly kind: 'EditRequest';
  objectId: number;
  objectType?: Composite;
}
