# SCMApp — Software Configuration Management Platform

## Stack

- **Frontend:** Vue 3 + TypeScript + Vite + PrimeVue + Pinia + Vue Router
- **Backend:** ASP.NET Core Web API (.NET 10)
- **Architecture:** Clean Architecture + CQRS (MediatR) — commands and queries in `src/Application/`
- **ORM:** Entity Framework Core + SQLite (persists across rebuild)
- **Auth:** Windows Authentication (Negotiate) for production; `DevAuthMiddleware` for local dev (Ubuntu)
- **Caching:** `ICacheService` wrapper over `IMemoryCache` — keys in `CacheKeys.cs`
- **Logging:** Serilog → rolling `/logs` files (all levels, 30-day retention)
- **Analytics:** `UserActivityMiddleware` captures all `/api` hits

## Backend Conventions

- CQRS via MediatR — commands in `Application/{Entity}/Commands/`, queries in `Application/{Entity}/Queries/`
- No DbContext calls in controllers or endpoints — always go through MediatR
- All writes must call `IAuditService.LogAsync()`
- XML doc on all public endpoint groups
- DTOs only cross the API boundary — never expose EF models directly
- Enum values stored as strings in SQL (`HasConversion<string>()`)
- `IUser.UserName` is the Windows identity name (e.g. `DOMAIN\user`)

## Frontend Conventions

- All API calls go through `/src/api/` — never fetch() inside components or stores
- TypeScript interfaces in `/src/types/index.ts` must mirror backend DTOs exactly
- Use composables (`/src/composables/`) for reusable logic, Pinia stores (`/src/stores/`) for shared state
- PrimeVue components only — do not mix UI libraries
- Run `npm run generate-api` to regenerate `/src/api/generated.ts` from the OpenAPI spec (nswag, fetch template)

## Caching

- Use `ICacheService` wrapper — never call `IMemoryCache` directly
- Cache keys defined in `CacheKeys.cs` — never use inline strings
- Default TTL: 15 minutes
- Always invalidate on write

## Logging

- Serilog → rolling `/logs` files (all levels, 30-day retention)
- Never log CUI values, passwords, tokens, or session IDs
- Log entity IDs only in audit entries — never log field values

## Admin Dashboard (Vue, role-gated)

- /admin/analytics
- /admin/audit
- /admin/errors
