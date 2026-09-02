# Business Rules — Track B (Marketplace Platform)

Not needed for Track A work. Read this before implementing any
matching, subscription, OTP, or admin feature.

## User types

| User | Purpose |
|---|---|
| Talent / Developer | Profile + skills, experience, projects, searchable |
| Student | Academic + technical + project profile |
| Intern | Skills, internship/project info, availability |
| Client / Organization | Requirement creation, search/match requests |
| Interviewer / Hiring Contact | Reviews candidates, joins interview scheduling |
| Krinexa Admin | Verifies profiles/payments, matches candidates, manages comms |

## Matching logic (MVP) — rule-based, not AI

| Match factor | Weight |
|---|---|
| Technology match | 40% |
| Experience-level match | 20% |
| Project/domain relevance | 15% |
| Availability | 10% |
| Budget fit | 10% |
| Location/work mode | 5% |

A junior candidate with the right stack and immediate availability
should outrank an unnecessarily senior/expensive candidate.

## Subscription model

- 15-day trial from successful account activation.
- ₹10/month plan (MVP validation price, not the long-term model — see
  Section 27 of the PRD for the monetization evolution path).
- Payment: QR code, user emails screenshot to Krinexa, admin verifies
  manually and flips `IsActive = Y`, sets `StartDate`/`ExpiryDate`.
- Access requires `IsActive == "Y" AND CurrentDate < ExpiryDate` —
  enforced both by a scheduled job and at authorization time.
- Screenshots stored in private storage only, never a public folder.

## OTP / auth security

- Cryptographically secure OTP generation.
- Short expiry (5–10 minutes), limited retry attempts, rate-limited
  per email/IP.
- Never store OTP in plaintext if persisted — hash it.
- Invalidate immediately after successful verification.
- Never reveal whether an email already exists (no account
  enumeration).

## Candidate privacy & consent

- Collect only what matching needs; mobile number stays optional.
- Explicit consent required before sharing contact details with a
  client, or before any WhatsApp contact.
- Never expose personal email/mobile publicly.
- Resumes and payment screenshots: protected storage only.
- Audit admin access to profile/payment actions.

## Interview workflow

- Client selects candidate(s) → admin confirms availability → system
  creates interview request → candidate notified by email (and
  WhatsApp only if consented, via an approved Business API — no
  unofficial automation).
- Status values: Requested, Confirmed, Rescheduled, Completed,
  Rejected, Cancelled.

## Candidate "Show Interest" (addition to PRD matching flow)

In addition to admin-driven matching, clients can post an open
requirement that approved talent can browse directly, and a candidate
can express interest in it without waiting to be matched.

- Only approved, email-verified profiles can browse open requirements
  or show interest — same eligibility gate as admin matching.
- Showing interest does **not** bypass admin review or the privacy/
  consent rules above — contact details are still shared only after
  admin approval and candidate consent.
- A requirement's status includes `Open for interest` alongside the
  existing matching statuses; a client can disable candidate interest
  on a given requirement if they want Krinexa-only matching.
- Interested candidates are shown to the client ranked by the same
  match-score formula as admin matching (see Matching logic above),
  not just by submission time.
- See `design/mockups/04-project-interest.html` for the reference UI
  and `database-schema.md` / `api-contracts.md` for the supporting
  data model and endpoints.

## Project chat (addition — client/team communication)

Each requirement gets its own chat thread between the client and the
Krinexa team (and the assigned developer once matched), rather than a
single global inbox. This keeps all communication attributable to a
specific requirement for the audit trail already required below, and
supports file attachments (e.g. a requirement brief). See
`design/mockups/03-chat.html` for the reference UI.

## Admin dashboard scope

Overview (activity counts), Talent (search/filter/approve/reject),
Requirements (review + assign matches), Matching (view/adjust score),
Interviews (schedule/track), Payments (verify/reject screenshots),
Subscriptions (active/expired history), Audit (admin action log).

## Search & filtering (talent)

Technology, experience level (Student/Intern/Junior/Mid/Senior), years
of experience, availability, work mode, location, budget/rate,
project/domain experience, email-verified flag, profile-approved flag.

## Critical business rules (quick reference)

| Rule | Behavior |
|---|---|
| Email verification | Not fully active until OTP succeeds |
| Profile approval | Admin approves/rejects before matching |
| Student fields | College + project info required |
| Mobile | Optional; consent required before sharing |
| Client matching | Only approved/eligible profiles matched |
| Subscription | Requires IsActive = Y and CurrentDate < ExpiryDate |
| Expiry | Expired subscription becomes N |
| Payment | Only admin-verified payment activates subscription (MVP) |
| Interview | Communication is logged |
| Privacy | Personal contact info never public |
