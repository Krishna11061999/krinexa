# API Contracts

## Track A — Portfolio Website

| Method | Route | Purpose |
|---|---|---|
| GET | /api/profile | Public profile |
| GET | /api/skills | Technical skills |
| GET | /api/experience | Experience timeline |
| GET | /api/team | Team members |
| GET | /api/projects | Project list |
| GET | /api/projects/{id} | Project details |
| GET | /api/articles | Published articles |
| GET | /api/articles/{slug} | Article details |
| GET | /health | Health check |

Phase 2 adds authenticated `POST` / `PUT` / `DELETE` equivalents for
each resource, gated by JWT + role-based authorization.

## Track B — Marketplace Platform

| Method | Route | Purpose |
|---|---|---|
| POST | /api/auth/send-otp | Send email OTP |
| POST | /api/auth/verify-otp | Verify OTP |
| POST | /api/talent/profile | Create/update talent profile |
| GET | /api/talent/profile/me | Get current profile |
| POST | /api/talent/projects | Add project |
| GET | /api/skills | Get skill taxonomy |
| POST | /api/client/requirements | Create requirement |
| GET | /api/client/requirements/{id}/matches | Get candidate matches |
| POST | /api/interviews | Request interview |
| GET | /api/subscription | Get subscription status |
| POST | /api/subscription/payment-proof | Submit payment proof |
| GET | /api/admin/payments/pending | Admin payment queue |
| POST | /api/admin/payments/{id}/verify | Verify payment |
| POST | /api/admin/payments/{id}/reject | Reject payment |
| GET | /api/admin/talent | Review talent profiles |

### New — Candidate interest & project chat

| Method | Route | Purpose |
|---|---|---|
| GET | /api/requirements/open | Browse open-for-interest requirements (talent view) |
| POST | /api/requirements/{id}/interest | Candidate shows interest in a requirement |
| GET | /api/client/requirements/{id}/interested | Client views interested candidates, ranked by score |
| GET | /api/requirements/{id}/thread | Get/create the chat thread for a requirement |
| GET | /api/threads/{id}/messages | Get chat history |
| POST | /api/threads/{id}/messages | Send a chat message (with optional attachment) |

Full matching/subscription/OTP business logic behind these endpoints
is in `business-rules.md`.
