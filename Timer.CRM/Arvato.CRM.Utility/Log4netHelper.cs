using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Arvato.CRM.Utility
{  
    public class Log4netHelper
    {
        public static void WriteInfoLog(string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("loginfo");
            log.Info(msg);
        }

        public static void WriteErrorLog(string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("logerror");
            log.Info(msg);
        }

    }
}
