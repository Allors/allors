import { IObject } from '@allors/workspace/system/domain';
import { HyperlinkType } from './hyperlink-type';

export interface Hyperlink {
  linkType: HyperlinkType;
  target: IObject;
  icon: string;
  name: string;
  description: string;
}
