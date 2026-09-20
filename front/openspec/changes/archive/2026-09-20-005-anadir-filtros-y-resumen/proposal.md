## Why

El listado actual no permite localizar movimientos concretos mediante criterios de búsqueda y no existe una vista específica donde el usuario pueda consultar su situación financiera acumulada.

La API ya ofrece filtros opcionales para el listado de movimientos y un endpoint protegido de resumen financiero, por lo que el frontend debe exponer estas capacidades sin duplicar cálculos, autorización ni lógica de filtrado que corresponde al backend.

## What Changes

- Añadir filtros combinables al listado de movimientos utilizando los parámetros soportados por `GET /api/Movimientos`:
  - `fechaDesde`;
  - `fechaHasta`;
  - `tipo`;
  - `categoriaId`;
  - `busqueda`.

- Enviar únicamente a la API los parámetros que tengan un valor definido.

- Reutilizar las categorías existentes para permitir seleccionar una categoría por nombre y enviar internamente su `categoriaId`.

- Mantener los filtros y la búsqueda dentro de `/movimientos`, sin crear una ruta ni una tarjeta independiente de búsqueda avanzada en Home.

- Crear la ruta protegida `/resumen` para mostrar el resumen financiero devuelto por:

  `GET /api/Movimientos/resumen`

- Al acceder inicialmente a `/resumen`, realizar la consulta sin `fechaHasta`, permitiendo que el backend utilice la fecha actual.

- Permitir seleccionar opcionalmente una fecha y recalcular el resumen mediante:

  `GET /api/Movimientos/resumen?fechaHasta=YYYY-MM-DD`

- Mostrar los valores devueltos por la API:
  - fecha de cálculo;
  - total de ingresos;
  - total de gastos;
  - saldo.

- No recalcular ingresos, gastos ni saldo en Angular.

- Añadir la tarjeta `Resumen financiero` a Home, manteniendo las dos acciones actuales y la identidad visual compartida.

## Capabilities

### New Capabilities

- `financial-summary`: Consulta y presenta los ingresos, gastos y saldo autorizados hasta la fecha actual o hasta una fecha seleccionada por el usuario.

- `movement-filtering`: Permite consultar movimientos mediante los filtros soportados por la API y combinarlos entre sí.

### Modified Capabilities

- `authenticated-home`: Añade mediante una nueva tarjeta el acceso al resumen financiero.

- `user-movements-list`: Permite realizar consultas filtradas delegando el filtrado a la API.

## Impact

- Amplía `MovimientosService` para soportar filtros opcionales y consultar el resumen financiero.

- Reutiliza `CategoriasService`.

- Añade los contratos TypeScript necesarios para filtros y resumen cuando corresponda.

- Modifica Home y el listado existente de movimientos.

- Añade una nueva página standalone protegida para `/resumen`.

- Añade la ruta protegida `/resumen` reutilizando el guard existente.

- Reutiliza autenticación, JWT, interceptor, navbar y estilos globales existentes.

- No modifica el backend.

- No modifica endpoints ni contratos HTTP existentes.

- No modifica el modelo de autorización.

- No añade dependencias externas.

- No calcula totales ni filtra usuarios en Angular.

- No incluye paginación, ordenación avanzada, gráficos, edición ni eliminación de movimientos.