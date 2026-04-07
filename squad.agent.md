# tetris-game — Squad Agent Configuration (Enmarchia)

> This project uses **Enmarchia**: a unified Spec-Kit + Squad autonomous development workflow.
> Specifications are defined in `specs/` and issues are auto-generated from them.

## Project Context

**Repository:** fjbelizon/tetris-game
**Team name:** Enmarchia Squad

## Enmarchia Autonomous Development Protocol

### What you are doing
You are implementing a software project that has been fully specified using Spec-Driven Development.
The specifications live in `specs/` and every GitHub Issue tagged `enmarchia` is a task
derived directly from those specs.

### How to implement a task

1. **Read the full issue body** — it contains the specification (`spec.md`) and technical plan (`plan.md`)
2. **Check existing code** — understand the codebase before writing anything
3. **Write tests** — every spec item should have at least one test
4. **Implement** — follow the spec, not just the issue title
5. **Review your work** — ensure all acceptance criteria in the spec are met
6. **Open a PR** — title: `feat: [Feature name] <brief description>`, body: `Closes #<issue-number>`

### Quality bar

- All existing tests must pass
- New code must have test coverage
- No security vulnerabilities (follow OWASP Top 10)
- Code must match existing conventions and style

### What to do when blocked

- Add a comment to the issue describing the blocker
- Label the issue `needs-human-review`
- Move on to the next issue

### Spec artifacts location

```
specs/
├── spec-<feature-name>/
│   ├── spec.md        ← What to build (requirements, user stories)
│   ├── plan.md        ← How to build it (tech stack, architecture)
│   └── tasks.md       ← Checklist of tasks (each = one GitHub Issue)
└── CONSTITUTION.md    ← Project principles and guidelines
```

### Squad spec knowledge

The `.squad/enmarchia-specs.md` file contains a full index of all specs.
Each agent's `.squad/agents/<name>/history.md` contains spec context relevant to their role.
