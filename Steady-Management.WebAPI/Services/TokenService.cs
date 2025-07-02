using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Steady_Management.Domain;

namespace Steady_Management.WebAPI.Services
{
    public class TokenService
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _key;
        private readonly int _expiresMinutes;

        public TokenService(IConfiguration cfg)
        {
            _issuer = cfg["JwtSettings:Issuer"] ?? throw new("Issuer missing");
            _audience = cfg["JwtSettings:Audience"] ?? throw new("Audience missing");
            _key = cfg["JwtSettings:Key"] ?? throw new("Key missing");
            _expiresMinutes = int.Parse(cfg["JwtSettings:ExpiresMinutes"] ?? "60");
        }

        public string BuildToken(WebAppUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,    user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserLogin),
                new Claim(ClaimTypes.Role,                user.RoleId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,    Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expiresMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
