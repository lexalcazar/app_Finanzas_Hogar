## Purpose

Permitir que usuarios autenticados consulten su situación financiera agregada hasta la fecha actual o hasta una fecha opcional, utilizando exclusivamente los cálculos proporcionados por la API.

## ADDED Requirements

### Requirement: Resumen financiero protegido

El sistema SHALL ofrecer la ruta protegida `/resumen`.

Un usuario sin token de autenticación que intente acceder a `/resumen` MUST ser redirigido a `/login` mediante el guard existente.

Al acceder inicialmente a `/resumen`, el sistema MUST consultar:

`GET /api/Movimientos/resumen`

sin enviar `fechaHasta`.

El sistema MUST mostrar exactamente los valores proporcionados por la API:

- `fechaCalculo`;
- `totalIngresos`;
- `totalGastos`;
- `saldo`.

El frontend MUST NOT recalcular estos importes.

#### Scenario: Consulta inicial de resumen

- **WHEN** un usuario autenticado accede a `/resumen`
- **THEN** el sistema solicita el resumen sin `fechaHasta` y muestra los valores devueltos por la API

#### Scenario: Acceso no autenticado

- **WHEN** un usuario sin token intenta acceder a `/resumen`
- **THEN** el sistema lo redirige a `/login`

### Requirement: Fecha de cálculo opcional

El sistema MUST permitir seleccionar una fecha y solicitar nuevamente el resumen utilizando:

`fechaHasta=YYYY-MM-DD`

La fecha mostrada como fecha efectiva del cálculo MUST ser `fechaCalculo` devuelta por la API.

#### Scenario: Recalcular hasta una fecha

- **WHEN** el usuario selecciona una fecha válida y solicita recalcular el resumen
- **THEN** el sistema solicita el resumen con `fechaHasta` y muestra la nueva respuesta de la API

### Requirement: Estados del resumen

El sistema MUST mostrar un estado de carga mientras espera la respuesta de la API.

Si la consulta falla, MUST mostrar un mensaje comprensible sin exponer detalles técnicos.

#### Scenario: Resumen en carga

- **WHEN** se ha iniciado una consulta y la API todavía no ha respondido
- **THEN** el sistema muestra que el resumen se está cargando

#### Scenario: Error al consultar el resumen

- **WHEN** la consulta de resumen falla
- **THEN** el sistema muestra un mensaje comprensible sin detalles técnicos