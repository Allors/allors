import { MetaPopulation } from '@allors/workspace/system/meta';
import { LazyMetaPopulation } from '@allors/workspace/system/meta-json';

describe('MetaPopulation', () => {
  describe('default constructor', () => {
    const metaPopulation = new LazyMetaPopulation({}) as MetaPopulation;

    it('should be newable', () => {
      expect(metaPopulation).toBeDefined();
    });
  });
});
