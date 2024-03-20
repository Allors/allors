import { Observable } from 'rxjs';
import { IObject } from '@allors/workspace-system-domain';

export type ActionResult = boolean;

export type ActionTarget = IObject | IObject[];

export interface Action {
  name: string;
  displayName: (target: ActionTarget) => string;
  description: (target: ActionTarget) => string;
  disabled: (target: ActionTarget) => boolean;
  execute: (target: ActionTarget) => void;

  result: Observable<ActionResult>;
}
