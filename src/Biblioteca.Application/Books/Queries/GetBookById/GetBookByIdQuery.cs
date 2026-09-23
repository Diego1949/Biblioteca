using Biblioteca.Application.Books.Dtos;
using MediatR;

namespace Biblioteca.Application.Books.Queries.GetBookById;

public record GetBookByIdQuery(int Id) : IRequest<BookDetailDto?>;
