namespace BuildingBlocks.Contracts.Carts
{
    public record GetCartInfoResponse(int CartId, int UserId, List<CartItemDTO> CartItems);
}