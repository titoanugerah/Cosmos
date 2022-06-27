using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cosmos.Controllers
{

    public class AuthController : Controller
    {
        private readonly ILogger logger;
        private readonly DatabaseContext databaseContext;
        private const string AuthSchemes =
            CookieAuthenticationDefaults.AuthenticationScheme + "," +
            OpenIdConnectDefaults.AuthenticationScheme;

        public AuthController(ILogger<AuthController> _logger, DatabaseContext _databaseContext)
        {
            logger = _logger;
            databaseContext = _databaseContext;
        }

        [Authorize(AuthenticationSchemes = AuthSchemes)]
        public async Task<IActionResult> Login()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var authenticatedUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    databaseContext.User.Where(x => x.Guid == authenticatedUserId).Any();
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception error)
            {
                logger.LogError(error, "Auth Controller - Login");
                throw;
            }
        }

        public async Task<IActionResult> Logout()
        {
            try
            {
                return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
            }
            catch (Exception error)
            {
                logger.LogError(error, "Auth controller - Logout");
                throw;
            }
        }
    }
}
