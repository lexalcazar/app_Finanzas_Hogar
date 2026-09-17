## Why

La zona autenticada ofrece acciones funcionales, pero carece de una identidad visual y una navegación coherentes, y Home no presenta al usuario ni prioriza claramente sus acciones.

Se necesita una experiencia privada clara, moderna y reutilizable sin modificar el modelo de seguridad existente basado en JWT.

## What Changes

* Obtener de forma centralizada en `AuthService` el nombre de presentación contenido en el JWT, exclusivamente para fines de interfaz.

  El JWT actual contiene el nombre en el claim:

  `http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name`

* Añadir cierre de sesión local que elimine el token y cualquier estado local de autenticación relacionado y redirija a `/login`.

* Incorporar una barra superior reutilizable en las páginas protegidas, con:

  * identidad visual de la aplicación;
  * navegación a `/home` desde el logo o nombre;
  * acción de cierre de sesión.

* Rediseñar Home con:

  * saludo personalizado;
  * paleta neutra basada principalmente en blanco, negro y tonos grises;
  * tarjetas navegables para consultar y crear movimientos;
  * iconos;
  * diseño responsive y accesible.

* Alinear visualmente las pantallas públicas de inicio de sesión y registro con la identidad neutra, sin modificar su comportamiento.

* Centralizar la paleta neutra de todas las rutas existentes para que Home, autenticación, listado y creación mantengan la misma identidad.

* Aplicar la barra de navegación común a las páginas protegidas existentes:

  * Home;
  * listado de movimientos;
  * creación de movimientos.

* Mantener las rutas, guard, interceptor, API y modelo de autorización existentes.

* No añadir dependencias externas ni nuevos endpoints.

## Capabilities

### New Capabilities

* `authenticated-navigation`: Proporciona una barra común para el área privada, navegación hacia Home y cierre local de sesión.

### Modified Capabilities

* `user-authentication`: Expone desde el servicio de autenticación el nombre de presentación incluido en el JWT y permite finalizar la sesión local.

* `authenticated-home`: Personaliza y rediseña la página principal mediante saludo del usuario y tarjetas accesibles para las acciones privadas existentes.

## Impact

* Afecta a `AuthService`.

* Añade un componente reutilizable de navegación dentro de la estructura Angular convencional existente.

* Afecta visualmente a las páginas protegidas:

  * Home;
  * listado de movimientos;
  * creación de movimientos.

* Afecta visualmente a las páginas públicas Login y Register.

* Modifica la presentación y navegación de Home sin cambiar sus destinos funcionales actuales.

* No modifica el backend.

* No modifica contratos HTTP.

* No modifica el mecanismo de autorización del backend.

* No modifica el guard ni el interceptor salvo que sea estrictamente necesario para integrar el comportamiento existente.

* No crea nuevas rutas de negocio.

* No añade dependencias externas.

* No incluye edición ni eliminación de movimientos.
