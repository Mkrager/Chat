using Chat.Domain.Models;
using MediatR;

namespace Chat.Application.Features.Account.Queries.GetUserDetails
{
    public class GetUserDetailsQuery : IRequest<GetUserDetailsResponse>
    {
    }
}
