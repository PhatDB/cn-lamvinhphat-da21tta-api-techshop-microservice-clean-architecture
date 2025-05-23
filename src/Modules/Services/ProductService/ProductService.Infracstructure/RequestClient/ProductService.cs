using BuildingBlocks.Contracts.Customers;
using BuildingBlocks.Results;
using MassTransit;
using ProductService.Application.Abtractions;

namespace ProductService.Infracstructure.RequestClient
{
    public class ProductService : IProductService
    {
        private readonly IRequestClient<GetProductReviewsRequest> _customerClient;
        private readonly IRequestClient<GetProductsRatingRequest> _ratingClient;

        public ProductService(
            IRequestClient<GetProductReviewsRequest> customerClient,
            IRequestClient<GetProductsRatingRequest> ratingClient)
        {
            _customerClient = customerClient;
            _ratingClient = ratingClient;
        }

        public async Task<Result<GetProductsRatingRespone>> GetProductsRating(List<int> productIds)
        {
            Response<GetProductsRatingRespone> ratingRespone =
                await _ratingClient.GetResponse<GetProductsRatingRespone>(new GetProductsRatingRequest(productIds));

            return Result.Success(ratingRespone.Message);
        }

        public async Task<Result<GetProductReviewsRespone>> GetProductReviews(int productId)
        {
            Response<GetProductReviewsRespone> reviewRespone =
                await _customerClient.GetResponse<GetProductReviewsRespone>(new GetProductReviewsRequest(productId));

            return Result.Success(reviewRespone.Message);
        }
    }
}