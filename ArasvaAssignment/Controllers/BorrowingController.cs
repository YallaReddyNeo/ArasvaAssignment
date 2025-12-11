using Arasva.Core.DTO.Create;
using Arasva.Core.DTO.Update;
using Arasva.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArasvaAssignment.Controllers
{
    [Route("api/v1/[controller]")]
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
        [HttpPost("borrowbook")] 
        public async Task<IActionResult> Borrow([FromBody] BorrowRequestDTO dto)
        {
            var response = await _service.BorrowBookAsync(dto);
            return response.success ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Member returns a borrowed book.
        /// </summary>
        [HttpPost("returnbook")] 
        public async Task<IActionResult> Return([FromBody] ReturnRequestDTO dto)
        {
            var response = await _service.ReturnBookAsync(dto);
            return response.success ? Ok(response) : BadRequest(response);

            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            //try
            //{
            //    var response = await _service.ReturnBookAsync(dto);
            //    return response.success ? Ok(response) : BadRequest(response);
            //}
            //catch (KeyNotFoundException ex)
            //{
            //    return NotFound(ex.Message);
            //}
            //catch (InvalidOperationException ex)
            //{
            //    return BadRequest(ex.Message);
            //}
        }

        /// <summary>
        /// Complete borrowing history for a given member.
        /// </summary>
        [HttpGet("borrowhistory/{memberId}")]
        public async Task<IActionResult> GetMemberHistory(int memberId)
        {
            try
            {
                var response = await _service.GetMemberHistoryAsync(memberId);
                return response.success ? Ok(response) : BadRequest(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
