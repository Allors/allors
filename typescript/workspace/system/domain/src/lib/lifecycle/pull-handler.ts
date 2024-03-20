import { IPullResult, Pull } from '@allors/workspace-system-domain';

export interface PullHandler {
  onPrePull(pulls: Pull[]): void;

  onPostPull(pullResult: IPullResult): void;
}
