using Biblioteca.Application.Books.Dtos;
using Biblioteca.Application.Books.Mappings;
using Biblioteca.Application.Common.Interfaces;
using MediatR;

namespace Biblioteca.Application.Books.Queries.GetAllBooks;

public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, IReadOnlyList<BookSummaryDto>>
{
    private readonly IBookRepository _bookRepository;

    public GetAllBooksQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IReadOnlyList<BookSummaryDto>> Handle(
        GetAllBooksQuery request,
        CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetAllAsync(cancellationToken);
        return books.ToSummaryDtoList();
    }
}
