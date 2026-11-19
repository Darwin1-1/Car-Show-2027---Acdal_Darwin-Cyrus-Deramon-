using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IAppDbContext
{
    DbSet<global::Domain.Entities.Registration> Registrations { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
