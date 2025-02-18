using Chat.Application.DTOs;

namespace Chat.Application.Contracts.Identity
{
    public interface IUserService
    {
        Task<GetUserDetailsResponse> GetUserDetailsAsync();
        Task<List<GetUserDetailsResponse>> GetUserListAsync();
    }
}
