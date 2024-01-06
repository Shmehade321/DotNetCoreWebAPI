using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotNetCoreWebAPI.Authority
{
    public static class Authenticator
    {
        public static bool Authenticate(string clientId, string secret)
        {
            var app = AppRepository.GetApplicationByClientId(clientId);
            if (app == null) return false;

            return (app.ClientId == clientId && app.Secret == secret);
        }

        public static string CreateToken(string clientId, DateTime expiresAt, string strSecretKey)
        {
            //Steps to create JWT Token
            //1. Algorithm
            //2. Paylaod (Claims)
            //3. Signature (Signing Key)

            var app = AppRepository.GetApplicationByClientId(clientId);

            var claims = new List<Claim>
            {
                new Claim("AppName", app.ApplicationName ?? string.Empty),
                new Claim("Read", (app.Scopes ?? string.Empty).Contains("read") ? "true": "false"),
                new Claim("Write", (app.Scopes ?? string.Empty).Contains("write") ? "true": "false"),
            };

            var secretKey = Encoding.ASCII.GetBytes(strSecretKey);

            var jwt = new JwtSecurityToken(
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature),
                claims: claims,
                expires: expiresAt,
                notBefore: DateTime.UtcNow
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static bool VerifyToken(string token, string strSecretKey)
        {
            if (string.IsNullOrWhiteSpace(token)) return false;

            var key = Encoding.ASCII.GetBytes(strSecretKey);
            SecurityToken validatedToken;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();                

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out validatedToken);
            }
            catch(SecurityTokenException)
            {
                return false;
            }
            catch
            {
                throw;
            }

            return validatedToken != null;
        }
    }
}
