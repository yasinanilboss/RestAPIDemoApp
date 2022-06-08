using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestAPIDemo.Helpers;
using System.Security.Claims;

namespace RestAPIDemo.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]/")]
    public class JwtController : ControllerBase
    {
        [HttpGet]
        public IActionResult Jwt()
        {
            return new ObjectResult(JwtToken.GenerateJwtToken());
        }

        [HttpGet("get")]
        public IActionResult Get()
        {
            string id = HttpContext.User.FindFirstValue("id");
            string email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            string username = HttpContext.User.FindFirstValue(ClaimTypes.Name);

            return Ok();
        }

    }
}
