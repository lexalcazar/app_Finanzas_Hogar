## Context

`AuthService` almacena el JWT en `sessionStorage` y el guard e interceptor reutilizan este servicio para acceder al token.

Las páginas `Home`, listado de movimientos y creación de movimientos son páginas standalone protegidas y actualmente no comparten una navegación común.

El JWT actual contiene el nombre de presentación del usuario en el claim:

`http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name`

La lectura de este dato en frontend se utilizará exclusivamente con fines de presentación.

Véase `proposal.md` para la motivación y las deltas de especificación para el comportamiento requerido.

## Goals / Non-Goals

### Goals

- Centralizar en `AuthService` la lectura del nombre de presentación almacenado en el JWT.

- Centralizar en `AuthService` la limpieza de la sesión local.

- Añadir una barra standalone reutilizable a las páginas privadas actuales.

- Incorporar una identidad visual navegable a `/home`.

- Incorporar una acción de cierre de sesión.

- Personalizar Home con el nombre del usuario cuando esté disponible.

- Presentar las dos acciones existentes de Home mediante tarjetas accesibles y responsive.

- Sustituir la estética verde predominante por una paleta basada principalmente en blanco, negro y tonos grises.

### Non-Goals

- No validar criptográficamente el JWT en el cliente.

- No utilizar claims decodificados como mecanismo de autorización.

- No cambiar la autorización del backend.

- No modificar el guard ni el interceptor salvo que sea estrictamente necesario para mantener el comportamiento existente.

- No crear endpoints de logout.

- No introducir refresh tokens.

- No añadir dependencias externas de iconos.

- No crear una carpeta `features`.

- No cambiar la lógica funcional de los formularios de movimientos.

- No añadir nuevas funcionalidades de negocio.

## Decisions

### Decodificación local y defensiva del payload JWT

`AuthService` expondrá una operación para obtener el nombre de presentación del usuario a partir del JWT almacenado.

Se utilizará específicamente el claim:

`http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name`

La implementación deberá:

- comprobar que existe un token;
- comprobar que el JWT contiene un payload;
- decodificar correctamente Base64URL;
- interpretar correctamente contenido UTF-8;
- manejar tokens malformados sin provocar errores de ejecución;
- devolver `null` cuando el nombre no pueda obtenerse.

La decodificación no verificará la autenticidad del JWT ni decidirá permisos.

El backend, guard e interceptor continuarán siendo responsables de sus funciones actuales de autenticación y autorización.

Se descarta incorporar una librería JWT porque únicamente se necesita leer un claim de presentación y no se justifica añadir una dependencia externa para esta operación.

---

### Saludo personalizado y comportamiento sin nombre

Home utilizará el nombre proporcionado por `AuthService`.

Cuando exista un nombre válido, mostrará un saludo equivalente a:

`Bienvenido, Cristina`

Si el nombre no está disponible, Home seguirá mostrando un saludo comprensible, por ejemplo:

`Bienvenido`

La ausencia del claim no deberá impedir el funcionamiento de la página.

---

### Logout local sin dependencia del backend

`AuthService` expondrá una operación de logout responsable de eliminar:

- el JWT almacenado en `sessionStorage`;
- cualquier otro estado local de autenticación que exista en el futuro.

`AuthService` no realizará ninguna petición HTTP de logout.

Tampoco será responsable de decidir la navegación posterior.

El componente de navegación:

1. solicitará a `AuthService` el cierre de sesión;
2. navegará posteriormente a `/login`.

Esta separación mantiene `AuthService` centrado en autenticación y gestión de sesión, mientras que el componente continúa siendo responsable de la interacción y navegación de interfaz.

Una vez eliminado el token, el guard existente impedirá el acceso posterior a las rutas protegidas.

---

### Componente standalone de navegación compartido

Se creará un componente standalone de navegación reutilizable dentro de la estructura Angular convencional existente.

Antes de crear nuevas carpetas se comprobará la estructura actual de `src/app`.

Si existe `src/app/components`, se reutilizará.

Si no existe, podrá crearse para alojar el componente compartido.

La barra se incorporará a las páginas protegidas actuales:

- Home;
- listado de movimientos;
- creación de movimientos.

No se mostrará en las páginas públicas de login y registro.

La barra contendrá:

#### Zona izquierda

Una identidad visual formada por:

- logo o símbolo de la aplicación;
- nombre de la aplicación.

Toda esta zona será navegable a:

`/home`

La navegación utilizará Angular Router.

#### Zona derecha

Una acción de cierre de sesión representada mediante un elemento semántico `button`.

La acción dispondrá de una etiqueta accesible independientemente del icono utilizado.

---

### Tarjetas con HTML semántico

Home utilizará tarjetas completas para representar las acciones existentes:

- `Mis movimientos` → `/movimientos`
- `Crear movimiento` → `/movimientos/nuevo`

Cada tarjeta incluirá:

- icono;
- título;
- descripción breve.

Las tarjetas se implementarán utilizando elementos de navegación semánticos, preferentemente enlaces mediante `routerLink`, de forma que:

- toda la tarjeta sea activable;
- funcione mediante teclado;
- conserve el comportamiento habitual de navegación;
- disponga de foco visible.

No se añadirán manejadores de click innecesarios cuando `routerLink` pueda resolver la navegación directamente.

---

### SVG inline

Los iconos se implementarán mediante SVG inline o recursos ya disponibles en el proyecto.

No se añadirá una librería externa de iconos.

Cuando un icono acompañe a un texto que ya identifica claramente la acción, el SVG será decorativo y no duplicará información para lectores de pantalla.

Los iconos que representen una acción sin texto visible deberán disponer de una etiqueta accesible adecuada.

---

### Estilos comunes de autenticación pública

Las clases existentes `auth-page`, `auth-card`, formularios, enlaces y mensajes se ajustarán en la hoja global para adoptar blanco, negro y grises sin cambiar las plantillas funcionales ni mostrar la barra autenticada. Reutilizar estas clases mantiene Login y Register consistentes y evita duplicación de estilos.

### Paleta global centralizada

`styles.css` definirá variables de fondo, superficie, texto, texto secundario, borde y acción principal. Todas las reglas de identidad para autenticación, Home, navegación, listado y creación las consumirán. Los únicos colores no neutros quedarán limitados a importes y mensajes con significado semántico.

---

### Paleta y diseño responsive

Home utilizará una paleta neutra basada principalmente en:

- blanco;
- negro;
- gris oscuro;
- gris medio;
- gris claro.

Se eliminará el verde como color predominante de la página.

La interfaz mantendrá:

- contraste adecuado;
- foco visible;
- estados `hover`;
- estados `focus`;
- separación clara entre elementos;
- adaptación a pantallas estrechas.

En pantallas con espacio suficiente, las tarjetas podrán mostrarse en varias columnas.

En pantallas estrechas se reorganizarán sin pérdida de información ni funcionalidad.

## Risks / Trade-offs

- **El JWT puede estar malformado o carecer del claim de nombre.**  
  La lectura defensiva devolverá `null` y Home mostrará un saludo genérico sin provocar errores.

- **El nombre decodificado podría confundirse con información utilizada para autorización.**  
  El dato se utilizará exclusivamente para presentación. El guard, interceptor y backend continuarán determinando el acceso y autorización.

- **Los nombres pueden contener caracteres no ASCII.**  
  La decodificación deberá tratar correctamente el contenido UTF-8 del payload y no depender únicamente de una conversión Base64 básica.

- **La barra se incorporará inicialmente a varias páginas protegidas.**  
  Se reutilizará un único componente para evitar duplicar su markup y comportamiento.

- **Los cambios visuales pueden afectar pantallas estrechas.**  
  Se verificará el comportamiento responsive, foco visible y navegación mediante teclado.

## Migration Plan

1. Revisar la estructura actual de `src/app` y los componentes existentes.

2. Ampliar `AuthService` con:
   - lectura defensiva del nombre del usuario;
   - limpieza de sesión mediante logout.

3. Añadir pruebas para:
   - nombre disponible;
   - claim ausente;
   - token ausente;
   - token malformado;
   - limpieza del token mediante logout.

4. Crear el componente standalone de navegación.

5. Implementar:
   - identidad visual navegable a `/home`;
   - acción accesible de logout;
   - navegación posterior a `/login`.

6. Incorporar la barra a:
   - Home;
   - listado de movimientos;
   - creación de movimientos.

7. Rediseñar Home con saludo personalizado.

8. Sustituir las acciones existentes por tarjetas navegables con SVG inline.

9. Aplicar la nueva paleta neutra y comportamiento responsive.

10. Verificar:
    - lectura del nombre;
    - fallback cuando no existe nombre;
    - navegación del logo a Home;
    - logout;
    - ausencia de token después del logout;
    - rechazo posterior de rutas protegidas;
    - navegación de ambas tarjetas;
    - navegación mediante teclado;
    - comportamiento responsive.

11. Ejecutar las pruebas configuradas.

12. Ejecutar `ng build` y corregir cualquier error antes de considerar terminado el cambio.

No requiere migraciones de datos ni modificaciones del backend.

Las sesiones existentes continúan utilizando el mismo JWT y la misma clave de `sessionStorage`.

El rollback consiste en retirar el componente compartido, revertir las modificaciones de `AuthService` asociadas a presentación/logout y restaurar las plantillas anteriores de las páginas privadas.
