import {Routes} from '@angular/router';
import {E404Component} from './shared/components/e404/e404.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {
    path: 'home',
    loadChildren: () => import('./home/home.module').then(m => m.HomeModule),
  },
  {
    path: '**',
    component: E404Component,
  }
];
