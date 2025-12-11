using Arasva.Core.DTO.Create;
using Arasva.Core.DTO.Update;
using Arasva.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArasvaAssignment.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly IBookService _service;

        public BookController(IBookService service)
        {
            _service = service;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll([FromQuery] bool? isAvailable, [FromQuery] string? author)
        {
            var response = await _service.GetAllAsync(isAvailable, author);
            return response.success ? Ok(response) : BadRequest(response);
        }
         

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _service.GetByIdAsync(id);
            return response.success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("create")] 
        public async Task<IActionResult> Create(BookCreateDTO bookdto)
        {
            var response = await _service.CreateAsync(bookdto);
            return Ok(response);
        }

        [HttpPut("update/{id}")]
        //[ActionName("Update")]
        public async Task<IActionResult> Update(int id, BookUpdateDTO bookdto)
        {
            var response = await _service.UpdateAsync(id, bookdto);
            return response.success ? Ok(response) : BadRequest(response);
        }
    }
}
