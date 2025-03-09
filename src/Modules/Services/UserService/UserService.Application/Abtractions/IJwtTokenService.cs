using System.Security.Claims;
using BuildingBlocks.Results;
using StackExchange.Redis;

namespace UserService.Application.Abtractions
{
    public interface IJwtTokenService
    {
        Result<string> GenerateAccessToken(IEnumerable<Claim> claims);
        Result<string> GenerateRefreshToken();
        Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);

        Task<Result<IDictionary<string, RedisValue>>> GetSessionAsync(
            int userId, string deviceId);

        Task<Result> SaveSessionAsync(
            int userId, string deviceId, string accessToken, string refreshToken);

        Task<Result> DeleteRefreshTokenAsync(int userId, string deviceId);
    }
}