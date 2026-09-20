## Purpose

Permitir localizar movimientos autorizados mediante consultas filtradas resueltas por la API.

## ADDED Requirements

### Requirement: Filtros combinables de movimientos

El sistema SHALL permitir filtrar el listado mediante los parámetros soportados por `GET /api/Movimientos`:

- `fechaDesde`;
- `fechaHasta`;
- `tipo`;
- `categoriaId`;
- `busqueda`.

Los filtros MUST poder combinarse entre sí.

Los parámetros sin valor MUST omitirse de la query string.

El filtrado MUST delegarse a la API y no reproducirse mediante filtrado local en Angular cuando exista un filtro equivalente en backend.

#### Scenario: Aplicar un único filtro

- **WHEN** el usuario aplica un único filtro con valor
- **THEN** el sistema consulta `GET /api/Movimientos` incluyendo únicamente ese parámetro

#### Scenario: Aplicar filtros combinados

- **WHEN** el usuario aplica más de un filtro con valor
- **THEN** el sistema solicita la colección con todos los parámetros correspondientes y muestra la respuesta autorizada

### Requirement: Filtro por tipo

El sistema SHALL permitir seleccionar:

- Todos;
- Ingresos;
- Gastos.

Cuando se seleccione `Ingresos`, el frontend MUST enviar:

`tipo=1`

Cuando se seleccione `Gastos`, MUST enviar:

`tipo=2`

Cuando se seleccione `Todos`, el parámetro `tipo` MUST omitirse.

#### Scenario: Filtrar gastos

- **WHEN** el usuario selecciona `Gastos` y aplica los filtros
- **THEN** el sistema consulta los movimientos con `tipo=2`

### Requirement: Filtro por categoría

El sistema SHALL obtener las categorías mediante el `CategoriasService` existente y mostrarlas por nombre.

El usuario MUST NOT tener que introducir manualmente un `categoriaId`.

Cuando seleccione una categoría, el frontend MUST enviar su identificador mediante `categoriaId`.

#### Scenario: Filtrar por categoría

- **WHEN** el usuario selecciona una categoría y aplica los filtros
- **THEN** el sistema consulta la API utilizando el `categoriaId` correspondiente

### Requirement: Búsqueda por texto

El sistema SHALL permitir introducir texto libre de búsqueda.

Cuando exista texto de búsqueda, MUST enviarlo mediante el parámetro `busqueda`.

#### Scenario: Buscar por descripción

- **WHEN** el usuario introduce texto y aplica los filtros
- **THEN** el sistema consulta la API incluyendo el parámetro `busqueda`

### Requirement: Restablecer filtros

El sistema MUST permitir limpiar todos los filtros y recuperar nuevamente el listado completo.

#### Scenario: Limpiar filtros aplicados

- **WHEN** el usuario limpia los filtros
- **THEN** el sistema elimina sus valores y solicita movimientos sin parámetros de filtro

### Requirement: Resultado vacío

Si una consulta filtrada devuelve una colección vacía, el sistema MUST mostrar un estado vacío comprensible.

#### Scenario: Filtros sin coincidencias

- **WHEN** la API responde correctamente con una colección vacía después de aplicar filtros
- **THEN** el sistema informa de que no se han encontrado movimientos que coincidan con los criterios seleccionados