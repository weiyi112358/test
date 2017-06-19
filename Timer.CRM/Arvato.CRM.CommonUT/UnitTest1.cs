using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Arvato.CRM.Utility;
using Arvato.CRM.Model;
using Arvato.CRM.EF;
using System.Linq;
using System.Threading.Tasks;
using Arvato.CRM.CommunicateEngine;

namespace Arvato.CRM.CommonUT
{
    [TestClass]
    public class UnitTest1
    {
        public void testSMS()
        {
            try
            {
                SmsManager.Send();
            }
            catch (Exception ex)
            {

                throw;
            }

        }



        [TestMethod]
        public void testc()
        {
            try
            {
                var db = new CRMEntities();
                var cardBox = db.Tm_Card_CardBox.Where(i => i.BeginCardNo.CompareTo("11111") > 0).FirstOrDefault();
            }
            catch (Exception ex)
            { 
            
            }
        }
    }





}
