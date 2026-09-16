import { TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { provideRouter, Router, RouterLink } from '@angular/router';
import { Home } from './home';

describe('Home', () => {
  it('provides navigation to the movements list', () => {
    TestBed.configureTestingModule({ providers: [provideRouter([])] });
    const router = TestBed.inject(Router);
    const fixture = TestBed.createComponent(Home);
    fixture.detectChanges();

    const link = fixture.debugElement.query(By.directive(RouterLink)).injector.get(RouterLink);

    if (link.urlTree === null) {
      throw new Error('Expected the movements link to have a URL tree.');
    }

    expect(router.serializeUrl(link.urlTree)).toBe('/movimientos');
  });

  it('provides navigation to movement creation', () => {
    TestBed.configureTestingModule({ providers: [provideRouter([])] });
    const router = TestBed.inject(Router); const fixture = TestBed.createComponent(Home); fixture.detectChanges();
    const links = fixture.debugElement.queryAll(By.directive(RouterLink)).map(item => item.injector.get(RouterLink));
    expect(links.some(link => link.urlTree !== null && router.serializeUrl(link.urlTree) === '/movimientos/nuevo')).toBe(true);
  });
});
