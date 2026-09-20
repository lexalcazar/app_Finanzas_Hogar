## ADDED Requirements

### Requirement: Consulta filtrada de movimientos
El listado MUST permitir consultas con filtros soportados por la API y mostrar sólo la respuesta resultante. El frontend MUST NOT filtrar por usuario ni aplicar en memoria filtros que la API recibe.

#### Scenario: Resultado filtrado vacío
- **WHEN** una consulta filtrada no devuelve movimientos
- **THEN** el sistema muestra el estado vacío existente
