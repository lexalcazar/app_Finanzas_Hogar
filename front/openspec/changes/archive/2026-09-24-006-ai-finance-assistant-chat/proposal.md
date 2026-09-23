## Why

Las personas usuarias no disponen actualmente de un punto dentro de la aplicación desde el que realizar preguntas en lenguaje natural sobre sus propios movimientos, ingresos, gastos y resúmenes financieros.

El backend ya dispone de un asistente financiero autenticado que interpreta las preguntas, consulta los datos autorizados del usuario y genera una respuesta mediante un modelo local.

El frontend necesita proporcionar una interfaz sencilla para utilizar esta capacidad sin replicar la lógica de IA, interpretación financiera ni acceso a datos existente en backend.

## What Changes

- Añadir una página de chat del asistente financiero en la ruta protegida `/asistente`.

- Incorporar en Home una tarjeta `Tu gestor de finanzas` que navegue al asistente.

- Integrar un `AsistenteService` que consuma exclusivamente:

  `POST /api/Asistente/chat`

  utilizando los contratos HTTP:

  Request:

  ```json
   {
     "mensaje": "¿En qué gasté más durante septiembre de 2026?"
   }

- Conservar el historial de mensajes del chat durante la sesión autenticada actual mediante `sessionStorage`, restaurándolo al volver a abrir la página y eliminándolo al cerrar sesión. No se persistirán estados de carga ni errores.
