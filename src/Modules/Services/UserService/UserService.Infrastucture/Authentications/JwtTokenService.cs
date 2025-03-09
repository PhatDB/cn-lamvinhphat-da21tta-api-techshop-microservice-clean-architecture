using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BuildingBlocks.Constants;
using BuildingBlocks.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using UserService.Application.Abtractions;
using UserService.Domain.Errors;
using UserService.Infrastucture.DependencyInjections.Options;

namespace UserService.Infrastucture.Authentications
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOption jwtOption = new();
        private readonly IConnectionMultiplexer redis;
        private readonly IDatabase redisDatabase;

        public JwtTokenService(IConfiguration configuration, IConnectionMultiplexer redis)
        {
            configuration.GetSection(nameof(JwtOption)).Bind(jwtOption);
            this.redis = redis;
            redisDatabase = redis.GetDatabase();
        }

        public Result<string> GenerateAccessToken(IEnumerable<Claim> claims)
        {
            try
            {
                SymmetricSecurityKey securityKey =
                    new(Encoding.UTF8.GetBytes(Const.SECRET_KEY));
                SigningCredentials signinCredentials =
                    new(securityKey, SecurityAlgorithms.HmacSha256);

                JwtSecurityToken tokenOptions = new(jwtOption.Issuer, jwtOption.Audience,
                    claims, expires: DateTime.UtcNow.AddMinutes(jwtOption.ExpiryMinutes),
                    signingCredentials: signinCredentials);

                string tokenString =
                    new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Result.Success(tokenString);
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(
                    TokenError.TokenGenerationFailed(ex.Message));
            }
        }

        public Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
        {
            try
            {
                byte[] key = Encoding.UTF8.GetBytes(Const.SECRET_KEY);

                TokenValidationParameters tokenValidationParameters = new()
                {
                    ValidateAudience = false,
                    ValidAudience = jwtOption.Audience,
                    ValidateIssuer = false,
                    ValidIssuer = jwtOption.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                };

                JwtSecurityTokenHandler tokenHandler = new();

                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                    tokenValidationParameters, out SecurityToken securityToken);
                JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(
                        SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase))
                    return Result.Failure<ClaimsPrincipal>(TokenError.InvalidToken);

                return Result.Success(principal);
            }
            catch (Exception ex)
            {
                return Result.Failure<ClaimsPrincipal>(
                    TokenError.TokenValidationFailed(ex.Message));
            }
        }

        public async Task<Result<IDictionary<string, RedisValue>>> GetSessionAsync(
            int userId, string deviceId)
        {
            try
            {
                string sessionKey = GetSessionRedisKey(userId, deviceId);

                HashEntry[] sessionData = await redisDatabase.HashGetAllAsync(sessionKey);

                if (sessionData.Length == 0)
                    return Result.Failure<IDictionary<string, RedisValue>>(SessionError
                        .SessionNotFound);

                return Result.Success<IDictionary<string, RedisValue>>(
                    sessionData.ToDictionary(x => x.Name.ToString(), x => x.Value));
            }
            catch (Exception ex)
            {
                return Result.Failure<IDictionary<string, RedisValue>>(
                    SessionError.SessionRetrievalFailed(ex.Message));
            }
        }

        public async Task<Result> DeleteRefreshTokenAsync(int userId, string deviceId)
        {
            try
            {
                bool deleted =
                    await redisDatabase.KeyDeleteAsync(
                        GetSessionRedisKey(userId, deviceId));
                return deleted
                    ? Result.Success()
                    : Result.Failure(SessionError.SessionNotFound);
            }
            catch (Exception ex)
            {
                return Result.Failure(SessionError.SessionDeletionFailed(ex.Message));
            }
        }

        public Result<string> GenerateRefreshToken()
        {
            try
            {
                byte[] randomNumber = new byte[32];
                using RandomNumberGenerator rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);
                string refreshToken = Convert.ToBase64String(randomNumber);
                return Result.Success(refreshToken);
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(
                    TokenError.RefreshTokenGenerationFailed(ex.Message));
            }
        }

        public async Task<Result> SaveSessionAsync(
            int userId, string deviceId, string accessToken, string refreshToken)
        {
            try
            {
                string sessionKey = GetSessionRedisKey(userId, deviceId);
                TimeSpan expiry = TimeSpan.FromDays(jwtOption.RefreshTokenExpiryDays);

                HashEntry[] sessionData =
                {
                    new("accessToken", accessToken),
                    new("refreshToken", refreshToken),
                    new("refreshTokenExpiry",
                        DateTimeOffset.UtcNow.Add(expiry).ToUnixTimeSeconds())
                };

                await redisDatabase.HashSetAsync(sessionKey, sessionData);
                await redisDatabase.KeyExpireAsync(sessionKey, expiry);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(SessionError.SessionSaveFailed(ex.Message));
            }
        }

        private string GetSessionRedisKey(int userId, string deviceId)
        {
            return $"session:{userId}:{deviceId}";
        }
    }
}