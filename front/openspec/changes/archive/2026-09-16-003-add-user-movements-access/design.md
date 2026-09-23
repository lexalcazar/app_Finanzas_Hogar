## Context

La autenticación existente conserva el JWT en `sessionStorage`, incorpora el token mediante un interceptor para solicitudes relativas a `/api` y expone rutas públicas de login y registro.

El backend protege `GET /api/movimientos` mediante JWT, obtiene la identidad del usuario desde el claim `NameIdentifier` y filtra los movimientos por `UsuarioId` antes de devolverlos.

Por tanto, la separación de movimientos entre usuarios está garantizada por el backend y el frontend no necesita ni debe aplicar filtros adicionales por usuario.

Véanse `proposal.md` y las especificaciones del cambio para los requisitos funcionales.

## Goals / Non-Goals

### Goals

* Añadir una página principal protegida y una navegación clara al listado de movimientos.

* Proteger `/home` y `/movimientos` basándose en la existencia del token de sesión ya gestionado por la aplicación.

* Consultar y mostrar el contrato real de movimientos sin filtrar datos por usuario en el cliente.

* Gestionar los estados de carga, vacío y error durante la consulta de movimientos.

* Reutilizar la infraestructura de autenticación e interceptor ya existente.

### Non-Goals

* No modificar el backend, sus reglas de autorización ni sus filtros de datos.

* No crear movimientos.

* No editar movimientos.

* No eliminar movimientos.

* No añadir filtros, búsquedas ni paginación.

* No añadir dashboard.

* No incorporar roles ni permisos adicionales.

* No añadir refresh tokens.

* No introducir una nueva solución global de estado.

* No implementar en este cambio una gestión global de expiración de sesión o respuestas `401`.

## Decisions

### Guard funcional basado en el token de sesión

Se añadirá un guard funcional reutilizando el servicio de autenticación existente.

El guard comprobará la existencia de un token almacenado en la sesión del navegador.

Si existe token, permitirá la navegación.

Si no existe token, devolverá un `UrlTree` hacia `/login`.

Las rutas `/home` y `/movimientos` utilizarán este guard.

No se verificará ni decodificará el JWT en el cliente para decidir el acceso.

El backend continúa siendo la autoridad responsable de validar el token y determinar los permisos reales del usuario.

Se descarta incorporar en esta feature lógica de roles, expiración, refresh token o validación criptográfica del JWT en Angular.

---

### Páginas standalone y rutas explícitas

`Home` y el listado de movimientos serán páginas standalone asociadas respectivamente a:

* `/home`
* `/movimientos`

`Home` tendrá una responsabilidad sencilla: actuar como punto de entrada posterior al login y permitir la navegación al listado de movimientos.

La página de movimientos será responsable de coord
