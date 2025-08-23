namespace Chat.Domain.Common
{
    public class AuditableEntity : BaseEntity
    {
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
