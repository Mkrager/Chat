using AutoMapper;
using Chat.Application.Features.User.Queries.GetUserList;
using Chat.Application.Profiles;
using Chat.Application.UnitTests.Mocks;
using Chat.Domain.Interfaces.Repositories;
using Moq;
using Shouldly;

namespace Chat.Application.UnitTests.UserT
{
    public class GetUserListQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly IMapper _mapper;

        public GetUserListQueryHandlerTests()
        {
            _mockUserRepository = RepositoryMocks.GetUserRepository();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task GetUserList_ShouldReturnListOfUsers_WhenInvoked()
        {
            var handler = new GetUserListQueryHandler(_mapper, _mockUserRepository.Object);

            var result = await handler.Handle(new GetUserListQuery(), CancellationToken.None);

            result.ShouldBeOfType<List<UserListVm>>();

            result.Count.ShouldBe(2);
        }

    }
}
