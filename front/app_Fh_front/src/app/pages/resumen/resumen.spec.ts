import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { Component } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { Resumen } from './resumen';

@Component({ template: '' }) class TestRoute {}

describe('Resumen', () => {
  let httpTesting: HttpTestingController;
  beforeEach(() => { TestBed.configureTestingModule({ providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([{ path: 'login', component: TestRoute }, { path: 'home', component: TestRoute }])] }); httpTesting = TestBed.inject(HttpTestingController); });
  afterEach(() => httpTesting.verify());
  it('loads and renders the API summary without a date', () => {
    const fixture = TestBed.createComponent(Resumen); fixture.detectChanges();
    expect(fixture.nativeElement.textContent).toContain('Calculando resumen');
    httpTesting.expectOne('/api/Movimientos/resumen').flush({ fechaCalculo: '2026-09-17', totalIngresos: 2500, totalGastos: 1320.5, saldo: 1179.5 }); fixture.detectChanges();
    expect(fixture.nativeElement.textContent).toContain('17/09/2026');
    expect(fixture.nativeElement.textContent).toContain('2,500.00');
    expect(fixture.nativeElement.textContent).toContain('1,320.50');
    expect(fixture.nativeElement.textContent).toContain('1,179.50');
  });
  it('recalculates with the selected date and shows errors', () => {
    const fixture = TestBed.createComponent(Resumen); fixture.detectChanges(); httpTesting.expectOne('/api/Movimientos/resumen').flush({ fechaCalculo: '2026-09-17', totalIngresos: 1, totalGastos: 2, saldo: -1 });
    (fixture.componentInstance as any).fechaHasta.setValue('2026-09-01'); fixture.detectChanges();
    const form = fixture.nativeElement.querySelector('form') as HTMLFormElement;
    form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
    fixture.detectChanges();
    httpTesting.expectOne(request => request.params.get('fechaHasta') === '2026-09-01').flush({ fechaCalculo: '2026-09-01', totalIngresos: 10, totalGastos: 4, saldo: 6 }); fixture.detectChanges();
    expect(fixture.nativeElement.textContent).toContain('01/09/2026');
  });
  it('shows a controlled error from the summary API', () => {
    const fixture = TestBed.createComponent(Resumen); fixture.detectChanges(); httpTesting.expectOne('/api/Movimientos/resumen').error(new ProgressEvent('error')); fixture.detectChanges();
    expect(fixture.nativeElement.textContent).toContain('No se pudo cargar el resumen');
  });
});
