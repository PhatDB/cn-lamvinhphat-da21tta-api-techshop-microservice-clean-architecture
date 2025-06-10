using BuildingBlocks.Contracts.Customers;
using BuildingBlocks.Results;

namespace ProductService.Application.Abtractions
{
    public interface IProductService
    {
        Task<Result<GetProductReviewsResponse>> GetProductReviews(int productId);

        Task<Result<GetProductsRatingResponse>> GetProductsRating(List<int> productIds);
    }
}