namespace BuildingBlocks.Contracts.Carts
{
    public record GetCartInfoResponse(int CartId, int CustomerId, List<CartItemDTO> CartItems);
}