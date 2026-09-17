import { TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { provideRouter, Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { AuthenticatedNav } from './authenticated-nav';

describe('AuthenticatedNav', () => {
  beforeEach(() => TestBed.configureTestingModule({ providers: [provideRouter([])] }));

  it('links the application identity to home', () => {
    const router = TestBed.inject(Router);
    const fixture = TestBed.createComponent(AuthenticatedNav);
    fixture.detectChanges();
    const link = fixture.debugElement.query(By.directive(RouterLink)).injector.get(RouterLink);

    expect(router.serializeUrl(link.urlTree!)).toBe('/home');
  });

  it('clears the session and navigates to login', () => {
    const authService = TestBed.inject(AuthService);
    const router = TestBed.inject(Router);
    const logoutSpy = vi.spyOn(authService, 'logout');
    const navigateSpy = vi.spyOn(router, 'navigate');
    const fixture = TestBed.createComponent(AuthenticatedNav);

    fixture.debugElement.query(By.css('button')).nativeElement.click();

    expect(logoutSpy).toHaveBeenCalled();
    expect(navigateSpy).toHaveBeenCalledWith(['/login']);
  });
});
