import { Injectable } from '@angular/core';
import { Composite } from '@allors/workspace/system/meta';
import { Node, toPaths } from '@allors/workspace/system/domain';
import { M } from '@allors/workspace/default/meta';
import { WorkspaceService } from '@allors/workspace/base/angular-foundation';
import {
  HyperlinkService,
  HyperlinkType,
} from '@allors/workspace/base/angular/material/application';

function create(tree: Node[], label?: string): HyperlinkType {
  return {
    label,
    tree,
    paths: toPaths(tree),
  };
}

@Injectable()
export class AppHyperlinkService implements HyperlinkService {
  linkTypesByObjectType: Map<Composite, HyperlinkType[]>;

  constructor(workspaceService: WorkspaceService) {
    const m = workspaceService.workspace.configuration.metaPopulation as M;
    const { treeBuilder: t } = m;

    this.linkTypesByObjectType = new Map<Composite, HyperlinkType[]>([
      [m.Organisation, [create(t.Organisation({ Address: {} }))]],
      [m.Person, [create(t.Person({ Address: {} }))]],
    ]);
  }

  linkTypes(objectType: Composite): HyperlinkType[] {
    return this.linkTypesByObjectType.get(objectType);
  }
}
