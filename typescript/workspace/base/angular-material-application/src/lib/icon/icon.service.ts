import { Composite, RelationType } from '@allors/workspace-system-meta';
import { Injectable } from '@angular/core';

@Injectable()
export abstract class IconService {
  abstract icon(meta: Composite | RelationType): string;
}
