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
                    CreatedDate = DateTime.Now.AddDays(1),
                    CreatedBy = "35b820e5-1cea-47c8-a6a0-cedccfbda4e6",
                    ReceiverId = "d463ee4a-ad5c-4eb7-be35-3f0991bc20ad"
                },
                new Message
                {
                    Id = Guid.Parse("d13b166b-8f28-4614-9fb7-32322fee1765"),
                    Content = "Test2",
                    CreatedDate = DateTime.Now.AddDays(1),
                    CreatedBy = "35b820e5-1cea-47c8-a6a0-cedccfbda4e6",
                    ReceiverId = "d463ee4a-ad5c-4eb7-be35-3f0991bc20ad"
                },
                new Message
                {
                    Id = Guid.Parse("d13b166b-8f28-4614-9fb7-32322fee1765"),
                    Content = "Test3",
                    CreatedDate = DateTime.Now.AddDays(1),
                    CreatedBy = "35b820e5-1cea-47c8-a6a0-cedccfbda4e6",
                    ReceiverId = "d463ee4a-ad5c-4eb7-be35-3f0991bc20ad"
                },            
            };
        }
    }
}