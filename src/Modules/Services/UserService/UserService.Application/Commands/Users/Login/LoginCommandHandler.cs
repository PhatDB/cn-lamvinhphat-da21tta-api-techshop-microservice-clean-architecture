using System.Security.Claims;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Abtractions;
using UserService.Application.DTOs;
using UserService.Domain.Abtractions.Repositories;
using UserService.Domain.Entities;
using UserService.Domain.Errors;
using Email = UserService.Domain.ValueObjects.Email;

namespace UserService.Application.Commands.Users.Login
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginDTO>
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUserRepository _userRepository;

        public LoginCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<Result<LoginDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.AsQueryable().AsNoTracking().Where(u => u.Email == new Email(request.Email)).FirstOrDefaultAsync(cancellationToken);

            if (user == null || user.IsActive == false)
                return Result.Failure<LoginDTO>(UserError.UserNotFound);

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password.Value))
                return Result.Failure<LoginDTO>(UserError.InvalidCredentials);

            string deviceId = Guid.NewGuid().ToString();

            List<Claim> claims = new()
            {
                new Claim("sub", user.Id.ToString()),
                new Claim("email", user.Email.Value),
                new Claim("userName", user.Username),
                new Claim("role", user.Role),
                new Claim("deviceId", deviceId)
            };

            string accessToken = _jwtTokenService.GenerateAccessToken(claims).Value;
            string refreshToken = _jwtTokenService.GenerateRefreshToken().Value;
            int accessTokenExpires = (int)DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds();

            await _jwtTokenService.SaveSessionAsync(user.Id, deviceId, accessToken, refreshToken);

            return Result.Success(new LoginDTO
            {
                UserId = user.Id,
                Email = user.Email.Value,
                UserName = user.Username,
                AccessToken = accessToken,
                AccessTokenExpires = accessTokenExpires,
                RefreshToken = refreshToken
            });
        }
    }
}