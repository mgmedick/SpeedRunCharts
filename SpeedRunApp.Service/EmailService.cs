using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using Serilog;
using SpeedRunApp.Interfaces.Services;
using System.Threading.Tasks;

namespace SpeedRunApp.Service
{
    public class EmailService : IEmailService
    {
        private readonly ITemplateService _templateService = null;
        private readonly IConfiguration _config = null;
        private readonly ILogger _logger = null;

        public EmailService(ITemplateService templateService, IConfiguration config, ILogger logger)
        {
            _templateService = templateService;
            _config = config;
            _logger = logger;
        }

        public async Task SendEmailTemplate(string emailTo, string subject, string templateName, object parameters)
        {
            var htmlBody = _templateService.RenderTemplate(templateName, parameters);

            if (!string.IsNullOrWhiteSpace(htmlBody))
            {
                await SendEmail(emailTo, subject, htmlBody);
            }
        }

        public async Task SendEmail(string emailTo, string subject, string htmlBody)
        {
            var emailFrom = _config.GetSection("SiteSettings").GetSection("FromEmail").Value;
            var emailUsername = _config.GetSection("SiteSettings").GetSection("EmailUsername").Value;
            var emailPassword = _config.GetSection("SiteSettings").GetSection("EmailPassword").Value;

            var smtpHost = _config.GetSection("SiteSettings").GetSection("SmtpHost").Value;
            var smtpPort = Convert.ToInt32(_config.GetSection("SiteSettings").GetSection("SmtpPort").Value);

            var message = new MailMessage(emailFrom, emailTo, subject, htmlBody);
            message.IsBodyHtml = true;

            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(emailUsername, emailPassword);
                await client.SendMailAsync(message);
            }
        }
    }
}
