# Krinexa — Development Knowledge Base & Project Structure

This folder is the single source of truth for any developer or AI coding
agent working on Krinexa. It was built from three source documents:

| Source doc | What it defines | Which track |
|---|---|---|
| `Krinexa_Developer_Outsourcing_Marketplace_PRD.docx` | Full marketplace platform (talent matching, subscriptions, interviews) | **Track B** — future product |
| `Krinexa_Organization_Portfolio_Content_and_Structure.docx` | Content, copy, pages, SEO, brand for the team portfolio site | **Track A** — build now |
| `Krinexa_Free_Full_Stack_Website_Blueprint.docx` | Technical implementation plan for the portfolio site (₹0 hosting) | **Track A** — build now |

## Why two tracks, not one

The PRD itself says (Section 34, *Important Recommendation Before
Development*): **do not build the entire marketplace in V1.** The
portfolio site (Track A) is the correct first build — it is small,
free to host, ships fast, and doubles as the interview/credibility
piece. The marketplace (Track B) is a materially larger system
(auth, OTP, matching engine, subscriptions, admin portal) that should
only start once Track A is live and validated.

**Rule for any agent working in this repo: default to Track A unless
the task explicitly says "marketplace" / "Track B."**

## Folder layout

```
krinexa-project/
├── PROJECT_STRUCTURE.md              ← this file (start here)
├── DEVELOPMENT_PLAN.md               ← phased build plan, both tracks
├── EXECUTION_PLAN.md                 ← granular, checkbox-level task list for the *current* phase
├── CODE_CHANGE_POLICY.md             ← rules every agent must follow before touching code
└── knowledge-index/
    ├── README.md                     ← index of this folder
    ├── SKILL_tech_stack.md           ← agent-facing skill file: full tech expertise required
    ├── architecture.md               ← layering, solution structure, both tracks
    ├── database-schema.md            ← tables/entities, both tracks
    ├── api-contracts.md              ← endpoints, both tracks
    ├── business-rules.md             ← Track B domain rules (matching, subscriptions, OTP, privacy)
    ├── brand-and-content.md          ← Track A copy, pages, SEO, team profiles, content rules
    └── deployment-and-ops.md         ← free-tier hosting, CI/CD, env vars, security checklist
└── design/
    └── mockups/                      ← visual reference (HTML + PNG) for core screens
        ├── README.md
        ├── 01-index.html / .png
        ├── 02-registration.html / .png
        ├── 03-chat.html / .png
        ├── 04-project-interest.html / .png
        └── styles.css                ← shared design tokens
```

## Recommended actual application repo layout (once code starts)

This mirrors the blueprint's suggested solution structure, kept
consistent across both tracks so Track B can later be added as new
projects in the same solution without restructuring Track A.

```
Krinexa.sln
├── src/
│   ├── Krinexa.Api/                  # ASP.NET Core Web API — controllers, Program.cs, middleware
│   ├── Krinexa.Application/          # DTOs, service interfaces, services, validators
│   ├── Krinexa.Domain/               # Entities, enums — no framework dependencies
│   └── Krinexa.Infrastructure/       # EF Core DbContext, repositories, migrations, DI wiring
├── tests/
│   ├── Krinexa.Tests.Unit/           # xUnit — primary
│   └── Krinexa.Tests.NUnit/          # NUnit — representative subset
└── frontend/
    └── krinexa-web/                  # Angular (recommended) or React + TypeScript
```

## How to use this knowledge base

1. Before starting any task, read `knowledge-index/SKILL_tech_stack.md`
   — it defines the expertise the agent must apply (stack, patterns,
   conventions).
2. Check `DEVELOPMENT_PLAN.md` to see which phase the project is in.
3. Check `EXECUTION_PLAN.md` for the specific next tasks and check
   them off as completed.
4. Before editing any existing code file, read `CODE_CHANGE_POLICY.md`
   — it is mandatory, not optional.
5. Pull domain detail from the relevant `knowledge-index/*.md` file
   rather than re-reading the original .docx files — the index files
   are the distilled, agent-ready version of the same content.
