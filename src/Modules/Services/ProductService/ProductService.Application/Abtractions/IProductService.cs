using BuildingBlocks.Contracts.Customers;
using BuildingBlocks.Contracts.Products;
using BuildingBlocks.Results;

namespace ProductService.Application.Abtractions
{
    public interface IProductService
    {
        Task<Result<GetProductReviewsRespone>> GetProductReviews(int productId);

        Task<Result<GetProductsRatingRespone>> GetProductsRating(List<int> productIds);
    }
}