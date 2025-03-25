using BuildingBlocks.Contracts.Users;
using BuildingBlocks.Results;
using MassTransit;
using UserService.Domain.Abtractions.Repositories;

namespace UserService.Infrastucture.Consumers
{
    public class UserExistConsumer : IConsumer<UserExistRequest>
    {
        private readonly IUserRepository _userRepository;

        public UserExistConsumer(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Consume(ConsumeContext<UserExistRequest> context)
        {
            Result<bool> exists = await _userRepository.IsExistAsync(context.Message.UserId);
            await context.RespondAsync(new UserExistResponse(exists.Value));
        }
    }
}