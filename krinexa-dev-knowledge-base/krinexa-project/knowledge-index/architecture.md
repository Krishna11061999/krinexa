# Architecture

## Track A — Portfolio Website (krinexa.in)

```
Internet
  │
  ▼
https://krinexa.in
  │
  ▼
Angular / React Frontend (Vercel)
  │  HTTPS REST API
  ▼
ASP.NET Core Web API (Render, Docker)
  │  EF Core / Npgsql
  ▼
PostgreSQL (Supabase Free)

GitHub ──► GitHub Actions ──► build + test
       ├──► Vercel ──► frontend deploy
       └──► Render ──► API deploy
```

The public domain points at the frontend. The API stays on Render's
free `onrender.com` subdomain initially — no second domain needed.

Build as a **modular monolith**: one API, one frontend, one Postgres
database. No microservices, Kafka, or Redis for V1.

## Track B — Marketplace Platform (future)

```
Frontend (Angular/React + TypeScript)
  │  HTTPS REST API
  ▼
ASP.NET Core Web API
  │
  Application Layer
  │
  Domain Layer
  │
  Infrastructure Layer
  │              │
  ▼              ▼
PostgreSQL   External services
              ├─ Email
              ├─ WhatsApp provider (approved Business API only)
              └─ Payment gateway (future — manual QR verification in MVP)

Admin Portal ──► Profile review, Requirement review, Candidate matching,
                 Payment verification, Subscription management,
                 Interview alignment
```

Same Clean Architecture layering as Track A, scaled up with an Admin
Portal and additional external integrations. Do not build this until
Track A is live (see `PROJECT_STRUCTURE.md`).

## Solution structure (shared pattern)

```
src/
  Krinexa.Api/            Controllers, Middleware, Extensions, Program.cs
  Krinexa.Application/    DTOs, Interfaces, Services, Validators
  Krinexa.Domain/         Entities, Enums
  Krinexa.Infrastructure/ Persistence, Repositories, Migrations, DI
tests/
  Krinexa.Tests.Unit/     xUnit
  Krinexa.Tests.NUnit/    NUnit (representative subset)
frontend/
  krinexa-web/            Angular or React + TypeScript
```

### Frontend structure — Angular (recommended)
```
src/app/
  core/        services/ guards/ interceptors/
  shared/      components/ models/
  features/    home/ about/ experience/ team/ skills/ projects/ articles/ contact/
  app.routes.ts
  app.config.ts
```

### Frontend structure — React (alternative)
```
src/
  components/  pages/  services/  hooks/  models/  layouts/  assets/
```

## Backend engineering rules (both tracks)

- DTOs at API boundaries; never expose EF entities directly.
- Dependency injection with correct service lifetimes; DbContext = scoped.
- Async/await for all database and external I/O.
- Validate all incoming data.
- Global exception handling with `ProblemDetails`.
- `AsNoTracking()` for read-only queries.
- Paginate lists that can grow large.
- Structured logs with correlation/trace IDs.
- Secrets stay out of source control.
