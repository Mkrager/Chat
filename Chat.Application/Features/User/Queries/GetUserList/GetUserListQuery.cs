using MediatR;

namespace Chat.Application.Features.User.Queries.GetUserList
{
    public class GetUserListQuery : IRequest<List<UserListVm>>
    {
    }
}
