## Purpose

Permitir que un usuario autenticado consulte los movimientos autorizados para su identidad, con estados claros durante la carga y ante la ausencia de datos o errores.

## Requirements

### Requirement: Acceso protegido al listado de movimientos

El sistema SHALL ofrecer la ruta `/movimientos` únicamente a usuarios que dispongan de un token de autenticación almacenado en la sesión del navegador.

Si un usuario sin token intenta acceder a esta ruta, el sistema MUST redirigirlo a `/login`.

#### Scenario: Acceso autenticado al listado

* **WHEN** un usuario con un token de autenticación almacenado accede a `/movimientos`
* **THEN** el sistema permite el acceso y comienza la consulta de movimientos

#### Scenario: Acceso no autenticado al listado

* **WHEN** un usuario sin token de autenticación accede a `/movimientos`
* **THEN** el sistema lo redirige a `/login`

---

### Requirement: Consulta de movimientos autorizados

Al acceder a `/movimientos`, el sistema MUST solicitar los datos mediante:

`GET /api/movimientos`

El sistema MUST mostrar únicamente los movimientos devueltos por la API.

El frontend MUST NOT filtrar movimientos mediante un identificador de usuario como mecanismo de seguridad.

La autorización y separación de movimientos entre usuarios corresponde al backend utilizando la identidad asociada al JWT.

#### Scenario: Listado con movimientos

* **WHEN** un usuario autenticado accede a `/movimientos` y la API devuelve uno o más movimientos
* **THEN** el sistema muestra los datos de cada movimiento devuelto

#### Scenario: Solicitud autenticada de movimientos

* **WHEN** el sistema solicita `GET /api/movimientos` y existe un token de autenticación almacenado
* **THEN** la solicitud incluye la credencial `Authorization: Bearer <token>` mediante el mecanismo de autenticación existente

#### Scenario: Sin filtrado de usuario en frontend

* **WHEN** la API devuelve la colección de movimientos autorizados
* **THEN** el frontend representa dicha colección sin utilizar un `userId` como mecanismo adicional de autorización

---

### Requirement: Estados del listado de movimientos

El sistema MUST informar al usuario mientras la consulta de movimientos está en curso.

Si la API devuelve una colección vacía, el sistema MUST mostrar un estado vacío comprensible.

Si la consulta falla, el sistema MUST mostrar un mensaje comprensible sin exponer detalles técnicos de la aplicación o de la API.

#### Scenario: Carga de movimientos

* **WHEN** se ha iniciado la solicitud de movimientos y todavía no se ha recibido una respuesta
* **THEN** el sistema muestra un estado que indica que los movimientos se están cargando

#### Scenario: Listado vacío

* **WHEN** la API responde correctamente con una colección de movimientos vacía
* **THEN** el sistema informa de que no hay movimientos para mostrar

#### Scenario: Error al obtener movimientos

* **WHEN** la solicitud de movimientos falla
* **THEN** el sistema muestra un mensaje controlado de error sin exponer detalles técnicos

---

### Requirement: Consulta filtrada de movimientos

El listado MUST permitir consultas con filtros soportados por la API y mostrar sólo la respuesta resultante. El frontend MUST NOT filtrar por usuario ni aplicar en memoria filtros que la API recibe.

#### Scenario: Resultado filtrado vacío
- **WHEN** una consulta filtrada no devuelve movimientos
- **THEN** el sistema muestra el estado vacío existente
