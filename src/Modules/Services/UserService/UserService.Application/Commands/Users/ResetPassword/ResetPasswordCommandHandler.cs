using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Abtractions;
using UserService.Domain.Abtractions.Repositories;
using UserService.Domain.Entities;
using UserService.Domain.Errors;
using UserService.Domain.ValueObjects;

namespace UserService.Application.Commands.Users.ResetPassword
{
    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand>
    {
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public ResetPasswordCommandHandler(IEmailSender emailSender, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.AsQueryable().Where(u => u.Email == new Email(request.Email)).FirstOrDefaultAsync(cancellationToken);

            if (user == null || user.IsActive == false)
                return Result.Failure(UserError.UserNotFound);

            user.UpdateOTP();

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.OTP);
            user.UpdatePassword(hashedPassword);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await SendVerificationEmail(user);

            return Result.Success();
        }

        private async Task SendVerificationEmail(User entity)
        {
            DTOs.Email email = new()
            {
                ToEmail = entity.Email.Value,
                Subject = "Password Reset",
                Body = $@"
                <h2>Password Reset</h2>
                <p>Dear {entity.Username},</p>
                <p>We have received a request to reset your password. Your temporary password is:</p>
                <h3>{entity.OTP}</h3>
                <p>Please use this password to log in and change it immediately for security reasons.</p>
                <p>If you did not request a password reset, please contact our support team.</p>
                <p>Best regards,<br>TechShop.com</p>"
            };
            await _emailSender.SendEmailAsync(email);
        }
    }
}