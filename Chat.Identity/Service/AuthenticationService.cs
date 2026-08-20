using Chat.Application.Contracts.Identity;
using Chat.Application.Contracts.Infrastructure;
using Chat.Application.Contracts.Persistance;
using Chat.Application.DTOs;
using Chat.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chat.Identity.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasherService;


        public AuthenticationService(
            IUserRepository userRepository, 
            IPasswordHasherService passwordHasherService, 
            IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _userRepository = userRepository;
            _passwordHasherService = passwordHasherService;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("Invalid credentials");

            if (!_passwordHasherService.Verify(request.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            var token = GenerateToken(user);

            return new AuthenticationResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = token
            };
        }

        public async Task<Guid> RegisterAsync(RegistrationRequest request)
        {
            var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
            if (existingEmail != null)
                throw new Exception("Email already exists");

            var existingUsername = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUsername != null)
                throw new Exception("Username already exists");

            var hashedPassword = _passwordHasherService.Hash(request.Password);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hashedPassword,
            };

            await _userRepository.AddAsync(user);

            return user.Id;
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
