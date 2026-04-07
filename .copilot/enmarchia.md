# Enmarchia Copilot Configuration

This project uses Enmarchia — a Spec-Kit + Squad autonomous development pipeline.

## Working with this project

- All features are defined in `specs/` using Spec-Driven Development
- GitHub Issues tagged `enmarchia` are auto-generated from specifications
- Squad agents work autonomously on those issues 24/7

## Agent Instructions

1. **Read the spec first**: Every `enmarchia` issue contains the full `spec.md` in the body
2. **Follow the plan**: The `plan.md` in the issue body defines the technical approach
3. **Test everything**: Spec-driven means every acceptance criterion must have a test
4. **Reference the issue**: All PRs must reference their originating issue
