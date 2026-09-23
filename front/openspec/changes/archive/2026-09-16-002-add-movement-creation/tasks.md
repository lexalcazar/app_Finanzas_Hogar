## 1. Contratos y acceso a datos

* [x] 1.1 Crear el contrato TypeScript correspondiente a `CreateMovimientoDto`, reflejando únicamente:

  * cantidad;
  * descripción;
  * fecha;
  * categoriaId.

* [x] 1.2 Crear el contrato TypeScript correspondiente a `CategoriaResponseDto`, reflejando únicamente los campos reales devueltos por la API.

* [x] 1.3 Crear `CategoriasService` para consultar las categorías mediante:

  `GET /api/Categorias`

  reutilizando la configuración HTTP, JWT e interceptor existentes.

* [x] 1.4 Extender `MovimientosService` con la creación autenticada mediante:

  `POST /api/Movimientos`

  enviando exclusivamente:

  * cantidad;
  * descripción;
  * fecha;
  * categoriaId.

* [x] 1.5 Verificar que las solicitudes de creación no incluyen:

  * `usuarioId`;
  * `tipo`;
  * ningún campo adicional que no pertenezca a `CreateMovimientoDto`.

## 2. Creación de movimientos

* [x] 2.1 Crear la página standalone `crear-movimiento` utilizando Reactive Forms.

* [x] 2.2 Incorporar estado local mediante Signals para gestionar:

  * carga de categorías;
  * envío del formulario;
  * errores de carga de categorías;
  * errores de creación.

* [x] 2.3 Cargar las categorías disponibles mediante `CategoriasService` al acceder a la página.

* [x] 2.4 Mostrar las categorías disponibles utilizando información comprensible para el usuario, como nombre y tipo, sin requerir que conozca el `categoriaId`.

* [x] 2.5 Gestionar el caso en el que la API devuelve una colección de categorías vacía:

  * informar al usuario;
  * impedir el envío del formulario.

* [x] 2.6 Gestionar el error al cargar categorías:

  * mostrar un mensaje comprensible;
  * impedir el envío del formulario.

* [x] 2.7 Configurar las validaciones del formulario para que:

  * cantidad sea obligatoria;
  * cantidad sea como mínimo `0.01`;
  * cantidad no supere `999999999`;
  * descripción no supere 250 caracteres;
  * fecha sea obligatoria;
  * categoría sea obligatoria.

* [x] 2.8 Evitar realizar la solicitud de creación mientras el formulario sea inválido.

* [x] 2.9 Implementar el envío de un formulario válido mediante `MovimientosService`.

* [x] 2.10 Impedir envíos duplicados mientras la solicitud de creación esté en curso.

* [x] 2.11 Ante una creación correcta, navegar a:

  `/movimientos`

* [x] 2.12 Ante un error de creación:

  * permanecer en el formulario;
  * conservar los datos introducidos;
  * mostrar un mensaje comprensible sin exponer detalles técnicos.

## 3. Rutas y navegación

* [x] 3.1 Configurar la ruta protegida:

  `/movimientos/nuevo`

  utilizando el guard de autenticación existente.

* [x] 3.2 Añadir una opción accesible en `/home` para navegar a:

  `/movimientos/nuevo`

* [x] 3.3 Mantener la ruta de creación fuera del alcance de usuarios sin token mediante el guard existente.

* [x] 3.4 Aplicar estilos responsivos y accesibles al formulario y a la nueva opción de `Home`, manteniendo coherencia con las páginas existentes.

## 4. Pruebas y verificación

* [x] 4.1 Añadir pruebas de `CategoriasService` verificando:

  * método `GET`;
  * ruta `/api/Categorias`;
  * uso de la infraestructura HTTP existente.

* [x] 4.2 Añadir pruebas de `MovimientosService` verificando:

  * método `POST`;
  * ruta `/api/Movimientos`;
  * body compatible con `CreateMovimientoDto`;
  * ausencia de `usuarioId`;
  * ausencia de `tipo`.

* [x] 4.3 Añadir pruebas de la página de creación para:

  * carga correcta de categorías;
  * categorías vacías;
  * error al cargar categorías;
  * formulario válido;
  * cantidad inferior a `0.01`;
  * cantidad superior a `999999999`;
  * fecha vacía;
  * categoría no seleccionada;
  * descripción superior a 250 caracteres;
  * prevención del envío con formulario inválido.

* [x] 4.4 Añadir pruebas para el envío del formulario verificando:

  * estado de envío;
  * prevención de envíos duplicados;
  * creación correcta;
  * error de creación;
  * conservación del formulario tras error;
  * navegación a `/movimientos` después del éxito.

* [x] 4.5 Añadir pruebas de la ruta protegida `/movimientos/nuevo`.

* [x] 4.6 Añadir una prueba de navegación desde `Home` hacia `/movimientos/nuevo`.

* [x] 4.7 Verificar que el interceptor existente
