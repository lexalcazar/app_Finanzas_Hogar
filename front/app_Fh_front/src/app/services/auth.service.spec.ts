import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { AuthService } from './auth.service';

describe('AuthService', () => {
  let service: AuthService;
  let httpTesting: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });
    service = TestBed.inject(AuthService);
    httpTesting = TestBed.inject(HttpTestingController);
    sessionStorage.clear();
  });

  afterEach(() => httpTesting.verify());

  it('sends login credentials and stores the returned token', () => {
    service.login({ email: 'ana@example.com', password: 'secret' }).subscribe();

    const request = httpTesting.expectOne('/api/Auth/login');
    expect(request.request.method).toBe('POST');
    expect(request.request.body).toEqual({ email: 'ana@example.com', password: 'secret' });
    request.flush({ token: 'jwt-token' });

    expect(service.getToken()).toBe('jwt-token');
  });

  it('sends registration data without storing a token', () => {
    service.register({ nombre: 'Ana', apellido: 'Pérez', email: 'ana@example.com', password: 'secret' }).subscribe();

    const request = httpTesting.expectOne('/api/Auth/register');
    expect(request.request.method).toBe('POST');
    expect(request.request.body).toEqual({
      nombre: 'Ana',
      apellido: 'Pérez',
      email: 'ana@example.com',
      password: 'secret',
    });
    request.flush({ message: 'Usuario registrado exitosamente' });

    expect(service.getToken()).toBeNull();
  });

  it('does not store a token when login is rejected or fails to connect', () => {
    service.login({ email: 'ana@example.com', password: 'invalid' }).subscribe({ error: () => undefined });
    httpTesting.expectOne('/api/Auth/login').flush(
      { message: 'Email o contraseña incorrectos' },
      { status: 401, statusText: 'Unauthorized' },
    );

    service.login({ email: 'ana@example.com', password: 'secret' }).subscribe({ error: () => undefined });
    httpTesting.expectOne('/api/Auth/login').error(new ProgressEvent('error'));

    expect(service.getToken()).toBeNull();
  });

  it('preserves the absence of a token when registration is rejected or fails to connect', () => {
    const request = { nombre: 'Ana', apellido: 'Pérez', email: 'ana@example.com', password: 'secret' };

    service.register(request).subscribe({ error: () => undefined });
    httpTesting.expectOne('/api/Auth/register').flush([], { status: 400, statusText: 'Bad Request' });

    service.register(request).subscribe({ error: () => undefined });
    httpTesting.expectOne('/api/Auth/register').error(new ProgressEvent('error'));

    expect(service.getToken()).toBeNull();
  });

  it('gets a UTF-8 display name from the JWT payload only for presentation', () => {
    const payload = btoa(unescape(encodeURIComponent(JSON.stringify({ 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name': 'María' })))).replace(/\+/g, '-').replace(/\//g, '_').replace(/=/g, '');
    sessionStorage.setItem('fh.auth-token', `header.${payload}.signature`);

    expect(service.getDisplayName()).toBe('María');
  });

  it('returns null when the token is absent, malformed, or missing its name claim', () => {
    expect(service.getDisplayName()).toBeNull();
    sessionStorage.setItem('fh.auth-token', 'not-a-jwt');
    expect(service.getDisplayName()).toBeNull();
    sessionStorage.setItem('fh.auth-token', 'header.eyJzdWIiOiIxIn0.signature');
    expect(service.getDisplayName()).toBeNull();
  });

  it('clears the stored token and assistant history when logging out without an HTTP request', () => {
    sessionStorage.setItem('fh.auth-token', 'jwt-token');
    sessionStorage.setItem('fh.asistente-chat-history', JSON.stringify([{ contenido: 'Consulta', autor: 'usuario' }]));

    service.logout();

    expect(service.getToken()).toBeNull();
    expect(sessionStorage.getItem('fh.asistente-chat-history')).toBeNull();
  });
});
