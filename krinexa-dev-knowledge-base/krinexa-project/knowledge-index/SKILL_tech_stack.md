---
name: krinexa-tech-stack
description: Read before any Krinexa coding task. Defines the required tech expertise, architectural patterns, and conventions an agent must apply across both Track A (portfolio site) and Track B (marketplace platform).
---

# Krinexa — Agent Tech Expertise

## Identity to adopt

Act as a senior full-stack engineer fluent in the .NET ecosystem and
modern SPA frontends, with production experience in cloud/DevOps and
automated testing. This mirrors the real team expertise the codebase
is meant to demonstrate (see `brand-and-content.md`).

## Required expertise by layer

### Backend
- C#, ASP.NET Core Web API (.NET 8 / current .NET, targeting **.NET 10 LTS** for Track A)
- Clean Architecture: Api → Application → Domain → Infrastructure, dependencies point inward only
- Entity Framework Core + Npgsql (PostgreSQL), Dapper/stored procedures where EF is inappropriate
- Dependency injection, correct service lifetimes (DbContext = scoped, never singleton)
- Async/await for all I/O; `AsNoTracking()` for read-only queries
- Global exception handling → RFC 7807 `ProblemDetails`
- DTOs at every API boundary — **never expose EF entities directly**
- JWT authentication, ASP.NET Identity, OAuth 2.0 / OpenID Connect, role-based authorization (Track B / Phase 2 Track A)
- Structured logging with correlation/trace IDs
- Background jobs (Hangfire) for scheduled tasks (e.g. subscription expiry sweep)

### Frontend
- Angular (recommended) or React + TypeScript (equivalent alternative) — pick one and stay consistent per project, do not mix
- Component architecture: core/shared/features separation (Angular) or components/pages/services/hooks (React)
- API calls isolated inside dedicated services; environment-based `API_BASE_URL`
- Explicit loading, empty, and error states on every data-driven view
- Accessible, semantic HTML; mobile-first responsive design

### Data
- PostgreSQL (Supabase Free for Track A hosting)
- Normalized schema — see `database-schema.md`
- Migrations via EF Core, never manual schema drift

### Testing
- xUnit = primary framework, required for all CI
- NUnit = representative subset, kept for framework familiarity — do not duplicate the full suite
- Moq or NSubstitute for mocking; FluentAssertions for readable assertions
- Integration tests against the real API + PostgreSQL for critical paths

### Cloud / DevOps
- Docker for the API (see `deployment-and-ops.md` for the Dockerfile)
- GitHub Actions: restore → build → test (xUnit + NUnit) → frontend build/lint → deploy
- Vercel (frontend) + Render (API, Docker) + Supabase (Postgres) for Track A, all free tier
- Azure / Azure DevOps / Docker / CI-CD is the target expertise for Track B and any enterprise-facing work
- Secrets only in deployment platform env vars / GitHub Secrets — never in git, commit only `.env.example`

## Conventions the agent must always follow

- **Layering discipline**: domain logic never references Infrastructure; Api never bypasses Application services to hit the DbContext directly.
- **Naming**: PascalCase for C# types/members, camelCase for TS/JS, kebab-case for Angular file names, RESTful plural nouns for routes (`/api/talent/projects`, not `/api/getProjects`).
- **Pagination**: add to any endpoint that can return an unbounded list.
- **Validation**: validate all incoming data at the Application layer (FluentValidation or equivalent), not just client-side.
- **Security defaults**: HTTPS only, CORS restricted to known origins, no stack traces leaked in production error responses, private storage (never public folders) for resumes/payment screenshots.
- **Cost discipline (Track A)**: default to free-tier-compatible choices. Don't introduce Redis, Kafka, RabbitMQ, or paid services unless the task explicitly calls for Track B scale.

## Code-change discipline

This is enforced in full in `../CODE_CHANGE_POLICY.md` — read it before
editing any existing file. Summary: annotate changes with a comment,
never modify a function that already works correctly, prefer reusing
an existing correct function over writing a new one, and only create a
new function when reuse genuinely isn't viable (naming it so its
purpose is obvious, to keep future context/token usage low).
