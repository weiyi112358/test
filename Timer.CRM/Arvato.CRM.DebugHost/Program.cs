using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arvato.CRM.WcfFramework.Server;

namespace Arvato.CRM.DebugHost
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                winout(TraceLevel.Info, "程序启动");
                WcfService wcf = WcfService.GetInstance();
                wcf.LunchNotify += wcf_LunchNotify;
                wcf.Start();
                winout(TraceLevel.Info, "启动成功");
            }
            catch (Exception exp)
            {
                winout(TraceLevel.Error, exp.Message);
            }
            Console.ReadKey();
            winout(TraceLevel.Info, "程序退出");
        }

        static void wcf_LunchNotify(object sender, TraceLevel lvl, Exception exp)
        {
            winout(lvl, exp.Message);
        }

        static void winout(TraceLevel lvl, string message)
        {
            Console.WriteLine("时间:{0} 等级:{1} 描述:{2}", DateTime.Now, lvl, message);
        }
    }
}
