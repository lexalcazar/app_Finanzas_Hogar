import { Component, inject, OnInit, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Categoria } from '../../models/categoria';
import { CategoriasService } from '../../services/categorias.service';
import { MovimientosService } from '../../services/movimientos.service';
@Component({ selector: 'app-crear-movimiento', imports: [ReactiveFormsModule, RouterLink], templateUrl: './crear-movimiento.html' })
export class CrearMovimiento implements OnInit {
  private readonly categoriasService = inject(CategoriasService); private readonly movimientosService = inject(MovimientosService); private readonly router = inject(Router);
  protected readonly categorias = signal<Categoria[]>([]); protected readonly isLoadingCategories = signal(true); protected readonly isSubmitting = signal(false); protected readonly errorMessage = signal<string | null>(null);
  protected readonly form = new FormGroup({ cantidad: new FormControl<number | null>(null, [Validators.required, Validators.min(.01), Validators.max(999999999)]), descripcion: new FormControl('', { nonNullable: true, validators: [Validators.maxLength(250)] }), fecha: new FormControl('', { nonNullable: true, validators: [Validators.required] }), categoriaId: new FormControl<number | null>(null, [Validators.required, Validators.min(1)]) });
  ngOnInit(): void { this.categoriasService.getAll().subscribe({ next: categorias => { this.categorias.set(categorias); this.isLoadingCategories.set(false); }, error: () => { this.errorMessage.set('No se pudieron cargar las categorías. Inténtalo de nuevo más tarde.'); this.isLoadingCategories.set(false); } }); }
  protected submit(): void { if (this.form.invalid || this.isSubmitting() || this.isLoadingCategories() || this.categorias().length === 0) { this.form.markAllAsTouched(); return; } const v = this.form.getRawValue(); if (v.cantidad === null || v.categoriaId === null) return; this.errorMessage.set(null); this.isSubmitting.set(true); this.movimientosService.create({ cantidad: v.cantidad, descripcion: v.descripcion.trim() || null, fecha: v.fecha, categoriaId: v.categoriaId }).subscribe({ next: () => { this.isSubmitting.set(false); this.router.navigate(['/movimientos']); }, error: () => { this.isSubmitting.set(false); this.errorMessage.set('No se pudo crear el movimiento. Revisa los datos e inténtalo de nuevo.'); } }); }
}
