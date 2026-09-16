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
});
