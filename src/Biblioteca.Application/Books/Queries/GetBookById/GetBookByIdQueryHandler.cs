using Biblioteca.Application.Books.Dtos;
using Biblioteca.Application.Books.Mappings;
using Biblioteca.Application.Common.Interfaces;
using MediatR;

namespace Biblioteca.Application.Books.Queries.GetBookById;

public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDetailDto?>
{
    private readonly IBookRepository _bookRepository;

    public GetBookByIdQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookDetailDto?> Handle(
        GetBookByIdQuery request,
        CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.Id, cancellationToken);
        return book?.ToDetailDto();
    }
}
