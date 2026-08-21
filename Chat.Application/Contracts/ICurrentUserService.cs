namespace Chat.Application.Contracts
{
    public interface ICurrentUserService
    {
        public Guid? UserId { get; }
    }
}
