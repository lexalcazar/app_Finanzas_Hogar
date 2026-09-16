import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { Home } from './pages/home/home';
import { Login } from './pages/login/login';
import { Movimientos } from './pages/movimientos/movimientos';
import { Register } from './pages/register/register';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  { path: 'home', component: Home, canActivate: [authGuard] },
  { path: 'movimientos', component: Movimientos, canActivate: [authGuard] },
  { path: '**', redirectTo: 'login' },
];
