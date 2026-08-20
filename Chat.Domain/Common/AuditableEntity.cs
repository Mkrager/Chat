namespace Chat.Domain.Common
{
    public class AuditableEntity : BaseEntity
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
