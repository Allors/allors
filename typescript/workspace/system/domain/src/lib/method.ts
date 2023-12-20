import { MethodType } from '@allors/workspace/system/meta';
import { IObject } from './iobject';

export interface Method {
  object: IObject;

  methodType: MethodType;
}
