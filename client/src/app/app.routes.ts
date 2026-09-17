import { Routes } from '@angular/router';
import { CustomerSearchComponent } from './features/customers/customer-search.component';
import { ProjectSearchComponent } from './features/projects/project-search.component';

export const routes: Routes = [
  { path: 'projects', component: ProjectSearchComponent, title: 'Projects | Enterprise Full-Stack Reference' },
  { path: 'customers', component: CustomerSearchComponent, title: 'Customers | Enterprise Full-Stack Reference' },
  { path: '', pathMatch: 'full', redirectTo: 'projects' },
  { path: '**', redirectTo: 'projects' }
];
