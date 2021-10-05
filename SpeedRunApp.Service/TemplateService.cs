using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using RazorEngineCore;
using Microsoft.Extensions.Caching.Memory;
using SpeedRunApp.Interfaces.Services;
using Serilog;

namespace SpeedRunApp.Service
{
    public class TemplateService : ITemplateService
    {
        private readonly IMemoryCache _cache = null;
        private readonly ILogger _logger = null;

        public TemplateService(IMemoryCache cache, ILogger logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public string RenderTemplate(string templateName, object model)
        {
            var result = string.Empty;
            try
            {
                var razorEngine = new RazorEngine();
                IRazorEngineCompiledTemplate template = null;

                if (!_cache.TryGetValue<IRazorEngineCompiledTemplate>(templateName, out template))
                {
                    string templateBody = GetTemplateContents(templateName);
                    template = razorEngine.Compile(templateBody);
                    _cache.Set(templateName, template);
                }

                result = template.Run(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "RenderTemplate");
            }

            return result;
        }

        private string GetTemplateContents(string templateFileName)
        {
            var assembly = Assembly.Load("SpeedRunApp.MVC");

            if (assembly != null)
            {
                StringBuilder sb = new StringBuilder();

                using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(String.Format("SpeedRunApp.MVC.Templates.{0}.cshtml", templateFileName))))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        sb.AppendLine(line);
                    }
                }

                return sb.ToString();
            }

            return null;
        }
    }
}
