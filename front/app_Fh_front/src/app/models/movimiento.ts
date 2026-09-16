export interface Movimiento {
  id: number;
  cantidad: number;
  fecha: string;
  descripcion: string | null;
  tipo: 1 | 2;
  categoriaId: number;
  categoria: string | null;
}
