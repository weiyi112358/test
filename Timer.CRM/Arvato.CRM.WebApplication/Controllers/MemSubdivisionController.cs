using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System.Text;
using System.Data;
using Arvato.CRM.EF;

namespace Arvato.CRM.WebApplication.Controllers
{
    public class MemSubdivisionController : Controller
    {
        //
        // GET: /MemSubdivision/

        public ActionResult Index()
        {
            return View();
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
        /// 获取细分类型
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSubdivisionType()
        {
            var result = Submit("MemSubdivision", "GetSubdivisionType", false);
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

        #region 查找会员细分
        /// <summary>
        /// 查找会员细分
        /// </summary>
        /// <param name="para"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public JsonResult GetMemberSubdivisionByKey(string key)
        {
            //object[] args = new object[] { };
            //args[0] = key;
            var result = Submit("MemSubdivision", "GetMemberSubdivisionByKey", true, key);
            return Json(result.Obj[0].ToString());
        }
        #endregion

        #region 根据ID查找会员细分
        /// <summary>
        /// 根据ID查找会员细分
        /// </summary>
        /// <param name="msid"></param>
        /// <returns></returns>
        public JsonResult GetMemberSubdivisionById(Guid msid)
        {
            var result = Submit("MemSubdivision", "GetMemberSubdivisionById", false, msid);
            return Json(result.Obj[0].ToString());
        }
        #endregion

        #region 根据会员细分规则下拉搜索值获取名字
        /// <summary>
        /// 根据会员细分规则下拉搜索值获取名字
        /// </summary>
        /// <param name="seleSearchVals"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSeleSearchItemsText(List<MemSubdRValTypeModel> seleSearchVals)
        {

            return Json(new Result(false, ""));
        }
        #endregion

        #region 编辑会员细分
        /// <summary>
        /// 编辑会员细分
        /// </summary>
        /// <param name="subdStr"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditMemberSubdivision(string subdStr, string importObj)
        {
            subdStr = HttpUtility.HtmlDecode(subdStr);
            var result = Submit("MemSubdivision", "EditOrAddMemberSubdivision", false, subdStr, Auth.UserID.ToString(), importObj);
            return Json(result);
        }
        #endregion

        #region 删除会员细分
        /// <summary>
        /// 删除会员细分
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleMemberSubdivision(Guid sid)
        {
            var result = Submit("MemSubdivision", "DeleMemberSubdivision", false, sid);
            return Json(result);
        }
        #endregion

        #region 激活/取消激活会员细分
        /// <summary>
        /// 激活/取消激活会员细分
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="isEnable"></param>
        /// <param name="modifiedUser"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EnableMemberSubdivision(Guid sid, bool isEnable)
        {
            var result = Submit("MemSubdivision", "EnableMemberSubdivision", false, sid, isEnable, Auth.UserID.ToString());
            return Json(result);
        }
        #endregion

        #region 细分规则相关

        /// <summary>
        /// 获取字典表数据通用方法
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public JsonResult GetCommonOptionData(string type)
        {
            var result = Submit("MemSubdivision", "GetCommonOptionData", false, type);
            return Json(result.Obj[0].ToString());
        }

        #region 获取所有会员细分规则模板
        /// <summary>
        /// 获取所有会员细分规则模板
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMemSubdFilterAll()
        {
            var result = Submit("MemSubdivision", "GetMemSubdFilterAll", false);
            return Json(result.Obj[0].ToString());
        }
        #endregion

        #region 获取会员细分过滤条件所有左值
        /// <summary>
        /// 获取会员细分过滤条件所有左值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMemSubdLeftValuesAll()
        {
            var result = Submit("MemSubdivision", "GetSubdLeftValuesAll", false);
            return Json(new { LeftAliasList = result.Obj[0].ToString(), LeftDynamicParameters = result.Obj[1].ToString(), ParameterOption = result.Obj[2].ToString() });
        }
        #endregion


        #region 根据会员细分规则右值项

        /// <summary>
        /// 根据左值获取右值下拉数据
        /// </summary>
        /// <param name="table"></param>
        /// <param name="dictType"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetRightSelectData(string table, string dictType)
        {
            var result = Submit("MemSubdivision", "GetRightSelectData", false, new object[] { table, dictType, Auth.DataGroupID });
            return Json(result.Obj[0].ToString());
        }

        [HttpPost]
        public JsonResult GetRightSelectDataByLimit(string fieldalias)
        {


            var result = Submit("MemSubdivision", "GetRightSelectDataByLimit", false, new object[] { fieldalias, Auth.DataGroupID, JsonHelper.Serialize(Auth.RoleIDs), Auth.CurPageID });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 根据会员细分过滤条件左值获取右值项
        /// </summary>
        /// <param name="dicRightValConfig"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMemSubdRightValues(string rightValCfgs)
        {
            if (string.IsNullOrEmpty(rightValCfgs))
                return null;
            var result = Submit("MemSubdivision", "GetMemSubdRightValues", false, new object[] { rightValCfgs, Auth.DataGroupID });
            JsonResult jr = new JsonResult();
            if (result.IsPass)
            {
                jr.Data = "{" + result.Obj[0].ToString() + "}";
                //jr.Data = "{" + Utility.JsonHelper.JsonCharFilter(result.Obj[0].ToString()) + "}";
            }
            else
            {
                jr.Data = "{}";
            }
            return Json(jr);
        }

        #endregion

        #endregion

        #region 分页查找会员细分结果
        /// <summary>
        /// 分页查找会员细分结果
        /// </summary>
        /// <param name="currSubdId"></param>
        /// <param name="memCard"></param>
        /// <param name="memName"></param>
        /// <param name="mobile"></param>
        /// <param name="registerStoreCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMemberSubdResultByPage(Guid currSubdId, string memCard, string memName, string mobile, string registerStoreCode, string table, string subDevDataType)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("MemSubdivision", "GetMemberSubdResultByPage", false, new object[] { currSubdId, memCard, memName, mobile, registerStoreCode, table, subDevDataType, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());

        }

        /// <summary>
        /// 根据会员细分获取动态表数据
        /// </summary>
        /// <param name="subdId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSubdDynamicTable(Guid subdId)
        {
            var result = Submit("MemSubdivision", "GetSubdDynamicTable", false, subdId);
            return Json(result);
        }
        #region 会员细分导出——新版
        
        
        /// <summary>
        /// 加载会员细分导出弹窗的下拉框
        /// </summary>
        /// <returns></returns>
        public JsonResult loadSubExportSelect()
        {
            var result = Submit("MemSubdivision", "loadSubExportSelect", false, null);
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 加载会员细分导出弹窗的表格
        /// </summary>
        /// <returns></returns>
        public ActionResult loadSubExportTable(Guid SubdivisionID)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("MemSubdivision", "loadSubExportTable", false, new object[] { SubdivisionID, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 会员细分导出框添加数据
        /// </summary>
        /// <param name="FieldAliasID"></param>
        /// <param name="SubdivisionID"></param>
        /// <returns></returns>
        public JsonResult AddSubExport(int FieldAliasID, Guid SubdivisionID)
        {
            var UserId = Auth.UserID;
            var result = Submit("MemSubdivision", "AddSubExport", false, new object[] { FieldAliasID, SubdivisionID, UserId });
            return Json(result);
        }

        /// <summary>
        /// 会员细分导出框删除数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public JsonResult deleteSubExportCol(string ID)
        {
            var UserId = Auth.UserID;
            var result = Submit("MemSubdivision", "deleteSubExportCol", false, new object[] { ID, UserId });
            return Json(result);
        }

        /// <summary>
        /// 会员细分结果导出
        /// </summary>
        /// <param name="currSubdId"></param>
        /// <param name="memCard"></param>
        /// <param name="memName"></param>
        /// <param name="memMobile"></param>
        /// <param name="registerStoreCode"></param>
        /// <returns></returns>
        [HttpPost]
        public FileResult ExportMemberSubdResult(Guid currSubdId, string memCard, string memName, string memMobile, string registerStoreCode, string dynamicTable, string subDevDataType)
        {
            List<V_U_TM_Mem_Info> list;
            List<ExcelColumnFormat<V_U_TM_Mem_Info>> excelColumnFormats = new List<ExcelColumnFormat<V_U_TM_Mem_Info>>();
            var queryResult = Submit("MemSubdivision", "loadSubExportColumn", false, new object[] { currSubdId });
            var queryList = JsonHelper.Deserialize<List<SubExportColumnModel>>(queryResult.Obj[0].ToString());
            var type = typeof(V_U_TM_Mem_Info);
            foreach (var item in queryList)
            {
                ExcelColumnFormat<V_U_TM_Mem_Info> ef = new ExcelColumnFormat<V_U_TM_Mem_Info> { ColumnName = item.Name, FuncGetValue = p => type.GetProperty(item.Code).GetValue(p) };
                excelColumnFormats.Add(ef);
            }
            try
            {
                var result = Submit("MemSubdivision", "ExportMemberSubdResult", false, new object[] { currSubdId, memCard, memName, memMobile, registerStoreCode, dynamicTable, subDevDataType });
                if (result.IsPass == false)
                {
                    list = new List<V_U_TM_Mem_Info>();
                    var workBook = ExcelHelper.DataToExcel(list, excelColumnFormats);
                    var stream = ExcelHelper.GetStream(workBook);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
                }
                else
                {
                    list = JsonHelper.Deserialize<List<V_U_TM_Mem_Info>>(result.Obj[0].ToString());
                    var workBook = ExcelHelper.DataToExcel(list, excelColumnFormats);
                    var stream = ExcelHelper.GetStream(workBook);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(stream, "application/vnd.ms-excel", "会员细分结果导出" + DateTime.Now.ToShortDateString() + ".xls");
                }
            }
            catch (Exception e)
            {
                list = new List<V_U_TM_Mem_Info>();
                var workBook = ExcelHelper.DataToExcel(list, excelColumnFormats);
                var stream = ExcelHelper.GetStream(workBook);
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                string errmsg = e.GetType().Name;
                if (errmsg == "OutOfMemoryException") errmsg = "导出数据量过大";
                return File(stream, "application/vnd.ms-excel", "会员细分结果导出异常" + DateTime.Now.ToShortDateString() + "_" + errmsg + "_无法导出.xls");
            }
        }
        #endregion

        #region 会员细分导出——旧版
        
        
        ///// <summary>
        ///// 会员细分结果导出
        ///// </summary>
        ///// <param name="currSubdId"></param>
        ///// <param name="memCard"></param>
        ///// <param name="memName"></param>
        ///// <param name="memMobile"></param>
        ///// <param name="registerStoreCode"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public FileResult ExportMemberSubdResult(Guid currSubdId, string memCard, string memName, string memMobile, string registerStoreCode, string dynamicTable, string subDevDataType)
        //{
        //    List<MemSubdivisionResultModel> list;
        //    List<ExcelColumnFormat<MemSubdivisionResultModel>> excelColumnFormats = new List<ExcelColumnFormat<MemSubdivisionResultModel>>{
        //        new ExcelColumnFormat<MemSubdivisionResultModel>{ColumnName="会员ID", FuncGetValue=p=>p.MemberID},
        //       new ExcelColumnFormat<MemSubdivisionResultModel>{ColumnName="会员卡号", FuncGetValue=p=>p.MemberCardNo},
        //       new ExcelColumnFormat<MemSubdivisionResultModel>{ColumnName="会员姓名", FuncGetValue=p=>p.CustomerName},
        //       new ExcelColumnFormat<MemSubdivisionResultModel>{ColumnName="性别", FuncGetValue=p=>p.Gender},
        //       new ExcelColumnFormat<MemSubdivisionResultModel>{ColumnName="手机号", FuncGetValue=p=>p.CustomerMobile},
        //       new ExcelColumnFormat<MemSubdivisionResultModel>{ColumnName="入会时间", FuncGetValue=p=>p.RegisterDate},
        //       new ExcelColumnFormat<MemSubdivisionResultModel>{ColumnName="入会门店", FuncGetValue=p=>p.StoreName},
        //       new ExcelColumnFormat<MemSubdivisionResultModel>{ColumnName="邮箱", FuncGetValue=p=>p.CustomerEmail }
        //    };
        //    try
        //    {
        //        //string roleIdsStr = Util.Serialize(Auth.UserRoleList.Select(r => r.RoleID));
        //        var result = Submit("MemSubdivision", "ExportMemberSubdResult", false, new object[] { currSubdId, memCard, memName, memMobile, registerStoreCode, dynamicTable, subDevDataType });
        //        //return JsonHelper.Deserialize<MemSubdivisionResultModel>(result.Obj[0].ToString());

        //        if (result.IsPass == false)
        //        {
        //            list = new List<MemSubdivisionResultModel>();
        //            var workBook = ExcelHelper.DataToExcel(list, excelColumnFormats);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //        else
        //        {
        //            list = JsonHelper.Deserialize<List<MemSubdivisionResultModel>>(result.Obj[0].ToString());
        //            var workBook = ExcelHelper.DataToExcel(list, excelColumnFormats);
        //            var stream = ExcelHelper.GetStream(workBook);
        //            stream.Seek(0, System.IO.SeekOrigin.Begin);
        //            return File(stream, "application/vnd.ms-excel", "会员细分结果导出" + DateTime.Now.ToShortDateString() + ".xls");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        list = new List<MemSubdivisionResultModel>();
        //        var workBook = ExcelHelper.DataToExcel(list, excelColumnFormats);
        //        var stream = ExcelHelper.GetStream(workBook);
        //        stream.Seek(0, System.IO.SeekOrigin.Begin);
        //        string errmsg = e.GetType().Name;
        //        if (errmsg == "OutOfMemoryException") errmsg = "导出数据量过大";
        //        return File(stream, "application/vnd.ms-excel", "会员细分结果导出异常" + DateTime.Now.ToShortDateString() + "_" + errmsg + "_无法导出.xls");
        //    }
        //}
        #endregion

        /// <summary>
        /// 手动导入会员细分结果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string ImportMemSubExcel()
        {
            try
            {
                var file = Request.Files[0];
                int FileLen = file.ContentLength;
                byte[] input = new byte[FileLen];
                System.IO.Stream MyStream = file.InputStream;
                MyStream.Read(input, 0, FileLen);
                DataTable dt = ExcelHelper.ExcelToDataTable(MyStream);
                List<string> LstImport = new List<string>();
                foreach (DataRow r in dt.Rows)
                {
                    if (r["会员ID"] != null)
                    {
                        if (!string.IsNullOrEmpty(r["会员ID"].ToString()))
                        {
                            LstImport.Add(r["会员ID"].ToString());
                        }
                    }
                }
                if (LstImport != null && LstImport.Count > 0)
                    return JsonHelper.Serialize(LstImport);
                else
                    return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 获取门店数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetStores()
        {
            var result = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
            return Json(result.Obj[0].ToString());
        }
        #endregion

        #region 统计分析图表数据源
        /// <summary>
        /// 获取会员细分统计分析图表数据源
        /// </summary>
        /// <param name="subdId"></param>
        /// <returns></returns>
        public JsonResult GetSubdStatiscicalResult(Guid subdId)
        {
            var result = Submit("MemSubdivision", "GetSubdStatiscicalResult", false, subdId);
            return Json(result);
        }
        #endregion
    }
}
