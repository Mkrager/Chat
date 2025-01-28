using Chat.Domain.Models;

namespace Chat.Domain.Interfaces.Identity
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
        Task<GetUserDetailsResponse> GetUserDetailsAsync();
    }
}
