using System.Security.Claims;
using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using CustomerService.Application.Abtractions;
using CustomerService.Application.DTOs;
using CustomerService.Domain.Abtractions.Repositories;
using CustomerService.Domain.Entities;
using CustomerService.Domain.ValueObjects;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Application.Commands.Customers.GoogleLogin
{
    public class GoogleLoginCommandHandler : ICommandHandler<GoogleLoginCommand, LoginDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUnitOfWork _unitOfWork;

        public GoogleLoginCommandHandler(
            ICustomerRepository customerRepository, IJwtTokenService jwtTokenService, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _jwtTokenService = jwtTokenService;
            _unitOfWork = unitOfWork;
        }


        public async Task<Result<LoginDto>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
            }
            catch
            {
                return Result.Failure<LoginDto>(Error.Validation("Invalid idToken", "Invalid idToken"));
            }

            Email emailValue = new(payload.Email);

            Customer? customer = await _customerRepository.AsQueryable().AsNoTracking()
                .FirstOrDefaultAsync(c => c.Email != null && c.Email == emailValue, cancellationToken);

            if (customer == null)
            {
                customer = Customer.Create(payload.Name, payload.Email, null, null).Value;
                customer.ActivateCustomer();
                await _customerRepository.AddAsync(customer, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            string deviceId = Guid.NewGuid().ToString();
            List<Claim> claims = new()
            {
                new Claim("sub", customer.Id.ToString()),
                new Claim("email", customer.Email.Value),
                new Claim("userName", customer.CustomerName),
                new Claim("deviceId", deviceId)
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
                RefreshToken = refreshToken
            });
        }
    }
}