using Chat.Domain.Common;

namespace Chat.Domain.Entities
{
    public class Message : AuditableEntity
    {
        public string Ciphertext { get; set; } = string.Empty;
        public string Iv { get; set; } = string.Empty;
        public Guid ReceiverId { get; set; }
    }
}
