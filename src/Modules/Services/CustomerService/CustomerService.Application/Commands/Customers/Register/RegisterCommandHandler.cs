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
                Subject = "[TechShop] Mã xác thực OTP của bạn",
                Body = $@"
            <!DOCTYPE html>
            <html lang=""vi"">
                <head><meta charset=""utf-8""></head>
                <body style=""font-family:Arial,Helvetica,sans-serif;color:#333;line-height:1.5;"">
                <h2 style=""margin:0 0 12px;"">Xác thực tài khoản</h2>
                <p>Xin chào <strong>{entity.CustomerName}</strong>,</p>
                <p>Cảm ơn bạn đã đăng ký tại <strong>TechShop.com</strong>. Để hoàn tất quá trình, vui lòng sử dụng mã OTP dưới đây:</p>
                <h3 style=""font-size:24px;margin:16px 0;color:#0f172a;"">{entity.Otp}</h3>
                <p>Mã OTP có hiệu lực trong 5 phút. Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email.</p>
                <p>Trân trọng,<br><strong>TechShop.com</strong></p>
            </body>
            </html>"
            };
            await _emailSender.SendEmailAsync(email);
        }
    }
}