using System;
using System.Threading.Tasks;
using Chat.API.Areas.Authentication.Dtos;
using Chat.API.Areas.Authentication.Helpers;
using Chat.API.Data.Repositories;
using Chat.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Areas.Authentication.Controllers
{   
    [Produces("application/json")]
    [Route("[controller]")]
    public class AuthenticationController :Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly IEncrypter _encrypter;
        public AuthenticationController(IUserRepository userRepository, IJwtHandler jwtHandler, IEncrypter encrypter)
        {
            _userRepository = userRepository;
            _jwtHandler = jwtHandler;
            _encrypter = encrypter;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginRequest request)
        {
            var userFromDatabase = _userRepository.GetUserByUsername(request.Username);
            if (userFromDatabase == null)
                return Unauthorized();

            var passHash = _encrypter.GetHash(request.Password, userFromDatabase.PasswordSalt);
            if (passHash != userFromDatabase.PasswordHash)
                return Unauthorized();

            var token = await _jwtHandler.CreateTokenAsync(request.Username);
            return Json(token);
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterRequest request)
        {
            //todo later- add model validation
            if (request == null)
                return Json(new Exception("user cannot be empty "));

            if (_userRepository.GetUserByUsername(request.Username) != null)
                return Json(new Exception("username already exists"));

            var salt = _encrypter.GetSalt();
            var hash = _encrypter.GetHash(request.Password, salt);

            var userToAdd = new User(request.Username, hash, salt, request.Email);

            await _userRepository.AddAsync(userToAdd);

            return StatusCode(201);
        }
    }
}