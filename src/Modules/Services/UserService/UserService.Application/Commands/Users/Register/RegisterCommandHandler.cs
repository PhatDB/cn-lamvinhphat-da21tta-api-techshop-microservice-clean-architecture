using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Abtractions;
using UserService.Application.DTOs;
using UserService.Domain.Abtractions.Repositories;
using UserService.Domain.Entities;
using UserService.Domain.Errors;

namespace UserService.Application.Commands.Users.Register
{
    public class RegisterCommandHandler : ICommandHandler<RegisterCommand, int>
    {
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public RegisterCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _emailSender = emailSender;
        }


        public async Task<Result<int>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            bool userExists =
                (await _userRepository.AsQueryable().AsNoTracking().Select(u => new { u.Id, Email = u.Email.Value }).ToListAsync(cancellationToken)).Any(u =>
                    u.Email == request.Email);

            if (userExists)
                return Result.Failure<int>(UserError.UserAlreadyExists);

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            Result<User> userResult = User.Create(request.Username, request.Email, hashedPassword);
            if (userResult.IsFailure)
                return Result.Failure<int>(userResult.Error);

            User user = userResult.Value;

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await SendVerificationEmail(user);

            return Result.Success(user.Id);
        }

        private async Task SendVerificationEmail(User entity)
        {
            Email email = new()
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