using Chat.Application.Contracts.Identity;
using Chat.Application.DTOs;
using Chat.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Chat.Identity.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetUserDetailsResponse> GetUserDetailsAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue("uid");
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            return new GetUserDetailsResponse
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<List<GetUserDetailsResponse>> GetUserListAsync()
        {
            var users = _userManager.Users.ToList();

            var userDetailsList = users.Select(user => new GetUserDetailsResponse
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email
            }).ToList();

            return userDetailsList;
        }
    }
}
