import { Media } from '@allors/workspace-default-domain';

export function isImage(media: Media): boolean {
  const type = media.Type || media.InType;
  return type?.indexOf('image') === 0;
}
