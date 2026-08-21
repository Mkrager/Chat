namespace Chat.Application.Features.Users.GetUserList
{
    public class UserListVm
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}
