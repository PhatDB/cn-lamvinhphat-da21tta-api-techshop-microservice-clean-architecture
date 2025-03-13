namespace UserService.Application.DTOs
{
    public class LoginDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public int AccessTokenExpires { get; set; }
        public string RefreshToken { get; set; }
    }
}