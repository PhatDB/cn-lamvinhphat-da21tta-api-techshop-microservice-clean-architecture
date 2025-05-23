using BuildingBlocks.Contracts.Customers;
using CustomerService.Domain.Abtractions.Repositories;
using CustomerService.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastucture.Consumers
{
    public class GetReviewConsumer : IConsumer<GetProductReviewsRequest>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetReviewConsumer(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task Consume(ConsumeContext<GetProductReviewsRequest> context)
        {
            GetProductReviewsRequest request = context.Message;

            List<Customer> customers = await _customerRepository.AsQueryable().Include(c => c.Reviews)
                .Where(c => c.Reviews.Any(r => r.ProductId == request.ProductId)).ToListAsync();

            List<ProductReviewDto> productReviews = customers.Select(c => new ProductReviewDto
            {
                CustomerId = c.Id,
                CustomerName = c.CustomerName,
                Reviews = c.Reviews.Where(r => r.ProductId == request.ProductId).Select(r =>
                    new ReviewDto
                    {
                        ReviewId = r.Id,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        IsVerified = r.IsVerified,
                        Created_at = r.CreatedAt ?? DateTime.MinValue
                    }).ToList()
            }).ToList();

            await context.RespondAsync(new GetProductReviewsRespone(productReviews));
        }
    }
}