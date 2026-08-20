using Chat.Application.DTOs;

namespace Chat.Application.Contracts.Identity
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<Guid> RegisterAsync(RegistrationRequest request);
    }
}
