using Demo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DemoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration _config;
       
        private IHostingEnvironment _env;


        public TokenController(IConfiguration config, IHostingEnvironment env)
        {
            _config = config;
          
            _env = env;
        }
       
        [Route("token")]
        [HttpPost]
        public IActionResult CreateToken(TokenLoginModel loginModel)
        {
         
            if (!ModelState.IsValid)
            {
                // bad request and logging logic
            }
            string tokenString = string.Empty;
            try
            {

                tokenString = generateJSONWebToken(loginModel);
            }
            catch (Exception ex)
            {
                  throw;
            }

            return Ok(new { token = tokenString });
        }

        #region Private Methods
        private string generateJSONWebToken(TokenLoginModel tokenModel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(ClaimTypes.Name, tokenModel.UserName),
                new Claim(ClaimTypes.Role, "Admi"),//this will be dynamic
               
               new Claim(ClaimTypes.Expiration, DateTime.Now.AddDays(14).ToString()),

            };

            JwtSecurityToken token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddDays(14),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        #endregion
    }
}
