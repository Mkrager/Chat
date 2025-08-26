using Chat.App.Contracts;
using Chat.App.Middlewares;
using Chat.App.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Chat.App.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new AuthenticateRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthenticateRequest authenticateRequest)
        {
            var result = await _authenticationService.Authenticate(authenticateRequest);
            authenticateRequest.ErrorMessage = HandleErrors.HandleResponse(result, "Success");

            if (result.IsSuccess)
            {
                return RedirectToAction("Chat", "Chat");
            }

            return View(authenticateRequest);
        }

        public IActionResult Register()
        {
            return View(new RegistrationRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequest registrationRequest)
        {
            var result = await _authenticationService.Register(registrationRequest);
            registrationRequest.ErrorMessage = HandleErrors.HandleResponse(result, "Success");

            if (result.IsSuccess)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(registrationRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
