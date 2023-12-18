import { MetaPopulation } from '@allors/system/workspace/meta';
import { LazyMetaPopulation } from '@allors/system/workspace/meta-json';
import { M } from '@allors/base/workspace/meta';
import { data } from '@allors/base/workspace/meta-json';
import { Filter } from '@allors/system/workspace/domain';

describe('Pulls', () => {
  const metaPopulation = new LazyMetaPopulation(data) as MetaPopulation;
  const m = metaPopulation as M;
  const { pullBuilder: p } = m;

  describe('with metadata', () => {
    it('should return pulls', () => {
      const pull = p.Organization({
        select: {
          Owner: {
            include: {
              OrganizationsWhereOwner: {},
            },
          },
        },
        skip: 20,
        take: 10,
      });

      expect(pull).toBeDefined();
      expect(pull.extent).toBeDefined();
      expect(pull.extent.kind).toBe('Filter');

      const extent = pull.extent as Filter;

      expect(extent.objectType).toBe(m.Organization);

      expect(pull.results).toBeDefined();
      expect(pull.results.length).toBe(1);

      const result = pull.results[0];

      expect(result.name).toBeUndefined();
      expect(result.select).toBeDefined();
      expect(result.selectRef).toBeUndefined();
      expect(result.skip).toBe(20);
      expect(result.take).toBe(10);

      const select = result.select;

      expect(select.include).toBeDefined();
      expect(select.next).toBeUndefined();
      expect(select.relationEndType).toBe(m.Organization.Owner);

      const include = select.include;

      expect(include.length).toBe(1);

      const node = include[0];

      expect(node.nodes).toBeUndefined();
      expect(node.ofType).toBeUndefined();
      expect(node.relationEndType).toBe(m.Person.OrganizationsWhereOwner);
    });
  });
});
