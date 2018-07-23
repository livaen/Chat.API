using System.Threading.Tasks;
using Chat.API.Areas.Authentication.Dtos;
using Chat.API.Areas.Authentication.Helpers;
using Chat.API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Areas.Authentication.Controllers
{   
    [Produces("application/json")]
    [Route("[controller]")]
    public class AuthenticationController :Controller
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IJwtHandler _jwtHandler;
        public AuthenticationController(/*IAuthenticationRepository authenticationRepository, */IJwtHandler jwtHandler)
        {
          //  _authenticationRepository = authenticationRepository;
            _jwtHandler = jwtHandler;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginRequest request)
        {

            var token = await _jwtHandler.CreateTokenAsync(request.Username);
            return Json(token);
        }
        
    }
}