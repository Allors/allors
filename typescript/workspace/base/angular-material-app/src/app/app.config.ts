import { HttpClient } from '@angular/common/http';
import { WorkspaceService } from '@allors/workspace/base/angular-foundation';

import { AppClient } from './app.client';
import { Configuration } from '@allors/workspace/system/domain';
import { LazyMetaPopulation } from '@allors/workspace/system/meta-json';
import { PrototypeObjectFactory } from '@allors/workspace/system/adapters';
import { DatabaseConnection } from '@allors/workspace/system/adapters-json';
import { data } from '@allors/workspace/default/meta-json';
import { M } from '@allors/workspace/default/meta';
import { AppContext } from './app.context';

export function config(
  workspaceService: WorkspaceService,
  httpClient: HttpClient,
  baseUrl: string,
  authUrl: string
) {
  const angularClient = new AppClient(httpClient, baseUrl, authUrl);

  const metaPopulation = new LazyMetaPopulation(data);
  const m = metaPopulation as unknown as M;

  let nextId = -1;

  const configuration: Configuration = {
    name: 'Default',
    metaPopulation,
    objectFactory: new PrototypeObjectFactory(metaPopulation),
    idGenerator: () => nextId--,
  };

  const database = new DatabaseConnection(configuration, angularClient);
  const workspace = database.createWorkspace();
  workspaceService.workspace = workspace;

  workspaceService.contextBuilder = () => new AppContext(workspaceService);
}
