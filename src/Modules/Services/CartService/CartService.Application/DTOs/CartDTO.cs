namespace CartService.Application.DTOs
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public int? CustomerId { get; set; }
        public List<ProductDTO> CartItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}