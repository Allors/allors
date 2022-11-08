import { MetaPopulation } from '@allors/system/workspace/meta';

export class LazyTreeBuilder {
  constructor(metaPopulation: MetaPopulation) {
    for (const composite of metaPopulation.composites) {
      this[composite.singularName] = (obj) => {
        if (Array.isArray(obj)) {
          return obj;
        }

        const entries = Object.entries(obj);
        return entries.length > 0
          ? entries.map(([key, value]) => {
              const relationEndType =
                composite.relationEndTypeByPropertyName.get(key);
              return value != null
                ? {
                    relationEndType,
                    ofType: this['ofType'],
                    nodes: this[relationEndType.objectType.singularName](value),
                  }
                : {
                    relationEndType,
                    ofType: this['ofType'],
                  };
            })
          : undefined;
      };
    }
  }
}
