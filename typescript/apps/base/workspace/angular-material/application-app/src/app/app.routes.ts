import { Routes } from '@angular/router';

import { AuthorizationService } from './auth/authorization.service';

import { LoginComponent } from './auth/login.component';
import { MainComponent } from './main/main.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FieldsComponent } from './fields/fields-form.component';

import { CountryListPageComponent } from './domain/country/list/country-list-page.component';
import { OrganizationListPageComponent } from './domain/organization/list/organization-list-page.component';
import { OrganizationOverviewPageComponent } from './domain/organization/overview/organization-overview-page.component';
import { PersonListPageComponent } from './domain/person/list/person-list-page.component';
import { PersonOverviewPageComponent } from './domain/person/overview/person-overview-page.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  {
    path: '',
    component: MainComponent,
    canActivate: [AuthorizationService],
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent,
      },
      {
        path: 'contacts',
        children: [
          { path: 'people', component: PersonListPageComponent },
          { path: 'person/:id', component: PersonOverviewPageComponent },
          {
            path: 'organizations',
            component: OrganizationListPageComponent,
          },
          {
            path: 'organization/:id',
            component: OrganizationOverviewPageComponent,
          },
          { path: 'countries', component: CountryListPageComponent },
        ],
      },
      {
        path: 'fields',
        component: FieldsComponent,
      },
    ],
  },
];

export const components: any[] = [
  LoginComponent,
  MainComponent,
  DashboardComponent,
  FieldsComponent,
  CountryListPageComponent,
  OrganizationListPageComponent,
  OrganizationOverviewPageComponent,
  PersonListPageComponent,
  PersonOverviewPageComponent,
];
