using Chat.Application.DTOs;

namespace Chat.Application.Contracts.Identity
{
    public interface IUserService
    {
        Task<GetUserDetailsResponse> GetUserDetailsAsync(string userId);
        Task<List<GetUserDetailsResponse>> GetUsersByIdsAsync(IEnumerable<string> userIds);
        Task<List<GetUserDetailsResponse>> GetUserListAsync();
    }
}
