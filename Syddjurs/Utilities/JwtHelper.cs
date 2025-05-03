using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

        

            foreach(var claim in token.Claims)
            {
                if (claim.Type == ClaimTypes.Name)
                {
                     userName = claim.Value;
                }
            }
         
            return userName;
        }
    }
}
