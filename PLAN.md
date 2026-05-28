# Next-Gen Tool — Architecture POC Plan

## Why

Our current tool is a legacy ASP.NET WebForms app — layered but tangled, hard
to test, and on a dead platform. We are modernizing. This POC proves out an
approach to replace it.

Goal: prove the architecture to ourselves and the team before committing.
Built on the Northwind sample DB so it's familiar and throwaway.

## What we're building toward

A clean, modern, **simple** architecture — not an over-engineered one. Fast to
develop, easy for a rotating team to pick up, and structured enough that it
won.t re-tangle over time.

## The decisions

**Stack**
- Backend: ASP.NET Core Web API, .NET 10, Minimal APIs (per-feature endpoint groups)
- Data: EF Core over SQL Server (local Northwind for the POC)
- Frontend: Vue 3 + TypeScript + Vite + PrimeVue + Pinia + Vue Router

**Architecture: pragmatic layered, dependencies point inward**
- Domain — entities, business rules. References nothing.
- Application — service interfaces, DTOs.
- Infrastructure — EF Core, data access, service implementations, entity→DTO translation.
- Api — thin endpoints. Wires it together.
- Separation enforced by project references, so it survives team rotation —
  it's mechanically hard to violate, not just documented.

**Plain services, NOT CQRS/MediatR.** Logic lives in services like
`RequestService.Release(id, user)` — close to a Rails service object, easy for
the team to read. MediatR can be added later if boilerplate pain shows up; the
service interfaces stay either way.

**Database-first.** The team designs features at the schema first, then builds
up to the UI — that's how we already think. The SQL Server schema is the source
of truth; code follows it. We do NOT use EF migrations to drive the database.

**Separate objects everywhere.** Every table maps to an entity, but the API/UI
work with a separate DTO, with translation in the service. A schema change is
absorbed at that seam instead of rippling into logic and UI. This is the
discipline our current tool lacked.

**Custom code generator.** A small tool that, given a table, produces the
backend slice (entity → DTO → service → endpoint) in our exact conventions.
- It runs EF's scaffolder under the hood to read the table and produce the
  entity (EF handles SQL→C# type mapping, including odd types in the real DB),
  then generates the DTO, service, and endpoint on top.
- Re-run safety via **partial classes**: generated code (e.g.
  `ProductService.Generated.cs`) is always overwritten; hand-written logic
  (`ProductService.cs`) is never touched. Re-running refreshes boilerplate and
  leaves real logic alone.
- Scope to start: backend slice only. Vue generation comes later.

## Everyday workflow this enables

1. Developer adds/changes a table in SQL Server (how we already work).
2. Run the generator for that table → working backend slice appears.
3. Hand-write the real rules in the non-generated service file (e.g.
   Pending→Released with an audit entry).
4. Wire up the Vue view.

Plain CRUD tables cost almost nothing. The few tables with real logic get the
structure they deserve, protected by the DTO seam.

## Testing (a first-class pillar)

"Hard to test" is the main pain we're escaping, so testing is core to this POC,
not an afterthought. We build the full pyramid:

- **Unit tests (the base, the most of them).** Service logic tested in isolation
  — no database, no UI. This is the direct payoff of the architecture: because
  logic lives in services behind interfaces, the Pending->Released-with-audit
  rule can be tested directly. Framework: **xUnit**.
- **Integration tests (fewer).** A service against a real test SQL Server, to
  verify EF mappings and queries actually work — the database-shaped bugs unit
  tests can't catch.
- **End-to-end tests (a focused few at the top).** **Playwright** drives the real
  Vue UI through real workflows (log in, release a request, see the audit entry),
  testing frontend + API together as a user would. Great fit for our Vue + TS
  stack.

Shape: a pyramid — many fast unit tests, fewer integration tests, a small set of
Playwright journeys covering critical paths. We deliberately avoid over-investing
in slow E2E tests; the value is in the fast base.

**The generator emits a test stub per slice.** Every generated backend slice
comes with a ready-to-fill xUnit test file. This makes testing the default path
rather than the thing skipped under deadline pressure — the main way the tool
stays tested as the team rotates.



**Audit log — core, built in the POC.** Recording who changed what and when is
part of our business rules, not an add-on (e.g. a state change must write an
audit entry). It lives in the hand-written service logic alongside the rule it
supports. If nearly every operation ends up auditing, that pervasiveness is the
signal to consider a centralized mechanism later.

**Analytics — in scope, two kinds, two designs.**
- *Usage tracking* (who did what) overlaps with audit — both record that an
  action happened. We capture the event once and let usage analytics read from
  it, rather than building a second parallel system.
- *Business reporting* (dashboards, metrics from the data) is a READ concern —
  often heavy aggregate queries. It gets its own read-side services, kept
  separate from the transactional services so reporting load and complexity stay
  out of everyday CRUD. This is the most likely future home for Dapper (complex
  report queries) and, if it grows heavy, the one place CQRS would earn its keep
  later — on the read side only.

Note for the team: reads and writes diverge here. Everyday feature work is
write-shaped (forms, state changes); reporting is read-shaped (aggregate,
read-only). Separate services is the lightweight version of that split.

**Where audit/analytics data lives.** In the main database for the POC, in
audit/event tables alongside the data. The documented future step is a dedicated
analytics store (separate DB) once reporting volume justifies it — the read-side
service seam means that move won't disturb callers.

**Caching — deferred, designed-for.** Not built now. Because everything sits
behind service interfaces, caching can be added per-operation later without
callers knowing. Reporting reads are the likely first place it helps.

## What this POC will demonstrate

- The full vertical slice for a couple of tables (e.g. Products, Categories).
- One feature with real business logic (a state transition + audit log) to show
  where hand-written logic lives and how it's protected.
- One reporting read (a read-side service with an aggregate query) to prove the
  read/write separation works in practice, not just on paper.
- The full test pyramid in action: unit tests on the service logic, an
  integration test against test SQL Server, and a Playwright E2E journey.
- The generator producing a slice from a real table, test stub included.

## Open / deferred

- Vue code generation (backend generation first).
- Auth (JWT) — wire up once the core pattern is proven.
- Dedicated analytics store (separate DB) — main DB for now; move when reporting
  volume justifies it.
- Caching — designed-for via the service seam; build when a real perf need shows.
- Whether the generator graduates from "adopt EF scaffold under the hood" to a
  fuller custom tool, based on how it feels in practice.
