import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { Movimiento } from '../../models/movimiento';
import { Movimientos } from './movimientos';

describe('Movimientos', () => {
  let httpTesting: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    });
    httpTesting = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpTesting.verify());

  it('shows loading and then the returned movements', () => {
    const fixture = TestBed.createComponent(Movimientos);
    fixture.detectChanges();
    expect(fixture.nativeElement.textContent).toContain('Cargando movimientos');

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
    httpTesting.expectOne('/api/movimientos').flush([]);
    fixture.detectChanges();

    expect(fixture.nativeElement.textContent).toContain('Aún no tienes movimientos');
  });

  it('shows a controlled error when the request fails', () => {
    const fixture = TestBed.createComponent(Movimientos);
    fixture.detectChanges();
    httpTesting.expectOne('/api/movimientos').error(new ProgressEvent('error'));
    fixture.detectChanges();

    expect(fixture.nativeElement.textContent).toContain('No se pudieron cargar los movimientos');
  });
});
