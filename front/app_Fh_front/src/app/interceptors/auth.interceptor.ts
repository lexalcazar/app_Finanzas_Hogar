import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { environment } from '../../environments/environment';
import { AuthService } from '../services/auth.service';

const publicAuthEndpoints = new Set([
  `${environment.apiUrl}/Auth/login`,
  `${environment.apiUrl}/Auth/register`,
]);

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const token = inject(AuthService).getToken();
  const isOwnApiRequest = request.url.startsWith(`${environment.apiUrl}/`);

  if (!token || !isOwnApiRequest || publicAuthEndpoints.has(request.url)) {
    return next(request);
  }

  return next(request.clone({ setHeaders: { Authorization: `Bearer ${token}` } }));
};
