using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AuthService.Application.Interfaces.IServices;
using static System.Net.WebRequestMethods;

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



        public async Task<bool> DrLicenseApprovedEmail(string email)
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
                mailMessage.Subject = " Your Medical License is Approved!";

                // Constructing the email body
                mailMessage.Body = $@"
                <html>
                <body style='font-family: Arial, sans-serif; color: #333;'>
                    <h2 style='color: #4CAF50;'> Congratulations, Doctor!</h2>
                    <p>We are pleased to inform you that your medical license verification has been successfully completed.</p>
                    
                    <p><strong>Your license is now officially approved.</strong></p>
                    
                    <p>You can now access all our services and start providing medical care through MediCloude.</p>
                    
                    <p>Thank you for your patience throughout the process. If you have any questions, feel free to reach out to us.</p>

                    <p>Best regards,</p>
                    <p><strong>MediCloude Team</strong></p>
                </body>
                </html>
            ";

                mailMessage.IsBodyHtml = true; 

                await smtpClient.SendMailAsync(mailMessage);
            }
            return true;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

        public async Task<bool> DrLicenseRejectedEmail(string email)
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
                    mailMessage.Subject = "Notification Regarding Your Medical License Application";

                    // Constructing the rejection email body
                    mailMessage.Body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <h2 style='color: #d9534f;'> Important Update on Your Application</h2>
                <p>Dear Applicant,</p>
                <p>Thank you for submitting your application for medical license verification. We appreciate your time and effort in the process.</p>
                
                <p>After careful review, we regret to inform you that we are unable to approve your application at this time.</p>
                
                <p>We understand this may be disappointing, and we encourage you to review the application criteria and consider reapplying in the future if circumstances permit.</p>
                
                <p>If you have any questions or require further clarification, please do not hesitate to reach out to our support team.</p>
                
                <p>Best regards,</p>
                <p><strong>MediCloude Team</strong></p>
            </body>
            </html>
        ";

                    mailMessage.IsBodyHtml = true;

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
