using Arasva.Core;
using Arasva.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArasvaAssignment.Controllers
{
    /// <summary>
    /// Added by YReddy for #URLContentRead
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReadURLContentController : Controller
    {
        private readonly IReadURLContentService _ulrreaderrepo;
        private readonly IWebHostEnvironment _env;

        public ReadURLContentController(IReadURLContentService ulrreaderrepo, IWebHostEnvironment env)
        {
            _ulrreaderrepo = ulrreaderrepo;
            _env = env; 
        }

        /// <summary>
        /// API used to read the data from URL
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("readurlcontent")]
        public async Task<IActionResult> Uploadfileandreadurlcontent(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            //change file name as per timestamp
            var _name = DateTime.Now.Ticks.ToString() + file.FileName;

            // Optional file type validation
            var allowedExtensions = new[] { ".txt" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return BadRequest("Invalid file type.");

            // Generate folder path
            var uploadDir = Path.Combine(_env.ContentRootPath, AppConstants.URLUploads);
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            // Create unique file name
            var filePath = Path.Combine(uploadDir, _name);

            // Save using async I/O
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
             

            var response = await _ulrreaderrepo.ReadURLContent(filePath, _env.ContentRootPath);
            return response.success ? Ok(response) : BadRequest(response);
        }
    }
}
