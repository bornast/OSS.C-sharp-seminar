using Microsoft.Extensions.Configuration;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IBaseRepository<RefreshToken> _repo;
        private readonly IConfiguration _configuration;

        public RefreshTokenService(IBaseRepository<RefreshToken> repo, IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }

        public async Task MarkAsUsed(string refreshToken, string jti, bool commit = false)
        {
            var storedRefreshToken = await _repo
                .FirstOrDefaultAsync(x => x.Token == refreshToken && x.JwtId == jti);

            storedRefreshToken.Used = true;

            if (commit)
                await _repo.SaveAsync();
        }

        public async Task MarkAsInvalid(string jti, bool commit = false)
        {
            var refreshTokens = await _repo.WhereAsync(x => x.JwtId == jti && x.Invalidated == false && x.Used == false);

            foreach (var refreshToken in refreshTokens)
            {
                refreshToken.Invalidated = true;
            }

            if (commit)
                await _repo.SaveAsync();
        }

        public async Task<RefreshToken> CreateRefreshToken(string jti, User user, bool commit = false)
        {            
            var refreshToken = new RefreshToken
            {
                JwtId = jti,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration.GetSection("RefreshTokenExpireMinutes").Value))
            };

            _repo.Add(refreshToken);

            if (commit)
                await _repo.SaveAsync();

            return refreshToken;
        }
    }
}
