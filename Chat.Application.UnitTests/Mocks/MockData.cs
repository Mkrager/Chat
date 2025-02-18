using Chat.Domain.Entities;

namespace Chat.Application.UnitTests.Mocks
{
    public class MockData
    {
        public static List<Message> GetMessages()
        {
            return new List<Message>
            {
                new Message
                {
                    Id = Guid.Parse("d13b166b-8f28-4614-9fb7-32322fee1765"),
                    Content = "Test",
                    SendDate = DateTime.Now.AddDays(1),
                    UserId = "35b820e5-1cea-47c8-a6a0-cedccfbda4e6",
                    ReceiverUserId = "d463ee4a-ad5c-4eb7-be35-3f0991bc20ad"
                },
                new Message
                {
                    Id = Guid.Parse("87cb9722-3a07-426f-84d2-a91d8ad67aec"),
                    Content = "Test2",
                    SendDate = DateTime.Now.AddDays(1),
                    UserId = "35b820e5-1cea-47c8-a6a0-cedccfbda4e6",
                    ReceiverUserId = "d463ee4a-ad5c-4eb7-be35-3f0991bc20ad"

                },
                new Message
                {
                    Id = Guid.Parse("8a7292d3-b045-47b0-8e84-d370e2898df1"),
                    Content = "Test3",
                    SendDate = DateTime.Now.AddDays(1),
                    UserId = "35b820e5-1cea-47c8-a6a0-cedccfbda4e6",
                    ReceiverUserId = "d463ee4a-ad5c-4eb7-be35-3f0991bc20ad"
                }
            };
        }
    }
}
