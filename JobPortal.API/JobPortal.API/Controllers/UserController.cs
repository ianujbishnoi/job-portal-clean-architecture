using Microsoft.AspNetCore.Mvc;

namespace JobPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
         

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            return Ok("result");
        }
    }
}
