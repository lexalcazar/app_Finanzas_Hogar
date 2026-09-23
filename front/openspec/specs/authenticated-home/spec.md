## Purpose

Ofrecer a los usuarios autenticados una página principal segura desde la que acceder al primer flujo de consulta de movimientos de la aplicación.

## Requirements

### Requirement: Página principal autenticada
El sistema SHALL ofrecer la ruta `/home` exclusivamente a usuarios con una sesión autenticada. La página MUST mostrar un saludo personalizado con el nombre de presentación disponible para el usuario y opciones claras para acceder al listado de movimientos y al formulario de creación de movimientos. Si un usuario sin sesión intenta acceder a `/home`, el sistema MUST redirigirlo a `/login`.

#### Scenario: Acceso autenticado a la página principal
- **WHEN** un usuario con un token de sesión accede a `/home`
- **THEN** el sistema muestra la página principal, su saludo personalizado y las opciones para ver y crear movimientos

#### Scenario: Acceso no autenticado a la página principal
- **WHEN** un usuario sin un token de sesión accede a `/home`
- **THEN** el sistema lo redirige a `/login`

### Requirement: Navegación al listado de movimientos
El sistema MUST permitir que un usuario autenticado navegue desde `/home` a `/movimientos` mediante la opción de consulta de movimientos.

#### Scenario: Selección de movimientos desde Home
- **WHEN** un usuario autenticado selecciona la opción de ver sus movimientos
- **THEN** el sistema navega a `/movimientos`

### Requirement: Acciones de Home mediante tarjetas accesibles
El sistema MUST presentar las acciones de consultar y crear movimientos, acceder al resumen financiero y abrir el asistente financiero como tarjetas independientes con icono, título y descripción. Cada tarjeta MUST tener un área completa activable, ser accesible mediante teclado y proporcionar estados de foco y hover perceptibles con contraste suficiente. La interfaz de Home MUST mantener una presentación responsive basada principalmente en blanco, negro y tonos grises.

#### Scenario: Consulta de movimientos desde una tarjeta
- **WHEN** un usuario autenticado activa la tarjeta de consulta de movimientos mediante puntero o teclado
- **THEN** el sistema navega a `/movimientos`

#### Scenario: Creación de movimientos desde una tarjeta
- **WHEN** un usuario autenticado activa la tarjeta de creación de movimientos mediante puntero o teclado
- **THEN** el sistema navega a `/movimientos/nuevo`

#### Scenario: Acceso al resumen desde Home
- **WHEN** un usuario autenticado activa la tarjeta Resumen financiero mediante puntero o teclado
- **THEN** el sistema navega a `/resumen`

#### Scenario: Acceso al asistente desde Home
- **WHEN** un usuario autenticado activa la tarjeta Asistente financiero mediante puntero o teclado
- **THEN** el sistema navega a `/asistente`
