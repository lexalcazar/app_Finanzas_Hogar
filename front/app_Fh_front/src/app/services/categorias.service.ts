import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Categoria } from '../models/categoria';
@Injectable({ providedIn: 'root' }) export class CategoriasService { private readonly http = inject(HttpClient); getAll() { return this.http.get<Categoria[]>(`${environment.apiUrl}/Categorias`); } }
