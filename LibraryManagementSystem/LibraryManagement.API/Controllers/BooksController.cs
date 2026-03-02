using Asp.Versioning;
using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Manager;
using LibraryManagement.Domain.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/books")] 
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public BooksController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        /// <summary>
        /// Retrieves a paginated list of all books.
        /// </summary>
        /// <param name="parameters">Pagination and filtering options.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of books.</returns>
        /// <response code="200">Returns the list of books</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<BookDto>>> GetAllBooks([FromQuery] BookParameters parameters,CancellationToken cancellationToken)
        {
            var books = await _serviceManager.BookService.GetAllBooksAsync(parameters,cancellationToken);
            return Ok(books);
        }
        /// <summary>
        /// Retrieves a specific book by its ID.
        /// </summary>
        /// <param name="id">The book's ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The requested book.</returns>
        /// <response code="200">Returns the book</response>
        /// <response code="404">If the book is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BookDto>> GetBookById(int id,CancellationToken cancellationToken)
        {
            var book = await _serviceManager.BookService.GetBookByIdAsync(id, cancellationToken);
            return Ok(book);
        }
        /// <summary>
        /// Creates a new book in the library.
        /// </summary>
        /// <param name="bookDto">The book details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The newly created book.</returns>
        /// <response code="201">Returns the newly created book</response>
        /// <response code="400">If the input data is null or invalid</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CreateBook([FromBody] CreateBookDto bookDto,CancellationToken cancellationToken)
        {
            var createdBookId = await _serviceManager.BookService.CreateBookAsync(bookDto,cancellationToken);
            //
            return CreatedAtAction(nameof(GetBookById), new { id = createdBookId }, new { id = createdBookId });
        }
        // PUt api/books/{id}
        /// <summary>
        /// Updates an existing book's details.
        /// </summary>
        /// <param name="id">The ID of the book to update.</param>
        /// <param name="bookDto">The updated book details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the update was successful</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> UpdateBook(int id, [FromBody] UpdateBookDto bookDto,CancellationToken cancellationToken)
        {
            await _serviceManager.BookService.UpdateBookAsync(id, bookDto,cancellationToken);
            return NoContent(); //standartuli 204 response updatebistvis
        }

        // DELETE api/books/{id}

        /// <summary>
        /// Deletes a book from the library.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the deletion was successful</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteBook(int id,CancellationToken cancellationToken)
        {
            await _serviceManager.BookService.DeleteBookAsync(id, cancellationToken);
            return NoContent(); // Standartuli 204 response for washlistvis
        }
        // GET api/books/{id}/availability
        /// <summary>
        /// Checks if a book is currently available for borrowing.
        /// </summary>
        /// <param name="id">The book's ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if available, false otherwise.</returns>
        /// <response code="200">Returns the availability status</response>
        [HttpGet("{id}/availability")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<bool>> CheckAvailability(int id,CancellationToken cancellationToken)
        {
            var isAvailable = await _serviceManager.BookService.CheckAvailabilityAsync(id,cancellationToken);
            return Ok(new { BookId = id, IsAvailable = isAvailable });
        }
    }
}

