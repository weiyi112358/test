using System;
using Arvato.CRM.MemberSubdivisionLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Arvato.CRM.Trigger;
using System.Collections.Generic;
using Arvato.CRM.MarketActivityLogic;

namespace Arvato.CRM.CommonUT
{
    [TestClass]
    public class EngineTest
    {
        [TestMethod]
        public void TestSubdivision()
        {
            try
            {
                var RunResult = MemberSubdivision.Run(DateTime.Now);
                string ddd = "";
            }
            catch (Exception ex)
            {
                string abc = ex.Message;
            }
            
        }

        [TestMethod]
        public void TestSingleSubdivision()
        {
            try
            {
                MemberSubdivision.ComputeDimension("select MemberID from TM_Mem_Master ", DateTime.Now, new List<int> { 2914 });
                //MemberSubdivision.ComputeDimension(new List<string> { "003d9eb80e14471285ce0d3d458d2a4c", "003d9eb80e14471285ce0d3d458d2a4c" }, DateTime.Now, new List<int> { 2914 });
                string ddd = "";
            }
            catch (Exception ex)
            {
                string abc = ex.Message;
            }

        }

        [TestMethod]
        public void TestKPI()
        {
            try
            {
                var RunResult = KPI.ComputeAllKPI(new DateTime(2015, 10, 26), new DateTime(2015, 10, 27), DateTime.Now);
                string ddd = "";
            }
            catch (Exception ex)
            {
                string abc = ex.Message;
            }
        }

        [TestMethod]
        public void TestLoy()
        {
            try
            {
                var el = new ExtraLoyalty
                {
                    DataGroupID = 1,
                    //RuleRunType ="1",
                    RuleType = "1"
                };
                LoyaltyTrigger tr = new LoyaltyTrigger(el);
                tr.MemberScript = "Select MemberID From TM_Mem_Master where MemberID ='4c645d67b74f4b7e8c182985d8d73a00'";
                tr.Ext.SearchTradeDetailSQL = "Select TradeDetailID From TM_Mem_TradeDetail where TradeID =41";
                tr.Ext.SearchTradeSQL = "Select TradeID From TM_Mem_Trade  where TradeID =41";
                tr.StartTime = DateTime.Now;
                //tr.Callback = new LoyaltyTrigger.CallBackMethod(gothisway);
                tr.Start();
            }
            catch (Exception ex)
            {
                string abc = ex.Message;
            }
        }

         [TestMethod]
        public void TestMarking()
        {
            ActivityManager.CreateCoupon();
        
        }

    }
}
