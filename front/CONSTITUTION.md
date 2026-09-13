# Project Constitution

## 1. Propósito del proyecto

Este proyecto es el frontend de una aplicación de gestión de finanzas personales.

El frontend está desarrollado con Angular y consume una API REST desarrollada en ASP.NET Core.

El proyecto seguirá un enfoque SDD (Spec-Driven Development), utilizando OpenSpec para definir y gestionar de forma incremental las distintas funcionalidades y cambios.

OpenSpec organiza el proceso de desarrollo y las especificaciones. No determina la estructura de carpetas de Angular.

---

## 2. Principios generales

El proyecto debe seguir los siguientes principios:

* Priorizar código sencillo, legible y fácil de mantener.
* Evitar abstracciones innecesarias.
* Evitar sobreingeniería.
* Mantener responsabilidades claramente separadas.
* Mantener una estructura Angular convencional y reconocible.
* Reutilizar código cuando exista una necesidad real.
* No introducir librerías externas si Angular proporciona una solución adecuada.
* No modificar partes del proyecto que no estén relacionadas con el cambio actual.
* Mantener coherencia con el código existente.
* Las decisiones técnicas importantes deben poder justificarse.

---

## 3. Desarrollo dirigido por especificaciones

Las funcionalidades y cambios relevantes se definirán antes de implementarse.

OpenSpec será la herramienta principal para gestionar este proceso.

El flujo general será:

1. Definir el cambio.
2. Crear la propuesta.
3. Definir requisitos y comportamiento esperado.
4. Establecer criterios de aceptación.
5. Dividir el trabajo en tareas.
6. Implementar.
7. Ejecutar pruebas.
8. Comprobar la compilación.
9. Validar la implementación contra la especificación.
10. Archivar el cambio cuando esté completado.

Las features o cambios definidos en OpenSpec son unidades de trabajo.

No es obligatorio que cada feature de OpenSpec tenga una carpeta equivalente dentro de `src/app`.

Una especificación puede afectar a varios componentes, servicios, modelos, rutas u otros archivos del proyecto.

---

## 4. Estructura Angular

El proyecto utilizará una estructura convencional de Angular organizada principalmente por responsabilidad.

La estructura general podrá ser similar a:

```text
src/app/
├── components/
├── pages/
├── services/
├── models/
├── guards/
├── interceptors/
├── pipes/
├── directives/
├── app.component.ts
├── app.config.ts
└── app.routes.ts
```

No será obligatorio crear una carpeta si todavía no existe código que justifique su uso.

La estructura deberá mantenerse simple y crecer según las necesidades reales del proyecto.

---

## 5. Pages

La carpeta `pages` contendrá componentes que representan pantallas completas asociadas normalmente a una ruta.

Ejemplos:

```text
pages/
├── login/
├── movimientos/
├── crear-movimiento/
├── editar-movimiento/
├── resumen-mensual/
└── dashboard/
```

Las páginas pueden coordinar componentes, servicios y navegación.

No deben contener lógica de negocio compleja.

---

## 6. Components

La carpeta `components` contendrá componentes reutilizables o partes concretas de una interfaz.

Ejemplos:

```text
components/
├── navbar/
├── movimiento-form/
├── movimiento-card/
└── loading/
```

No todos los elementos de una página necesitan convertirse en componentes independientes.

Un componente debe extraerse cuando exista una razón clara, como reutilización, separación de responsabilidades o reducción de complejidad.

---

## 7. Services

La carpeta `services` contendrá los servicios Angular.

Ejemplos:

```text
services/
├── auth.service.ts
├── movimientos.service.ts
└── resumen.service.ts
```

El acceso a la API debe realizarse mediante servicios.

Los componentes y páginas no deben realizar directamente peticiones HTTP.

El flujo habitual será:

```text
Page / Component
       ↓
Service
       ↓
HttpClient
       ↓
ASP.NET Core API
```

---

## 8. Models

La carpeta `models` contendrá interfaces, tipos y modelos TypeScript utilizados por la aplicación.

Ejemplo:

```text
models/
├── movimiento.ts
├── movimiento-create.ts
├── movimiento-update.ts
├── login-request.ts
├── login-response.ts
└── resumen-mensual.ts
```

Los contratos del frontend deben corresponderse con los DTOs que realmente expone el backend.

No deben inventarse campos que la API no proporciona.

Debe evitarse el uso de `any`.

---

## 9. Guards

Los guards de Angular se almacenarán en:

```text
guards/
```

Ejemplo:

```text
guards/
└── auth.guard.ts
```

Se utilizarán para proteger rutas cuando sea necesario.

---

## 10. Interceptors

Los interceptores HTTP se almacenarán en:

```text
interceptors/
```

Ejemplo:

```text
interceptors/
└── auth.interceptor.ts
```

La incorporación del JWT a las peticiones autenticadas debe hacerse mediante un interceptor y no manualmente desde cada componente o servicio.

---

## 11. Angular

Se utilizarán componentes standalone.

Se utilizarán las APIs modernas de Angular cuando sean adecuadas para la versión instalada.

Los componentes deben centrarse principalmente en:

* presentación;
* interacción con el usuario;
* coordinación de acciones.

La lógica reutilizable y el acceso a datos deben extraerse a servicios cuando corresponda.

---

## 12. Signals

Se podrán utilizar Angular Signals para gestionar estado local y sencillo.

Ejemplo:

```typescript
movimientos = signal<Movimiento[]>([]);
cargando = signal(false);
error = signal<string | null>(null);
```

No se añadirá una librería global de estado salvo que la complejidad real de la aplicación lo justifique.

---

## 13. Comunicación con la API

La comunicación con la API se realizará mediante `HttpClient`.

Las URLs completas de la API no deben repetirse por distintos componentes.

La URL base debe mantenerse en la configuración correspondiente.

Ejemplo conceptual:

```typescript
environment.apiUrl
```

y utilizarse de forma similar a:

```typescript
`${environment.apiUrl}/api/movimientos`
```

Los componentes no deben contener URLs del backend.

---

## 14. Routing

La navegación se gestionará mediante Angular Router.

Ejemplos de rutas:

```text
/login
/movimientos
/movimientos/nuevo
/movimientos/:id/editar
/resumen
/dashboard
```

Las rutas privadas deberán protegerse cuando corresponda.

---

## 15. Autenticación

La autenticación se realizará contra la API ASP.NET Core mediante JWT.

La responsabilidad debe mantenerse separada:

```text
Login page
    ↓
AuthService
    ↓
API
```

Las peticiones autenticadas utilizarán:

```text
Authorization: Bearer <token>
```

La incorporación de esta cabecera será responsabilidad del interceptor.

---

## 16. Formularios

Se utilizarán las soluciones oficiales de Angular.

Para formularios sencillos podrán utilizarse Template-Driven Forms.

Para formularios más complejos o con validaciones importantes se priorizarán Reactive Forms.

El backend seguirá siendo la autoridad final para validar los datos.

---

## 17. Manejo de errores

Los errores de la API deberán tratarse de forma controlada.

Los usuarios deben recibir mensajes comprensibles.

No deben mostrarse directamente errores técnicos del framework o de `HttpClient`.

Cuando sea útil durante desarrollo, los detalles técnicos podrán escribirse en consola.

---

## 18. Estados de interfaz

Las páginas que obtengan información de la API deberán considerar cuando proceda:

* estado de carga;
* datos obtenidos;
* datos vacíos;
* error.

La interfaz nunca debe asumir que una petición HTTP tendrá éxito.

---

## 19. TypeScript

Se utilizará TypeScript de forma estricta.

Debe evitarse:

```typescript
any
```

Se utilizarán tipos específicos, interfaces, enums o unions cuando corresponda.

---

## 20. Nombres

Los nombres deben describir claramente su responsabilidad.

Ejemplos:

```text
MovimientoService
AuthService
LoginComponent
ResumenMensual
CrearMovimientoRequest
```

Debe evitarse utilizar nombres vagos como:

```text
data
manager
helper
temp
service2
```

---

## 21. Dependencias

No debe añadirse una librería externa sin una necesidad clara.

Antes de incorporar una dependencia debe evaluarse:

1. qué problema resuelve;
2. si Angular ya proporciona una solución;
3. si aporta suficiente valor;
4. qué coste de mantenimiento introduce.

---

## 22. Pruebas

Las funcionalidades importantes deberán probarse cuando sea razonable.

Se priorizarán pruebas de:

* servicios;
* guards;
* interceptores;
* lógica relevante;
* componentes con comportamiento significativo.

No se crearán tests únicamente para aumentar métricas.

Las pruebas deben comprobar comportamiento útil.

---

## 23. Criterio de finalización

Una funcionalidad o cambio no estará terminado simplemente porque funcione visualmente.

Cuando corresponda debe cumplir:

* especificación OpenSpec;
* criterios de aceptación;
* compilación correcta;
* ausencia de errores TypeScript;
* integración correcta con la API;
* manejo de errores;
* navegación correcta;
* pruebas relevantes superadas.

Como mínimo deberá ejecutarse:

```bash
ng build
```

antes de considerar terminado un cambio relevante.

---

## 24. Alcance de los cambios

Cada cambio deberá tener un alcance limitado.

Una implementación no debe modificar archivos no relacionados sin una necesidad técnica real.

Si aparece una mejora no necesaria para completar la tarea actual, deberá tratarse como un cambio posterior.

---

## 25. Refactorización

Los refactors deben tener una finalidad clara:

* reducir duplicación;
* reducir complejidad;
* mejorar legibilidad;
* separar responsabilidades;
* facilitar una funcionalidad concreta.

No deben realizarse grandes refactors como efecto secundario de una tarea pequeña.

---

## 26. Seguridad

Nunca deben almacenarse en el repositorio:

* contraseñas;
* secretos;
* claves privadas;
* credenciales;
* tokens reales;
* cadenas de conexión privadas.

El código ejecutado en el navegador debe considerarse visible para el usuario.

Por tanto, ningún secreto debe depender exclusivamente del frontend para permanecer oculto.

---

## 27. Relación con el backend

La API ASP.NET Core define el contrato de comunicación.

Antes de implementar una integración debe comprobarse:

* endpoint;
* método HTTP;
* parámetros;
* body esperado;
* respuesta;
* códigos de estado;
* necesidad de autenticación.

El frontend debe adaptarse al contrato real del backend.

---

## 28. Implementación incremental

La aplicación se construirá mediante cambios pequeños gestionados con OpenSpec.

Ejemplo de secuencia:

```text
Autenticación
↓
Listado de movimientos
↓
Crear movimiento
↓
Editar movimiento
↓
Eliminar movimiento
↓
Resumen mensual
↓
Dashboard
```

Estas son unidades de desarrollo de OpenSpec.

No representan necesariamente la estructura de carpetas de Angular.

---

## 29. Aprendizaje y comprensión

Este proyecto también tiene un objetivo formativo.

El código generado o modificado mediante agentes de IA debe poder entenderse y explicarse.

Por ello:

* evitar patrones innecesariamente complejos;
* evitar generar código que no sea necesario;
* favorecer soluciones explícitas;
* mantener el nivel de complejidad adecuado al proyecto;
* justificar decisiones técnicas relevantes.

La IA debe ayudar a desarrollar el proyecto, no ocultar su funcionamiento.

---

## 30. Autoridad de esta constitución

Este documento establece las reglas generales de desarrollo.

Las especificaciones de OpenSpec pueden definir requisitos concretos para un cambio, pero no deben contradecir esta constitución sin una decisión explícita.

En caso de conflicto debe priorizarse:

1. Corrección.
2. Seguridad.
3. Claridad.
4. Simplicidad.
5. Mantenibilidad.
6. Coherencia con el proyecto existente.
7. Rendimiento cuando realmente sea relevante.

La solución más sofisticada no será considerada automáticamente la mejor.
