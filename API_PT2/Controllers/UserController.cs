using API_PT2.DTO;
using API_PT2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

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
            var user = _bookStoreContext.Users.FirstOrDefault(x => x.EmailAddress == model.Email && x.Password == model.Password);
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
                    Data = null
                });
            }
        }

        private string GenerateToken(User user)
        {
            var jwtTokenHandle = new JwtSecurityTokenHandler();
            return "";
        }
    }
}
