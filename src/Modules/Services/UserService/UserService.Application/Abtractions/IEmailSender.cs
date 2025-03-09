using UserService.Application.DTOs;

namespace UserService.Application.Abtractions
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Email toEmail);
    }
}