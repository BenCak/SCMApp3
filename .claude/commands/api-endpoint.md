Scaffold a new API endpoint pair for the given entity.
Generate:
1. Controller method (GET/POST/PUT/PATCH) with XML doc comments
2. Request/Response DTO classes in SCMApp.Api/DTOs/
3. Repository interface method in SCMApp.Domain/Repositories/Interfaces/
4. Repository implementation in SCMApp.Infrastructure/Repositories/
5. Audit log call on all write operations
6. Role authorization attribute

Follow existing patterns in SystemsController.cs and SystemRepository.cs
