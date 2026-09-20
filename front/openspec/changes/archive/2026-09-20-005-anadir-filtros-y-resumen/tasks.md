## 1. Contratos y servicios

- [x] 1.1 Crear el contrato TypeScript para filtros de movimientos con parámetros opcionales:
  - fechaDesde;
  - fechaHasta;
  - tipo;
  - categoriaId;
  - busqueda.

- [x] 1.2 Crear el contrato TypeScript para el resumen financiero con:
  - fechaCalculo;
  - totalIngresos;
  - totalGastos;
  - saldo.

- [x] 1.3 Extender `MovimientosService` para aceptar filtros opcionales en:

  `GET /api/Movimientos`

- [x] 1.4 Construir `HttpParams` únicamente con valores definidos.

- [x] 1.5 Normalizar `busqueda` eliminando espacios exteriores y omitir el parámetro si queda vacío.

- [x] 1.6 Extender `MovimientosService` para consultar:

  `GET /api/Movimientos/resumen`

  sin fecha o con:

  `fechaHasta=YYYY-MM-DD`

- [x] 1.7 Añadir pruebas HTTP verificando:
  - consulta sin filtros;
  - omisión de parámetros vacíos;
  - un único filtro;
  - varios filtros combinados;
  - `tipo=1`;
  - `tipo=2`;
  - categoría;
  - búsqueda;
  - resumen sin fecha;
  - resumen con fecha.

## 2. Listado filtrable

- [x] 2.1 Añadir a `/movimientos` un formulario de filtros con:
  - fecha desde;
  - fecha hasta;
  - tipo;
  - categoría;
  - texto de búsqueda.

- [x] 2.2 Mostrar para tipo:
  - Todos;
  - Ingresos;
  - Gastos.

- [x] 2.3 Convertir los valores de tipo para la API:
  - Todos → sin parámetro;
  - Ingresos → `1`;
  - Gastos → `2`.

- [x] 2.4 Reutilizar `CategoriasService` para cargar las categorías.

- [x] 2.5 Mostrar las categorías por nombre y utilizar internamente su `id` como `categoriaId`.

- [x] 2.6 Si la carga de categorías falla:
  - mostrar un mensaje controlado;
  - deshabilitar únicamente el filtro de categoría;
  - mantener disponibles los filtros de fechas, tipo y búsqueda.

- [x] 2.7 Validar que `fechaDesde` no sea posterior a `fechaHasta`.

- [x] 2.8 Impedir aplicar los filtros cuando la combinación de fechas sea inválida y mostrar un mensaje comprensible.

- [x] 2.9 Implementar `Aplicar filtros` mediante una nueva consulta a la API.

- [x] 2.10 Mantener los estados existentes de:
  - carga;
  - datos;
  - error.

- [x] 2.11 Mostrar un estado vacío específico cuando una consulta filtrada no devuelve resultados.

- [x] 2.12 Implementar `Limpiar filtros` para:
  - restablecer todos los controles;
  - eliminar los filtros;
  - consultar nuevamente `GET /api/Movimientos` sin parámetros.

- [x] 2.13 Añadir pruebas para:
  - aplicación de un filtro;
  - filtros combinados;
  - fechas inválidas;
  - búsqueda vacía o con espacios;
  - filtro por tipo;
  - filtro por categoría;
  - error al cargar categorías;
  - resultado vacío;
  - limpieza de filtros.

## 3. Resumen financiero

- [x] 3.1 Crear la página standalone protegida:

  `/resumen`

  utilizando el guard existente.

- [x] 3.2 Integrar la barra de navegación autenticada compartida.

- [x] 3.3 Consultar el resumen inicialmente sin enviar `fechaHasta`.

- [x] 3.4 Mostrar exactamente los valores devueltos por la API:
  - `fechaCalculo`;
  - `totalIngresos`;
  - `totalGastos`;
  - `saldo`.

- [x] 3.5 No realizar cálculos locales de ingresos, gastos o saldo.

- [x] 3.6 Añadir un selector opcional de fecha.

- [x] 3.7 Permitir recalcular el resumen mediante:

  `fechaHasta=YYYY-MM-DD`

- [x] 3.8 Mostrar siempre `fechaCalculo` devuelta por la API como fecha efectiva del cálculo.

- [x] 3.9 Gestionar mediante estado local:
  - carga;
  - datos;
  - error;
  - fecha seleccionada.

- [x] 3.10 Mantener el error del resumen independiente del estado del listado de movimientos.

- [x] 3.11 Añadir pruebas para:
  - consulta inicial sin fecha;
  - consulta con fecha;
  - estado de carga;
  - datos recibidos;
  - error;
  - ausencia de cálculos locales;
  - ruta protegida sin token.

## 4. Home

- [x] 4.1 Añadir una tarjeta accesible:

  `Resumen financiero`

  con destino:

  `/resumen`

- [x] 4.2 Mantener las tarjetas actuales:
  - `Mis movimientos`;
  - `Crear movimiento`.

- [x] 4.3 No añadir una tarjeta independiente de búsqueda avanzada.

- [x] 4.4 Utilizar SVG inline, `routerLink`, foco visible y área completa navegable.

- [x] 4.5 Añadir pruebas de navegación desde Home a `/resumen`.

## 5. Identidad visual y verificación

- [x] 5.1 Aplicar a filtros y resumen la identidad visual global definida en:
  - `constitution.md`;
  - `AGENTS.md`;
  - `styles.css`.

- [x] 5.2 Reutilizar las variables CSS globales existentes y no introducir una paleta independiente.

- [x] 5.3 Mantener diseño responsive, contraste accesible y estados `hover` y `focus`.

- [x] 5.4 No añadir dependencias externas.

- [x] 5.5 Ejecutar `npm test` y corregir únicamente errores relacionados con este cambio.

- [x] 5.6 Ejecutar `npm run build` y corregir únicamente errores relacionados con este cambio.

- [x] 5.7 Ejecutar la validación estricta de OpenSpec y dejar todas las tareas completadas antes de archivar.
