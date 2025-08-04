using BuildingBlocks.Contracts.Orders;
using BuildingBlocks.Results;
using CustomerService.Domain.Abtractions.Repositories;
using CustomerService.Domain.Entities;
using MassTransit;

namespace CustomerService.Infrastucture.Consumers
{
    public class GetCustomerInfoConsumer : IConsumer<GetCustomerInfoRequest>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerInfoConsumer(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task Consume(ConsumeContext<GetCustomerInfoRequest> context)
        {
            GetCustomerInfoRequest req = context.Message;

            Result<Customer> result = await _customerRepository.GetByIdAsync(req.CustomerId, context.CancellationToken);

            if (result.IsFailure || result.Value is null)
            {
                await context.RespondAsync(new GetCustomerInfoResponse(string.Empty, string.Empty, string.Empty));
                return;
            }

            Customer c = result.Value;

            await context.RespondAsync(new GetCustomerInfoResponse(c.CustomerName, c.Phone?.Value ?? string.Empty,
                c.Email.Value));
        }
    }
}