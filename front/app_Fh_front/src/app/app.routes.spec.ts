import { provideHttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { provideLocationMocks } from '@angular/common/testing';
import { provideRouter, Router } from '@angular/router';
import { routes } from './app.routes';

describe('application routes', () => {
  it('redirects unauthenticated users from protected routes to login', async () => {
    sessionStorage.clear();
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideRouter(routes), provideLocationMocks()],
    });
    const router = TestBed.inject(Router);

    await router.navigateByUrl('/home');
    expect(router.url).toBe('/login');

    await router.navigateByUrl('/movimientos');
    expect(router.url).toBe('/login');

    await router.navigateByUrl('/movimientos/nuevo');
    expect(router.url).toBe('/login');
  });

  it('redirects protected routes to login after logout removes the token', async () => {
    sessionStorage.setItem('fh.auth-token', 'jwt-token');
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideRouter(routes), provideLocationMocks()],
    });
    const router = TestBed.inject(Router);
    sessionStorage.removeItem('fh.auth-token');

    await router.navigateByUrl('/movimientos');

    expect(router.url).toBe('/login');
  });
});
