## Why

La aplicación no ofrece todavía un punto de entrada para que los usuarios se autentiquen ni creen una cuenta. Se necesita integrar la interfaz Angular con los endpoints de autenticación existentes para permitir el acceso inicial de forma segura y comprensible.

## What Changes

* Añadir una vista de inicio de sesión con validación de correo y contraseña, estados de envío y mensajes de error controlados.

* Conectar el inicio de sesión con `POST /api/Auth/login` mediante rutas relativas `/api` y conservar el JWT recibido para mantener la sesión del usuario.

* Añadir una vista de registro accesible desde el inicio de sesión y conectarla con `POST /api/Auth/register`.

* Incorporar un servicio de autenticación y los contratos TypeScript necesarios para aislar la comunicación con la API de las páginas.

* Incorporar el mecanismo necesario para que las solicitudes que requieran autenticación incluyan automáticamente el JWT obtenido durante el inicio de sesión.

* Configurar las rutas públicas de autenticación y la navegación entre inicio de sesión y registro.

## Capabilities

### New Capabilities

* `user-authentication`: Permite a un usuario registrarse e iniciar sesión desde el frontend mediante los endpoints de autenticación de la API y utilizar el JWT obtenido en las posteriores solicitudes autenticadas.

### Modified Capabilities

* Ninguna.

## Impact

* Afecta a las rutas, la configuración HTTP global y el proxy de desarrollo de la aplicación Angular.

* Añade páginas, modelos y un servicio de autenticación dentro de `app_Fh_front/src/app`.

* Consume los endpoints públicos `POST /api/Auth/login` y `POST /api/Auth/register` del backend ASP.NET Core.

* No modifica el backend ni configura CORS; el proxy de desarrollo redirige `/api` a `http://localhost:5240`.

* No incorpora dependencias externas.
