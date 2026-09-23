using Biblioteca.Application.Books.Dtos;
using Biblioteca.Domain.Entities;

namespace Biblioteca.Application.Books.Mappings;

public static class BookMappings
{
    public static BookSummaryDto ToSummaryDto(this Book book) => new(
        book.Id,
        book.Title,
        book.Isbn,
        book.PublicationYear,
        book.Author.FullName,
        book.Category.Name);

    public static BookDetailDto ToDetailDto(this Book book) => new(
        book.Id,
        book.Title,
        book.Isbn,
        book.PublicationYear,
        new AuthorDto(book.Author.Id, book.Author.FullName, book.Author.Biography),
        new CategoryDto(book.Category.Id, book.Category.Name, book.Category.Description));

    public static IReadOnlyList<BookSummaryDto> ToSummaryDtoList(this IEnumerable<Book> books) =>
        books.Select(ToSummaryDto).ToList();
}
