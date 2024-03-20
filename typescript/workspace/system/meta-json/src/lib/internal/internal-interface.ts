import {
  AssociationType,
  Interface,
  MethodType,
  RoleType,
} from '@allors/workspace-system-meta';
import { InternalComposite } from './internal-composite';

export interface InternalInterface extends InternalComposite, Interface {
  deriveSub(): void;

  associationTypeGenerator(): IterableIterator<AssociationType>;

  roleTypeGenerator(): IterableIterator<RoleType>;

  methodTypeGenerator(): IterableIterator<MethodType>;
}
