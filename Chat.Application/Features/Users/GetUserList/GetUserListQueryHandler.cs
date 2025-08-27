using AutoMapper;
using Chat.Application.Contracts.Identity;
using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Features.Users.GetUserList
{
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, List<UserListVm>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public GetUserListQueryHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<List<UserListVm>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var allUsers = (await _userService.GetUserListAsync()).OrderBy(x => x.UserId).ToList();

            return _mapper.Map<List<UserListVm>>(allUsers);
        }
    }
}
