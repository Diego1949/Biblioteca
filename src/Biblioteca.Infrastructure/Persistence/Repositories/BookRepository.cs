using Biblioteca.Application.Common.Interfaces;
using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Infrastructure.Persistence.Repositories;

public class BookRepository : IBookRepository
{
    private readonly BibliotecaDbContext _context;

    public BookRepository(BibliotecaDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await BooksWithRelations()
            .OrderBy(b => b.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await BooksWithRelations()
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Book>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await BooksWithRelations()
            .Where(b => b.CategoryId == categoryId)
            .OrderBy(b => b.Title)
            .ToListAsync(cancellationToken);
    }

    private IQueryable<Book> BooksWithRelations() =>
        _context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Category);
}
