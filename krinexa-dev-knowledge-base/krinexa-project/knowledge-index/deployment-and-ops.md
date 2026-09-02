# Deployment & Ops

## Track A — ₹0/month application hosting target

| Component | Free option | Notes |
|---|---|---|
| Git repository | GitHub Free | |
| CI/CD | GitHub Actions | ₹0 on public repo |
| Frontend | Vercel Hobby | custom domain + HTTPS |
| Backend | Render Free (Docker) | spins down after 15 min inactivity |
| Database | Supabase Free | 500 MB allowance, can pause after inactivity |
| HTTPS | Vercel + Render managed TLS | |
| Email | not added initially | |
| Domain | krinexa.in | already owned; renewal is a separate cost |

## Dockerfile (ASP.NET Core API)

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish src/Krinexa.Api/Krinexa.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:${PORT}
ENTRYPOINT ["dotnet", "Krinexa.Api.dll"]
```

Adjust project paths if the repo layout differs. Build and run the
container locally before pushing to Render.

## Environment variables

```
# Backend / Render
ConnectionStrings__Default=...
AllowedOrigins__0=https://krinexa.in
AllowedOrigins__1=http://localhost:4200

# Frontend / Vercel
API_BASE_URL=https://<your-api>.onrender.com
```

Commit only `.env.example` with placeholders — never real credentials.

## CI/CD pipeline (GitHub Actions)

```
git push → GitHub Actions
  ├─ dotnet restore
  ├─ dotnet build
  ├─ dotnet test (xUnit + NUnit)
  ├─ npm ci
  ├─ frontend test/lint
  ├─ frontend build
  ├─ → Vercel deploy
  └─ → Render deploy
```

Pipeline rules: build fails the pipeline if backend compilation fails;
tests must pass before deploy; frontend build must pass; secrets only
as repo/deployment secrets; use PRs for major changes; keep
main/master always deployable.

## Deployment steps

**Frontend (Vercel Hobby):** create account → import GitHub repo → set
frontend root dir (monorepo) → configure build → set `API_BASE_URL` →
deploy, test `.vercel.app` URL → add `krinexa.in` in Project Settings →
Domains → update DNS at registrar exactly as instructed → verify HTTPS.

**Backend (Render Free):** create account, connect GitHub → create Web
Service → deploy with Dockerfile → select Free compute plan → set DB
connection string + allowed origins as env vars → make the app listen
on the `PORT` env var → deploy, test the `.onrender.com` URL.

**Database (Supabase Free):** create project (Free plan) → create
schema via EF Core migrations → store connection string only in Render
→ seed portfolio data → monitor free-tier usage.

## Security checklist

- HTTPS in production.
- CORS restricted to required origins.
- No secrets in Git.
- Input validation on every write endpoint.
- EF Core parameterized access only.
- Global exception handling without leaking stack traces.
- Secure auth once admin functionality is added (JWT secrets as
  deployment secrets).
- Security headers where practical.
- Dependency updates / vulnerability checks.

## Reference notes (as of the source blueprint)

- .NET 10 is LTS, supported through November 14, 2028.
- Vercel Hobby: free plan for personal projects, custom domains, Git
  deploys.
- Render Free: custom domains + managed TLS, but usage limits and
  spin-down after 15 min idle; .NET deploys via Docker.
- Supabase Free: 500 MB Postgres allowance per project; can pause when
  inactive.
- GitHub Actions: no runner-minute charges on public repos.
