namespace BuildingBlocks.Contracts.Orders
{
    public record OrderCreated(int OrderId, int CustomerId, int CartId, List<OrderItemInfo> Items);
}