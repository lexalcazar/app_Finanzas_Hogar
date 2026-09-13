import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
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

  it('does not submit an invalid registration form', () => {
    const fixture = TestBed.createComponent(Register);
    fixture.detectChanges();

    fixture.nativeElement.querySelector('form').dispatchEvent(new Event('submit'));
    fixture.detectChanges();

    expect(httpTesting.match('/api/Auth/register')).toEqual([]);
    expect(fixture.nativeElement.querySelector('.field-error')).not.toBeNull();
  });
});
