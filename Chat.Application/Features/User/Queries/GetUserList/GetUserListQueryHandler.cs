using AutoMapper;
using Chat.Domain.Interfaces.Repositories;
using MediatR;

namespace Chat.Application.Features.User.Queries.GetUserList
{
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, List<UserListVm>>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public GetUserListQueryHandler(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<List<UserListVm>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var allUsers = (await _userRepository.ListAllUsers()).OrderBy(x => x.UserName);
            return _mapper.Map<List<UserListVm>>(allUsers);
        }
    }
}
