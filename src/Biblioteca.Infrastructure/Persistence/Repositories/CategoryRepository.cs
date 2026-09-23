using Biblioteca.Application.Common.Interfaces;
using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly BibliotecaDbContext _context;

    public CategoryRepository(BibliotecaDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AsNoTracking()
            .AnyAsync(c => c.Id == id, cancellationToken);
    }
}
