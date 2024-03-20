import {
  MenuItem,
  MenuService,
} from '@allors/workspace-base-angular-application';
import { WorkspaceService } from '@allors/workspace-base-angular-foundation';
import { M } from '@allors/workspace-default-meta';
import { Injectable } from '@angular/core';

@Injectable()
export class AppMenuService implements MenuService {
  private _menu: MenuItem[];

  constructor(workspaceService: WorkspaceService) {
    const m = workspaceService.workspace.configuration.metaPopulation as M;
    this._menu = [
      { title: 'Home', icon: 'home', link: '/' },
      {
        title: 'Contacts',
        icon: 'business',
        children: [
          { objectType: m.Person },
          { objectType: m.Organization },
          { objectType: m.Country },
        ],
      },
      { title: 'Fields', icon: 'build', link: '/fields' },
    ];
  }

  menu(): MenuItem[] {
    return this._menu;
  }
}
