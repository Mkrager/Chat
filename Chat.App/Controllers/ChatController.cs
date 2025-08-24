using Chat.App.Contracts;
using Chat.App.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Chat.App.Controllers
{
    public class ChatController : Controller
    {
        private readonly IUserDataService _userDataService;
        private readonly IChatDataService _chatDataService;
        public ChatController(IUserDataService userDataService, IChatDataService chatDataService)
        {
            _userDataService = userDataService;
            _chatDataService = chatDataService;
        }

        [HttpGet]
        public async Task<IActionResult> Chat()
        {
            var users = await _userDataService.GetAllUsers();

            var messages = await _chatDataService.GetAllMessages("");

            var chatViewModel = new ChatViewModel()
            {
                Messages = messages,
                Users = users
            };

            return View(chatViewModel);
        }
    }
}