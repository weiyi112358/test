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
using Timer.CRM.WechatInterfaceLogic;
using System.Data.Entity.Validation;



namespace Timer.Interface.WechatWebApi.Controllers
{
    public class WechatController : ApiController
    {

        #region 新会员入会
        /// <summary>
        /// 新会员入会
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("AddWeChatMember")]
        public async Task<HttpResponseMessage> AddWeChatMember()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("AddWeChatMember:" + result);
            if (result == null || string.IsNullOrWhiteSpace(result))
            {
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = "入参错误" });
            }

            try
            {
                WechatInterfaceBiz biz = new WechatInterfaceBiz();
                var r = biz.AddWeChatMember(result);
                Log4netHelper.WriteInfoLog("AddWeChatMember：//result:" + r);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                return resp;
            }
            catch (DbEntityValidationException ex)
            {

                foreach (var item in ex.EntityValidationErrors.ToList())
                {
                    foreach (var v in item.ValidationErrors.ToList())
                    {
                        Console.WriteLine(v.ErrorMessage + v.PropertyName);
                    }
                }
                Log4netHelper.WriteErrorLog("EC:AddMemberWTC：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { Status = 1, ErrorMsg = ex.ToString() });
            }

            //catch (Exception ex)
            //{
            //    Log4netHelper.WriteErrorLog("EC:AddWeChatMember：//input:" + result + "//false：" + ex.ToString());
            //    return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = ex.Message });
            //}
        }


        #endregion

        #region 顾问绑定
        /// <summary>
        /// 新会员入会
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("BindConsultant")]
        public async Task<HttpResponseMessage> BindConsultant()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("BindConsultant:" + result);
            if (result == null || string.IsNullOrWhiteSpace(result))
            {
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = "入参错误" });
            }

            try
            {
                WechatInterfaceBiz biz = new WechatInterfaceBiz();
                var r = biz.BindConsultant(result);
                Log4netHelper.WriteInfoLog("BindConsultant：//result:" + r);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                return resp;
            }

            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:QueryMember：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = ex.Message });
            }
        }


        #endregion

        #region 我的等级
        /// <summary>
        /// 新会员入会
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("QueryGrade")]
        public async Task<HttpResponseMessage> QueryGrade()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("QueryGrade:" + result);
            if (result == null || string.IsNullOrWhiteSpace(result))
            {
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = "入参错误" });
            }

            try
            {
                WechatInterfaceBiz biz = new WechatInterfaceBiz();
                var r = biz.QueryGrade(result);
                Log4netHelper.WriteInfoLog("QueryGrade：//result:" + r);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                return resp;
            }

            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:QueryGrade：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = ex.Message });
            }
        }


        #endregion

        #region 积分明细
        /// <summary>
        /// 新会员入会
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("QueryPointsDetail")]
        public async Task<HttpResponseMessage> QueryPointsDetail()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("QueryPointsDetail:" + result);
            if (result == null || string.IsNullOrWhiteSpace(result))
            {
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = "入参错误" });
            }

            try
            {
                WechatInterfaceBiz biz = new WechatInterfaceBiz();
                var r = biz.QueryPointsDetail(result);
                Log4netHelper.WriteInfoLog("QueryPointsDetail：//result:" + r);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                return resp;
            }

            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:QueryPointsDetail：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = ex.Message });
            }
        }


        #endregion

        #region 我的积分
        /// <summary>
        /// 新会员入会
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("QueryPoints")]
        public async Task<HttpResponseMessage> QueryPoints()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("QueryPoints:" + result);
            if (result == null || string.IsNullOrWhiteSpace(result))
            {
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = "入参错误" });
            }

            try
            {
                WechatInterfaceBiz biz = new WechatInterfaceBiz();
                var r = biz.QueryPoints(result);
                Log4netHelper.WriteInfoLog("QueryPoints：//result:" + r);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                return resp;
            }

            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:QueryPoints：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = ex.Message });
            }
        }


        #endregion



        #region 成员同步
        /// <summary>
        /// 成员同步
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("QueryUser")]
        public async Task<HttpResponseMessage> QueryUser()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("QueryUser:" + result);
            if (result == null || string.IsNullOrWhiteSpace(result))
            {
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = "入参错误" });
            }

            try
            {
                WechatInterfaceBiz biz = new WechatInterfaceBiz();
                var r = biz.QueryUser(result);
                Log4netHelper.WriteInfoLog("QueryUser：//result:" + r);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                return resp;
            }

            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:QueryUser：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = ex.Message });
            }
        }


        #endregion

        #region 组织架构同步
        /// <summary>
        /// 组织架构同步
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("QueryOrganization")]
        public async Task<HttpResponseMessage> QueryOrganization()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("QueryOrganization:" + result);
            if (result == null || string.IsNullOrWhiteSpace(result))
            {
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = "入参错误" });
            }

            try
            {
                WechatInterfaceBiz biz = new WechatInterfaceBiz();
                var r = biz.QueryOrganization(result);
                Log4netHelper.WriteInfoLog("QueryOrganization：//result:" + r);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                return resp;
            }

            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:QueryOrganization：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = ex.Message });
            }
        }
        #endregion

        #region 会员查询
        /// <summary>
        /// 会员查询
        /// </summary>
        /// <returns></returns>
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
                WechatInterfaceBiz biz = new WechatInterfaceBiz();
                var r = biz.QueryMember(result);
                Log4netHelper.WriteInfoLog("QueryMember：//result:" + r);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                return resp;
            }

            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:QueryMember：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = ex.Message });
            }
        }
        #endregion

        #region 更新会员
        /// <summary>
        /// 更新会员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateMember")]
        public async Task<HttpResponseMessage> UpdateMember()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("UpdateMember:" + result);
            if (result == null || string.IsNullOrWhiteSpace(result))
            {
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = "入参错误" });
            }

            try
            {
                WechatInterfaceBiz biz = new WechatInterfaceBiz();
                var r = biz.UpdateMember(result);
                Log4netHelper.WriteInfoLog("UpdateMember：//result:" + r);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                return resp;
            }

            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:UpdateMember：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = ex.Message });
            }
        }
        #endregion


        #region 增加短信记录
        /// <summary>
        /// 增加短信记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("AddSMS")]
        public async Task<HttpResponseMessage> AddSMS()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("AddSMS:" + result);
            if (result == null || string.IsNullOrWhiteSpace(result))
            {
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = "入参错误" });
            }

            try
            {
                WechatInterfaceBiz biz = new WechatInterfaceBiz();
                var r = biz.AddSMS(result);
                Log4netHelper.WriteInfoLog("AddSMS：//result:" + r);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                return resp;
            }

            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:AddSMS：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = ex.Message });
            }
        }
        #endregion


        #region 会员订单查询
        /// <summary>
        /// 增加短信记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("QueryMemberTrades")]
        public async Task<HttpResponseMessage> QueryMemberTrades()
        {
            string result = await Request.Content.ReadAsStringAsync();
            Log4netHelper.WriteInfoLog("QueryMemberTrades:" + result);
            if (result == null || string.IsNullOrWhiteSpace(result))
            {
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = "入参错误" });
            }

            try
            {
                WechatInterfaceBiz biz = new WechatInterfaceBiz();
                var r = biz.QueryMemberTrades(result);
                Log4netHelper.WriteInfoLog("QueryMemberTrades：//result:" + r);
                var resp = new HttpResponseMessage { Content = new StringContent(r, System.Text.Encoding.UTF8, "application/json") };
                return resp;
            }

            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("EC:QueryMemberTrades：//input:" + result + "//false：" + ex.ToString());
                return ReturnValue(new CommonJsonOutput() { Status = -1, ErrorMsg = ex.Message });
            }
        }
        #endregion

        /******************************************************************/
        private static HttpResponseMessage ReturnValue(object obj)
        {
            var resp = new HttpResponseMessage { Content = new StringContent(JsonHelper.Serialize(obj), System.Text.Encoding.UTF8, "application/json") };
            return resp;
        }

    }
}