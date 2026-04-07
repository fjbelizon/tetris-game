# ENMARCHIA

Spec-Kit + Squad en un flujo unico para desarrollo autonomo guiado por especificaciones.

ENMARCHIA es la capa de orquestacion sobre:

- Spec-Kit: define que construir.
- Squad: ejecuta de forma autonoma.

## Versiones soportadas (obligatorias)

Hasta nuevo aviso en ENMARCHIA, se deben usar estas versiones:

- Spec-Kit (specify-cli): v0.5.0
- Squad CLI (@bradygaster/squad-cli): v0.9.1

Version minima recomendada de ENMARCHIA para este flujo: v0.1.4

## Proyectos base referenciados

- Spec-Kit: https://github.com/github/spec-kit (MIT)
- Squad CLI: https://github.com/bradygaster/squad (MIT)

Detalles de cumplimiento de terceros: THIRD_PARTY_NOTICES.md

## Objetivo

Convertir especificaciones en trabajo ejecutable sin ambiguedad:

1. Definir especificaciones en specs/{spec-name}.
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

## Limitaciones conocidas

**Squad Configuration debe estar en rama default (main)**

La configuración de Squad (.squad/team.md y .squad/agents/*) reside en la rama por defecto del repositorio (generalmente `main`). GitHub Actions y otros procesos de integración leen esta configuración desde la rama default.

- Si ejecutas `enmarchia bridge` desde una rama de feature, asegúrate de que .squad/team.md y .squad/agents/* estén mergeados a main.
- Si no están en main, GitHub Actions comentará "No squad member found matching label squad:<name>" en las issues, incluso si el equipo existe en tu rama local.

**Solución:** Mergea los cambios de `.squad/*` a main antes de ejecutar `enmarchia bridge`, o ejecuta bridge solo desde main.

## Instalacion

1. Instalar ENMARCHIA:

   npm install -g @fjbelizon/enmarchia@0.1.4

   Si ya tienes una version anterior:
   npm install -g @fjbelizon/enmarchia@0.1.4

2. Instalar Spec-Kit CLI:

   uv tool install specify-cli --from git+https://github.com/github/spec-kit.git@v0.5.0

3. Instalar Squad CLI:

   npm install -g @bradygaster/squad-cli@0.9.1

## Estructura esperada de Spec-Kit

ENMARCHIA trabaja sobre esta estructura:

specs/
- CONSTITUTION.md
- {spec-name}/
  - spec.md
  - plan.md
  - tasks.md

Notas:

- Se procesan carpetas específicas al ejecutar `enmarchia bridge --spec-path "specs/{spec-name}"`.
- Cada item no completado de tasks.md se transforma en un issue.
- En 0.1.2+, ambos patrones son soportados (generados por /speckit.specify).

## Plan de accion secuencial (recomendado)

### Paso 1. Inicializar proyecto con ENMARCHIA

En un repositorio nuevo o existente:

enmarchia init --owner tu-org-o-user --repo tu-repo --with-spec-kit --with-squad --with-squad-copilot

Compatibilidad:

- ENMARCHIA >= 0.1.4: evita `spawn EINVAL` en `launch` (Windows/Node.js 24), evita duplicado de `--execute` y autocrea labels `squad:<member>` faltantes antes del triage.
- ENMARCHIA >= 0.1.3: inicializa Spec-Kit automáticamente en Windows, soporta carpetas nnn-xxx, bridge requiere --spec-path, `--force` reprocesa tareas anotadas y detecta `tasks.md` con CRLF en Windows.
- ENMARCHIA >= 0.1.3: incluye fix ESM para evitar `ReferenceError: require is not defined` al sincronizar conocimiento de Squad en Node.js 24.
- ENMARCHIA 0.1.1: soporta --with-squad-copilot pero puede tener problemas con Spec-Kit en Windows.
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
- specs/{spec-name}/spec.md
- specs/{spec-name}/plan.md
- specs/{spec-name}/tasks.md

### Paso 3. Crear issues desde tareas

**Prerequisito:** Asegúrate de que .squad/team.md y .squad/agents/* estén en la rama default (main). Si trabajas en una rama de feature, mergea primero los cambios de Squad a main.

enmarchia bridge --spec-path specs/001-tetris-console-game

o

enmarchia bridge --spec-path spec-user-auth

o

enmarchia bridge --spec-path C:\proyectos\mi-repo\specs\001-tetris-console-game

Que hace:

- Lee tasks.md de la carpeta indicada en --spec-path.
- Crea un issue por tarea pendiente.
- Anota (#numero) en tasks.md para trazabilidad.
- Con --force, reprocesa tareas no completadas aunque ya tengan (#N), y actualiza la anotacion al nuevo issue.
- Si autoSync esta activo, sincroniza conocimiento a Squad.

Nota: En 0.1.2+, --spec-path es obligatorio para mapear directamente a la especificación destino en Squad.
Tambien acepta id directo, ruta relativa o ruta absoluta, con separadores / o \\.

### Paso 4. Sincronizar conocimiento de specs

enmarchia sync

Que hace:

- Genera .squad/enmarchia-specs.md.
- Inyecta contexto de spec y plan en .squad/agents/*/history.md.

### Paso 5. Lanzar ejecucion autonoma de Squad

enmarchia launch

Que hace:

- Ejecuta squad triage en modo de ejecucion continua con parametros compatibles.
- Si `.squad/team.md` no tiene miembros, genera automaticamente un equipo base a partir de tareas pendientes y continua el arranque.
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
