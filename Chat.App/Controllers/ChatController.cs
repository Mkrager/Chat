using Chat.App.Contracts;
using Chat.App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.App.Controllers
{
    public class ChatController : Controller
    {
        private readonly IUserDataService _userDataService;
        private readonly IChatDataService _chatDataService;
        private readonly IAuthenticationService _authenticationService;
        public ChatController(IUserDataService userDataService, IChatDataService chatDataService, IAuthenticationService authenticationService)
        {
            _userDataService = userDataService;
            _chatDataService = chatDataService;
            _authenticationService = authenticationService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userDataService.GetAllUsers();

            var messages = await _chatDataService.GetAllMessages("");

            var chatViewModel = new ChatViewModel()
            {
                Messages = messages,
                Users = users,
                JwtToken = _authenticationService.GetAccessToken()
            };

            return View(chatViewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetMessages([FromQuery] string userId)
        {
            var messages = await _chatDataService.GetAllMessages(userId);

            return Json(messages);
        }

    }
}