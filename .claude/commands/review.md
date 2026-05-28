Review the selected code against these criteria:
1. Repository pattern followed — no direct DbContext calls in controllers
2. XML doc comments present on all public members
3. IAuditService.LogAsync() called on all write operations
4. StatusTransitionValidator.Validate() called before all status changes
5. AD role check before any privileged operation
6. No raw SQL — EF Core LINQ only
7. DTOs used at API boundary — no EF models exposed directly
8. No CUI/sensitive values in log statements

Flag issues by severity: Critical / Warning / Suggestion
