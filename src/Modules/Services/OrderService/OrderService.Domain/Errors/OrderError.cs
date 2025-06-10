using BuildingBlocks.Error;

namespace OrderService.Domain.Errors
{
    public static class OrderError
    {
        public static readonly Error OrderNotFound = Error.Validation("Order.NotFound", "Order not found.");
    }
}