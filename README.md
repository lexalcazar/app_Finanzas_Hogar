# App Finanzas Hogar

Aplicacion para la gestion de las finanzas personales del hogar. El proyecto se ha iniciado desde cero el **13 de septiembre de 2026** y esta organizado como una solucion separada en frontend, backend y base de datos.

## Estado actual

Actualmente se ha construido la base tecnica del proyecto:

- Backend ASP.NET Core funcionando como API REST.
- Persistencia configurada con Entity Framework Core y PostgreSQL.
- Modelo inicial de usuarios, categorias y movimientos.
- Migraciones de base de datos creadas.
- Registro e inicio de sesion implementados con ASP.NET Core Identity.
- Emision de tokens JWT para autenticar las peticiones.
- Endpoint protegido para consultar los movimientos del usuario autenticado.
- Frontend Angular inicializado y preparado para continuar el desarrollo.
- Estructura y reglas de trabajo SDD (Spec-Driven Development) incorporadas mediante OpenSpec.

> **Nota sobre el frontend:** la aplicacion Angular todavia contiene la plantilla inicial generada por Angular CLI. Las pantallas, rutas y servicios de autenticacion del frontend quedan como siguiente etapa; la funcionalidad de registro y login implementada hoy corresponde a la API.

## Arquitectura

```text
app_Finanzas_Hogar/
├── back/
│   └── app_Fh_back/       # API ASP.NET Core
└── front/
    ├── app_Fh_front/      # Aplicacion Angular
    ├── .opencode/         # Comandos y skills para OpenSpec
    ├── AGENTS.md          # Instrucciones de trabajo del frontend
    ├── CONSTITUTION.md    # Principios y reglas del proyecto
    └── openspec/          # Configuracion del proceso SDD
```

## Tecnologias utilizadas

### Backend

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- PostgreSQL
- Npgsql Entity Framework Core provider
- ASP.NET Core Identity
- JWT Bearer Authentication
- OpenAPI y Scalar para documentar y probar la API en desarrollo

### Frontend

- Angular 22
- TypeScript 6
- Angular Router
- Angular Forms
- RxJS
- Tailwind CSS 4 mediante PostCSS
- Vitest a traves del soporte de pruebas de Angular
- pnpm 12

## Trabajo realizado el 13/09/2026

### 1. Inicializacion del repositorio

- Se creo el repositorio y su commit inicial.
- Se inicializaron los proyectos de backend y frontend.
- Se configuraron los perfiles de ejecucion del backend y los archivos base de Angular.
- Se agregaron los archivos de ignorados de Git y la licencia.

### 2. Creacion del dominio de finanzas

Se definieron los modelos principales:

- `Usuario`, basado en `IdentityUser`, con nombre y relacion con sus movimientos.
- `Categoria`, con nombre, tipo de movimiento y relacion con sus movimientos.
- `Movimiento`, con cantidad, fecha, descripcion, tipo, categoria y usuario propietario.
- `TipoMovimiento`, con los valores `Ingreso` y `Egreso`.

Cada movimiento queda asociado tanto a una categoria como al usuario que lo ha creado. Esto permite filtrar posteriormente la informacion por usuario.

### 3. Conexion con PostgreSQL y persistencia

- Se creo `ApplicationDbContext` heredando de `IdentityDbContext<Usuario>`.
- Se registraron los conjuntos `Movimientos` y `Categorias`.
- Se configuro Entity Framework Core para utilizar PostgreSQL.
- Se generaron las migraciones:
  - `20260913114543_InitialCreate`
  - `20260913120403_AddIdentity`
- Se incluyeron el snapshot y los archivos auxiliares de las migraciones.
- Se preparo un inicializador de base de datos para crear los roles `User` y `Admin`.
- Tambien se dejo preparada la creacion opcional de un usuario administrador a partir de la configuracion.

### 4. Registro, login y JWT

Se implemento `AuthController` con los endpoints:

| Metodo | Ruta | Descripcion |
| --- | --- | --- |
| `POST` | `/api/Auth/register` | Registra un usuario y le asigna el rol `User`. |
| `POST` | `/api/Auth/login` | Valida el email y la contrasena y devuelve un token JWT. |

Ademas:

- Se crearon `RegisterDto` y `LoginDto`.
- Se integro `UserManager` y `SignInManager` de ASP.NET Core Identity.
- Se creo `TokenService` para generar tokens con identificador, email, nombre y roles.
- Los tokens tienen una duracion de dos horas.
- Se configuro la validacion de issuer, audience, expiracion y firma del JWT.
- Se establecieron respuestas `401 Unauthorized` para credenciales incorrectas y `400 Bad Request` para errores de validacion o registro.

### 5. Primer endpoint protegido de movimientos

Se implemento `MovimientosController`:

| Metodo | Ruta | Autenticacion | Descripcion |
| --- | --- | --- | --- |
| `GET` | `/api/Movimientos` | JWT obligatoria | Devuelve los movimientos del usuario autenticado, ordenados por fecha descendente. |

La logica de consulta se separo en `MovimientoService`. La respuesta utiliza `MovimientoResponseDto` y devuelve la categoria junto con los datos del movimiento.

### 6. Preparacion del frontend y del proceso SDD

- Se inicializo una aplicacion Angular 22 con Angular CLI.
- Se configuraron los scripts de desarrollo, compilacion y pruebas.
- Se habilito Tailwind CSS 4.
- Se dejo preparada la configuracion base del router y de la aplicacion standalone.
- Se incorporaron las reglas del proyecto en `AGENTS.md` y `CONSTITUTION.md`.
- Se configuro OpenSpec para trabajar mediante SDD:
  1. Definir el cambio.
  2. Crear la propuesta.
  3. Especificar requisitos y criterios de aceptacion.
  4. Dividir el trabajo en tareas.
  5. Implementar.
  6. Probar, compilar y validar.
  7. Archivar el cambio.
- Se añadieron comandos y skills para explorar, proponer, aplicar, sincronizar y archivar cambios OpenSpec.

## Configuracion local

### Requisitos

- .NET SDK 10.
- Node.js compatible con Angular 22.
- pnpm 12.
- PostgreSQL.

### Configurar el backend

La configuracion se encuentra en `back/app_Fh_back/appsettings.json`. Antes de ejecutar la API hay que proporcionar:

- `ConnectionStrings:DefaultConnection`: cadena de conexion de PostgreSQL.
- `Jwt:Key`: clave privada suficientemente larga para firmar los tokens.
- `Jwt:Issuer`: emisor del token.
- `Jwt:Audience`: audiencia del token.

La configuracion del administrador inicial es opcional:

- `Admin:Email`
- `Admin:Password`

Para no guardar credenciales en el repositorio, se recomienda utilizar `appsettings.Development.json`, variables de entorno o User Secrets.

Ejemplo de estructura de conexion:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=app_finanzas_hogar;Username=postgres;Password=TU_PASSWORD"
  },
  "Jwt": {
    "Key": "CAMBIAR_POR_UNA_CLAVE_SEGURA",
    "Issuer": "app_Fh_back",
    "Audience": "app_Fh_front"
  }
}
```

Ejecutar la API:

```bash
cd back/app_Fh_back
dotnet restore
dotnet run
```

Perfiles disponibles:

- HTTP: `http://localhost:5240`
- HTTPS: `https://localhost:7142`

En entorno de desarrollo, la documentacion OpenAPI y Scalar queda disponible a traves de los endpoints configurados por ASP.NET Core.

### Configurar el frontend

```bash
cd front/app_Fh_front
pnpm install
pnpm start
```

La aplicacion Angular se sirve normalmente en `http://localhost:4200/`.

Comandos disponibles:

```bash
pnpm start   # Servidor de desarrollo
pnpm build   # Compilacion
pnpm test    # Pruebas unitarias
pnpm run watch
```

## Endpoints de ejemplo

Registro:

```http
POST /api/Auth/register
Content-Type: application/json

{
  "nombre": "Nombre de usuario",
  "apellido": "Apellido",
  "email": "usuario@example.com",
  "password": "UnaPasswordSegura123!"
}
```

Login:

```http
POST /api/Auth/login
Content-Type: application/json

{
  "email": "usuario@example.com",
  "password": "UnaPasswordSegura123!"
}
```

Consulta de movimientos:

```http
GET /api/Movimientos
Authorization: Bearer <token>
```

## Proximos pasos

- Crear las especificaciones OpenSpec de las funcionalidades del frontend.
- Sustituir la plantilla inicial de Angular por las pantallas de registro y login.
- Conectar Angular con los endpoints de autenticacion.
- Guardar y renovar el token de forma segura en el frontend.
- Añadir rutas protegidas, guards e interceptor HTTP.
- Implementar altas, bajas y modificaciones de movimientos.
- Implementar la gestion de categorias.
- Crear dashboard, resumen mensual y filtros.
- Añadir pruebas unitarias y de integracion para backend y frontend.
- Incorporar validaciones, manejo de errores y una configuracion separada por entorno.

## Historial de commits de hoy

| Commit | Mensaje |
| --- | --- |
| `e150884` | Initial commit |
| `c480b05` | proyectos back/front inicializados |
| `b6a5df4` | Modelos bd creados, sin migraciones |
| `a88a94b` | Conectada bd y primera migracion hecha |
| `a148d77` | Login Registro funcionando comienzo SDD en front |
| `dad5c32` | Agent host session - baseline checkpoint |
