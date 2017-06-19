using Arvato.CRM.Utility;
using Arvato.CRM.WcfFramework.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.WCFHost
{
    public partial class Wcf : ServiceBase
    {
        public Wcf()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                WcfService wcf = WcfService.GetInstance();
                wcf.LunchNotify += wcf_LunchNotify;
                wcf.Start();
                Log.WriteFormatLog("4SCRMWcfHost", "OnStart");
            }
            catch (Exception e)
            {
                Log.WriteFormatLog("4SCRMWcfHost", e.Message);
            }
        }

        protected override void OnStop()
        {
            try
            {
                WcfService wcf = WcfService.GetInstance();
                wcf.Stop();
                Log.WriteFormatLog("4SCRMWcfHost", "OnStop");
            }
            catch (Exception e)
            {
                Log.WriteFormatLog("4SCRMWcfHost", e.Message);
            }
        }

        private void wcf_LunchNotify(object sender, TraceLevel level, Exception exp)
        {
            switch (level)
            {
                case TraceLevel.Error:
                    Log.WriteFormatLog("4SCRMWcfHost", exp.ToString());
                    break;
                case TraceLevel.Info:
                    Log.WriteFormatLog("4SCRMWcfHost", exp.ToString());
                    break;
                case TraceLevel.Verbose:
                    Log.WriteFormatLog("4SCRMWcfHost", exp.ToString());
                    break;
                case TraceLevel.Warning:
                    Log.WriteFormatLog("4SCRMWcfHost", exp.ToString());
                    break;
            }
        }
    }
}
