using ArasvaAssignment.Helper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace ArasvaAssignment.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class URLReaderController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private static readonly HttpClient _httpClient = new HttpClient();

        public URLReaderController(IWebHostEnvironment env)
        {
            _env = env;
        }

        
        [HttpPost("upload-file1")]
        public async Task<IActionResult> UploadFile1(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // Optional file type validation
            var allowedExtensions = new[] { ".txt" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return BadRequest("Invalid file type.");

            // Generate folder path
            var uploadDir = Path.Combine(_env.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            string content;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                content = await reader.ReadToEndAsync();
            }

            // Create unique file name
            var filePath = Path.Combine(uploadDir, $"{Guid.NewGuid()}{extension}");

           // var urls = File.ReadAllLinesAsync(content);

            return Ok(new
            {
                FileName = Path.GetFileName(filePath),
                Size = file.Length,
                Message = "File uploaded successfully."
            });
        }

        [HttpPost("upload-file")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // Optional file type validation
            var allowedExtensions = new[] { ".txt" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return BadRequest("Invalid file type.");

            // Generate folder path
            var uploadDir = Path.Combine(_env.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            // Create unique file name
            var filePath = Path.Combine(uploadDir, $"{Guid.NewGuid()}{extension}");

            // Save using async I/O
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new
            {
                FileName = Path.GetFileName(filePath),
                Size = file.Length,
                Message = "File uploaded successfully."
            });
        }
    }
}
