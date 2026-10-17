# ADR-0002: Documentation as code

**Status:** Accepted
**Date:** 2026-10-15

## Context

Documentation (README, design documents, wiki) is a primary deliverable. Systems, mechanics, formulas and decisions must be documented and kept current, and documentation must be in place before implementation.

## Decision

All documentation is Markdown in the source repository: `README.md` and `CHANGELOG.md` at the root, everything else under `Docs/`. `Docs/Home.md` is the documentation home page. Documentation changes are committed in the same commit as the code they describe.

## Alternatives considered

- **GitHub Wiki.** Rejected: a separate repository that is not versioned with the code, so docs and code drift apart. Wikis on private repositories also require a paid GitHub plan.
- **External tool (Notion, Google Docs).** Rejected: not versioned with the code and not Markdown.

## Consequences

- Any commit shows the docs as they were at that point in the code's history.
- GitHub renders the Markdown, including links between documents.
- `Docs/` is outside `Assets/`, so Unity does not import it.
