using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Features.Account.Queries.GetUserList
{
    public class GetUserListQuery : IRequest<List<GetUserDetailsResponse>>
    {
    }
}
