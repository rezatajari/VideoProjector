using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VideoProjector.Services.Impelements;

namespace VideoProjector.Controllers
{
    [Route(template: "api/[controller]")]
    [ApiController]
    public class AccountController(AccountService accountService) : ControllerBase
    {

        [Route(template:"account/login"),HttpPost]
        public IActionResult Login(string email, string password)
        {
            EntityEntry entityEntry = accountService.Login(email, password);
            if (entityEntry == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            return StatusCode(StatusCodes.Status200OK);
        }


    }
}
