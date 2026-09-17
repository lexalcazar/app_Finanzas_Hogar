## Purpose

Permitir que los usuarios creen una cuenta e inicien sesión desde el frontend utilizando la API de autenticación, con validación de formularios, gestión de sesión y respuestas claras ante errores.

## Requirements

### Requirement: Inicio de sesión de usuario

El sistema SHALL ofrecer una ruta pública de inicio de sesión donde el usuario proporcione un correo electrónico y una contraseña.

El formulario MUST validar ambos campos antes de enviar la solicitud y MUST enviar las credenciales a `POST /api/Auth/login`.

Cuando la API responda correctamente con un token, el sistema MUST conservarlo durante la sesión actual del navegador para utilizarlo en posteriores solicitudes autenticadas y MUST navegar al usuario a `/home`. El sistema MUST poder obtener desde ese token el nombre de presentación contenido en el claim `http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name` exclusivamente para mostrarlo en la interfaz; esta lectura MUST NOT utilizarse para autorizar acciones ni sustituir la autorización del backend.

Cuando la autenticación falle o la solicitud no pueda completarse, el sistema MUST mostrar un mensaje comprensible sin exponer detalles técnicos y MUST NOT conservar un token procedente de la operación fallida.

#### Scenario: Inicio de sesión exitoso

* **WHEN** un usuario envía un correo electrónico y una contraseña válidos y la API responde correctamente con un token
* **THEN** el sistema conserva el token durante la sesión actual del navegador y navega a `/home`

#### Scenario: Nombre disponible para presentación

* **WHEN** existe un token de sesión cuyo payload incluye el claim de nombre configurado
* **THEN** el sistema expone ese nombre exclusivamente para su presentación en la interfaz

#### Scenario: Credenciales rechazadas

* **WHEN** un usuario envía el formulario de inicio de sesión y la API rechaza las credenciales
* **THEN** el sistema muestra un mensaje indicando que las credenciales no son correctas, no conserva ningún token procedente de la operación y mantiene al usuario en el formulario de inicio de sesión

#### Scenario: Formulario de inicio de sesión inválido

* **WHEN** un usuario intenta enviar el formulario sin un correo electrónico válido o sin contraseña
* **THEN** el sistema evita realizar la solicitud HTTP y muestra los errores de validación correspondientes

#### Scenario: Error al comunicar con la API

* **WHEN** un usuario intenta iniciar sesión y la solicitud no puede completarse por un error de comunicación o del servidor
* **THEN** el sistema muestra un mensaje comprensible, mantiene al usuario en el formulario y no conserva ningún token procedente de la operación

---

### Requirement: Presentación coherente de autenticación pública

Las rutas públicas `/login` y `/register` MUST utilizar una presentación limpia basada principalmente en blanco, negro y tonos grises, con tarjetas blancas, contraste suficiente, foco y hover visibles y diseño responsive. Esta presentación MUST NOT añadir la barra de navegación autenticada ni modificar las validaciones, servicios, endpoints o comportamiento de autenticación.

#### Scenario: Formularios públicos disponibles

* **WHEN** un visitante accede a `/login` o `/register`
* **THEN** puede utilizar el mismo formulario y navegación pública existentes con una presentación visual coherente y accesible

---

### Requirement: Registro de usuario

El sistema SHALL ofrecer una ruta pública de registro accesible desde la pantalla de inicio de sesión.

El formulario MUST solicitar:

* nombre;
* apellido;
* correo electrónico;
* contraseña.

El formulario MUST validar los campos requeridos y el formato del correo electrónico antes de enviar la solicitud a `POST /api/Auth/register`.

Cuando el registro sea aceptado, el sistema MUST informar al usuario de que la cuenta se ha creado correctamente y MUST dirigirlo al inicio de sesión.

Cuando la API rechace el registro o la solicitud no pueda completarse, el sistema MUST mostrar un mensaje comprensible y MUST conservar los datos introducidos para permitir su corrección.

#### Scenario: Registro exitoso

* **WHEN** un usuario envía datos de registro válidos y la API acepta la solicitud
* **THEN** el sistema informa de que la cuenta se ha creado correctamente y navega al inicio de sesión

#### Scenario: Registro rechazado por la API

* **WHEN** un usuario envía el formulario de registro y la API rechaza los datos recibidos
* **THEN** el sistema muestra un mensaje comprensible, mantiene al usuario en la pantalla de registro y conserva los datos introducidos para que puedan corregirse

#### Scenario: Formulario de registro inválido

* **WHEN** un usuario intenta enviar el formulario con campos requeridos vacíos o con un correo electrónico cuyo formato no es válido
* **THEN** el sistema evita realizar la solicitud HTTP y muestra los errores de validación correspondientes

#### Scenario: Error al comunicar con la API durante el registro

* **WHEN** un usuario intenta registrarse y la solicitud no puede completarse por un error de comunicación o del servidor
* **THEN** el sistema muestra un mensaje comprensible, mantiene al usuario en la pantalla de registro y conserva los datos introducidos

#### Scenario: Navegación a registro

* **WHEN** un visitante selecciona la opción de crear una cuenta desde la pantalla de inicio de sesión
* **THEN** el sistema navega a la ruta pública de registro

---

### Requirement: Protección de solicitudes autenticadas

El sistema MUST incluir el token de sesión como credencial Bearer en las solicitudes dirigidas a la API propia de la aplicación que requieran autenticación.

El sistema MUST NOT añadir esta credencial a las solicitudes públicas de inicio de sesión y registro.

El sistema MUST NOT enviar el token de autenticación a APIs o dominios externos.

Durante el desarrollo, las solicitudes relativas que comiencen con `/api` MUST redirigirse mediante el proxy Angular a `http://localhost:5240`. Los servicios Angular MUST usar rutas relativas y MUST NOT contener URLs completas del backend.

#### Scenario: Solicitud autenticada con token disponible

* **WHEN** la aplicación realiza una solicitud protegida dirigida a la API propia y existe un token de sesión
* **THEN** la solicitud incluye la cabecera `Authorization` utilizando el esquema `Bearer` y el token almacenado

#### Scenario: Solicitud pública de inicio de sesión

* **WHEN** la aplicación envía una solicitud a `POST /api/Auth/login`
* **THEN** la solicitud no incluye una cabecera `Authorization` añadida por el mecanismo de autenticación

#### Scenario: Solicitud pública de registro

* **WHEN** la aplicación envía una solicitud a `POST /api/Auth/register`
* **THEN** la solicitud no incluye una cabecera `Authorization` añadida por el mecanismo de autenticación

#### Scenario: Solicitud externa

* **WHEN** la aplicación realiza una solicitud HTTP a una API o dominio distinto de la API propia de la aplicación
* **THEN** el sistema no añade el token de autenticación a dicha solicitud

#### Scenario: Solicitud de desarrollo a la API

* **WHEN** la aplicación en desarrollo envía una solicitud relativa que comienza con `/api`
* **THEN** el proxy Angular la redirige a `http://localhost:5240` sin requerir CORS en el backend
