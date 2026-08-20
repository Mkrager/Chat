namespace Chat.Application.DTOs
{
    public class GetUserDetailsResponse
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
