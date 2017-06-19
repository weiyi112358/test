using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.BizLogic
{
    public class CreditTransfer
    {
        #region 积分转让列表
        public static Result GetCreditTransfers(string dp, int dataGroupId, string name, string mobile, DateTime? start, DateTime? end)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from c in db.TL_Mem_AccountChange
                            join d1 in db.TM_Mem_AccountDetail on c.AccountDetailID equals d1.AccountDetailID
                            join a1 in db.TM_Mem_Account on c.AccountID equals a1.AccountID
                            join u1 in db.V_U_TM_Mem_Info on a1.MemberID equals u1.MemberID
                            join a2 in db.TM_Mem_Account on c.ReferenceNo equals a2.AccountID
                            join u2 in db.V_U_TM_Mem_Info on a2.MemberID equals u2.MemberID
                            where c.AccountChangeType == "transfer-to" && u1.DataGroupID == dataGroupId
                            orderby c.AddedDate descending
                            select new
                            {
                                c.LogID,
                                c.ChangeValue,
                                UsedEndDate = d1.SpecialDate2,
                                c.AddedDate,
                                ToMemberId = u1.MemberID,
                                ToName = u1.CustomerName,
                                ToMobile = u1.CustomerMobile,
                                ToAccountId = c.AccountID,
                                ToDetailId = c.AccountDetailID,
                                FromMemberId = u2.MemberID,
                                FromName = u2.CustomerName,
                                FromMobile = u2.CustomerMobile,
                                FromAccountId = a2.AccountID
                            };
                if (!string.IsNullOrWhiteSpace(name))
                {
                    query = query.Where(p => p.ToName.Contains(name) || p.FromName.Contains(name));
                }
                if (!string.IsNullOrWhiteSpace(mobile))
                {
                    query = query.Where(p => p.ToMobile.Contains(mobile) || p.FromMobile.Contains(mobile));
                }
                if (start != null)
                {
                    query = query.Where(p => p.AddedDate >= start);
                }

                if (end != null)
                {
                    end = end.Value.AddDays(1);
                    query = query.Where(p => p.AddedDate <= end);
                }
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }
        #endregion

        #region 积分转让
        public static Result GetMembersForCredit(string dp, int dataGroupId, string pageIds, string dataRoleIds, string mobile, string certificate, string vehicleNo, string vin)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
                //var query = db.sp_CRM_Mem_Search(dataGroupId, pageIds, dataRoleIds, "1",
                //    null, null, mobile, null, vehicleNo, null, null, null, null, null, null, "",
                //    0, 0, recordCount).ToList();
                ////var query = from u in db.V_U_TM_Mem_Info
                ////            where u.DataGroupID == dataGroupId && u.CustomerStatus == "1"
                ////            orderby u.AddedDate
                ////            select new
                ////            {
                ////                u.MemberID,
                ////                u.CustomerName,
                ////                u.MemberCardNo,
                ////                u.CustomerMobile,
                ////                u.CertificateNo,
                ////                u.AddedDate
                ////            };
                ////if (!string.IsNullOrWhiteSpace(mobile))
                ////{
                ////    query = query.Where(p => p.CustomerMobile.Contains(mobile));
                ////}
                //if (!string.IsNullOrWhiteSpace(certificate))
                //{
                //    //query = query.Where(p => p.CertificateNoKey.Contains(certificate)).ToList();
                //}
                //if (!string.IsNullOrWhiteSpace(vin))
                //{
                //    query = query.Where(p => p.VIN.Contains(vin)).ToList();
                //}

                //var q = query.Skip(myDp.iDisplayStart / myDp.iDisplayLength).Take(myDp.iDisplayLength).ToList();
                //var res = new DatatablesSourceVsPage();
                //res.iDisplayStart = myDp.iDisplayStart;
                //res.iDisplayLength = myDp.iDisplayLength;
                //res.iTotalRecords = query.Count();
                //res.aaData = q;
                return new Result(true, "");
            }
        }

        public static Result GetCreditSummary(string memberId)
        {
            using (var db = new CRMEntities())
            {
                var account = (from a in db.TM_Mem_Account
                               where a.MemberID == memberId && a.AccountType == "2"
                               select a).FirstOrDefault();
                decimal valid = 0, total = 0;
                if (account != null)
                {
                    valid = account.Value1;
                    total = account.Value1 + account.Value2;
                }

                return new Result(true, "", new List<object> { valid, total });
            }
        }

        public static Result GetCreditDetails(string memberId)
        {
            using (var db = new CRMEntities())
            {
                var account = (from a in db.TM_Mem_Account
                               where a.MemberID == memberId && a.AccountType == "2"
                               select a).FirstOrDefault();
                if (account == null)
                {
                    return new Result(true, "", new List<object> { new List<object>() });
                }

                var query1 = (from l in db.TM_Mem_AccountLimit
                              join o in db.TD_SYS_BizOption.Where(o => o.OptionType == "AccountLimitType") on l.LimitType equals o.OptionValue
                              //join vtemp in db.V_M_TM_Mem_SubExt_vehicle on l.LimitValue equals SqlFunctions.StringConvert((double)vtemp.MemberSubExtID).Trim()
                              into vt
                              from v in vt.DefaultIfEmpty()
                              where l.AccountID == account.AccountID && l.LimitType == "vehicle"
                              orderby l.AccountLimitID
                              select new
                              {
                                  l.AccountLimitID,
                                  l.AccountID,
                                  l.AccountDetailID,
                                  LimitType = "",
                                  LimitValue = l.LimitValue//(v.CarNo ?? "") + "," + (v.VIN ?? "")
                              })
                              .Union(from l in db.TM_Mem_AccountLimit
                                     join o in db.TD_SYS_BizOption.Where(o => o.OptionType == "AccountLimitType") on l.LimitType equals o.OptionValue
                                     join vtemp in db.V_M_TM_SYS_BaseData_store on l.LimitValue equals vtemp.StoreCode
                                     into vt
                                     from v in vt.DefaultIfEmpty()
                                     where l.AccountID == account.AccountID && l.LimitType == "store"
                                     orderby l.AccountLimitID
                                     select new
                                     {
                                         l.AccountLimitID,
                                         l.AccountID,
                                         l.AccountDetailID,
                                         LimitType = o.OptionText,
                                         LimitValue = v.StoreName
                                     })
                              .Union(from l in db.TM_Mem_AccountLimit
                                     //join a in db.TM_Mem_Account on l.AccountID equals a.AccountID
                                     join o in db.TD_SYS_BizOption.Where(o => o.OptionType == "AccountLimitType") on l.LimitType equals o.OptionValue
                                     join vtemp in db.V_M_TM_SYS_BaseData_brand on l.LimitValue equals vtemp.BrandCodeBase
                                     into vt
                                     from v in vt.DefaultIfEmpty()
                                     where l.AccountID == account.AccountID && l.LimitType == "brand"
                                     orderby l.AccountLimitID
                                     select new
                                     {
                                         l.AccountLimitID,
                                         l.AccountID,
                                         l.AccountDetailID,
                                         LimitType = o.OptionText,
                                         LimitValue = v.BrandNameBase
                                     })
                              .ToList();
                List<AccountLimit> limits = new List<AccountLimit>();
                //limits.Add(new AccountLimit { AccountID = "", LimitText = "" });
                foreach (var detailId in query1.Select(o => o.AccountDetailID).Distinct())
                {
                    if (detailId != null)
                    {
                        string str = "";
                        foreach (var l in query1.Where(o => o.AccountDetailID == detailId))
                        {
                            str += l.LimitType + ":" + l.LimitValue + "; ";
                        }
                        limits.Add(new AccountLimit { ActDetailID = detailId, LimitText = str });
                    }
                }

                var query2 = (from d in db.TM_Mem_AccountDetail
                              //join o in db.TD_SYS_BizOption on d.AccountDetailType equals o.OptionValue
                              where d.AccountID == account.AccountID && d.AccountDetailType == "value1" && (d.SpecialDate2 == null || d.SpecialDate2 >= DateTime.Now) && d.DetailValue > 0//&& o.OptionType == "AccountDetailType"
                              //orderby a.AccountID, o.Sort, d.AccountDetailID
                              orderby d.AccountDetailID
                              select d).ToList();
                var query = from a in
                                (from q in query2
                                 join ltemp in limits on q.AccountDetailID equals ltemp.ActDetailID into lt
                                 from l in lt.DefaultIfEmpty()
                                 select new
                                 {
                                     q.AccountID,
                                     //q.AccountDetailType,
                                     //q.DetailTypeText,
                                     //q.Value1,
                                     //q.Value2,
                                     q.AccountDetailID,
                                     q.DetailValue,
                                     q.SpecialDate1,
                                     q.SpecialDate2,
                                     AccountLimit = l == null ? "" : l.LimitText ?? ""
                                 })
                            group a by new { a.SpecialDate2, a.AccountLimit } into g
                            orderby g.Key.SpecialDate2
                            select new
                            {
                                AccountDetailIds = g.Select(p => p.AccountDetailID).ToList(),
                                EndDate = g.Key.SpecialDate2,
                                AccountLimit = g.Key.AccountLimit,
                                ValidValue = g.Sum(o => o.DetailValue)
                            };
                return new Result(true, "", new List<object> { query.ToList() });
                //return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }
        //public static Result GetMemActDetails(string mid)
        //{
        //    using (var db = new CRMEntities())
        //    {
        //        //拿到账户id
        //        var q1 = db.TM_Mem_Account.Where(p => p.MemberID == mid && p.AccountType == "2").FirstOrDefault();
        //        if (q1 != null)
        //        {
        //            var actId = q1.AccountID;
        //            List<AccountLimit> limits = new List<AccountLimit>();
        //            //根据账户id获取明细id列表
        //            var q2 = db.TM_Mem_AccountDetail.Where(p => p.AccountID == actId && p.DetailValue > 0).ToList();
        //            if (q2 != null)
        //            {
        //                long detailId;
        //                for (int i = 0; i < q2.Count; i++)
        //                {
        //                    detailId = q2[i].AccountDetailID;
        //                    //获取限制列表
        //                    // var q3 = db.TM_Mem_AccountLimit.Where(p => p.AccountDetailID == detailId).ToList();
        //                    var q3 = (from a in db.TM_Mem_AccountLimit
        //                              join b in db.V_M_TM_SYS_BaseData_store on a.LimitValue equals b.StoreCode into ba
        //                              from baa in ba.DefaultIfEmpty()
        //                              join c in db.V_M_TM_SYS_BaseData_brand on a.LimitValue equals c.BrandCodeBase into ca
        //                              from caa in ca.DefaultIfEmpty()
        //                              join o in db.TD_SYS_BizOption on a.LimitType equals o.OptionValue
        //                              where a.AccountDetailID == detailId && o.OptionType == "AccountLimitType"
        //                              orderby a.AccountLimitID
        //                              select new
        //                              {
        //                                  a.LimitValue,
        //                                  a.LimitType,
        //                                  LimitTypeText = o.OptionText,
        //                                  StoreName = baa.StoreName,
        //                                  caa.BrandNameBase,
        //                              }).ToList();

        //                    string str = "";
        //                    foreach (var item in q3)
        //                    {
        //                        if (item.LimitType == "store")
        //                        {
        //                            str += item.LimitTypeText + ":" + item.StoreName + "; ";
        //                        }
        //                        else if (item.LimitType == "brand")
        //                        {
        //                            str += item.LimitTypeText + ":" + item.BrandNameBase + "; ";
        //                        }
        //                        else
        //                        {
        //                            str += item.LimitTypeText + ":" + item.LimitValue + "; ";
        //                        }
        //                    }
        //                    limits.Add(new AccountLimit { ActDetailID = detailId, LimitText = str });
        //                }

        //            }

        //            var query = from q in q2
        //                        join ltemp in limits on q.AccountDetailID equals ltemp.ActDetailID into lt
        //                        from l in lt.DefaultIfEmpty()
        //                        join o in db.TD_SYS_BizOption on q.AccountDetailType equals o.OptionValue
        //                        where o.OptionType == "AccountDetailType" && o.Enable == true
        //                        select new
        //                        {
        //                            q.AccountID,
        //                            q.AccountDetailType,
        //                            DetailTypeText = o.OptionText,
        //                            AccountDetailIds = q.AccountDetailID,
        //                            ValidValue = q.DetailValue,
        //                            q.SpecialDate1,
        //                            EndDate = q.SpecialDate2,
        //                            AccountLimit = l == null ? "" : l.LimitText ?? ""
        //                        };
        //            return new Result(true, "", new List<object> { query.ToList() });
        //        }
        //        return new Result(false, "没有账户");
        //    }
        //}
        public static Result CreateCreditTransfer(string fromMemId, string toMemId, string detailsStr, int dataGroupId, string modifiedBy)
        {
            try
            {
                var changes = Util.Deserialize<List<CreditTransferDetail>>(detailsStr);
                using (var db = new CRMEntities())
                {
                    db.BeginTransaction();
                    var from = db.TM_Mem_Account.Where(p => p.MemberID == fromMemId && p.AccountType == "2").FirstOrDefault();
                    if (from == null)
                    {
                        return new Result(false, "找不到转出账户");
                    }
                    var to = db.TM_Mem_Account.Where(p => p.MemberID == toMemId && p.AccountType == "2").FirstOrDefault();
                    if (to == null)
                    {
                        //转入会员如果没有积点账户，则创建
                        to = new TM_Mem_Account();
                        to.AccountID = Guid.NewGuid().ToString().Replace("-", "");
                        to.AccountType = "2";
                        to.MemberID = toMemId;
                        to.Value1 = 0;
                        to.Value2 = 0;
                        to.Value3 = 0;
                        to.AddedDate = DateTime.Now;
                        to.AddedUser = modifiedBy;
                        to.ModifiedDate = DateTime.Now;
                        to.ModifiedUser = modifiedBy;
                        db.TM_Mem_Account.Add(to);
                        db.SaveChanges();
                    }
                    var limits = db.TM_Mem_AccountLimit.Where(p => p.AccountID == from.AccountID && p.AccountDetailID != null).ToList();
                    var details = db.TM_Mem_AccountDetail.Where(p => p.AccountID == from.AccountID && p.AccountDetailType == "value1").ToList();

                    decimal transferTotalFrom = 0;
                    decimal transferTotalTo = 0;
                    decimal temp = 0;
                    //查询转让比例
                    double rate = 1;
                    //var rateModel = db.TL_SYS_TransferRate.Where(p => p.DataGroupId == dataGroupId).FirstOrDefault();
                    //if (rateModel != null)
                    //{
                    //    rate = rateModel.RateValue / 100;
                    //}

                    foreach (var change in changes)
                    {
                        decimal transferValueFrom = change.TransferValue;
                        decimal transferValueTo = Math.Round(transferValueFrom * (decimal)rate, 4);
                        temp = transferValueTo;
                        if (transferValueFrom <= 0)
                        {
                            break;
                        }
                        var ds = details.Where(p => change.AccountDetailIds.Contains(p.AccountDetailID)).OrderBy(p => p.SpecialDate1).ToList();

                        transferTotalFrom += transferValueFrom;
                        if (transferTotalFrom > from.Value1 || ds.Count < 1 || transferValueFrom > ds.Sum(p => p.DetailValue))
                        {
                            return new Result(false, "转出账户积点余额不足");
                        }

                        transferTotalTo += transferValueTo;
                        //转入AccountDetail插入
                        var toDetail = new TM_Mem_AccountDetail();
                        toDetail.AccountID = to.AccountID;
                        toDetail.AccountDetailType = "value1";
                        toDetail.DetailValue = transferValueTo;
                        toDetail.SpecialDate1 = ds.Last().SpecialDate1;
                        toDetail.DetailValue = transferValueTo;
                        toDetail.AddedDate = DateTime.Now;
                        toDetail.AddedUser = modifiedBy;
                        toDetail.ModifiedDate = DateTime.Now;
                        toDetail.ModifiedUser = modifiedBy;
                        db.TM_Mem_AccountDetail.Add(toDetail);
                        db.SaveChanges();

                        //转入AccountDetail日志
                        TL_Mem_AccountChange toC = new TL_Mem_AccountChange();
                        toC.AccountID = to.AccountID;
                        toC.AccountDetailID = toDetail.AccountDetailID;
                        toC.AccountChangeType = "transfer-to";
                        toC.ChangeValue = temp;
                        toC.ChangeReason = "积点转让-转入";
                        toC.ReferenceNo = from.AccountID;//转出账号的AccountID
                        toC.AddedDate = DateTime.Now;
                        toC.AddedUser = modifiedBy;
                        db.TL_Mem_AccountChange.Add(toC);
                        db.SaveChanges();

                        var ls = limits.Where(p => change.AccountDetailIds.Contains(p.AccountDetailID)).Select(p => new { p.LimitType, p.LimitValue }).Distinct();
                        //转入AccountDetail限制
                        foreach (var l in ls)
                        {
                            var toL = new TM_Mem_AccountLimit();
                            toL.AccountID = toC.AccountID;
                            toL.AccountDetailID = toC.AccountDetailID;
                            toL.LimitType = l.LimitType;
                            toL.LimitValue = l.LimitValue;
                            toL.AddedDate = DateTime.Now;
                            db.TM_Mem_AccountLimit.Add(toL);
                        }

                        foreach (var d in ds)
                        {
                            //转出AccountDetail日志
                            TL_Mem_AccountChange c = new TL_Mem_AccountChange();
                            c.AccountID = d.AccountID;
                            c.AccountDetailID = d.AccountDetailID;
                            c.AccountChangeType = "transfer-from";
                            c.ChangeReason = "积点转让-转出";
                            c.ReferenceNo = toC.LogID.ToString();//转入log的LogID
                            c.AddedDate = DateTime.Now;
                            c.AddedUser = modifiedBy;

                            if (d.DetailValue <= transferValueFrom)
                            {
                                transferValueFrom = transferValueFrom - d.DetailValue;
                                c.ChangeValue = 0 - d.DetailValue;
                                d.DetailValue = 0;
                            }
                            else
                            {
                                c.ChangeValue = 0 - transferValueFrom;
                                d.DetailValue = d.DetailValue - transferValueFrom;
                                transferValueFrom = 0;
                            }

                            db.TL_Mem_AccountChange.Add(c);

                            //转出AccountDetail日志
                            d.ModifiedDate = DateTime.Now;
                            d.ModifiedUser = modifiedBy;
                            db.Entry(d).State = EntityState.Modified;

                            if (transferValueFrom <= 0)
                            {
                                break;
                            }
                        }
                    }

                    //转入Account数据增加
                    to.Value1 += transferTotalTo;
                    to.ModifiedDate = DateTime.Now;
                    to.ModifiedUser = modifiedBy;
                    db.Entry(to).State = EntityState.Modified;

                    //转出Account数据扣除
                    from.Value1 -= transferTotalFrom;
                    from.ModifiedDate = DateTime.Now;
                    from.ModifiedUser = modifiedBy;
                    db.Entry(from).State = EntityState.Modified;

                    db.SaveChanges();
                    db.Commit();
                    Service.SendMsgPointTrans(fromMemId, toMemId, DateTime.Now, transferTotalFrom, dataGroupId);
                    return new Result(true);
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        //判断两个数组是否相等
        public static bool compareArr(string arr1, string arr2)
        {
            if (string.IsNullOrEmpty(arr1) && string.IsNullOrEmpty(arr2)) return true;
            if (string.IsNullOrEmpty(arr1) || string.IsNullOrEmpty(arr2)) return false;
            string[] arr11 = arr1.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] arr22 = arr2.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var q = from a in arr11 join b in arr22 on a equals b select a;
            bool flag = arr11.Length == arr22.Length && q.Count() == arr11.Length;
            return flag;//内容相同返回true,反之返回false。
        }
        //public static Result CreateCreditTransfer1(string fromMemId, string toMemId, string detailsStr, int dataGroupId, string modifiedBy)
        //{
        //    try
        //    {
        //        var changes = Util.Deserialize<List<CreditTransferDetail>>(detailsStr);
        //        using (var db = new CRMEntities())
        //        {
        //            db.BeginTransaction();
        //            var from = db.TM_Mem_Account.Where(p => p.MemberID == fromMemId && p.AccountType == "2").FirstOrDefault();
        //            if (from == null)
        //            {
        //                return new Result(false, "找不到转出账户");
        //            }
        //            var to = db.TM_Mem_Account.Where(p => p.MemberID == toMemId && p.AccountType == "2").FirstOrDefault();
        //            if (to == null)
        //            {
        //                //转入会员如果没有积点账户，则创建
        //                to = new TM_Mem_Account();
        //                to.AccountID = Guid.NewGuid().ToString().Replace("-", "");
        //                to.AccountType = "2";
        //                to.MemberID = toMemId;
        //                to.Value1 = 0;
        //                to.Value2 = 0;
        //                to.Value3 = 0;
        //                to.AddedDate = DateTime.Now;
        //                to.AddedUser = modifiedBy;
        //                to.ModifiedDate = DateTime.Now;
        //                to.ModifiedUser = modifiedBy;
        //                db.TM_Mem_Account.Add(to);
        //                db.SaveChanges();
        //            }
        //            //转出账户的限制
        //            var limits = db.TM_Mem_AccountLimit.Where(p => p.AccountID == from.AccountID && p.AccountDetailID != null).ToList();
        //            //转出账户明细表
        //            var details = db.TM_Mem_AccountDetail.Where(p => p.AccountID == from.AccountID && p.AccountDetailType == "value1").ToList();

        //            decimal transferTotalFrom = 0;
        //            decimal transferTotalTo = 0;
        //            decimal temp = 0;
        //            //查询转让比例
        //            double rate = 1;
        //            //var rateModel = db.TL_SYS_TransferRate.Where(p => p.DataGroupId == dataGroupId).FirstOrDefault();
        //            //if (rateModel != null)
        //            //{
        //            //    rate = rateModel.RateValue / 100;
        //            //}
        //            //先判断转入人的同类型的账户明细有还是没有，有的话，则直接修改，没有的话，新增
        //            var qq = from a in db.TM_Mem_AccountDetail
        //                     join b in db.TM_Mem_AccountLimit on a.AccountDetailID equals b.AccountDetailID into bb
        //                     from bba in bb.DefaultIfEmpty()
        //                     where a.AccountID == to.AccountID && a.AccountDetailType == "value1"
        //                     select new
        //                     {
        //                         a.AccountDetailID,
        //                         a.SpecialDate1,
        //                         a.SpecialDate2,
        //                         LimitType = bba == null ? null : bba.LimitType,
        //                         LimitValue = bba == null ? null : bba.LimitValue,
        //                     };
        //            var qq1 = qq.ToList().GroupBy(t => new { t.AccountDetailID,t.SpecialDate1, t.SpecialDate2 }).Select(g => new {AccountDetailID=g.Key.AccountDetailID, SpecialDate1 = g.Key.SpecialDate1, SpecialDate2 = g.Key.SpecialDate2, limitName = string.Join(",", g.Select(s => s.LimitValue).Where(s => s != null).ToArray()) });


        //            foreach (var change in changes)
        //            {
        //                decimal transferValueFrom = change.TransferValue;//转出值
        //                decimal transferValueTo = Math.Round(transferValueFrom * (decimal)rate, 4);//转入值
        //                temp = transferValueTo;//临时值
        //                long detailId;
        //                if (transferValueFrom <= 0)
        //                {
        //                    break;
        //                }
        //                //转出人账户被选中的账户
        //                var ds = details.Where(p => change.AccountDetailIds.Contains(p.AccountDetailID)).OrderBy(p => p.SpecialDate2).ToList();

        //                transferTotalFrom += transferValueFrom;
        //                if (transferTotalFrom > from.Value1 || ds.Count < 1 || transferValueFrom > ds.Sum(p => p.DetailValue))
        //                {
        //                    return new Result(false, "转出账户积点余额不足");
        //                }

        //                transferTotalTo += transferValueTo;


        //                //转出账户的限制
        //                var ls = limits.Where(p => change.AccountDetailIds.Contains(p.AccountDetailID.Value)).Select(p => new { p.LimitType, p.LimitValue }).Distinct();
        //                string st1 = "";
        //                if (ls.Count() > 0)
        //                {
        //                    foreach (var l in ls)
        //                    {
        //                        st1 += l.LimitValue + ",";
        //                    }
        //                }
        //                //先判断转入人的同类型的账户明细有还是没有，有的话，则直接修改，没有的话，新增
        //                var qq2 = qq1.Where(p => p.SpecialDate2 == ds.Last().SpecialDate2 && compareArr(p.limitName, st1)).ToList();
        //                if (qq2.Count > 0)//存在一样的记录，更新数据
        //                {
        //                    detailId=qq2[0].AccountDetailID;
        //                    var toDetail_M = db.TM_Mem_AccountDetail.Where(p => p.AccountDetailID == detailId).FirstOrDefault();
        //                    toDetail_M.DetailValue += transferValueTo;
        //                    toDetail_M.ModifiedDate = DateTime.Now;
        //                    toDetail_M.ModifiedUser = modifiedBy;

        //                    db.Entry(toDetail_M).State = EntityState.Modified;
        //                }
        //                else //新增
        //                {
        //                    //转入AccountDetail插入
        //                    var toDetail = new TM_Mem_AccountDetail();
        //                    toDetail.AccountID = to.AccountID;
        //                    toDetail.AccountDetailType = "value1";
        //                    toDetail.DetailValue = transferValueTo;
        //                    toDetail.SpecialDate2 = ds.Last().SpecialDate2;
        //                    toDetail.DetailValue = transferValueTo;
        //                    toDetail.AddedDate = DateTime.Now;
        //                    toDetail.AddedUser = modifiedBy;
        //                    toDetail.ModifiedDate = DateTime.Now;
        //                    toDetail.ModifiedUser = modifiedBy;
        //                    db.TM_Mem_AccountDetail.Add(toDetail);
        //                    db.SaveChanges();
        //                    detailId = toDetail.AccountDetailID;

        //                    //转入AccountDetail限制
        //                    foreach (var l in ls)
        //                    {
        //                        var toL = new TM_Mem_AccountLimit();
        //                        toL.AccountID = to.AccountID;
        //                        toL.AccountDetailID = detailId;
        //                        toL.LimitType = l.LimitType;
        //                        toL.LimitValue = l.LimitValue;
        //                        toL.AddedDate = DateTime.Now;
        //                        db.TM_Mem_AccountLimit.Add(toL);
        //                    }
        //                }
                        

        //                //转入AccountDetail日志
        //                TL_Mem_AccountChange toC = new TL_Mem_AccountChange();
        //                toC.AccountID = to.AccountID;
        //                toC.AccountDetailID = detailId;
        //                toC.AccountChangeType = "transfer-to";
        //                toC.ChangeValue = temp;
        //                toC.ChangeReason = "积点转让-转入";
        //                toC.ReferenceNo = from.AccountID;//转出账号的AccountID
        //                toC.AddedDate = DateTime.Now;
        //                toC.AddedUser = modifiedBy;
        //                db.TL_Mem_AccountChange.Add(toC);
        //                db.SaveChanges();

        //                //var ls = limits.Where(p => change.AccountDetailIds.Contains(p.AccountDetailID.Value)).Select(p => new { p.LimitType, p.LimitValue }).Distinct();
                        

        //                foreach (var d in ds)
        //                {
        //                    //转出AccountDetail日志
        //                    TL_Mem_AccountChange c = new TL_Mem_AccountChange();
        //                    c.AccountID = d.AccountID;
        //                    c.AccountDetailID = d.AccountDetailID;
        //                    c.AccountChangeType = "transfer-from";
        //                    c.ChangeReason = "积点转让-转出";
        //                    c.ReferenceNo = toC.LogID.ToString();//转入log的LogID
        //                    c.AddedDate = DateTime.Now;
        //                    c.AddedUser = modifiedBy;

        //                    if (d.DetailValue <= transferValueFrom)
        //                    {
        //                        transferValueFrom = transferValueFrom - d.DetailValue;
        //                        c.ChangeValue = 0 - d.DetailValue;
        //                        d.DetailValue = 0;
        //                    }
        //                    else
        //                    {
        //                        c.ChangeValue = 0 - transferValueFrom;
        //                        d.DetailValue = d.DetailValue - transferValueFrom;
        //                        transferValueFrom = 0;
        //                    }

        //                    db.TL_Mem_AccountChange.Add(c);

        //                    //转出AccountDetail日志
        //                    d.ModifiedDate = DateTime.Now;
        //                    d.ModifiedUser = modifiedBy;
        //                    db.Entry(d).State = EntityState.Modified;

        //                    if (transferValueFrom <= 0)
        //                    {
        //                        break;
        //                    }
        //                }
        //            }

        //            //转入Account数据增加
        //            to.Value1 += transferTotalTo;
        //            to.ModifiedDate = DateTime.Now;
        //            to.ModifiedUser = modifiedBy;
        //            db.Entry(to).State = EntityState.Modified;

        //            //转出Account数据扣除
        //            from.Value1 -= transferTotalFrom;
        //            from.ModifiedDate = DateTime.Now;
        //            from.ModifiedUser = modifiedBy;
        //            db.Entry(from).State = EntityState.Modified;

        //            db.SaveChanges();
        //            db.Commit();
        //            Service.SendMsgPointTrans(fromMemId, toMemId, DateTime.Now, transferTotalFrom, dataGroupId);
        //            return new Result(true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Result(false, ex.Message);
        //    }
        //}
        #endregion

        #region 积分转让比例管理
        public static Result GetTransferRates(string dp)
        {
            //DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var query = from r in db.TL_SYS_TransferRate
            //                join d in db.TM_SYS_DataGroup on r.DataGroupId equals d.DataGroupID
            //                orderby r.ModifiedDate descending
            //                select new
            //                {
            //                    r.RateId,
            //                    r.DataGroupId,
            //                    d.DataGroupName,
            //                    r.RateValue,
            //                    r.Remark,
            //                    r.ModifiedDate
            //                };

            //    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            //}

            throw new NotImplementedException();
        }

        public static Result GetTransferRateById(int id)
        {
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var model = db.TL_SYS_TransferRate.Find(id);
            //    if (model == null)
            //    {
            //        return new Result(true, "记录不存在");
            //    }
            //    return new Result(true, "", model);
            //}

            throw new NotImplementedException();
        }

        public static Result SaveTransferRate(string rateStr, string modifiedBy)
        {
            using (CRMEntities db = new CRMEntities())
            {
                try
                {
                    //var rate = JsonHelper.Deserialize<TL_SYS_TransferRate>(rateStr);
                    //var elist = db.TL_SYS_TransferRate.Where(p => p.DataGroupId == rate.DataGroupId).ToList();
                    //if (rate.RateId == 0)
                    //{
                    //    if (elist.Count > 0)
                    //    {
                    //        return new Result(true, "已经存在此数据群组的记录");
                    //    }
                    //    rate.AddedDate = DateTime.Now;
                    //    rate.AddedUser = modifiedBy;
                    //    rate.ModifiedDate = DateTime.Now;
                    //    rate.ModifiedUser = modifiedBy;
                    //    db.TL_SYS_TransferRate.Add(rate);
                    //}
                    //else
                    //{
                    //    var model = db.TL_SYS_TransferRate.Find(rate.RateId);
                    //    if (model == null)
                    //    {
                    //        return new Result(true, "记录不存在");
                    //    }
                    //    if (elist.Count > 1 || elist.Count == 1 && !elist.Contains(model))
                    //    {
                    //        return new Result(true, "已经存在此数据群组的记录");
                    //    }
                    //    model.DataGroupId = rate.DataGroupId;
                    //    model.RateValue = rate.RateValue;
                    //    model.Remark = rate.Remark;
                    //    model.ModifiedDate = DateTime.Now;
                    //    model.ModifiedUser = modifiedBy;
                    //    db.Entry(model).State = EntityState.Modified;
                    //}
                    db.SaveChanges();
                    return new Result(true, "保存成功");
                }
                catch (Exception ex)
                {
                    return new Result(false, ex.Message);
                }
            }
        }

        public static Result DeleteTransferRate(int id)
        {
            //using (CRMEntities db = new CRMEntities())
            //{
            //    try
            //    {
            //        var model = db.TL_SYS_TransferRate.Find(id);
            //        if (model == null)
            //        {
            //            return new Result(true, "记录不存在");
            //        }
            //        db.TL_SYS_TransferRate.Remove(model);
            //        db.SaveChanges();
            //        return new Result(true);
            //    }
            //    catch (Exception ex)
            //    {
            //        return new Result(false, ex.Message);
            //    }
            //}

            throw new NotImplementedException();
        }

        public static Result GetSubDataGroupList(int dataGroupId, int? selected)
        {
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var list = db.TL_SYS_TransferRate.Select(p => p.DataGroupId).Distinct();
            //    var query = from d in db.V_Sys_DataGroupRelation
            //                where d.DataGroupID == dataGroupId && (!list.Contains(d.SubDataGroupID) || (selected != null && d.SubDataGroupID == selected))
            //                select d;
            //    return new Result(true, "", query.ToList());
            //}

            throw new NotImplementedException();
        }
        #endregion


    }
}
