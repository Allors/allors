import { MetaPopulation } from '@allors/workspace/system/meta';
import { IObjectFactory } from './iobject-factory';

export interface Configuration {
  name: string;

  metaPopulation: MetaPopulation;

  objectFactory: IObjectFactory;

  idGenerator(): number;
}
