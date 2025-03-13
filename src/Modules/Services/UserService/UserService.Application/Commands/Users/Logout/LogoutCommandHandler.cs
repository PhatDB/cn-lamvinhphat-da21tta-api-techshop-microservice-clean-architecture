using System.Security.Claims;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.AspNetCore.Http;
using UserService.Application.Abtractions;
using UserService.Domain.Errors;

namespace UserService.Application.Commands.Users.Logout
{
    public class LogoutCommandHandler : ICommandHandler<LogoutCommand, int>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtTokenService _jwtTokenService;

        public LogoutCommandHandler(IJwtTokenService jwtTokenService, IHttpContextAccessor httpContextAccessor)
        {
            _jwtTokenService = jwtTokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<int>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal? userClaims = _httpContextAccessor.HttpContext?.User;

            if (userClaims == null)
                return Result.Failure<int>(SessionError.SessionNotFound);

            Claim? userIdClaim = userClaims.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Result.Failure<int>(SessionError.SessionNotFound);

            int userId = int.Parse(userIdClaim.Value);

            Claim? deviceIdClaim = userClaims.FindFirst("deviceId");
            if (deviceIdClaim == null)
                return Result.Failure<int>(SessionError.SessionNotFound);

            string deviceId = deviceIdClaim.Value;

            Result result = await _jwtTokenService.DeleteRefreshTokenAsync(userId, deviceId);
            return result.IsSuccess ? Result.Success(userId) : Result.Failure<int>(SessionError.SessionDeletionFailed());
        }
    }
}