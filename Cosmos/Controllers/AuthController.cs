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
        private const string AuthSchemes =
            CookieAuthenticationDefaults.AuthenticationScheme + "," +
            OpenIdConnectDefaults.AuthenticationScheme;

        public AuthController(ILogger<AuthController> _logger)
        {
            logger = _logger;
        }

        [Authorize(AuthenticationSchemes = AuthSchemes)]
        public async Task<IActionResult> Login()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var authenticatedUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
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
