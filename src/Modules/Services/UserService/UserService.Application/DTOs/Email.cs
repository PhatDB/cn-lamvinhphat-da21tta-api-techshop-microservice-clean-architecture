namespace UserService.Application.DTOs
{
    public class Email
    {
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}