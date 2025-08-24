using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Chat.App.Services
{
    public class TokenService
    {
        public ClaimsPrincipal GetClaimsPrincipal(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

            if (jsonToken == null)
            {
                return null;
            }

            return new ClaimsPrincipal(new ClaimsIdentity(jsonToken.Claims, "Bearer"));
        }

        public string GetClaimValue(string jwtToken, string claimType)
        {
            var claimsPrincipal = GetClaimsPrincipal(jwtToken);
            var data = claimsPrincipal?.FindFirst(claimType)?.Value;
            return data;
        }
    }
}
