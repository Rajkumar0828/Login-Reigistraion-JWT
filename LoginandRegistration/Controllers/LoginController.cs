using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LoginandRegistration.Authorization;

using LoginandRegistration.Model;
using LoginandRegistration.DataContext;
using LoginandRegistration.API_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Data.Entity;
using System.Security.Claims;

namespace LoginandRegistration.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;

        public LoginController(ApplicationDbContext Context, IConfiguration Configuration)
        {
            _context = Context;
            _configuration = Configuration;
        }
        [HttpPost]

        public IActionResult LoginUser([FromBody] Login loginuser)
        {
            AuthMessageModel authmessage;
            List<Claim> authClaim;
            if (loginuser != null)
            {
                if (_context.User.Any(user => user.Email == loginuser.Email))
                {
                    Users user = _context.User.Where(user => user.Email == loginuser.Email).FirstOrDefault()!;
                    if (user.Password == SHA256Encrypt.ComputePasswordToSha256Hash(loginuser.Password))
                    {
                        var userroleid = _context.UserRole.Where(userrole => userrole.User == user)
                        .Select(user => user.Id)
                          .FirstOrDefault()!;
                        var roleid = _context.UserRole.Where(ur => ur.Id == userroleid) // Filter by userrole id
                        .Select(ur => ur.Role.RolesId) // Select the role id
                        .FirstOrDefault();
                        authClaim = new List<Claim>()
                        {
                            new Claim("Id",user.UsersId),
                            new Claim("Role",roleid!)
                        };
                        var token = Jwt.GenerateJWTToken(authClaim, _configuration);
                        authmessage = new AuthMessageModel
                        {
                            AccountExists = true,
                            PasswordStatus = true,
                            Token = token
                        };
                        return Ok(authmessage);
                    }
                      authmessage = new AuthMessageModel
                      {
                        AccountExists = true,
                        PasswordStatus = false,
                        Token = null
                      };
                      return Ok(authmessage);

                  
                }
                    authmessage = new AuthMessageModel
                    {
                    AccountExists = false,
                    PasswordStatus = false,
                    Token = null
                    };
                     return Ok(authmessage);
            }
                 return BadRequest();
        }


    }
}

            

   

