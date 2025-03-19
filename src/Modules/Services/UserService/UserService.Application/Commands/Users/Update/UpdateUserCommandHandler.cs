using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Abtractions.Repositories;
using UserService.Domain.Entities;
using UserService.Domain.Errors;

namespace UserService.Application.Commands.Users.Update
{
    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.AsQueryable().Include(u => u.UserAddresses).Where(u => u.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                return Result.Failure(UserError.UserNotFound);

            Result updateResult = user.UpdateUserAndAddress(request.Username, request.AddressLine, request.PhoneNumber, request.Province, request.District);

            if (updateResult.IsFailure)
                return Result.Failure(updateResult.Error);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}