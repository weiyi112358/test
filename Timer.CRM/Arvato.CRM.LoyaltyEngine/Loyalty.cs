using Arvato.CRM.EF;
using Arvato.CRM.LoyaltyEngine.Properties;
using Arvato.CRM.Trigger;
using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.LoyaltyEngine
{
    public partial class Loyalty : ServiceBase
    {
        private static int ComputeTimes;
        private Result RunResult;

        public Loyalty()
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

        private void gothisway(Result rst)
        {
            RunResult = rst;
            if (!RunResult.IsPass)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Loy Stop At {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                sb.AppendLine(RunResult.MSG);
                Log.ServiceLog(sb.ToString());
            }

        }

        private void tim_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (ComputeTimes == 0 || ComputeTimes > 99)
            {
                ComputeTimes += 1;

              
                using (CRMEntities db = new CRMEntities())
                {
          
                    //执行冻结转可用
                    var planTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + Settings.Default.FREEZE_TO_AVAI);
             
                    if (Settings.Default.LAST_CHANGE_FREEZE_TIME < planTime && planTime <= DateTime.Now)
                    {
               
                        try
                        {
                            var cmd = db.Database.Connection.CreateCommand();
                            cmd.CommandTimeout = 360;
                            cmd.CommandText = "sp_Loy_FreezeToThawAccountUpdate";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            if (db.Database.Connection.State == System.Data.ConnectionState.Closed) db.Database.Connection.Open();
                            cmd.ExecuteNonQuery();
                            Settings.Default.LAST_CHANGE_FREEZE_TIME = DateTime.Now;
                    
                        }
                        catch (Exception ex)
                        {                          
                            Log.ServiceLog("FreezeToAvai Stop At " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                            Log.ServiceLog(ex.Message);
                        }
                    }

              
                    //执行可用转失效
                    var planTime1 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + Settings.Default.FREEZE_TO_AVAI);
                    //Log.ServiceLog("6");
                    if (Settings.Default.LAST_CHANGE_AVAI_TIME < planTime1 && planTime1 <= DateTime.Now)
                    {
                        //Log.ServiceLog("7");
                        try
                        {
                            var cmd = db.Database.Connection.CreateCommand();
                            cmd.CommandTimeout = 360;
                            cmd.CommandText = "sp_Loy_DisabledAccountUpdate";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            if (db.Database.Connection.State == System.Data.ConnectionState.Closed) db.Database.Connection.Open();
                            cmd.ExecuteNonQuery();
                            Settings.Default.LAST_CHANGE_AVAI_TIME = DateTime.Now;
                        }
                        catch
                        {
                            Log.ServiceLog("AvaiToZero Stop At " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        }
                    }

                    //执行非增量规则
                    foreach (var dg in db.TM_SYS_DataGroup.ToList())
                    {
                        var el = new ExtraLoyalty
                        {
                            RuleType = "2",
                            db = db,
                            DataGroupID = dg.DataGroupID
                        };

                        LoyaltyTrigger tr = new LoyaltyTrigger(el);

                        tr.MemberScript = "Select MemberID From TM_Mem_Master where DataGroupID = " + dg.DataGroupID.ToString();
                        tr.Ext.SearchTradeDetailSQL = "Select TradeDetailID From TM_Mem_TradeDetail inner join TM_Mem_Trade on TM_Mem_TradeDetail.TradeID = TM_Mem_Trade.TradeID and TM_Mem_Trade.Bool_Attr_2=1 and TM_Mem_Trade.NeedLoyCompute = 1 and DataGroupID = " + dg.DataGroupID.ToString();
                        tr.Ext.SearchTradeSQL = "Select TradeID From TM_Mem_Trade where NeedLoyCompute = 1 and DataGroupID =  " + dg.DataGroupID.ToString();

                        tr.StartTime = DateTime.Now;
                        tr.Callback = new LoyaltyTrigger.CallBackMethod(gothisway);
                        tr.Start();
                    }
                }
                ComputeTimes = 0;

            }
            else
            {
                Log.ServiceLog("Loy Delay " + ComputeTimes.ToString() + " Times At " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                ComputeTimes += 1;
            }
        }
    }
}
