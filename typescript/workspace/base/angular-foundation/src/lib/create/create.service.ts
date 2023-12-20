import { Observable } from 'rxjs';
import { Composite } from '@allors/workspace/system/meta';
import { IObject } from '@allors/workspace/system/domain';
import { Injectable } from '@angular/core';
import { CreateRequest } from './create-request';

@Injectable()
export abstract class CreateService {
  abstract canCreate(objectType: Composite): boolean;

  abstract create(request: CreateRequest): Observable<IObject>;
}
