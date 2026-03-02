using LibraryManagement.Application.DTOs.Borrowing;
using LibraryManagement.Application.Manager;
using LibraryManagement.Domain.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    [Route("api/borrow-records")]
    public class BorrowRecordsController:ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public BorrowRecordsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
            
        }
        //Get api/borrow-records
        /// <summary>
        /// Retrieves a paginated list of borrow records with filtering options.
        /// </summary>
        /// <param name="parameters">Filtering options (PatronId, BookId, Status) and pagination.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of borrow records.</returns>
        /// <response code="200">Returns the list of records</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<BorrowRecordDto>>> GetAllRecords([FromQuery] BorrowRecordParameters parameters,CancellationToken cancellationToken)
        {
            var records = await _serviceManager.BorrowService.GetAllRecordsAsync(parameters, cancellationToken);
            return Ok(records);
        }
        //POST  api/borrow-records
        /// <summary>
        /// Borrows a book for a patron.
        /// </summary>
        /// <param name="requestDto">The borrow request details (BookId, PatronId).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created record ID.</returns>
        /// <response code="200">If borrowing was successful</response>
        /// <response code="400">If the book is unavailable or patron not found</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> BorrowBook([FromBody]CreateBorrowRequestDto requestDto,CancellationToken cancellationToken)
        {
            try
            {
                var recordId = await _serviceManager.BorrowService.BorrowBookAsync(requestDto, cancellationToken);
                return Ok(new { Message = "Book borrowed successfully", RecordId = recordId });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //PUT api/borrow-records/5/return
        /// <summary>
        /// Returns a borrowed book.
        /// </summary>
        /// <param name="id">The ID of the borrow record.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Success message.</returns>
        /// <response code="200">If the return was successful</response>
        /// <response code="400">If the record is invalid or already returned</response>
        [HttpPut("{id}/return")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> ReturnBook(int id,CancellationToken cancellationToken)
        {
            try
            {
                await _serviceManager.BorrowService.ReturnBookAsync(id, cancellationToken);
                return Ok(new { Message = "Book returned successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet]
        //public async Task<ActionResult<List<BorrowRecordDto>>> GetAllRecords()
        //{
        //    var records = await _serviceManager.BorrowService.GetAllRecordsAsync();
        //    return Ok(records);
        //}


        // GET   api/borrow-records/overdue
        /// <summary>
        /// Retrieves all overdue borrow records.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of overdue records.</returns>
        /// <response code="200">Returns the list of overdue books</response>
        [HttpGet("overdue")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<BorrowRecordDto>>> GetOverdueRecords(CancellationToken cancellationToken)
        {
            var records = await _serviceManager.BorrowService.GetOverdueRecordsAsync(cancellationToken);
            return Ok(records);
        }

    }
}
