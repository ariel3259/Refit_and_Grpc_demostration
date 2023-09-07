using ApiRefitConsumer.Model;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiRefitConsumer.Controllers
{
    [ApiController]
    [Route("/api/users")]
    public class UsersController : ControllerBase
    {
        private readonly List<Users> users;

        public UsersController()
        {
            users = new()
            {
                new Users()
                {
                    Email = "admin@gmail.com",
                    Password = "$2y$10$LqJNoeXjZrKkHL6XbZx/z.hg5.YLHtv416TV3vmIMDt6JU1VgKB2S",
                }
            };
        }


        [HttpPost]
        public IActionResult LogIn([FromBody] Users req)
        {
            if (req.Email == null || req.Password == null)
                return BadRequest();
            Users? user = users.FirstOrDefault(x => x.Email == req.Email);
            
            if (user == null) return BadRequest();
            bool result = BCrypt.Net.BCrypt.Verify(req.Password, user.Password);
            if (!result) return Unauthorized();
            byte[] key = Encoding.UTF8.GetBytes("loremloremloremlorem");
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMonths(6),
                SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler tokenHandler = new();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string jwt = tokenHandler.WriteToken(token);
            Console.WriteLine(token);
            return Ok(new
            {
                access_token = jwt
            });
        }
    }
}
