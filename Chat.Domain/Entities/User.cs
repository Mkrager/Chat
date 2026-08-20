using Chat.Domain.Common;

namespace Chat.Domain.Entities
{
    public class User : AuditableEntity
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PublicKey { get; set; } = string.Empty;
    }
}
