import {
  SharedPullService,
  RefreshService,
  MetaService,
} from '@allors/workspace/base/angular-foundation';
import { Directive, Input } from '@angular/core';
import { ScopedService } from '../../scoped/scoped.service';
import { PanelService } from '../../panel/panel.service';
import { AllorsCustomExtentPanelComponent } from './extent-panel.component';
import { Composite } from '@allors/workspace/system/meta';

@Directive()
export abstract class AllorsCustomEditExtentPanelComponent extends AllorsCustomExtentPanelComponent {
  override dataAllorsKind = 'edit-extent-panel';

  readonly panelMode = 'Edit';

  @Input()
  factory: Composite;

  constructor(
    itemPageService: ScopedService,
    panelService: PanelService,
    sharedPullService: SharedPullService,
    refreshService: RefreshService
  ) {
    super(itemPageService, panelService, sharedPullService, refreshService);
  }
}
