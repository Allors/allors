import { Class } from '@allors/workspace/system/meta';
import { Initializer } from '@allors/workspace/system/domain';

export interface CreateRequest {
  readonly kind: 'CreateRequest';
  objectType: Class;
  initializer?: Initializer;
}
