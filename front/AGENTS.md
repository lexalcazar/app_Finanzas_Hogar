# AGENTS.md

## 1. Objetivo

Este archivo contiene las instrucciones de trabajo para cualquier agente de IA que modifique este proyecto.

Antes de realizar cambios, el agente debe leer y respetar:

1. `constitution.md`
2. este archivo `AGENTS.md`
3. la especificación OpenSpec correspondiente, si existe

Las reglas de `constitution.md` tienen prioridad sobre las decisiones de implementación.

---

## 2. Contexto del proyecto

Este repositorio contiene el frontend de una aplicación de gestión de finanzas personales.  

Tecnologías principales:

* Angular
* TypeScript
* HTML
* CSS
* Angular HttpClient
* Angular Router
* Angular Signals cuando sean apropiados

El frontend consume una API REST desarrollada con ASP.NET Core.

Base de datos del backend:

* PostgreSQL

---

## 3. Ubicación de los proyectos

Raíz de trabajo SDD:
/front

OpenSpec:
/front/openspec

OpenCode:
/front/opencode

Proyecto Angular:
/front/app_Fh_front

Todo código Angular debe crearse o modificarse exclusivamente dentro de:

/front/app_Fh_front

No crear componentes, servicios, modelos ni archivos Angular directamente en /front.

---

## 4. Identidad visual

Las páginas y componentes nuevos o modificados deben respetar la identidad visual definida en `constitution.md`: diseño limpio y minimalista, blanco, negro y tonos grises, tarjetas blancas, contraste accesible, foco y hover visibles, responsividad e iconografía SVG o recursos existentes.

Los ajustes de estilo necesarios para mantener esta coherencia forman parte de la implementación normal de cada nueva pantalla o feature; no requieren una feature OpenSpec independiente. No usar colores llamativos como estilo predominante salvo una necesidad semántica concreta y no añadir dependencias externas únicamente para iconos.

La paleta neutra compartida debe centralizarse en `app_Fh_front/src/styles.css` mediante variables CSS globales y reutilizarse en lugar de declarar colores de identidad por página. Los colores no neutros se reservan para estados semánticos como ingreso, gasto, éxito, error o advertencia.

Antes de crear o modificar una página, revisar los estilos existentes y mantener la identidad visual definida en `constitution.md`.

Toda nueva página debe salir ya integrada visualmente con el resto de la aplicación.

No crear interfaces con una paleta o lenguaje visual diferente salvo que la especificación lo requiera expresamente.

No esperar a una feature posterior para corregir estilos básicos de coherencia visual.
