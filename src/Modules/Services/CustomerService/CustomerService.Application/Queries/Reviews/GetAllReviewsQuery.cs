using BuildingBlocks.CQRS;
using CustomerService.Application.DTOs;

namespace CustomerService.Application.Queries.Reviews
{
    public record GetAllReviewsQuery : IQuery<List<ReviewDto>>;
}