import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Movimiento } from '../models/movimiento';
import { CreateMovimientoRequest } from '../models/create-movimiento-request';
import { MovimientoFiltros } from '../models/movimiento-filtros';
import { ResumenFinanciero } from '../models/resumen-financiero';

@Injectable({ providedIn: 'root' })
export class MovimientosService {
  private readonly http = inject(HttpClient);

  getAll(filtros: MovimientoFiltros = {}) {
    let params = new HttpParams();
    for (const [key, value] of Object.entries(filtros)) {
      const normalized = key === 'busqueda' && typeof value === 'string' ? value.trim() : value;
      if (normalized !== undefined && normalized !== null && normalized !== '') params = params.set(key, String(normalized));
    }
    return this.http.get<Movimiento[]>(`${environment.apiUrl}/movimientos`, { params });
  }

  getResumen(fechaHasta?: string) { return this.http.get<ResumenFinanciero>(`${environment.apiUrl}/Movimientos/resumen`, { params: fechaHasta ? { fechaHasta } : undefined }); }

  create(request: CreateMovimientoRequest) { return this.http.post<Movimiento>(`${environment.apiUrl}/Movimientos`, request); }
}
