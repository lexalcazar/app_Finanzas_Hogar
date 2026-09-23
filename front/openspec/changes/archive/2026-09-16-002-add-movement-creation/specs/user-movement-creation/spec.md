## Purpose

Permitir que un usuario autenticado registre un movimiento utilizando exactamente los datos aceptados por la API y continúe al listado de movimientos después de una creación correcta.

## ADDED Requirements

### Requirement: Acceso protegido a la creación de movimientos

El sistema SHALL ofrecer una ruta protegida para crear movimientos.

Un usuario sin token de autenticación almacenado en la sesión que intente acceder a dicha ruta MUST ser redirigido a `/login`.

#### Scenario: Acceso autenticado al formulario

* **WHEN** un usuario con token de autenticación accede a la ruta de creación
* **THEN** el sistema permite acceder al formulario de creación de movimiento

#### Scenario: Acceso no autenticado al formulario

* **WHEN** un usuario sin token de autenticación accede a la ruta de creación
* **THEN** el sistema lo redirige a `/login`

---

### Requirement: Formulario de creación de movimiento

El sistema SHALL ofrecer a un usuario autenticado un formulario para introducir:

* cantidad;
* descripción opcional;
* fecha;
* categoría.

El formulario MUST validar antes del envío que:

* la cantidad sea como mínimo `0.01`;
* la cantidad no sea superior a `999999999`;
* la fecha esté informada;
* se haya seleccionado una categoría válida;
* la descripción no supere los 250 caracteres.

El frontend MUST impedir el envío mientras el formulario sea inválido.

#### Scenario: Formulario válido

* **WHEN** el usuario introduce una cantidad válida, una fecha y una categoría válida, y la descripción no supera los 250 caracteres
* **THEN** el formulario permite iniciar la creación del movimiento

#### Scenario: Formulario inválido

* **WHEN** un usuario intenta enviar el formulario con una cantidad fuera del rango permitido, una fecha vacía, sin categoría válida o con una descripción superior a 250 caracteres
* **THEN** el sistema evita realizar la solicitud y muestra los errores de validación correspondientes

---

### Requirement: Selección de categoría

Al cargar el formulario, el sistema MUST obtener las categorías disponibles mediante:

`GET /api/Categorias`

La solicitud MUST utilizar el mecanismo de autenticación existente.

El usuario MUST seleccionar una de las categorías recibidas y el frontend utilizará su identificador como `categoriaId` al crear el movimiento.

#### Scenario: Categorías disponibles

* **WHEN** la API devuelve una o más categorías al cargar el formulario
* **THEN** el sistema las muestra para que el usuario pueda seleccionar una

#### Scenario: Sin categorías disponibles

* **WHEN** la API responde correctamente con una colección de categorías vacía
* **THEN** el sistema informa de que no existen categorías disponibles e impide la creación del movimiento

#### Scenario: Error al cargar categorías

* **WHEN** la solicitud de categorías falla
* **THEN** el sistema muestra un mensaje comprensible e impide enviar el formulario

---

### Requirement: Creación autenticada de movimiento

Cuando el formulario sea válido, el sistema MUST enviar una solicitud a:

`POST /api/Movimientos`

utilizando el mecanismo Bearer de autenticación existente.

El cuerpo de la solicitud MUST contener exclusivamente los datos requeridos por `CreateMovimientoDto`:

* `cantidad`;
* `descripcion`;
* `fecha`;
* `categoriaId`.

El frontend MUST NOT enviar un identificador de usuario.

El frontend MUST NOT enviar un campo `tipo`, ya que no forma parte del contrato `CreateMovimientoDto`.

#### Scenario: Creación exitosa

* **WHEN** un usuario autenticado envía datos válidos y la API acepta el movimiento
* **THEN** el sistema considera completada correctamente la creación y navega a `/movimientos`

#### Scenario: Creación rechazada

* **WHEN** la API rechaza la creación del movimiento
* **THEN** el sistema muestra un mensaje comprensible, permanece en el formulario y conserva los datos introducidos para permitir su corrección

#### Scenario: Error de comunicación durante la creación

* **WHEN** la solicitud no puede completarse debido a un error de comunicación o del servidor
* **THEN** el sistema muestra un mensaje comprensible, permanece en el formulario y conserva los datos introducidos

---

### Requirement: Estado de envío

Mientras se esté enviando un movimiento, el sistema MUST informar de que la operación está en curso e impedir envíos duplicados.

#### Scenario: Envío en curso

* **WHEN** un usuario envía un formulario válido y la API todavía no ha respondido
* **THEN** el sistema muestra el estado de envío y deshabilita un nuevo envío
