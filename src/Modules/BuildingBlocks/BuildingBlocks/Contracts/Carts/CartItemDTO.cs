namespace BuildingBlocks.Contracts.Carts
{
    public class CartItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductName { get; set; }
        public string ImgUrl { get; set; }
    }
}