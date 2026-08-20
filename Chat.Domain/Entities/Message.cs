using Chat.Domain.Common;

namespace Chat.Domain.Entities
{
    public class Message : AuditableEntity
    {
        public string Content { get; set; } = string.Empty;
        public Guid ReceiverId { get; set; }
    }
}
