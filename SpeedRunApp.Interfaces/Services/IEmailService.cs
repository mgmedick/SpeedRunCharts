using System;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IEmailService
    {
        void SendEmailTemplate(string emailTo, string subject, string templateName, object parameters);
        void SendEmail(string emailTo, string subject, string htmlBody);
    }
}
