using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ParrotWings.Services.Secure
{
    public class PWJwtDataFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly string algorithm;
        private readonly TokenValidationParameters validationParameters;

        public PWJwtDataFormat(string algorithm, TokenValidationParameters validationParameters)
        {
            this.algorithm = algorithm;
            this.validationParameters = validationParameters;
        }

        public string Protect(AuthenticationTicket data)
        {
            throw new NotImplementedException();
        }

        public string Protect(AuthenticationTicket data, string purpose)
        {
            throw new NotImplementedException();
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            return Unprotect(protectedText, null);
        }

        public AuthenticationTicket Unprotect(string protectedText, string purpose)
        {
            var handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = null;
            SecurityToken validToken = null;

            try
            {
                principal = handler.ValidateToken(protectedText, validationParameters, out validToken);

                var validJwt = validToken as JwtSecurityToken;

                if (validJwt == null)
                    throw new ArgumentException("Invalid JWT");

                if (!validJwt.Header.Alg.Equals(algorithm, StringComparison.Ordinal))
                    throw new ArgumentException($"Algorithm must be '{algorithm}'");
            }
            catch (SecurityTokenValidationException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }

            // Validation passed. Return a valid AuthenticationTicket:
            return new AuthenticationTicket(principal, new AuthenticationProperties(), "Cookie");
        }
    }
}
