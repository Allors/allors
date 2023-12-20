import { Injectable } from '@angular/core';

import { Locale } from 'date-fns';

@Injectable({
  providedIn: 'root',
})
export abstract class DateService {
  abstract readonly locale: Locale;
}
