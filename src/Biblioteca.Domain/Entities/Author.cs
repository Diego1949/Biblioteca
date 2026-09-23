using Biblioteca.Domain.Common;

namespace Biblioteca.Domain.Entities;

public class Author : Entity
{
    private readonly List<Book> _books = new();

    private Author()
    {
    }

    public Author(string firstName, string lastName, string? biography = null)
    {
        FirstName = Guard.AgainstNullOrWhiteSpace(firstName, nameof(firstName));
        LastName = Guard.AgainstNullOrWhiteSpace(lastName, nameof(lastName));
        Biography = biography?.Trim();
    }

    public string FirstName { get; private set; } = null!;

    public string LastName { get; private set; } = null!;

    public string? Biography { get; private set; }

    public string FullName => $"{FirstName} {LastName}";

    public IReadOnlyCollection<Book> Books => _books.AsReadOnly();
}
