# ENMARCHIA

Spec-Kit + Squad en un flujo unico para desarrollo autonomo guiado por especificaciones.

ENMARCHIA es la capa de orquestacion sobre:

- Spec-Kit: define que construir.
- Squad: ejecuta de forma autonoma.

## Versiones soportadas (obligatorias)

Hasta nuevo aviso en ENMARCHIA, se deben usar estas versiones:

- Spec-Kit (specify-cli): v0.5.0
- Squad CLI (@bradygaster/squad-cli): v0.9.1

Version minima recomendada de ENMARCHIA para este flujo: v0.1.2

## Proyectos base referenciados

- Spec-Kit: https://github.com/github/spec-kit (MIT)
- Squad CLI: https://github.com/bradygaster/squad (MIT)

Detalles de cumplimiento de terceros: THIRD_PARTY_NOTICES.md

## Objetivo

Convertir especificaciones en trabajo ejecutable sin ambiguedad:

1. Definir especificaciones en specs/spec-xxx.
2. Convertir tareas a issues de GitHub.
3. Sincronizar conocimiento en agentes Squad.
4. Lanzar ejecucion autonoma.

## Requisitos

- Node.js 20+
- Python 3.11+
- uv
- Git
- GitHub CLI autenticado (gh auth login)
- Token en GITHUB_TOKEN (scope repo)

## Instalacion

1. Instalar ENMARCHIA:

   npm install -g @fjbelizon/enmarchia@0.1.2

   Si ya tienes una version anterior:
   npm install -g @fjbelizon/enmarchia@0.1.2

2. Instalar Spec-Kit CLI:

   uv tool install specify-cli --from git+https://github.com/github/spec-kit.git@v0.5.0

3. Instalar Squad CLI:

   npm install -g @bradygaster/squad-cli@0.9.1

## Estructura esperada de Spec-Kit

ENMARCHIA trabaja sobre esta estructura:

specs/
- CONSTITUTION.md
- spec-xxx/
  - spec.md
  - plan.md
  - tasks.md

Notas:
- Solo se procesan carpetas con prefijo spec-.
- Cada item no completado de tasks.md se transforma en un issue.

## Plan de accion secuencial (recomendado)

### Paso 1. Inicializar proyecto con ENMARCHIA

En un repositorio nuevo o existente:

enmarchia init --owner tu-org-o-user --repo tu-repo --with-spec-kit --with-squad --with-squad-copilot

Compatibilidad:

- ENMARCHIA >= 0.1.1: soporta --with-squad-copilot.
- ENMARCHIA 0.1.0: usa enmarchia init --owner <owner> --repo <repo> --with-spec-kit --with-squad y luego ejecuta squad copilot manualmente.

Que hace este comando:

- Crea .enmarchia.json.
- Crea squad.agent.md y .copilot/enmarchia.md.
- Crea scaffold de specs si no existe.
- Ejecuta specify init mediante ENMARCHIA (si esta disponible).
- Ejecuta squad init y opcionalmente squad copilot mediante ENMARCHIA.

### Paso 2. Redactar especificaciones Spec-Kit

En GitHub Copilot Chat, en este orden:

/speckit.constitution
/speckit.specify
/speckit.clarify
/speckit.plan
/speckit.tasks

Resultado esperado:

- specs/CONSTITUTION.md
- specs/spec-xxx/spec.md
- specs/spec-xxx/plan.md
- specs/spec-xxx/tasks.md

### Paso 3. Crear issues desde tareas

enmarchia bridge

Que hace:

- Lee specs/spec-xxx/tasks.md.
- Crea un issue por tarea pendiente.
- Anota (#numero) en tasks.md para trazabilidad.
- Si autoSync esta activo, sincroniza conocimiento a Squad.

### Paso 4. Sincronizar conocimiento de specs

enmarchia sync

Que hace:

- Genera .squad/enmarchia-specs.md.
- Inyecta contexto de spec y plan en .squad/agents/*/history.md.

### Paso 5. Lanzar ejecucion autonoma de Squad

enmarchia launch

Que hace:

- Ejecuta squad triage --execute con parametros compatibles.
- Mantiene ciclo de trabajo sobre issues etiquetados para ENMARCHIA.

### Paso 6. Monitorear estado end-to-end

enmarchia status

Que valida:

- Estado de specs y tareas.
- Estado de Squad.
- Issues abiertos/cerrados y tareas sin bridge.

## Comandos principales

- enmarchia init
- enmarchia bridge
- enmarchia sync
- enmarchia launch
- enmarchia status

Referencia completa: docs/commands.md

## Configuracion

Archivo: .enmarchia.json

Campos clave:

- specKit.featuresDir: por defecto specs
- specKit.constitutionFile: por defecto specs/CONSTITUTION.md
- squad.dir: por defecto .squad
- bridge.labels: por defecto [enmarchia, squad]

## Criterio operativo

Para operar sin friccion:

1. Mantener actualizado tasks.md con /speckit.tasks.
2. Ejecutar enmarchia bridge despues de cada cambio de tareas.
3. Ejecutar enmarchia sync despues de cambios en spec o plan.
4. Usar enmarchia launch para ejecucion continua.
5. Verificar con enmarchia status antes y despues de cada ciclo.

## Licencia

MIT
