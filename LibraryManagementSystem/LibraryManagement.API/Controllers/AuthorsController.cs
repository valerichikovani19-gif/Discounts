using LibraryManagement.Application.DTOs.Author;
using LibraryManagement.Application.Manager;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AuthorsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        /// <summary>
        /// Retrieves a list of all authors.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of authors.</returns>
        /// <response code="200">Returns the list of authors</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<AuthorDto>>> GetAllAuthors(CancellationToken cancellationToken)
        {
            var authors = await _serviceManager.AuthorService.GetAllAuthorsAsync(cancellationToken);
            return Ok(authors);
        }
        /// <summary>
        /// Retrieves a specific author by their unique ID.
        /// </summary>
        /// <param name="id">The ID of the author.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The requested author.</returns>
        /// <response code="200">Returns the author</response>
        /// <response code="404">If the author is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AuthorDto>> GetAuthorById(int id,CancellationToken cancellationToken)
        {
            var author = await _serviceManager.AuthorService.GetAuthorByIdAsync(id,cancellationToken);
            if (author == null) return NotFound();
            return Ok(author);
        }
        /// <summary>
        /// Creates a new author.
        /// </summary>
        /// <param name="authorDto">The author creation details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The newly created author.</returns>
        /// <response code="201">Returns the newly created author</response>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult> CreateAuthor([FromBody] CreateAuthorDto authorDto,CancellationToken cancellationToken)
        {
            var createdId = await _serviceManager.AuthorService.CreateAuthorAsync(authorDto, cancellationToken);
            return CreatedAtAction(nameof(GetAuthorById), new { id = createdId }, new { id = createdId });
        }
        //PuT api/authors/{id}
        /// <summary>
        /// Updates an existing author's details.
        /// </summary>
        /// <param name="id">The ID of the author to update.</param>
        /// <param name="authorDto">The updated author details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the update was successful</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> UpdateAuthor(int id, [FromBody] UpdateAuthorDto authorDto,CancellationToken cancellationToken)
        {
            await _serviceManager.AuthorService.UpdateAuthorAsync(id, authorDto,cancellationToken);
            return NoContent();
        }
        //DELETE api/authors/{id}
        /// <summary>
        /// Deletes an author from the system.
        /// </summary>
        /// <param name="id">The ID of the author to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the deletion was successful</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteAuthor(int id,CancellationToken cancellationToken)
        {
            await _serviceManager.AuthorService.DeleteAuthorAsync(id,cancellationToken);
            return NoContent();
        }
        //GET api/authors/{id}/books
        /// <summary>
        /// Retrieves all books written by a specific author.
        /// </summary>
        /// <param name="id">The author's ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of books.</returns>
        /// <response code="200">Returns the list of books</response>
        [HttpGet("{id}/books")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> GetBooksByAuthor(int id,CancellationToken cancellationToken)
        {
            var books = await _serviceManager.AuthorService.GetBooksByAuthorAsync(id,cancellationToken);
            return Ok(books);
        }
    }
}