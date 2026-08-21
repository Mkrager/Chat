using AutoMapper;
using Chat.Application.Contracts.Persistance;
using MediatR;

namespace Chat.Application.Features.Users.GetUserList
{
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, List<UserListVm>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetUserListQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserListVm>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var allUsers = (await _userRepository.ListAllAsync()).OrderBy(x => x.Id).ToList();

            return _mapper.Map<List<UserListVm>>(allUsers);
        }
    }
}
