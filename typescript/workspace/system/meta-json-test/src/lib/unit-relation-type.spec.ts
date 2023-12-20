import {
  Composite,
  Unit,
  RoleType,
  Interface,
  UnitTags,
  Multiplicity,
} from '@allors/workspace/system/meta';
import { LazyMetaPopulation } from '@allors/workspace/system/meta/json';

describe('Unit Relation in MetaPopulation', () => {
  describe('with minimal unit relation metadata', () => {
    interface Organization extends Composite {
      Name: RoleType;
    }

    interface M extends LazyMetaPopulation {
      String: Unit;

      Organization: Organization;
    }

    const metaPopulation = new LazyMetaPopulation({
      c: [['10', 'Organization', [], [['11', UnitTags.String, 'Name']]]],
    }) as M;

    const { Organization, String } = metaPopulation;
    const { Name: roleType } = Organization;

    it('should have the relation with its defaults', () => {
      expect(roleType).toBeDefined();
      expect(roleType.objectType).toBe(String);
      expect(roleType.isOne).toBeTruthy();
      expect(roleType.isMany).toBeFalsy();
      expect(roleType.singularName).toBe('Name');
      expect(roleType.pluralName).toBe('Names');
      expect(roleType.name).toBe('Name');
      expect(roleType.isRequired).toBeFalsy();
      expect(roleType.size).toBeUndefined();
      expect(roleType.scale).toBeUndefined();
      expect(roleType.precision).toBeUndefined();
      expect(roleType.mediaType).toBeUndefined();

      const { relationType, associationType } = roleType;

      expect(relationType).toBeDefined;
      expect(relationType.multiplicity).toBe(Multiplicity.OneToOne);
      expect(relationType.isDerived).toBeFalsy();
      expect(relationType).not.toBeNull();

      expect(associationType).toBeDefined;
      expect(associationType.objectType).toBe(Organization);
      expect(associationType.isOne).toBeTruthy();
      expect(associationType.isMany).toBeFalsy();
    });
  });

  describe('with maximal unit relation metadata', () => {
    interface Organization extends Composite {
      Name: RoleType;
    }

    interface M extends LazyMetaPopulation {
      String: Unit;

      Organization: Organization;
    }

    const metaPopulation = new LazyMetaPopulation({
      c: [
        [
          '10',
          'Organization',
          [],
          [['11', UnitTags.String, 'Name', 'Names', 1024]],
        ],
      ],
      m: [['11']],
      d: ['11'],
      r: ['11'],
      t: { 'application/pdf': ['11'] },
    }) as M;

    const { Organization, String } = metaPopulation;
    const { Name: roleType } = Organization;

    it('should have the relation with its defaults', () => {
      expect(roleType).toBeDefined();
      expect(roleType.objectType).toBe(String);
      expect(roleType.isOne).toBeTruthy(); // Always for Unit
      expect(roleType.isMany).toBeFalsy();
      expect(roleType.singularName).toBe('Name');
      expect(roleType.pluralName).toBe('Names');
      expect(roleType.name).toBe('Name');
      expect(roleType.isRequired).toBeTruthy();
      expect(roleType.size).toBeDefined();
      expect(roleType.size).toBe(1024);
      expect(roleType.scale).toBeUndefined();
      expect(roleType.precision).toBeUndefined();
      expect(roleType.mediaType).toBeDefined();
      expect(roleType.mediaType).toBe('application/pdf');

      const { relationType, associationType } = roleType;

      expect(relationType).toBeDefined;
      expect(relationType.multiplicity).toBe(Multiplicity.OneToOne);
      expect(relationType.isDerived).toBeTruthy();
      expect(relationType).not.toBeNull();

      expect(associationType).toBeDefined;
      expect(associationType.objectType).toBe(Organization);
      expect(associationType.isOne).toBeTruthy(); // Always for Unit
      expect(associationType.isMany).toBeFalsy();
    });
  });

  describe('with decimal relation no names scale metadata', () => {
    interface Organization extends Composite {
      Decimal: RoleType;
    }

    interface M extends LazyMetaPopulation {
      Decimal: Unit;

      Organization: Organization;
    }

    const metaPopulation = new LazyMetaPopulation({
      c: [['10', 'Organization', [], [['11', UnitTags.Decimal, 21]]]],
    }) as M;

    const { Organization, Decimal } = metaPopulation;
    const { Decimal: roleType } = Organization;

    it('should have the relation with its values', () => {
      expect(roleType).toBeDefined();
      expect(roleType.objectType).toBe(Decimal);
      expect(roleType.isOne).toBeTruthy();
      expect(roleType.isMany).toBeFalsy();
      expect(roleType.singularName).toBe('Decimal');
      expect(roleType.pluralName).toBe('Decimals');
      expect(roleType.name).toBe('Decimal');
      expect(roleType.isRequired).toBeFalsy();
      expect(roleType.size).toBeUndefined();
      expect(roleType.scale).toBe(21);
      expect(roleType.precision).toBeUndefined();
      expect(roleType.mediaType).toBeUndefined();

      const { relationType, associationType } = roleType;

      expect(relationType).toBeDefined;
      expect(relationType.multiplicity).toBe(Multiplicity.OneToOne);
      expect(relationType.isDerived).toBeFalsy();
      expect(relationType).not.toBeNull();

      expect(associationType).toBeDefined;
      expect(associationType.objectType).toBe(Organization);
      expect(associationType.isOne).toBeTruthy();
      expect(associationType.isMany).toBeFalsy();
    });
  });

  describe('with decimal relation no names scale and precision metadata', () => {
    interface Organization extends Composite {
      Decimal: RoleType;
    }

    interface M extends LazyMetaPopulation {
      Decimal: Unit;

      Organization: Organization;
    }

    const metaPopulation = new LazyMetaPopulation({
      c: [['10', 'Organization', [], [['11', UnitTags.Decimal, 21, 3]]]],
    }) as M;

    const { Organization, Decimal } = metaPopulation;
    const { Decimal: roleType } = Organization;

    it('should have the relation with its values', () => {
      expect(roleType).toBeDefined();
      expect(roleType.objectType).toBe(Decimal);
      expect(roleType.isOne).toBeTruthy();
      expect(roleType.isMany).toBeFalsy();
      expect(roleType.singularName).toBe('Decimal');
      expect(roleType.pluralName).toBe('Decimals');
      expect(roleType.name).toBe('Decimal');
      expect(roleType.isRequired).toBeFalsy();
      expect(roleType.size).toBeUndefined();
      expect(roleType.scale).toBe(21);
      expect(roleType.precision).toBe(3);
      expect(roleType.mediaType).toBeUndefined();

      const { relationType, associationType } = roleType;

      expect(relationType).toBeDefined;
      expect(relationType.multiplicity).toBe(Multiplicity.OneToOne);
      expect(relationType.isDerived).toBeFalsy();
      expect(relationType).not.toBeNull();

      expect(associationType).toBeDefined;
      expect(associationType.objectType).toBe(Organization);
      expect(associationType.isOne).toBeTruthy();
      expect(associationType.isMany).toBeFalsy();
    });
  });

  describe('with decimal relation singularName scale metadata', () => {
    interface Organization extends Composite {
      Balance: RoleType;
    }

    interface M extends LazyMetaPopulation {
      Decimal: Unit;

      Organization: Organization;
    }

    const metaPopulation = new LazyMetaPopulation({
      c: [
        ['10', 'Organization', [], [['11', UnitTags.Decimal, 'Balance', 25]]],
      ],
    }) as M;

    const { Organization, Decimal } = metaPopulation;
    const { Balance: roleType } = Organization;

    it('should have the relation with its values', () => {
      expect(roleType).toBeDefined();
      expect(roleType.objectType).toBe(Decimal);
      expect(roleType.isOne).toBeTruthy();
      expect(roleType.isMany).toBeFalsy();
      expect(roleType.singularName).toBe('Balance');
      expect(roleType.pluralName).toBe('Balances');
      expect(roleType.name).toBe('Balance');
      expect(roleType.isRequired).toBeFalsy();
      expect(roleType.size).toBeUndefined();
      expect(roleType.scale).toBe(25);
      expect(roleType.precision).toBeUndefined();
      expect(roleType.mediaType).toBeUndefined();

      const { relationType, associationType } = roleType;

      expect(relationType).toBeDefined;
      expect(relationType.multiplicity).toBe(Multiplicity.OneToOne);
      expect(relationType.isDerived).toBeFalsy();
      expect(relationType).not.toBeNull();

      expect(associationType).toBeDefined;
      expect(associationType.objectType).toBe(Organization);
      expect(associationType.isOne).toBeTruthy();
      expect(associationType.isMany).toBeFalsy();
    });
  });

  describe('with decimal relation singularName scale and precision metadata', () => {
    interface Organization extends Composite {
      Balance: RoleType;
    }

    interface M extends LazyMetaPopulation {
      Decimal: Unit;

      Organization: Organization;
    }

    const metaPopulation = new LazyMetaPopulation({
      c: [
        [
          '10',
          'Organization',
          [],
          [['11', UnitTags.Decimal, 'Balance', 25, 2]],
        ],
      ],
    }) as M;

    const { Organization, Decimal } = metaPopulation;
    const { Balance: roleType } = Organization;

    it('should have the relation with its values', () => {
      expect(roleType).toBeDefined();
      expect(roleType.objectType).toBe(Decimal);
      expect(roleType.isOne).toBeTruthy();
      expect(roleType.isMany).toBeFalsy();
      expect(roleType.singularName).toBe('Balance');
      expect(roleType.pluralName).toBe('Balances');
      expect(roleType.name).toBe('Balance');
      expect(roleType.isRequired).toBeFalsy();
      expect(roleType.size).toBeUndefined();
      expect(roleType.scale).toBe(25);
      expect(roleType.precision).toBe(2);
      expect(roleType.mediaType).toBeUndefined();

      const { relationType, associationType } = roleType;

      expect(relationType).toBeDefined;
      expect(relationType.multiplicity).toBe(Multiplicity.OneToOne);
      expect(relationType.isDerived).toBeFalsy();
      expect(relationType).not.toBeNull();

      expect(associationType).toBeDefined;
      expect(associationType.objectType).toBe(Organization);
      expect(associationType.isOne).toBeTruthy();
      expect(associationType.isMany).toBeFalsy();
    });
  });

  describe('with decimal relation singularName and pluralName scale metadata', () => {
    interface Organization extends Composite {
      Balance: RoleType;
    }

    interface M extends LazyMetaPopulation {
      Decimal: Unit;

      Organization: Organization;
    }

    const metaPopulation = new LazyMetaPopulation({
      c: [
        [
          '10',
          'Organization',
          [],
          [['11', UnitTags.Decimal, 'Balance', 'PluralBalance', 26]],
        ],
      ],
    }) as M;

    const { Organization, Decimal } = metaPopulation;
    const { Balance: roleType } = Organization;

    it('should have the relation with its values', () => {
      expect(roleType).toBeDefined();
      expect(roleType.objectType).toBe(Decimal);
      expect(roleType.isOne).toBeTruthy();
      expect(roleType.isMany).toBeFalsy();
      expect(roleType.singularName).toBe('Balance');
      expect(roleType.pluralName).toBe('PluralBalance');
      expect(roleType.name).toBe('Balance');
      expect(roleType.isRequired).toBeFalsy();
      expect(roleType.size).toBeUndefined();
      expect(roleType.scale).toBe(26);
      expect(roleType.precision).toBeUndefined();
      expect(roleType.mediaType).toBeUndefined();

      const { relationType, associationType } = roleType;

      expect(relationType).toBeDefined;
      expect(relationType.multiplicity).toBe(Multiplicity.OneToOne);
      expect(relationType.isDerived).toBeFalsy();
      expect(relationType).not.toBeNull();

      expect(associationType).toBeDefined;
      expect(associationType.objectType).toBe(Organization);
      expect(associationType.isOne).toBeTruthy();
      expect(associationType.isMany).toBeFalsy();
    });
  });

  describe('with decimal relation singularName and pluralName scale and precision metadata', () => {
    interface Organization extends Composite {
      Balance: RoleType;
    }

    interface M extends LazyMetaPopulation {
      Decimal: Unit;

      Organization: Organization;
    }

    const metaPopulation = new LazyMetaPopulation({
      c: [
        [
          '10',
          'Organization',
          [],
          [['11', UnitTags.Decimal, 'Balance', 'PluralBalance', 26, 5]],
        ],
      ],
    }) as M;

    const { Organization, Decimal } = metaPopulation;
    const { Balance: roleType } = Organization;

    it('should have the relation with its values', () => {
      expect(roleType).toBeDefined();
      expect(roleType.objectType).toBe(Decimal);
      expect(roleType.isOne).toBeTruthy();
      expect(roleType.isMany).toBeFalsy();
      expect(roleType.singularName).toBe('Balance');
      expect(roleType.pluralName).toBe('PluralBalance');
      expect(roleType.name).toBe('Balance');
      expect(roleType.isRequired).toBeFalsy();
      expect(roleType.size).toBeUndefined();
      expect(roleType.scale).toBe(26);
      expect(roleType.precision).toBe(5);
      expect(roleType.mediaType).toBeUndefined();

      const { relationType, associationType } = roleType;

      expect(relationType).toBeDefined;
      expect(relationType.multiplicity).toBe(Multiplicity.OneToOne);
      expect(relationType.isDerived).toBeFalsy();
      expect(relationType).not.toBeNull();

      expect(associationType).toBeDefined;
      expect(associationType.objectType).toBe(Organization);
      expect(associationType.isOne).toBeTruthy();
      expect(associationType.isMany).toBeFalsy();
    });
  });

  describe('with string relation no names metadata', () => {
    interface Organization extends Composite {
      String: RoleType;
    }

    interface M extends LazyMetaPopulation {
      String: Unit;

      Organization: Organization;
    }

    const metaPopulation = new LazyMetaPopulation({
      c: [['10', 'Organization', [], [['11', UnitTags.String, 256]]]],
    }) as M;

    const { Organization, String } = metaPopulation;
    const { String: roleType } = Organization;

    it('should have the relation with its values', () => {
      expect(roleType).toBeDefined();
      expect(roleType.objectType).toBe(String);
      expect(roleType.isOne).toBeTruthy();
      expect(roleType.isMany).toBeFalsy();
      expect(roleType.singularName).toBe('String');
      expect(roleType.pluralName).toBe('Strings');
      expect(roleType.name).toBe('String');
      expect(roleType.isRequired).toBeFalsy();
      expect(roleType.size).toBe(256);
      expect(roleType.scale).toBeUndefined();
      expect(roleType.precision).toBeUndefined();
      expect(roleType.mediaType).toBeUndefined();

      const { relationType, associationType } = roleType;

      expect(relationType).toBeDefined;
      expect(relationType.multiplicity).toBe(Multiplicity.OneToOne);
      expect(relationType.isDerived).toBeFalsy();
      expect(relationType).not.toBeNull();

      expect(associationType).toBeDefined;
      expect(associationType.objectType).toBe(Organization);
      expect(associationType.isOne).toBeTruthy();
      expect(associationType.isMany).toBeFalsy();
    });
  });

  describe('with string relation no names precision overflow metadata', () => {
    interface Organization extends Composite {
      String: RoleType;
    }

    interface M extends LazyMetaPopulation {
      String: Unit;

      Organization: Organization;
    }

    const metaPopulation = new LazyMetaPopulation({
      c: [['10', 'Organization', [], [['11', UnitTags.String, 256, 5]]]],
    }) as M;

    const { Organization, String } = metaPopulation;
    const { String: roleType } = Organization;

    it('should have the relation with its values', () => {
      expect(roleType).toBeDefined();
      expect(roleType.objectType).toBe(String);
      expect(roleType.isOne).toBeTruthy();
      expect(roleType.isMany).toBeFalsy();
      expect(roleType.singularName).toBe('String');
      expect(roleType.pluralName).toBe('Strings');
      expect(roleType.name).toBe('String');
      expect(roleType.isRequired).toBeFalsy();
      expect(roleType.size).toBe(256);
      expect(roleType.scale).toBeUndefined();
      expect(roleType.precision).toBeUndefined();
      expect(roleType.mediaType).toBeUndefined();

      const { relationType, associationType } = roleType;

      expect(relationType).toBeDefined;
      expect(relationType.multiplicity).toBe(Multiplicity.OneToOne);
      expect(relationType.isDerived).toBeFalsy();
      expect(relationType).not.toBeNull();

      expect(associationType).toBeDefined;
      expect(associationType.objectType).toBe(Organization);
      expect(associationType.isOne).toBeTruthy();
      expect(associationType.isMany).toBeFalsy();
    });
  });

  describe('with string relation singularName metadata', () => {
    interface Organization extends Composite {
      Text: RoleType;
    }

    interface M extends LazyMetaPopulation {
      String: Unit;

      Organization: Organization;
    }

    const metaPopulation = new LazyMetaPopulation({
      c: [['10', 'Organization', [], [['11', UnitTags.String, 'Text', 512]]]],
    }) as M;

    const { Organization, String } = metaPopulation;
    const { Text: roleType } = Organization;

    it('should have the relation with its values', () => {
      expect(roleType).toBeDefined();
      expect(roleType.objectType).toBe(String);
      expect(roleType.isOne).toBeTruthy();
      expect(roleType.isMany).toBeFalsy();
      expect(roleType.singularName).toBe('Text');
      expect(roleType.pluralName).toBe('Texts');
      expect(roleType.name).toBe('Text');
      expect(roleType.isRequired).toBeFalsy();
      expect(roleType.size).toBe(512);
      expect(roleType.scale).toBeUndefined();
      expect(roleType.precision).toBeUndefined();
      expect(roleType.mediaType).toBeUndefined();

      const { relationType, associationType } = roleType;

      expect(relationType).toBeDefined;
      expect(relationType.multiplicity).toBe(Multiplicity.OneToOne);
      expect(relationType.isDerived).toBeFalsy();
      expect(relationType).not.toBeNull();

      expect(associationType).toBeDefined;
      expect(associationType.objectType).toBe(Organization);
      expect(associationType.isOne).toBeTruthy();
      expect(associationType.isMany).toBeFalsy();
    });
  });

  describe('with string relation singularName precision overflow metadata', () => {
    interface Organization extends Composite {
      Text: RoleType;
    }

    interface M extends LazyMetaPopulation {
      String: Unit;

      Organization: Organization;
    }

    const metaPopulation = new LazyMetaPopulation({
      c: [
        ['10', 'Organization', [], [['11', UnitTags.String, 'Text', 512, 10]]],
      ],
    }) as M;

    const { Organization, String } = metaPopulation;
    const { Text: roleType } = Organization;

    it('should have the relation with its values', () => {
      expect(roleType).toBeDefined();
      expect(roleType.objectType).toBe(String);
      expect(roleType.isOne).toBeTruthy();
      expect(roleType.isMany).toBeFalsy();
      expect(roleType.singularName).toBe('Text');
      expect(roleType.pluralName).toBe('Texts');
      expect(roleType.name).toBe('Text');
      expect(roleType.isRequired).toBeFalsy();
      expect(roleType.size).toBe(512);
      expect(roleType.scale).toBeUndefined();
      expect(roleType.precision).toBeUndefined();
      expect(roleType.mediaType).toBeUndefined();

      const { relationType, associationType } = roleType;

      expect(relationType).toBeDefined;
      expect(relationType.multiplicity).toBe(Multiplicity.OneToOne);
      expect(relationType.isDerived).toBeFalsy();
      expect(relationType).not.toBeNull();

      expect(associationType).toBeDefined;
      expect(associationType.objectType).toBe(Organization);
      expect(associationType.isOne).toBeTruthy();
      expect(associationType.isMany).toBeFalsy();
    });
  });

  describe('with string relation singularName and pluralName metadata', () => {
    interface Organization extends Composite {
      Text: RoleType;
    }

    interface M extends LazyMetaPopulation {
      String: Unit;

      Organization: Organization;
    }

    const metaPopulation = new LazyMetaPopulation({
      c: [
        [
          '10',
          'Organization',
          [],
          [['11', UnitTags.String, 'Text', 'PluralText', 512]],
        ],
      ],
    }) as M;

    const { Organization, String } = metaPopulation;
    const { Text: roleType } = Organization;

    it('should have the relation with its values', () => {
      expect(roleType).toBeDefined();
      expect(roleType.objectType).toBe(String);
      expect(roleType.isOne).toBeTruthy();
      expect(roleType.isMany).toBeFalsy();
      expect(roleType.singularName).toBe('Text');
      expect(roleType.pluralName).toBe('PluralText');
      expect(roleType.name).toBe('Text');
      expect(roleType.isRequired).toBeFalsy();
      expect(roleType.size).toBe(512);
      expect(roleType.scale).toBeUndefined();
      expect(roleType.precision).toBeUndefined();
      expect(roleType.mediaType).toBeUndefined();

      const { relationType, associationType } = roleType;

      expect(relationType).toBeDefined;
      expect(relationType.multiplicity).toBe(Multiplicity.OneToOne);
      expect(relationType.isDerived).toBeFalsy();
      expect(relationType).not.toBeNull();

      expect(associationType).toBeDefined;
      expect(associationType.objectType).toBe(Organization);
      expect(associationType.isOne).toBeTruthy();
      expect(associationType.isMany).toBeFalsy();
    });
  });

  describe('with string relation singularName and pluralName precision overflow metadata', () => {
    interface Organization extends Composite {
      Text: RoleType;
    }

    interface M extends LazyMetaPopulation {
      String: Unit;

      Organization: Organization;
    }

    const metaPopulation = new LazyMetaPopulation({
      c: [
        [
          '10',
          'Organization',
          [],
          [['11', UnitTags.String, 'Text', 'PluralText', 512, 1]],
        ],
      ],
    }) as M;

    const { Organization, String } = metaPopulation;
    const { Text: roleType } = Organization;

    it('should have the relation with its values', () => {
      expect(roleType).toBeDefined();
      expect(roleType.objectType).toBe(String);
      expect(roleType.isOne).toBeTruthy();
      expect(roleType.isMany).toBeFalsy();
      expect(roleType.singularName).toBe('Text');
      expect(roleType.pluralName).toBe('PluralText');
      expect(roleType.name).toBe('Text');
      expect(roleType.isRequired).toBeFalsy();
      expect(roleType.size).toBe(512);
      expect(roleType.scale).toBeUndefined();
      expect(roleType.precision).toBeUndefined();
      expect(roleType.mediaType).toBeUndefined();

      const { relationType, associationType } = roleType;

      expect(relationType).toBeDefined;
      expect(relationType.multiplicity).toBe(Multiplicity.OneToOne);
      expect(relationType.isDerived).toBeFalsy();
      expect(relationType).not.toBeNull();

      expect(associationType).toBeDefined;
      expect(associationType.objectType).toBe(Organization);
      expect(associationType.isOne).toBeTruthy();
      expect(associationType.isMany).toBeFalsy();
    });
  });

  describe('with inherited unit relation metadata', () => {
    interface Named extends Interface {
      Name: RoleType;
    }

    interface Organization extends Composite {
      Name: RoleType;
    }

    interface M extends LazyMetaPopulation {
      String: Unit;

      Named: Named;

      Organization: Organization;
    }

    const metaPopulation = new LazyMetaPopulation({
      i: [['9', 'Named', [], [['11', UnitTags.String, 'Name']]]],
      c: [['10', 'Organization', ['9']]],
    }) as M;

    const { Named, Organization } = metaPopulation;
    const { Name: namedRoleType } = Named;
    const { Name: organizationRoleType } = Organization;

    it('should have the same RoleType', () => {
      expect(namedRoleType).toBeDefined();
      expect(organizationRoleType).toBeDefined();
      expect(organizationRoleType).toBe(namedRoleType);
    });
  });
});
