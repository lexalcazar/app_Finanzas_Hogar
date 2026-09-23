import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { Home } from './pages/home/home';
import { CrearMovimiento } from './pages/crear-movimiento/crear-movimiento';
import { Login } from './pages/login/login';
import { Movimientos } from './pages/movimientos/movimientos';
import { Register } from './pages/register/register';
import { Resumen } from './pages/resumen/resumen';
import { Asistente } from './pages/asistente/asistente';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  { path: 'home', component: Home, canActivate: [authGuard] },
  { path: 'movimientos', component: Movimientos, canActivate: [authGuard] },
  { path: 'movimientos/nuevo', component: CrearMovimiento, canActivate: [authGuard] },
  { path: 'resumen', component: Resumen, canActivate: [authGuard] },
  { path: 'asistente', component: Asistente, canActivate: [authGuard] },
  { path: '**', redirectTo: 'login' },
];
