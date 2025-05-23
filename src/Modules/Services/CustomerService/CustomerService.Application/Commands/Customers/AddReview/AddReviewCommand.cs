using BuildingBlocks.CQRS;

namespace CustomerService.Application.Commands.Customers.AddReview
{
    public record AddReviewCommand(int CustomerId, int ProductId, byte Rating, string Comment) : ICommand;
}