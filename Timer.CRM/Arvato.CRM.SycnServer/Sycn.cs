using Arvato.CRM.SycnLogic;
using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Arvato.CRM.SycnServer.Properties;

namespace Arvato.CRM.SycnServer
{
    public partial class Sycn : ServiceBase
    {

        private Timer tSync;
        private static int ComputeTimes;
        private Timer tSync_Loy;
        private static int ComputeTimes_Loy;
        public Sycn()
        {
            //计算次数
            ComputeTimes = 0;
            ComputeTimes_Loy = 0;
            InitializeComponent();

            tSync = new Timer();
            tSync.Elapsed += tSync_Tick;
            tSync.AutoReset = true;

            tSync_Loy = new Timer();
            tSync_Loy.Elapsed += tSyncLoy_Tick;
            tSync_Loy.AutoReset = true;
        }

        protected override void OnStart(string[] args)
        {
            tSync.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["CouponSync"]);
            tSync_Loy.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["LoySync"]);
            tSync.Start();
            tSync_Loy.Start();
        }

        protected override void OnStop()
        {
            tSync.Stop();
            tSync_Loy.Stop();
        }

        private void tSync_Tick(object sender, EventArgs e)
        {
            try
            {
                Log4netHelper.WriteInfoLog("同步购物券开始");
                if (ComputeTimes == 0 || ComputeTimes > 99)
                {
                    ComputeTimes += 1;
                    POSSycnBiz.syncCoupon();
                    ComputeTimes = 0;
                    Log4netHelper.WriteInfoLog("同步购物券结束");
                }
                else
                {
                    Log4netHelper.WriteInfoLog("CouponSync：delay" + ComputeTimes);
                    ComputeTimes += 1;
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("CouponSync：" + ex.ToString() + ex.Message + ex.InnerException);
            }
        }

        private void tSyncLoy_Tick(object sender, EventArgs e)
        {
            try
            {
                Log4netHelper.WriteInfoLog("执行订单积分升降级开始");
                if (ComputeTimes_Loy == 0 || ComputeTimes_Loy > 99)
                {
                    ComputeTimes_Loy += 1;
                    POSSycnBiz.loy();
                    ComputeTimes_Loy = 0;
                    Log4netHelper.WriteInfoLog("执行订单积分升降级结束");
                }
                else
                {
                    Log4netHelper.WriteInfoLog("LoySync：delay" + ComputeTimes_Loy);
                    ComputeTimes_Loy += 1;
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("LoySync：" + ex.ToString() + ex.Message + ex.InnerException);
            }
        }
    }
}
