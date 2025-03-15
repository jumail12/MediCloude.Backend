using AuthService.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> SendOtp(string email, int otp)
        {
            try
            {
                string host = Environment.GetEnvironmentVariable("EMAIL_HOST");
                string senderEmail = Environment.GetEnvironmentVariable("EMAIL_USERNAME");
                string password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");

                if (!int.TryParse(Environment.GetEnvironmentVariable("EMAIL_PORT"), out int port))
                {
                    throw new Exception("Invalid SMTP port configuration.");

                }
                using (var smtpClient = new SmtpClient(host))
                using (var mailMessage = new MailMessage())
                {
                    smtpClient.Port = port;
                    smtpClient.Credentials = new NetworkCredential(senderEmail, password);
                    smtpClient.EnableSsl = true;

                    mailMessage.From = new MailAddress(senderEmail);
                    mailMessage.To.Add(email);
                    mailMessage.Subject = "OTP Verification";
                    mailMessage.Body = $"Your OTP for email verification is {otp}";

                    await smtpClient.SendMailAsync(mailMessage);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
