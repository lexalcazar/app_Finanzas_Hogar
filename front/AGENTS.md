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
