## Why

Después de autenticarse, el usuario no tiene una pantalla principal desde la que continuar en la aplicación ni puede consultar sus movimientos.

Esta funcionalidad habilita el primer flujo protegido de la aplicación, reutilizando el sistema de autenticación JWT ya existente y permitiendo al usuario acceder únicamente a los movimientos que la API autorice para su identidad.

## What Changes

* Añadir una página principal protegida en `/home` con acceso al listado de movimientos.

* Redirigir a `/home` después de un inicio de sesión correcto.

* Añadir una página protegida en `/movimientos` que consulte `GET /api/movimientos` y muestre la respuesta devuelta por el backend para el usuario autenticado.

* Incorporar estados de carga, listado vacío y error controlado en la página de movimientos.

* Añadir un guard de autenticación para `/home` y `/movimientos`, que redirija a `/login` cuando no exista un token de autenticación almacenado en la sesión del navegador.

* Reutilizar el JWT almacenado y el interceptor existente para las solicitudes protegidas.

* No aplicar filtrados por usuario en el frontend como mecanismo de seguridad.

* Antes de implementar el listado, verificar en el backend que `GET /api/movimientos` limita realmente los resultados utilizando la identidad asociada al JWT.

## Capabilities

### New Capabilities

* `authenticated-home`: Permite a un usuario autenticado acceder a una página principal y navegar desde ella al listado de movimientos.

* `user-movements-list`: Permite a un usuario autenticado consultar y visualizar los movimientos que la API autoriza para su identidad.

### Modified Capabilities

* `user-authentication`: El inicio de sesión correcto redirige al usuario autenticado a `/home`.

## Impact

* Afecta al enrutamiento Angular, al flujo posterior al login y al servicio de autenticación existente.

* Añade páginas, un guard, un servicio y contratos TypeScript dentro de `app_Fh_front/src/app`.

* Consume el endpoint protegido `GET /api/movimientos`.

* Reutiliza el interceptor HTTP existente para incorporar el JWT a las solicitudes protegidas dirigidas a la API.

* Requiere revisar el backend para confirmar que `GET /api/movimientos` devuelve únicamente movimientos autorizados para el usuario autenticado.

* La revisión del backend no autoriza cambios en él.

* No modifica el backend.

* No añade dependencias externas.

* No incluye creación, actualización ni eliminación de movimientos.

## Security Constraint

El frontend no debe utilizar propiedades como `userId` para ocultar movimientos de otros usuarios.

La autorización y separación de datos entre usuarios debe ser responsabilidad del backend.

Si durante la revisión se detecta que `GET /api/movimientos` devuelve movimientos pertenecientes a otros usuarios, la implementación deberá detenerse y señalar el problema antes de continuar.

## Expected Result

Al finalizar este cambio:

* un usuario que inicia sesión correctamente es redirigido a `/home`;

* un usuario sin token de autenticación que intenta acceder a `/home` o `/movimientos` es redirigido a `/login`;

* desde `/home`, el usuario puede acceder al listado de movimientos;

* `/movimientos` realiza una solicitud autenticada a `GET /api/movimientos`;

* el usuario ve únicamente los movimientos que el backend devuelve como autorizados para su identidad;

* si no existen movimientos se muestra un estado vacío comprensible;

* si ocurre un error se muestra un mensaje comprensible sin exponer detalles técnicos;

* no se crean, actualizan ni eliminan movimientos dentro de esta feature.
