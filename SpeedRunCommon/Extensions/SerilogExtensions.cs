using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Sinks.Email;
using Serilog.Sinks.PeriodicBatching;
using Serilog.Settings.Configuration;
using System;
using System.Collections.Generic;
using System.Net;

namespace SpeedRunCommon.Extensions
{
    public static class SerilogExtensions
    {
        private const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}";
        private const int DefaultBatchPostingLimit = 100;

        public static LoggerConfiguration EmailCustom(
        this LoggerSinkConfiguration loggerConfiguration,
        string fromEmail,
        string toEmail,
        string mailServer,
        string userName,
        string password,
        int port = 25,
        bool enableSsl = false,
        ICredentialsByHost networkCredential = null,
        string outputTemplate = DefaultOutputTemplate,
        LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
        int batchPostingLimit = DefaultBatchPostingLimit,
        TimeSpan? period = null,
        IFormatProvider formatProvider = null,
        string mailSubject = EmailConnectionInfo.DefaultSubject)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");
            if (fromEmail == null) throw new ArgumentNullException("fromEmail");
            if (toEmail == null) throw new ArgumentNullException("toEmail");

            var connectionInfo = new EmailConnectionInfo
            {
                FromEmail = fromEmail,
                ToEmail = toEmail,
                MailServer = mailServer,
                Port = port,
                EnableSsl = enableSsl,
                NetworkCredentials = new NetworkCredential(userName, password),
                EmailSubject = mailSubject
            };

            return loggerConfiguration.Email(connectionInfo, outputTemplate, restrictedToMinimumLevel, batchPostingLimit, period, formatProvider);
        }
    }
}
