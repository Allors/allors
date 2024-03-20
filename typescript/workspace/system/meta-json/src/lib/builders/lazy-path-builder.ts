import { Path } from '@allors/workspace-system-domain';
import { Composite, MetaPopulation } from '@allors/workspace-system-meta';

export class LazyPathBuilder {
  constructor(metaPopulation: MetaPopulation) {
    for (const composite of metaPopulation.composites) {
      this[composite.singularName] = (obj) => {
        const entry = Object.entries(obj).find(([k]) => k !== 'ofType');

        if (!entry) {
          return null;
        }

        const [key, value] = entry;
        const relationEndType =
          composite.relationEndTypeByPropertyName.get(key);
        const builder = this[relationEndType.objectType.singularName];

        const path: Path = {
          relationEndType,
        };

        const ofTypeEntry = Object.entries(obj).find(([k]) => k === 'ofType');
        if (ofTypeEntry) {
          const [, ofType] = ofTypeEntry;
          path.ofType = ofType as Composite;
        }

        const next = builder(value);
        if (next) {
          path.next = next;
        }

        return path;
      };
    }
  }
}
