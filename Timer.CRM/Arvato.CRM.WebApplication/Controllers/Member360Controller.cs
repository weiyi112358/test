using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPOI.HSSF.UserModel;
using System.IO;
using Arvato.CRM.EF;
using System.Data;

namespace Arvato.CRM.WebApplication.Controllers
{
    public class Member360Controller : Controller
    {
        #region 门店快速查询会员

        public ActionResult MemberForStore()
        {
            return View();
        }

        public ActionResult MemberDetailForStore(string mid)
        {
            ViewBag.MemberId = mid;
            var brief = Submit("Member360", "GetMemberBasicInfo", false, new object[] { mid });
            ViewBag.BriefInfo = brief.Obj[0];

            return View();
        }

        #endregion

        #region 会员360列表
        /// <summary>
        /// 获取会员明细
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public ActionResult MemberDetail(string mid)
        {
            try
            {
                Log4netHelper.WriteInfoLog("aaaa:");

                ViewBag.MemberId = mid;

                Log4netHelper.WriteInfoLog("mid:" +mid);

                var brief = Submit("Member360", "GetMemberBasicInfo", false, new object[] { mid });
                ViewBag.BriefInfo = brief.Obj[0];
                Log4netHelper.WriteInfoLog("brief:" + JsonHelper.Serialize(brief));
                //            dynamic info = brief.Obj[0];
                //            int k = 0;
                //            if (info != null)
                //            {
                //                foreach (var kid in info.Kids)
                //                {
                //                    k++;
                //                }
                //            }
                //            JObject baby = JObject.Parse(@"{
                //                MemberSubExtID : 0,
                //                BabyName : '',
                //                BabyGender : '',
                //                BabyBrithday : '',
                //                BabyHeight : '',
                //                BabyWeight : ''
                //            }");
                //            if (k == 0)
                //            {
                //                ViewBag.Kids1 = baby;
                //                ViewBag.Kids2 = baby;
                //            }
                //            else if (k == 1)
                //            {
                //                ViewBag.Kids1 = ViewBag.BriefInfo.Kids[0];
                //                ViewBag.Kids2 = baby;
                //            }
                //            else
                //            {
                //                ViewBag.Kids1 = ViewBag.BriefInfo.Kids[0];
                //                ViewBag.Kids2 = ViewBag.BriefInfo.Kids[1];
                //            }

                var chan = Submit("Service", "GetStoreChannels", false);
                ViewBag.Channels = chan.Obj[0];
                Log4netHelper.WriteInfoLog("brief:" + JsonHelper.Serialize(chan));
                var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
                ViewBag.Stores = stores.Obj[0];
                //Log4netHelper.WriteInfoLog("brief:" + JsonHelper.Serialize(stores));
            }

            catch (Exception ex)
            {
                Log4netHelper.WriteInfoLog("360log:" + ex.ToString());
            }
            return View();
            

        }
        /// <summary>
        /// 保存会员明细
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="name"></param>
        /// <param name="nickname"></param>
        /// <param name="mobile"></param>
        /// <param name="gender"></param>
        /// <param name="email"></param>
        /// <param name="certno"></param>
        /// <param name="birthday"></param>
        /// <param name="tel"></param>
        /// <param name="city"></param>
        /// <param name="dist"></param>
        /// <param name="addr"></param>
        /// <param name="b1id"></param>
        /// <param name="b1name"></param>
        /// <param name="b1gender"></param>
        /// <param name="b1birth"></param>
        /// <param name="b1height"></param>
        /// <param name="b1weight"></param>
        /// <param name="b2id"></param>
        /// <param name="b2name"></param>
        /// <param name="b2gender"></param>
        /// <param name="b2birth"></param>
        /// <param name="b2height"></param>
        /// <param name="b2weight"></param>
        /// <returns></returns>
        //public JsonResult SaveMemberDetail(string mid, string name, string nickname, string mobile, string gender, string email, string certno, DateTime? birthday, string tel, string provid, string cityid, string distid, string addr, string zip, int b1id, string b1name, string b1gender, DateTime? b1birth, string b1height, string b1weight, int b2id, string b2name, string b2gender, DateTime? b2birth, string b2height, string b2weight)
        //{
        //    var userId = Auth.UserID.ToString();
        //    var rst = Submit("Member360", "SaveMemberDetail", false, new object[] { mid, name, nickname, mobile, gender, email, certno, birthday, tel, provid, cityid, distid, addr, zip, b1id, b1name, b1gender, b1birth, b1height, b1weight, b2id, b2name, b2gender, b2birth, b2height, b2weight, userId });
        //    return Json(rst);
        //}
        public JsonResult SaveMemberDetail(string meminfo)
        {
            var userId = Auth.UserID.ToString();
            var rst = Submit("Member360", "SaveMemberDetail", false, new object[] { meminfo, userId });
            return Json(rst);
        }
        /// <summary>
        /// 获取订单明细
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public JsonResult GetOrderDetail(long oid)
        {
            var dts = Request.CreateDataTableParameter();
            int dataGroupId = (int)Auth.DataGroupID;
            dts.iDisplayLength = 10000;
            var rst = Submit("Member360", "GetTradeHistoryDetail", false, new object[] { JsonHelper.Serialize(dts), oid });
            return Json(rst.Obj[0].ToString());

        }

        /// <summary>
        /// 获取订单支付信息
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public JsonResult GetOrderPayment(long oid)
        {
            var dts = Request.CreateDataTableParameter();
            int dataGroupId = (int)Auth.DataGroupID;
            dts.iDisplayLength = 10000;
            var rst = Submit("Member360", "GetTradeHistoryPayment", false, new object[] { JsonHelper.Serialize(dts), oid });
            return Json(rst.Obj[0].ToString());

        }

        public JsonResult GetOrderData(string mid, DateTime? start, DateTime? end, string chan, decimal? amount_start, decimal? amount_end, string ordercode, string storecode)
        {
            var dts = Request.CreateDataTableParameter();
            int dataGroupId = (int)Auth.DataGroupID;
            string pageIds = Auth.CurPageID.HasValue ? Auth.CurPageID.ToString() : "";
            string dataRoleIds = "(" + string.Join(",", Auth.RoleIDs) + ")";
            var rst = Submit("Member360", "GetTradeHistoryData", false, new object[] { JsonHelper.Serialize(dts), dataGroupId, mid, start, end, chan, amount_start, amount_end, ordercode, storecode });
            return Json(rst.Obj[0].ToString());

        }


        /// <summary>
        /// 会员360视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //var area = Submit("Service", "GetStoreArea", false);
            //ViewBag.Areas = area.Obj[0];
            //var chan = Submit("Service", "GetStoreChannels", false);
            //ViewBag.Channels = chan.Obj[0];
            var customerLevels = Submit("Member360", "GetBizOptionsMemberLevel", false, new object[] { "CustomerLevel", Auth.DataGroupID, true });
            ViewBag.CustomerLevels = customerLevels.Obj[0];
            //var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
            //ViewBag.Stores = stores.Obj[0];
            //var source = Submit("Service", "GetBizOptionsByType", false, new object[] { "CustomerSource", true });
            //ViewBag.source = source.Obj[0];

            return View();
        }


        //public JsonResult GetMembersByPage(string memberNo, string customerName, string customerMobile,
        //    string customerLevel, string registerStoreCode, string regStoreArea, string regStoreChan, string RegStartDate, string RegEndDate, decimal? consumeAmountStart, decimal? consumeAmountEnd, decimal? consumePointStart, decimal? consumePointEnd, string customerSource)
        //{
        //    Member360SearchModel search = new Member360SearchModel();
        //    search.DataGroupId = Auth.DataGroupID.Value;
        //    search.PageIds = Auth.CurPageID.HasValue ? Auth.CurPageID.ToString() : "";
        //    search.DataRoleIds = "(" + string.Join(",", Auth.RoleIDs) + ")";
        //    search.MemberNo = memberNo;
        //    search.CustomerName = customerName;
        //    search.CustomerMobile = customerMobile;
        //    search.CustomerLevel = customerLevel;
        //    search.RegisterStoreCode = registerStoreCode;
        //    search.RegStoreArea = regStoreArea;
        //    search.RegStoreChan = regStoreChan;
        //    search.ConsumeAmountStart = consumeAmountStart;
        //    search.ConsumeAmountEnd = consumeAmountEnd;
        //    search.ConsumePointStart = consumePointStart;
        //    search.ConsumePointEnd = consumePointEnd;
        //    search.RegisterStoreCode = registerStoreCode;
        //    search.CustomerSource = customerSource;
        //    search.RegisterDateStart = RegStartDate;
        //    search.RegisterDateEnd = RegEndDate;

        //    var dts = Request.CreateDataTableParameter();
        //    var rst = Submit("Member360", "GetMembersByPage", false, new object[] { JsonHelper.Serialize(dts), search });
        //    return Json(rst.Obj[0].ToString());
        //}
        public JsonResult GetMembersByPage(string memberNo, string customerName, string customerMobile)
        {

            var dts = Request.CreateDataTableParameter();
            var a = Auth.CurPageID;
            var sc = Auth.StoreCode;
            var ld = Auth.LimitData;

            var rds = Auth.RoleIDs;
            var rst = Submit("Member360", "DtToDatatable", false, new object[] { a, JsonHelper.Serialize(rds), "TM_Mem_Ext", a, "0", JsonHelper.Serialize(dts), memberNo, customerName, customerMobile });
            return Json(rst.Obj[0]);
        }

        public JsonResult GetMembersByPageColumn(string memberNo, string customerName, string customerMobile)
        {
            var a = Auth.CurPageID;
            var rst = Submit("Member360", "DtToColumn", false, new object[] { "TM_Mem_Ext", a, "0", memberNo, customerName, customerMobile });
            return Json(rst.Obj[0]);
        }

        public JsonResult GetChildMembersByPage(string mid)
        {
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("Member360", "GetChildMembersByPage", false, new object[] { JsonHelper.Serialize(dts), mid });
            return Json(rst.Obj[0].ToString());
        }


        public JsonResult GetVehicleByPageColumn()//string mid
        {
            var a = Auth.CurPageID;
            var rst = Submit("Member360", "DtToColumn", false, new object[] { "TM_Mem_SubExt", a, "2" });
            return Json(rst.Obj[0]);
        }
        public JsonResult GetVehicleByPage(string mid)
        {

            var dts = Request.CreateDataTableParameter();
            var a = Auth.CurPageID;
            var sc = Auth.StoreCode;
            var ld = Auth.LimitData;
            var rst = Submit("Member360", "DtToDatatableNew", false, new object[] { a, "", "TM_Mem_SubExt", "vehicle", "", a, "2", JsonHelper.Serialize(dts), mid });
            return Json(rst.Obj[0]);
        }

        public JsonResult GetColumnByPage(string tablename, string blockcode)//string mid
        {
            //List<CommonSearchDetail> detail = 
            var a = Auth.CurPageID;
            var rst = Submit("Member360", "DtToColumn", false, new object[] { tablename, a, blockcode });
            return Json(rst.Obj[0]);
        }

        

        public JsonResult GetTabInfo(string tablename, string aliaskey, string aliassubkey, string blockcode, string parm)
        {

            var dts = Request.CreateDataTableParameter();
            var a = Convert.ToInt32(Auth.CurPageID);
            var sc = Auth.StoreCode;
            var ld = Auth.LimitData;
            var rds = Auth.RoleIDs;
            var rst = Submit("Member360", "DtToDatatableNew", false, new object[] { a, JsonHelper.Serialize(rds), tablename, aliaskey, aliassubkey, a, blockcode, JsonHelper.Serialize(dts), parm });
            return Json(rst.Obj == null ? null : rst.Obj[0]);
        }

        public JsonResult SaveMemberSMS(SMSSendModel model)
        {
            var userid = Auth.UserID.ToString();
            model.SMSInfo = HttpUtility.HtmlDecode(model.SMSInfo);
            var rst = Submit("Member360", "SaveMemberSMS", false, new object[] { model, userid });
            return Json(rst);
        }
        #endregion

        #region 会员360详情
        public ActionResult Detail(string mid)
        {
            ViewBag.MemberId = mid;
            //初始化基本信息页面下拉列表
            var customerStatus = Submit("Service", "GetBizOptionsByType", false, new object[] { "CustomerStatus", true });
            ViewBag.CustomerStatus = customerStatus.Obj[0];
            var customerLevels = Submit("Service", "GetBizOptionsByType", false, new object[] { "CustomerLevel", true });
            ViewBag.CustomerLevels = customerLevels.Obj[0];
            //var customerTypes = Submit("", "", false, new object[] { });
            //ViewBag.CustomerTypes = customerTypes.Obj[0];
            //var certificateTypes = Submit("", "", false, new object[] { });
            //ViewBag.CertificateTypes = certificateTypes.Obj[0];

            //初始化交易历史页面下拉列表
            var stores = Submit("Service", "GetAllStoresByDataGroupId", false, new object[] { Auth.DataGroupID });
            ViewBag.Stores = stores.Obj[0];

            var brief = Submit("Member360", "GetMemberBasicInfo", false, new object[] { mid });
            ViewBag.BriefInfo = brief.Obj[0];

            var cardList = Submit("Member360", "GetMemberCardList", false, new object[] { mid });
            ViewBag.CardList = cardList.Obj[0];

            var accountList = Submit("Member360", "GetMemAccountInfo", false, new object[] { mid });
            ViewBag.AccountList = accountList.Obj[0];
            return View();
        }

        //检验手机唯一性
        public JsonResult CheckMobileExist(string mid, string mobile)
        {
            var info = Submit("Member360", "CheckMobileExist", false, new object[] { mid, mobile });
            return Json(info);
        }

        public JsonResult CheckEmailExist(string mid, string email)
        {
            var info = Submit("Member360", "CheckEmailExist", false, new object[] { mid, email });
            return Json(info);
        }

        //获取会员统计图数据
        public JsonResult GetMemChartData(string mid)
        {
            var info = Submit("Member360", "GetMemChartData", false, new object[] { mid });
            return Json(info);
        }

        public ActionResult GetMemLevelHistory(string mid)
        {
            var dts = Request.CreateDataTableParameter();
            var info = Submit("Member360", "GetMemLevelHistory", false, new object[] { JsonHelper.Serialize(dts), mid });
            return Json(info.Obj[0]);
        }
        public ActionResult GetMemInfoChangeHistory(string mid, DateTime? start, DateTime? end)
        {
            //ToDo 获取会员的信息变更历史
            return Json("");
        }

        public JsonResult GetMemContacts(string mid)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("Member360", "GetMemContacts", false, new object[] { JsonHelper.Serialize(dts), mid });
            return Json(result.Obj[0]);
        }
        public JsonResult GetMemContactDetail(long cid)
        {
            var info = Submit("Member360", "GetMemContactDetail", false, new object[] { cid });
            return Json(info);
        }

        /// <summary>
        /// 获取会员车辆信息列表
        /// </summary>
        /// <param name="mid">会员ID</param>
        /// <returns></returns>
        public JsonResult GetMemberCarInfo(string mid)
        {
            var dts = Request.CreateDataTableParameter();
            var info = Submit("Member360", "GetMemberCarInfosByPage", false, new object[] { JsonHelper.Serialize(dts), mid });
            return Json(info.Obj[0]);
        }
        /// <summary>
        /// 根据ID查询车辆信息
        /// </summary>
        /// <param name="id">车辆记录ID</param>
        /// <returns></returns>
        public JsonResult GetMemberCarInfoById(long id)
        {
            var info = Submit("Member360", "GetMemberCarInfoById", false, new object[] { id });
            return Json(info);
        }

        public JsonResult GetMemPackages(string mid, string name, DateTime? start, DateTime? end, bool valid)
        {
            var dts = Request.CreateDataTableParameter();
            var info = Submit("Member360", "GetMemPackages", false, new object[] { JsonHelper.Serialize(dts), mid, name, start, end, valid });
            return Json(info.Obj[0]);
        }

        public JsonResult GetMemCoupon(string mem_id, string start, string end, string couponcode, string coupontype, string isvalid, string isused, string couponname)
        {
            var dts = Request.CreateDataTableParameter();
            var info = Submit("Member360", "GetMemCoupon", false, new object[] { JsonHelper.Serialize(dts), mem_id, start, end, couponcode, coupontype, isvalid, isused, couponname });
            return Json(info.Obj[0]);
        }
        public JsonResult GetPackageDetailsById(long piid)
        {
            var dts = Request.CreateDataTableParameter();
            var info = Submit("Member360", "GetPackageDetailsById", false, new object[] { JsonHelper.Serialize(dts), piid });
            return Json(info.Obj[0]);
        }
        public JsonResult GetPackageLimitsById(long piid)
        {
            var dts = Request.CreateDataTableParameter();
            var info = Submit("Member360", "GetPackageLimitsById", false, new object[] { JsonHelper.Serialize(dts), piid });
            return Json(info.Obj[0]);
        }
        /// <summary>
        /// 获取套餐明细使用历史
        /// </summary>
        /// <param name="memId"></param>
        /// <param name="piid"></param>
        /// <returns></returns>
        public JsonResult GetMemPackageHistory(string memId, long piid)
        {
            var dts = Request.CreateDataTableParameter();
            var info = Submit("Member360", "GetMemPackageHistory", false, new object[] { JsonHelper.Serialize(dts), memId, piid });
            return Json(info.Obj[0]);
        }

        public JsonResult GetMemAccountInfo(string mid)
        {
            var info = Submit("Member360", "GetMemAccountInfo", false, new object[] { mid });
            return Json(info.Obj[0]);
        }

        public JsonResult GetMemAccountDetails(string mid, string accType)
        {
            var info = Submit("Member360", "GetMemAccountDetails", false, new object[] { mid, accType });
            return Json(info.Obj[0]);
        }

        public JsonResult GetMemAccountChangeHistory(string mid, string accChgType, DateTime? start, DateTime? end, string refid, decimal? chgnumStart, decimal? chgnumEnd)
        {
            var dts = Request.CreateDataTableParameter();
            var info = Submit("Member360", "GetMemAccountChangeHistory", false, new object[] { JsonHelper.Serialize(dts), mid, accChgType, start, end, refid, chgnumStart, chgnumEnd });
            return Json(info.Obj[0]);
        }

        public JsonResult GetMemAccountDetailChangeHistory(long detailId)
        {
            var dts = Request.CreateDataTableParameter();
            var info = Submit("Member360", "GetMemAccountDetailChangeHistory", false, new object[] { JsonHelper.Serialize(dts), detailId });
            return Json(info.Obj[0]);
        }

        public JsonResult GetMemAccountCoupon(string mid)
        {
            var info = Submit("Member360", "GetMemAccountCoupon", false, new object[] { mid });
            return Json(info);
        }
        public JsonResult GetMemAccountExchange(string mid)
        {
            var info = Submit("Member360", "GetMemAccountExchange", false, new object[] { mid });
            return Json(info.Obj[0]);
        }

        public JsonResult GetMemAccountCouponList(string mid)
        {
            var dts = Request.CreateDataTableParameter();
            var info = Submit("Member360", "GetMemAccountCouponList1", false, new object[] { JsonHelper.Serialize(dts), mid });
            return Json(info.Obj[0]);
        }

        //获取订单历史数据
        public JsonResult GetTradeHistoryData(string tabName, string mid, string tradeCode, DateTime? start, DateTime? end, string store)
        {
            var dts = Request.CreateDataTableParameter();
            var info = Submit("Member360", "GetTradeHistoryData", false, new object[] { JsonHelper.Serialize(dts), tabName, mid, tradeCode, start, end, store });
            return Json(info.Obj[0]);
        }
        public JsonResult GetTradeHistoryDetail(string tabName, long tid)
        {
            var dts = Request.CreateDataTableParameter();
            var info = Submit("Member360", "GetTradeHistoryDetail", false, new object[] { JsonHelper.Serialize(dts), tabName, tid });
            return Json(info.Obj[0]);
        }
        public JsonResult GetTradeRightsDetail(long tradeId)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("Member360", "GetTradeRightsDetail", false, new object[] { JsonHelper.Serialize(dts), tradeId });
            return Json(result.Obj[0]);
        }
        public JsonResult GetMemberCommunicateHistory(string mid, DateTime? start, DateTime? end)
        {
            var dts = Request.CreateDataTableParameter();
            var info = Submit("Member360", "GetMemberCommunicateHistory", false, new object[] { JsonHelper.Serialize(dts), mid, start, end });
            return Json(info.Obj[0]);
        }

        public JsonResult GetMemberActivity(string mid, string aid)
        {
            var dts = Request.CreateDataTableParameter();
            var info = Submit("Member360", "GetMemberActivity", false, new object[] { JsonHelper.Serialize(dts), mid, aid });
            return Json(info.Obj[0]);
        }

        public JsonResult GetMemberCardChangeHistory(string cardNo)
        {
            var dts = Request.CreateDataTableParameter();
            var info = Submit("Member360", "GetMemberCardChangeHistory", false, new object[] { JsonHelper.Serialize(dts), cardNo });
            return Json(info.Obj[0]);
        }

        public JsonResult GetActDetail(string actid)
        {
            var info = Submit("Member360", "GetActDetail", false, new object[] { actid });
            return Json(info.Obj[0]);
        }

        #endregion

        #region 自动生成页面
        public JsonResult GetTagandMemInfo(string memid)
        {
            var info = Submit("Member360", "GetTagandMemInfo", false, new object[] { memid, Auth.CurPageID });
            var taginfo = info.Obj[0];
            var meminfo = info.Obj[1];
            var tabinfo = info.Obj[2];
            //List<CommonSearchDetail> serlist = new List<CommonSearchDetail>();
            //serlist.Add(new CommonSearchDetail()
            //{
            //    FieldAlias = "StoreCode",
            //    Value = "S030102",
            //    TableName = "V_M_TM_SYS_BaseData_store",
            //    Condition = ">",
            //});
            //serlist.Add(new CommonSearchDetail()
            //{
            //    FieldAlias = "StoreCode",
            //    Value = "S030102",
            //    TableName = "V_M_TM_SYS_BaseData_store",
            //    Condition = "=",
            //});
            //List<ReturnField> retfield = new List<ReturnField>();
            //retfield.Add(new ReturnField()
            //{
            //    FieldAlias = "Name",
            //    TableName = "TM_Mem_Ext",
            //    IsSource = "1",
            //});
            //retfield.Add(new ReturnField()
            //{
            //    FieldAlias = "Gender",
            //    TableName = "TM_Mem_Ext",
            //    IsSource = "1",
            //});

            //var m = JsonHelper.Serialize(new CommonSearch() { TableName = "", s = serlist, f = retfield });
            var temp = new
            {
                taginfo,
                meminfo,
                tabinfo,
            };
            return Json(temp);
        }
        /// <summary>
        /// 获取Tab中表格数据
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="jsonstr"></param>
        /// <returns></returns>
        public JsonResult GetTabDataInfo(string mid, string jsonstr)
        {

            var dts = Request.CreateDataTableParameter();
            var rst = Submit("Member360", "Member360AutoSearch", false, new object[] { mid, jsonstr, Auth.DataGroupID, JsonHelper.Serialize(dts) });
            return Json(rst.Obj[0].ToString());
        }
        public JsonResult GetBasicInfo(string mid)
        {
            var info = Submit("Member360", "GetMemberBasicInfo", false, new object[] { mid });
            return Json(info);
        }
        #endregion

        #region 账户调整
        public ActionResult AccountAdjust()
        {
            //账户类型
            string optType = "AccountType";
            var result = Submit("Service", "GetOptionDataList", false, new object[] { optType });
            var actList = JsonHelper.Deserialize<List<BizOption>>(result.Obj[0].ToString());
            ViewBag.ActTypeList = actList;
            ViewBag.DataGroupId = Auth.DataGroupID;
            return View();
        }
        /// <summary>
        /// 根据id获取会员基本信息
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public JsonResult GetMemberInfoByMid(string mid)
        {
            var info = Submit("Member360", "GetMemberBasicInfo", false, new object[] { mid });
            return Json(info);
        }
        /// <summary>
        /// 获取会员姓名列表
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="memName"></param>
        /// <param name="memMobile"></param>
        /// <param name="vehicleNo"></param>
        /// <param name="vehicleVIN"></param>
        /// <returns></returns>
        public JsonResult GetMembersNameByPage(string memNo, string memName, string memMobile)
        {
            var dts = Request.CreateDataTableParameter();
            memName = HttpUtility.HtmlDecode(memName);
            memMobile = HttpUtility.HtmlDecode(memMobile);
            if (!string.IsNullOrEmpty(memNo))
                memNo = HttpUtility.HtmlDecode(memNo).ToUpper();
            var rds = Auth.RoleIDs;
            var pid = Auth.CurPageID;
            var rst = Submit("Member360", "GetMemberNameByPage", false, new object[] { JsonHelper.Serialize(dts), memNo, memName, memMobile, JsonHelper.Serialize(rds), pid });
            return Json(rst.Obj[0].ToString());
        }
        /// <summary>
        /// 调整账户信息
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="detailType"></param>
        /// <param name="oper"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public JsonResult SaveImproveActInfo(string mid, string detailType, string oper, string num, string did, string actId, string actType)
        {
            decimal changeValue = Convert.ToDecimal(num);
            var rst = Submit("Member360", "SaveImproveActInfo", false, new object[] { mid, detailType, oper, changeValue, did, actId, actType, Auth.UserID });
            return Json(rst);
        }
        /// <summary>
        /// 根据id获取账户详细信息
        /// </summary>
        /// <param name="detailId"></param>
        /// <returns></returns>
        public JsonResult GetActDetailById(string detailId)
        {
            var info = Submit("Member360", "GetActDetailById", false, new object[] { detailId });
            return Json(info.Obj[0].ToString());
        }

        /// <summary>
        /// 根据Id更新帐户明细信息
        /// </summary>
        /// <param name="did"></param>
        /// <param name="detailType"></param>
        /// <param name="num"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public JsonResult SaveEditActInfoById(string did, string detailType, string num, DateTime? startDate, DateTime? endDate, string oper, string reason)
        {
            decimal changeValue = Convert.ToDecimal(num);
            var userId = Auth.UserID;
            reason = HttpUtility.HtmlDecode(reason);
            var rst = Submit("Member360", "SaveEditActInfoById", false, new object[] { did, detailType, changeValue, startDate, endDate, userId, oper, reason, Auth.DataGroupID });
            return Json(rst);
        }
        /// <summary>
        /// 新增账户信息
        /// </summary>
        /// <param name="detailType"></param>
        /// <param name="num"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public JsonResult SaveAddActDetailInfo(string actId, string detailType, string num, DateTime? startDate, DateTime? endDate, string actType, string mid)
        {
            decimal changeValue = Convert.ToDecimal(num);
            var userId = Auth.UserID;
            var rst = Submit("Member360", "SaveAddActDetailInfo1", false, new object[] { actId, detailType, changeValue, startDate, endDate, userId, actType, mid, Auth.DataGroupID });
            return Json(rst);
        }
        /// <summary>
        /// 获取账户使用限制列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetActLimitList()
        {
            string optType = "AccountLimitType";
            var result = Submit("Service", "GetOptionDataList", false, new object[] { optType });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 具体限制车辆列表
        /// </summary>
        /// <param name="memId"></param>
        /// <returns></returns>
        public JsonResult GetActLimitVehicleList(string memId)
        {
            var result = Submit("Member360", "GetActLimitVehicleList", false, new object[] { memId });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 具体限制品牌列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetActLimitBrandList()
        {
            var dataGroupID = (int)Auth.DataGroupID;
            var result = Submit("BaseData", "GetVehicleBrandList", false, new object[] { dataGroupID });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 具体限制门店列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetActLimitStoreList()
        {
            var dataGroupID = (int)Auth.DataGroupID;
            var result = Submit("BaseData", "GetStoreLimitList", false, new object[] { dataGroupID, Auth.UserID });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 获取账户明细信息（360明细用）
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="accType"></param>
        /// <returns></returns>
        public JsonResult GetMemActDetails(string mid, string accType)
        {
            var info = Submit("Member360", "GetMemActDetails1", false, new object[] { mid, accType });
            return Json(info);
        }
        /// <summary>
        /// 获取账户明细信息（账户调整用）
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="accType"></param>
        /// <returns></returns>
        public JsonResult GetMemActDetails1(string mid, string accType)
        {
            var info = Submit("Member360", "GetMemActDetails", false, new object[] { mid, accType, Auth.UserID });
            return Json(info);
        }

        [HttpPost]
        public JsonResult BatchImportPoint()
        {
            try
            {
                var file = Request.Files[0];
                int FileLen = file.ContentLength;
                byte[] input = new byte[FileLen];
                System.IO.Stream MyStream = file.InputStream;
                MyStream.Read(input, 0, FileLen);
                DataTable dt = ExcelHelper.ExcelToDataTable(MyStream);

                var pointLists = new List<string>();               
                foreach (DataRow r in dt.Rows)
                {
                    if (r["账户调整卡号"] != null)
                    {
                        if (!string.IsNullOrEmpty(r["账户调整卡号"].ToString()))
                        {

                            pointLists.Add(r["账户调整卡号"].ToString() + "," + r["类型"].ToString() + "," + r["调整数"].ToString() + "," + r["积分过期时间"].ToString());
                        }
                    }
                }

                var result = Submit("Member360", "BatchImportPoint", false, new object[] { JsonHelper.Serialize(pointLists), Auth.UserID });

                return Json(result);
            }
            catch (Exception)
            {

                return Json("");
            }

        }




        #endregion

        #region 套餐销售
        public ActionResult PackageSale()
        {
            ViewBag.UserName = Auth.UserDisplayName;
            //门店名称
            var store = Submit("Service", "GetStoreName", false, new object[] { Auth.UserID });
            if (store.Obj[0] != null)
            {
                ViewBag.StoreName = store.Obj[0].ToString();
            }
            else
            {
                ViewBag.StoreName = "";

            }
            //门店名称
            //var printer = Submit("Service", "GetPrinterName", false, new object[] { Auth.UserID });
            //if (printer.Obj[0] != null)
            //{
            //    ViewBag.Printer = printer.Obj[0].ToString();
            //}
            //else
            //{
            //    ViewBag.Printer = "";
            //}
            ViewBag.Printer = "";
            return View();
        }

        /// <summary>
        /// 获取套餐列表
        /// </summary>
        /// <param name="pName"></param>
        /// <returns></returns>
        public JsonResult GetPackageList(string pName)
        {
            var dts = Request.CreateDataTableParameter();
            var groupId = Auth.DataGroupID;
            var result = Submit("Member360", "GetPackageList", false, new object[] { pName, groupId, Auth.UserID, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 保存套餐购买信息
        /// </summary>
        /// <param name="packageList"></param>
        /// <param name="actLimit"></param>
        /// <returns></returns>
        public JsonResult SavePackageSaleData(List<PackageModel> packageList, string mid, decimal total)
        {
            var userId = Auth.UserID;
            var groupId = Auth.DataGroupID;
            var rst = Submit("Member360", "SavePackageSaleData", false, new object[] { JsonHelper.Serialize(packageList), mid, total, userId, groupId });
            return Json(rst);
        }
        #endregion

        #region 套餐删除
        public ActionResult PackageAdjust()
        {
            return View();
        }
        /// <summary>
        /// 获取会员套餐列表
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public JsonResult GetMemPackageList(string mid)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("Member360", "GetMemPackageList", false, new object[] { mid, Auth.UserID, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 删除会员套餐
        /// </summary>
        /// <param name="packageID"></param>
        /// <returns></returns>
        public JsonResult DeleteMemPackage(int packageID, string mid)
        {
            var userId = Auth.UserID;
            var rst = Submit("Member360", "DeleteMemPackage", false, new object[] { packageID, mid, userId, Auth.DataGroupID });
            return Json(rst);
        }

        #endregion

        #region 添加优惠券
        public ActionResult CouponAdd()
        {
            return View();
        }

        public JsonResult GetCouponAddData(string couponName)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("Member360", "GetCouponAddData", false, new object[] { JsonHelper.Serialize(dts), couponName, Auth.UserID });
            return Json(result.Obj[0]);
        }

        //添加优惠券
        public JsonResult AddCouponByCode(string mid, int templetID, string couponCode, string reason)
        {
            DateTime time = DateTime.Now;
            string userId = Auth.UserID.ToString();
            couponCode = "C" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString() + new Random().Next(10, 99).ToString();
            var result = Submit("Member360", "AddCouponByCode", false, new object[] { Auth.DataGroupID, mid, templetID, couponCode, time, userId, reason });

            return Json(result);
        }
        #endregion

        #region 会员权益减扣
        public ActionResult MemInterestDeduction()
        {
            return View();
        }
        /// <summary>
        /// 验证委托书号
        /// </summary>
        /// <param name="tradeCode"></param>
        /// <returns></returns>
        public JsonResult CheckTradeCode(string mId, string tradeCode)
        {
            var result = Submit("Member360", "CheckTradeCode", false, new object[] { mId, tradeCode });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 获取账户信息
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public JsonResult GetAccountInfo(string mid)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("Member360", "GetAccountInfo", false, new object[] { mid, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 获取优惠券信息
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public JsonResult GetCouponInfo(string mid, string storeCode)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("Member360", "GetCouponInfo", false, new object[] { mid, storeCode, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 获取套餐信息
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public JsonResult GetPackageInfo(string mid, string storeCode)
        {
            var dts = Request.CreateDataTableParameter();

            var result = Submit("Member360", "GetPackageInfo", false, new object[] { mid, storeCode, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 获取公共优惠券
        /// </summary>
        /// <param name="couponCode"></param>
        /// <returns></returns>
        public JsonResult GetPublicCoupon(string couponCode)
        {
            var info = Submit("Member360", "GetPublicCoupon", false, new object[] { couponCode });
            return Json(info);
        }

        /// <summary>
        /// 会员权益扣减数据保存
        /// </summary>
        /// <param name="packageList"></param>
        /// <param name="accountList"></param>
        /// <param name="couponList"></param>
        /// <param name="mid"></param>
        /// <returns></returns>
        public JsonResult SaveMemInterestDeduction(List<PackageModel> packageList, List<AccountModel> accountList, List<CouModel> couponList, string mid)
        {
            var userId = Auth.UserID;
            var rst = Submit("Member360", "SaveMemInterestDeduction", false, new object[] { JsonHelper.Serialize(packageList), JsonHelper.Serialize(accountList), JsonHelper.Serialize(couponList), mid, userId });
            return Json(rst);
        }
        #endregion

        #region 储值管理
        public ActionResult StoreValue()
        {
            ViewBag.UserName = Auth.UserDisplayName;
            //额度等级
            var slevel = Submit("Service", "GetOptionDataList", false, new object[] { "StoreValueLevel" });
            if (slevel.Obj[0] != null)
            {
                ViewBag.StoreValueLevel = slevel.Obj[0];
            }
            else
            {
                ViewBag.StoreValueLevel = "";

            }


            //门店名称
            var store = Submit("Service", "GetStoreName", false, new object[] { Auth.UserID });
            if (store.Obj[0] != null)
            {
                ViewBag.StoreName = store.Obj[0].ToString();
            }
            else
            {
                ViewBag.StoreName = "";

            }
            //门店名称
            //var printer = Submit("Service", "GetPrinterName", false, new object[] { Auth.UserID });
            //if (printer.Obj[0] != null)
            //{
            //    ViewBag.Printer = printer.Obj[0].ToString();
            //}
            //else
            //{
            //    ViewBag.Printer = "";
            //}
            ViewBag.Printer = "";
            return View();
        }

        public ActionResult StoreValue_Finance()
        {
            ViewBag.UserName = Auth.UserDisplayName;
            //额度等级
            var slevel = Submit("Service", "GetOptionDataList", false, new object[] { "StoreValueLevel" });
            if (slevel.Obj[0] != null)
            {
                ViewBag.StoreValueLevel = slevel.Obj[0];
            }
            else
            {
                ViewBag.StoreValueLevel = "";

            }


            //门店名称
            var store = Submit("Service", "GetStoreName", false, new object[] { Auth.UserID });
            if (store.Obj[0] != null)
            {
                ViewBag.StoreName = store.Obj[0].ToString();
            }
            else
            {
                ViewBag.StoreName = "";

            }

            ViewBag.Printer = "";
            return View();
        }

        public ActionResult StoreValue_Search()
        {
            ViewBag.UserName = Auth.UserDisplayName;
            //额度等级
            var slevel = Submit("Service", "GetOptionDataList", false, new object[] { "StoreValueLevel" });
            if (slevel.Obj[0] != null)
            {
                ViewBag.StoreValueLevel = slevel.Obj[0];
            }
            else
            {
                ViewBag.StoreValueLevel = "";

            }


            //门店名称
            var store = Submit("Service", "GetStoreName", false, new object[] { Auth.UserID });
            if (store.Obj[0] != null)
            {
                ViewBag.StoreName = store.Obj[0].ToString();
            }
            else
            {
                ViewBag.StoreName = "";

            }

            ViewBag.Printer = "";
            return View();
        }

        /// <summary>
        /// 保存账户储值
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="changeValue"></param>
        /// <param name="isBack"></param>
        /// <returns></returns>
        public JsonResult SaveStoreValue(string mid, decimal changeValue, string storeCode)
        {
            decimal value = Convert.ToDecimal(changeValue);
            var userID = Auth.UserID.ToString();
            var dataGroupId = (int)Auth.DataGroupID;
            //storeCode = Auth.StoreCode.ToString();
            //if (!string.IsNullOrEmpty(storeCode))
            //    storeCode = storeCode.Split(',')[0];
            //else
            //    storeCode = Auth.StoreCode;
            var rst = Submit("Member360", "SaveStoreValueNew", false, new object[] { mid, value, userID, storeCode, dataGroupId });
            return Json(rst);
        }
        /// <summary>
        /// 获取不可退账户金额
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public JsonResult GetMemIsBackAccountInfo(string mid)
        {
            var info = Submit("Member360", "GetMemIsBackAccountInfo", false, new object[] { mid });
            return Json(info.Obj[0]);
        }

        public JsonResult GetMemIsBackAccountInfo_store(string mid, string storecode)
        {
            var info = Submit("Member360", "GetMemIsBackAccountInfo_store", false, new object[] { mid, storecode });
            return Json(info.Obj[0]);
        }

        public JsonResult GetStoreValueRecord(string mid)
        {
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("Member360", "GetStoreValueRecord", false, new object[] { mid, JsonHelper.Serialize(dts), JsonHelper.Serialize(Auth.RoleIDs), Auth.CurPageID });
            return Json(rst.Obj[0].ToString());
        }

        public JsonResult GetStoreValueRecord_Finance(string mid, string cardNO, string name, string mobile, string status, string startDate, string endDate)
        {
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("Member360", "GetStoreValueRecord_Finance", false, new object[] { mid, cardNO, name, mobile, status, startDate, endDate, JsonHelper.Serialize(dts), JsonHelper.Serialize(Auth.RoleIDs), Auth.CurPageID });
            return Json(rst.Obj[0].ToString());
        }

        public JsonResult GetStoreValueRecord_Search(string mid, string cardNO, string name, string mobile, string status, string startDate, string endDate)
        {
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("Member360", "GetStoreValueRecord_Search", false, new object[] { mid, cardNO, name, mobile, status, startDate, endDate, JsonHelper.Serialize(dts), JsonHelper.Serialize(Auth.RoleIDs), Auth.CurPageID });
            return Json(rst.Obj[0].ToString());
        }



        public JsonResult InActiveStoreValueById(long itemId)
        {

            var userID = Auth.UserID.ToString();
            var result = Submit("Member360", "InActiveStoreValueById", false, new object[] { itemId, userID });
            return Json(result);
        }

        #region 修改储值信息

        public JsonResult GetEditById(long mid)
        {
            //var userId = Auth.UserID.ToString();
            var result = Submit("Member360", "GetEditById", false, new object[] { mid });
            return Json(result);
        }

        public JsonResult SaveEditValue(long mid, string changeValue, string storeCode, string date, decimal? invoice, string payment, bool isStore)
        {
            decimal value = Convert.ToDecimal(changeValue);
            var userId = Auth.UserID.ToString();
            var rst = Submit("Member360", "SaveEditValue", false, new object[] { mid, value, userId, storeCode, invoice, payment, isStore });
            return Json(rst);
        }

        #endregion
        #endregion

        #region 卡挂失/解挂
        //卡变更
        public ActionResult CardChange()
        {
            var CardTypes = Submit("Service", "GetBizOptionsByType", false, new object[] { "CardChangeType", true });
            ViewBag.CardType = CardTypes.Obj[0];
            return View();
        }
        /// <summary>
        /// 卡变更列表
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public JsonResult CardChangeSearch(string orderNo, string cardNo, string phone, string status, string type, DateTime? startTime, DateTime? endTime)
        {
            var dts = Request.CreateDataTableParameter();
            int dataGroupId = (int)Auth.DataGroupID;
            startTime = startTime == null ? Convert.ToDateTime("1900-01-01 00:00:00") : startTime;
            endTime = endTime == null ? Convert.ToDateTime("9999-01-01 00:00:00") : endTime.Value.AddDays(1);
            var rst = Submit("Member360", "CardChangeSearch", false, new object[] { JsonHelper.Serialize(dts), orderNo, cardNo, phone, status, type, startTime, endTime, JsonHelper.Serialize(Auth.RoleIDs), Auth.CurPageID });
            return Json(rst.Obj[0].ToString());
        }

        /// <summary>
        /// 换卡列表查询
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public JsonResult GetCardByCardChange(string cardNo, string memName, string memMobile, string vehicleNo)
        {
            var dts = Request.CreateDataTableParameter();
            int dataGroupId = (int)Auth.DataGroupID;
            var rst = Submit("Member360", "GetCardByCardChange", false, new object[] { JsonHelper.Serialize(dts), cardNo, memName, memMobile, vehicleNo, JsonHelper.Serialize(Auth.RoleIDs), Auth.CurPageID });
            return Json(rst.Obj[0].ToString());
        }



        //卡挂失
        public ActionResult CardLock()
        {
            return View();
        }

        /// <summary>
        /// 卡变更列表
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="name"></param>
        /// <param name="mobile"></param>
        /// <param name="vehicleNo"></param>
        /// <returns></returns>
        public JsonResult SearchCardList(string cardNo, string phone, string mem, string idNum)
        {
            //string cardType = "member";
            var dts = Request.CreateDataTableParameter();
            int dataGroupId = (int)Auth.DataGroupID;
            //string pageIds = Auth.CurPageID.HasValue ? Auth.CurPageID.ToString() : "";
            //string dataRoleIds = "(" + string.Join(",", Auth.RoleIDs) + ")";
            var rst = Submit("Member360", "SearchCardList", false, new object[] { JsonHelper.Serialize(dts), cardNo, phone, mem, idNum, dataGroupId, Auth.CurPageID, JsonHelper.Serialize(Auth.RoleIDs) });
            return Json(rst.Obj[0].ToString());
        }
        //卡解挂
        public ActionResult CardUnlock()
        {
            return View();
        }
        /// <summary>
        /// 获取卡解挂列表
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="name"></param>
        /// <param name="mobile"></param>
        /// <param name="vehicleNo"></param>
        /// <returns></returns>
        public JsonResult SearchCardUnLockList(string cardNo, string phone, string mem, string idNum)
        {
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("Member360", "SearchCardUnLockList", false, new object[] { JsonHelper.Serialize(dts), cardNo, phone, mem, idNum, Auth.CurPageID, JsonHelper.Serialize(Auth.RoleIDs) });
            return Json(rst.Obj[0].ToString());
        }

        /// <summary>
        /// 锁卡保存
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public JsonResult LockAddCardPrepare(CardPrepare card, string type)
        {
            card.operatorStr = Auth.UserID.ToString();
            var rst = Submit("Member360", "LockAddCardPrepare", false, new object[] { card, type });
            return Json(rst);
        }
        /// <summary>
        /// 锁卡审核
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public JsonResult CardLockCheck(CardChange card, string type)
        {
            card.AddedUser = Auth.UserID.ToString();
            var rst = Submit("Member360", "CardLockCheck", false, new object[] { card, type });
            return Json(rst);
        }
        /// <summary>
        /// 锁卡保存并审核
        /// </summary>
        /// <param name="pCard"></param>
        /// <param name="cCard"></param>
        /// <returns></returns>
        public JsonResult LockAddAndCheck(CardPrepare pCard, CardChange cCard, string type)
        {
            pCard.operatorStr = Auth.UserID.ToString();
            cCard.AddedUser = Auth.UserID.ToString();
            var rst = Submit("Member360", "LockSaveAndCheck", false, new object[] { pCard, cCard, type });
            return Json(rst);
        }
        public ActionResult BulkCardLockAndUnlock()
        {
            return View();
        }
        /// <summary>
        /// 保存卡变更信息
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public JsonResult AddCardChange(CardChange card)
        {
            card.AddedUser = Auth.UserID.ToString();
            var rst = Submit("Member360", "AddCardChange", false, new object[] { card });
            return Json(rst);
        }


        public ActionResult OldToNew()
        {
            return View();
        }

        /// <summary>
        /// 旧卡换新卡
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public JsonResult SaveCardChange(CardChange card)
        {
            card.AddedUser = Auth.UserID.ToString();
            card.CardNo = HttpUtility.HtmlDecode(card.CardNo);
            card.Remark = HttpUtility.HtmlDecode(card.Remark);
            var rst = Submit("Member360", "SaveCardChange", false, new object[] { card });
            return Json(rst);
        }
        public JsonResult GetMembersNameByPage1(string cardNo, string memName, string memMobile, string vehicleNo)
        {
            var dts = Request.CreateDataTableParameter();
            int dataGroupId = (int)Auth.DataGroupID;
            string pageIds = Auth.CurPageID.HasValue ? Auth.CurPageID.ToString() : "";
            string dataRoleIds = "(" + string.Join(",", Auth.RoleIDs) + ")";
            var rst = Submit("Member360", "GetMemberNameByPage1", false, new object[] { JsonHelper.Serialize(dts), dataGroupId, pageIds, dataRoleIds, cardNo, memName, memMobile, vehicleNo });
            return Json(rst.Obj[0].ToString());
        }

        /// <summary>
        /// 批量卡挂失
        /// </summary>
        /// <param name="batchCardNo"></param>
        /// <returns></returns>
        public JsonResult BatchCardLock(string batchCardNo)
        {
            var result = Submit("Member360", "BatchCardLock", false, new object[] { batchCardNo });
            return Json(result);
        }
        #endregion

        #region 储值历史打印
        public ActionResult StoreValuePrint()
        {
            //门店名称
            var store = Submit("Service", "GetStoreName", false, new object[] { Auth.UserID });
            if (store.Obj[0] != null)
            {
                ViewBag.StoreName = store.Obj[0].ToString();
            }
            else
            {
                ViewBag.StoreName = "";

            }
            //门店名称
            //var printer = Submit("Service", "GetPrinterName", false, new object[] { Auth.UserID });
            //if (printer.Obj[0] != null)
            //{
            //    ViewBag.Printer = printer.Obj[0].ToString();
            //}
            //else
            //{
            //    ViewBag.Printer = "";
            //}
            ViewBag.Printer = "";
            return View();
        }
        /// <summary>
        /// 获取储值历史
        /// </summary>
        /// <param name="memName"></param>
        /// <param name="memMobile"></param>
        /// <param name="vehicleNo"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public JsonResult GetActHistory(string memName, string memMobile, string vehicleNo, DateTime? startDate, DateTime? endDate)
        {
            var dts = Request.CreateDataTableParameter();
            int dataGroupId = (int)Auth.DataGroupID;
            var rst = Submit("Member360", "GetActHistoryByPage", false, new object[] { JsonHelper.Serialize(dts), dataGroupId, memName, memMobile, vehicleNo, startDate, endDate, Auth.UserID });
            return Json(rst.Obj[0].ToString());
        }
        #endregion

        #region 套餐销售历史打印
        public ActionResult PackageSalePrint()
        {
            //门店名称
            var store = Submit("Service", "GetStoreName", false, new object[] { Auth.UserID });
            if (store.Obj[0] != null)
            {
                ViewBag.StoreName = store.Obj[0].ToString();
            }
            else
            {
                ViewBag.StoreName = "";

            }
            //门店名称
            //var printer = Submit("Service", "GetPrinterName", false, new object[] { Auth.UserID });
            //if (printer.Obj[0] != null)
            //{
            //    ViewBag.Printer = printer.Obj[0].ToString();
            //}
            //else
            //{
            //    ViewBag.Printer = "";
            //}
            ViewBag.Printer = "";
            return View();
        }
        /// <summary>
        /// 获取套餐销售历史
        /// </summary>
        /// <param name="memName"></param>
        /// <param name="memMobile"></param>
        /// <param name="vehicleNo"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public JsonResult GetPackageSaleHistory(string memName, string memMobile, string vehicleNo, DateTime? startDate, DateTime? endDate)
        {
            var dts = Request.CreateDataTableParameter();
            int dataGroupId = (int)Auth.DataGroupID;
            var rst = Submit("Member360", "GetPackageSaleHistoryByPage", false, new object[] { JsonHelper.Serialize(dts), dataGroupId, memName, memMobile, vehicleNo, startDate, endDate, Auth.UserID });
            return Json(rst.Obj[0].ToString());
        }

        public JsonResult GetPakcgeHistoryByTradeID(long? tradeId)
        {
            var result = Submit("Member360", "GetPakcgeHistoryByTradeID", false, new object[] { tradeId });
            return Json(result.Obj[0].ToString());
        }
        #endregion

        #region 会员历史金额调整
        public ActionResult HistoryAmountAdjust()
        {
            return View();
        }

        /// <summary>
        /// 保存调整历史金额信息
        /// </summary>
        /// <param name="ajustType"></param>
        /// <param name="value"></param>
        /// <param name="memId"></param>
        /// <param name="ajustReason"></param>
        /// <returns></returns>

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveAjustAmountInfo(string memId, string ajustType, decimal value, string ajustReason)
        {
            decimal ajustValue = Convert.ToDecimal(value);
            var result = Submit("Member360", "SaveAjustAmountInfo", false, new object[] { memId, ajustType, ajustValue, ajustReason, Auth.UserID.ToString() });
            return Json(result);
        }

        ///// <summary>
        ///// 查询历史金额调整记录
        ///// </summary>
        ///// <param name="cardNo"></param>
        ///// <param name="memNo"></param>
        ///// <param name="memName"></param>
        ///// <param name="memMobile"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult GetAjustHistoryAmountData(string cardNo, string memNo, string memName, string memMobile)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var result = Submit("Member360", "GetAjustHistoryAmountData", false, new object[] { cardNo, memNo, memName, memMobile, JsonHelper.Serialize(dts)});
        //    return Json(result);
        //}

        /// <summary>
        /// 查询历史金额调整记录
        /// </summary>
        /// <param name="memId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAjustHistoryAmountData(string memId)
        {
            //var dts = Request.CreateDataTableParameter();
            var result = Submit("Member360", "GetAjustHistoryAmountData", false, new object[] { memId });
            return Json(result);
        }

        /// <summary>
        /// 根据id获取会员信息及历史金额
        /// </summary>
        /// <param name="memId"></param>
        /// <returns></returns>
        public JsonResult GetMemberAmountByMemberId(string memId)
        {
            var info = Submit("Member360", "GetMemberAmountByMemberId", false, new object[] { memId });
            return Json(info);
        }
        #endregion

        #region 360新页面
        public ActionResult NewIndex()
        {
            return View();
        }


        public ActionResult MemberDetailNew()
        {
            //ViewBag.BrandCode = Auth.BrandCode;
            //ViewBag.AreaCode = Auth.AreaCode;
            //颜色 
            //var chan = Submit("Service", "GetColors", false);
            //ViewBag.Colors = chan.Obj[0];
            //区域 
            //var Areas = Submit("Service", "GetAreas", false);
            //ViewBag.Areas = Areas.Obj[0];
            ////品牌 
            //var Brands = Submit("Service", "GetBrands", false);
            //ViewBag.Brands = Brands.Obj[0];
            ////内饰 
            //var Trims = Submit("Service", "GetTrims", false);
            //ViewBag.Trims = Trims.Obj[0];
            return View();
        }
        public JsonResult AddMemberData(MemberInfo mem)
        {
            var userid = Auth.UserID.ToString();

            var result = Submit("Member360", "AddMemberData", false, new object[] { JsonHelper.Serialize(mem), userid });

            return Json(result);
        }
        #endregion

        #region 结算页面
        public ActionResult Statement()
        {

            ViewBag.UserId = Auth.Username;//登录名
            ViewBag.UserName = Auth.UserDisplayName;//名字
            //渠道 
            var chan = Submit("Service", "GetStoreChannels", false);
            ViewBag.Channels = chan.Obj[0];



            //var pages = Submit("Member360", "GetCouponByLimit", false);
            //ViewBag.AllPages = pages.Obj[0];

            return View();
        }

        public ActionResult Statement_Finance()
        {

            //渠道 
            var chan = Submit("Service", "GetStoreChannels", false);
            ViewBag.Channels = chan.Obj[0];

            return View();
        }

        public ActionResult Statement_Search()
        {

            //渠道 
            var chan = Submit("Service", "GetStoreChannels", false);
            ViewBag.Channels = chan.Obj[0];

            return View();
        }

        public ActionResult Statement_FinalCheck()
        {

            //渠道 
            var chan = Submit("Service", "GetStoreChannels", false);
            ViewBag.Channels = chan.Obj[0];

            return View();
        }

        /// <summary>
        /// 保存结算信息
        /// </summary>
        /// <param name="busitype"></param>
        /// <param name="busichild"></param>
        /// <param name="channel"></param>
        /// <param name="memcard"></param>
        /// <param name="busino"></param>
        /// <param name="date"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public JsonResult AddStatementData(TradeInfo trade)
        {
            var userid = Auth.UserID.ToString();
            var storecode = Auth.StoreCode;
            //if (string.IsNullOrEmpty(storecode))
            //{
            //    storecode = storecode.Split(',')[0];
            //}
            //trade.StoreCode = storecode;
            var result = Submit("Member360", "AddStatementData", false, new object[] { JsonHelper.Serialize(trade), userid });

            return Json(result);
        }

        public JsonResult EditStatementData(TradeInfo trade, long mid, bool isFinance)
        {
            var userid = Auth.UserID.ToString();
            var result = Submit("Member360", "EditStatementData", false, new object[] { JsonHelper.Serialize(trade), userid, mid, isFinance });

            return Json(result);
        }

        /// <summary>
        /// 加载结算信息列表
        /// </summary>
        /// <param name="busiType"></param>
        /// <param name="busiChild"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public JsonResult GetStatementData(string busiType, string busiChild, string mid, DateTime? startDate, DateTime? endDate)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("Member360", "GetStatementData", false, new object[] { busiType, busiChild, mid, startDate, endDate, JsonHelper.Serialize(dts), JsonHelper.Serialize(Auth.RoleIDs), Auth.CurPageID });

            return Json(result.Obj[0]);
        }
        public JsonResult GetAuthName()
        {
            var result = Submit("Member360", "GetAuthName", false, new object[] { Auth.UserID });
            return Json(result.Obj[0]);
        }



        public JsonResult GetStatementData_Finance(string cardNo, string name, string mobile, string status, string startDate, string endDate, string busiType, string busiChild, string channel, string mid, string vehno, string vinno)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("Member360", "GetStatementData_Finance", false, new object[] { cardNo, name, mobile, status, startDate, endDate, busiType, busiChild, channel, mid, JsonHelper.Serialize(dts), JsonHelper.Serialize(Auth.RoleIDs), Auth.CurPageID, vehno, vinno });

            return Json(result.Obj[0]);
        }

        public JsonResult GetStatementData_Search(string cardNo, string name, string mobile, string status, string startDate, string endDate, string busiType, string busiChild, string channel, string mid, string vehno, string vinno, string department)
        {

            var dts = Request.CreateDataTableParameter();
            var result = Submit("Member360", "GetStatementData_Search", false, new object[] { cardNo, name, mobile, status, startDate, endDate, busiType, busiChild, channel, mid, JsonHelper.Serialize(dts), JsonHelper.Serialize(Auth.RoleIDs), Auth.CurPageID, vehno, vinno, department });

            return Json(result.Obj[0]);
        }

        public JsonResult GetStatementData_FinalCheck(string cardNo, string name, string mobile, string status, string startDate, string endDate, string busiType, string busiChild, string channel, string mid, string vehno, string vinno, string tradeCode)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("Member360", "GetStatementData_FinalCheck", false, new object[] { cardNo, name, mobile, status, startDate, endDate, busiType, busiChild, channel, mid, JsonHelper.Serialize(dts), JsonHelper.Serialize(Auth.RoleIDs), Auth.CurPageID, vehno, vinno, tradeCode });

            return Json(result.Obj[0]);
        }

        [HttpPost]
        public FileResult ExportStatementSearch(string exprCardNo, string exprMemName, string exprMobile, string exprVinNo, string exprVehNo, string exprStatus, string exprBusitype, string exprChildType, DateTime? exprDtStart, DateTime? exprDtEnd)
        {

            List<ExcelColumnFormat<TradeSales>> mainSheetFormats = new List<ExcelColumnFormat<TradeSales>>{
                    new ExcelColumnFormat<TradeSales>{ColumnName="业务类型", FuncGetValue=p=>p.TradeTypeA},
                    new ExcelColumnFormat<TradeSales>{ColumnName="子业务类型", FuncGetValue=p=>p.TradeTypeB},
                    new ExcelColumnFormat<TradeSales>{ColumnName="会员卡号", FuncGetValue=p=>p.MemberCardNo},
                    new ExcelColumnFormat<TradeSales>{ColumnName="会员姓名", FuncGetValue=p=>p.CustomerName},
                    new ExcelColumnFormat<TradeSales>{ColumnName="车牌号", FuncGetValue=p=>p.PlateNumVehicle},
                    new ExcelColumnFormat<TradeSales>{ColumnName="业务单号", FuncGetValue=p=>p.TradeCode},
                    new ExcelColumnFormat<TradeSales>{ColumnName="发生日期", FuncGetValue=p=>p.ListDateSales},
                    new ExcelColumnFormat<TradeSales>{ColumnName="单据总金额", FuncGetValue=p=>p.StandardAmountSales},
                    new ExcelColumnFormat<TradeSales>{ColumnName="应付金额", FuncGetValue=p=>p.Amount},
                    new ExcelColumnFormat<TradeSales>{ColumnName="金币金额", FuncGetValue=p=>p.GoldAmountSales},
                    new ExcelColumnFormat<TradeSales>{ColumnName="非金币金额", FuncGetValue=p=>p.NotGoldAmountSales},
                    new ExcelColumnFormat<TradeSales>{ColumnName="审批状态", FuncGetValue=p=>p.StatusSalesText}
            };
            try
            {
                var result = Submit("Member360", "ExportStatementData_Search", false, new object[] { exprCardNo, exprMemName, exprMobile, exprVinNo, exprVehNo, exprStatus, exprBusitype, exprChildType, exprDtStart, exprDtEnd, JsonHelper.Serialize(Auth.RoleIDs), Auth.CurPageID });

                if (result.IsPass == false)
                {
                    List<TradeSales> list = new List<TradeSales>();
                    var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
                    var stream = ExcelHelper.GetStream(workBook);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
                }
                else
                {
                    List<TradeSales> list = JsonHelper.Deserialize<List<TradeSales>>(result.Obj[0].ToString());
                    HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, null);
                    var stream = ExcelHelper.GetStream(workBook);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(stream, "application/vnd.ms-excel", "结算单导出" + DateTime.Now.ToShortDateString() + ".xls");
                }
            }
            catch (Exception e)
            {
                MemoryStream strm = new MemoryStream();
                return File(strm, "application/vnd.ms-excel", "结算单统计异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
            }
        }
        public JsonResult InActiveItemById(string itemId, string password)
        {

            var userID = Auth.UserID.ToString();
            var result = Submit("Member360", "InActiveItemById", false, new object[] { itemId, userID, password });
            return Json(result);
        }

        public JsonResult GetStatementEditInfo(long mid)
        {
            var result = Submit("Member360", "GetStatementEditInfo", false, mid);
            return Json(result);
        }

        public JsonResult GetAvailableCoupon(int tradeId)
        {
            var result = Submit("Member360", "GetCouponByLimit", false, tradeId);
            return Json(result.Obj[0]);
        }

        public JsonResult DeleteItemById(int itemId)
        {

            var userID = Auth.UserID.ToString();
            var result = Submit("Member360", "DeleteItemById", false, new object[] { itemId });
            return Json(result);
        }

        public JsonResult EditCheckedData(TradeInfo trade, long mid)
        {
            var userid = Auth.UserID.ToString();
            var result = Submit("Member360", "EditCheckedData", false, new object[] { JsonHelper.Serialize(trade), userid, mid });

            return Json(result);
        }



        public JsonResult GetStatementStatus(string mid)
        {

            var result = Submit("Member360", "GetStatementStatus", false, new object[] { mid });

            return Json(result);
        }


        #endregion

        #region 开票页面
        public ActionResult Invoice()
        {
            return View();
        }
        public JsonResult AddOrUpdateInvoiceData(InvoiceInfo invoice)
        {

            invoice.Address = HttpUtility.HtmlDecode(invoice.Address);
            var userid = Auth.UserID.ToString();
            var result = Submit("Member360", "AddOrUpdateInvoiceData", false, new object[] { JsonHelper.Serialize(invoice), userid });

            return Json(result);


        }
        public JsonResult EditInvoiceData(InvoiceInfo invoice)
        {

            invoice.Address = HttpUtility.HtmlDecode(invoice.Address);
            var userid = Auth.UserID.ToString();
            var result = Submit("Member360", "EditInvoiceData", false, new object[] { JsonHelper.Serialize(invoice), userid });

            return Json(result);


        }

        public JsonResult GetInvoiceTotal(string mid)
        {
            var result = Submit("Member360", "GetInvoiceTotal", false, new object[] { mid });

            return Json(result.Obj[0]);
        }

        public JsonResult GetInvoiceData(string mid, DateTime? startDate, DateTime? endDate)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("Member360", "GetInvoiceData", false, new object[] { mid, startDate, endDate, JsonHelper.Serialize(dts), JsonHelper.Serialize(Auth.RoleIDs), Auth.CurPageID });

            return Json(result.Obj[0]);
        }
        public JsonResult GetInvoiceEditInfo(long tid)
        {
            var result = Submit("Member360", "GetInvoiceEditInfo", false, tid);
            return Json(result);
        }
        #endregion

        #region 开票审批页面
        public ActionResult Invoice_Finance()
        {
            return View();
        }
        //门店审批查询
        public JsonResult GetInvoice_Finance(string startDate, string endDate, string status)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("Member360", "GetInvoice_Finance", false, new object[] { startDate, endDate, status, JsonHelper.Serialize(dts), JsonHelper.Serialize(Auth.RoleIDs), Auth.CurPageID });

            return Json(result.Obj[0]);
        }
        //集团审批查询
        public JsonResult GetInvoiceGroup_Finance(string startDate, string endDate, string status)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("Member360", "GetInvoiceGroup_Finance", false, new object[] { startDate, endDate, status, JsonHelper.Serialize(dts), JsonHelper.Serialize(Auth.RoleIDs), Auth.CurPageID });

            return Json(result.Obj[0]);
        }

        public JsonResult UpdateInvoiceStatus(long tid)
        {
            var result = Submit("Member360", "UpdateInvoiceStatus", false, new object[] { tid });

            return Json(result);
        }
        #endregion

        #region 开票审批页面集团审批
        public ActionResult InvoiceGroup_Finance()
        {
            return View();
        }

        #endregion

        #region 车辆管理
        public ActionResult VehcileManage()
        { //颜色 
            var chan = Submit("Service", "GetColors", false);
            ViewBag.Colors = chan.Obj[0];
            //内饰 
            var Trims = Submit("Service", "GetTrims", false);
            ViewBag.Trims = Trims.Obj[0];
            return View();
        }
        /// <summary>
        /// 编辑车辆
        /// </summary>
        /// <param name="vechile"></param>
        /// <returns></returns>
        public JsonResult UpdateVehcileData(VechileInfo vechile)
        {
            var userid = Auth.UserID.ToString();
            var result = Submit("Member360", "UpdateVehcileData", false, new object[] { JsonHelper.Serialize(vechile), userid });

            return Json(result);
        }
        /// <summary>
        /// 获取车辆信息(根据人)
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public JsonResult GetVehcileListByMid(string mid)
        {

            var dts = Request.CreateDataTableParameter();
            var result = Submit("Member360", "GetVehcileListByMid", false, new object[] { mid, JsonHelper.Serialize(dts) });

            return Json(result.Obj[0]);
        }
        /// <summary>
        /// 获取单辆车信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetVehcileInfoByid(long id)
        {
            var result = Submit("Member360", "GetVehcileInfoByid", false, new object[] { id });

            return Json(result.Obj[0]);
        }
        /// <summary>
        /// 删除车辆信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteVehcileData(long id)
        {
            var result = Submit("Member360", "DeleteVehcileData", false, new object[] { id });
            return Json(result);
        }

        public JsonResult TransferVehcileData(long id)
        {
            var result = Submit("Member360", "TransferVehcileData", false, new object[] { id });
            return Json(result);
        }
        #endregion

        #region 账户调整新
        public ActionResult AccountAdjustNew()
        {
            return View();
        }
        public JsonResult SaveAccountChange(string mid, string tid, decimal changeValue, string changereson, string reasoncode, string remark, string storecode)
        {
            decimal value = Convert.ToDecimal(changeValue);
            var userID = Auth.UserID.ToString();
            remark = HttpUtility.HtmlDecode(remark);

            var rst = Submit("Member360", "SaveAccountChange", false, new object[] { mid, tid, value, userID, changereson, reasoncode, remark, storecode });
            return Json(rst);
        }
        public JsonResult GetAccountChangeRecord(string mid, string startdate, string enddate)
        {
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("Member360", "GetAccountChangeRecord", false, new object[] { mid, startdate, enddate, JsonHelper.Serialize(Auth.RoleIDs), Auth.CurPageID, JsonHelper.Serialize(dts) });
            return Json(rst.Obj[0].ToString());
        }
        public JsonResult GetActChangeEditInfo(long tid)
        {
            var result = Submit("Member360", "GetActChangeEditInfo", false, tid);
            return Json(result);
        }

        #endregion

        #region 账户调整审批
        public ActionResult AccountAdjust_Finance()
        {
            return View();
        }
        public JsonResult GetAccountChangeRecord_Finance(string cardNO, string name, string mobile, string status, string startDate, string endDate)
        {
            var dts = Request.CreateDataTableParameter();
            var rst = Submit("Member360", "GetAccountChangeRecord_Finance", false, new object[] { cardNO, name, mobile, status, startDate, endDate, JsonHelper.Serialize(dts), JsonHelper.Serialize(Auth.RoleIDs), Auth.CurPageID });
            return Json(rst.Obj[0].ToString());
        }
        public JsonResult InActiveActChangeById(long itemId)
        {

            var userID = Auth.UserID.ToString();
            var result = Submit("Member360", "InActiveActChangeById", false, new object[] { itemId, userID });
            return Json(result);
        }


        #endregion

        public ActionResult GetStoreListByGroupID()
        {

            var resutl = Submit("BaseData", "GetStoreListByGroupID", false, new object[] { JsonHelper.Serialize(Auth.RoleIDs), Auth.CurPageID, Auth.DataGroupID });
            return Json(resutl.Obj[0]);
        }

        public ActionResult GetStoreByCode(string storeCode)
        {

            var resutl = Submit("BaseData", "GetStoreByCode", false, new object[] { storeCode });
            return Json(resutl.Obj[0]);
        }

        #region 老会员审批
        public ActionResult OldMemApprove()
        {
            return View();
        }

        public JsonResult GetOldMembersByPage(string memNo, string memName, string memMobile, string status, string startDate, string endDate)
        {
            var dts = Request.CreateDataTableParameter();
            memName = HttpUtility.HtmlDecode(memName);
            memMobile = HttpUtility.HtmlDecode(memMobile);
            if (!string.IsNullOrEmpty(memNo))
                memNo = HttpUtility.HtmlDecode(memNo).ToUpper();

            var rds = Auth.RoleIDs;
            var pid = Auth.CurPageID;
            var rst = Submit("Member360", "GetOldMembersByPage", false, new object[] { JsonHelper.Serialize(dts), memNo, memName, memMobile, status, startDate, endDate, JsonHelper.Serialize(rds), pid });
            return Json(rst.Obj[0].ToString());
        }
        /// <summary>
        /// 老会员审批
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public JsonResult UpdateOldMemStatus(string mid, Decimal? total, Decimal? noinvoice, Decimal? recharge, Decimal? send)
        {
            var result = Submit("Member360", "UpdateOldMemStatus", false, new object[] { mid, total, noinvoice, recharge, send, Auth.UserID.ToString() });

            return Json(result);
        }

        public JsonResult GetOldMemActInfo(string mid)
        {
            var result = Submit("Member360", "GetOldMemActInfo", false, mid);
            return Json(result.Obj[0]);
        }

        #endregion

        #region 体验券核销
        public ActionResult CouponVerification()
        {
            return View();
        }

        /// <summary>
        /// 获取体验券信息(根据人)
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public JsonResult GetCouponListByMid(string mid)
        {

            var dts = Request.CreateDataTableParameter();
            var result = Submit("Member360", "GetCouponListByMid", false, new object[] { mid, JsonHelper.Serialize(dts) });

            return Json(result.Obj[0]);
        }

        public JsonResult UsingCoupon(long cid)
        {

            var result = Submit("Member360", "UsingCoupon", false, new object[] { cid });

            return Json(result);
        }


        #endregion

        #region Member360导出
        [HttpPost]
        public FileResult ExportMember360List(string condition)
        {
            var a = Auth.CurPageID;
            var rds = Auth.RoleIDs;
            List<ExportMember360Model> list;
            List<ExcelColumnFormat<ExportMember360Model>> excelColumnFormats = new List<ExcelColumnFormat<ExportMember360Model>>();
            var queryResult = Submit("Member360", "Export360Column", false, new object[] { "TM_Mem_ext", 2, "0" });
            var queryList = JsonHelper.Deserialize<List<ExportColumnModel>>(queryResult.Obj[0].ToString());
            var type = typeof(ExportMember360Model);
            foreach (var item in queryList)
            {
                ExcelColumnFormat<ExportMember360Model> ef = new ExcelColumnFormat<ExportMember360Model> { ColumnName = item.Name, FuncGetValue = p => type.GetProperty(item.Code).GetValue(p) };
                excelColumnFormats.Add(ef);
            }
            try
            {

                //string roleIdsStr = Util.Serialize(Auth.UserRoleList.Select(r => r.RoleID));
                var result = Submit("Member360", "ExportMember360List", false, new object[] { a, JsonHelper.Serialize(rds), "TM_Mem_Ext", null, null, a, "0", null, condition });
                //return JsonHelper.Deserialize<MemSubdivisionResultModel>(result.Obj[0].ToString());

                if (result.IsPass == false)
                {
                    list = new List<ExportMember360Model>();
                    var workBook = ExcelHelper.DataToExcel(list, excelColumnFormats);
                    var stream = ExcelHelper.GetStream(workBook);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
                }
                else
                {
                    list = JsonHelper.Deserialize<List<ExportMember360Model>>(result.Obj[0].ToString());
                    var workBook = ExcelHelper.DataToExcel(list, excelColumnFormats);
                    var stream = ExcelHelper.GetStream(workBook);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(stream, "application/vnd.ms-excel", "会员360导出" + DateTime.Now.ToShortDateString() + ".xls");
                }
            }
            catch (Exception e)
            {
                list = new List<ExportMember360Model>();
                var workBook = ExcelHelper.DataToExcel(list, excelColumnFormats);
                var stream = ExcelHelper.GetStream(workBook);
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                string errmsg = e.GetType().Name;
                if (errmsg == "OutOfMemoryException") errmsg = "导出数据量过大";
                return File(stream, "application/vnd.ms-excel", "会员360导出异常" + DateTime.Now.ToShortDateString() + "_" + errmsg + "_无法导出.xls");
            }
        }
        #endregion

        #region 异常购物
        public ActionResult AbnormalTrade()
        {
            return View();
        }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <param name="tradeID"></param>
        /// <returns></returns>
        public ActionResult AbnormalTradeList(string tradeID)
        {
            var page = Request.CreateDataTableParameter();
            var result = Submit("Member360", "AbnormalTradeList", false, new object[] { tradeID,JsonHelper.Serialize(page)});
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <returns></returns>
        public FileResult AbnormalTradeToExcel()
        {
            var result = Submit("Member360", "AbnormalTradeToExcel", false, new object[] { });
            var abnormalTradeList = JsonHelper.Deserialize<List<AbnormalTradeToExcel>>(result.Obj[0].ToString());
            List<ExcelColumnFormat<AbnormalTradeToExcel>> excelColumnFormats = new List<ExcelColumnFormat<AbnormalTradeToExcel>>{
               new ExcelColumnFormat<AbnormalTradeToExcel>{ColumnName="ID", FuncGetValue=p=>p.TradeID},          
               new ExcelColumnFormat<AbnormalTradeToExcel>{ColumnName="代理商", FuncGetValue=p=>p.StoreCode},
               new ExcelColumnFormat<AbnormalTradeToExcel>{ColumnName="会员ID", FuncGetValue=p=>p.MemberID},
               new ExcelColumnFormat<AbnormalTradeToExcel>{ColumnName="会员名称", FuncGetValue=p=>p.CustomerName},      
                  new ExcelColumnFormat<AbnormalTradeToExcel>{ColumnName="手机号", FuncGetValue=p=>p.Mobile},      
               new ExcelColumnFormat<AbnormalTradeToExcel>{ColumnName="原因", FuncGetValue=p=>p.Reason},      
               new ExcelColumnFormat<AbnormalTradeToExcel>{ColumnName="操作人", FuncGetValue=p=>p.AddedUser},       
               new ExcelColumnFormat<AbnormalTradeToExcel>{ColumnName="操作时间", FuncGetValue=p=>p.AddedDate},           
            };
            var wordBook = ExcelHelper.DataToExcel(abnormalTradeList, excelColumnFormats);
            var stream = ExcelHelper.GetStream(wordBook);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, "异常购物详情导出" + DateTime.Now.ToShortDateString() + ".xls");
        }

      
        public JsonResult DeleteTrade(string jsonParam)
        {
            var result = Submit("Member360", "DeleteTrade", false, new object[] { jsonParam, Auth.UserID});
            return Json(result);
        }
        #endregion

    }



}
             
 