using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Common.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
