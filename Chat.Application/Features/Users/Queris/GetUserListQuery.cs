using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Features.Users.Queris
{
    public class GetUserListQuery : IRequest<List<UserListVm>>
    {
    }
}
