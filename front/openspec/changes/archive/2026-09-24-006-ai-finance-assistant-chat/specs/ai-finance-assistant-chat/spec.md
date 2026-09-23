## Purpose

Permitir que los usuarios autenticados consulten al asistente financiero y lean sus respuestas dentro de la aplicación, delegando toda la interpretación financiera y generación de respuestas al backend.

## ADDED Requirements

### Requirement: Acceso protegido al asistente financiero

El sistema SHALL ofrecer el chat del asistente financiero en la ruta `/asistente` únicamente a usuarios con sesión autenticada.

Si un usuario sin sesión intenta acceder a esa ruta, el sistema MUST redirigirlo a `/login` mediante el mecanismo de protección existente.

#### Scenario: Acceso autenticado al asistente

- **WHEN** un usuario con una sesión autenticada accede a `/asistente`
- **THEN** el sistema muestra la interfaz de chat del asistente financiero

#### Scenario: Acceso no autenticado al asistente

- **WHEN** un usuario sin sesión accede a `/asistente`
- **THEN** el sistema lo redirige a `/login`

### Requirement: Consulta y respuesta del asistente

El sistema SHALL permitir que un usuario autenticado envíe un mensaje al asistente desde el chat.

Para cada consulta válida, el sistema MUST enviar una solicitud:

`POST /api/Asistente/chat`

con un cuerpo:

```json
{
  "mensaje": "texto de la consulta"
}
```

#### Scenario: Consulta respondida correctamente

- **WHEN** un usuario autenticado envía una consulta no vacía desde el chat
- **THEN** el sistema realiza `POST /api/Asistente/chat` con el campo `mensaje` y añade el campo `respuesta` devuelto por la API a la conversación

### Requirement: Historial del chat durante la sesión autenticada

El sistema MUST conservar exclusivamente los mensajes de usuario y asistente en `sessionStorage` mientras dure la sesión autenticada actual. El sistema MUST restaurar ese historial al abrir de nuevo la página del asistente y MUST tolerar datos ausentes, corruptos o con estructura inválida sin mostrar un error técnico. El sistema MUST persistir el historial únicamente al añadir un mensaje de usuario o del asistente; los estados de carga y error MUST permanecer solo en memoria. Al cerrar sesión, el sistema MUST eliminar el historial almacenado.

#### Scenario: Restauración de historial de sesión

- **WHEN** un usuario autenticado vuelve a abrir `/asistente` durante la misma sesión con un historial válido almacenado
- **THEN** el sistema muestra los mensajes almacenados sin realizar una nueva consulta

#### Scenario: Historial almacenado corrupto

- **WHEN** la página encuentra un historial que no puede analizarse o no corresponde a mensajes válidos
- **THEN** el sistema lo descarta y muestra una conversación vacía sin errores técnicos

#### Scenario: Error de consulta

- **WHEN** una consulta falla después de añadir el mensaje del usuario
- **THEN** el sistema conserva solo los mensajes ya añadidos y no persiste el estado de carga ni el error

#### Scenario: Cierre de sesión

- **WHEN** el usuario cierra sesión
- **THEN** el sistema elimina el historial del asistente almacenado para esa sesión
