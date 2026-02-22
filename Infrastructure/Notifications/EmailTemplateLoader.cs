using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Notifications
{
    public static class EmailTemplateLoader
    {
        public static string Load(string templateName, Dictionary<string, string> tokens)
        {
            var basePath = Path.Combine(
                AppContext.BaseDirectory,
                "Notifications",
                "EmailTemplates"
            );

            var layoutPath = Path.Combine(basePath, "EmailLayout.html");
            var bodyPath = Path.Combine(basePath, templateName);

            if (!File.Exists(layoutPath) || !File.Exists(bodyPath))
                throw new FileNotFoundException("Email template not found.");

            var layout = File.ReadAllText(layoutPath);
            var body = File.ReadAllText(bodyPath);


                foreach (var token in tokens)
                    body = body.Replace($"{{{{{token.Key}}}}}", token.Value);

            return layout.Replace("{{Body}}", body);
        }
    }


}
