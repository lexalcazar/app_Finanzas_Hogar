## Context

La página `/movimientos` consulta actualmente `GET /api/Movimientos` sin parámetros y representa los estados de carga, listado con datos, listado vacío y error.

`CategoriasService` ya proporciona las categorías disponibles mediante `GET /api/Categorias`.

La aplicación dispone además de:

- autenticación JWT;
- interceptor Bearer;
- guard de rutas protegidas;
- barra de navegación autenticada compartida;
- página Home con tarjetas de navegación;
- identidad visual global centralizada.

El backend ha ampliado `GET /api/Movimientos` para aceptar filtros opcionales:

- `fechaDesde`;
- `fechaHasta`;
- `tipo`;
- `categoriaId`;
- `busqueda`.

También existe el endpoint protegido:

`GET /api/Movimientos/resumen`

que acepta opcionalmente:

`fechaHasta`

y devuelve:

- `fechaCalculo`;
- `totalIngresos`;
- `totalGastos`;
- `saldo`.

El backend es la fuente de verdad tanto para el filtrado como para los cálculos financieros.

Véase `proposal.md` y las specs `movement-filtering` y `financial-summary` para el comportamiento requerido.

## Goals / Non-Goals

### Goals

- Ampliar `/movimientos` con filtros combinables delegados a la API.

- Enviar únicamente parámetros de consulta que tengan un valor real.

- Reutilizar `CategoriasService` para el filtro de categoría.

- Mantener los estados existentes de carga, vacío y error del listado.

- Crear una página protegida `/resumen`.

- Mostrar en `/resumen` exactamente los valores calculados por la API.

- Permitir recalcular el resumen hasta una fecha opcional.

- Añadir acceso a `/resumen` desde Home.

- Mantener la identidad visual y navegación compartidas existentes.

### Non-Goals

- No modificar el backend.

- No realizar filtrado de movimientos en memoria cuando la API ya soporta ese filtro.

- No calcular ingresos, gastos ni saldo en Angular.

- No añadir paginación.

- No añadir ordenación avanzada.

- No realizar búsquedas automáticas mientras se escribe.

- No introducir debounce.

- No añadir gráficos.

- No implementar todavía análisis por categoría, mensual o anual.

- No añadir nuevas dependencias.

## Decisions

### Formulario explícito de filtros

La página de movimientos utilizará un formulario para gestionar:

- fecha desde;
- fecha hasta;
- tipo;
- categoría;
- texto de búsqueda.

La modificación de un campo no realizará automáticamente una nueva petición.

El usuario deberá activar explícitamente:

`Aplicar filtros`

para ejecutar una nueva consulta.

Esto evita solicitudes HTTP mientras se escribe y mantiene el comportamiento sencillo y predecible.

Se descarta búsqueda reactiva con debounce dentro de esta feature.

---

### Contrato tipado de filtros

Se creará un contrato TypeScript para representar los filtros opcionales del listado.

Conceptualmente contendrá:

```text
fechaDesde?
fechaHasta?
tipo?
categoriaId?
busqueda?