import {
  Configuration,
  IWorkspace,
  Operations,
} from '@allors/workspace/system/domain';
import { Class, OperandType } from '@allors/workspace/system/meta';

import { DefaultNumberRanges } from '../collections/ranges/default-number-ranges';
import { Ranges } from '../collections/ranges/ranges';
import { DatabaseRecord } from './database-record';

export type IdGenerator = () => number;

export abstract class DatabaseConnection {
  ranges: Ranges<number>;

  constructor(public configuration: Configuration) {
    this.ranges = new DefaultNumberRanges();
  }

  abstract createWorkspace(): IWorkspace;

  abstract getRecord(id: number): DatabaseRecord | undefined;

  abstract getPermission(
    cls: Class,
    operandType: OperandType,
    operation: Operations
  ): number | undefined;

  nextId(): number {
    return this.configuration.idGenerator();
  }
}
