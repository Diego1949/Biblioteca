using Biblioteca.Domain.Common;

namespace Biblioteca.Domain.Entities;

public class Category : Entity
{
    private readonly List<Book> _books = new();

    private Category()
    {
    }

    public Category(string name, string? description = null)
    {
        Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Description = description?.Trim();
    }

    public string Name { get; private set; } = null!;

    public string? Description { get; private set; }

    public IReadOnlyCollection<Book> Books => _books.AsReadOnly();
}
