# Development Plan

Two tracks, built sequentially. Do not start Track B until Track A's
Definition of Done is met and the site is live.

---

## Track A — Portfolio Website (krinexa.in)

| Phase | Deliverable |
|---|---|
| 1 | GitHub repo, .NET solution skeleton, Angular/React shell |
| 2 | Profile, skills, experience, team, projects API (Application + Domain + Infrastructure) |
| 3 | EF Core + PostgreSQL (Supabase) + migrations + seed data |
| 4 | Responsive frontend, integrated with the API |
| 5 | xUnit tests + representative NUnit tests |
| 6 | Docker + GitHub Actions CI |
| 7 | Deploy frontend (Vercel) + API (Render) |
| 8 | Connect krinexa.in, verify HTTPS |
| 9 | SEO, accessibility, performance pass |
| 10 | Phase 2: admin authentication + content management (CRUD, JWT) |

### Definition of Done — Track A V1

- `https://krinexa.in` loads over HTTPS.
- Home, About, Experience, Team, Skills, Projects work on mobile and desktop.
- Frontend consumes the ASP.NET Core API.
- API deployed and reachable; PostgreSQL data persists in Supabase.
- xUnit tests pass; NUnit tests pass; GitHub Actions passes build+test.
- Frontend and backend both deploy successfully.
- `/health` endpoint works.
- CORS restricted; no production secrets in Git.
- Basic SEO metadata + sitemap configured.

---

## Track B — Marketplace Platform (future, after Track A ships)

| Sprint | Deliverable |
|---|---|
| 1 | Brand, landing page, registration, OTP verification |
| 2 | Student/intern/junior/experienced profile forms |
| 3 | Skills, projects, GitHub links, profile approval |
| 4 | Client requirement form and search |
| 5 | Rule-based matching and candidate shortlist |
| 6 | Interview request + email workflow |
| 7 | ₹10 subscription + QR + payment proof + admin verification |
| 8 | Admin dashboard + expiry automation |
| 9 | xUnit/NUnit tests + security + audit logging |
| 10 | Deployment, domain, SEO, production launch |

### Definition of Done — Track B MVP

- krinexa.in presents Krinexa as a one-stop technology talent organization.
- Talent can register as student, intern, junior, or experienced; email OTP works.
- Student-specific fields display dynamically; skills stored in normalized tables.
- Projects can carry repository/demo links; mobile number stays optional.
- Client can submit a requirement; admin can view/filter/shortlist talent.
- Interview requests + email notifications work.
- ₹10 monthly subscription recorded; 15-day trial tracked.
- QR payment proof submitted; admin can verify/reject; IsActive Y/N; ExpiryDate enforced; expired → N.
- Admin actions audited.
- xUnit/NUnit cover core business rules.
- Deployed on the free-tier architecture where limits permit.

### Before Track B goes commercial

Define who pays the ₹10, what access it grants, refund rules, tax/invoice
requirements, and terms of service — before charging users.

---

## Long-term vision (post-MVP, both tracks)

- Verified talent marketplace, developer availability calendars, project
  ratings, automated skill/profile verification, coding assessments.
- AI-assisted matching once the rule-based system is stable.
- Automated payment gateway + subscription webhooks.
- WhatsApp Business API integration.
- Contracts, timesheets, project management, organization dashboards.
- Additional Krinexa service/product verticals.
