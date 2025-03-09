namespace UserService.Application.DTOs
{
    public record Email(string ToEmail, string ToName, string Subject, string Body);
}