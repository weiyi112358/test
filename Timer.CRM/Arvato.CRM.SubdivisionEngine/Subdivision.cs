using Arvato.CRM.MemberSubdivisionLogic;
using Arvato.CRM.SubdivisionEngine.Properties;
using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.SubdivisionEngine
{
    public partial class Subdivision : ServiceBase
    {
        private static int ComputeTimes;
        private Result RunResult;
    
        public Subdivision()
        {
            //计算次数
            ComputeTimes = 0;
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //关闭计时器
            tim.Start();
        }

        protected override void OnStop()
        {
            //关闭计时器
            tim.Stop();
        }

        private void tim_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (ComputeTimes == 0 || ComputeTimes > 99)
            {
                ComputeTimes += 1;
                ////计算全量维度
                var planComputeTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + Settings.Default.DIVISION_PLAN_COMPUTE_TIME);
                RunResult = MemberSubdivision.ComputeAllDimension(Settings.Default.DIVISION_LAST_COMPUTE_TIME, planComputeTime, DateTime.Now, Settings.Default.COMMAND_TIMEOUT);
                if (!RunResult.IsPass)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("ComputeAllDimension Stop At {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    sb.AppendLine(RunResult.MSG);
                    Log.ServiceLog(sb.ToString());
                }
                else
                {
                    Settings.Default.DIVISION_LAST_COMPUTE_TIME = DateTime.Now;
                }
                //执行会员细分
                RunResult = MemberSubdivision.Run(DateTime.Now);
                if (!RunResult.IsPass)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("MemberSubdivision Stop At {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    sb.AppendLine(RunResult.MSG);
                    Log.ServiceLog(sb.ToString());
                }

               // 执行KPI计算
                planComputeTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + Settings.Default.KPI_PLAN_COMPUTE_TIME);
                RunResult = KPI.ComputeAllKPI(Settings.Default.KPI_LAST_COMPUTE_TIME, planComputeTime, DateTime.Now);
                if (!RunResult.IsPass)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("KPI Compute Stop At {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    sb.AppendLine(RunResult.MSG);
                    Log.ServiceLog(sb.ToString());
                }
                else
                {
                    Settings.Default.KPI_LAST_COMPUTE_TIME = DateTime.Now;
                }

                ComputeTimes = 0;
            }
            else
            {
                Log.ServiceLog("MemberSubdivision Delay " + ComputeTimes.ToString() + " Times At " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                ComputeTimes += 1;
            }

        }
    }
}
