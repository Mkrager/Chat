using Chat.Application.Contracts.Infrastructure;

namespace Chat.Infrastructure.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
