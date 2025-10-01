import { Routes } from '@angular/router';
import { AuthGuard } from './home/auth.guard';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: async () => {
      const m = await import('./home/home');
      return m.Home;
    },
    canActivate: [AuthGuard],
  },
  {
    path: 'login',
    loadComponent: async () => {
      const m = await import('./login/login');
      return m.Login;
    },
  },
  {
    path: 'register',
    loadComponent: async () => {
      const m = await import('./register/register');
      return m.Register;
    },
  },
];
