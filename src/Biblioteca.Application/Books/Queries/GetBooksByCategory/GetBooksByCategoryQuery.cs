using Biblioteca.Application.Books.Dtos;
using MediatR;

namespace Biblioteca.Application.Books.Queries.GetBooksByCategory;

public record GetBooksByCategoryQuery(int CategoryId) : IRequest<IReadOnlyList<BookSummaryDto>?>;
