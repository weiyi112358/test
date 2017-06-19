using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using Arvato.CRM.Model;

namespace Arvato.CRM.WebApplication.Controllers
{
    public class BusinessPlanController : Controller
    {
        public JsonResult SearchBusinessPlan(string planName, string planType, DateTime? planStartTime, DateTime? planEndTime,
            string planCode, string status)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("BusinessPlan", "GetBusinessPlan", false, new object[] { planName, planType, planStartTime, planEndTime, planCode,status,
                JsonHelper.Serialize(pageParamers),Auth.DataGroupID });
            return Json(result.Obj[0].ToString());
        }

        public ActionResult BusinessPlanInsert(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            try
            {
                var result = Submit("BusinessPlan", "GetBusinessPlanDetailByPlanID", false, new object[] { id, Auth.DataGroupID });
                if (result.IsPass)
                {
                    BusinessPlanModel model = JsonHelper.Deserialize<BusinessPlanModel>(JsonHelper.Serialize(result.Obj.FirstOrDefault()));
                    return View(model);
                }
                else
                {
                    return Redirect("/BusinessPlan");
                }
            }
            catch
            {
                return Redirect("/BusinessPlan");
            }
        }

        public ActionResult BusinessPlanDetail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Redirect("/BusinessPlan");
            }
            try
            {
                var result = Submit("BusinessPlan", "GetBusinessPlanDetailByPlanID", false, new object[] { id, Auth.DataGroupID });
                if (result.IsPass)
                {
                    BusinessPlanModel model = JsonHelper.Deserialize<BusinessPlanModel>(JsonHelper.Serialize(result.Obj.FirstOrDefault()));
                    return View(model);
                }
                else
                {
                    return Redirect("/BusinessPlan");
                }
            }
            catch
            {
                return Redirect("/BusinessPlan");
            }

        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllBusinessPlan()
        {
            try
            {
                var result = Submit("BusinessPlan", "GetAllBusinessPlan", false, new object[] { Auth.DataGroupID });
                if (result.IsPass)
                {
                    List<BusinessPlanModel> list = JsonHelper.Deserialize<List<BusinessPlanModel>>(JsonHelper.Serialize(result.Obj[0]));
                    return Json(list);
                }
                else
                {
                    return Json("");
                }
            }
            catch
            {
                return Json("");
            }
        }

        public JsonResult GetAllEnableBusinessPlan()
        {
            try
            {
                var result = Submit("BusinessPlan", "GetAllEnableBusinessPlan", false, new object[] { Auth.DataGroupID });
                if (result.IsPass)
                {
                    List<BusinessPlanModel> list = JsonHelper.Deserialize<List<BusinessPlanModel>>(JsonHelper.Serialize(result.Obj[0]));
                    return Json(list);
                }
                else
                {
                    return Json("");
                }
            }
            catch
            {
                return Json("");
            }
        }

        public JsonResult GetKPIData()
        {
            DatatablesParameter para = new DatatablesParameter();
            para.iDisplayLength = 0;
            para.iDisplayStart = 0;
            try
            {
                var result = Submit("KPI", "GetKPI", false, new object[] { null, null, "3", null, Auth.DataGroupID });
                List<KPIModel> list = JsonHelper.Deserialize<List<KPIModel>>(JsonHelper.Serialize(result.Obj[0]));
                DatatablesSourceVsPage page = new DatatablesSourceVsPage();

                if (list != null && list.Count > 0)
                {
                    page.aaData = list;
                    page.iDisplayLength = list.Count;
                    page.iDisplayStart = 0;
                    page.iTotalRecords = list.Count;
                    para.iDisplayLength = list.Count;

                    return GenerateAssignedModelList<object>(page, para);
                }

                return DefaultNullModelList(para);

            }
            catch
            {
                return DefaultNullModelList(para);
            }
        }

        public JsonResult SearchBusinessPlanResult(string planID)
        {
            var result = Submit("KPIResult", "GetKPIResult", false, new object[] { null, null, "3", planID, null, null, false, Auth.DataGroupID });
            return Json(result.Obj[0].ToString());
        }

        public JsonResult SearchBusinessPlanTarget(string planID)
        {
            var result = Submit("KPITarget", "GetKPITarget", false, new object[] { null, "3", planID, Auth.DataGroupID });
            return Json(result.Obj[0].ToString());
        }

        public JsonResult SearchActivityByBusinessPlan(string planID)
        {
            DatatablesParameter pageInfo = Request.CreateDataTableParameter();
            var result = Submit("MarketActivity", "SearchActivity", false,
                new object[] { null, null, null, null, null, null, null, planID, JsonHelper.Serialize(pageInfo), Auth.DataGroupID });
            return Json(result.Obj[0].ToString());
        }

        public JsonResult AddBusinessPlan(string businessPlan, string businessPlanTargetList)
        {
            try
            {
                BusinessPlanModel plan = JsonHelper.Deserialize<BusinessPlanModel>(businessPlan);
                plan.BusinessPlanID = Guid.NewGuid().ToString().Replace("-", "");
                plan.AddedDate = DateTime.Now;
                plan.AddedUser = Auth.UserID.ToString();

                if (Auth.DataGroupID.HasValue)
                {
                    plan.DataGroupID = Auth.DataGroupID.Value;
                    var result = Submit("BusinessPlan", "InsertBusinessPlan", false, new object[] { JsonHelper.Serialize(plan), businessPlanTargetList });
                    return Json(result);
                }

                return Json(new Result(false, "没有新增商业计划的权限", null));
            }
            catch (Exception ex)
            {
                return Json(new Result(false, ex.Message, null));
            }
        }

        public JsonResult UpdateBusinessPlan(string businessPlan, string businessPlanTargetList)
        {
            try
            {
                BusinessPlanModel plan = JsonHelper.Deserialize<BusinessPlanModel>(businessPlan);
                plan.ModifiedUser = Auth.UserID.ToString();

                var result = Submit("BusinessPlan", "UpdateBusinessPlan", false, new object[] { JsonHelper.Serialize(plan), businessPlanTargetList, Auth.DataGroupID });
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new Result(false, ex.Message, null));
            }
        }

        public JsonResult UpdateBusinessPlanStatus(string businessPlanID, string status, DateTime? updateDate)
        {
            try
            {
                var result = Submit("BusinessPlan", "UpdateBusinessPlanStatusByID", false, new object[] { businessPlanID, status, Auth.UserID.ToString(), updateDate, null });
                return Json(result);

            }
            catch (Exception ex)
            {
                return Json(new Result(false, ex.Message, null));
            }
        }

        public JsonResult DeleteBusinessPlanByID(string businessPlanID)
        {
            try
            {
                var result = Submit("BusinessPlan", "DeleteBusinessPlanByID", false, new object[] { businessPlanID, Auth.DataGroupID });
                return Json(result);

            }
            catch (Exception ex)
            {
                return Json(new Result(false, ex.Message, null));
            }
        }
    }
}
