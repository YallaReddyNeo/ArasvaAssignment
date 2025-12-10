using Arasva.Core.DTO.Create;
using Arasva.Core.DTO.Update;
using Arasva.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArasvaAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingController : Controller
    {
        private readonly IBorrowingService _service;

        public BorrowingController(IBorrowingService service)
        {
            _service = service;
        }

        /// <summary>
        /// Member borrows a book.
        /// </summary>
        [HttpPost("Borrow")]
        [ActionName("Borrow")]
        public async Task<IActionResult> Borrow([FromBody] BorrowRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _service.BorrowBookAsync(dto);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Member returns a borrowed book.
        /// </summary>
        [HttpPost("Return")]
        [ActionName("Return")]
        public async Task<IActionResult> Return([FromBody] ReturnRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _service.ReturnBookAsync(dto);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Complete borrowing history for a given member.
        /// </summary>
        [HttpGet("Member/{memberId}/History")]
        public async Task<IActionResult> GetMemberHistory(int memberId)
        {
            try
            {
                var response = await _service.GetMemberHistoryAsync(memberId);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
