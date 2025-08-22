using Chat.Application.Contracts;

namespace Chat.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string UserId =>
            _httpContextAccessor.HttpContext.User.FindFirst("uid")?.Value;
    }
}
