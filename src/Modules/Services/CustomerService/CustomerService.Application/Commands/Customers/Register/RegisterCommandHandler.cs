using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using CustomerService.Application.Abtractions;
using CustomerService.Application.DTOs;
using CustomerService.Domain.Abtractions.Repositories;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Application.Commands.Customers.Register
{
    public class RegisterCommandHandler : ICommandHandler<RegisterCommand, int>
    {
        private readonly ICustomerRepository _customerRepository;

        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(
            IEmailSender emailSender, IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
        {
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
        }


        public async Task<Result<int>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            bool userExists =
                (await _customerRepository.AsQueryable().AsNoTracking().Select(u => new { u.Id, Email = u.Email.Value })
                    .ToListAsync(cancellationToken)).Any(u => u.Email == request.Email);

            if (userExists)
                return Result.Failure<int>(CustomerError.CustomerAlreadyExists);

            if (request.Password.Length < 8)
                return Result.Failure<int>(CustomerError.PasswordWeak);

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            Result<Customer> customerResult =
                Customer.Create(request.CustomerName, request.Email, null, hashedPassword);
            if (customerResult.IsFailure)
                return Result.Failure<int>(customerResult.Error);

            Customer customer = customerResult.Value;

            await _customerRepository.AddAsync(customer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await SendVerificationEmail(customer);

            return Result.Success(customer.Id);
        }

        private async Task SendVerificationEmail(Customer entity)
        {
            EmailDto email = new()
            {
                ToEmail = entity.Email.Value,
                Subject = "Verification",
                Body = $"<h2>OTP Verification</h2>\n" + $"<p>Dear {entity.CustomerName},</p>\n" +
                       $"<p>Thank you for registering with us. To complete your registration, please use the following OTP:</p>\n" +
                       $"<h3>{entity.Otp}</h3>\n" +
                       $"<p>If you did not request this, please ignore this email.</p>\n" +
                       $"<p>Best regards,<br>TechShop.com</p>"
            };
            await _emailSender.SendEmailAsync(email);
        }
    }
}