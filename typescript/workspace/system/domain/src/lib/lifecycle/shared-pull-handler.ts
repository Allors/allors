import { IPullResult, Pull } from '@allors/workspace-system-domain';

export interface SharedPullHandler {
  onPreSharedPull(pulls: Pull[], prefix: string): void;

  onPostSharedPull(pullResult: IPullResult, prefix: string): void;
}
