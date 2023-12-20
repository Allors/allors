import { Injectable } from '@angular/core';
import { Action } from '@allors/workspace/base/angular-foundation';
import { NavigationService } from '@allors/workspace/base/angular-application';
import { OverviewAction } from './overview-action';

@Injectable({
  providedIn: 'root',
})
export class OverviewActionService {
  constructor(private navigation: NavigationService) {}

  overview(): OverviewAction {
    return new OverviewAction(this.navigation);
  }
}
