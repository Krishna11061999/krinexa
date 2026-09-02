# Design Mockups

Visual reference for the four core screens, rendered as both live HTML
(open `01-index.html` etc. directly, `styles.css` is shared) and static
PNG for quick review without a browser.

| File | Screen | Notes |
|---|---|---|
| `01-index` | Marketplace homepage | Track B homepage per `knowledge-index/business-rules.md` + PRD §31 |
| `02-registration` | Talent registration | Shows the profile-type selector and dynamic student fields (§6 of the PRD) |
| `03-chat` | Project chat | Client ↔ Krinexa team chat, scoped per requirement — see "New feature" note below |
| `04-project-interest` | Requirement + interested candidates | New feature — see below |

## Design system

Corner-tick cards and hairline rules instead of rounded/shadowed
"SaaS-card" defaults — a deliberate "engineering schematic" motif that
matches the matching-score/structured-data nature of the product.
Tokens (colors, type) are defined once in `styles.css` and shared by
every page; edit there rather than per-page if the palette changes.

- Headline type: TeX Gyre Adventor (geometric sans)
- Body type: TeX Gyre Heros
- Data/score/tag type: TeX Gyre Cursor (monospace)
- Navy `#1D2B4F` (primary), brass `#C1792F` (accent), green `#3F7A5D` (positive/matched signal)

These are system fonts available in this environment for rendering
consistency. If the real product ships different licensed fonts,
swap the `font-family` values in `styles.css` — the rest of the layout
is unaffected.

## New feature captured in these mockups: candidate "Show Interest"

`04-project-interest.html` adds a capability not in the original PRD:
in addition to Krinexa's admin-driven matching, a **client can post a
requirement and candidates can browse it and show interest directly**.
Admin review and consent rules still gate when contact details are
shared — this is additive to the existing matching flow, not a
replacement for it. See the corresponding additions in
`../knowledge-index/business-rules.md`, `database-schema.md`, and
`api-contracts.md`.

`03-chat.html` documents the project-scoped chat: each client
requirement gets its own conversation thread with the Krinexa team
(and, once assigned, the matched developer), rather than one global
inbox — keeping all project communication attributable to a specific
requirement for the audit trail already required in `business-rules.md`.
