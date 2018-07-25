using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Chat.API.Areas.Authentication.Dtos;
using Chat.API.Models;
using Chat.API.Extensions;
using Microsoft.Extensions.Configuration;
using Chat.API.Settings;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Chat.API.Areas.Authentication.Helpers
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtSettings _jwtSettings;

        public JwtHandler(IConfiguration configuration)
        {
            _jwtSettings = configuration.GetSettings<JwtSettings>();
        }
        public async Task<JwtDto> CreateTokenAsync(string userName)
        {
            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userName)
            };

            var signingCredentials = await Task.FromResult(new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                SecurityAlgorithms.HmacSha256));
            var expires = now.AddMinutes(_jwtSettings.ExpiryMinutes);
            var jwt = await Task.FromResult(new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: signingCredentials
                ));
            var token = await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwt));

            return new JwtDto
            {
                Token = token,
                Expiry = expires.ToTimestamp()
            };
        }

        public async Task<JwtDto> CreateTokenByUserObject(User user)
            => await CreateTokenAsync(user.Username);

        public Task<JwtDto> RefreshTokenAsync(ClaimsPrincipal userToken)
        {
            throw new NotImplementedException();
        }
        
    }
    
}