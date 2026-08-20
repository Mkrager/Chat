using Chat.Application.Contracts.Persistance;
using Chat.Application.DTOs;
using Moq;

namespace Chat.Application.UnitTests.Mocks
{
    public class UserServiceMock
    {
        public static Mock<IUserRepository> GetUserService()
        {
            var users = new List<GetUserDetailsResponse>
                {
                    new GetUserDetailsResponse
                    {
                        UserId = Guid.Parse("35b820e5-1cea-47c8-a6a0-cedccfbda4e6"),
                        UserName = "user1", Email = "user1@example.com",
                    },
                    new GetUserDetailsResponse
                    {
                        UserId = Guid.Parse("d463ee4a-ad5c-4eb7-be35-3f0991bc20ad"),
                        UserName = "user2", Email = "user2@example.com",
                    }
                };

            var mockUserService = new Mock<IUserRepository>();

            //mockUserService.Setup(repo => repo.GetUserDetailsAsync(It.IsAny<string>()))
            //    .ReturnsAsync((string userId) => users.First(a => a.UserId == userId));


            //mockUserService.Setup(repo => repo.GetUserListAsync())
            //    .ReturnsAsync(users);

            //mockUserService.Setup(repo => repo.GetUsersByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            //    .ReturnsAsync((IEnumerable<Guid> userIds) =>
            //    {
            //        return users.Where(u => userIds.Contains(u.UserId))
            //        .Select(u => new GetUserDetailsResponse()
            //        {
            //            UserId = u.UserId,
            //            UserName = u.UserName
            //        }).ToList();
            //    });

            return mockUserService;
        }
    }
}