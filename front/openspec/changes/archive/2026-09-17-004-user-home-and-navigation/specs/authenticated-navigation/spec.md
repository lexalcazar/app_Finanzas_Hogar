## Purpose

Proporcionar una navegación común, clara y accesible para las páginas privadas de la aplicación, personalizar la experiencia del usuario autenticado y mejorar visualmente la página Home sin modificar el modelo de autorización existente.

## ADDED Requirements

### Requirement: Barra de navegación autenticada reutilizable

El sistema SHALL mostrar una barra superior común en las páginas protegidas existentes:

- `/home`
- `/movimientos`
- `/movimientos/nuevo`

La barra MUST presentar una identidad visual de la aplicación en su zona izquierda y una acción accesible para cerrar sesión en su zona derecha.

La identidad visual completa MUST ser navegable a `/home`.

La barra MUST implementarse como un componente reutilizable y no duplicarse de forma independiente en cada página.

#### Scenario: Barra visible en Home

- **WHEN** un usuario autenticado accede a `/home`
- **THEN** el sistema muestra la barra de navegación autenticada

#### Scenario: Barra visible en listado de movimientos

- **WHEN** un usuario autenticado accede a `/movimientos`
- **THEN** el sistema muestra la misma barra de navegación autenticada

#### Scenario: Barra visible en creación de movimiento

- **WHEN** un usuario autenticado accede a `/movimientos/nuevo`
- **THEN** el sistema muestra la misma barra de navegación autenticada

#### Scenario: Regreso a Home desde una página privada

- **WHEN** un usuario autenticado activa la identidad visual de la barra desde una página protegida
- **THEN** el sistema navega a `/home`

---

### Requirement: Cierre de sesión local

El sistema SHALL ofrecer una acción accesible de cierre de sesión desde la barra de navegación.

Al cerrar sesión, el sistema MUST eliminar el token almacenado en `sessionStorage` y cualquier otro estado local de autenticación asociado.

El sistema MUST navegar posteriormente a `/login`.

El sistema MUST NOT llamar a un endpoint de logout del backend.

La lectura o eliminación del JWT en el frontend MUST NOT utilizarse como mecanismo de autorización.

#### Scenario: Cierre de sesión desde la barra

- **WHEN** un usuario autenticado activa la acción de cerrar sesión
- **THEN** el sistema elimina su token de sesión y navega a `/login`

#### Scenario: Acceso protegido después de cerrar sesión

- **WHEN** un usuario ha cerrado sesión e intenta acceder a una ruta protegida
- **THEN** el guard existente detecta la ausencia de token y lo redirige a `/login`

---

### Requirement: Iconos de interfaz sin nuevas dependencias

La barra de navegación y las tarjetas de Home SHALL utilizar iconos cuando ayuden a identificar visualmente las acciones.

El sistema MUST NOT añadir una nueva dependencia externa únicamente para proporcionar estos iconos.

Los iconos MUST disponer de un uso accesible y no depender exclusivamente de su representación visual para comunicar una acción.

#### Scenario: Acción acompañada de icono

- **WHEN** una acción de navegación incorpora un icono
- **THEN** el usuario puede comprender y activar igualmente la acción mediante su texto, etiqueta accesible o contexto correspondiente

## MODIFIED Requirements

### Requirement: Información de presentación del usuario autenticado

`AuthService` SHALL proporcionar el nombre de presentación del usuario a partir del JWT almacenado.

El nombre MUST obtenerse del claim existente:

`http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name`

La lectura del claim MUST utilizarse exclusivamente para presentación en la interfaz.

El frontend MUST NOT utilizar el nombre ni otros claims decodificados como mecanismo de autorización.

Si el token no existe, no puede decodificarse o no contiene el claim esperado, el sistema MUST manejar la situación sin provocar un error de ejecución.

#### Scenario: Nombre disponible en el JWT

- **WHEN** existe un JWT almacenado que contiene el claim de nombre
- **THEN** `AuthService` proporciona ese nombre para su uso en la interfaz

#### Scenario: Nombre no disponible

- **WHEN** el JWT no contiene un nombre utilizable o no puede decodificarse
- **THEN** la interfaz continúa funcionando sin mostrar información incorrecta ni producir un error

---

### Requirement: Home autenticada personalizada

La página `/home` SHALL mostrar un saludo personalizado utilizando el nombre proporcionado por `AuthService` cuando esté disponible.

El saludo deberá ser equivalente a:

`Bienvenido, Cristina`

para un usuario cuyo nombre de presentación sea `Cristina`.

#### Scenario: Saludo personalizado

- **WHEN** un usuario autenticado accede a `/home` y su nombre está disponible
- **THEN** el sistema muestra un saludo que incluye su nombre

---

### Requirement: Acciones de Home mediante tarjetas

Las acciones existentes de Home SHALL presentarse como tarjetas navegables.

Las tarjetas existentes serán:

- `Mis movimientos`, con destino `/movimientos`
- `Crear movimiento`, con destino `/movimientos/nuevo`

Cada tarjeta MUST incluir:

- un icono;
- un título;
- una breve descripción;
- un área completa de interacción.

Las tarjetas MUST poder utilizarse mediante ratón y teclado y MUST presentar estados visibles de `hover` y `focus`.

#### Scenario: Acceso al listado mediante tarjeta

- **WHEN** el usuario activa la tarjeta `Mis movimientos`
- **THEN** el sistema navega a `/movimientos`

#### Scenario: Acceso a creación mediante tarjeta

- **WHEN** el usuario activa la tarjeta `Crear movimiento`
- **THEN** el sistema navega a `/movimientos/nuevo`

#### Scenario: Navegación mediante teclado

- **WHEN** una tarjeta recibe el foco mediante navegación por teclado y el usuario la activa
- **THEN** el sistema ejecuta la misma navegación que mediante interacción con ratón

---

### Requirement: Rediseño visual de Home

La página Home SHALL utilizar una estética neutra basada principalmente en:

- blanco;
- negro;
- gris oscuro;
- gris medio;
- gris claro.

La interfaz MUST mantener contraste suficiente entre texto, fondos y elementos interactivos.

El diseño MUST ser responsive y adaptarse a diferentes tamaños de pantalla.

La nueva presentación MUST sustituir el uso predominante del color verde de la Home actual.

#### Scenario: Visualización en pantalla amplia

- **WHEN** Home se muestra en una pantalla con espacio suficiente
- **THEN** las tarjetas se distribuyen de forma clara aprovechando el espacio disponible

#### Scenario: Visualización en pantalla estrecha

- **WHEN** Home se muestra en una pantalla estrecha
- **THEN** la distribución se adapta sin pérdida de contenido ni funcionalidad