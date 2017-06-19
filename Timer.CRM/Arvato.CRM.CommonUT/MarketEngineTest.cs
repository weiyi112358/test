using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Arvato.CRM.MarketActivityLogic;
using Arvato.CRM.BizLogic;
using Arvato.CRM.Model;
using System.Collections.Generic;

namespace Arvato.CRM.CommonUT
{
    [TestClass]
    public class MarketEngineTest
    {
        [TestMethod]
        public void ActiveTest()
        {
            ActivityManager.Active();
        }

        [TestMethod]
        public void TranslateTest()
        {
            ActivityManager.Translate();
        }

        [TestMethod]
        public void PushTest()
        {
            ActivityManager.Push();
        }

        [TestMethod]
        public void PullTest()
        {
            ActivityManager.Pull();
        }

        [TestMethod]
        public void NextTest()
        {
            ActivityManager.Next();
        }

        [TestMethod]
        public void FinishTest()
        {
            ActivityManager.Finish();
        }

        [TestMethod]
        public void CreateCoupon()
        {
            try {
                ActivityManager.CreateCoupon();

                List<string> l = new List<string>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }


        //[TestMethod]
        //public void GetCouponByLimit()
        //{
        //    List<CouponLimitPair> lists = new List<CouponLimitPair>();
        //    CouponLimitPair pa = new CouponLimitPair();
        //    pa.LimitType = "BrandStore";
        //    pa.LimitValue = "YQFT";

        //    lists.Add(pa);
        //    try
        //    {
        //        Member360.GetCouponByLimit();
        //    }
        //    catch (Exception e)
        //    { 
            
        //    }
            
        //}
    }
}
