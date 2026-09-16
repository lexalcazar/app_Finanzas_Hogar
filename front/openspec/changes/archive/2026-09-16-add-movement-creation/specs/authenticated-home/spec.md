## MODIFIED Requirements

### Requirement: Página principal autenticada
El sistema SHALL ofrecer la ruta `/home` exclusivamente a usuarios con una sesión autenticada. La página MUST presentar opciones claras para acceder al listado de movimientos y al formulario de creación de movimientos. Si un usuario sin sesión intenta acceder a `/home`, el sistema MUST redirigirlo a `/login`.

#### Scenario: Acceso autenticado a la página principal
- **WHEN** un usuario con un token de sesión accede a `/home`
- **THEN** el sistema muestra la página principal y las opciones para ver y crear movimientos

#### Scenario: Acceso no autenticado a la página principal
- **WHEN** un usuario sin un token de sesión accede a `/home`
- **THEN** el sistema lo redirige a `/login`

### Requirement: Navegación al listado de movimientos
El sistema MUST permitir que un usuario autenticado navegue desde `/home` a `/movimientos` mediante la opción de consulta de movimientos.

#### Scenario: Selección de movimientos desde Home
- **WHEN** un usuario autenticado selecciona la opción de ver sus movimientos
- **THEN** el sistema navega a `/movimientos`
