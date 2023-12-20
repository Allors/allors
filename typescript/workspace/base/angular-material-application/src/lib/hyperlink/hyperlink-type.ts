import { Node, Path } from '@allors/workspace/system/domain';

export interface HyperlinkType {
  paths: Path[];
  tree: Node[];
  label: string;
}
