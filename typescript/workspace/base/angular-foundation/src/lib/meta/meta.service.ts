import { Composite, RelationEndType } from '@allors/workspace-system-meta';
import { Injectable } from '@angular/core';

@Injectable()
export abstract class MetaService {
  abstract singularName(metaObject: Composite | RelationEndType): string;

  abstract pluralName(metaObject: Composite | RelationEndType): string;
}
