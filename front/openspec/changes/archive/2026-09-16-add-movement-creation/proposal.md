## Why

Los usuarios autenticados pueden consultar sus movimientos, pero no pueden registrar nuevos datos desde el frontend. La creación de movimientos completa el flujo básico de registro financiero usando el endpoint protegido que ya proporciona la API.

## What Changes

- Añadir una opción en `/home` para crear un movimiento.
- Añadir una ruta protegida y una página standalone con un formulario de creación.
- Cargar las categorías disponibles desde `GET /api/Categorias` para seleccionar el `categoriaId` requerido.
- Enviar `cantidad`, `descripcion` opcional, `fecha` y `categoriaId` a `POST /api/Movimientos` mediante el servicio de movimientos existente.
- Validar los datos antes de enviar y mostrar estados de envío, éxito y error controlados.
- Navegar a `/movimientos` después de una creación correcta.
- Reutilizar el JWT, interceptor y guard existentes sin modificar el backend.

## Capabilities

### New Capabilities
- `user-movement-creation`: Permite a un usuario autenticado crear un movimiento mediante la API y volver al listado tras el éxito.

### Modified Capabilities
- `authenticated-home`: La página principal ofrece una opción para navegar al formulario de creación de movimientos.

## Impact

- Afecta a la página Home, las rutas protegidas, `MovimientosService` y modelos dentro de `app_Fh_front/src/app`.
- Consume `GET /api/Categorias` y `POST /api/Movimientos`, que requieren JWT; la creación recibe `cantidad`, `descripcion`, `fecha` y `categoriaId`.
- No añade dependencias ni modifica el backend, edición o eliminación de movimientos.
