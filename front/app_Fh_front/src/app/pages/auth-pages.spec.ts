import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { vi } from 'vitest';
import { Login } from './login/login';
import { Register } from './register/register';

describe('authentication pages', () => {
  let httpTesting: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    });
    httpTesting = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpTesting.verify());

  it('does not submit an invalid login form', () => {
    const fixture = TestBed.createComponent(Login);
    fixture.detectChanges();

    fixture.nativeElement.querySelector('form').dispatchEvent(new Event('submit'));
    fixture.detectChanges();

    expect(httpTesting.match('/api/Auth/login')).toEqual([]);
    expect(fixture.nativeElement.querySelector('.field-error')).not.toBeNull();
  });

  it('navigates to home after a successful login', () => {
    const router = TestBed.inject(Router);
    const navigate = vi.spyOn(router, 'navigate').mockResolvedValue(true);
    const fixture = TestBed.createComponent(Login);
    fixture.detectChanges();
    const inputs = fixture.nativeElement.querySelectorAll('input') as NodeListOf<HTMLInputElement>;
    inputs[0].value = 'ana@example.com';
    inputs[0].dispatchEvent(new Event('input'));
    inputs[1].value = 'secret';
    inputs[1].dispatchEvent(new Event('input'));
    fixture.nativeElement.querySelector('form').dispatchEvent(new Event('submit'));

    httpTesting.expectOne('/api/Auth/login').flush({ token: 'jwt-token' });

    expect(sessionStorage.getItem('fh.auth-token')).toBe('jwt-token');
    expect(navigate).toHaveBeenCalledWith(['/home']);
  });

  it('does not submit an invalid registration form', () => {
    const fixture = TestBed.createComponent(Register);
    fixture.detectChanges();

    fixture.nativeElement.querySelector('form').dispatchEvent(new Event('submit'));
    fixture.detectChanges();

    expect(httpTesting.match('/api/Auth/register')).toEqual([]);
    expect(fixture.nativeElement.querySelector('.field-error')).not.toBeNull();
  });

  it('keeps public authentication pages free of the authenticated navigation', () => {
    const loginFixture = TestBed.createComponent(Login);
    const registerFixture = TestBed.createComponent(Register);
    loginFixture.detectChanges();
    registerFixture.detectChanges();

    expect(loginFixture.nativeElement.querySelector('.auth-page .auth-card')).not.toBeNull();
    expect(registerFixture.nativeElement.querySelector('.auth-page .auth-card')).not.toBeNull();
    expect(loginFixture.nativeElement.querySelector('app-authenticated-nav')).toBeNull();
    expect(registerFixture.nativeElement.querySelector('app-authenticated-nav')).toBeNull();
  });
});
