using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using UserService.Domain.Abtractions.Repositories;
using UserService.Domain.Entities;
using UserService.Domain.Errors;

namespace UserService.Application.Commands.Users.ChangePassword
{
    public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public ChangePasswordCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            Result<User> userResult = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (userResult.IsFailure)
                return Result.Failure<int>(UserError.UserNotFound);

            User user = userResult.Value;

            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.Password.Value))
                return Result.Failure<int>(UserError.IncorrectOldPassword);

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            user.UpdatePassword(hashedPassword);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(user.Id);
        }
    }
}