import {Class } from '@allors/system/workspace/meta';
import { LazyMetaPopulation } from '@allors/system/workspace/meta-json';

describe('Class in MetaPopulation', () => {
  describe('with minimal class metadata', () => {
    type Organization = Class;

    interface M extends LazyMetaPopulation {
      Organization: Organization;
    }

    const metaPopulation = new LazyMetaPopulation({
      c: [['10', 'Organization']],
    }) as M;

    const { Organization } = metaPopulation;

    it('should have the class with its defaults', () => {
      expect(Organization).toBeDefined();
      expect(Organization.metaPopulation).toBe(metaPopulation);
      expect(Organization.tag).toBe('10');
      expect(Organization.singularName).toBe('Organization');
      expect(Organization.pluralName).toBe('Organizations');
      expect(Organization.isUnit).toBeFalsy();
      expect(Organization.isComposite).toBeTruthy();
      expect(Organization.isInterface).toBeFalsy();
      expect(Organization.isClass).toBeTruthy();
      expect(Organization.isRelationship).toBeFalsy();
    });
  });

  describe('with maximal class metadata', () => {
    type C1 = Class;
    type C2 = Class;

    interface M extends LazyMetaPopulation {
      C1: C1;
      C2: C2;
    }

    const metaPopulation = new LazyMetaPopulation({
      c: [
        ['10', 'C1', [], [], [], 'PluralC1'],
        ['11', 'C2', [], [], [], 'PluralC2'],
      ],
    }) as M;

    const { C1, C2 } = metaPopulation;

    it('should have the class with its defaults', () => {
      expect(C1).toBeDefined();
      expect(C1.metaPopulation).toBe(metaPopulation);
      expect(C1.tag).toBe('10');
      expect(C1.singularName).toBe('C1');
      expect(C1.pluralName).toBe('PluralC1');
      expect(C1.isUnit).toBeFalsy();
      expect(C1.isComposite).toBeTruthy();
      expect(C1.isInterface).toBeFalsy();
      expect(C1.isClass).toBeTruthy();
      expect(C1.isRelationship).toBeFalsy();

      expect(C2).toBeDefined();
      expect(C2.metaPopulation).toBe(metaPopulation);
      expect(C2.tag).toBe('11');
      expect(C2.singularName).toBe('C2');
      expect(C2.pluralName).toBe('PluralC2');
      expect(C2.isUnit).toBeFalsy();
      expect(C2.isComposite).toBeTruthy();
      expect(C2.isInterface).toBeFalsy();
      expect(C2.isClass).toBeTruthy();
      expect(C2.isRelationship).toBeFalsy();
    });
  });
});
