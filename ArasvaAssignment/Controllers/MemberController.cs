using Arasva.Core.DTO.Create;
using Arasva.Core.DTO.Update;
using Arasva.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArasvaAssignment.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MemberController : Controller
    {
        private readonly IMemberService _service;

        public MemberController(IMemberService service)
        {
            _service = service;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllAsync();
            return response.success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _service.GetByIdAsync(id);
            return response.success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("create")] 
        public async Task<IActionResult> Create(MemberCreateDTO memberDto)
        {
            var response = await _service.CreateAsync(memberDto);
            return response.success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("update/{id}")] 
        public async Task<IActionResult> Update(int id, MemberUpdateDTO memberDto)
        {
            var response = await _service.UpdateAsync(id, memberDto);
            return response.success ? Ok(response) : BadRequest(response.error = $"Member with ID {id} not found.");
        }
    }
}
