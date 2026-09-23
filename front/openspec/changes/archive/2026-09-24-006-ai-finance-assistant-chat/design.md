## Context

La aplicación Angular usa componentes standalone, rutas protegidas por `authGuard`, un interceptor que añade el JWT y una barra de navegación autenticada compartida.

La página Home ya ofrece acciones mediante tarjetas y los servicios existentes utilizan `HttpClient` siguiendo la configuración de API centralizada del proyecto.

El backend expone el endpoint autenticado:

`POST /api/Asistente/chat`

Request:

```json
{
  "mensaje": "¿En qué gasté más durante septiembre de 2026?"
}

## Decisiones

- El historial se guarda en `sessionStorage` bajo una clave propia del asistente y contiene exclusivamente el array de mensajes `{ contenido, autor }`.
- La página restaura el historial al crearse. Datos ausentes, malformados o con una estructura no válida se descartan sin impedir que el chat se muestre.
- La persistencia se actualiza solo al añadir un mensaje de usuario o una respuesta del asistente; los estados de carga y error permanecen en memoria.
- `AuthService.logout()` elimina el token y el historial del asistente para aislar sesiones autenticadas consecutivas.
