namespace CustomerService.Application.DTOs
{
    public class LoginDto
    {
        public int CustomerId { get; set; }
        public string Email { get; set; }
        public string CustomerName { get; set; }
        public string AccessToken { get; set; }
        public int AccessTokenExpires { get; set; }
        public string RefreshToken { get; set; }
    }
}