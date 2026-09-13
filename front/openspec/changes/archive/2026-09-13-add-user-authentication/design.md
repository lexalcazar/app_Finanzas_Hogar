## Context

La aplicación Angular actual utiliza componentes standalone. Antes de implementar el cambio se deberá comprobar la configuración real existente dentro de `app_Fh_front`, incluyendo rutas, proveedores HTTP y estructura de carpetas, para reutilizar lo que ya exista y evitar configuraciones paralelas.

El backend ya expone endpoints públicos de autenticación.

Los contratos existentes son:

* `POST /api/Auth/login`

  * Request: `{ email, password }`
  * Response: `{ token }`

* `POST /api/Auth/register`

  * Request: `{ nombre, apellido, email, password }`
  * Response: mensaje de confirmación

Véase `proposal.md` para la motivación del cambio y `specs/user-authentication/spec.md` para el comportamiento requerido.

---

## Goals / Non-Goals

### Goals

* Incorporar formularios de inicio de sesión y registro con validación reactiva.

* Mostrar estados de carga y mensajes de error controlados durante las operaciones de autenticación.

* Centralizar los contratos y las llamadas HTTP relacionadas con autenticación en un servicio específico.

* Conservar el JWT durante la sesión del navegador.

* Incorporar automáticamente el JWT a las solicitudes dirigidas a la API que requieran autenticación.

* Centralizar la URL base de la API mediante la configuración de entorno.

* Configurar las rutas públicas de inicio de sesión y registro.

* Mantener la implementación compatible con la estructura y arquitectura Angular ya existentes.

* No modificar el backend.

### Non-Goals

* No implementar recuperación de contraseña.

* No implementar todavía una interfaz de cierre de sesión.

* No implementar perfiles de usuario.

* No implementar refresh tokens.

* No implementar autenticación mediante proveedores externos o redes sociales.

* No crear un sistema de roles o permisos adicional.

* No proteger rutas de negocio que todavía no formen parte de este cambio.

* No crear una nueva página privada únicamente como destino posterior al inicio de sesión.

* No modificar los contratos, validaciones ni persistencia existentes en el backend.

---

## Decisions

### Formularios reactivos en páginas standalone

Las páginas `login` y `register` utilizarán Reactive Forms.

Ambos formularios requieren:

* validación explícita de campos;
* control del estado de envío;
* presentación de errores;
* gestión de una operación HTTP asíncrona.

Reactive Forms permite mantener estas responsabilidades de forma clara y fácilmente testeable.

Las páginas seguirán siendo componentes standalone, respetando la arquitectura actual del proyecto.

Se descartan Template-Driven Forms para estas pantallas porque el control explícito sobre validaciones y estados resulta más apropiado para este caso.

---

### Servicio y contratos de autenticación aislados

Los requests y responses de autenticación se modelarán mediante interfaces TypeScript que reflejen los contratos reales del backend.

Como mínimo se contemplarán los contratos necesarios para:

* login;
* registro;
* respuesta del login.

`AuthService` será responsable de:

* realizar las llamadas HTTP de autenticación;
* gestionar el acceso al JWT almacenado;
* ofrecer las operaciones relacionadas directamente con el estado de autenticación.

Las páginas serán responsables únicamente de:

* gestionar los formularios;
* presentar estados y errores;
* invocar al servicio;
* coordinar la navegación correspondiente.

No se realizarán llamadas HTTP directamente desde las páginas.

Tampoco se incorporarán al servicio responsabilidades propias de presentación, como mostrar mensajes visuales o gestionar directamente formularios.

---

### URL base y HttpClient centralizados

La URL base de la API se almacenará en la configuración de entorno del proyecto Angular como la ruta relativa `/api`.

Los servicios construirán sus endpoints utilizando esa URL base.

Los componentes y servicios no contendrán URLs completas del backend. En desarrollo, el dev server Angular usará una configuración de proxy que redirige `/api` a `http://localhost:5240`; esta configuración no formará parte del build de producción y no requiere cambios de CORS en el backend.

Antes de añadir nueva configuración de `HttpClient`, se comprobará si el proyecto ya dispone de proveedores HTTP configurados.

Si ya existen, se reutilizarán.

Si no existen, se habilitará `HttpClient` mediante los proveedores recomendados por la versión actual de Angular utilizada en el proyecto.

Esta decisión evita duplicaciones y facilita cambiar entre distintos entornos de desarrollo o despliegue.

---

### Token por sesión

El JWT recibido tras un inicio de sesión correcto se almacenará en `sessionStorage`.

Se utilizará una única clave de autenticación definida de forma centralizada.

`sessionStorage` permite que el token permanezca disponible después de recargar la página, pero limita su persistencia a la sesión actual del navegador.

Esta decisión reduce la duración temporal de la persistencia frente al uso de `localStorage`.

No obstante, `sessionStorage` no protege el token frente a vulnerabilidades XSS, ya que sigue siendo accesible mediante JavaScript ejecutado en el mismo origen.

Por este motivo:

* el token no se mostrará en la interfaz;
* el token no se escribirá en logs de consola;
* no se almacenará información sensible adicional junto al token;
* se evitará cualquier uso innecesario del valor del JWT fuera del servicio de autenticación y del interceptor.

Se descarta `localStorage` para esta primera implementación con el objetivo de no mantener la sesión entre sesiones independientes del navegador.

---

### Interceptor funcional para solicitudes autenticadas

Se utilizará un interceptor HTTP funcional de Angular.

Su responsabilidad será añadir:

`Authorization: Bearer <token>`

a las solicitudes que:

* estén dirigidas a la API propia de la aplicación;
* requieran autenticación;
* dispongan de un JWT almacenado.

Los endpoints públicos de autenticación deberán quedar excluidos expresamente:

* `/api/Auth/login`
* `/api/Auth/register`

El interceptor no debe añadir el JWT indiscriminadamente a cualquier petición HTTP externa que pudiera incorporarse en el futuro.

Esto evita enviar accidentalmente credenciales de autenticación a dominios o APIs ajenos a la aplicación.

Se descarta añadir manualmente la cabecera `Authorization` desde cada servicio, ya que supondría duplicar una responsabilidad transversal.

---

### Rutas públicas de autenticación

Se crearán las rutas públicas:

* `/login`
* `/register`

La página de login permitirá navegar a la página de registro.

La página de registro permitirá volver al inicio de sesión.

Tras un registro correcto, el usuario será redirigido a:

`/login`

para que pueda iniciar sesión con sus nuevas credenciales.

---

### Estado posterior al inicio de sesión

Tras un inicio de sesión correcto se conservará el token y se mostrará en la misma pantalla un estado visual de autenticación completada. No se navegará a `/` ni se creará una página privada únicamente como destino de esta feature. El destino definitivo se definirá cuando exista una pantalla de negocio adecuada, como un dashboard o un listado principal de movimientos.

---

### Sin guard de autenticación en este cambio

No se implementará todavía un guard de autenticación.

Actualmente este cambio no introduce nuevas rutas privadas de negocio que necesiten protección.

Cuando se cree la primera pantalla que deba ser accesible únicamente por usuarios autenticados, se podrá incorporar un guard utilizando el mismo `AuthService` y el estado del JWT existente.

Esto evita añadir infraestructura que todavía no es necesaria.

---

### Respeto de la estructura Angular existente

Antes de crear archivos o modificar configuración, se inspeccionará:

* `app.routes.ts`;
* `app.config.ts`;
* servicios existentes;
* modelos existentes;
* configuración de entornos;
* proveedores HTTP;
* estructura actual de carpetas.

Si alguno de estos elementos ya existe, deberá reutilizarse.

No se crearán estructuras paralelas ni configuraciones duplicadas.

El código de Angular permanecerá dentro de:

`/front/app_Fh_front`

Las features definidas mediante OpenSpec no implican crear una carpeta `features` dentro de Angular.

---

## Risks / Trade-offs

* **El token almacenado en el navegador puede quedar expuesto ante una vulnerabilidad XSS.**
  `sessionStorage` limita la duración de la persistencia, pero no elimina este riesgo. Se evitará exponer, registrar o utilizar el token fuera de los puntos necesarios de autenticación.

* **La API puede devolver errores de registro provenientes de ASP.NET Core Identity con una estructura que no forma parte de un contrato estable del frontend.**
  Cuando sea posible extraer un mensaje seguro y comprensible se podrá mostrar. En caso contrario se utilizará un mensaje genérico controlado.

* **No existe todavía una pantalla de negocio a la que navegar tras el login.**
  La pantalla de login confirmará la autenticación sin redirigir. Una futura especificación definirá el destino definitivo.

* **El interceptor podría enviar el token a una solicitud no deseada si solo se comprueba la ausencia de los endpoints públicos.**
  Por ello deberá comprobar también que la solicitud pertenece a la API propia antes de añadir la cabecera `Authorization`.

* **Los contratos del backend pueden cambiar en el futuro.**
  Los modelos TypeScript deben reflejar los DTOs reales del backend y no depender de propiedades inventadas o inferidas.

---

## Migration Plan

1. Revisar la estructura y configuración existentes dentro de `app_Fh_front`.

2. Añadir o reutilizar la configuración de entorno para la ruta base relativa `/api` y configurar el proxy solo para desarrollo hacia `http://localhost:5240`.

3. Añadir o reutilizar la configuración de `HttpClient`.

4. Crear los contratos TypeScript necesarios para login y registro.

5. Crear `AuthService` utilizando los contratos reales del backend.

6. Incorporar el almacenamiento del JWT mediante `sessionStorage`.

7. Crear y registrar el interceptor funcional para las solicitudes autenticadas.

8. Crear las páginas standalone de login y registro utilizando Reactive Forms.

9. Configurar o ampliar las rutas existentes con `/login` y `/register`.

10. Verificar manualmente:

    * registro correcto;
    * error de registro;
    * login correcto;
    * login incorrecto;
    * persistencia del token tras recargar la página;
    * confirmación de autenticación sin redirección tras login;
    * redirección tras registro;
    * envío de la cabecera Bearer a un endpoint protegido;
    * ausencia de Bearer en login y registro.

11. Ejecutar la compilación Angular y los tests relacionados antes de considerar completado el cambio.

Si fuese necesario revertir el cambio, se podrán retirar las rutas, páginas, servicio, contratos, interceptor y proveedores añadidos.

No existen migraciones de base de datos ni cambios persistentes en el servidor asociados a esta implementación.
