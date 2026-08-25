using MediatR;

namespace Chat.Application.Features.Users.Queries.GetPublicKeyByUser
{
    public class GetPublicKeyByUserQuery : IRequest<string>
    {
        public Guid UserId { get; set; }
    }
}
