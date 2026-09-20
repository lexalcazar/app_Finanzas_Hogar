import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { Component } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { Movimiento } from '../../models/movimiento';
import { Movimientos } from './movimientos';

@Component({ template: '' }) class TestRoute {}

describe('Movimientos', () => {
  let httpTesting: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([{ path: 'login', component: TestRoute }, { path: 'home', component: TestRoute }])],
    });
    httpTesting = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpTesting.verify());

  it('shows loading and then the returned movements', () => {
    const fixture = TestBed.createComponent(Movimientos);
    fixture.detectChanges();
    expect(fixture.nativeElement.textContent).toContain('Cargando movimientos');
    httpTesting.expectOne('/api/Categorias').flush([]);

    const movements: Movimiento[] = [{
      id: 1,
      cantidad: 1200,
      fecha: '2026-09-13',
      descripcion: 'Nómina',
      tipo: 1,
      categoriaId: 1,
      categoria: 'Salario',
    }];
    httpTesting.expectOne('/api/movimientos').flush(movements);
    fixture.detectChanges();

    expect(fixture.nativeElement.textContent).toContain('Nómina');
    expect(fixture.nativeElement.textContent).toContain('Ingreso');
  });

  it('shows an empty state when no movements are returned', () => {
    const fixture = TestBed.createComponent(Movimientos);
    fixture.detectChanges();
    httpTesting.expectOne('/api/Categorias').flush([]);
    httpTesting.expectOne('/api/movimientos').flush([]);
    fixture.detectChanges();

    expect(fixture.nativeElement.textContent).toContain('Aún no tienes movimientos');
  });

  it('shows a controlled error when the request fails', () => {
    const fixture = TestBed.createComponent(Movimientos);
    fixture.detectChanges();
    httpTesting.expectOne('/api/Categorias').flush([]);
    httpTesting.expectOne('/api/movimientos').error(new ProgressEvent('error'));
    fixture.detectChanges();

    expect(fixture.nativeElement.textContent).toContain('No se pudieron cargar los movimientos');
  });

  it('applies combined filters and clears them with a new unfiltered request', () => {
    const fixture = TestBed.createComponent(Movimientos); fixture.detectChanges();
    httpTesting.expectOne('/api/Categorias').flush([{ id: 3, nombre: 'Compras', tipo: 2 }]); httpTesting.expectOne('/api/movimientos').flush([]);
    (fixture.componentInstance as any).filters.patchValue({ fechaDesde: '2026-09-01', fechaHasta: '2026-09-30', tipo: '2', categoriaId: 3, busqueda: '  mercado ' });
    (fixture.componentInstance as any).applyFilters(); fixture.detectChanges();
    httpTesting.expectOne(request => request.params.get('tipo') === '2' && request.params.get('categoriaId') === '3' && request.params.get('busqueda') === 'mercado').flush([]);
    fixture.nativeElement.querySelector('.movement-filters button[type="button"]').click();
    httpTesting.expectOne('/api/movimientos').flush([]);
    expect((fixture.componentInstance as any).filters.getRawValue()).toEqual({ fechaDesde: '', fechaHasta: '', tipo: '', categoriaId: '', busqueda: '' });
  });

  it('prevents invalid date ranges and keeps non-category filters available after category errors', () => {
    const fixture = TestBed.createComponent(Movimientos); fixture.detectChanges();
    httpTesting.expectOne('/api/Categorias').error(new ProgressEvent('error')); httpTesting.expectOne('/api/movimientos').flush([]);
    (fixture.componentInstance as any).filters.patchValue({ fechaDesde: '2026-10-01', fechaHasta: '2026-09-01', tipo: '1', busqueda: 'texto' });
    (fixture.componentInstance as any).applyFilters(); fixture.detectChanges();
    expect(fixture.nativeElement.textContent).toContain('La fecha inicial no puede ser posterior');
    expect(fixture.nativeElement.querySelector('#categoria').disabled).toBe(true);
    expect(fixture.nativeElement.querySelector('#tipo').disabled).toBe(false);
  });

  it('sends individual filter values and shows a filtered empty state', () => {
    const fixture = TestBed.createComponent(Movimientos); fixture.detectChanges(); httpTesting.expectOne('/api/Categorias').flush([{ id: 4, nombre: 'Hogar', tipo: 2 }]); httpTesting.expectOne('/api/movimientos').flush([]);
    const filters = (fixture.componentInstance as any).filters;
    for (const [value, parameter] of [[{ tipo: '1' }, 'tipo'], [{ tipo: '2' }, 'tipo'], [{ categoriaId: 4 }, 'categoriaId'], [{ fechaDesde: '2026-09-01' }, 'fechaDesde'], [{ fechaHasta: '2026-09-30' }, 'fechaHasta'], [{ busqueda: '  pan ' }, 'busqueda']] as const) {
      filters.reset({ fechaDesde: '', fechaHasta: '', tipo: '', categoriaId: '', busqueda: '' }); filters.patchValue(value); fixture.nativeElement.querySelector('.movement-filters').dispatchEvent(new Event('submit'));
      httpTesting.expectOne((request: any) => request.params.get(parameter) === (parameter === 'busqueda' ? 'pan' : String(Object.values(value)[0]))).flush([]);
    }
    fixture.detectChanges(); expect(fixture.nativeElement.textContent).toContain('No hay movimientos que coincidan');
  });
});
