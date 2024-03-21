import { Composite, RoleType } from '@allors/workspace-system-meta';
import { Injectable } from '@angular/core';

@Injectable()
export abstract class IconService {
  abstract icon(meta: Composite | RoleType): string;
}
