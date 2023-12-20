import { MetaPopulation } from '@allors/workspace/system/meta';
import { LazyMetaPopulation } from '@allors/workspace/system/meta-json';
import { M } from '@allors/workspace/base/meta';
import { data } from '@allors/workspace/base/meta-json';

describe('SelectBuilder', () => {
  const metaPopulation = new LazyMetaPopulation(data) as MetaPopulation;
  const m = metaPopulation as M;
  const { selectBuilder: s } = m;

  describe('with metadata', () => {
    it('should return selectBuilder', () => {
      const selection = s.Organization({
        Owner: {
          OrganizationsWhereOwner: {
            include: {
              Shareholders: {},
            },
          },
        },
      });

      expect(selection).toBeDefined();
      expect(selection.relationEndType).toBe(m.Organization.Owner);
      expect(selection.include).toBeUndefined();

      const next = selection.next;

      expect(next).toBeDefined();
      expect(next.next).toBeUndefined();
      expect(next.relationEndType).toBe(m.Person.OrganizationsWhereOwner);
      expect(next.include).toBeDefined();

      const include = next.include;

      expect(include.length).toBe(1);
      expect(include[0].relationEndType).toBe(m.Organization.Shareholders);
      expect(include[0].nodes).toBeUndefined();
    });
  });
});
