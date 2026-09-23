using Biblioteca.Application.Books.Dtos;
using Biblioteca.Application.Books.Mappings;
using Biblioteca.Application.Common.Interfaces;
using MediatR;

namespace Biblioteca.Application.Books.Queries.GetBooksByCategory;

public class GetBooksByCategoryQueryHandler
    : IRequestHandler<GetBooksByCategoryQuery, IReadOnlyList<BookSummaryDto>?>
{
    private readonly IBookRepository _bookRepository;
    private readonly ICategoryRepository _categoryRepository;

    public GetBooksByCategoryQueryHandler(
        IBookRepository bookRepository,
        ICategoryRepository categoryRepository)
    {
        _bookRepository = bookRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IReadOnlyList<BookSummaryDto>?> Handle(
        GetBooksByCategoryQuery request,
        CancellationToken cancellationToken)
    {
        // Null distingue "la categoría no existe" (404) de "no tiene libros" (200 con lista vacía).
        if (!await _categoryRepository.ExistsAsync(request.CategoryId, cancellationToken))
        {
            return null;
        }

        var books = await _bookRepository.GetByCategoryIdAsync(request.CategoryId, cancellationToken);
        return books.ToSummaryDtoList();
    }
}
