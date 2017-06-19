using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Objects.SqlClient;

namespace Arvato.CRM.BizLogic
{
    public static class CouponUseRule
    {
        #region 公共方法

        #region 商品列表

        public static Result LoadProduct(string productArr, string dp)
        {
            var myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            try
            {
                string[] productList = productArr.Split(',');
                using (var db = new CRMEntities())
                {
                    var query = db.TD_Sys_Product
                        .Where(i => productList.Contains(i.GoodsCode))
                        .Select(i => new
                        {
                            i.BaseDataID,
                            i.GoodsCode,
                            i.GoodsName,
                            i.GoodsSort,
                            i.GoodsBrand
                        });
                    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
                }
            }
            catch (Exception)
            {
                return new Result(false, "", new List<object> { new List<dynamic>().ToDataTableSourceVsPage(myDp) });
            }

        }

        #endregion

        #region 判断该产品是否存在

        public static Result CheckGoodsCode(string GoodsCode)
        {
            using (var db = new CRMEntities())
            {
                var queryCount = db.TD_Sys_Product.Count(i => i.GoodsCode == GoodsCode);
                if (queryCount > 0)
                {
                    return new Result(true, "添加成功");
                }
                return new Result(false, "该产品不存在");
            }
        }

        #endregion

        #region 商品列表——弹窗

        public static Result LoadProducts(string Code, string Name, string dp)
        {
            var myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            try
            {
                using (var db = new CRMEntities())
                {
                    var query = db.TD_Sys_Product
                        .Select(i => new
                        {
                            i.BaseDataID,
                            i.GoodsCode,
                            i.GoodsName,
                            i.GoodsSort,
                            i.GoodsBrand
                        });
                    if (!string.IsNullOrEmpty(Code)) query = query.Where(i => i.GoodsCode.Contains(Code));
                    if (!string.IsNullOrEmpty(Name)) query = query.Where(i => i.GoodsName.Contains(Name));
                    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
                }
            }
            catch (Exception)
            {
                return new Result(false, "", new List<object> { new List<dynamic>().ToDataTableSourceVsPage(myDp) });
            }

        }
        #endregion

        #endregion

        #region 购物券使用规则

        #region 购物券列表
        public static Result LoadCouponUseRule(CouponUseRuleModel model, string dp)
        {
            var myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            try
            {
                using (var db = new CRMEntities())
                {
                    var query = db.TM_JPOS_CouponUseRule
                        .Where(i => i.IsDelete == false)
                        .OrderBy(i => i.CouponSort)
                        .Select(i => new
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
                            AppStatus = db.TD_SYS_BizOption.Where(x => x.OptionType == "ApproveStatus" && x.OptionValue == i.ApproveStatus).Select(x => x.OptionText),
                            ExeStatus = db.TD_SYS_BizOption.Where(x => x.OptionType == "ExecuteStatus" && x.OptionValue == i.ExecuteStatus).Select(x => x.OptionText)
                        });
                    if (model.ID != null) query = query.Where(i => i.ID == model.ID);
                    if (!string.IsNullOrEmpty(model.CouponName)) query = query.Where(i => i.CouponName.Contains(model.CouponName));
                    if (model.CouponValue != null) query = query.Where(i => i.CouponValue == model.CouponValue);
                    if (!string.IsNullOrEmpty(model.CouponRemark)) query = query.Where(i => i.CouponRemark.Contains(model.CouponRemark));
                    if (model.StartDate != null && model.EndDate == null) query = query.Where(i => i.StartDate == model.StartDate);
                    if (model.StartDate != null && model.EndDate != null) query = query.Where(i => i.StartDate >= model.StartDate && i.EndDate <= model.EndDate);
                    if (model.StartDate == null && model.EndDate != null) query = query.Where(i => i.EndDate == model.StartDate);
                    if (!string.IsNullOrEmpty(model.ApproveStatus)) query = query.Where(i => i.ApproveStatus == model.ApproveStatus);
                    if (!string.IsNullOrEmpty(model.ExecuteStatus)) query = query.Where(i => i.ExecuteStatus == model.ExecuteStatus);
                    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
                }
            }
            catch
            {
                return new Result(false, "", new List<object> { new List<CouponUseRuleModel>().ToDataTableSourceVsPage(myDp) });
            }
        }
        #endregion

        #region 添加购物券

        public static Result AddCoupon(CouponUseRuleModel model, int User)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var dt = DateTime.Now;
                    TM_JPOS_CouponUseRule query = new TM_JPOS_CouponUseRule();
                    query.CouponName = model.CouponName;
                    query.CouponValue = model.CouponValue;
                    query.CouponRemark = model.CouponRemark;
                    query.CouponSort = model.CouponSort;
                    query.LimitValue = model.LimitValue;
                    query.LogoPath = model.LogoPath;
                    query.LimitRemark = model.LimitRemark;
                    query.IsMember = model.IsMember;
                    query.StartDate = model.StartDate;
                    query.EndDate = model.EndDate;
                    query.IsDelete = false;
                    query.IsEnble = true;
                    query.AddedUser = User.ToString();
                    query.AddedDate = dt;
                    query.ModifiedUser = User.ToString();
                    query.ModifiedDate = dt;
                    query.ApproveStatus = "0";
                    query.ExecuteStatus = "0";
                    db.TM_JPOS_CouponUseRule.Add(query);
                    db.SaveChanges();
                    return new Result(true, "添加成功");
                }
            }
            catch
            {
                return new Result(false, "添加失败");
            }
        }
        #endregion

        #region 根据Id获取购物券信息

        public static Result GetCouponById(int id)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var query = db.TM_JPOS_CouponUseRule.Find(id);
                    return new Result(true, "", query);
                }
            }
            catch
            {
                return new Result(false);
            }
        }
        #endregion

        #region 编辑购物券

        public static Result UpdateCoupon(CouponUseRuleModel model, int User)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var dt = DateTime.Now;
                    var query = db.TM_JPOS_CouponUseRule.Find(model.ID);
                    query.CouponName = model.CouponName;
                    query.CouponValue = model.CouponValue;
                    query.CouponRemark = model.CouponRemark;
                    query.CouponSort = model.CouponSort;
                    query.LimitValue = model.LimitValue;
                    query.LogoPath = model.LogoPath;
                    query.LimitRemark = model.LimitRemark;
                    query.IsMember = model.IsMember;
                    query.StartDate = model.StartDate;
                    query.EndDate = model.EndDate;
                    query.ModifiedUser = User.ToString();
                    query.ModifiedDate = dt;
                    if (query.StartDate > DateTime.Today)
                        query.ExecuteStatus = "0";
                    else if (query.EndDate <= DateTime.Today)
                        query.ExecuteStatus = "3";
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

        #region 改变购物券的执行状态

        public static Result ChangeCouponStatus(int Id, int User)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var dt = DateTime.Now;
                    var query = db.TM_JPOS_CouponUseRule.Find(Id);
                    var msg = "休眠";
                    if (!(bool)query.IsEnble)
                        msg = "唤醒";
                    if ((bool)query.IsEnble)
                        query.ExecuteStatus = "1";
                    else if (query.StartDate > DateTime.Today)
                        query.ExecuteStatus = "0";
                    else if (query.EndDate >= DateTime.Today)
                        query.ExecuteStatus = "2";
                    else
                        query.ExecuteStatus = "3";
                    query.IsEnble = !query.IsEnble;
                    query.ModifiedDate = dt;
                    query.ModifiedUser = User.ToString();
                    db.SaveChanges();
                    return new Result(true, msg + "成功");
                }
            }
            catch
            {
                return new Result(false, "操作失败：数据不存在");
            }
        }
        #endregion

        #region 删除购物券券

        public static Result DeleteCouponById(int Id, int User)
        {
            try
            {

                using (var db = new CRMEntities())
                {
                    var dt = DateTime.Now;
                    var query = db.TM_JPOS_CouponUseRule.Find(Id);
                    query.IsDelete = true;
                    query.ModifiedDate = dt;
                    query.ModifiedUser = User.ToString();
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

        #region 改变购物券的审核状态
        public static Result ApproveCouponById(int Id, string active, int User)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var dt = DateTime.Now;
                    var query = db.TM_JPOS_CouponUseRule.Find(Id);
                    query.ApproveStatus = active;
                    if (!(bool)query.IsEnble)
                        query.ExecuteStatus = "1";
                    else if (query.StartDate > DateTime.Today)
                        query.ExecuteStatus = "0";
                    else if (query.EndDate >= DateTime.Today)
                        query.ExecuteStatus = "2";
                    else
                        query.ExecuteStatus = "3";
                    if (active == "2")
                    {
                        query.ExecuteStatus = "3";
                    }
                    query.ModifiedDate = dt;
                    query.ModifiedUser = User.ToString();
                    db.SaveChanges();
                    var msg = "审核成功";
                    if (active == "2")
                        msg = "作废成功";
                    return new Result(true, msg);
                }
            }
            catch
            {
                return new Result(false, "操作失败：数据不存在");
            }
        }

        #endregion



        #endregion

        #region 购物券派送规则

        #region 购物券列表
        public static Result LoadCouponSendRule(CouponSendRuleModel model, string dp)
        {
            var myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            try
            {
                using (var db = new CRMEntities())
                {
                    var query = db.TM_JPOS_CouponSendRule
                        .Where(i => i.IsDelete == false)
                        .Select(i => new
                        {
                            i.ID,
                            i.CouponNo,
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
                    if (model.ID != null) query = query.Where(i => i.CouponNo == model.ID);
                    if (!string.IsNullOrEmpty(model.CouponName)) query = query.Where(i => i.CouponName.Contains(model.CouponName));
                    if (model.CouponValue != null) query = query.Where(i => i.CouponValue == model.CouponValue);
                    if (!string.IsNullOrEmpty(model.CouponRemark)) query = query.Where(i => i.CouponRemark.Contains(model.CouponRemark));
                    if (model.StartDate != null && model.EndDate == null) query = query.Where(i => i.StartDate == model.StartDate);
                    if (model.StartDate != null && model.EndDate != null) query = query.Where(i => i.StartDate >= model.StartDate && i.EndDate <= model.EndDate);
                    if (model.StartDate == null && model.EndDate != null) query = query.Where(i => i.EndDate == model.StartDate);
                    if (!string.IsNullOrEmpty(model.ApproveStatus)) query = query.Where(i => i.ApproveStatus == model.ApproveStatus);
                    if (!string.IsNullOrEmpty(model.ExecuteStatus)) query = query.Where(i => i.ExecuteStatus == model.ExecuteStatus);
                    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
                }
            }
            catch
            {
                return new Result(false, "", new List<object> { new List<CouponSendRuleModel>().ToDataTableSourceVsPage(myDp) });
            }
        }
        #endregion

        #region 添加购物券

        public static Result AddCouponSendRule(CouponSendRuleModel model, int User)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var dt = DateTime.Now;
                    TM_JPOS_CouponSendRule query = new TM_JPOS_CouponSendRule();
                    query.CouponNo = model.CouponNo;
                    query.CouponName = model.CouponName;
                    query.CouponValue = model.CouponValue;
                    query.CouponRemark = model.CouponRemark;
                    query.LimitValue = model.LimitValue;
                    query.IsMember = model.IsMember;
                    query.StartDate = model.StartDate;
                    query.EndDate = model.EndDate;
                    query.IsDelete = false;
                    query.IsEnble = true;
                    query.AddedUser = User.ToString();
                    query.AddedDate = dt;
                    query.ModifiedUser = User.ToString();
                    query.ModifiedDate = dt;
                    query.ApproveStatus = "0";
                    query.ExecuteStatus = "0";
                    db.TM_JPOS_CouponSendRule.Add(query);
                    db.SaveChanges();
                    return new Result(true, "添加成功");
                }
            }
            catch
            {
                return new Result(false, "添加失败");
            }
        }
        #endregion

        #region 根据Id获取购物券信息

        public static Result GetCouponSendRuleById(int id)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var query = db.TM_JPOS_CouponSendRule.Find(id);
                    return new Result(true, "", query);
                }
            }
            catch
            {
                return new Result(false);
            }
        }
        #endregion

        #region 编辑购物券

        public static Result UpdateCouponSendRule(CouponSendRuleModel model, int User)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var dt = DateTime.Now;
                    var query = db.TM_JPOS_CouponSendRule.Find(model.ID);
                    query.CouponNo = model.CouponNo;
                    query.CouponName = model.CouponName;
                    query.CouponValue = model.CouponValue;
                    query.CouponRemark = model.CouponRemark;
                    query.IsMember = model.IsMember;
                    query.LimitValue = model.LimitValue;
                    query.StartDate = model.StartDate;
                    query.EndDate = model.EndDate;
                    query.ModifiedUser = User.ToString();
                    query.ModifiedDate = dt;
                    if (query.StartDate > DateTime.Today)
                        query.ExecuteStatus = "0";
                    else if (query.EndDate <= DateTime.Today)
                        query.ExecuteStatus = "3";
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

        #region 改变购物券的执行状态

        public static Result ChangeCouponSendRuleStatus(int Id, int User)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var dt = DateTime.Now;
                    var query = db.TM_JPOS_CouponSendRule.Find(Id);
                    var msg = "休眠";
                    if (!(bool)query.IsEnble)
                        msg = "唤醒";
                    if ((bool)query.IsEnble)
                        query.ExecuteStatus = "1";
                    else if (query.StartDate > DateTime.Today)
                        query.ExecuteStatus = "0";
                    else if (query.EndDate >= DateTime.Today)
                        query.ExecuteStatus = "2";
                    else
                        query.ExecuteStatus = "3";
                    query.IsEnble = !query.IsEnble;
                    query.ModifiedDate = dt;
                    query.ModifiedUser = User.ToString();
                    db.SaveChanges();
                    return new Result(true, msg + "成功");
                }
            }
            catch
            {
                return new Result(false, "操作失败：数据不存在");
            }
        }
        #endregion

        #region 删除购物券

        public static Result DeleteCouponSendRuleById(int Id, int User)
        {
            try
            {

                using (var db = new CRMEntities())
                {
                    var dt = DateTime.Now;
                    var query = db.TM_JPOS_CouponSendRule.Find(Id);
                    query.IsDelete = true;
                    query.ModifiedDate = dt;
                    query.ModifiedUser = User.ToString();
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

        #region 改变购物券的审核状态
        public static Result ApproveCouponSendById(int Id, string active, int User)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var dt = DateTime.Now;
                    var query = db.TM_JPOS_CouponSendRule.Find(Id);
                    query.ApproveStatus = active;
                    if (!(bool)query.IsEnble)
                        query.ExecuteStatus = "1";
                    else if (query.StartDate > DateTime.Today)
                        query.ExecuteStatus = "0";
                    else if (query.EndDate >= DateTime.Today)
                        query.ExecuteStatus = "2";
                    else
                        query.ExecuteStatus = "3";
                    if (active == "2")
                    {
                        query.ExecuteStatus = "3";
                    }
                    query.ModifiedDate = dt;
                    query.ModifiedUser = User.ToString();
                    db.SaveChanges();
                    var msg = "审核成功";
                    if (active == "2")
                        msg = "作废成功";
                    return new Result(true, msg);
                }
            }
            catch
            {
                return new Result(false, "操作失败：数据不存在");
            }
        }

        #endregion

        #endregion
    }
}
