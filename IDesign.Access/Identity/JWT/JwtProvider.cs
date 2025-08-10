﻿using IDesign.Access.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IDesign.Access.Identity.JWT
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions;

        public JwtProvider(IOptions<JwtOptions> options) {
            _jwtOptions = options.Value;
        }

        public string Generate(User user)
        {

            var claims = new Claim[] { 
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new (JwtRegisteredClaimNames.Email, user.Email),
                new (ClaimTypes.Role, user.Role)
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: this._jwtOptions.Issuer,audience: this._jwtOptions.Audience, 
                claims: claims,expires: DateTime.UtcNow.AddMinutes(this._jwtOptions.AccessTokenExpiration),
                signingCredentials: signingCredentials);

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
