# Execution Plan

Granular, checkbox-level tasks. This file tracks **current** progress —
update it (check items off, move the "Current phase" marker) as work
lands. `DEVELOPMENT_PLAN.md` stays high-level and rarely changes;
this file is the working list.

**Current phase: Track A, Phase 1 — not yet started.**

---

## Phase 1 — Repo & solution skeleton

- [ ] Create GitHub repository (public, for free Actions minutes)
- [ ] `dotnet new sln -n Krinexa`
- [ ] `dotnet new webapi -n Krinexa.Api`
- [ ] `dotnet new classlib -n Krinexa.Application`
- [ ] `dotnet new classlib -n Krinexa.Domain`
- [ ] `dotnet new classlib -n Krinexa.Infrastructure`
- [ ] `dotnet new xunit -n Krinexa.Tests.Unit`
- [ ] `dotnet new nunit -n Krinexa.Tests.NUnit`
- [ ] `dotnet sln add **/*.csproj`
- [ ] Wire project references: Api → Application → Domain; Infrastructure → Application + Domain
- [ ] Frontend shell: `ng new krinexa-web --routing --style=scss` (or Vite React-TS equivalent)
- [ ] `.gitignore` for both .NET and Node
- [ ] `.env.example` with placeholder keys only
- [ ] Commit skeleton, open PR, verify branch protection on main

## Phase 2 — Core read API

- [ ] Domain entities: `Profile`, `Skill`, `Experience`, `TeamMember`, `Project`, `Technology`, `Article` (see `knowledge-index/database-schema.md`)
- [ ] Application DTOs + service interfaces for each entity
- [ ] Infrastructure: `KrinexaDbContext`, repository implementations
- [ ] Api controllers for all Track A endpoints (see `knowledge-index/api-contracts.md`)
- [ ] `/health` endpoint
- [ ] Global exception handling middleware → ProblemDetails
- [ ] DI registration in `Program.cs`

## Phase 3 — Database

- [ ] Supabase Free project created
- [ ] Npgsql + EF Core packages added (see `knowledge-index/deployment-and-ops.md` for package list)
- [ ] Initial migration generated and applied
- [ ] Seed script for Profile, Skills, Experience, Team, Projects, Technology
- [ ] Connection string stored only in local `.env` / Render env (never committed)

## Phase 4 — Frontend integration

- [ ] Angular/React services per resource, reading `API_BASE_URL` from environment config
- [ ] Home, About, Experience, Team, Skills, Projects, Contact pages wired to live API
- [ ] Loading / empty / error states on every data view
- [ ] Mobile-first responsive pass
- [ ] Content and copy sourced from `knowledge-index/brand-and-content.md` — no placeholder client testimonials

## Phase 5 — Tests

- [ ] xUnit tests: profile service, projects service (published-only filter), missing-project 404, article-by-slug, invalid-input validation, exception handler safety, empty-DB handling, `/health` success
- [ ] NUnit representative subset covering the same core cases (not a full duplicate suite)
- [ ] `dotnet test` green locally before pushing

## Phase 6 — CI

- [ ] GitHub Actions workflow: restore → build → test (xUnit + NUnit) → npm ci → frontend lint/test → frontend build
- [ ] Workflow fails the build on any red step
- [ ] Secrets added as GitHub Actions secrets (none in code)

## Phase 7 — Deploy

- [ ] Render Web Service created, Dockerfile deploy, Free plan, env vars set, listens on `PORT`
- [ ] Vercel project created, frontend root configured, `API_BASE_URL` set, deployed
- [ ] Both live URLs smoke-tested

## Phase 8 — Domain

- [ ] `krinexa.in` added in Vercel → Domains
- [ ] DNS updated at registrar exactly as Vercel instructs
- [ ] HTTPS verified on the public domain

## Phase 9 — Polish

- [ ] SEO metadata + sitemap (see `knowledge-index/brand-and-content.md` for title/description/keywords)
- [ ] Accessibility pass (semantic HTML, alt text, contrast)
- [ ] Performance pass (image optimization, bundle size)

## Phase 10 — Track A Phase 2 (admin)

- [ ] JWT auth, admin login
- [ ] CRUD for projects, experience, skills, team members
- [ ] Publish/unpublish articles
- [ ] Role-based authorization if multiple admins introduced

---

## Track B execution

Not started. Do not begin until Track A's Definition of Done (in
`DEVELOPMENT_PLAN.md`) is met. When ready, expand this file's Track B
section into the same checkbox granularity, sprint by sprint, pulling
detail from `knowledge-index/business-rules.md`.
