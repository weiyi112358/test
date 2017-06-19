using Arvato.CRM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using Arvato.CRM.Utility.WorkFlow;
using System.IO;
using System.Drawing.Imaging;

namespace Arvato.CRM.WebApplication.Controllers
{
    public class MarkController : Controller
    {
        //
        // GET: /Mark/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMarketActivities(string modelStr)
        {
            MarketActivityFilterModel model = JsonHelper.Deserialize<MarketActivityFilterModel>(modelStr);
            var dts = Request.CreateDataTableParameter();
            var result = Submit("MarketActivity", "SearchActivity", false,
                new object[] { model.ActivityID, model.ActivityName, model.Enable,model.Status, model.PlanStartTimeFrom, model.PlanStartTimeEnd, model.PlanEndDateFrom, model.PlanEndDateEnd, model.BusinessPlanID, JsonHelper.Serialize(dts), model.DataGroupID});
            return Json(result.Obj[0].ToString());

            ////var model = new MarketActivityFilterModel();
            ////if (TryUpdateModel(model))
            ////{
            //var userId = Auth.UserID;
            //var userGroupId = Auth.DataGroupID;
            //var result = Submit("MarketActivity", "GetActivities", false, new object[] { model.ActivityID, model.ActivityName, model.Enable, model.PlanStartTimeFrom, model.PlanStartTimeEnd, model.PlanEndDateFrom, model.PlanEndDateEnd, userId, userGroupId, model.DataGroupID, model.StoreCode, JsonHelper.Serialize(dts) });
            //return Json(result.Obj[0].ToString());
            ////}
            ////else
            ////    return null;
        }

        public JsonResult UpdateActivityStatus(int activityID, short status)
        {
            try
            {
                var result = Submit("MarketActivity", "UpdateActivityStatusByID", false, new object[] { activityID, status, Auth.UserID.ToString(), null });
                return Json(result);

            }
            catch (Exception ex)
            {
                return Json(new Result(false, ex.Message, null));
            }
        }

        /// <summary>
        /// 查找群组
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDataGroups()
        {
            int? dataGroupId = Auth.DataGroupID;
            var result = Submit("Service", "GetDataGroupsExceptRootByParentId", false, dataGroupId);
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 获取群组下门店
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public JsonResult GetDataGroupStore(int groupId)
        {
            int userId = Auth.UserID;
            var result = Submit("Service", "GetAuthStoreByGroupUserID", false, groupId, userId);
            return Json(result.Obj[0].ToString());
        }

        public ActionResult Edit(int activityId = 0)
        {
            var res = Submit("MarketActivity", "GetMemberSubdivisionByActivityId", false, activityId);
            if (res.Obj == null) ViewBag.Subdivision = null;
            else ViewBag.Subdivision = res.Obj[0];//.ToString();

            var resTemplates = Submit("MarketActivity", "GetGroupedTemplates", true);
            ViewBag.GroupedTemplates = resTemplates.Obj[0];

            var dataLimitTypes = Submit("Service", "GetDataLimitTypes", false, new object[] { true });
            ViewBag.DataLimitTypes = dataLimitTypes.Obj[0];

            ActMaster ent;
            if (activityId == 0)
            {
                ent = new ActMaster
                {
                    //BrandID = WebSite.Default.BrandID,
                    PlanStartDate = DateTime.Now,
                    Workflow = "",
                    Schedule = "",
                    Kpi = "",
                    ReferenceNo = "",
                    KPIResultList = null,
                    KPITargetList = null
                };
            }
            else
            {
                ent = new ActMaster();
                var resSubd = Submit("MarketActivity", "GetMemberSubdivisionList", false, new object[] { activityId });
                ViewBag.Subdivisions = resSubd.Obj[0];
                var resIns = Submit("MarketActivity", "GetActivityInstanceList", false, new object[] { activityId });
                ViewBag.Instances = resIns.Obj[0];
                var resAct = Submit("MarketActivity", "GetActivity", false, new object[] { activityId });
                ent = Util.Deserialize<ActMaster>(resAct.Obj[0].ToString());
                var resultList = Submit("MarketActivity", "GetActivityKPIResult", false, new object[] { activityId, null });
                ent.KPIResultList = resultList.IsPass ? Util.Deserialize<List<KPIResultModel>>(resultList.Obj[0].ToString()) : null;
                var targetList = Submit("MarketActivity", "GetAcitivityKPITarget", false, new object[] { activityId, null });
                ent.KPITargetList = targetList.IsPass ? Util.Deserialize<List<KPITargetModel>>(targetList.Obj[0].ToString()) : null;
            }
            return View(ent);
        }

        public JsonResult SearchActivityKPIResultForPage(string activityID)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("KPIResult", "GetKPIResultForPage", false, new object[] { null, null, "4", activityID, null, null, JsonHelper.Serialize(pageParamers), Auth.DataGroupID });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 添加或编辑活动信息
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(string subdivisionIds, int activityId = 0, bool hasSids = false)
        {
            var ids = new List<Guid>();
            if (!string.IsNullOrWhiteSpace(subdivisionIds))
            {
                ids = subdivisionIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(p => Guid.Parse(p)).ToList();
            }
            ActMaster ent;
            bool Enable = false;
            if (activityId == 0)
            {
                ent = new ActMaster
                {
                    //BrandID = WebSite.Default.BrandID,
                    PlanStartDate = DateTime.Now,
                    Workflow = "",
                    Schedule = "",
                    Enable = Enable,
                    AddedDate = DateTime.Now,
                    AddedUser = Auth.Username,
                    ModifiedDate = DateTime.Now,
                    ModifiedUser = Auth.Username
                };
            }
            else
            {
                var res = Submit("MarketActivity", "GetActivity", false, new object[] { activityId });
                ent = JsonHelper.Deserialize<ActMaster>(res.Obj[0].ToString());
                ent.ModifiedDate = DateTime.Now;
                ent.ModifiedUser = Auth.Username;
                Enable = ent.Enable;
            }
            if (TryUpdateModel(ent))
            {
                //ent.Workflow = Request.Form["Workflow"];
                //ent.Schedule = Request.Form["Schedule"];
                if (Enable && !ent.Enable && ent.Status == 2)
                {
                    ent.Status = 0;
                }
                var act = Util.Deserialize<Activity>(ent.Workflow);
                if (act != null)
                {
                    ent.WfRootId = act.Id;
                }
                var res = Submit("MarketActivity", "SaveActivity", false, JsonHelper.Serialize(ent), JsonHelper.Serialize(ids), hasSids, ent.Kpi);
                return Json(res);
            }
            else
                return null;

            //return DefaultJson();
        }

        /// <summary>
        /// 删除活动信息
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DelActivity(int activityId)
        {
            var res = Submit("MarketActivity", "GetActivity", false, new object[] { activityId });
            return Json(res.Obj[0].ToString());
        }
        /// <summary>
        /// 生成流程图
        /// </summary>
        /// <param name="activityJson"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPic(string activityJson)
        {
            var activity = Util.Deserialize<Activity>(activityJson);
            var bitmap = WorkFlowHelper.CreateImage(activity);
            var memory = new MemoryStream();
            bitmap.Save(memory, ImageFormat.Png);
            var buf = memory.GetBuffer();
            return Json(Convert.ToBase64String(buf));
            //return File(memory, "application/octet-stream");
            //var filename = string.Format("/ActivityPics/{0}.jpg", Guid.NewGuid());
            //bitmap.Save(Util.MapPath("~" + filename), ImageFormat.Png);
            //return Json(new Result(true, "", new { file = filename, width = bitmap.Width, height = bitmap.Height }));
        }

        /// <summary>
        /// 查看市场活动会员细分
        /// </summary>
        /// <param name="Instance"></param>
        /// <param name="SubdivisionId"></param>
        /// <param name="Name"></param>
        /// <param name="CardNo"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetActivitySubdivision(long Instance, Guid? SubdivisionId, string Name, string CardNo)
        {
            var dts = Request.CreateDataTableParameter();
            var res = Submit("MarketActivity", "GetActivitySubdivisionList", false, new object[] { Instance, CardNo, Name, SubdivisionId, JsonHelper.Serialize(dts) });
            return Json(res.Obj[0].ToString());
        }
        /// <summary>
        /// 按门店获取会员细分
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public ActionResult GetMemberSubdivisionListByStore(string store)
        {
            var res = Submit("MarketActivity", "GetMemberSubdivisionListByStore", false, new object[] { store });
            return Json(res.Obj[0].ToString());
        }

        /// <summary>
        /// 导出市场活动会员细分
        /// </summary>
        /// <param name="Instance"></param>
        /// <param name="SubdivisionId"></param>
        /// <param name="Name"></param>
        /// <param name="CardNo"></param>
        /// <returns></returns>
        [HttpPost]
        public FileResult ActivitySubdivisionExport(long Instance, Guid? SubdivisionId, string Name, string CardNo)
        {
            var res = Submit("MarketActivity", "GetActivitySubdivisionListNoPage", false, new object[] { Instance, CardNo, Name, SubdivisionId });

            var colFormats = new List<ExcelColumnFormat<MarketActivitySubdivisionResult>> { 
                    new ExcelColumnFormat<MarketActivitySubdivisionResult>{ColumnName="会员卡号", FuncGetValue=p=>p.CardNo},
                    new ExcelColumnFormat<MarketActivitySubdivisionResult>{ColumnName="会员姓名", FuncGetValue=p=>p.Name},
                    new ExcelColumnFormat<MarketActivitySubdivisionResult>{ColumnName="手机", FuncGetValue=p=>p.Mobile},
                    new ExcelColumnFormat<MarketActivitySubdivisionResult>{ColumnName="邮箱", FuncGetValue=p=>p.Email},
                    new ExcelColumnFormat<MarketActivitySubdivisionResult>{ColumnName="所属会员细分", FuncGetValue=p=>p.SubdivisionName},
                    new ExcelColumnFormat<MarketActivitySubdivisionResult>{ColumnName="跟踪结果", FuncGetValue=p=>p.WfRootId},
                    new ExcelColumnFormat<MarketActivitySubdivisionResult>{ColumnName="关联问卷", FuncGetValue=p=>p.Templates}
                };
            //var res = Util.Deserialize<Result>(lstStr);
            List<MarketActivitySubdivisionResult> lst =
                res.IsPass ?
                 Util.Deserialize<List<MarketActivitySubdivisionResult>>(res.Obj[0].ToString()) :
                new List<MarketActivitySubdivisionResult>();

            var workBook = ExcelHelper.DataToExcel(lst, colFormats);
            var stream = ExcelHelper.GetStream(workBook);
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            return File(stream, "application/vnd.ms-excel", "市场活动跟踪结果导出_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
        }

        /// <summary>
        /// 获取活动关联细分的人数
        /// </summary>
        /// <param name="subIds"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSubdivisionMemCount(string subIds)
        {
            var res = Submit("MarketActivity", "GetSubdivisionMemCount", false, new object[] { subIds });
            return Json(res.Obj[0]);
        }

        /// <summary>
        /// 删除活动信息
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int activityId)
        {
            var res = Submit("MarketActivity", "DelActivity", false, new object[] { activityId });
            return Json(res);
        }

        public JsonResult GetKPIData()
        {
            DatatablesParameter para = new DatatablesParameter();
            para.iDisplayLength = 0;
            para.iDisplayStart = 0;
            try
            {
                var result = Submit("KPI", "GetKPI", false, new object[] { null, null, "4", null, Auth.DataGroupID });
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

        public JsonResult SearchActivityKPIResult(string activityID)
        {
            var result = Submit("KPIResult", "GetKPIResult", false, new object[] { null, null, "4", activityID, null, null, false, Auth.DataGroupID });
            return Json(result.Obj[0].ToString());
        }

        public JsonResult SearchActivityKPITarget(string activityID)
        {
            var result = Submit("KPITarget", "GetKPITarget", false, new object[] { null, "4", activityID, Auth.DataGroupID });
            return Json(result.Obj[0].ToString());
        }
        public JsonResult CheckRefExist(string refNO)
        {
            var result = Submit("MarketActivity", "CheckRefExist", false, new object[] { refNO });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
