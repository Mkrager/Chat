namespace Chat.Application.Contracts.Infrastructure
{
    public interface IPasswordHasherService
    {
        string Hash(string password);
        bool Verify(string password, string hash);
    }
}