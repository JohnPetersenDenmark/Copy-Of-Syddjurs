using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace Syddjurs.Utilities
{
    public static class JwtHelper
    {
        public static string? GetUserNameFromToken(string jwtToken)
        {
            var userName = "";

            if (string.IsNullOrWhiteSpace(jwtToken))
                return null;

            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(jwtToken))
                return null;

            var token = handler.ReadJwtToken(jwtToken);

            // Try the most common claim types
             //userName = token.Claims.FirstOrDefault(c => c.Type == "name");

            foreach(var claim in token.Claims)
            {
                if (claim.Type == "name")
                {
                     userName = claim.Value;
                }
            }
            // token.Claims.FirstOrDefault(c => c.Type == "email")?.Value ??
            // token.Claims.FirstOrDefault(c => c.Type == "name")?.Value ??
            // token.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value ??
            // token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            return userName;
        }
    }
}
