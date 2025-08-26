using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Features.Users.GetUserList
{
    public class GetUserListQuery : IRequest<List<GetUserDetailsResponse>>
    {
    }
}
