using BuildingBlocks.Contracts.Users;
using CustomerService.Domain.Abtractions.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastucture.Consumers
{
    public class IsCustomerExistConsumer : IConsumer<IsCustomerExistRequest>
    {
        private readonly ICustomerRepository _customerRepository;

        public IsCustomerExistConsumer(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task Consume(ConsumeContext<IsCustomerExistRequest> context)
        {
            int customerId = context.Message.CustomerId;

            bool exists = await _customerRepository.AsQueryable().AnyAsync(c => c.Id == customerId);

            await context.RespondAsync(new IsCustomerExistResponse(exists));
        }
    }
}