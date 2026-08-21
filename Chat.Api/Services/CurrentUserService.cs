using Chat.Application.Contracts;
using System.Security.Claims;

namespace Chat.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid? UserId
        {
            get
            {
                var value = _httpContextAccessor.HttpContext?
                    .User
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return Guid.TryParse(value, out var userId)
                    ? userId
                    : null;
            }
        }
    }
}
