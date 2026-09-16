import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { CategoriasService } from './categorias.service';
describe('CategoriasService', () => { let service: CategoriasService; let http: HttpTestingController; beforeEach(() => { TestBed.configureTestingModule({ providers: [provideHttpClient(), provideHttpClientTesting()] }); service = TestBed.inject(CategoriasService); http = TestBed.inject(HttpTestingController); }); afterEach(() => http.verify()); it('loads categories', () => { service.getAll().subscribe(); const request = http.expectOne('/api/Categorias'); expect(request.request.method).toBe('GET'); request.flush([]); }); });
