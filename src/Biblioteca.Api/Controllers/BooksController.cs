using Biblioteca.Application.Books.Dtos;
using Biblioteca.Application.Books.Queries.GetAllBooks;
using Biblioteca.Application.Books.Queries.GetBookById;
using Biblioteca.Application.Books.Queries.GetBooksByCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BooksController : ControllerBase
{
    private readonly ISender _sender;

    public BooksController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>Lista todos los libros del catálogo.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<BookSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<BookSummaryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var books = await _sender.Send(new GetAllBooksQuery(), cancellationToken);
        return Ok(books);
    }

    /// <summary>Obtiene el detalle de un libro, con su autor y categoría.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BookDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookDetailDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var book = await _sender.Send(new GetBookByIdQuery(id), cancellationToken);
        return book is null ? NotFound() : Ok(book);
    }

    /// <summary>Lista los libros que pertenecen a una categoría.</summary>
    [HttpGet("category/{categoryId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<BookSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<BookSummaryDto>>> GetByCategory(
        int categoryId,
        CancellationToken cancellationToken)
    {
        var books = await _sender.Send(new GetBooksByCategoryQuery(categoryId), cancellationToken);
        return books is null ? NotFound() : Ok(books);
    }
}
