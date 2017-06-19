using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arvato.CRM.Utility.Datatables;
using Arvato.CRM.Utility;
using Arvato.CRM.Model;

namespace Arvato.CRM.WebApplication.Controllers
{
    public class CreditTransferController : Controller
    {
        //
        // GET: /CreditTransfer/
        #region 积分转让列表
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetCreditTransfers(string name, string mobile, DateTime? start, DateTime? end)
        {
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("CreditTransfer", "GetCreditTransfers", false, new object[] { JsonHelper.Serialize(dts), Auth.DataGroupID ?? 0, name, mobile, start, end });
            return Json(rst.Obj[0].ToString());
        }
        #endregion

        #region 积分转让Add
        public ActionResult Add()
        {
            return View();
        }

        public JsonResult GetMembersForCredit(string mobile, string certificate, string vehicleNo, string vin)
        {
            var dts = Request.CreateDataTableParameter();
            string pageIds = Auth.CurPageID.HasValue ? Auth.CurPageID.ToString() : "";
            string dataRoleIds = "(" + string.Join(",", Auth.RoleIDs) + ")";
            var rst = Submit("CreditTransfer", "GetMembersForCredit", false, new object[] { JsonHelper.Serialize(dts), Auth.DataGroupID ?? 0,pageIds,dataRoleIds, mobile, certificate, vehicleNo, vin });
            return Json(rst.Obj[0].ToString());
        }

        public JsonResult GetCreditSummary(string memberId)
        {
            var rst = Submit("CreditTransfer", "GetCreditSummary", false, new object[] { memberId });
            return Json(rst);
        }

        public JsonResult GetCreditDetails(string memberId)
        {
            var rst = Submit("CreditTransfer", "GetCreditDetails", false, new object[] { memberId });
            return Json(rst);
        }

        public JsonResult SubmitCreditTransfer(string fromMemId, string toMemId, List<CreditTransferDetail> details)
        {
            var rst = Submit("CreditTransfer", "CreateCreditTransfer1", false, new object[] { fromMemId, toMemId, Util.Serialize(details), Auth.DataGroupID.Value, Auth.UserID.ToString() });
            return Json(rst);
        }
        #endregion

        #region 积分转让比例管理
        public ActionResult TransferRate()
        {
            //var result = Submit("CreditTransfer", "GetSubDataGroupList", false, new object[] { Auth.DataGroupID });
            //ViewBag.DataGroupList = result.Obj[0];
            return View();
        }

        public JsonResult GetTransferRates()
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("CreditTransfer", "GetTransferRates", false, new object[] { JsonHelper.Serialize(dts) });
            return Json(result.Obj[0]);
        }

        public JsonResult GetSubDataGroupList(int? selected)
        {
            var result = Submit("CreditTransfer", "GetSubDataGroupList", false, new object[] { Auth.DataGroupID.Value, selected
            });
            return Json(result);
        }

        public JsonResult EditRate(int id)
        {
            var result = Submit("CreditTransfer", "GetTransferRateById", false, new object[] { id });
            return Json(result);
        }

        public JsonResult EditRateSubmit(string modelStr)
        {
            var result = Submit("CreditTransfer", "SaveTransferRate", false, new object[] { modelStr, Auth.UserID.ToString() });
            return Json(result);
        }

        public JsonResult DeleteRate(int id)
        {
            var result = Submit("CreditTransfer", "DeleteTransferRate", false, new object[] { id });
            return Json(result);
        }
        #endregion
    }
}
