namespace BuildingBlocks.Contracts.Orders
{
    public record OrderCancel(List<OrderItemDTO> OrderItems);
}