namespace CustomerService.Application.DTOs
{
    public class AddressDto
    {
        public int AddressId { get; set; }
        public string? Street { get; set; }
        public string? Hamlet { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
    }
}