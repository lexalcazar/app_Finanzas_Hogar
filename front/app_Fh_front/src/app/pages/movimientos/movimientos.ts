import { DecimalPipe, DatePipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Movimiento } from '../../models/movimiento';
import { MovimientosService } from '../../services/movimientos.service';

@Component({
  selector: 'app-movimientos',
  imports: [DatePipe, DecimalPipe, RouterLink],
  templateUrl: './movimientos.html',
})
export class Movimientos implements OnInit {
  private readonly movimientosService = inject(MovimientosService);
  protected readonly movimientos = signal<Movimiento[]>([]);
  protected readonly isLoading = signal(true);
  protected readonly errorMessage = signal<string | null>(null);

  ngOnInit(): void {
    this.movimientosService.getAll().subscribe({
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
    return tipo === 1 ? 'Ingreso' : 'Egreso';
  }
}
