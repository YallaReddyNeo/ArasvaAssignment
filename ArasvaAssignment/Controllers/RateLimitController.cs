using Arasva.Core.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ArasvaAssignment.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [EnableRateLimiting("RateLimitPolicy")]
    public class RateLimitController : Controller
    {
        private readonly IRateLimitRepository _ratelimitrepo; 

        public RateLimitController(IRateLimitRepository ratelimitrepo)
        {
            _ratelimitrepo = ratelimitrepo; 
        }

        /// <summary>
        /// API used to read rate limit access user wise #RateLimit
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("checkaccess")]
        [EnableRateLimiting("RateLimitPolicy")]
        public async Task<IActionResult> CheckUserAccess([FromQuery] string UserId)
        {
            var response = await _ratelimitrepo.CheckUserAccessLimit(UserId);
            return response.success ? Ok(response) : BadRequest(response);
        }
    }
}
