import { Input, Directive } from '@angular/core';
import { assert, humanize } from '@allors/system/workspace/meta';
import { M } from '@allors/default/workspace/meta';
import { Locale, LocalizedText } from '@allors/default/workspace/domain';

import { RoleField } from './role-field';

@Directive()
// tslint:disable-next-line: directive-class-suffix
export abstract class LocalizedRoleField extends RoleField {
  @Input()
  public locale: Locale;

  get localizedObject(): LocalizedText | null {
    if (this.locale) {
      const all: LocalizedText[] = this.model;
      if (all) {
        const filtered: LocalizedText[] = all.filter(
          (v: LocalizedText) => v.Locale === this.locale
        );
        return filtered?.[0] ?? null;
      }
    }

    return null;
  }

  get localizedText(): string | null {
    if (this.locale) {
      return this.localizedObject?.Text ?? null;
    }

    return null;
  }

  set localizedText(value: string | null) {
    if (this.locale) {
      if (!this.localizedObject) {
        const m = this.roleType.relationType.metaPopulation as M;
        const localizedText: LocalizedText =
          this.object.strategy.session.create<LocalizedText>(m.LocalizedText);
        localizedText.Locale = this.locale;
        this.object.strategy.addCompositesRole(this.roleType, localizedText);
      }

      assert(this.localizedObject);
      this.localizedObject.Text = value;
    }
  }

  get localizedName(): string {
    if (this.locale) {
      return this.name + '_' + this.locale.Name;
    }

    return null;
  }

  get localizedLabel(): string {
    if (this.locale) {
      let name = this.roleType.name;
      const localized = 'Localized';
      if (name.indexOf(localized) === 0) {
        name = name.slice(localized.length);
        name = name.slice(0, name.length - 1);
      }

      const label = this.assignedLabel ? this.assignedLabel : humanize(name);
      return label + ' (' + this.locale.Language?.Name + ')';
    }

    return null;
  }
}
