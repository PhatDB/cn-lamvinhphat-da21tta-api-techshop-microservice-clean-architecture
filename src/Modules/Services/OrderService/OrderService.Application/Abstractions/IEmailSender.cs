using OrderService.Application.DTOs;

namespace OrderService.Application.Abstractions
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailDto toEmail);
    }
}