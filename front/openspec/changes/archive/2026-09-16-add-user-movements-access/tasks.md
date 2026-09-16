## 1. Rutas y acceso autenticado

* [x] 1.1 Confirmar que el backend protege `GET /api/movimientos` mediante JWT y limita los resultados al `UsuarioId` obtenido de la identidad autenticada, sin modificar el backend.

* [x] 1.2 Crear un guard funcional que permita el acceso a rutas protegidas cuando exista un token de autenticación en la sesión y redirija a `/login` en caso contrario.

* [x] 1.3 Crear la página standalone `Home` con una opción accesible para navegar al listado de movimientos.

* [x] 1.4 Configurar las rutas protegidas `/home` y `/movimientos` utilizando el guard, manteniendo las rutas públicas existentes.

* [x] 1.5 Modificar el flujo de login correcto para navegar a `/home` después de almacenar correctamente el token.

## 2. Listado de movimientos

* [x] 2.1 Crear la interfaz TypeScript que refleje exactamente `MovimientoResponseDto`, sin añadir campos relacionados con la identidad del usuario que no formen parte del contrato.

* [x] 2.2 Implementar un servicio de movimientos que realice `GET /api/movimientos` utilizando la infraestructura HTTP existente y la ruta relativa `/api/movimientos`.

* [x] 2.3 Crear la página standalone de movimientos utilizando estado local para representar:

  * carga;
  * listado con datos;
  * listado vacío;
  * error.

* [x] 2.4 Mostrar los campos disponibles de cada movimiento devuelto por la API sin realizar filtrados por usuario en el frontend.

* [x] 2.5 Mantener el listado limitado a la responsabilidad de consulta y visualización, sin incorporar creación, edición o eliminación de movimientos.

* [x] 2.6 Aplicar estilos responsivos y accesibles coherentes con las páginas existentes a `Home` y al listado de movimientos.

## 3. Pruebas y verificación

* [x] 3.1 Añadir pruebas del guard verificando:

  * acceso cuando existe token;
  * redirección a `/login` cuando no existe token.

* [x] 3.2 Añadir una prueba de navegación desde `Home` hacia `/movimientos`.

* [x] 3.3 Añadir pruebas del servicio de movimientos verificando:

  * solicitud `GET /api/movimientos`;
  * ausencia de parámetros o filtros de usuario en la solicitud.

* [x] 3.4 Añadir pruebas de la página de movimientos para:

  * estado de carga;
  * listado con datos;
  * listado vacío;
  * estado de error.

* [x] 3.5 Actualizar las pruebas de login para verificar la navegación a `/home` después de una autenticación correcta y del almacenamiento del token.

* [x] 3.6 Verificar que el interceptor existente incorpora correctamente `Authorization: Bearer <token>` en `GET /api/movimientos`.

* [x] 3.7 Verificar que un usuario sin token no puede acceder a `/home` ni a `/movimientos` y es redirigido a `/login`.

* [x] 3.8 Ejecutar las pruebas configuradas del proyecto y corregir cualquier error relacionado con el cambio.

* [x] 3.9 Ejecutar `ng build` desde `/front/app_Fh_front` y corregir cualquier error antes de considerar completado el cambio.
