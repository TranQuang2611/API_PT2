using API_PT2.DTO;
using API_PT2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_PT2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BookStoreContext _bookStoreContext;
        private readonly AppSetttings _appSetttings;

        public UserController(BookStoreContext bookStoreContext, IOptionsMonitor<AppSetttings> optionsMonitor)
        {
            _bookStoreContext = bookStoreContext;
            _appSetttings = optionsMonitor.CurrentValue;
        }

        [HttpPost("Login")]
        public IActionResult ValidateUser(LoginModel model)
        {
            var user = _bookStoreContext.Users.Include(x => x.Role).FirstOrDefault(x => x.EmailAddress == model.Email && x.Password == model.Password);
            if(user == null)
            {
                return Ok(new ApiRespond
                {
                    Success = false,
                    Message = "Invalid Email or Password"
                });
            }
            else
            {
                //cấp token
                return Ok(new ApiRespond
                {
                    Success = true,
                    Message = "Authenticate success",
                    Data = GenerateToken(user)
                });
            }
        }

        private string GenerateToken(User user)
        {
            var jwtTokenHandle = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetttings.SecretKey);
            var tokenDes = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, user.EmailAddress),
                    new Claim("Id", user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.FirstName + user.LastName),

                    //role
                    new Claim(ClaimTypes.Role, user.Role.RoleName),
                    new Claim("TokenId", Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)

            };

            var token = jwtTokenHandle.CreateToken(tokenDes);
            return jwtTokenHandle.WriteToken(token);
        }
    }
}
