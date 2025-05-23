﻿using System.Net;
using System.Net.Mail;
using CustomerService.Application.Abtractions;
using CustomerService.Application.DTOs;
using CustomerService.Infrastucture.DependencyInjections.Options;
using Microsoft.Extensions.Configuration;

namespace CustomerService.Infrastucture.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailOption emailOption = new();

        public EmailSender(IConfiguration configuration)
        {
            configuration.GetSection(nameof(EmailOption)).Bind(emailOption);
        }

        public async Task SendEmailAsync(EmailDto email)
        {
            SmtpClient smtpClient = new(emailOption.Host)
            {
                Port = emailOption.Port,
                Credentials = new NetworkCredential(emailOption.Username, emailOption.Password),
                EnableSsl = true
            };

            MailMessage mailMessage = new()
            {
                From = new MailAddress(emailOption.From, emailOption.DisplayName),
                Subject = email.Subject,
                Body = email.Body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email.ToEmail);

            string[] bbcs = emailOption.Bcc.Split(',').ToArray();
            bool bccHasEmptyString = bbcs.Any(string.IsNullOrEmpty);
            if (!bccHasEmptyString)
                foreach (string bcc in bbcs)
                    mailMessage.Bcc.Add(new MailAddress(bcc));

            string[] ccs = emailOption.Cc.Split(',').ToArray();
            bool ccHasEmptyString = ccs.Any(string.IsNullOrEmpty);
            if (!ccHasEmptyString)
                foreach (string cc in ccs)
                    mailMessage.CC.Add(new MailAddress(cc));

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}