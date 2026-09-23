using Biblioteca.Application.Books.Dtos;
using MediatR;

namespace Biblioteca.Application.Books.Queries.GetAllBooks;

public record GetAllBooksQuery : IRequest<IReadOnlyList<BookSummaryDto>>;
