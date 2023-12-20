import {
  DialogConfig,
  PromptType,
} from '@allors/workspace/base/angular-foundation';

export interface AllorsMaterialDialogData {
  alert?: boolean;
  confirmation?: boolean;
  prompt?: boolean;
  promptType?: PromptType;

  config: DialogConfig;
}
