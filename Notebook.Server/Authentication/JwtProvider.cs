﻿using Microsoft.IdentityModel.Tokens;
using Notebook.Server.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Notebook.Server.Authentication
{
    public class JwtProvider : IJwtProvider
    {
        private IConfiguration _config;

        public JwtProvider(IConfiguration config)
        {
            _config = config;
        }

        public string Generate(AccountModel account)
        {
            var secutiryKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(secutiryKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,account.Email),
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
