namespace ApiGateway.ReverseProxy.DependencyInjections.Options
{
    public class JwtOption
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryMinutes { get; set; }
        public int RefreshTokenExpiryDays { get; set; }
    }
}