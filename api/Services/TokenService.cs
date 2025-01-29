using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using api.Interface;
using api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        
        public TokenService(IConfiguration configuration)
        {
            _key = configuration.GetValue<string>("TokenService:_key");
            _issuer = configuration.GetValue<string>("TokenService:Issuer");
            _audience = configuration.GetValue<string>("TokenService:Audience");
            
        }

        public string GenerateToken(User user)
        {
            // take roles
            var roleNames = user.UserRoles.Select(ur => ur.Role.Name).ToList();

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));

            // user cred
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Email, user.Email)
            };

            // role is embbeded jwt
            claims.AddRange(roleNames.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Token geçerlilik süresi
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha256Signature)
            };

            // Token'ı oluşturuyoruz
            var token = tokenHandler.CreateToken(tokenDesc);
            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
