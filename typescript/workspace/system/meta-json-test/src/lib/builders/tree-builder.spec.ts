import { MetaPopulation } from '@allors/workspace-system-meta';
import { LazyMetaPopulation } from '@allors/workspace-system-meta-json';
import { M } from '@allors/workspace-base-meta';
import { data } from '@allors/workspace-base-meta-json';

describe('TreeBuilder', () => {
  const metaPopulation = new LazyMetaPopulation(data) as MetaPopulation;
  const m = metaPopulation as M;
  const { treeBuilder: t } = m;

  describe('with metadata', () => {
    it('should return nodes', () => {
      const tree = t.Organization({
        Owner: {
          OrganizationsWhereOwner: {},
        },
      });

      expect(tree).toBeDefined();
      expect(tree.length).toBe(1);

      const node = tree[0];

      expect(node.relationEndType).toBe(m.Organization.Owner);
      expect(node.nodes.length).toBe(1);

      const subnode = node.nodes[0];
      expect(subnode.relationEndType).toBe(m.Person.OrganizationsWhereOwner);
      expect(subnode.nodes).toBeUndefined();
    });
  });
});
