using System;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailTemplate(string emailTo, string subject, string templateName, object parameters);
        Task SendEmail(string emailTo, string subject, string htmlBody);
    }
}
