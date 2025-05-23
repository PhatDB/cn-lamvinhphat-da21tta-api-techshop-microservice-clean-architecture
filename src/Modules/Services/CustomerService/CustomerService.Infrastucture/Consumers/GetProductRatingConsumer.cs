using BuildingBlocks.Contracts.Customers;
using CustomerService.Domain.Abtractions.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastucture.Consumers
{
    public class GetProductRatingConsumer : IConsumer<GetProductsRatingRequest>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetProductRatingConsumer(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task Consume(ConsumeContext<GetProductsRatingRequest> context)
        {
            List<int> productIds = context.Message.ProductIds;

            List<RatingRespone> ratings = await _customerRepository.AsQueryable().Include(c => c.Reviews)
                .SelectMany(c => c.Reviews).Where(r => productIds.Contains(r.ProductId) && r.Rating != null)
                .GroupBy(r => r.ProductId)
                .Select(g => new RatingRespone(g.Key, (byte)Math.Round(g.Average(r => r.Rating.Value)))).ToListAsync();

            await context.RespondAsync(new GetProductsRatingRespone(ratings));
        }
    }
}