import { provideHttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { provideRouter, Router, RouterLink } from '@angular/router';
import { Home } from './home';
import { AuthService } from '../../services/auth.service';

describe('Home', () => {
  it('provides navigation to the movements list', () => {
    TestBed.configureTestingModule({ providers: [provideHttpClient(), provideRouter([])] });
    const router = TestBed.inject(Router);
    const fixture = TestBed.createComponent(Home);
    fixture.detectChanges();

    const link = fixture.debugElement.query(By.css('.home-action-card')).injector.get(RouterLink);

    if (link.urlTree === null) {
      throw new Error('Expected the movements link to have a URL tree.');
    }

    expect(router.serializeUrl(link.urlTree)).toBe('/movimientos');
  });

  it('provides navigation to movement creation', () => {
    TestBed.configureTestingModule({ providers: [provideHttpClient(), provideRouter([])] });
    const router = TestBed.inject(Router); const fixture = TestBed.createComponent(Home); fixture.detectChanges();
    const links = fixture.debugElement.queryAll(By.directive(RouterLink)).map(item => item.injector.get(RouterLink));
    expect(links.some(link => link.urlTree !== null && router.serializeUrl(link.urlTree) === '/movimientos/nuevo')).toBe(true);
  });

  it('shows a personalized greeting when a display name is available', () => {
    TestBed.configureTestingModule({ providers: [provideHttpClient(), provideRouter([])] });
    vi.spyOn(TestBed.inject(AuthService), 'getDisplayName').mockReturnValue('Cristina');
    const fixture = TestBed.createComponent(Home);
    fixture.detectChanges();

    expect(fixture.nativeElement.textContent).toContain('Bienvenido, Cristina');
  });

  it('shows a generic greeting and both action cards without a display name', () => {
    TestBed.configureTestingModule({ providers: [provideHttpClient(), provideRouter([])] });
    const fixture = TestBed.createComponent(Home);
    fixture.detectChanges();

    expect(fixture.nativeElement.textContent).toContain('Bienvenido');
    expect(fixture.nativeElement.textContent).toContain('Mis movimientos');
    expect(fixture.nativeElement.textContent).toContain('Crear movimiento');
    expect(fixture.debugElement.queryAll(By.css('.home-action-card')).length).toBe(2);
  });
});
