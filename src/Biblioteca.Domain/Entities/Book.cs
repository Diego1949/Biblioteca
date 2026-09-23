using Biblioteca.Domain.Common;
using Biblioteca.Domain.Exceptions;

namespace Biblioteca.Domain.Entities;

public class Book : Entity
{
    private const int EarliestPublicationYear = 1450;

    private Book()
    {
    }

    public Book(string title, string isbn, int publicationYear, int authorId, int categoryId)
    {
        Title = Guard.AgainstNullOrWhiteSpace(title, nameof(title));
        Isbn = NormalizeIsbn(isbn);
        PublicationYear = EnsureValidPublicationYear(publicationYear);
        AuthorId = Guard.AgainstNonPositive(authorId, nameof(authorId));
        CategoryId = Guard.AgainstNonPositive(categoryId, nameof(categoryId));
    }

    public string Title { get; private set; } = null!;

    public string Isbn { get; private set; } = null!;

    public int PublicationYear { get; private set; }

    public int AuthorId { get; private set; }

    public int CategoryId { get; private set; }

    public Author Author { get; private set; } = null!;

    public Category Category { get; private set; } = null!;

    private static string NormalizeIsbn(string isbn)
    {
        var normalized = Guard.AgainstNullOrWhiteSpace(isbn, nameof(isbn))
            .Replace("-", string.Empty)
            .Replace(" ", string.Empty);

        if (normalized.Length is not (10 or 13))
        {
            throw new DomainException("ISBN must have 10 or 13 characters.");
        }

        return normalized;
    }

    private static int EnsureValidPublicationYear(int publicationYear)
    {
        if (publicationYear < EarliestPublicationYear || publicationYear > DateTime.UtcNow.Year)
        {
            throw new DomainException(
                $"Publication year must be between {EarliestPublicationYear} and {DateTime.UtcNow.Year}.");
        }

        return publicationYear;
    }
}
