using CustomerService.Application.DTOs;

namespace CustomerService.Application.Abtractions
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailDto toEmail);
    }
}