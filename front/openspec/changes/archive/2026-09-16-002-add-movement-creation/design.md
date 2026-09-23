## Context

La aplicación ya dispone de:

* rutas protegidas;
* JWT almacenado en sesión;
* interceptor Bearer;
* `MovimientosService`;
* página `Home`.

La API protege `POST /api/Movimientos` y recibe un `CreateMovimientoDto` con:

* `cantidad`: decimal entre `0.01` y `999999999`;
* `descripcion`: opcional, máximo 250 caracteres;
* `fecha`;
* `categoriaId`: identificador positivo de categoría.

El backend obtiene el usuario a partir del JWT y determina el tipo del movimiento a partir de la categoría seleccionada.

Las categorías disponibles se consultan mediante:

`GET /api/Categorias`

La respuesta incluye los datos necesarios para mostrar cada categoría al usuario, incluyendo:

* `id`;
* `nombre`;
* `tipo`.

## Goals / Non-Goals

### Goals

* Añadir un flujo protegido y validado para crear movimientos.

* Reutilizar el guard, interceptor y sistema de autenticación existentes.

* Reutilizar `MovimientosService` para la creación de movimientos.

* Encapsular la consulta de categorías en un servicio específico.

* Permitir al usuario seleccionar una categoría real antes de enviar el `categoriaId` requerido.

* Mantener el formulario alineado con las validaciones reales de `CreateMovimientoDto`.

* Navegar al listado de movimientos después de una creación correcta.

### Non-Goals

* No modificar endpoints, DTOs, categorías ni lógica del backend.

* No crear categorías.

* No editar categorías.

* No eliminar categorías.

* No editar movimientos.

* No eliminar movimientos.

* No filtrar ni paginar movimientos.

* No introducir una solución global de estado.

* No modificar el sistema de autenticación existente.

## Decisions

### Ruta y página standalone de creación

Se añadirá una ruta protegida:

`/movimientos/nuevo`

La ruta utilizará el guard de autenticación existente.

Se creará una página standalone en:

`pages/crear-movimiento`

La página `Home` incorporará una opción de navegación hacia esta ruta.

No se creará una carpeta `features`.

Tampoco se extraerá inicialmente el formulario a un componente independiente, ya que únicamente será utilizado por esta página dentro del alcance actual.

Si en el futuro el formulario puede reutilizarse para edición, esa decisión podrá revisarse en el cambio correspondiente.

---

### Reactive Form y validaciones

La página utilizará Reactive Forms.

El formulario incluirá:

* cantidad;
* descripción;
* fecha;
* categoría.

Las validaciones del frontend reflejarán las restricciones de `CreateMovimientoDto`.

#### Cantidad

La cantidad:

* será obligatoria;
* tendrá un valor mínimo de `0.01`;
* tendrá un valor máximo de `999999999`.

#### Descripción

La descripción:

* será opcional;
* tendrá una longitud máxima de 250 caracteres.

#### Fecha

La fecha será obligatoria.

El valor enviado deberá ser compatible con el formato esperado por `DateOnly` en la API.

#### Categoría

La categoría será obligatoria.

El formulario almacenará el identificador de la categoría seleccionada y enviará dicho valor como `categoriaId`.

El envío permanecerá deshabilitado mientras el formulario sea inválido.

---

### Contrato TypeScript de creación

Se añadirá un contrato TypeScript que refleje exactamente `CreateMovimientoDto`.

Conceptualmente contendrá:

```text
cantidad
descripcion
fecha
categoriaId
```

No incluirá:

* `usuarioId`;
* `tipo`;
* otros campos no aceptados por el DTO del backend.

El frontend no inventará propiedades adicionales.

---

### Separación entre MovimientosService y CategoriasService

`MovimientosService` continuará siendo responsable de las operaciones relacionadas directamente con movimientos.

Se ampliará con la operación necesaria para crear un movimiento mediante:

`POST /api/Movimientos`

La consulta de categorías no se incorporará a `MovimientosService`.

Se creará un `CategoriasService` específico responsable de:

`GET /api/Categorias`

También se añadirá un contrato TypeScript para representar las categorías devueltas por la API.

La separación quedará conceptualmente así:

```text
CrearMovimientoPage
        │
        ├── MovimientosService
        │       └── POST /api/Movimientos
        │
        └── CategoriasService
                └── GET /api/Categorias
```

Esto mantiene las responsabilidades separadas y permite reutilizar `CategoriasService` en futuras funcionalidades, como la edición de movimientos.

Las páginas no realizarán peticiones HTTP directamente.

---

### Categorías cargadas desde la API

Al cargar la página de creación se realizará:

`GET /api/Categorias`

La solicitud utilizará el interceptor Bearer existente.

Las categorías recibidas se mostrarán al usuario como opciones seleccionables.

El usuario visualizará información comprensible de la categoría, como su nombre y, cuando resulte útil, su tipo.

El identificador interno no será el dato principal mostrado al usuario.

Si `tipo` se representa en la API mediante un valor técnico o enum numérico, la interfaz deberá mostrar una etiqueta comprensible para el usuario en lugar del valor interno cuando sea necesario.

Se descarta permitir al usuario introducir manualmente un `categoriaId`.

---

### Categorías no disponibles

Si la API de categorías falla:

* se mostrará un mensaje comprensible;
* se impedirá el envío del formulario;
* no se permitirá introducir manualmente un identificador alternativo.

Si la API responde correctamente pero devuelve una colección vacía:

* se informará al usuario de que no existen categorías disponibles;
* la creación permanecerá deshabilitada.

Esto evita enviar identificadores inexistentes o inventados.

---

### Estado local mediante Signals

La página utilizará Signals para gestionar el estado local relacionado con la interfaz.

Como mínimo se contemplarán estados para:

* carga de categorías;
* envío del movimiento;
* errores de carga de categorías;
* errores durante la creación.

No se introducirá una librería global de estado.

El estado pertenece únicamente a esta página dentro del alcance actual.

---

### Creación del movimiento

Cuando el formulario sea válido, la página construirá un request compatible con `CreateMovimientoDto` y utilizará `MovimientosService` para realizar:

`POST /api/Movimientos`

El cuerpo incluirá exclusivamente:

* `cantidad`;
* `descripcion`;
* `fecha`;
* `categoriaId`.

No se enviará `usuarioId`.

No se enviará `tipo`.

El backend continuará siendo responsable de asociar el movimiento al usuario autenticado y determinar su tipo a partir de la categoría.

Mientras la operación esté en curso:

* se mostrará un estado de envío;
* se impedirá realizar envíos duplicados.

---

### Respuesta de creación

Cuando la API responda correctamente a la creación:

* se considerará completada la operación;
* el usuario será dirigido a `/movimientos`.

La lógica del frontend no dependerá innecesariamente de un código HTTP concreto si el contrato actual de la API considera satisfactoria la operación mediante una respuesta `2xx`.

Si el endpoint está confirmado específicamente como `201 Created`, las pruebas podrán reflejar dicho comportamiento.

Si la creación falla:

* el usuario permanecerá en el formulario;
* se conservarán los datos introducidos;
* se mostrará un mensaje controlado y comprensible.

---

### Acceso desde Home

La página `Home` se ampliará con una opción para crear un nuevo movimiento.

La navegación será:

```text
/home
   ↓
Crear movimiento
   ↓
/movimientos/nuevo
```

No se añadirán enlaces a funcionalidades que todavía no estén implementadas dentro de este cambio.

## Risks / Trade-offs

* **Una categoría puede dejar de existir entre su carga y el envío del formulario.**
  El backend continuará validando `categoriaId`. Si rechaza la creación, el formulario conservará los datos y mostrará un mensaje comprensible para que el usuario pueda seleccionar otra categoría.

* **La API de categorías puede no responder.**
  El formulario impedirá la creación mientras no exista una colección válida de categorías.

* **La API puede devolver una colección vacía de categorías.**
  La interfaz informará de que no existen categorías disponibles y mantendrá deshabilitado el envío.

* **El token puede expirar mientras el usuario completa el formulario.**
  La API rechazará la operación. La página mostrará un error controlado. La gestión global de expiración de sesión permanece fuera de esta feature.

* **La separación de servicios introduce un archivo adicional.**
  Se acepta este coste porque categorías y movimientos representan recursos distintos y `CategoriasService` podrá reutilizarse en funcionalidades posteriores.

* **El contrato del backend puede cambiar.**
  Los contratos TypeScript reflejarán únicamente los DTOs actuales y deberán actualizarse explícitamente si estos cambian.

## Migration Plan

1. Revisar los contratos actuales de `CreateMovimientoDto` y de la respuesta de `GET /api/Categorias`.

2. Crear el contrato TypeScript correspondiente a `CreateMovimientoDto`.

3. Crear el contrato TypeScript de categoría.

4. Crear `CategoriasService` con la consulta `GET /api/Categorias`.

5. Ampliar `MovimientosService` con la operación `POST /api/Movimientos`.

6. Crear la página standalone `CrearMovimiento`.

7. Implementar el Reactive Form con las validaciones:

   * cantidad entre `0.01` y `999999999`;
   * descripción máxima de 250 caracteres;
   * fecha obligatoria;
   * categoría obligatoria.

8. Implementar la carga de categorías.

9. Gestionar:

   * carga correcta;
   * categorías vacías;
   * error de carga.

10. Implementar la creación del movimiento y el estado de envío.

11. Añadir la ruta protegida `/movimientos/nuevo`.

12. Añadir el acceso desde `Home`.

13. Verificar:

* acceso protegido;
* validaciones;
* carga de categorías;
* selección de categoría;
* creación correcta;
* creación rechazada;
* error de comunicación;
* prevención de envíos duplicados;
* navegación posterior a `/movimientos`;
* ausencia de `usuarioId` y `tipo` en el request.

14. Ejecutar las pruebas configuradas.

15. Ejecutar `ng build` y corregir cualquier error antes de considerar completado el cambio.

La reversión consistirá en retirar:

* la ruta añadida;
* el enlace desde `Home`;
* la página de creación;
* los contratos nuevos;
* `CategoriasService`;
* el método de creación añadido a `MovimientosService`.

No existen migraciones de base de datos ni cambios persistentes en el backend asociados a esta feature.
