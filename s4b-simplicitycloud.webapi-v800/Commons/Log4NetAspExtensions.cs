using log4net;
using log4net.Config;
using Microsoft.Extensions.Logging;
//using log4net.Repository.Hierarchy;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using log4net.Repository;
using System.Reflection;

namespace SimplicityOnlineWebApi.Commons
{
    public static class Log4NetAspExtensions
    {
        public static void ConfigureLog4Net(this IWebHostEnvironment hostingEnv, string configFileRelativePath)
        {
            GlobalContext.Properties["appRoot"] = hostingEnv.ContentRootPath;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository,new FileInfo(Path.Combine(hostingEnv.ContentRootPath, configFileRelativePath)));
        }

        public static void AddLog4Net(this ILoggerFactory loggerFactory)
        {
            loggerFactory.AddProvider(new Log4NetProvider());
        }
    }
}
