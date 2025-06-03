namespace RecipeManagement.Web
{
    using Microsoft.IdentityModel.JsonWebTokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;

    public class ClaimsHelper
    {
        public static ClaimsPrincipal GetClaimsPrincipalFromJwt(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(jwtToken) as JwtSecurityToken;

            if (token == null || token.Claims == null)
            {
                return null;
            }

            var identity = new ClaimsIdentity(token.Claims);
            return new ClaimsPrincipal(identity);
        }

    }
}
