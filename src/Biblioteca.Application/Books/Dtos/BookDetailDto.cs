namespace Biblioteca.Application.Books.Dtos;

public record BookDetailDto(
    int Id,
    string Title,
    string Isbn,
    int PublicationYear,
    AuthorDto Author,
    CategoryDto Category);
