import { Injectable } from '@angular/core';
import { Composite } from '@allors/workspace-system-meta';
import { HyperlinkType } from './hyperlink-type';

@Injectable()
export abstract class HyperlinkService {
  abstract linkTypes(objectType: Composite): HyperlinkType[];
}
