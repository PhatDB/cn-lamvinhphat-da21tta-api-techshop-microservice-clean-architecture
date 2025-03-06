using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Abtractions.Repositories;
using UserService.Domain.Entities;
using UserService.Domain.Errors;

namespace UserService.Application.Commands.Users.Register
{
    public class RegisterCommandHandler : ICommandHandler<RegisterCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public RegisterCommandHandler(
            IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(
            RegisterCommand request, CancellationToken cancellationToken)
        {
            bool userExists = (await _userRepository.AsQueryable().AsNoTracking()
                .Select(u => new { u.Id, Email = u.Email.Value })
                .ToListAsync(cancellationToken)).Any(u => u.Email == request.Email);

            if (userExists)
                return Result.Failure<int>(UserError.UserAlreadyExists);

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            Result<User> userResult = User.Create(request.Username,
                request.Email, hashedPassword);
            if (userResult.IsFailure)
                return Result.Failure<int>(userResult.Error);

            User user = userResult.Value;

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(user.Id);
        }
    }
}