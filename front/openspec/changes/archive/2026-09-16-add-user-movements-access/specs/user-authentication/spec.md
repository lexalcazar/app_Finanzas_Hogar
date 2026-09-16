## MODIFIED Requirements

### Requirement: Inicio de sesión de usuario
El sistema SHALL ofrecer una ruta pública de inicio de sesión donde el usuario proporcione un correo electrónico y una contraseña.

El formulario MUST validar ambos campos antes de enviar la solicitud y MUST enviar las credenciales a `POST /api/Auth/login`.

Cuando la API responda correctamente con un token, el sistema MUST conservarlo durante la sesión actual del navegador para utilizarlo en posteriores solicitudes autenticadas y MUST navegar al usuario a `/home`.

Cuando la autenticación falle o la solicitud no pueda completarse, el sistema MUST mostrar un mensaje comprensible sin exponer detalles técnicos y MUST NOT conservar un token procedente de la operación fallida.

#### Scenario: Inicio de sesión exitoso

* **WHEN** un usuario envía un correo electrónico y una contraseña válidos y la API responde correctamente con un token
* **THEN** el sistema conserva el token durante la sesión actual del navegador y navega a `/home`

#### Scenario: Credenciales rechazadas

* **WHEN** un usuario envía el formulario de inicio de sesión y la API rechaza las credenciales
* **THEN** el sistema muestra un mensaje indicando que las credenciales no son correctas, no conserva ningún token procedente de la operación y mantiene al usuario en el formulario de inicio de sesión

#### Scenario: Formulario de inicio de sesión inválido

* **WHEN** un usuario intenta enviar el formulario sin un correo electrónico válido o sin contraseña
* **THEN** el sistema evita realizar la solicitud HTTP y muestra los errores de validación correspondientes

#### Scenario: Error al comunicar con la API

* **WHEN** un usuario intenta iniciar sesión y la solicitud no puede completarse por un error de comunicación o del servidor
* **THEN** el sistema muestra un mensaje comprensible, mantiene al usuario en el formulario y no conserva ningún token procedente de la operación
