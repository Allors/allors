import { Injectable } from '@angular/core';
import { IObject } from '@allors/workspace/system/domain';
import { Composite, ObjectType } from '@allors/workspace/system/meta';

@Injectable()
export abstract class NavigationService {
  abstract hasList(objectType: Composite): boolean;

  abstract listUrl(objectType: Composite): string;

  abstract list(objectType: Composite): void;

  abstract hasOverview(obj: IObject): boolean;

  abstract overviewUrl(objectType: Composite): string;

  abstract overview(obj: IObject): void;
}
