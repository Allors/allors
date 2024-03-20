import {
  Composite,
  Interface,
  MethodType,
} from '@allors/workspace-system-meta';
import { LazyMetaPopulation } from '@allors/workspace-system-meta-json';

interface Action extends Interface {
  Do: MethodType;
}

interface Organization extends Composite {
  Do: MethodType;
}

interface M extends LazyMetaPopulation {
  Action: Action;

  Organization: Organization;
}

describe('MethodType in MetaPopulation', () => {
  describe('with minimal method metadata', () => {
    const metaPopulation = new LazyMetaPopulation({
      c: [['10', 'Organization', [], [], [['11', 'Do']]]],
    }) as M;

    const { Organization } = metaPopulation;
    const { Do: methodType } = Organization;

    it('should have the relation with its defaults', () => {
      expect(methodType).toBeDefined();
      expect(methodType.objectType).toBe(Organization);
      expect(methodType.name).toBe('Do');
    });
  });

  describe('with inherited method metadata', () => {
    const metaPopulation = new LazyMetaPopulation({
      i: [['9', 'Action', [], [], [['11', 'Do']]]],
      c: [['10', 'Organization', ['9']]],
    }) as M;

    const { Action, Organization } = metaPopulation;
    const { Do: actionDo } = Action;
    const { Do: organizationDo } = Organization;

    it('should have the same RoleType', () => {
      expect(actionDo).toBeDefined();
      expect(organizationDo).toBeDefined();
      expect(organizationDo).toBe(actionDo);
    });
  });
});
