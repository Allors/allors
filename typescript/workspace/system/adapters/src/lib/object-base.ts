import { IObject, IStrategy } from '@allors/workspace/system/domain';

export abstract class ObjectBase implements IObject {
  get id(): number {
    return this.strategy.id;
  }

  strategy: IStrategy;

  init() {}
}
