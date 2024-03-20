import { Composite, RoleType } from '@allors/workspace-system-meta';
import { And, Predicate, Node } from '@allors/workspace-system-domain';

export interface SearchOptions {
  objectType: Composite;
  roleTypes: RoleType[];
  predicates?: Predicate[];
  post?: (and: And) => void;
  include?: Node[] | any;
  take?: number;
}
