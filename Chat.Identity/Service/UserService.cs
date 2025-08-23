using Chat.Application.Contracts.Identity;
using Chat.Application.DTOs;
using Chat.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chat.Identity.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetUserDetailsResponse> GetUserDetailsAsync(string userId)
        {
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
            var users = await _userManager.Users.ToListAsync();

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

        public async Task<List<GetUserDetailsResponse>> GetUsersByIdsAsync(IEnumerable<string> userIds)
        {
            var users = _userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new GetUserDetailsResponse()
                {
                    UserId = u.Id,
                    UserName = u.UserName
                })
                .ToList();

            return await Task.FromResult(users);
        }
    }
}
