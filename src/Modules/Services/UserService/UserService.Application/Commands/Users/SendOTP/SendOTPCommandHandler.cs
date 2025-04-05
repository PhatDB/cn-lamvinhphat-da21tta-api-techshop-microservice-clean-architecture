using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Abtractions;
using UserService.Domain.Abtractions.Repositories;
using UserService.Domain.Entities;
using UserService.Domain.Errors;
using UserService.Domain.ValueObjects;

namespace UserService.Application.Commands.Users.SendOTP
{
    public class SendOTPCommandHandler : ICommandHandler<SendOTPCommand>
    {
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public SendOTPCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }

        public async Task<Result> Handle(SendOTPCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.AsQueryable().Where(u => u.Email == new Email(request.Email)).FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                return Result.Failure(UserError.UserNotFound);

            user.UpdateOTP();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await SendVerificationEmail(user);

            return Result.Success();
        }

        private async Task SendVerificationEmail(User entity)
        {
            DTOs.Email email = new()
            {
                ToEmail = entity.Email.Value,
                Subject = "Verification",
                Body = $"<h2>OTP Verification</h2>\n" + $"<p>Dear {entity.Username},</p>\n" +
                       $"<p>Thank you for registering with us. To complete your registration, please use the following OTP:</p>\n" + $"<h3>{entity.OTP}</h3>\n" +
                       $"<p>If you did not request this, please ignore this email.</p>\n" + $"<p>Best regards,<br>TechShop.com</p>"
            };
            await _emailSender.SendEmailAsync(email);
        }
    }
}