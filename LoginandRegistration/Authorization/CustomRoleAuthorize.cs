using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace LoginandRegistration.Authorization
{
    public class CustomRoleAuthorize : AuthorizeAttribute,IAuthorizationFilter
        
    {
        public string _role;

        public CustomRoleAuthorize(string role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var jwtToken = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var tokenHandler = new JwtSecurityTokenHandler();
            var payload = JsonConvert.SerializeObject(tokenHandler.ReadJwtToken(jwtToken).Payload);
            dynamic JsonPayload = JsonConvert.DeserializeObject(payload)!;
            string Role = JsonPayload.Role;
            if (Role != null)
            {
                if (Role != null)
                {
                    if(Role == _role)
                    {
                        
                    }
                }
                else
                {
                    context.Result = new NotFoundResult();
                    return;
                }
            }
        }

    }
}
