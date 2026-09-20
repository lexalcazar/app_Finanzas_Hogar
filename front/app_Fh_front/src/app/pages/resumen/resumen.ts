import { DatePipe, DecimalPipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { AuthenticatedNav } from '../../components/authenticated-nav/authenticated-nav';
import { ResumenFinanciero } from '../../models/resumen-financiero';
import { MovimientosService } from '../../services/movimientos.service';
@Component({ selector: 'app-resumen', imports: [AuthenticatedNav, DatePipe, DecimalPipe, ReactiveFormsModule], templateUrl: './resumen.html' })
export class Resumen implements OnInit { private readonly service = inject(MovimientosService); protected readonly fechaHasta = new FormControl('', { nonNullable: true }); protected readonly resumen = signal<ResumenFinanciero | null>(null); protected readonly loading = signal(true); protected readonly error = signal<string | null>(null); ngOnInit() { this.load(); } protected onSubmit(event: Event) { event.preventDefault(); this.load(); } protected load() { this.loading.set(true); this.error.set(null); this.service.getResumen(this.fechaHasta.value || undefined).subscribe({ next: data => { this.resumen.set(data); this.loading.set(false); }, error: () => { this.error.set('No se pudo cargar el resumen. Inténtalo de nuevo más tarde.'); this.loading.set(false); } }); } }
