import { Component, Optional } from '@angular/core';
import { NgForm } from '@angular/forms';
import { LocalizedRoleField } from '@allors/base/workspace/angular/foundation';

@Component({
  selector: 'a-mat-localized-text',
  templateUrl: './localized-text.component.html',
})
export class AllorsMaterialLocalizedTextComponent extends LocalizedRoleField {
  constructor(@Optional() form: NgForm) {
    super(form);
  }
}
