## 1. Configuración de autenticación

* [x] 1.1 Revisar la configuración Angular existente y configurar `HttpClient` únicamente si todavía no está habilitado.

* [x] 1.2 Configurar la ruta base relativa `/api` mediante el entorno Angular y un proxy exclusivo de desarrollo hacia `http://localhost:5240`, evitando URLs completas del backend en componentes o servicios.

* [x] 1.3 Crear los contratos TypeScript tipados para:

  * solicitud de login;
  * respuesta de login con token;
  * solicitud de registro;
    respetando exactamente los contratos existentes del backend.

* [x] 1.4 Implementar `AuthService` para registrar usuarios, iniciar sesión y gestionar el token mediante `sessionStorage`.

* [x] 1.5 Implementar y registrar un interceptor HTTP funcional que:

  * añada `Authorization: Bearer <token>` a las solicitudes protegidas dirigidas a la API propia;
  * no añada el token a `/api/Auth/login`;
  * no añada el token a `/api/Auth/register`;
  * no envíe el token a APIs o dominios externos.

## 2. Vistas y navegación pública

* [x] 2.1 Crear la página standalone de inicio de sesión utilizando Reactive Forms, con:

  * validación de correo electrónico;
  * validación de contraseña requerida;
  * estado de carga;
  * mensajes de validación;
  * manejo comprensible de credenciales incorrectas;
  * manejo de errores de comunicación con la API.

* [x] 2.2 Crear la página standalone de registro utilizando Reactive Forms para:

  * nombre;
  * apellido;
  * correo electrónico;
  * contraseña;
    incluyendo validación, estado de carga y manejo comprensible de errores.

* [x] 2.3 Configurar las rutas públicas `/login` y `/register`, reutilizando la configuración de rutas existente si ya está presente.

* [x] 2.4 Implementar la navegación desde inicio de sesión hacia registro y desde registro hacia inicio de sesión.

* [x] 2.5 Tras un inicio de sesión correcto, conservar el token y mostrar un estado de autenticación completada sin redirigir ni crear una pantalla de negocio.

* [x] 2.6 Tras un registro correcto, informar al usuario de que la cuenta se ha creado y navegar a `/login`.

* [x] 2.7 Aplicar estilos responsivos, claros y accesibles a las páginas de login y registro, manteniendo la coherencia con los estilos existentes cuando los haya.

## 3. Verificación

* [x] 3.1 Añadir pruebas unitarias relevantes para `AuthService`, incluyendo:

  * petición de login;
  * petición de registro;
  * almacenamiento del token;
  * recuperación del token desde `sessionStorage`.

* [x] 3.2 Añadir pruebas relevantes para el interceptor verificando que:

  * añade el Bearer token a una solicitud protegida dirigida a la API propia;
  * no añade el token al login;
  * no añade el token al registro;
  * no añade el token a una solicitud dirigida a una API o dominio externo;
  * no añade una cabecera Bearer cuando no existe token almacenado.

* [x] 3.3 Añadir pruebas relevantes para los comportamientos significativos de los formularios de login y registro, especialmente validación y bloqueo de solicitudes con formularios inválidos.

* [x] 3.4 Verificar el flujo de inicio de sesión con:

  * credenciales válidas;
  * credenciales incorrectas;
  * error de comunicación con la API;
  * almacenamiento del token;
  * confirmación de autenticación sin redirección posterior al login.

* [x] 3.5 Verificar el flujo de registro con:

  * datos válidos;
  * datos rechazados por la API;
  * formulario inválido;
  * error de comunicación;
  * redirección posterior al registro.

* [x] 3.6 Verificar que una solicitud protegida realizada después del login incluye correctamente la cabecera `Authorization: Bearer <token>`.

* [x] 3.7 Ejecutar las pruebas configuradas del proyecto y corregir cualquier error relacionado con el cambio.

* [x] 3.8 Ejecutar `ng build` desde el proyecto Angular `/front/app_Fh_front` y corregir cualquier error antes de considerar completada la implementación.
