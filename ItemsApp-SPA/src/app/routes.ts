import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListsResolver } from './resolvers/lists.resolver';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent, resolve: { items: ListsResolver } },
    { path: '**', redirectTo: '', pathMatch: 'full' }
];