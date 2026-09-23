namespace Biblioteca.Application.Books.Dtos;

public record BookSummaryDto(
    int Id,
    string Title,
    string Isbn,
    int PublicationYear,
    string Author,
    string Category);
