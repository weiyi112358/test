using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Arvato.CRM.WebApplication.Controllers
{
    public class TmpCommunicationController : Controller
    {
        //
        // GET: /Communication/

        #region 短信沟通模板
        public ActionResult TmpMessage()
        {
            return View();
        }

        /// <summary>
        /// 增加或者更新短信模板信息
        /// </summary>
        /// <param name="templet"></param>
        /// <returns></returns>
        public JsonResult AddOrUpdateTmpMessageData(TmpCommunicationModel templet)
        {
            templet.DataGroupID = Auth.DataGroupID;
            templet.AddedUser = Auth.UserID.ToString();
            templet.ModifiedUser = Auth.UserID.ToString();
            templet.Name = HttpUtility.HtmlDecode(templet.Name);
            templet.BasicContent = HttpUtility.HtmlDecode(templet.BasicContent);
            templet.Remark = HttpUtility.HtmlDecode(templet.Remark);
            //templet.SchemaId = HttpUtility.HtmlDecode(templet.SchemaId);
            var result = Submit("TmpCommunication", "AddOrUpdateTmpMessageData", false, new object[] { templet });
            return Json(result);
        }

        #endregion

        #region 邮件沟通模板
        public ActionResult TmpMail()
        {
            return View();
        }

        /// <summary>
        /// 增加或者更新邮件模板信息
        /// </summary>
        /// <param name="templet"></param>
        /// <returns></returns>
        public JsonResult AddOrUpdateTmpMailData(TmpCommunicationModel templet)
        {
            templet.DataGroupID = Auth.DataGroupID;
            templet.AddedUser = Auth.UserID.ToString();
            templet.ModifiedUser = Auth.UserID.ToString();
            templet.Name = HttpUtility.HtmlDecode(templet.Name);
            templet.Topic = HttpUtility.HtmlDecode(templet.Topic);
            templet.BasicContent=HttpUtility.HtmlDecode(templet.BasicContent);
            templet.Remark = HttpUtility.HtmlDecode(templet.Remark);
            var result = Submit("TmpCommunication", "AddOrUpdateTmpMailData", false, new object[] { templet });
            return Json(result);
        }
        #endregion

        #region 微信沟通模板
        public ActionResult TmpWeChat()
        {
            return View();
        }

        public ActionResult TmpWeChatNew()
        {
            return View();
        }

        /// <summary>
        /// 增加或者更新微信模板信息
        /// </summary>
        /// <param name="templet"></param>
        /// <returns></returns>
        public JsonResult AddOrUpdateTmpWeChatData(TmpCommunicationModel templet)
        {
            templet.DataGroupID = Auth.DataGroupID;
            templet.AddedUser = Auth.UserID.ToString();
            templet.ModifiedUser = Auth.UserID.ToString();

            templet.Name = HttpUtility.HtmlDecode(templet.Name);
            templet.Remark = HttpUtility.HtmlDecode(templet.Remark);
            templet.BasicContent = HttpUtility.HtmlDecode(templet.BasicContent);
            var result = Submit("TmpCommunication", "AddOrUpdateTmpWeChatData", false, new object[] { templet });
            return Json(result);
        }
        #endregion

        #region 优惠券沟通模板
        public ActionResult TmpCoupon()
        {
            return View();
        }

        /// <summary>
        /// 增加或者更新优惠券模板信息
        /// </summary>
        /// <param name="templet"></param>
        /// <returns></returns>
        public JsonResult AddOrUpdateTmpCouponData(TmpCommunicationModel templet,string couponLimit)
        {
            templet.DataGroupID = Auth.DataGroupID;
            templet.AddedUser = Auth.UserID.ToString();
            templet.ModifiedUser = Auth.UserID.ToString();
            templet.Name = HttpUtility.HtmlDecode(templet.Name);
            templet.Topic = HttpUtility.HtmlDecode(templet.Topic);
            templet.BasicContent = HttpUtility.HtmlDecode(templet.BasicContent);
            templet.Remark = HttpUtility.HtmlDecode(templet.Remark);
            templet.ReferenceNo = HttpUtility.HtmlDecode(templet.ReferenceNo);
            var result = Submit("TmpCommunication", "AddOrUpdateTmpCouponData", false, new object[] { templet, couponLimit });
            return Json(result);
        }

        /// <summary>
        /// 获取优惠券限制
        /// </summary>
        /// <param name="tmpId"></param>
        /// <returns></returns>
        public JsonResult GetTmpCouponLimit(int tmpId)
        {
            var result = Submit("TmpCommunication", "GetTmpCouponLimit", false, new object[] { tmpId });
            return Json(result.Obj[0].ToString());
        }
        #endregion

        
        #region 沟通模板公共方法
        /// <summary>
        /// 加载沟通模板树图
        /// </summary>
        /// <param name="type">"SMS""Question""Coupon""EDM"</param>
        /// <returns></returns>
        public JsonResult GetTmpDataList(string type, string key)
        {
            key = HttpUtility.HtmlDecode(key);
            var result = Submit("TmpCommunication", "GetTmpDataList", true, new object[] { type,key });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 根据主键ID获取单条沟通模板信息
        /// </summary>
        /// <param name="dataGroupID"></param>
        /// <returns></returns>
        public JsonResult GetTmpDataById(int tmpId)
        {
            var result = Submit("TmpCommunication", "GetTmpDataById", false, new object[] { tmpId });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 根据主键ID删除单条沟通模板信息
        /// </summary>
        /// <param name="dataGroupID"></param>
        /// <returns></returns>
        public JsonResult DeleteTmpDataById(int tmpId)
        {
            var result = Submit("TmpCommunication", "DeleteTmpDataById", false, new object[] { tmpId });
            return Json(result);
        }

        /// <summary>
        /// 获取沟通模板类型
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTmpCatg()
        {
            var result = Submit("TmpCommunication", "GetTmpCatg", false);
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 获取微信类型
        /// </summary>
        /// <returns></returns>
        public JsonResult GetWeChatCatg()
        {
            string optType = "WeChatType";
            var result = Submit("Service", "GetOptionDataList", false, new object[] { optType });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 获取短信类型 车享平台
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTmpCXBusinessType()
        {
            var result = Submit("TmpCommunication", "GetTmpCXBusinessType", false);
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 获取个性化元素列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetElementsList()
        {
            var result = Submit("TmpCommunication", "GetElementsList", false);
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 获取问卷列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetQuestionList()
        {
            var result = Submit("TmpCommunication", "GetQuestionList", false);
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 获取优惠券列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSubTypeList()
        {
            string optType = "CouponType";
            var result = Submit("Service", "GetOptionDataList", false, new object[] { optType });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 获取体验项目列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetExperienceItemList()
        {
            string optType = "experience";
            int dataGroupId = (int)Auth.DataGroupID;
            var result = Submit("TmpCommunication", "GetItemList", false, new object[] { dataGroupId, optType });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 获取条目列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetItemList()
        {
            string optType = "gift";
            int dataGroupId = (int)Auth.DataGroupID;
            var result = Submit("TmpCommunication", "GetItemList", false, new object[] { dataGroupId, optType });
            return Json(result.Obj[0].ToString());
        }
        #endregion

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string UpLoadFileImg()
        {
            var file = Request.Files[0];
            string filePath = string.Empty;
            string path = FileUploadHelper.UpLoadFile("TmpWeChat", file, ref filePath);
            return filePath;
        }
    }
}
