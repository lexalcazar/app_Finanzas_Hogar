## 1. Contratos y servicio del asistente

- [x] 1.1 Crear los contratos TypeScript de solicitud y respuesta del asistente con los campos:
  - `mensaje`;
  - `respuesta`.

- [x] 1.2 Crear `AsistenteService` para realizar `POST /api/Asistente/chat`, siguiendo exactamente la misma convención de construcción de URL utilizada por los servicios existentes.

  Antes de implementarlo, inspeccionar:
  - `environment.apiUrl`;
  - servicios Angular existentes.

  No asumir si `environment.apiUrl` contiene ya `/api`.

  El resultado final debe corresponder al endpoint:

  `POST /api/Asistente/chat`

- [x] 1.3 Mantener la petición completamente tipada y no utilizar `any`.

- [x] 1.4 Verificar que `AsistenteService` no:
  - añada manualmente el JWT;
  - se comunique con LM Studio;
  - contenga lógica financiera;
  - interprete el mensaje del usuario.

- [x] 1.5 Añadir pruebas HTTP de `AsistenteService` que comprueben:
  - método `POST`;
  - URL final correcta;
  - body exacto `{ mensaje }`;
  - lectura de `{ respuesta }`.

## 2. Ruta y acceso desde Home

- [x] 2.1 Inspeccionar la estructura actual de `src/app` y crear la página standalone del asistente siguiendo la organización existente, bajo `pages/asistente` únicamente si coincide con la estructura actual.

- [x] 2.2 Integrar en la página la navegación autenticada compartida existente.

- [x] 2.3 Registrar `/asistente` utilizando `authGuard`.

- [x] 2.4 Añadir a Home una tarjeta accesible:

  `Tu gestor de finanzas`

  con:
  - SVG inline;
  - descripción breve;
  - área completa navegable;
  - `routerLink="/asistente"`;
  - foco y hover visibles.

- [x] 2.5 Mantener intactas las tarjetas existentes de Home.

- [x] 2.6 Añadir pruebas de ruta para comprobar:
  - acceso autenticado a `/asistente`;
  - protección mediante el guard existente;
  - redirección a `/login` sin autenticación.

- [x] 2.7 Actualizar las pruebas de Home para comprobar:
  - existencia de la tarjeta;
  - destino `/asistente`;
  - mantenimiento de las tarjetas anteriores.

## 3. Chat y estado de sesión

- [x] 3.1 Crear un modelo frontend para representar los mensajes del chat cuando resulte apropiado, diferenciando:
  - usuario;
  - asistente.

  Este modelo no debe modificar el contrato HTTP.

- [x] 3.2 Implementar la interfaz responsive del chat utilizando la identidad visual global existente.

  Incluir:
  - título `Tu gestor de finanzas`;
  - zona de conversación;
  - mensajes diferenciados visualmente;
  - campo de mensaje;
  - acción de envío;
  - estados de foco;
  - estado visual de carga;
  - estado disabled.

- [x] 3.3 Gestionar mediante Signals locales:
  - historial de mensajes;
  - estado de carga;
  - error genérico.

- [x] 3.4 Normalizar el mensaje mediante `trim()` antes del envío.

- [x] 3.5 Impedir el envío cuando el mensaje:
  - esté vacío;
  - contenga únicamente espacios.

- [x] 3.6 Al enviar una consulta válida:
  - comprobar primero que no exista otra solicitud pendiente;
  - añadir el mensaje del usuario al historial;
  - limpiar errores anteriores;
  - activar el estado de carga;
  - llamar a `AsistenteService`.

- [x] 3.7 Al recibir una respuesta correcta:
  - utilizar únicamente el campo `respuesta`;
  - añadirlo al historial como mensaje del asistente;
  - finalizar el estado de carga;
  - permitir una nueva consulta.

- [x] 3.8 Deshabilitar visualmente el envío durante una solicitud y proteger también el manejador para impedir peticiones duplicadas aunque se invoque más de una vez.

- [x] 3.9 Mostrar durante la espera un estado comprensible que indique que el asistente está procesando la consulta.

- [x] 3.10 Si la petición falla:
  - finalizar la carga;
  - mantener el historial existente;
  - mostrar un error genérico y comprensible;
  - no mostrar detalles técnicos;
  - permitir un nuevo envío.

- [x] 3.11 Mantener el historial de mensajes durante la sesión autenticada mediante `sessionStorage`.

  - Restaurar de forma defensiva solo mensajes válidos al crear la página.
  - Persistir exclusivamente tras añadir un mensaje de usuario o asistente.
  - No persistir estados de carga ni errores.
  - No utilizar `localStorage` ni persistencia backend.

- [x] 3.12 Añadir pruebas del componente para:
  - renderizado inicial;
  - envío correcto;
  - normalización mediante `trim()`;
  - rechazo de mensaje vacío;
  - rechazo de mensaje compuesto solo por espacios;
  - incorporación del mensaje del usuario al historial;
  - incorporación de la respuesta del asistente;
  - conservación de mensajes anteriores;
  - estado de carga mientras la petición está pendiente;
  - envío deshabilitado durante carga;
  - prevención lógica de solicitudes duplicadas;
  - finalización de carga tras éxito;
  - error genérico;
  - conservación del historial tras error;
  - posibilidad de volver a enviar después de un error.
  - restauración desde `sessionStorage`;
  - escritura y restauración al recrear la página;
  - descarte de JSON corrupto;
  - no persistencia de carga ni errores.

- [x] 3.13 Eliminar el historial del asistente al cerrar sesión y añadir la prueba correspondiente en `AuthService`.

## 4. Identidad visual

- [x] 4.1 Aplicar la identidad visual definida en:
  - `constitution.md`;
  - `AGENTS.md`;
  - `styles.css`.

- [x] 4.2 Reutilizar las variables CSS globales existentes.

- [x] 4.3 No introducir una nueva paleta visual para el chat.

- [x] 4.4 Diferenciar usuario y asistente mediante composición, alineación, superficies o bordes manteniendo la paleta general neutra.

- [x] 4.5 Mantener comportamiento responsive y accesible.

- [x] 4.6 No añadir librerías externas de chat, estado, iconos o gráficos.

## 5. Verificación

- [x] 5.1 Ejecutar la suite completa del frontend:

  `npm test`

  y corregir únicamente fallos relacionados con este cambio.

- [x] 5.2 Confirmar:
  - 0 pruebas fallidas;
  - ausencia de solicitudes HTTP pendientes;
  - ausencia de errores no controlados de Router/TestBed.

- [x] 5.3 Ejecutar:

  `npm run build`

  y corregir únicamente errores relacionados con este cambio.

- [x] 5.4 Revisar que todas las tareas implementadas estén correctamente marcadas.

- [x] 5.5 Ejecutar:

  `openspec validate "006-ai-finance-assistant-chat" --strict`

- [x] 5.6 Corregir únicamente inconsistencias de los artefactos OpenSpec si la validación falla.

- [x] 5.7 Realizar prueba manual con:
  - backend activo;
  - LM Studio activo;
  - usuario autenticado.

  Comprobar:
  - acceso desde Home;
  - envío de una pregunta;
  - estado de espera;
  - respuesta del asistente;
  - bloqueo de doble envío;
  - recuperación después de error.

- [x] 5.8 Confirmar que no quedan tareas pendientes antes de archivar.
