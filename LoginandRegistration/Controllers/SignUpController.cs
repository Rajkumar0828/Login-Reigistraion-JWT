using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LoginandRegistration.Authorization;

using LoginandRegistration.Model;
using LoginandRegistration.DataContext;
using LoginandRegistration.API_Model;
using Org.BouncyCastle.Bcpg;
using System.Security.Claims;
using Google.Protobuf.Reflection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace LoginandRegistration.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;

        public SignUpController(ApplicationDbContext Context, IConfiguration Configuration)
        {
            _context = Context;
            _configuration =  Configuration;
        }
        [HttpPost]
        public IActionResult PostUser(SignUp user)
        {
            List<Claim> authClaim;
            string token;
            SignUpMessage message = null;

            Users user1 = new Users()
            {
                UsersId = Guid.NewGuid().ToString(),
                UserName = user.UserName,
                Email  = user.Email,
                Password = SHA256Encrypt.ComputePasswordToSha256Hash(user.Password),
                MobileNumber = user.MobileNumber,
             };

            Roles role = _context.Roles.FirstOrDefault(role => role.RoleName == user.Role)!;
            UserRoles userrole = new UserRoles()
            {
                Id = Guid.NewGuid().ToString(),
                User = user1,
                Role = role
            };
            if (_context.User.Any(user=>user.Email==user1.Email) || _context.User.Any(user => user.MobileNumber == user1.MobileNumber))
            {
                if (_context.User.Any(user => user.Email == user1.Email))
                {
                    message = new SignUpMessage()
                    {
                        EmailExists = true,
                        MobileNumberExists = false,
                        Username = null,
                        Token = null

                    };
                }
                if (_context.User.Any(user => user.MobileNumber == user1.MobileNumber))
                {
                    message = new SignUpMessage()
                    {
                        EmailExists = false,
                        MobileNumberExists = true,
                        Username = null,
                        Token = null

                    };

                }
                if (_context.User.Any(user => user.MobileNumber == user1.MobileNumber) && _context.User.Any(user => user.Email == user1.Email))
                {
                    message = new SignUpMessage()
                    {
                        EmailExists = true,
                        MobileNumberExists = true,
                        Username = null,
                        Token = null

                    };

                }

                return Ok(message);

            }
            _context.User.Add(user1);
            _context.UserRole.Add(userrole);
            if (_context.SaveChanges() > 0)
            {
                Users resuser = _context.User.FirstOrDefault(user => user.UserName == user1.UserName)!;
                UserRoles resuserrole = _context.UserRole.FirstOrDefault(userrole => userrole.User == resuser)!;
                authClaim = new List<Claim>
                {
                    new Claim("Id",resuser.UsersId),
                    new Claim("Role",resuserrole.Role.RolesId),
                    
                 
                };
                token = GenerateJWTToken(authClaim);
                message = new SignUpMessage()
                {
                    EmailExists = true,
                    MobileNumberExists = false,
                    Token = token,
                    Username = resuser.UserName
                };
                return Ok(message);
            }
            return BadRequest("Internal Server Error ! please try again later");

        }
        [HttpPost]
        public IActionResult PostRole([FromBody]Roles roles)
        {
            Roles roles1 = new Roles()
            {
                RolesId = Guid.NewGuid().ToString(),
                RoleName = roles.RoleName.ToUpper()
            };
            _context.Roles.Add(roles1);
            _context.SaveChanges();
            return Ok();
        }
        [NonAction]
        public string GenerateJWTToken(List<Claim> authClaims)
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
