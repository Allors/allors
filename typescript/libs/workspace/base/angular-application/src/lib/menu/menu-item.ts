import { Composite } from '@allors/workspace/system/meta';

export interface MenuItem {
  objectType?: Composite;
  link?: string;
  title?: string;
  icon?: string;
  children?: MenuItem[];
}
