import {
  AssociationType,
  Composite,
  RelationEndType,
  RoleType,
} from '@allors/workspace-system-meta';
import { IObject } from '../iobject';
import { IStrategy } from '../istrategy';

export interface Node {
  relationEndType: RelationEndType;
  ofType?: Composite;
  nodes?: Node[];
}

function getComposite(
  strategy: IStrategy,
  relationEndType: RelationEndType,
  ofType: Composite,
  skipMissing?: boolean
): IObject {
  const composite = relationEndType.isRoleType
    ? strategy.getCompositeRole(relationEndType as RoleType, skipMissing)
    : strategy.getCompositeAssociation(relationEndType as AssociationType);

  if (composite == null || ofType == null) {
    return composite;
  }

  return ofType.isAssignableFrom(composite.strategy.cls) ? composite : null;
}

function getComposites(
  strategy: IStrategy,
  relationEndType: RelationEndType,
  ofType: Composite,
  skipMissing?: boolean
): Readonly<IObject[]> {
  const composites = relationEndType.isRoleType
    ? strategy.getCompositesRole(relationEndType as RoleType, skipMissing)
    : strategy.getCompositesAssociation(relationEndType as AssociationType);

  if (composites == null || ofType == null) {
    return composites;
  }

  return composites.filter((v) => ofType.isAssignableFrom(v.strategy.cls));
}

function resolveRecursive(
  object: IObject,
  node: Node,
  results: Set<IObject>,
  skipMissing?: boolean
): void {
  if (node.relationEndType.isOne) {
    const resolved = getComposite(
      object.strategy,
      node.relationEndType,
      node.ofType,
      skipMissing
    );
    if (resolved != null) {
      if (node.nodes?.length > 0) {
        for (const subNode of node.nodes) {
          resolveRecursive(resolved, subNode, results, skipMissing);
        }
      } else {
        results.add(resolved);
      }
    }
  } else {
    const resolveds = getComposites(
      object.strategy,
      node.relationEndType,
      node.ofType,
      skipMissing
    );
    if (resolveds != null) {
      if (node.nodes?.length > 0) {
        for (const resolved of resolveds) {
          for (const subNode of node.nodes) {
            resolveRecursive(resolved, subNode, results, skipMissing);
          }
        }
      } else {
        for (const resolved of resolveds) {
          results.add(resolved);
        }
      }
    }
  }
}

export function nodeResolve(
  obj: IObject,
  node: Node,
  skipMissing?: boolean
): Set<IObject> {
  const results: Set<IObject> = new Set();
  resolveRecursive(obj, node, results, skipMissing);
  return results;
}

function resolveLeafs(node: Node, results: Set<Node>): void {
  if (node.nodes.length > 0) {
    for (const child of node.nodes) {
      resolveLeafs(child, results);
    }
  } else {
    results.add(this);
  }
}

export function nodeLeafs(node: Node): Set<Node> {
  const results: Set<Node> = new Set();
  resolveLeafs(node, results);
  return results;
}
