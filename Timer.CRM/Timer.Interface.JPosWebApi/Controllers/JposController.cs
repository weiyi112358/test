using Arvato.CRM.Model.Interface;
using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Timer.CRM.JposInterfaceLogic;



namespace Timer.Interface.JposWebApi.Controllers
{
    public class JposController : ApiController
    {

        /******************************************************************/
        private static HttpResponseMessage ReturnValue(object obj)
        {
            var resp = new HttpResponseMessage { Content = new StringContent(JsonHelper.Serialize(obj), System.Text.Encoding.UTF8, "application/json") };
            Log4netHelper.WriteInfoLog("EC:Result：//result:" + resp); return resp;
        }

    }

    public class CardController : ApiController
    {
        #region 卡信息查询
        /// <summary>
        /// 卡信息查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("QueryCard")]
        public async Task<HttpResponseMessage> QueryCard()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("QueryCard:" + result);
            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.QueryCard(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };


                Log4netHelper.WriteInfoLog("EC:QueryCard：//result:" + r);
                return resp;

            }

            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:QueryCard：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion

        #region 卡挂失查询
        /// <summary>
        /// 卡挂失查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("QueryLossCard")]
        public async Task<HttpResponseMessage> QueryLossCard()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("QueryLossCard:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.QueryLossCard(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:QueryLossCard：//result:" + r);
                return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:QueryLossCard：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }
        #endregion

        #region 卡挂失prepare
        /// <summary>
        /// 卡挂失prepare
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("LossCardPrepare")]
        public async Task<HttpResponseMessage> LossCardPrepare()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("LossCardPrepare:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.LossCardPrepare(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:LossCardPrepare：//result:" + r);
                return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:LossCardPrepare：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }


        }
        #endregion

        #region 卡挂失confirm
        /// <summary>
        /// 卡挂失comfirm
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("LossCardConfirm")]
        public async Task<HttpResponseMessage> LossCardConfirm()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("LossCardConfirm:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.LossCardConfirm(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:LossCardConfirm：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:LossCardConfirm：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }
        #endregion

        #region 查询费用

        [HttpPost]
        [ActionName("QueryFee")]
        public async Task<HttpResponseMessage> QueryFee()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("QueryFee:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.QueryFee(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:QueryFee：//result:" + r);
                return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:QueryFee：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }
        #endregion



        #region ReissureCardCancel

        [HttpPost]
        [ActionName("ReissureCardCancel")]
        public async Task<HttpResponseMessage> ReissureCardCancel()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("ReissureCardCancel:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.ReissureCardCancel(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:ReissureCardCancel：//result:" + r);
                return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:ReissureCardCancel：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion





        #region SendSMS

        [HttpPost]
        [ActionName("SendSMS")]
        public async Task<HttpResponseMessage> SendSMS()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("SendSMS:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.SendSMS(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:SendSMS：//result:" + r);
                return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:SendSMS：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion

        #region BatchSaleCardPrepare

        [HttpPost]
        [ActionName("BatchSaleCardPrepare")]
        public async Task<HttpResponseMessage> BatchSaleCardPrepare()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("BatchSaleCardPrepare:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.BatchSaleCardPrepare(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:BatchSaleCardPrepare：//result:" + r);
                return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:BatchSaleCardPrepare：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion






        #region BatchSaleCardConfirm

        [HttpPost]
        [ActionName("BatchSaleCardConfirm")]
        public async Task<HttpResponseMessage> BatchSaleCardConfirm()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("BatchSaleCardConfirm:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.BatchSaleCardConfirm(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:BatchSaleCardConfirm：//result:" + r);
                return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:BatchSaleCardConfirm：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion





        #region BatchSaleCardPrepareCancel

        [HttpPost]
        [ActionName("BatchSaleCardPrepareCancel")]
        public async Task<HttpResponseMessage> BatchSaleCardPrepareCancel()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("BatchSaleCardPrepareCancel:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.BatchSaleCardPrepareCancel(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:BatchSaleCardPrepareCancel：//result:" + r);
                return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:BatchSaleCardPrepareCancel：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion


        #region SaleCard

        [HttpPost]
        [ActionName("SaleCard")]
        public async Task<HttpResponseMessage> SaleCard()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("SaleCard:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.SaleCard(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:SaleCard：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:SaleCard：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion





        #region ChangeCardPassword

        [HttpPost]
        [ActionName("ChangeCardPassword")]
        public async Task<HttpResponseMessage> ChangeCardPassword()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("ChangeCardPassword:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.ChangeCardPassword(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:ChangeCardPassword：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:ChangeCardPassword：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion





        #region ChangePasswordPrepare

        [HttpPost]
        [ActionName("ChangePasswordPrepare")]
        public async Task<HttpResponseMessage> ChangePasswordPrepare()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("ChangePasswordPrepare:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.ChangePasswordPrepare(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:ChangePasswordPrepare：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:ChangePasswordPrepare：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion





        #region DepositPrepare

        [HttpPost]
        [ActionName("DepositPrepare")]
        public async Task<HttpResponseMessage> DepositPrepare()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("DepositPrepare:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.DepositPrepare(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:DepositPrepare：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:DepositPrepare：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion





        #region Deposit

        [HttpPost]
        [ActionName("Deposit")]
        public async Task<HttpResponseMessage> Deposit()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("Deposit:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.Deposit(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:Deposit：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:Deposit：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion







        /******************************************************************/
        private static HttpResponseMessage ReturnValue(object obj)
        {
            var resp = new HttpResponseMessage { Content = new StringContent(JsonHelper.Serialize(obj), System.Text.Encoding.UTF8, "application/json") };
            Log4netHelper.WriteInfoLog("EC:CardError：//result:" + JsonHelper.Serialize(obj)); return resp;
        }
    }

    public class MemberController : ApiController
    {
        #region 查询会员

        [HttpPost]
        [ActionName("QueryMember")]
        public async Task<HttpResponseMessage> QueryMember()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("QueryMember:" + result);
            if (result == null || string.IsNullOrWhiteSpace(result))
            {
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = "入参错误" });
            }

            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.QueryMember(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:QueryMember：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:QueryMember：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = ex.Message });
            }
        }
        #endregion


        #region GetAllMemberGrades

        [HttpPost]
        [ActionName("GetAllMemberGrades")]
        public async Task<HttpResponseMessage> GetAllMemberGrades()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("GetAllMemberGrades:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.GetAllMemberGrades(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:GetAllMemberGrades：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:GetAllMemberGrades：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion





        #region QueryMemberSource

        [HttpPost]
        [ActionName("QueryMemberSource")]
        public async Task<HttpResponseMessage> QueryMemberSource()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("QueryMemberSource:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.QueryMemberSource(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:QueryMemberSource：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:QueryMemberSource：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion


        #region mbrPay

        [HttpPost]
        [ActionName("mbrPay")]
        public async Task<HttpResponseMessage> mbrPay()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("mbrPay:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.mbrPay(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:mbrPay：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:mbrPay：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion





        #region GetMemberAttributeOptions

        [HttpPost]
        [ActionName("GetMemberAttributeOptions")]
        public async Task<HttpResponseMessage> GetMemberAttributeOptions()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("GetMemberAttributeOptions:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.GetMemberAttributeOptions(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:GetMemberAttributeOptions：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:GetMemberAttributeOptions：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion





        #region SaveMember

        [HttpPost]
        [ActionName("SaveMember")]
        public async Task<HttpResponseMessage> SaveMember()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("SaveMember:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.SaveMember(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:SaveMember：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:SaveMember：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion


        #region 卡补发

        [HttpPost]
        [ActionName("ReissureCard")]
        public async Task<HttpResponseMessage> ReissureCard()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("ReissureCard:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.ReissureCard(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:ReissureCard：//result:" + r);
                return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:ReissureCard：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }
        #endregion


        /******************************************************************/
        private static HttpResponseMessage ReturnValue(object obj)
        {
            var resp = new HttpResponseMessage { Content = new StringContent(JsonHelper.Serialize(obj), System.Text.Encoding.UTF8, "application/json") };
            Log4netHelper.WriteInfoLog("EC:MemberError：//result:" + JsonHelper.Serialize(obj)); return resp;
        }
    }

    public class ScoreController : ApiController
    {
        #region ScoreSave

        [HttpPost]
        [ActionName("ScoreSave")]
        public async Task<HttpResponseMessage> ScoreSave()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("ScoreSave:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.ScoreSave(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:ScoreSave：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:ScoreSave：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion

        #region QueryCardScore

        [HttpPost]
        [ActionName("QueryCardScore")]
        public async Task<HttpResponseMessage> QueryCardScore()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("QueryCardScore:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.QueryCardScore(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:QueryCardScore：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:QueryCardScore：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion

        #region ScorePrize

        [HttpPost]
        [ActionName("ScorePrize")]
        public async Task<HttpResponseMessage> ScorePrize()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("ScorePrize:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.ScorePrize(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:ScorePrize：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:ScorePrize：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion

        #region ScorePrizeCancel

        [HttpPost]
        [ActionName("ScorePrizeCancel")]
        public async Task<HttpResponseMessage> ScorePrizeCancel()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("ScorePrizeCancel:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.ScorePrizeCancel(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:ScorePrizeCancel：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:ScorePrizeCancel：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion

        /******************************************************************/
        private static HttpResponseMessage ReturnValue(object obj)
        {
            var resp = new HttpResponseMessage { Content = new StringContent(JsonHelper.Serialize(obj), System.Text.Encoding.UTF8, "application/json") };
            Log4netHelper.WriteInfoLog("EC:ScoreError：//result:" + JsonHelper.Serialize(obj)); return resp;
        }
    }

    public class PrizeController : ApiController
    {
        #region QueryPrizeGoodsList

        [HttpPost]
        [ActionName("QueryPrizeGoodsList")]
        public async Task<HttpResponseMessage> QueryPrizeGoodsList()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("QueryPrizeGoodsList:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.QueryPrizeGoodsList(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:QueryPrizeGoodsList：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:QueryPrizeGoodsList：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion

        #region 查询会员参与活动次数

        [HttpPost]
        [ActionName("QueryMbrJoinRuleCount")]
        public async Task<HttpResponseMessage> QueryMbrJoinRuleCount()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("QueryMbrJoinRuleCount:" + result);
            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.QueryMbrJoinRuleCount(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:QueryMbrJoinRuleCount：//result:" + r); return resp;
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:QueryMbrJoinRuleCount：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion


        #region 查询会员参与活动次数

        [HttpPost]
        [ActionName("QueryMemberGetPresentCounts")]
        public async Task<HttpResponseMessage> QueryMemberGetPresentCounts()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("QueryMemberGetPresentCounts:" + result);
            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.QueryMemberGetPresentCounts(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:QueryMemberGetPresentCounts：//result:" + r); return resp;
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:QueryMemberGetPresentCounts：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion

        #region QueryPrizeGoods

        [HttpPost]
        [ActionName("QueryPrizeGoods")]
        public async Task<HttpResponseMessage> QueryPrizeGoods()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("QueryPrizeGoods:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.QueryPrizeGoods(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:QueryPrizeGoods：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:QueryPrizeGoods：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion





        #region Prize

        [HttpPost]
        [ActionName("Prize")]
        public async Task<HttpResponseMessage> Prize()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("Prize:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.Prize(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:Prize：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:Prize：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion





        #region PrizeCancel

        [HttpPost]
        [ActionName("PrizeCancel")]
        public async Task<HttpResponseMessage> PrizeCancel()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("PrizeCancel:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.PrizeCancel(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:PrizeCancel：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:PrizeCancel：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion


        /******************************************************************/
        private static HttpResponseMessage ReturnValue(object obj)
        {
            var resp = new HttpResponseMessage { Content = new StringContent(JsonHelper.Serialize(obj), System.Text.Encoding.UTF8, "application/json") };
            Log4netHelper.WriteInfoLog("EC:PrizeError：//result:" + JsonHelper.Serialize(obj));
            return resp;
        }
    }

    public class VoucherController : ApiController
    {



        #region QueryVoucher

        [HttpPost]
        [ActionName("QueryVoucher")]
        public async Task<HttpResponseMessage> QueryVoucher()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("QueryVoucher:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.QueryVoucher(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:QueryVoucher：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:QueryVoucher：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion





        #region VoucherAbolish

        [HttpPost]
        [ActionName("VoucherAbolish")]
        public async Task<HttpResponseMessage> VoucherAbolish()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("VoucherAbolish:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.VoucherAbolish(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:VoucherAbolish：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:VoucherAbolish：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion





        #region VoucherActivate

        [HttpPost]
        [ActionName("VoucherActivate")]
        public async Task<HttpResponseMessage> VoucherActivate()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("VoucherActivate:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.VoucherActivate(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:VoucherActivate：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:VoucherActivate：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion





        #region VoucherPrint

        [HttpPost]
        [ActionName("VoucherPrint")]
        public async Task<HttpResponseMessage> VoucherPrint()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("VoucherPrint:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.VoucherPrint(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:VoucherPrint：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:VoucherPrint：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion





        #region VoucherUse

        [HttpPost]
        [ActionName("VoucherUse")]
        public async Task<HttpResponseMessage> VoucherUse()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("VoucherUse:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.VoucherUse(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:VoucherUse：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:VoucherUse：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion





        #region LocalData

        [HttpPost]
        [ActionName("LocalData")]
        public async Task<HttpResponseMessage> LocalData()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("LocalData:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.LocalData(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:LocalData：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:LocalData：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion





        #region LocalDataDone

        [HttpPost]
        [ActionName("LocalDataDone")]
        public async Task<HttpResponseMessage> LocalDataDone()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("LocalDataDone:" + result);


            try
            {
                JposInterfaceBiz biz = new JposInterfaceBiz();
                var r = biz.LocalDataDone(result);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                Log4netHelper.WriteInfoLog("EC:LocalDataDone：//result:" + r); return resp;
            }


            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:LocalDataDone：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { ErrorMsg = ex.Message });
            }
        }

        #endregion











        /******************************************************************/
        private static HttpResponseMessage ReturnValue(object obj)
        {
            var resp = new HttpResponseMessage { Content = new StringContent(JsonHelper.Serialize(obj), System.Text.Encoding.UTF8, "application/json") };
            Log4netHelper.WriteInfoLog("EC:VoucherError：//result:" + JsonHelper.Serialize(obj)); return resp;
        }
    }


}
