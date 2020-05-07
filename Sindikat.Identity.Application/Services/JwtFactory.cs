using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Sindikat.Identity.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sindikat.Identity.Application.Interfaces;
using System.Linq;
using SystemClaim = System.Security.Claims.Claim;

namespace Sindikat.Identity.Application.Services
{
    public class JwtFactory : IJwtFactory
    {
        public IConfiguration Configuration { get; set; }

        public JwtFactory(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public object Generate(User user)
        {
            var claims = new List<SystemClaim>
            {
                new SystemClaim(JwtRegisteredClaimNames.Sub, user.UserName),
                new SystemClaim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new SystemClaim(ClaimTypes.NameIdentifier, user.Id),
            };

            var roles = user.UserRoles.Select(x => x.Role).ToList();

            foreach (var role in roles)
            {
                claims.Add(new SystemClaim(ClaimTypes.Role, role.Name));
            }

            foreach (var userClaim in user.UserClaims)
            {
                claims.Add(new SystemClaim(userClaim.Claim.Name, userClaim.ClaimValue));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(Configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                Configuration["JwtIssuer"],
                Configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
