using Chat.App.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.App.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserDataService _userDataService;

        public UserController(IUserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        [Authorize]
        [HttpPatch("public-key", Name = "SavePublicKey")]
        public async Task<IActionResult> SavePublicKey([FromBody] string publicKey)
        {
            await _userDataService.SavePublicKey(publicKey);

            return Ok();
        }
    }
}
