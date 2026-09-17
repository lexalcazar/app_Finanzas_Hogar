## 1. Sesión y autenticación local

- [x] 1.1 Ampliar `AuthService` para decodificar defensivamente el payload JWT y exponer el nombre de presentación obtenido del claim:

  `http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name`

  exclusivamente para fines de interfaz.

- [x] 1.2 La lectura del JWT debe:
  - manejar token ausente;
  - manejar token malformado;
  - decodificar correctamente Base64URL;
  - soportar contenido UTF-8;
  - devolver `null` cuando el nombre no pueda obtenerse;
  - no utilizar los claims decodificados como mecanismo de autorización.

- [x] 1.3 Implementar en `AuthService` una operación de logout local que elimine:
  - el token almacenado en `sessionStorage`;
  - cualquier estado local de autenticación relacionado.

  `AuthService` no debe realizar navegación ni peticiones HTTP de logout.

- [x] 1.4 Añadir pruebas de `AuthService` para:
  - nombre válido;
  - token ausente;
  - token malformado;
  - claim de nombre ausente;
  - limpieza del token mediante logout.

## 2. Navegación privada compartida

- [x] 2.1 Revisar la estructura actual de `src/app` y reutilizar `components` si ya existe. Si no existe, crear la carpeta convencional necesaria para alojar el componente compartido.

- [x] 2.2 Crear un componente standalone reutilizable para la barra superior autenticada.

- [x] 2.3 Implementar en la zona izquierda:
  - identidad visual de la aplicación;
  - logo o símbolo;
  - nombre de la aplicación;
  - navegación completa a `/home` mediante Angular Router.

- [x] 2.4 Implementar en la zona derecha una acción accesible de cierre de sesión que:
  1. invoque `AuthService.logout()`;
  2. navegue posteriormente a `/login`.

- [x] 2.5 Integrar la barra en:
  - Home;
  - listado de movimientos;
  - creación de movimientos;

  sin duplicar su markup.

- [x] 2.6 No mostrar la barra en las páginas públicas de login y registro.

- [x] 2.7 Aplicar estilos responsive, contraste suficiente y foco visible a la barra y sus controles.

- [x] 2.8 Añadir pruebas del componente para:
  - navegación a `/home`;
  - ejecución del logout;
  - navegación posterior a `/login`.

## 3. Home personalizada

- [x] 3.1 Obtener el nombre de presentación mediante `AuthService`.

- [x] 3.2 Mostrar un saludo personalizado cuando exista nombre:

  `Bienvenido, Cristina`

- [x] 3.3 Mostrar un saludo genérico comprensible cuando el nombre no esté disponible:

  `Bienvenido`

- [x] 3.4 Sustituir las acciones actuales por tarjetas independientes para:
  - `Mis movimientos` → `/movimientos`;
  - `Crear movimiento` → `/movimientos/nuevo`.

- [x] 3.5 Cada tarjeta debe incluir:
  - SVG inline;
  - título;
  - breve descripción;
  - área completa navegable.

- [x] 3.6 Implementar las tarjetas mediante elementos semánticos de navegación y `routerLink`, evitando manejadores `click` innecesarios.

- [x] 3.7 Garantizar que las tarjetas puedan utilizarse mediante:
  - ratón;
  - teclado;
  - foco visible.

- [x] 3.8 Rediseñar Home utilizando principalmente:
  - blanco;
  - negro;
  - gris oscuro;
  - gris medio;
  - gris claro.

- [x] 3.9 Eliminar el verde como color predominante y aplicar:
  - jerarquía visual clara;
  - contraste suficiente;
  - estados hover;
  - estados focus;
  - layout responsive.

- [x] 3.10 Actualizar las pruebas de Home para:
  - saludo con nombre;
  - saludo sin nombre;
  - contenido de ambas tarjetas;
  - navegación a `/movimientos`;
  - navegación a `/movimientos/nuevo`.

## 4. Verificación

- [x] 4.1 Verificar mediante pruebas que después del logout el token ya no existe en `sessionStorage`.

- [x] 4.2 Verificar mediante pruebas que, después del logout, el guard existente redirige una ruta protegida a `/login`.

- [x] 4.3 Verificar que Home, movimientos y crear movimiento utilizan el mismo componente de navegación y no implementaciones duplicadas.

- [x] 4.4 Verificar que no se han añadido dependencias externas para los iconos.

- [x] 4.5 Ejecutar `npm test` y corregir únicamente fallos relacionados con el cambio.

- [x] 4.6 Ejecutar `npm run build` y corregir únicamente fallos relacionados con el cambio.

## 5. Coherencia visual de autenticación pública

- [x] 5.1 Ajustar los estilos comunes de Login y Register a la identidad visual neutra sin modificar su lógica funcional ni añadir la barra autenticada.
- [x] 5.2 Mantener contraste, foco, hover y responsive en los formularios públicos mediante estilos compartidos.
- [x] 5.3 Añadir o actualizar pruebas de presentación pública sin alterar las pruebas funcionales existentes.
- [x] 5.4 Actualizar `constitution.md` y `AGENTS.md` con la convención visual permanente y el tratamiento normal de los ajustes de coherencia.
- [x] 5.5 Ejecutar `npm test` y `npm run build` tras los cambios de estilo.

## 6. Unificación visual global

- [x] 6.1 Centralizar la paleta neutra compartida en variables CSS globales y aplicar las variables a las rutas existentes.
- [x] 6.2 Normalizar listado y creación de movimientos con fondos, superficies, bordes, controles y estados neutros, preservando colores semánticos contenidos.
- [x] 6.3 Actualizar la constitución, AGENTS y artefactos OpenSpec con la convención de paleta centralizada.
- [x] 6.4 Ejecutar `npm test`, `npm run build` y la validación estricta de OpenSpec.
