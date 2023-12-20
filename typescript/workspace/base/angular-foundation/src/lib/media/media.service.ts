import { Injectable } from '@angular/core';
import { Media } from '@allors/workspace/default/domain';

@Injectable()
export abstract class MediaService {
  abstract url(media: Media): string;
}
