import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { vi } from 'vitest';
import { CrearMovimiento } from './crear-movimiento';

describe('CrearMovimiento', () => {
  let http: HttpTestingController;
  beforeEach(() => { TestBed.configureTestingModule({ providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])] }); http = TestBed.inject(HttpTestingController); });
  afterEach(() => http.verify());
  function fixture() { const result = TestBed.createComponent(CrearMovimiento); result.detectChanges(); return result; }
  function loadCategories() { http.expectOne('/api/Categorias').flush([{ id: 1, nombre: 'Salario', tipo: 1 }]); }
  function component(value: ReturnType<typeof fixture>) { return value.componentInstance as unknown as { form: { patchValue(value: object): void; invalid: boolean; getRawValue(): unknown }; submit(): void; isSubmitting(): boolean }; }

  it('handles empty categories and category loading errors', () => { let result = fixture(); http.expectOne('/api/Categorias').flush([]); result.detectChanges(); expect(result.nativeElement.textContent).toContain('No hay categorías'); result = fixture(); http.expectOne('/api/Categorias').error(new ProgressEvent('error')); result.detectChanges(); expect(result.nativeElement.textContent).toContain('No se pudieron cargar'); });
  it('validates all required field constraints and blocks invalid submission', () => { const result = fixture(); loadCategories(); const page = component(result); page.form.patchValue({ cantidad: 0, descripcion: 'a'.repeat(251), fecha: '', categoriaId: null }); expect(page.form.invalid).toBe(true); page.submit(); expect(http.match('/api/Movimientos')).toEqual([]); page.form.patchValue({ cantidad: 999999999.01, descripcion: '', fecha: '2026-09-16', categoriaId: 1 }); expect(page.form.invalid).toBe(true); });
  it('prevents duplicates, preserves data after error, and navigates after success', () => { const result = fixture(); loadCategories(); const page = component(result); const navigate = vi.spyOn(TestBed.inject(Router), 'navigate').mockResolvedValue(true); page.form.patchValue({ cantidad: 20, descripcion: 'Compra', fecha: '2026-09-16', categoriaId: 1 }); page.submit(); expect(page.isSubmitting()).toBe(true); page.submit(); const [request] = http.match('/api/Movimientos'); expect(request).toBeTruthy(); request.flush({}, { status: 400, statusText: 'Bad Request' }); expect(page.form.getRawValue()).toEqual({ cantidad: 20, descripcion: 'Compra', fecha: '2026-09-16', categoriaId: 1 }); page.submit(); http.expectOne('/api/Movimientos').flush({}); expect(navigate).toHaveBeenCalledWith(['/movimientos']); });
});
