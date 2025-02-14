using Chat.Application.Contracts.Identity;
using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Features.Account.Queries.GetUserList
{
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, List<GetUserDetailsResponse>>
    {
        private readonly IUserService _userService;

        public GetUserListQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<List<GetUserDetailsResponse>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var allUsers = (await _userService.GetUserListAsync()).OrderBy(x => x.UserId);

            return allUsers.ToList();
        }
    }
}
