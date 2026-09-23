## MODIFIED Requirements

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
