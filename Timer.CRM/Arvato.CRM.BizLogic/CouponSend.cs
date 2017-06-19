using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.BizLogic
{
    public static class CouponSend
    {
        #region 获取购物券派送列表
        public static Result LoadCouponSend(CouponListModel model, string dp)
        {
            var myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            try
            {
                using (var db = new CRMEntities())
                {
                    var userLst = db.TM_AUTH_User.Select(v => new { UserID = v.UserID, UserName = v.UserName }).ToList();
                    var userDic = new Dictionary<string, string>();
                    userLst.ForEach(v => userDic[v.UserID.ToString()] = v.UserName);
                    var query = db.TM_JPOS_CouponList
                        .Select(i => new
                        {
                            i.CouponListID,
                            i.CouponId,
                            i.SendCount,
                            i.CouponValue,
                            i.ModifiedDate,
                            i.ModifiedUser,
                            i.Statu,
                            i.EndDate,
                            i.BeginDate,
                            i.OddNumber,
                            i.CouponDesc,
                            i.CouponInfo,
                            AppStatus = db.TD_SYS_BizOption.Where(x => x.OptionType == "ApproveStatus" && x.OptionValue == SqlFunctions.StringConvert((double)i.Statu)).Select(x => x.OptionText),

                        });
                    if (model.Statu != null) query = query.Where(i => i.Statu == model.Statu);
                    if (!string.IsNullOrEmpty(model.CouponInfo)) query = query.Where(i => i.CouponInfo.Contains(model.CouponInfo));
                    if (model.CouponValue != null) query = query.Where(i => i.CouponValue == model.CouponValue);
                    if (model.OddNumber != null) query = query.Where(i => SqlFunctions.StringConvert((double)i.OddNumber).Contains(SqlFunctions.StringConvert((double)model.OddNumber)));
                    var res = query.ToList().Select(i => new
                    {
                        i.CouponListID,
                        i.CouponId,
                        i.SendCount,
                        i.CouponValue,
                        i.ModifiedDate,
                        ModifiedUser = userDic[i.ModifiedUser],
                        i.Statu,
                        i.EndDate,
                        i.BeginDate,
                        i.OddNumber,
                        i.CouponDesc,
                        i.CouponInfo,
                        i.AppStatus

                    });
                    return new Result(true, "", new List<object> { res.ToList().ToDataTableSourceVsPage(myDp) });
                }
            }
            catch
            {
                return new Result(false, "", new List<object> { new List<CouponSendRuleModel>().ToDataTableSourceVsPage(myDp) });
            }
        }
        #endregion

        #region 改变购物券派送的审核状态
        public static Result ApproveCouponSendById(string Id, string active, int User, string mapPath)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var dt = DateTime.Now;
                    var query = db.TM_JPOS_CouponList.Find(Id);
                    query.Statu = Convert.ToInt32(active);
                    var ts = ToolsHelper.DateTimeToStamp(dt);
                    query.ModifiedDate = dt;
                    query.ModifiedUser = User.ToString();

                    var ct = db.TM_JPOS_CouponUseRule.Where(p => p.ID == query.CouponId).FirstOrDefault();
                    var couponlst = db.TM_JPOS_CouponPool.Where(p => p.ListId == Id).ToList();
                    int j = 0;
                    foreach (var item in couponlst)
                    {
                        List<VouchersModel> lvm = new List<VouchersModel>();

                        for (int i = 0; i < item.SendCount; i++)
                        {
                            TM_Mem_CouponPool cp = new TM_Mem_CouponPool();
                            cp.AddedDate = DateTime.Now;
                            cp.BatchNo = item.StoreCode;
                            Random ran = new Random();
                            cp.CouponCode = ts.ToString() + ran.Next(100, 1000).ToString() + j.ToString();
                            cp.TempletID = Convert.ToInt32(query.CouponId);
                            cp.CouponType = "1";
                            cp.StartDate = ct.StartDate;
                            cp.EndDate = ct.EndDate;
                            cp.IsUsed = false;
                            cp.Enable = true;
                            db.TM_Mem_CouponPool.Add(cp);
                            j++;
                            VouchersModel vm = new VouchersModel();
                            vm.beginDate = ct.StartDate;
                            vm.endDate = ct.EndDate;
                            vm.imageName = "";
                            vm.parValue = ct.CouponValue.ToString();
                            vm.purchaseRemark = ct.LimitRemark;
                            vm.voucherName = ct.CouponName;
                            lvm.Add(vm);
                        }
                        CreatTxtModel cm = new CreatTxtModel();
                        IdRetCodeModel im = new IdRetCodeModel();
                        retCodeModel rm = new retCodeModel();

                        rm.code = 96960; rm.message = "OK";
                        im.code = "00"; im.message = "交易成功";
                        cm.ldRetCode = im;
                        cm.retCode = rm;
                        cm.time = DateTime.Now;
                        cm.vouchers = lvm;
                        //默认空对象
                        cm.command = new object();
                        var url = CreateTXT.WriteTxt(cm, mapPath);
                        item.FilePath = url;
                    }
                    db.SaveChanges();

                    var msg = "审核成功";
                    if (active == "2")
                        msg = "撤销成功";
                    return new Result(true, msg);
                }
            }
            catch
            {
                return new Result(false, "操作失败：数据不存在");
            }
        }

        #endregion

        #region 删除购物券

        public static Result DeleteCouponSendById(string Id, int User)
        {
            try
            {

                using (var db = new CRMEntities())
                {
                    var dt = DateTime.Now;
                    var query = db.TM_JPOS_CouponList.Find(Id);
                    db.TM_JPOS_CouponList.Remove(query);
                    db.SaveChanges();
                    return new Result(true, "删除成功");
                }
            }
            catch
            {
                return new Result(false, "删除失败");
            }
        }

        #endregion

        #region 获取门店列表
        public static Result LoadStoreList(string storeArr, string dp)
        {
            var myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            try
            {
                string[] codeList = storeArr.Split(',');
                using (var db = new CRMEntities())
                {
                    var query = db.V_M_TM_SYS_BaseData_store
                        .Where(i => codeList.Contains(i.StoreCode))
                        .Select(i => new
                        {
                            i.BaseDataID,
                            i.StoreCode,
                            i.StoreName
                        }).ToList();

                    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
                }
            }
            catch (Exception)
            {
                return new Result(false, "", new List<object> { new List<dynamic>().ToDataTableSourceVsPage(myDp) });
            }

        }
        #endregion

        #region 门店列表——弹窗

        public static Result LoadStores(string Code, string Name, string codelist, string dp)
        {
            var myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            try
            {
                using (var db = new CRMEntities())
                {

                    string[] storeList = codelist.Split(',');
                    var query = (from a in db.V_M_TM_SYS_BaseData_store
                                 join b in db.V_M_TM_SYS_BaseData_company
                                 on a.ChannelCodeStore equals b.CompanyCode
                                 into c
                                 from ab in c.DefaultIfEmpty()
                                 select new
                                 {
                                     a.BaseDataID,
                                     a.StoreCode,
                                     a.StoreName,
                                     ab.CompanyName
                                 }).Where(i => !storeList.Contains(i.StoreCode))
                        .Select(i => new
                        {
                            i.BaseDataID,
                            i.StoreCode,
                            i.StoreName,
                            i.CompanyName
                        });

                    if (!string.IsNullOrEmpty(Code)) query = query.Where(i => i.StoreCode.Contains(Code));
                    if (!string.IsNullOrEmpty(Name)) query = query.Where(i => i.StoreName.Contains(Name));
                    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
                }
            }
            catch (Exception)
            {
                return new Result(false, "", new List<object> { new List<dynamic>().ToDataTableSourceVsPage(myDp) });
            }

        }
        #endregion

        #region 根据Id获取购物券派送信息

        public static Result GetCouponSendById(string id)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var details = db.TM_JPOS_CouponPool.Where(p => p.ListId == id);
                    var list = db.TM_JPOS_CouponList.Find(id);
                    string limitvalue = String.Join(",", details.Select(p => p.StoreCode).ToList()) + ",";
                    string codeStr = "";
                    foreach (var item in details)
                    {
                        codeStr += item.StoreCode + ":" + item.SendCount + ",";
                    }
                    var query = new CouponListModel
                    {
                        CouponListID = list.CouponListID,
                        CouponId = list.CouponId,
                        SendCount = list.SendCount,
                        CouponValue = list.CouponValue,
                        ModifiedDate = list.ModifiedDate,
                        ModifiedUser = list.ModifiedUser,
                        Statu = list.Statu,
                        EndDate = list.EndDate,
                        BeginDate = list.BeginDate,
                        OddNumber = list.OddNumber,
                        CouponDesc = list.CouponDesc,
                        CouponInfo = list.CouponInfo,
                        LimitValue = limitvalue,
                        SendedCount = db.TM_JPOS_CouponList.Where(x => x.CouponId == list.CouponId && x.Statu == 1).Sum(p => p.SendCount),
                        codeStr = codeStr
                    };

                    //  query.LimitValue = 
                    return new Result(true, "", query);
                }
            }
            catch
            {
                return new Result(false);
            }
        }
        #endregion

        public static Result LoadCoupon(string CouponName, string dp)
        {
            var myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            try
            {
                using (var db = new CRMEntities())
                {
                    var query = db.TM_JPOS_CouponUseRule
                        .Where(i => i.IsDelete == false && i.IsEnble == true)
                        .Select(i => new
                        {
                            i.ID,
                            i.CouponName,
                            i.CouponValue,
                            i.IsEnble,
                            i.IsMember,
                            i.StartDate,
                            i.EndDate,
                            i.CouponRemark,
                            i.ApproveStatus,
                            i.ExecuteStatus,
                            AppStatus = db.TD_SYS_BizOption.Where(x => x.OptionType == "ApproveStatus" && x.OptionValue == i.ApproveStatus).Select(x => x.OptionText),
                            ExeStatus = db.TD_SYS_BizOption.Where(x => x.OptionType == "ExecuteStatus" && x.OptionValue == i.ExecuteStatus).Select(x => x.OptionText)
                        });

                    if (!string.IsNullOrEmpty(CouponName)) query = query.Where(i => i.CouponName.Contains(CouponName));

                    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
                }
            }
            catch
            {
                return new Result(false, "", new List<object> { new List<CouponSendRuleModel>().ToDataTableSourceVsPage(myDp) });
            }
        }

        #region 根据Id获取购物券信息

        public static Result GetCouponById(int id)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var query = db.TM_JPOS_CouponUseRule.Where(p => p.ID == id).Select(i => new
                    {
                        i.ID,
                        i.CouponName,
                        i.CouponValue,
                        i.IsEnble,
                        i.IsMember,
                        i.StartDate,
                        i.EndDate,
                        i.LogoPath,
                        i.CouponRemark,
                        i.LimitRemark,
                        i.ApproveStatus,
                        i.ExecuteStatus,
                        SendCount = db.TM_JPOS_CouponList.Where(x => x.CouponId == i.ID && x.Statu == 1).Sum(p => p.SendCount),
                        AppStatus = db.TD_SYS_BizOption.Where(x => x.OptionType == "ApproveStatus" && x.OptionValue == i.ApproveStatus).Select(x => x.OptionText),
                        ExeStatus = db.TD_SYS_BizOption.Where(x => x.OptionType == "ExecuteStatus" && x.OptionValue == i.ExecuteStatus).Select(x => x.OptionText)
                    }).FirstOrDefault();
                    return new Result(true, "", query);
                }
            }
            catch
            {
                return new Result(false);
            }
        }
        #endregion

        #region 添加购物券派送

        public static Result AddCouponSend(CouponListModel model, int User)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var dt = DateTime.Now;
                    long oddnumber = 100001;
                    var odd = db.TM_JPOS_CouponList.OrderByDescending(p => p.OddNumber).FirstOrDefault();
                    if (odd != null)
                        oddnumber = odd.OddNumber + 1;
                    TM_JPOS_CouponList list = new TM_JPOS_CouponList();
                    list.CouponListID = Guid.NewGuid().ToString("N");
                    list.AddedDate = DateTime.Now;
                    list.AddedUser = User.ToString();
                    list.ModifiedDate = DateTime.Now;
                    list.ModifiedUser = User.ToString();
                    list.OddNumber = oddnumber;
                    list.Statu = 0;
                    list.SendCount = model.SendCount;
                    list.CouponDesc = model.CouponDesc;
                    list.CouponId = model.CouponId;
                    list.CouponInfo = model.CouponInfo;
                    list.CouponValue = model.CouponValue;
                    list.BeginDate = model.BeginDate;
                    list.EndDate = model.EndDate;

                    db.TM_JPOS_CouponList.Add(list);
                    string[] storeList = model.LimitValue.Split(',');
                    foreach (var item in storeList)
                    {
                        if (!string.IsNullOrWhiteSpace(item))
                        {
                            string[] codeList = item.Split(':');
                            TM_JPOS_CouponPool pool = new TM_JPOS_CouponPool();
                            pool.PoolID = Guid.NewGuid().ToString("N");
                            pool.ListId = list.CouponListID;
                            pool.SendCount = Convert.ToInt32(codeList[1]);
                            pool.SendValue = list.CouponValue * pool.SendCount;
                            pool.StoreCode = codeList[0];
                            pool.StoreName = db.V_M_TM_SYS_BaseData_store.Where(p => p.StoreCode == pool.StoreCode).FirstOrDefault().StoreName;
                            db.TM_JPOS_CouponPool.Add(pool);
                        }

                    }
                    db.SaveChanges();
                    return new Result(true, "添加成功");
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("AddCouponSend:" + ex.ToString());
                return new Result(false, "添加失败");
            }
        }
        #endregion

        #region 编辑购物券派送

        public static Result UpdateCouponSend(CouponListModel model, int User)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var dt = DateTime.Now;
                    var list = db.TM_JPOS_CouponList.Find(model.CouponListID);
                    list.ModifiedDate = DateTime.Now;
                    list.ModifiedUser = User.ToString();
                    list.SendCount = model.SendCount;
                    list.CouponDesc = model.CouponDesc;
                    list.CouponId = model.CouponId;
                    list.CouponInfo = model.CouponInfo;
                    list.CouponValue = model.CouponValue;
                    list.BeginDate = model.BeginDate;
                    list.EndDate = model.EndDate;

                    var pools = db.TM_JPOS_CouponPool.Where(p => p.ListId == model.CouponListID).ToList();
                    foreach (var item in pools)
                    {
                        db.TM_JPOS_CouponPool.Remove(item);
                    }
                    string[] storeList = model.LimitValue.Split(',');
                    foreach (var item in storeList)
                    {
                        if (!string.IsNullOrWhiteSpace(item))
                        {
                            string[] codeList = item.Split(':');
                            var code = codeList[0];

                            TM_JPOS_CouponPool pool = new TM_JPOS_CouponPool();
                            pool.PoolID = Guid.NewGuid().ToString("N");
                            pool.ListId = list.CouponListID;
                            pool.SendCount = Convert.ToInt32(codeList[1]);
                            pool.SendValue = list.CouponValue * pool.SendCount;
                            pool.StoreCode = code;
                            pool.StoreName = db.V_M_TM_SYS_BaseData_store.Where(p => p.StoreCode == pool.StoreCode).FirstOrDefault().StoreName;
                            db.TM_JPOS_CouponPool.Add(pool);

                        }

                    }
                    db.SaveChanges();
                    return new Result(true, "编辑成功");

                }
            }
            catch
            {

                return new Result(false, "编辑失败");
            }
        }


        #endregion

    }
}
