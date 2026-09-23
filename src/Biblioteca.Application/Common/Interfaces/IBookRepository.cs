using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Common.Interfaces;

public interface IBookRepository
{
    Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Book>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);
}
