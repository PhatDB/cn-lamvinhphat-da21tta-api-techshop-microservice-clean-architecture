using BuildingBlocks.Contracts.Products;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using CustomerService.Application.Abtractions;
using CustomerService.Application.DTOs;
using CustomerService.Domain.Abtractions.Repositories;
using CustomerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Application.Queries.Reviews
{
    public class GetAllReviewsQueryHandler : IQueryHandler<GetAllReviewsQuery, List<ReviewDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerService _customerService;

        public GetAllReviewsQueryHandler(ICustomerRepository customerRepository, ICustomerService customerService)
        {
            _customerRepository = customerRepository;
            _customerService = customerService;
        }

        public async Task<Result<List<ReviewDto>>> Handle(
            GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
            List<Customer> customers = await _customerRepository.AsQueryable().Include(c => c.Reviews).AsNoTracking()
                .ToListAsync(cancellationToken);

            List<Review> allReviews = customers.SelectMany(c => c.Reviews).ToList();

            List<int> productIds = allReviews.Select(r => r.ProductId).Distinct().ToList();

            Result<GetListProductInfoResponse> productInfoResult =
                await _customerService.ReviewGetListProductInfo(productIds);

            if (productInfoResult.IsFailure)
                return Result.Failure<List<ReviewDto>>(productInfoResult.Error);

            Dictionary<int, ProductInfoResponse> productDict =
                productInfoResult.Value.ProductInfos.ToDictionary(p => p.ProductId, p => p);

            List<ReviewDto> result = new();

            foreach (Customer customer in customers)
            foreach (Review review in customer.Reviews)
            {
                if (!productDict.TryGetValue(review.ProductId, out ProductInfoResponse? product)) continue;

                result.Add(new ReviewDto
                {
                    CustomerId = customer.Id,
                    CustomerName = customer.CustomerName,
                    Email = customer.Email,
                    Comment = review.Comment,
                    Rating = review.Rating.Value,
                    IsVerified = review.IsVerified.Value,
                    ProductId = review.ProductId,
                    ProductName = product.ProductName,
                    ImageUrl = product.ImageUrl
                });
            }

            return Result.Success(result);
        }
    }
}