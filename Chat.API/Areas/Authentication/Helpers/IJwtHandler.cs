using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Chat.API.Areas.Authentication.Dtos;
using Chat.API.Models;

namespace Chat.API.Areas.Authentication.Helpers
{
    public interface IJwtHandler
    {
        Task<JwtDto> CreateTokenAsync(string userName);
        Task<JwtDto> CreateTokenByUserObject(User user);
        Task<JwtDto> RefreshTokenAsync(ClaimsPrincipal userToken);
    }
}