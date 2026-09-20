import { DecimalPipe, DatePipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthenticatedNav } from '../../components/authenticated-nav/authenticated-nav';
import { Movimiento } from '../../models/movimiento';
import { Categoria } from '../../models/categoria';
import { MovimientoFiltros } from '../../models/movimiento-filtros';
import { CategoriasService } from '../../services/categorias.service';
import { MovimientosService } from '../../services/movimientos.service';

@Component({
  selector: 'app-movimientos',
  imports: [AuthenticatedNav, DatePipe, DecimalPipe, ReactiveFormsModule, RouterLink],
  templateUrl: './movimientos.html',
})
export class Movimientos implements OnInit {
  private readonly movimientosService = inject(MovimientosService);
  private readonly categoriasService = inject(CategoriasService);
  protected readonly movimientos = signal<Movimiento[]>([]);
  protected readonly isLoading = signal(true);
  protected readonly errorMessage = signal<string | null>(null);
  protected readonly categorias = signal<Categoria[]>([]);
  protected readonly categoriesError = signal(false);
  protected readonly filtersError = signal<string | null>(null);
  protected readonly hasFilters = signal(false);
  protected readonly filters = new FormGroup({ fechaDesde: new FormControl('', { nonNullable: true }), fechaHasta: new FormControl('', { nonNullable: true }), tipo: new FormControl<'' | '1' | '2'>('', { nonNullable: true }), categoriaId: new FormControl<'' | number>('', { nonNullable: true }), busqueda: new FormControl('', { nonNullable: true }) });

  ngOnInit(): void {
    this.load();
    this.categoriasService.getAll().subscribe({ next: categorias => this.categorias.set(categorias), error: () => { this.categoriesError.set(true); this.filters.controls.categoriaId.disable(); } });
  }

  protected applyFilters(): void {
    const value = this.filters.getRawValue();
    if (value.fechaDesde && value.fechaHasta && value.fechaDesde > value.fechaHasta) { this.filtersError.set('La fecha inicial no puede ser posterior a la fecha final.'); return; }
    this.filtersError.set(null); this.hasFilters.set(true);
    this.load({ fechaDesde: value.fechaDesde || undefined, fechaHasta: value.fechaHasta || undefined, tipo: value.tipo ? Number(value.tipo) as 1 | 2 : undefined, categoriaId: value.categoriaId || undefined, busqueda: value.busqueda });
  }

  protected clearFilters(): void { this.filters.reset({ fechaDesde: '', fechaHasta: '', tipo: '', categoriaId: '', busqueda: '' }); this.filtersError.set(null); this.hasFilters.set(false); this.load(); }

  private load(filtros: MovimientoFiltros = {}): void {
    this.isLoading.set(true); this.errorMessage.set(null);
    this.movimientosService.getAll(filtros).subscribe({
      next: (movimientos) => {
        this.movimientos.set(movimientos);
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set('No se pudieron cargar los movimientos. Inténtalo de nuevo más tarde.');
        this.isLoading.set(false);
      },
    });
  }

  protected getTipo(tipo: Movimiento['tipo']): string {
    return tipo === 1 ? 'Ingreso' : 'Gasto';
  }
}
