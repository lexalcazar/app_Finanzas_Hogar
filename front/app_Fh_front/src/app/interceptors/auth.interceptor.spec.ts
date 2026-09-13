import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { AuthService } from '../services/auth.service';
import { authInterceptor } from './auth.interceptor';

describe('authInterceptor', () => {
  let authService: AuthService;
  let http: HttpClient;
  let httpTesting: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(withInterceptors([authInterceptor])), provideHttpClientTesting()],
    });
    authService = TestBed.inject(AuthService);
    http = TestBed.inject(HttpClient);
    httpTesting = TestBed.inject(HttpTestingController);
    sessionStorage.clear();
  });

  afterEach(() => httpTesting.verify());

  it('adds the token only to protected requests sent to the application API', () => {
    authService.login({ email: 'ana@example.com', password: 'secret' }).subscribe();
    httpTesting.expectOne('/api/Auth/login').flush({ token: 'jwt-token' });

    http.get('/api/movimientos').subscribe();

    const request = httpTesting.expectOne('/api/movimientos');
    expect(request.request.headers.get('Authorization')).toBe('Bearer jwt-token');
    request.flush({});
  });

  it('does not add the token to public or external requests', () => {
    authService.login({ email: 'ana@example.com', password: 'secret' }).subscribe();
    const loginRequest = httpTesting.expectOne('/api/Auth/login');
    expect(loginRequest.request.headers.has('Authorization')).toBe(false);
    loginRequest.flush({ token: 'jwt-token' });

    http.post('/api/Auth/register', {}).subscribe();
    const registerRequest = httpTesting.expectOne('/api/Auth/register');
    expect(registerRequest.request.headers.has('Authorization')).toBe(false);
    registerRequest.flush({});

    http.get('https://example.com/data').subscribe();
    const externalRequest = httpTesting.expectOne('https://example.com/data');
    expect(externalRequest.request.headers.has('Authorization')).toBe(false);
    externalRequest.flush({});
  });
});
