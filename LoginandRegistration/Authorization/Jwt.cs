using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoginandRegistration.Authorization
{
    public class Jwt
    {
        
            public static string GenerateJWTToken(List<Claim> authClaims, IConfiguration _configuration)
            {

                var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!));
                var tokenObject = new JwtSecurityToken(
                   issuer: _configuration["JWT:ValidIssuer"],
                   audience: _configuration["JWT:ValidAudience"],
                   expires: DateTime.Now.AddDays(2),
                   claims: authClaims,
                   signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                   );

                string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
                return token;
            }
        }
    }

