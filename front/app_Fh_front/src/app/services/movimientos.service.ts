import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Movimiento } from '../models/movimiento';
import { CreateMovimientoRequest } from '../models/create-movimiento-request';

@Injectable({ providedIn: 'root' })
export class MovimientosService {
  private readonly http = inject(HttpClient);

  getAll() {
    return this.http.get<Movimiento[]>(`${environment.apiUrl}/movimientos`);
  }

  create(request: CreateMovimientoRequest) { return this.http.post<Movimiento>(`${environment.apiUrl}/Movimientos`, request); }
}
