import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { MovimientosService } from './movimientos.service';

describe('MovimientosService', () => {
  let service: MovimientosService;
  let httpTesting: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });
    service = TestBed.inject(MovimientosService);
    httpTesting = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpTesting.verify());

  it('requests all authorized movements without user filters', () => {
    service.getAll().subscribe();

    const request = httpTesting.expectOne('/api/movimientos');
    expect(request.request.method).toBe('GET');
    expect(request.request.params.keys()).toEqual([]);
    request.flush([]);
  });

  it('creates a movement without user or type fields', () => {
    service.create({ cantidad: 20, descripcion: null, fecha: '2026-09-16', categoriaId: 1 }).subscribe();
    const request = httpTesting.expectOne('/api/Movimientos');
    expect(request.request.method).toBe('POST');
    expect(request.request.body).toEqual({ cantidad: 20, descripcion: null, fecha: '2026-09-16', categoriaId: 1 });
    request.flush({});
  });

  it('omits empty filters and normalizes text searches', () => {
    service.getAll({ fechaDesde: '', fechaHasta: undefined, busqueda: '   ' }).subscribe();
    const request = httpTesting.expectOne('/api/movimientos');
    expect(request.request.params.keys()).toEqual([]);
    request.flush([]);

    service.getAll({ busqueda: '  mercado  ' }).subscribe();
    const searchRequest = httpTesting.expectOne(request => request.url === '/api/movimientos' && request.params.get('busqueda') === 'mercado');
    searchRequest.flush([]);
  });

  it('sends individual and combined movement filters', () => {
    service.getAll({ tipo: 1 }).subscribe();
    httpTesting.expectOne(request => request.params.get('tipo') === '1').flush([]);
    service.getAll({ tipo: 2, categoriaId: 3, fechaDesde: '2026-09-01', fechaHasta: '2026-09-30', busqueda: 'mercado' }).subscribe();
    const request = httpTesting.expectOne('/api/movimientos?tipo=2&categoriaId=3&fechaDesde=2026-09-01&fechaHasta=2026-09-30&busqueda=mercado');
    expect(request.request.params.keys()).toHaveLength(5);
    request.flush([]);
  });

  it('requests the financial summary with and without a date', () => {
    service.getResumen().subscribe();
    httpTesting.expectOne('/api/Movimientos/resumen').flush({});
    service.getResumen('2026-09-17').subscribe();
    const request = httpTesting.expectOne(request => request.url === '/api/Movimientos/resumen' && request.params.get('fechaHasta') === '2026-09-17');
    request.flush({});
  });
});
