using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.DTOs.Patron;
using LibraryManagement.Application.Manager;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatronsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public PatronsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        /// <summary>
        /// Retrieves a list of all patrons.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of patrons.</returns>
        /// <response code="200">Returns the list of patrons</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<PatronDto>>> GetAllPatrons(CancellationToken cancellationToken)
        {
            var patrons = await _serviceManager.PatronService.GetAllPatronsAsync(cancellationToken);
            return Ok(patrons);
        }
        /// <summary>
        /// Retrieves a specific patron by their ID.
        /// </summary>
        /// <param name="id">The patron's ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The requested patron.</returns>
        /// <response code="200">Returns the patron</response>
        /// <response code="404">If the patron is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PatronDto>> GetPatronById(int id,CancellationToken cancellationToken)
        {
            var patron = await _serviceManager.PatronService.GetPatronByIdAsync(id, cancellationToken);
            if (patron == null) return NotFound();
            return Ok(patron);
        }
        /// <summary>
        /// Creates a new patron (library member).
        /// </summary>
        /// <param name="patronDto">The patron details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The newly created patron.</returns>
        /// <response code="201">Returns the newly created patron</response>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult> CreatePatron([FromBody] CreatePatronDto patronDto, CancellationToken cancellationToken)
        {
            var createdId = await _serviceManager.PatronService.CreatePatronAsync(patronDto, cancellationToken);
            return CreatedAtAction(nameof(GetPatronById), new { id = createdId }, new { id = createdId });
        }
        //put api/patrons/{id}
        /// <summary>
        /// Updates an existing patron's details.
        /// </summary>
        /// <param name="id">The ID of the patron to update.</param>
        /// <param name="patronDto">The updated patron details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the update was successful</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> UpdatePatron(int id, [FromBody] UpdatePatronDto patronDto,CancellationToken cancellationToken)
        {
            await _serviceManager.PatronService.UpdatePatronAsync(id, patronDto,cancellationToken);
            return NoContent();
        }
        //dELETE api/patrons/{id}
        /// <summary>
        /// Deletes a patron from the system.
        /// </summary>
        /// <param name="id">The ID of the patron to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the deletion was successful</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeletePatron(int id,CancellationToken cancellationToken)
        {
            await _serviceManager.PatronService.DeletePatronAsync(id, cancellationToken);
            return NoContent();
        }
        //GET api/patrons/{id}books
        /// <summary>
        /// Retrieves all books currently borrowed by a specific patron.
        /// </summary>
        /// <param name="id">The patron's ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of borrowed books.</returns>
        /// <response code="200">Returns the list of borrowed books</response>
        [HttpGet("{id}/books")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<BookDto>>> GetBorrowerBooks(int id,CancellationToken cancellationToken)
        {
            var books = await _serviceManager.PatronService.GetBorrowedBooksAsync(id, cancellationToken);
            return Ok(books);
        }
    }
}
