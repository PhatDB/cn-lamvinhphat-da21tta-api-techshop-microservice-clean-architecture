using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Abtractions;
using UserService.Domain.Abtractions.Repositories;
using UserService.Domain.Entities;
using UserService.Domain.Errors;
using UserService.Domain.ValueObjects;

namespace UserService.Application.Commands.Users.VertifyOTP
{
    public class VertitfyOTPCommandHandler : ICommandHandler<VertifyOTPCommand>
    {
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public VertitfyOTPCommandHandler(IEmailSender emailSender, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(VertifyOTPCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.AsQueryable().Where(u => u.Email == new Email(request.Email)).FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                return Result.Failure(UserError.UserNotFound);
            if (!user.ValidateOTP(request.OTP))
                return Result.Failure(Error.Validation("OTP.InValid", "OTP is invalid"));

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}