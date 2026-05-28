using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SCMApp3.Infrastructure.Data.Interceptors;

// Domain events are not used in DB-first — scaffolded entities have no BaseEntity.
// Keeping this class as a no-op so the DI registration compiles without changes.
public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
{
    public DispatchDomainEventsInterceptor(IMediator mediator) { }
}
