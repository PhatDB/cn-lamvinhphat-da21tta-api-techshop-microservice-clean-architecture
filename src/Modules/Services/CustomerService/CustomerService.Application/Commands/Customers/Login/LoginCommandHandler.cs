using System.Security.Claims;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using CustomerService.Application.Abtractions;
using CustomerService.Application.DTOs;
using CustomerService.Domain.Abtractions.Repositories;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Errors;
using CustomerService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Application.Commands.Customers.Login
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginDto>
    {
        private readonly ICustomerRepository _customerRepository;

        private readonly IJwtTokenService _jwtTokenService;

        public LoginCommandHandler(IJwtTokenService jwtTokenService, ICustomerRepository customerRepository)
        {
            _jwtTokenService = jwtTokenService;
            _customerRepository = customerRepository;
        }


        public async Task<Result<LoginDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            Customer? customer = await _customerRepository.AsQueryable().AsNoTracking()
                .Where(u => u.Email == new Email(request.Email)).FirstOrDefaultAsync(cancellationToken);

            if (customer == null || customer.Status == false)
                return Result.Failure<LoginDto>(CustomerError.CustomerNotFound);

            if (!BCrypt.Net.BCrypt.Verify(request.Password, customer.Password.Value))
                return Result.Failure<LoginDto>(CustomerError.InvalidCredentials);

            string deviceId = Guid.NewGuid().ToString();

            List<Claim> claims = new()
            {
                new Claim("sub", customer.Id.ToString()),
                new Claim("email", customer.Email.Value),
                new Claim("userName", customer.CustomerName),
                new Claim("deviceId", deviceId),
                new Claim("role", customer.Role)
            };

            string accessToken = _jwtTokenService.GenerateAccessToken(claims).Value;
            string refreshToken = _jwtTokenService.GenerateRefreshToken().Value;
            int accessTokenExpires = (int)DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds();

            await _jwtTokenService.SaveSessionAsync(customer.Id, deviceId, accessToken, refreshToken);

            return Result.Success(new LoginDto
            {
                CustomerId = customer.Id,
                Email = customer.Email.Value,
                CustomerName = customer.CustomerName,
                AccessToken = accessToken,
                AccessTokenExpires = accessTokenExpires,
                RefreshToken = refreshToken,
                Role = customer.Role
            });
        }
    }
}