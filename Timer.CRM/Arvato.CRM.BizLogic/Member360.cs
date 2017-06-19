using Arvato.CRM.EF;
using Arvato.CRM.MemberSubdivisionLogic;
using Arvato.CRM.Model;
using Arvato.CRM.Trigger;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data.SqlClient;
using System.Threading;
using System.Net.Http;
using System.Configuration;
using System.Data.Entity.Validation;



namespace Arvato.CRM.BizLogic
{
    public static class Member360
    {
        #region 会员360视图列表
        /// <summary>
        /// 分页查询会员360数据
        /// </summary>
        /// <param name="dp">datatable信息</param>
        /// <param name="search">查询条件</param>
        /// <returns></returns>
        public static Result GetMembersByPage(string dp, Member360SearchModel search)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
                //调用存储过程
                var query = db.sp_CRM_Mem_Search(search.DataGroupId, search.PageIds, search.DataRoleIds,
                    search.MemberNo, search.CustomerName, search.CustomerMobile, search.CustomerLevel, search.RegisterStoreCode,
                    search.RegStoreArea, search.RegStoreChan, search.ConsumeAmountStart, search.ConsumeAmountEnd,
                    search.ConsumePointStart, search.ConsumePointEnd, "", search.CustomerSource, search.RegisterDateStart, search.RegisterDateEnd, myDp.SortCols[0] + " " + myDp.SortDirs[0],
                    myDp.iDisplayStart / myDp.iDisplayLength, myDp.iDisplayLength, recordCount).ToList();

                var res = new DatatablesSourceVsPage();
                res.iDisplayStart = myDp.iDisplayStart;
                res.iDisplayLength = myDp.iDisplayLength;
                res.iTotalRecords = (int)recordCount.Value;
                res.aaData = query;
                return new Result(true, "", new List<object> { res });
            }
        }

        /// <summary>
        /// 根据会员ID查询子会员
        /// </summary>
        /// <param name="dp">datatable信息</param>
        /// <param name="parentMemberId">父会员ID</param>
        /// <returns></returns>
        public static Result GetChildMembersByPage(string dp, string memberId)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (var db = new CRMEntities())
            {
                //var cars = db.V_M_TM_Mem_SubExt_vehicle.Where(o => o.MemberID == memberId).ToList();
                //string carNo = "";
                //if (cars != null && cars.Count > 0)
                //{
                //    for (int i = 0; i < cars.Count; i++)
                //    {
                //        carNo += cars[i].CarNo + ",";
                //    }
                //    if (!string.IsNullOrEmpty(carNo))
                //        carNo = carNo.Remove(carNo.Length - 1, 1);
                //}
                var query = from m in db.V_U_TM_Mem_Info
                            //join b in db.V_U_TM_Mem_Info on m.MemberID equals b.ParentMemberID
                            join o1temp in db.TD_SYS_BizOption.Where(o => o.OptionType == "CustomerStatus") on m.CustomerStatus equals o1temp.OptionValue into o1t
                            from o1 in o1t.DefaultIfEmpty()
                            join o2temp in db.TD_SYS_BizOption.Where(o => o.OptionType == "CustomerLevel") on m.CustomerLevel equals o2temp.OptionValue into o2t
                            from o2 in o2t.DefaultIfEmpty()
                            join storetemp in db.V_M_TM_SYS_BaseData_store on m.RegisterStoreCode equals storetemp.StoreCode into storet
                            from store in storet.DefaultIfEmpty()
                            where m.ParentMemberID == memberId //&& o1.DataGroupID == b.DataGroupID && o2.DataGroupID == b.DataGroupID
                            select new
                            {
                                m.MemberID,
                                m.MemberCardNo,
                                m.CustomerStatus,
                                CustomerStatusText = o1.OptionText,
                                m.CustomerName,
                                m.CustomerMobile,
                                m.CustomerLevel,
                                CustomerLevelText = o2.OptionText,
                                CarNo = "",
                                m.RegisterDate,
                                m.RegisterStoreCode,
                                RegisterStoreName = store.StoreName
                            };
                return new Result(true, "", new List<object> { query.Take(1).ToDataTableSourceVsPage(myDp) });
            }
        }
        #endregion

        #region 会员详情
        /// <summary>
        /// 获取会员简要信息
        /// </summary>
        /// <param name="mid">会员ID</param>
        /// <returns></returns>
        public static Result GetMemberBasicInfo(string mid)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    string wechatApi = ConfigurationSettings.AppSettings["wechatApi"];
                    try
                    {

                    }
                    catch (Exception ex)
                    {
                        Log4netHelper.WriteErrorLog("同步会员：" + mid + ",微信金币失败|" + ex.ToString());
                    }


                    var qw = db.V_U_TM_Mem_Info.Where(p => p.MemberID == mid).FirstOrDefault();

                    var querybasic = from a in db.V_U_TM_Mem_Info
                                     join b in db.V_M_TM_SYS_BaseData_customerlevel on a.CustomerLevel equals b.CustomerLevelBase into bb
                                     from bba in bb.DefaultIfEmpty()
                                     join c in db.TD_SYS_BizOption.Where(p => p.OptionType == "MemberType") on a.CustomerType2 equals c.OptionValue into cc
                                     from cca in cc.DefaultIfEmpty()
                                     join d in db.TD_SYS_BizOption.Where(p => p.OptionType == "CustomerStatus") on a.CustomerStatus equals d.OptionValue into dd
                                     from dda in dd.DefaultIfEmpty()
                                     join d in db.TM_Mem_Account on a.MemberID equals d.MemberID
                                     join storetemp in db.V_M_TM_SYS_BaseData_store on a.RegisterStoreCode equals storetemp.StoreCode into storet
                                     from store in storet.DefaultIfEmpty()
                                     join e in db.TM_Mem_Card on a.MemberID equals e.MemberID into ee
                                     from eea in ee.DefaultIfEmpty()
                                     where a.MemberID == mid
                                     select new
                                     {
                                         a.MemberID,//2017-04-27 wy
                                         eea.CardNo,
                                         a.CustomerName,
                                         a.DataGroupID,
                                         a.MemberGrade,
                                         a.Gender,
                                         a.MemberCardNo,
                                         a.CustomerMobile,
                                         a.ProductColorLike,
                                         a.BestProductType,
                                         a.RecentPurchaseDate,
                                         a.ConsumptionCounts,
                                         a.ConsumptionYearly,
                                         a.CumulativeIntegral_6,
                                         a.ConsumeIntegral_6,
                                         a.ConsumptionCounts_24,
                                         a.ActConsumption_24,
                                         a.AvgMemPrice,
                                         a.CustomerStatus,
                                         CustomerStatusText = dda.OptionText,
                                         a.StrMem_bk_3,//=cca.OptionText,
                                         a.CertificateType,
                                         a.CertificateNo,
                                         a.StrMem_bk_1,
                                         a.StrMem_bk_2,
                                         a.StrMem_bk_4,
                                         a.StrMem_bk_5,
                                         a.CustomerEmail,
                                         a.Province,
                                         a.City,
                                         a.District,
                                         a.RegisterStoreCode,
                                         RegisterStore = store.StoreName,
                                         RegisterDate = a.AddedDate,
                                         a.ProvinceCodeExt,
                                         a.CityCodeExt,
                                         a.DistrictCodeExt,
                                         a.Birthday,
                                         a.Address1,
                                         a.CustomerType2,
                                         a.CustomerLevel,
                                         GoldTotal = d.Value1,
                                         CustomerLevelText = bba.CustomerLevelNameBase,
                                         MaxIntergral = bba.MaxIntergral,
                                         a.BusinessDepartment,
                                         //eea.Active,
                                         //eea.Lock,
                                         //CustomerStatus = (a.CustomerStatus == "1" && (eea == null ? false : eea.Active) == true && (eea == null ? false : eea.Lock) == false) ? 1 : 0
                                     };
                    //var query = from a in querybasic
                    //            join storetemp in db.V_M_TM_SYS_BaseData_store on a.RegisterStoreCode equals storetemp.StoreCode into storet
                    //            from store in storet.DefaultIfEmpty()
                    //            join b in db.TD_SYS_BizOption.Where(p => p.OptionType == "CustomerLevel") on a.CustomerLevel equals b.OptionValue into bl
                    //            from bll in bl.DefaultIfEmpty()
                    //            join prv in db.TD_SYS_Region on a.ProvinceCodeExt equals prv.RegionID into prvlj
                    //            from p in prvlj.DefaultIfEmpty()
                    //            join cty in db.TD_SYS_Region on a.CityCodeExt equals cty.RegionID into ctylj
                    //            from c in ctylj.DefaultIfEmpty()
                    //            join dst in db.TD_SYS_Region on a.DistrictCodeExt equals dst.RegionID into dstlj
                    //            from d in dstlj.DefaultIfEmpty()
                    //            join acct in db.TM_Mem_Account on new { a.MemberID, AccountType = "1" } equals new { acct.MemberID, acct.AccountType } into acclj
                    //            from acc in acclj.DefaultIfEmpty()
                    //            select new
                    //            {
                    //                a.AddedDate,
                    //                a.AddedUser,
                    //                a.Address1,
                    //                a.Address2,
                    //                a.Birthday,
                    //                a.MemberOpenID,
                    //                City = c.NameZH,
                    //                a.Corp,

                    //                a.CustomerEmail,
                    //                CustomerLevel = bll.OptionText,
                    //                a.MemberLevelEndDate,
                    //                a.MemberEndDate,
                    //                a.CustomerMobile,
                    //                a.CustomerName,
                    //                a.CustomerSource,
                    //                a.RegChannelName,
                    //                a.NickName,
                    //                //a.CustomerType,
                    //                a.CustomerType2,
                    //                a.DataGroupID,
                    //                District = d.NameZH,
                    //                a.Education,
                    //                a.FamilyIncome,
                    //                a.Gender,
                    //                a.Hobbies,
                    //                a.IsEmail,
                    //                a.IsMessage,
                    //                a.Job,
                    //                a.MemberCardNo,
                    //                a.MemberCardStatus,
                    //                a.MemberGrade,
                    //                a.MemberID,
                    //                a.MemberCode,
                    //                a.MemberLoyalty,
                    //                a.MembershipActivity,
                    //                a.ModifiedDate,
                    //                a.ModifiedUser,
                    //                a.NearlyRemindTime,
                    //                a.NearlyShopTime,
                    //                a.ParentMemberID,
                    //                a.Profession,
                    //                Province = p.NameZH,
                    //                a.RegisterDate,
                    //                a.RegisterStoreCode,
                    //                a.Remark,
                    //                a.CertificateNo,
                    //                a.Tel,
                    //                a.Zip,
                    //                a.IsBirthday,
                    //                a.RecommenderName,
                    //                a.RecommenderCode,
                    //                a.InfoCompletedFlag,
                    //                //a.ProvinceNameSales,
                    //                //a.ProvinceNameService,
                    //                //a.CityNameSales,
                    //                //a.CityNameService,
                    //                //a.DistrictNameSales,
                    //                //a.DistrictNameService,
                    //                StoreName = store.StoreName,
                    //                Kids = querykids,
                    //                ProductColorLike = a.ProductColorLike_1N,
                    //                a.RecentPurchaseDate,
                    //                a.ConsumptionCounts,
                    //                a.ConsumptionYearly,
                    //                a.HasChild,
                    //                acc.AccountID,
                    //                a.CumulativeIntegral_6,
                    //                a.ConsumeIntegral_6,
                    //                a.ConsumptionCounts_24,
                    //                a.ActConsumption_24,
                    //                a.AvgMemPrice,
                    //                a.BestProductType,
                    //                a.BestProductMaterial,
                    //                a.MemberIntroducer,
                    //                a.RegisterDateProtal,
                    //                AccCumulativeIntegral = a.AccCumulativeIntegral == null ? 0 : a.AccCumulativeIntegral,
                    //                AvailPoint = acc.Value1 == null ? 0M : acc.Value1,
                    //                a.ActConsumption
                    //            };
                    return new Result(true, "获取成功", querybasic.FirstOrDefault());

                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public static Result SaveMemberDetail(string mem_info, string userid)
        {
            using (CRMEntities db = new CRMEntities())
            {
                try
                {


                    V_S_TM_Mem_Ext ext = JsonHelper.Deserialize<V_S_TM_Mem_Ext>(mem_info);
                    V_S_TM_Mem_Master master = JsonHelper.Deserialize<V_S_TM_Mem_Master>(mem_info);
                    var querymobileexist = (from p in db.V_S_TM_Mem_Master
                                            where p.Mobile == master.Mobile
                                            select p).FirstOrDefault();
                    if (querymobileexist != null)
                    {
                        if (querymobileexist.MemberID != master.MemberID)
                        {
                            return new Result(true, "手机号已存在,无法修改！");
                        }
                    }
                    master.Mobile = ext.CustomerMobile;
                    V_S_TM_Mem_Ext ext1 = db.V_S_TM_Mem_Ext.Where(p => p.MemberID == ext.MemberID).FirstOrDefault();
                    V_S_TM_Mem_Master master1 = db.V_S_TM_Mem_Master.Where(p => p.MemberID == ext.MemberID).FirstOrDefault();

                    ext.AddedUser = userid;
                    ext.AddedDate = DateTime.Now;
                    master.AddedUser = userid;
                    ext.ModifiedDate = DateTime.Now;
                    ext.ModifiedUser = userid;
                    master.ModifiedDate = DateTime.Now;
                    master.ModifiedUser = userid;
                    master.AddedDate = DateTime.Now;

                    V_S_TM_Mem_Ext ext2 = ToolsHelper.MargeObject<V_S_TM_Mem_Ext>(ext1, ext);
                    V_S_TM_Mem_Master master2 = ToolsHelper.MargeObject<V_S_TM_Mem_Master>(master1, master);

                    db.UpdateViewRow<V_S_TM_Mem_Ext, TM_Mem_Ext>(ext2);
                    db.UpdateViewRow<V_S_TM_Mem_Master, TM_Mem_Master>(master2);
                    db.SaveChanges();
                    //string sql = "insert into  [TL_SYS_InterfaceLog]( [LogType],[InterfacePara],[InterfaceMethod],[Status],[RunTimes] ,[AddedDate]) select  'ActMember','" + master.MemberID + "','ActMember',0,0,GETDATE()";
                    //db.Database.ExecuteSqlCommand(sql);
                    //Log4netHelper.WriteInfoLog(sql + master.MemberID + ext.MemberID + mem_info);
                }
                catch (Exception ex)
                {
                    return new Result(true, "修改失败", ex.ToString());
                }
                //catch (DbEntityValidationException ex)
                //{
                //    foreach (var item in ex.EntityValidationErrors.ToList())
                //    {
                //        foreach (var v in item.ValidationErrors.ToList())
                //        {
                //            Console.WriteLine(v.ErrorMessage + v.PropertyName);
                //        }
                //    }
                //    //Log4netHelper.WriteErrorLog("360会员修改" + ex.ToString());
                //    return new Result(true, "修改失败", ex.ToString());
                //}

            }
            return new Result(true, "修改成功");
        }


        public static Result SaveMemberDetail(string mid, string name, string nickname, string mobile, string gender, string email, string certno, DateTime? birthday, string tel, string provid, string cityid, string distid, string addr, string zip, int b1id, string b1name, string b1gender, DateTime? b1birth, string b1height, string b1weight, int b2id, string b2name, string b2gender, DateTime? b2birth, string b2height, string b2weight, string userId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var m = db.TM_Mem_Master.Where(p => p.MemberID == mid).FirstOrDefault();
                var s = db.TM_Mem_Ext.Where(p => p.MemberID == mid).FirstOrDefault();
                var provname = db.TD_SYS_Region.Where(p => p.RegionID == provid).FirstOrDefault();//获取省名称
                var cityname = db.TD_SYS_Region.Where(p => p.RegionID == cityid).FirstOrDefault();//获取市名称
                var distname = db.TD_SYS_Region.Where(p => p.RegionID == distid).FirstOrDefault();//获取区名称
                if (m != null && s != null)
                {
                    m.ModifiedUser = userId;
                    m.Str_Key_1 = mobile; //手机号
                    m.ModifiedDate = DateTime.Now;
                    s.Str_Attr_3 = name;        //name
                    s.Str_Attr_4 = mobile;
                    s.Str_Attr_6 = nickname;
                    s.Str_Attr_7 = gender;
                    s.Str_Attr_9 = certno;      //certificate no
                    s.Str_Attr_11 = provname == null ? "" : provname.NameZH;       //provincename
                    s.Str_Attr_12 = cityname == null ? "" : cityname.NameZH; ;       //cityname
                    s.Str_Attr_13 = distname == null ? "" : distname.NameZH; ;       //districtname
                    s.Str_Attr_15 = tel;
                    s.Str_Attr_18 = addr;       //address
                    s.Str_Attr_14 = zip;
                    s.Str_Attr_37 = provid;     //provicecode
                    s.Str_Attr_38 = cityid;     //code
                    s.Str_Attr_39 = distid;     //code
                    s.Str_Attr_100 = email;
                    s.Date_Attr_2 = birthday;
                    s.ModifiedUser = userId;
                    s.ModifiedDate = DateTime.Now;
                    m.Str_Key_2 = email;

                    if (b1id != 0 || b1name != "" || b1gender != "" || b1birth != null || b1height != "" || b1weight != "")
                    {
                        if (b1id == 0)
                        {
                            var b1 = new TM_Mem_SubExt();
                            b1.ExtType = "kid";
                            b1.MemberID = mid;
                            b1.Str_Attr_1 = b1name;
                            b1.Str_Attr_2 = b1gender;   //gender
                            b1.Str_Attr_3 = b1height;
                            b1.Str_Attr_4 = b1weight;
                            b1.Date_Attr_1 = b1birth;
                            db.TM_Mem_SubExt.Add(b1);
                        }
                        else
                        {
                            var b1 = db.TM_Mem_SubExt.Where(b => b.MemberSubExtID == b1id).FirstOrDefault();
                            if (b1 != null)
                            {
                                b1.Str_Attr_1 = b1name;
                                b1.Str_Attr_2 = b1gender;   //gender
                                b1.Str_Attr_3 = b1height;
                                b1.Str_Attr_4 = b1weight;
                                b1.Date_Attr_1 = b1birth;
                                var bbentry = db.Entry(b1);
                                bbentry.State = EntityState.Modified;
                            }
                            else
                            {
                                return new Result(true, "找不到此会员Baby信息");
                            }
                        }
                        s.Bool_Attr_4 = true;
                    }
                    if (b2id != 0 || b2name != "" || b2gender != "" || b2birth != null || b2height != "" || b2weight != "")
                    {
                        if (b2id == 0)
                        {
                            var b2 = new TM_Mem_SubExt();
                            b2.ExtType = "kid";
                            b2.MemberID = mid;
                            b2.Str_Attr_1 = b2name;
                            b2.Str_Attr_2 = b2gender;   //gender
                            b2.Str_Attr_3 = b2height;
                            b2.Str_Attr_4 = b2weight;
                            b2.Date_Attr_1 = b2birth;
                            db.TM_Mem_SubExt.Add(b2);
                        }
                        else
                        {
                            var b2 = db.TM_Mem_SubExt.Where(b => b.MemberSubExtID == b2id).FirstOrDefault();
                            if (b2 != null)
                            {
                                b2.Str_Attr_1 = b2name;
                                b2.Str_Attr_2 = b2gender;   //gender
                                b2.Str_Attr_3 = b2height;
                                b2.Str_Attr_4 = b2weight;
                                b2.Date_Attr_1 = b2birth;
                                var bbentry = db.Entry(b2);
                                bbentry.State = EntityState.Modified;
                            }
                            else
                            {
                                return new Result(true, "找不到此会员Baby信息");
                            }
                        }
                        s.Bool_Attr_4 = true;
                    }
                    if (b1name == "" && b2name == "")
                    {
                        s.Bool_Attr_4 = false;
                    }
                    else
                    {
                        s.Bool_Attr_4 = true;
                    }
                    //var entry = db.Entry(m);
                    //entry.State = EntityState.Modified;
                    //var entry2 = db.Entry(s);
                    //entry2.State = EntityState.Modified;
                    //db.UpdateViewRow<V_S_TM_Mem_Master, TM_Mem_Master>(m);
                    //db.SaveChanges();
                    //s.MemberID = mid;
                    //s.CustomerEmail = email;
                    //db.UpdateViewRow<V_S_TM_Mem_Ext, TM_Mem_Ext>(s);
                    db.SaveChanges();
                }
                else
                {
                    return new Result(true, "找不到此会员");
                }
            }

            return new Result(true, "修改成功");
        }

        //检查手机唯一性
        public static Result CheckMobileExist(string mid, string mobile)
        {
            using (var db = new CRMEntities())
            {
                var query = (from a in db.V_U_TM_Mem_Info
                             where a.MemberID != mid && a.CustomerMobile == mobile
                             select new
                             {
                                 exist = 1
                             });
                return new Result(true, "", query.ToList());
            }
        }

        //检查Email唯一性
        public static Result CheckEmailExist(string mid, string email)
        {
            using (var db = new CRMEntities())
            {
                var query = (from a in db.V_U_TM_Mem_Info
                             where a.MemberID != mid && a.CustomerEmail == email
                             select new
                             {
                                 exist = 1
                             }).ToList();
                return new Result(true, "", query.ToList());
            }
        }

        //public static Result GetMemLevelHistory(string dp, string mid)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (var db = new CRMEntities())
        //    {
        //        var query = from a in db.TL_Mem_LevelChange
        //                    join u in db.TM_AUTH_User on a.AddedUser equals SqlFunctions.StringConvert((double)u.UserID).Trim()
        //                    join o1 in db.TD_SYS_BizOption.Where(o => o.OptionType == "LevelChangeType") on a.LevelChangeType equals o1.OptionValue
        //                    join o2 in db.TD_SYS_BizOption.Where(o => o.OptionType == "CustomerLevel") on a.ChangeLevelTo equals o2.OptionValue
        //                    join o3 in db.TD_SYS_BizOption.Where(o => o.OptionType == "CustomerLevel") on a.ChangeLevelFrom equals o3.OptionValue
        //                    where a.MemberID == mid
        //                    orderby a.AddedDate descending
        //                    select new
        //                    {
        //                        ChangeLevelFrom = o3.OptionText,
        //                        a.StartDateFrom,
        //                        a.EndDateFrom,
        //                        a.StartDateTo,
        //                        a.EndDateTo,
        //                        a.AddedDate,
        //                        a.AddedUser,
        //                        a.ChangeLevelTo,
        //                        a.ChangeReason,
        //                        a.LevelChangeType,
        //                        a.LogID,
        //                        a.MemberID,
        //                        LevelChangeTypeText = o1.OptionText,
        //                        ChangeLevelToText = o2.OptionText,
        //                        ChangeUserName = u.UserName
        //                    };
        //        var res = query.Distinct();
        //        return new Result(true, "", new List<object> { res.ToDataTableSourceVsPage(myDp) });
        //    }
        //}


        public static Result GetTradeHistoryData(string dp, int dataGroupId, string mid, DateTime? start, DateTime? end, string chan, decimal? amount_start, decimal? amount_end, string ordercode, string storecode)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (var db = new CRMEntities())
            {
                //var query = from p in db.V_M_TM_Mem_Trade_sales.AsNoTracking() where p.MemberID == mid select p;
                //var sales = from a in query
                //            join tp in db.TD_SYS_BizOption.AsNoTracking() on a.TradeType equals tp.OptionValue into tplj
                //            from t in tplj.DefaultIfEmpty()
                //            join chlj in db.V_M_TM_SYS_BaseData_store.AsNoTracking() on a.StoreCodeSales equals chlj.StoreCode into clj
                //            from c in clj.DefaultIfEmpty()
                //            where t.OptionType == "MemberTrade" &&
                //            (chan != "" ? (c.ChannelTypeCodeStore == chan) : true) &&
                //            (start != null ? (a.ListDateSales >= start) : true) &&
                //            (end != null ? (a.ListDateSales <= end) : true) &&
                //            (amount_start != null ? (a.Amount >= amount_start) : true) &&
                //            (amount_end != null ? (a.Amount <= amount_end) : true) &&
                //            (string.IsNullOrEmpty(storecode) ? true : (c.StoreCode == storecode)) &&
                //            a.TradeCode.Contains(ordercode)
                //            select new
                //            {
                //                a.TradeID,
                //                a.TradeCode,
                //                a.SourceBillNOSales,
                //                a.StandardAmountSales,
                //                a.Amount,
                //                a.TotalMoneySales,
                //                a.DiscountAmountSales,
                //                QuantitySales = a.QuantitySales == null ? 0 : a.QuantitySales,
                //                a.Discount,
                //                a.AddedDate,
                //                a.PayMethodSales,
                //                a.ConsumeIntegralSales,
                //                BirthdayprivilegeQtySales = a.BirthdayprivilegeQtySales == null ? 0 : a.BirthdayprivilegeQtySales,
                //                c.StoreName,
                //                c.StoreCode,
                //                a.TradeAmoutSales,
                //                a.IntergralAccountingSales,
                //                TradeType = t.OptionText,
                //                Channel = c.ChannerTypeNameStore,
                //                a.ListDateSales
                //            };
                return new Result(true, "");
            }
        }

        public static Result GetTradeHistoryDetail(string dp, long oid)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (var db = new CRMEntities())
            {
                //var saledet = from a in db.V_M_TM_Mem_TradeDetail_sales_product.AsNoTracking()
                //              join tp in db.TD_SYS_BizOption.AsNoTracking().Where(p => p.OptionType == "TradeDetailStatus") on SqlFunctions.StringConvert((double)a.StatusProduct).Trim() equals tp.OptionValue into tplj
                //              from t in tplj.DefaultIfEmpty()
                //              join prd in db.V_M_TM_SYS_BaseData_product.AsNoTracking() on a.GoodsCodeProduct equals prd.ProductCode into plj
                //              from p in plj.DefaultIfEmpty()
                //              join clr in db.V_M_TM_SYS_BaseData_colors.AsNoTracking() on a.ColorCodeProduct equals clr.ColorCode into clj
                //              from c in clj.DefaultIfEmpty()
                //              where a.TradeID == oid
                //              select new
                //              {
                //                  TradeDetailType = t.OptionText,
                //                  a.GoodsCodeProduct,
                //                  p.ProductName,
                //                  c.ColorName,
                //                  a.SizeCodeProduct,
                //                  a.QuantityProduct,
                //                  a.PriceProduct,
                //                  a.AmountProduct
                //              };
                return new Result(true, "");
            }
        }

        public static Result GetTradeHistoryPayment(string dp, long oid)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (var db = new CRMEntities())
            {
                //var saledet = from a in db.V_M_TM_Mem_TradeDetail_sales_payment.AsNoTracking()
                //              where a.TradeID == oid
                //              select new
                //              {
                //                  a.PmtNamePayment,
                //                  a.AmountPayment,
                //                  a.ReceivedAmountPayment,
                //                  IntegralCostPayment = a.IntegralCostPayment == null ? 0 : a.IntegralCostPayment,
                //                  a.PmtCardNoPayment,
                //                  a.IsReturn
                //              };
                return new Result(true, "");
            }
        }


        public static Result GetTradeRightsDetail(string dp, long tradeId)
        {
            //DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var query = from rights in db.V_M_TM_Mem_TradeDetail_service_memberinterests
            //                where rights.TradeID == tradeId
            //                select rights;

            //    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });

            //}

            throw new NotImplementedException();
        }
        public static Result GetMemPackages(string dp, string mid, string name, DateTime? start, DateTime? end, bool valid)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (var db = new CRMEntities())
            {


                var query = from a in db.TM_Mem_Package
                            where a.MemberID == mid
                            select a;

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(o => o.PackageName.Contains(name));
                }
                if (start != null)
                {
                    query = query.Where(o => o.StartDate <= start);
                }
                if (end != null)
                {
                    query = query.Where(o => o.EndDate >= end);
                }
                //if (valid)
                //{
                //    query = query.Where(o => o.Enable == true);
                //}
                var q = query.ToList();
                //套餐限制
                List<CouponLimit> limits = new List<CouponLimit>();
                if (q.Count > 0)
                {

                    for (int i = 0; i < q.Count; i++)
                    {
                        var pid = q[i].PackageInstanceID;
                        var q1 = db.TM_Mem_PackageLimit.Where(p => p.PackageInstanceID == pid).ToList();
                        var q3 = (from a in db.TM_Mem_PackageLimit
                                  join b in db.V_M_TM_SYS_BaseData_store on a.LimitValue equals b.StoreCode into ba
                                  from baa in ba.DefaultIfEmpty()
                                  join c in db.V_M_TM_SYS_BaseData_brand on a.LimitValue equals c.BrandCodeBase into ca
                                  from caa in ca.DefaultIfEmpty()
                                  join o in db.TD_SYS_BizOption on a.LimitType equals o.OptionValue
                                  where a.PackageInstanceID == pid && o.OptionType == "AccountLimitType"
                                  orderby a.PackageInstanceLimitID
                                  select new
                                  {
                                      a.LimitValue,
                                      a.LimitType,
                                      LimitTypeText = o.OptionText,
                                      StoreName = baa.StoreName,
                                      caa.BrandNameBase,
                                  }).ToList();
                        string str = "";
                        foreach (var item in q3)
                        {
                            if (item.LimitType == "store")
                            {
                                str += item.LimitTypeText + ":" + item.StoreName + "; ";
                            }
                            else if (item.LimitType == "brand")
                            {
                                str += item.LimitTypeText + ":" + item.BrandNameBase + "; ";
                            }
                            else
                            {
                                str += item.LimitTypeText + ":" + item.LimitValue + "; ";
                            }
                        }
                        limits.Add(new CouponLimit { CouponID = pid, LimitText = str });
                    }
                }
                var q2 = (from a in q
                          join b in db.TM_AUTH_User on a.AddedUser equals b.UserID.ToString() into bb
                          from bba in bb.DefaultIfEmpty()
                          join ltemp in limits on a.PackageInstanceID equals ltemp.CouponID into lt
                          from l in lt.DefaultIfEmpty()
                          where a.Enable == valid
                          select new
                          {
                              a.PackageInstanceID,
                              a.PackageName,
                              a.PurchasePrice1,
                              a.PurchasePrice2,//购买价格
                              a.StartDate,
                              a.EndDate,
                              a.AddedDate,
                              bba.UserName,//操作人
                              AccountLimit = l == null ? "" : l.LimitText ?? ""
                          }).ToList();
                return new Result(true, "", q2.ToDataTableSourceVsPage(myDp));
            }
        }
        public static Result GetPackageDetailsById(string dp, long piid)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (var db = new CRMEntities())
            {
                var query = from a in db.TM_Mem_PackageDetail
                            where a.PackageInstanceID == piid
                            select a;
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }
        public static Result GetPackageLimitsById(string dp, long piid)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (var db = new CRMEntities())
            {
                var query = from a in db.TM_Mem_PackageLimit
                            where a.PackageInstanceID == piid
                            select a;
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }
        /// <summary>
        /// 获取套餐明细使用历史
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="memId"></param>
        /// <param name="piid"></param>
        /// <returns></returns>
        public static Result GetMemPackageHistory(string dp, string memId, long piid)
        {
            throw new NotImplementedException();
            //DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            //using (var db = new CRMEntities())
            //{
            //    var query = from a in db.TL_Mem_ValueAddedChange
            //                join b in db.TD_SYS_PackageDetail on a.InstanceID equals b.PackageDetailID into bb
            //                from bba in bb.DefaultIfEmpty()
            //                where a.InstanceID == piid && a.Type == "package" && a.MemberID == memId
            //                select new
            //                {
            //                    a.ChangeValue,
            //                    a.ChangeReason,
            //                    a.AddedDate,
            //                    a.AddedUser,
            //                    a.HasReverse,
            //                    ItemName = bba == null ? "" : bba.ItemName
            //                };
            //    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            //}
        }
        public static Result GetMemContacts(string dp, string mid)
        {
            //DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            //using (var db = new CRMEntities())
            //{
            //    var query = from a in db.V_M_TM_Mem_SubExt_contact
            //                where a.MemberID == mid
            //                select a;
            //    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            //}

            throw new NotImplementedException();
        }
        public static Result GetMemContactDetail(long cid)
        {
            //using (var db = new CRMEntities())
            //{
            //    var query = from a in db.V_M_TM_Mem_SubExt_contact
            //                where a.MemberSubExtID == cid
            //                select a;
            //    return new Result(true, "", query.FirstOrDefault());
            //}
            throw new NotImplementedException();
        }

        public static Result GetMemAccountInfo(string mid)
        {
            using (var db = new CRMEntities())
            {
                var query = from t in
                                (
                                    from a in db.TM_Mem_Account
                                    join o in db.TD_SYS_BizOption.Where(o => o.OptionType == "AccountType") on a.AccountType equals o.OptionValue
                                    where a.MemberID == mid && a.AccountType == "3"
                                    orderby o.Sort
                                    select new { a.AccountType, AccountTypeText = o.OptionText, a.Value1, a.Value2, a.Value3 }
                                )
                            group t by new { t.AccountType, t.AccountTypeText } into g
                            select new
                            {
                                AccountType = g.Key.AccountType,
                                AccountTypeText = g.Key.AccountTypeText,
                                ValidValue = g.Sum(o => o.Value1),
                                FrozenValue = g.Sum(o => o.Value2),
                                UnValidValue = g.Sum(o => o.Value3),
                                TotalValue = g.Sum(o => o.Value1) + g.Sum(o => o.Value2)
                            };
                return new Result(true, "", query.ToList());
            }
        }
        //public static Result GetMemAccountDetails(string mid, string accType)
        //{
        //    using (var db = new CRMEntities())
        //    {
        //        var query1 = (from l in db.TM_Mem_AccountLimit
        //                      join a in db.TM_Mem_Account on l.AccountID equals a.AccountID
        //                      join o in db.TD_SYS_BizOption on l.LimitType equals o.OptionValue
        //                      join vtemp in db.V_M_TM_Mem_SubExt_vehicle on l.LimitValue equals SqlFunctions.StringConvert((double)vtemp.MemberSubExtID)
        //                      into vt
        //                      from v in vt.DefaultIfEmpty()
        //                      where a.MemberID == mid && a.AccountType == accType && l.LimitType == "vehicle" && o.OptionType == "AccountLimitType"
        //                      orderby l.AccountLimitID
        //                      select new
        //                      {
        //                          l.AccountLimitID,
        //                          l.AccountID,
        //                          LimitType = o.OptionText,
        //                          LimitValue = (v.CarNo ?? "") + "," + (v.VIN ?? "")
        //                      })
        //                      .Union(from l in db.TM_Mem_AccountLimit
        //                             join a in db.TM_Mem_Account on l.AccountID equals a.AccountID
        //                             join o in db.TD_SYS_BizOption on l.LimitType equals o.OptionValue
        //                             join vtemp in db.V_M_TM_SYS_BaseData_store on l.LimitValue equals vtemp.StoreCode
        //                             into vt
        //                             from v in vt.DefaultIfEmpty()
        //                             where a.MemberID == mid && a.AccountType == accType && l.LimitType == "store" && o.OptionType == "AccountLimitType"
        //                             orderby l.AccountLimitID
        //                             select new
        //                             {
        //                                 l.AccountLimitID,
        //                                 l.AccountID,
        //                                 LimitType = o.OptionText,
        //                                 LimitValue = v.StoreName
        //                             })
        //                      .Union(from l in db.TM_Mem_AccountLimit
        //                             join a in db.TM_Mem_Account on l.AccountID equals a.AccountID
        //                             join o in db.TD_SYS_BizOption on l.LimitType equals o.OptionValue
        //                             join vtemp in db.V_M_TM_SYS_BaseData_brand on l.LimitValue equals vtemp.BrandCode
        //                             into vt
        //                             from v in vt.DefaultIfEmpty()
        //                             where a.MemberID == mid && a.AccountType == accType && l.LimitType == "brand" && o.OptionType == "AccountLimitType"
        //                             orderby l.AccountLimitID
        //                             select new
        //                             {
        //                                 l.AccountLimitID,
        //                                 l.AccountID,
        //                                 LimitType = o.OptionText,
        //                                 LimitValue = v.BrandName
        //                             })
        //                      .ToList();
        //        List<AccountLimit> limits = new List<AccountLimit>();
        //        //limits.Add(new AccountLimit { AccountID = "", LimitText = "" });
        //        //foreach (var accountId in query1.Select(o => o.AccountID).Distinct())
        //        //{
        //        //    string str = "";
        //        //    foreach (var l in query1.Where(o => o.AccountID == accountId))
        //        //    {
        //        //        str += l.LimitType + ":" + l.LimitValue + "; ";
        //        //    }
        //        //    limits.Add(new AccountLimit { AccountID = accountId, LimitText = str });
        //        //}

        //        var query2 = (from a in db.TM_Mem_Account
        //                      join d in db.TM_Mem_AccountDetail on a.AccountID equals d.AccountID
        //                      join o in db.TD_SYS_BizOption on d.AccountDetailType equals o.OptionValue
        //                      where a.MemberID == mid && a.AccountType == accType && o.OptionType == "AccountDetailType"
        //                      orderby a.AccountID, o.Sort, d.AccountDetailID
        //                      select new
        //                      {
        //                          a.AccountID,
        //                          d.AccountDetailType,
        //                          DetailTypeText = o.OptionText ?? "",
        //                          a.Value1,
        //                          a.Value2,
        //                          d.AccountDetailID,
        //                          d.DetailValue,
        //                          d.SpecialDate1,
        //                          d.SpecialDate2
        //                      }).ToList();
        //        var query = from q in query2
        //                    join ltemp in limits on q.AccountID equals ltemp.AccountID into lt
        //                    from l in lt.DefaultIfEmpty()
        //                    select new
        //                    {
        //                        q.AccountID,
        //                        q.AccountDetailType,
        //                        q.DetailTypeText,
        //                        q.Value1,
        //                        q.Value2,
        //                        q.AccountDetailID,
        //                        q.DetailValue,
        //                        q.SpecialDate1,
        //                        q.SpecialDate2,
        //                        AccountLimit = l == null ? "" : l.LimitText ?? ""
        //                    };
        //        return new Result(true, "", new List<object> { query.ToList() });
        //    }
        //}
        public static Result GetMemAccountChangeHistory(string dp, string mid, string accChgType, DateTime? start, DateTime? end, string refid, decimal? chgnumStart, decimal? chgnumEnd)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (var db = new CRMEntities())
            {
                var queryMAD = from mad in db.TM_Mem_AccountDetail.AsNoTracking() join ma in db.TM_Mem_Account.AsNoTracking() on mad.AccountID equals ma.AccountID where ma.MemberID == mid select new { mad.AccountID, mad.AccountDetailID, mad.AccountDetailType, ma.MemberID };
                var queryTD = from td in db.TM_Mem_Trade.AsNoTracking() where td.MemberID == mid select td;
                var query = from c in db.TL_Mem_AccountChange.AsNoTracking()
                            join a in queryMAD on new { c.AccountID, c.AccountDetailID } equals new { a.AccountID, a.AccountDetailID }
                            join b in queryTD on c.ReferenceNo equals SqlFunctions.StringConvert((double)b.TradeID).Trim() into ab
                            from t in ab.DefaultIfEmpty()
                            join o in db.TD_SYS_BizOption.AsNoTracking().Where(o => o.OptionType == "AccountChangeType") on c.AccountChangeType equals o.OptionValue
                            join qqq in db.TD_SYS_BizOption.AsNoTracking().Where(d => d.OptionType == "AccountDetailType") on a.AccountDetailType equals qqq.OptionValue
                            where c.HasReverse == false
                            select new
                            {
                                c.LogID,
                                c.AccountDetailID,
                                c.AccountID,
                                c.AccountChangeType,
                                c.ChangeValue,
                                RestPoint = c.ChangeValueBefore + c.ChangeValue,
                                c.ReferenceNo,
                                t.TradeCode,
                                c.AddedDate,
                                c.ChangeReason,
                                AccountType = qqq.OptionText,
                                ChangeUserName = c.AddedUser,
                                ChangeTypeText = o.OptionText,
                                ChangeTypeValue = o.OptionValue,
                            };
                if (start != null) query = query.Where(o => o.AddedDate >= start);
                if (end != null) query = query.Where(o => o.AddedDate <= end);
                if (chgnumStart != null) query = query.Where(o => o.ChangeValue >= chgnumStart);
                if (chgnumEnd != null) query = query.Where(o => o.ChangeValue <= chgnumEnd);
                if (!string.IsNullOrEmpty(refid)) query = query.Where(o => o.TradeCode.Contains(refid));
                if (!string.IsNullOrEmpty(accChgType)) query = query.Where(o => o.ChangeTypeValue == accChgType);

                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }

        //public static Result GetMemAccountDetailChangeHistory(string dp, long detailId)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (var db = new CRMEntities())
        //    {
        //        var query = from c in db.TL_Mem_AccountChange
        //                    join u in db.TM_AUTH_User on c.AddedUser equals SqlFunctions.StringConvert((double)u.UserID).Trim()
        //                    join o in db.TD_SYS_BizOption.Where(o => o.OptionType == "AccountChangeType") on c.AccountChangeType equals o.OptionValue
        //                    where c.AccountDetailID == detailId && c.HasReverse == false
        //                    select new
        //                    {
        //                        c.LogID,
        //                        c.AccountDetailID,
        //                        c.AccountID,
        //                        c.AddedDate,
        //                        c.ChangeReason,
        //                        c.ChangeValue,
        //                        c.ReferenceNo,
        //                        ChangeUserName = u.UserName ?? "",
        //                        ChangeTypeText = o.OptionText ?? ""
        //                    };
        //        return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
        //    }
        //}
        public static Result GetMemAccountCoupon(string mid)
        {
            using (var db = new CRMEntities())
            {
                var query = (from a in db.TM_Mem_CouponPool.AsNoTracking()
                             where a.MemberID == mid
                             select a).ToList();
                int valid = query.Where(o => o.IsUsed == false && (o.EndDate == null || DateTime.Now.AddDays(-1) <= o.EndDate)).Count();
                int used = query.Where(o => o.IsUsed == true).Count();
                int expired = query.Where(o => o.IsUsed == false && DateTime.Now.AddDays(-1) > o.EndDate).Count();
                return new Result(true, "", new List<object> { valid, used, expired });
            }
        }



        public static Result GetMemChartData(string mid)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var query = from a in db.V_U_TM_Mem_Info
                                where a.MemberID == mid
                                select new
                                {
                                    promotion = (string.IsNullOrEmpty(a.PromotionLike_1N) ? "" : a.PromotionLike_1N)
                                    + (string.IsNullOrEmpty(a.PromotionLike_2N) ? "" : ("," + a.PromotionLike_2N))
                                    + (string.IsNullOrEmpty(a.PromotionLike_3N) ? "" : ("," + a.PromotionLike_3N)),
                                    shoptime = (string.IsNullOrEmpty(a.OftenBuyTime_1N) ? "" : a.OftenBuyTime_1N)
                                    + (string.IsNullOrEmpty(a.OftenBuyTime_2N) ? "" : ("," + a.OftenBuyTime_2N))
                                    + (string.IsNullOrEmpty(a.OftenBuyTime_3N) ? "" : ("," + a.OftenBuyTime_3N))
                                    + (string.IsNullOrEmpty(a.OftenBuyTime_4N) ? "" : ("," + a.OftenBuyTime_4N))
                                    + (string.IsNullOrEmpty(a.OftenBuyTime_5N) ? "" : ("," + a.OftenBuyTime_5N)),
                                    shopstore = (string.IsNullOrEmpty(a.StoreOftenGo_1N) ? "" : a.StoreOftenGo_1N)
                                    + (string.IsNullOrEmpty(a.StoreOftenGo_2N) ? "" : ("," + a.StoreOftenGo_2N))
                                    + (string.IsNullOrEmpty(a.StoreOftenGo_3N) ? "" : ("," + a.StoreOftenGo_3N)),
                                    shopbrand = (string.IsNullOrEmpty(a.BrandLike_1N) ? "" : a.BrandLike_1N)
                                    + (string.IsNullOrEmpty(a.BrandLike_2N) ? "" : ("," + a.BrandLike_2N))
                                    + (string.IsNullOrEmpty(a.BrandLike_3N) ? "" : ("," + a.BrandLike_3N))
                                    + (string.IsNullOrEmpty(a.BrandLike_4N) ? "" : ("," + a.BrandLike_4N))
                                    + (string.IsNullOrEmpty(a.BrandLike_5N) ? "" : ("," + a.BrandLike_5N)),
                                    shoptype = (string.IsNullOrEmpty(a.ProductTypeLike_1N) ? "" : a.ProductTypeLike_1N)
                                    + (string.IsNullOrEmpty(a.ProductTypeLike_2N) ? "" : ("," + a.ProductTypeLike_2N))
                                    + (string.IsNullOrEmpty(a.ProductTypeLike_3N) ? "" : ("," + a.ProductTypeLike_3N))
                                    + (string.IsNullOrEmpty(a.ProductTypeLike_4N) ? "" : ("," + a.ProductTypeLike_4N))
                                    + (string.IsNullOrEmpty(a.ProductTypeLike_5N) ? "" : ("," + a.ProductTypeLike_5N)),
                                };

                    return new Result(true, "", query.ToList());
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        //public static Result GetMemAccountCouponList(string dp, string mid)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (var db = new CRMEntities())
        //    {
        //        var query1 = (from l in db.TM_Mem_CouponLimit
        //                      join a in db.TM_Mem_CouponPool on l.CouponID equals a.CouponID
        //                      join o in db.TD_SYS_BizOption on l.LimitType equals o.OptionValue
        //                      join vtemp in db.V_M_TM_Mem_SubExt_vehicle on l.LimitValue equals SqlFunctions.StringConvert((double)vtemp.MemberSubExtID)
        //                      into vt
        //                      from v in vt.DefaultIfEmpty()
        //                      where a.MemberID == mid && l.LimitType == "vehicle" && o.OptionType == "CouponLimitType"
        //                      orderby l.CouponLimitID
        //                      select new
        //                      {
        //                          l.CouponLimitID,
        //                          l.CouponID,
        //                          LimitType = o.OptionText,
        //                          LimitValue = (v.CarNo ?? "") + "," + (v.VIN ?? "")
        //                      })
        //                      .Union(from l in db.TM_Mem_CouponLimit
        //                             join a in db.TM_Mem_CouponPool on l.CouponID equals a.CouponID
        //                             join o in db.TD_SYS_BizOption on l.LimitType equals o.OptionValue
        //                             join vtemp in db.V_M_TM_SYS_BaseData_store on l.LimitValue equals vtemp.StoreCode
        //                             into vt
        //                             from v in vt.DefaultIfEmpty()
        //                             where a.MemberID == mid && l.LimitType == "store" && o.OptionType == "CouponLimitType"
        //                             orderby l.CouponLimitID
        //                             select new
        //                             {
        //                                 l.CouponLimitID,
        //                                 l.CouponID,
        //                                 LimitType = o.OptionText,
        //                                 LimitValue = v.StoreName
        //                             })
        //                        .Union(from l in db.TM_Mem_CouponLimit
        //                               join a in db.TM_Mem_CouponPool on l.CouponID equals a.CouponID
        //                               join o in db.TD_SYS_BizOption on l.LimitType equals o.OptionValue
        //                               join vtemp in db.V_M_TM_SYS_BaseData_brand on l.LimitValue equals vtemp.BrandCode
        //                               into vt
        //                               from v in vt.DefaultIfEmpty()
        //                               where a.MemberID == mid && l.LimitType == "brand" && o.OptionType == "CouponLimitType"
        //                               orderby l.CouponLimitID
        //                               select new
        //                               {
        //                                   l.CouponLimitID,
        //                                   l.CouponID,
        //                                   LimitType = o.OptionText,
        //                                   LimitValue = v.BrandName
        //                               })
        //                      .ToList();
        //        List<CouponLimit> limits = new List<CouponLimit>();
        //        foreach (var cid in query1.Select(o => o.CouponID).Distinct())
        //        {
        //            string str = "";
        //            foreach (var l in query1.Where(o => o.CouponID == cid))
        //            {
        //                str += l.LimitType + ":" + l.LimitValue + "; ";
        //            }
        //            limits.Add(new CouponLimit { CouponID = cid, LimitText = str });
        //        }
        //        //if (limits.Count == 0)
        //        //{
        //        //    limits.Add(new CouponLimit { CouponID = 0, LimitText = "" });
        //        //}

        //        var query2 = (from a in db.TM_Mem_CouponPool
        //                      join t in db.TM_Act_CommunicationTemplet on a.TempletID equals t.TempletID into ta
        //                      from taa in ta.DefaultIfEmpty()
        //                      join o in db.TD_SYS_BizOption.Where(o => o.OptionType == "CouponType") on a.CouponType equals o.OptionValue
        //                      where a.MemberID == mid
        //                      select new
        //                      {
        //                          a.CouponID,
        //                          a.CouponCode,
        //                          CouponName = taa.Name,
        //                          a.CouponType,
        //                          CouponTypeText = o.OptionText,
        //                          a.Enable,
        //                          a.EndDate,
        //                          a.IsUsed,
        //                          a.MemberID,
        //                          a.StartDate,
        //                          a.AddedDate
        //                      }).ToList();
        //        var query = (from q in query2
        //                     join ltemp in limits on q.CouponID equals ltemp.CouponID into lt
        //                     from l in lt.DefaultIfEmpty()
        //                     select new
        //                     {
        //                         q.CouponID,
        //                         q.CouponCode,
        //                         q.CouponName,
        //                         q.CouponType,
        //                         q.CouponTypeText,
        //                         q.Enable,
        //                         q.EndDate,
        //                         q.IsUsed,
        //                         q.MemberID,
        //                         q.StartDate,
        //                         q.AddedDate,
        //                         LimitText = l == null ? "" : l.LimitText ?? ""
        //                     }).ToList();
        //        return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
        //    }
        //}
        public static Result GetMemAccountCouponList1(string dp, string mid)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (var db = new CRMEntities())
            {


                var query = from a in db.TM_Mem_CouponPool
                            join b in db.TM_Act_CommunicationTemplet on a.TempletID equals b.TempletID into ta
                            from taa in ta.DefaultIfEmpty()
                            join o in db.TD_SYS_BizOption.Where(o => o.OptionType == "CouponType") on a.CouponType equals o.OptionValue
                            where a.MemberID == mid
                            select new
                            {
                                a.CouponID,
                                taa.TempletID,
                                taa.Name,
                                o.OptionText,
                                a.Enable,
                                a.IsUsed,
                                a.StartDate,
                                a.EndDate,
                                a.AddedDate,
                            };
                var q = query.ToList();
                //优惠券限制
                List<CouponLimit> limits = new List<CouponLimit>();
                if (q.Count > 0)
                {

                    for (int i = 0; i < q.Count; i++)
                    {
                        var pid = q[i].CouponID;
                        //var q1 = db.TM_Mem_CouponLimit.Where(p => p.CouponID == pid).ToList();
                        //var q3 = (from a in db.TM_Mem_CouponLimit
                        //          join b in db.V_M_TM_SYS_BaseData_store on a.LimitValue equals b.StoreCode into ba
                        //          from baa in ba.DefaultIfEmpty()
                        //          join c in db.V_M_TM_SYS_BaseData_brand on a.LimitValue equals c.BrandCode into ca
                        //          from caa in ca.DefaultIfEmpty()
                        //          join o in db.TD_SYS_BizOption on a.LimitType equals o.OptionValue
                        //          where a.CouponID == pid && o.OptionType == "CouponLimitType"
                        //          orderby a.CouponLimitID
                        //          select new
                        //          {
                        //              a.LimitValue,
                        //              a.LimitType,
                        //              LimitTypeText = o.OptionText,
                        //              StoreName = baa.StoreName,
                        //              caa.BrandName,
                        //          }).ToList();
                        string str = "";
                        //foreach (var item in q3)
                        //{
                        //    if (item.LimitType == "store")
                        //    {
                        //        if (!str.Contains(item.StoreName))
                        //            str += item.LimitTypeText + ":" + item.StoreName + "; ";
                        //    }
                        //    else if (item.LimitType == "brand")
                        //    {
                        //        if (!str.Contains(item.BrandName))
                        //            str += item.LimitTypeText + ":" + item.BrandName + "; ";
                        //    }
                        //    else
                        //    {
                        //        str += item.LimitTypeText + ":" + item.LimitValue + "; ";
                        //    }
                        //}
                        CouponLimit li = new CouponLimit() { CouponID = pid, LimitText = str };
                        if (!limits.Select(p => p.CouponID).ToList().Contains(li.CouponID)) { limits.Add(li); };
                    }
                }
                var q2 = (from a in q
                          join ltemp in limits on a.CouponID equals ltemp.CouponID into lt
                          from l in lt.DefaultIfEmpty()
                          select new
                          {
                              CouponName = a.Name,
                              CouponTypeText = a.OptionText,
                              a.Enable,
                              a.IsUsed,
                              a.StartDate,
                              a.EndDate,
                              a.AddedDate,
                              LimitText = l == null ? "" : l.LimitText ?? ""
                          }).ToList();
                return new Result(true, "", q2.ToDataTableSourceVsPage(myDp));
            }
        }
        public static Result GetMemberCommunicateHistory(string dp, string mid, DateTime? start, DateTime? end)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (var db = new CRMEntities())
            {
                var query = from a in db.TL_Act_Communication
                            join o1 in db.TD_SYS_BizOption.Where(o => o.OptionType == "CommunicationStatus") on SqlFunctions.StringConvert((double)a.Status).Trim() equals o1.OptionValue
                            join btemp in db.TM_Act_CommunicationTemplet on a.TempletID equals btemp.TempletID
                            into bt
                            from b in bt.DefaultIfEmpty()
                            join o2temp in db.TD_SYS_BizOption.Where(o => o.OptionType == "CommunicationType") on b.Category equals o2temp.OptionValue
                            into o2t
                            from o2 in o2t.DefaultIfEmpty()
                            join o in db.TD_SYS_BizOption.Where(o => o.OptionType == "CommType") on a.CommType equals o.OptionValue
                           into oo
                            from ooo in oo.DefaultIfEmpty()
                            where a.MemberID == mid &&
                                (start != null ? (a.OccurTime >= start) : true) && (end != null ? a.OccurTime <= end : true)
                            orderby a.OccurTime descending
                            select new
                            {
                                a.ContentDesc,
                                a.Direction,
                                a.LogID,
                                a.MemberID,
                                a.OccurTime,
                                a.ReferenceActID,
                                a.Status,
                                StatusText = o1.OptionText,
                                a.TempletID,
                                a.AddedUser,
                                TempletName = b.Name ?? "",
                                TempletCategoryText = o2.OptionText,
                                CommType = ooo.OptionText
                            };
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });

            }
        }

        /// <summary>
        /// 获取积分历史
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="mid"></param>
        /// <param name="aid"></param>
        /// <returns></returns>
        public static Result GetMemberActivity(string dp, string mid, string aid)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (var db = new CRMEntities())
            {
                var acts = (from a in db.TR_Mem_MarketActivity
                            where a.MemberID == mid && SqlFunctions.StringConvert((double)a.ActivityID).Contains(aid)
                            orderby a.ActivityInstanceID descending
                            select a).ToList();
                return new Result(true, "", new List<object> { acts.ToDataTableSourceVsPage(myDp) });
            }
        }

        public static Result GetMemberCardList(string mid)
        {
            using (var db = new CRMEntities())
            {
                var query = from a in db.TM_Mem_Card
                            where a.MemberID == mid
                            select a;
                return new Result(true, "", new List<object> { query.ToList() });
            }
        }
        public static Result GetMemberCardChangeHistory(string dp, string cardNo)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (var db = new CRMEntities())
            {
                var query = from a in db.TL_Mem_CardChange
                            join o in db.TD_SYS_BizOption.Where(o => o.OptionType == "CardChangeType") on a.ChangeType equals o.OptionValue
                            join u in db.TM_AUTH_User on a.AddedUser equals SqlFunctions.StringConvert((double)u.UserID).Trim()
                            where a.CardNo == cardNo
                            orderby a.ChangeTime descending
                            select new
                            {
                                a.AddedDate,
                                a.AddedUser,
                                ChangeUserName = u.UserName,
                                a.CardNo,
                                a.ChangePlace,
                                a.ChangeTime,
                                a.ChangeType,
                                ChangeTypeText = o.OptionText,
                                a.LogID
                            };
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }

        public static Result GetMemCoupon(string dp, string mem_id, string start, string end, string couponcode, string coupontype, string isvalid, string isused, string couponname)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (var db = new CRMEntities())
            {
                var coupon = from a in db.TM_Mem_CouponPool
                             where a.MemberID == mem_id
                             select a;
                var acts = from a in coupon
                           join b in db.TM_Act_CommunicationTemplet
                           on a.TempletID equals b.TempletID into temp
                           from c in temp.DefaultIfEmpty()
                           where (couponcode != null ? (a.CouponCode.Contains(couponcode)) : true) && (couponname != null ? (c.Name.Contains(couponname)) : true)
                           select new { a.CouponID, a.CouponCode, a.CouponType, a.StartDate, a.EndDate, a.Enable, a.IsUsed, c.Name, c.BasicContent, a.AddedDate };
                acts = string.IsNullOrEmpty(coupontype) ? acts : acts.Where(p => p.CouponType == coupontype);
                DateTime startdate = string.IsNullOrEmpty(start) ? DateTime.Now : DateTime.Parse(start);
                DateTime enddate = string.IsNullOrEmpty(end) ? DateTime.Now : DateTime.Parse(end);
                acts = string.IsNullOrEmpty(start) ? acts : acts.Where(p => p.EndDate >= startdate);
                acts = string.IsNullOrEmpty(end) ? acts : acts.Where(p => p.StartDate <= enddate);
                acts = isvalid == "" ? acts : isvalid == "0" ? acts.Where(p => p.Enable == false || p.IsUsed == true) : acts.Where(p => p.Enable == true && p.StartDate < DateTime.Now && p.EndDate > DateTime.Now);
                acts = isused == "" ? acts : isused == "0" ? acts.Where(p => p.IsUsed == false) : acts.Where(p => p.IsUsed == true);
                return new Result(true, "", new List<object> { acts.ToDataTableSourceVsPage(myDp) });
            }
        }


        public static Result SaveMemberSMS(SMSSendModel model, string userid)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    V_S_TM_Mem_Ext ext = JsonHelper.Deserialize<V_S_TM_Mem_Ext>(model.MemberInfo);
                    TM_Sys_SMSSendingQueue sms = new TM_Sys_SMSSendingQueue();
                    sms.Mobile = ext.CustomerMobile;
                    sms.Message = model.SMSInfo;
                    sms.MemberID = model.MemberId;
                    sms.AddedDate = DateTime.Now;
                    sms.AddedUser = userid;
                    db.TM_Sys_SMSSendingQueue.Add(sms);
                    db.SaveChanges();
                    return new Result(true, "保存成功");
                }
            }
            catch
            {
                return new Result(false, "保存失败");
            }
        }


        #endregion

        #region 自动生成页面
        public static Result GetTagandMemInfo(string memid, int pageid)
        {
            using (var db = new CRMEntities())
            {
                //显示Tab
                var querytab = db.TM_SYS_BlockType.Where(p => p.PageID == pageid && p.IsUsed == true).OrderBy(p => p.Sort).ToList();
                //显示Table表头列名
                var querytable = db.TD_SYS_FieldAlias.ToList();
                var querytag = from p in db.TM_SYS_BlockSetting
                               where p.PageID == pageid
                               join fileds in db.TD_SYS_FieldAlias on p.FieldID equals fileds.AliasID into temp
                               from q in temp.DefaultIfEmpty()
                               select new
                               {
                                   q.FieldDesc,
                                   q.FieldAlias,
                                   p.DisplayName,
                                   p.Format,
                                   p.BlockCode,
                                   p.Sort,
                                   p.Span,
                                   p.BlockType,
                                   p.IsUpdate,
                                   BReg = p.Reg,
                                   q.TableName,
                                   q.ViewName,
                                   q.FieldName,
                                   q.ControlType,
                                   q.DictTableName,
                                   q.DictTableType,
                                   FReg = q.Reg,
                                   p.FuncName,
                                   p.FuncContent,
                                   BErrorTip = p.ErrorTip,
                                   FErrorTip = p.ErrorTip,
                                   p.Required,
                                   p.MaxLength,
                               };

                var querymeminfo = from p in db.V_U_TM_Mem_Info
                                   where p.MemberID == memid
                                   select p;
                return new Result(true, "", new List<object> { querytag.ToList(), querymeminfo.FirstOrDefault(), querytab });
            }
        }
        #endregion

        #region 账户调整
        /// <summary>
        /// 获取会员姓名列表
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="cardNo"></param>
        /// <param name="memName"></param>
        /// <param name="memMobile"></param>
        /// <param name="vehicleNo"></param>
        /// <param name="vehicleVIN"></param>
        /// <returns></returns>
        public static Result GetMemberNameByPage(string dp, string memNo, string memName, string memMobile, string rds, int pid)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {

                StringBuilder ruleSql = new StringBuilder();
                ruleSql.AppendLine(" select  t1.* from V_U_TM_Mem_Info t1");
                List<string> rid = JsonHelper.Deserialize<List<string>>(rds);
                StringBuilder limitSql = new StringBuilder();
                limitSql.AppendLine("select distinct MemberID from TR_Mem_MemberLimit ");
                var limlst = db.TM_AUTH_DataLimit.Where(p => p.HierarchyType == "role" && rid.Contains(p.HierarchyValue)
                             && (p.PageID == 9999 || p.PageID == pid)).ToList();

                if (limlst.Count > 0)
                {
                    string limwhere = "";
                    foreach (var item in limlst)
                    {
                        if (limwhere == "")
                            limwhere = " where " + item.RangeType + " = '" + item.RangeValue + "'";
                        else
                            limwhere += " or " + item.RangeType + " = '" + item.RangeValue + "'";
                    }
                    limitSql.AppendLine(limwhere);
                    ruleSql.AppendLine(" inner join (" + limitSql + ")l on t1.MemberID = l.MemberID");
                }
                string where = "Where 1=1 and t1.CustomerStatus=1 ";
                if (!string.IsNullOrEmpty(memNo))
                {
                    where += " and t1.MemberCardNo like '%" + memNo.ToUpper() + "%'";
                }
                if (!string.IsNullOrEmpty(memName))
                {
                    where += " and t1.CustomerName like '%" + memName.ToUpper() + "%'";
                }
                if (!string.IsNullOrEmpty(memMobile))
                {
                    where += " and t1.Mobile like '%" + memMobile + "%'";
                }
                ruleSql.AppendLine(where);
                var query = db.Database.SqlQuery<V_U_TM_Mem_Info>(ruleSql.ToString(), DBNull.Value).ToList();
                if (query.Count == 0)
                {
                    if (!string.IsNullOrEmpty(memNo) || !string.IsNullOrEmpty(memMobile))
                    {
                        query = db.V_U_TM_Mem_Info.Where(p => p.CustomerStatus == "1").ToList();
                        if (!string.IsNullOrEmpty(memNo)) query = query.Where(p => p.MemberCardNo.ToUpper() == memNo).ToList();
                        //if (!string.IsNullOrEmpty(memName)) query = query.Where(p => p.CustomerName.Contains(memName)).ToList();
                        if (!string.IsNullOrEmpty(memMobile)) query = query.Where(p => p.Mobile != null && p.Mobile == memMobile).ToList();
                    }
                }


                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }
        /// <summary>
        /// 更新账户信息
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="detailType"></param>
        /// <param name="oper"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static Result SaveImproveActInfo(string mid, string detailType, string operation, decimal changeValue, string did, string actId, string actType, int userId)
        {
            //#region 调用触发器
            //using (CRMEntities db1 = new CRMEntities())
            //{
            //    var dids = new List<string>();
            //    dids.Add(did);
            //    var aids = new List<string>();
            //    aids.Add(actId);

            //    var el = new ExtraAccount
            //    {
            //        db = db1,
            //        AccountIDs = aids,
            //        AccountType = actType,
            //        AccountDetailType = detailType,
            //        AccountDetailIDs = dids,
            //        ChangeValue = changeValue,
            //        Operation = operation
            //    };

            //    AccountTrigger tr = new AccountTrigger(el);
            //    //tr.MemberScript = "Select MemberID From TM_Mem_Master";
            //    tr.MemberIDs.Add(mid);
            //    tr.StartTime = DateTime.Now;
            //    //tr.Callback = new AccountTrigger.CallBackMethod(gothisway);
            //    tr.Start();
            //    return new Result(true, "调整成功");
            //}
            //#endregion

            #region 原始代码
            using (CRMEntities db = new CRMEntities())
            {
                //账户调整代码
                var query1 = db.TM_Mem_Account.Where(p => p.MemberID == mid && p.AccountType == actType).FirstOrDefault();
                if (query1 == null) return new Result(false, "此用户还未建立账户，请先新建");
                //会员某个类型的账户明细
                var query2 = db.TM_Mem_AccountDetail.Where(p => p.AccountID == query1.AccountID);

                //调整一个账户增加的时候判断限制，判断时间，如果没有就要新增
                if (operation == "add")
                {
                    //日志
                    TL_Mem_AccountChange entt = new TL_Mem_AccountChange();
                    entt.AccountID = query1.AccountID;
                    entt.AccountDetailID = "0";
                    entt.AccountChangeType = "manu";
                    entt.ChangeValue = changeValue;
                    entt.ChangeReason = "总额直接调整";
                    entt.HasReverse = false;
                    entt.AddedDate = DateTime.Now;
                    entt.AddedUser = userId.ToString();
                    db.TL_Mem_AccountChange.Add(entt);

                    if (detailType == "value1")//如果是可用类型的话，直接操作
                    {
                        query1.Value1 += changeValue;
                        //日志

                        var query3 = query2.Where(p => p.SpecialDate1 == null && p.SpecialDate2 == null).FirstOrDefault();
                        if (query3 != null)
                        {
                            query3.DetailValue = query3.DetailValue + changeValue;
                            var entry = db.Entry(query3);
                            entry.State = EntityState.Modified;

                            db.SaveChanges();
                            return new Result(true, "修改成功");
                        }
                        else//新增明细
                        {
                            TM_Mem_AccountDetail ent = new TM_Mem_AccountDetail();
                            ent.AccountID = actId;
                            ent.AccountDetailType = detailType;
                            ent.DetailValue = changeValue;

                            ent.AddedDate = DateTime.Now;
                            ent.AddedUser = userId.ToString();
                            ent.ModifiedDate = DateTime.Now;
                            ent.ModifiedUser = userId.ToString();

                            db.TM_Mem_AccountDetail.Add(ent);
                            db.SaveChanges();
                            return new Result(true, "修改成功");
                        }

                    }
                    else if (detailType == "value2")//如果是冻结类型的话，找出时间最长远的
                    {
                        query1.Value2 += changeValue;
                        //日志
                        var query4 = query2.Where(p => p.AccountDetailType == "value2").OrderByDescending(p => p.SpecialDate1).ToList();
                        if (query4.Count > 0)
                        {
                            query4[0].DetailValue = query4[0].DetailValue + changeValue;
                            var entry = db.Entry(query4[0]);
                            entry.State = EntityState.Modified;
                            db.SaveChanges();
                            return new Result(true, "修改成功");
                        }
                        else
                        {
                            TM_Mem_AccountDetail ent = new TM_Mem_AccountDetail();
                            ent.AccountID = actId;
                            ent.AccountDetailType = detailType;
                            ent.DetailValue = changeValue;

                            ent.AddedDate = DateTime.Now;
                            ent.AddedUser = userId.ToString();
                            ent.ModifiedDate = DateTime.Now;
                            ent.ModifiedUser = userId.ToString();

                            db.TM_Mem_AccountDetail.Add(ent);
                            db.SaveChanges();
                            return new Result(true, "修改成功");
                        }
                    }
                    else
                    {
                        return new Result(false, "修改失败");
                    }
                }
                else if (operation == "sub")
                {

                    if (detailType == "value1")//如果是可用类型的话，直接操作
                    {
                        query2 = query2.Where(p => p.AccountDetailType == "value1").OrderBy(p => p.SpecialDate2);
                        var query21 = query2.Where(p => p.SpecialDate2 == null);
                        var query22 = query2.Where(p => p.SpecialDate2 != null);
                        var query23 = query22.Concat(query21).ToList();
                        if (query1.Value1 < changeValue) { return new Result(false, "可用积分余额不足，不允许扣减"); }
                        query1.Value1 -= changeValue;
                        //日志
                        TL_Mem_AccountChange entt = new TL_Mem_AccountChange();
                        entt.AccountID = query1.AccountID;
                        entt.MemberID = query1.MemberID;
                        entt.AccountDetailID = "0";
                        entt.AccountChangeType = "manu";
                        entt.ChangeValue = -changeValue;
                        entt.ChangeReason = "总额直接调整";
                        entt.HasReverse = false;
                        entt.AddedDate = DateTime.Now;
                        entt.AddedUser = userId.ToString();
                        db.TL_Mem_AccountChange.Add(entt);

                        SubActOperation(changeValue, db, query23);
                        return new Result(true, "修改成功");
                        //var query3 = query2.ToList();
                        //for (int i = 0; i < query3.Count; i++)
                        //{
                        //    if (query3[i].AccountDetailType == detailType)
                        //    {
                        //        query3[i].DetailValue = query3[i].DetailValue - changeValue;
                        //        var entry = db.Entry(query3[i]);
                        //        entry.State = EntityState.Modified;
                        //        db.SaveChanges();
                        //        return new Result(true, "修改成功");
                        //    }
                        //}
                    }
                    //减少的时候判断时间，从快过期的账户减，然后多出来的话，从第二个快过期的减。
                    else if (detailType == "value2")//如果是冻结类型的话，找出时间最长远的
                    {
                        if (query1.Value2 < changeValue) { return new Result(false, "冻结积分余额不足，不允许扣减"); }
                        query1.Value2 -= changeValue;
                        //日志
                        TL_Mem_AccountChange entt = new TL_Mem_AccountChange();
                        entt.AccountID = query1.AccountID;
                        entt.MemberID = query1.MemberID;
                        entt.AccountDetailID = "0";
                        entt.AccountChangeType = "manu";
                        entt.ChangeValue = changeValue;
                        entt.ChangeReason = "总额直接调整";
                        entt.HasReverse = false;
                        entt.AddedDate = DateTime.Now;
                        entt.AddedUser = userId.ToString();
                        db.TL_Mem_AccountChange.Add(entt);
                        //if (query1.Value2 <= 0) { query1.Value2 = 0; }
                        query2 = query2.Where(p => p.AccountDetailType == "value2").OrderBy(p => p.SpecialDate1);
                        SubActOperation(changeValue, db, query2.ToList());
                        return new Result(true, "修改成功");
                    }
                    else
                    {
                        return new Result(false, "修改失败");
                    }
                }
                else
                {
                    return new Result(false, "修改失败");
                }
            }
            #endregion
        }
        public static void gothisway(Result rst)
        {
            object b = rst.Obj[0];

        }

        /// <summary>
        /// 递归减去积分现金积点
        /// </summary>
        /// <param name="changeValue"></param>
        /// <param name="db"></param>
        /// <param name="query2"></param>
        private static void SubActOperation(decimal changeValue, CRMEntities db, List<TM_Mem_AccountDetail> query4)
        {
            //var query4 = query2.ToList();
            //for (int i = 0; i < query4.Count; i++)
            //{
            if (query4.Count > 0)
            {
                if ((query4[0].DetailValue - changeValue) > 0)
                {
                    query4[0].DetailValue = query4[0].DetailValue - changeValue;
                    var entry = db.Entry(query4[0]);
                    entry.State = EntityState.Modified;
                    db.SaveChanges();
                    //break;
                    //return new Result(true, "修改成功");
                }
                else //if ((query4[i].DetailValue - changeValue) <= 0)
                {
                    changeValue = changeValue - query4[0].DetailValue;
                    db.TM_Mem_AccountDetail.Remove(query4[0]);
                    db.SaveChanges();
                    query4.Remove(query4[0]);
                    if (changeValue > 0)
                        SubActOperation(changeValue, db, query4);
                    //return new Result(true, "修改成功");
                }
            }
            //break;
            //else
            //{
            //    SubActOperation(changeValue - query4[i].DetailValue, db, query2);
            //}
            //}
        }

        /// <summary>
        /// 根据id获取账户明细信息
        /// </summary>
        /// <param name="detailId"></param>
        /// <returns></returns>
        public static Result GetActDetailById(string detailId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                //var query = db.TM_Mem_AccountDetail.Where(p => p.AccountDetailID == detailId).FirstOrDefault();
                //获取账户的使用限制
                var query = from a in db.TM_Mem_AccountDetail
                            join b in db.TM_Mem_AccountLimit on a.AccountDetailID equals b.AccountDetailID into bg
                            from bgg in bg.DefaultIfEmpty()
                            where a.AccountDetailID == detailId
                            select new
                            {
                                a.AccountDetailID,
                                a.AccountDetailType,
                                a.AccountID,
                                a.DetailValue,
                                a.SpecialDate1,
                                a.SpecialDate2,

                                LimitType = bgg.LimitType ?? "",
                                LimitValue = bgg.LimitValue ?? "",
                            };
                return new Result(true, "", query.ToList());
            }
        }
        /// <summary>
        /// 更新账户明细信息
        /// </summary>
        /// <param name="did"></param>
        /// <param name="detailType"></param>
        /// <param name="num"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static Result SaveEditActInfoById(string did, string detailType, decimal num, DateTime? startDate, DateTime? endDate, int userId, string oper, string reason, int groupId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                using (db.BeginTransaction())
                {
                    var storeCode = Service.GetStoreName(userId).Obj[0].ToString();
                    var storeName = Service.GetStoreNameByCode(storeCode).Obj[0].ToString();

                    var query = db.TM_Mem_AccountDetail.Where(p => p.AccountDetailID == did).FirstOrDefault();
                    var actID = query.AccountID;
                    var actDetailID = query.AccountDetailID;
                    decimal temp = 0;//如果账户中金额不够扣，则记录一下，改变金额，对账的时候能对上
                    if (query != null)
                    {

                        if (oper == "add")
                        {
                            query.DetailValue = query.DetailValue + num;
                        }
                        if (oper == "sub")
                        {
                            temp = query.DetailValue;
                            query.DetailValue = query.DetailValue - num;
                        }
                        if (query.DetailValue <= 0)
                        {
                            num = temp;
                            query.DetailValue = 0;
                            var entry = db.Entry(query);
                            entry.State = EntityState.Modified;
                            //db.TM_Mem_AccountDetail.Remove(query);
                            //db.SaveChanges();
                            //return new Result(true, "此帐户金额减至0，自动删除");
                        }
                        else
                        {
                            query.AccountDetailType = detailType;
                            query.SpecialDate1 = startDate;
                            query.SpecialDate2 = endDate;
                            query.ModifiedDate = DateTime.Now;
                            query.ModifiedUser = userId.ToString();

                            var entry = db.Entry(query);
                            entry.State = EntityState.Modified;
                        }
                        //db.SaveChanges();

                        //还要更改帐户总额
                        //To Do
                        var q1 = db.TM_Mem_Account.Where(p => p.AccountID == actID).FirstOrDefault();

                        if (query.AccountDetailType == "value1" && oper == "add")
                        {
                            q1.Value1 = q1.Value1 + num;
                        }
                        if (query.AccountDetailType == "value1" && oper == "sub")
                        {
                            q1.Value1 = (q1.Value1 - num) < 0 ? 0 : (q1.Value1 - num);
                            q1.NoBackAccount = (q1.NoBackAccount == null ? 0 : q1.NoBackAccount - num) < 0 ? 0 : (q1.NoBackAccount == null ? 0 : q1.NoBackAccount - num);
                        }
                        if (query.AccountDetailType == "value2" && oper == "add")
                        {
                            q1.Value2 = q1.Value2 + num;
                        }
                        if (query.AccountDetailType == "value2" && oper == "sub")
                        {
                            q1.Value2 = (q1.Value2 - num) < 0 ? 0 : (q1.Value2 - num);
                        }
                        var entry1 = db.Entry(q1);
                        entry1.State = EntityState.Modified;
                        //db.SaveChanges();

                        //还要记录帐户更改
                        //To Do
                        TL_Mem_AccountChange ent = new TL_Mem_AccountChange();
                        ent.AccountID = actID;
                        ent.AccountDetailID = did;
                        ent.AccountChangeType = "manu";
                        ent.MemberID = q1.MemberID;
                        if (oper == "add")
                        {
                            ent.ChangeValue = num;
                        }
                        if (oper == "sub")
                        {
                            ent.ChangeValue = -num;
                        }
                        ent.ChangeReason = reason;
                        ent.HasReverse = false;
                        ent.AddedDate = DateTime.Now;
                        ent.AddedUser = userId.ToString();
                        db.TL_Mem_AccountChange.Add(ent);
                        // db.SaveChanges();

                        //V_M_TM_Mem_Trade_charge
                        //if (oper == "add")
                        //{
                        //    V_M_TM_Mem_Trade_charge charge = new V_M_TM_Mem_Trade_charge()
                        //    {
                        //        DataGroupID = groupId,
                        //        TradeCode = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString(),
                        //        TradeType = "charge",
                        //        MemberID = q1.MemberID,
                        //        NeedLoyCompute = true,
                        //        ChargeValueTrade = num,
                        //        IsInvoiceCharge = true,
                        //        ChargeTypeTrade = q1.AccountType,
                        //        RechargeTimeTrade = DateTime.Now,

                        //        StoreCodeRechargeTrade = storeCode,
                        //        StoreNameRechargeTrade = storeName,
                        //        AddedDate = DateTime.Now,
                        //        AddedUser = userId.ToString(),
                        //        ModifiedDate = DateTime.Now,
                        //        ModifiedUser = userId.ToString()
                        //    };
                        //    dynamic t = db.AddViewRow<V_M_TM_Mem_Trade_charge, TM_Mem_Trade>(charge);
                        //}

                        //更改帐户限制
                        //if (actLimit != "null")
                        //{
                        //    var q3 = db.TM_Mem_AccountLimit.Where(p => p.AccountDetailID == actDetailID).ToList();
                        //    if (q3.Count > 0)
                        //    {
                        //        for (int i = 0; i < q3.Count; i++)
                        //        {
                        //            db.TM_Mem_AccountLimit.Remove(q3[i]);

                        //        }
                        //        //db.SaveChanges();
                        //    }
                        //    List<ActLimit> limit = JsonHelper.Deserialize<List<ActLimit>>(actLimit);
                        //    for (int i = 0; i < limit.Count; i++)
                        //    {
                        //        var liType = limit[i].LimitType;
                        //        var liValue = limit[i].LimitValue;
                        //        //var q2 = db.TM_Mem_AccountLimit.Where(p => p.AccountID == actID && p.LimitType == liType && p.LimitValue == liValue).ToList();
                        //        //if (q2.Count <= 0)
                        //        //{
                        //        TM_Mem_AccountLimit model = new TM_Mem_AccountLimit();
                        //        model.AccountID = actID;
                        //        model.AccountDetailID = actDetailID;
                        //        model.LimitValue = liValue;
                        //        model.LimitType = liType;
                        //        model.AddedDate = DateTime.Now;
                        //        db.TM_Mem_AccountLimit.Add(model);
                        //        //}
                        //    }
                        //}
                        //else //删除以前的限制
                        //{
                        //    var q3 = db.TM_Mem_AccountLimit.Where(p => p.AccountDetailID == actDetailID).ToList();
                        //    if (q3.Count > 0)
                        //    {
                        //        foreach (var item in q3)
                        //        {
                        //            db.TM_Mem_AccountLimit.Remove(item);
                        //            //db.SaveChanges();
                        //        }
                        //    }
                        //}
                        db.SaveChanges();
                        //处理实时积分维度  2815 -累计获取积分 2782 -半年内累计获取积分
                        MemberSubdivision.ComputeDimension(new List<string> { q1.MemberID }, DateTime.Now, new List<int> { 2815, 2782 });
                        db.Commit();
                    }
                    return new Result(true, "修改成功");
                }
                //return new Result(false, "修改失败");
            }
        }
        /// <summary>
        /// 新增账户明细信息
        /// </summary>
        /// <param name="detailType"></param>
        /// <param name="num"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static Result SaveAddActDetailInfo(string actId, string detailType, decimal num, DateTime? startDate, DateTime? endDate, int userId, string actLimit, string actType, string mid)
        {
            using (CRMEntities db = new CRMEntities())
            {
                using (db.BeginTransaction())
                {
                    if (!string.IsNullOrEmpty(actId))//已经入会
                    {
                        //还要更改帐户总额
                        //To Do
                        var q1 = db.TM_Mem_Account.Where(p => p.AccountID == actId).FirstOrDefault();

                        if (detailType == "value1")
                        {
                            q1.Value1 = q1.Value1 + num;

                            var qq1 = db.TM_Mem_AccountDetail.Where(p => p.AccountID == actId && p.AccountDetailType == detailType).FirstOrDefault();
                            if (qq1 != null)//修改明细
                            {
                                qq1.DetailValue = qq1.DetailValue + num;
                                var entryq1 = db.Entry(qq1);
                                entryq1.State = EntityState.Modified;
                            }
                            else//新增明细
                            {
                                TM_Mem_AccountDetail ent = new TM_Mem_AccountDetail();
                                ent.AccountID = actId;
                                ent.AccountDetailType = detailType;
                                ent.DetailValue = num;
                                ent.SpecialDate1 = startDate;
                                ent.SpecialDate2 = endDate;

                                ent.AddedDate = DateTime.Now;
                                ent.AddedUser = userId.ToString();
                                ent.ModifiedDate = DateTime.Now;
                                ent.ModifiedUser = userId.ToString();

                                db.TM_Mem_AccountDetail.Add(ent);
                            }
                        }
                        if (detailType == "value2")
                        {
                            q1.Value2 = q1.Value2 + num;

                            TM_Mem_AccountDetail ent = new TM_Mem_AccountDetail();
                            ent.AccountID = actId;
                            ent.AccountDetailType = detailType;
                            ent.DetailValue = num;
                            ent.SpecialDate1 = startDate;
                            ent.SpecialDate2 = endDate;

                            ent.AddedDate = DateTime.Now;
                            ent.AddedUser = userId.ToString();
                            ent.ModifiedDate = DateTime.Now;
                            ent.ModifiedUser = userId.ToString();

                            db.TM_Mem_AccountDetail.Add(ent);
                        }
                        var entry1 = db.Entry(q1);
                        entry1.State = EntityState.Modified;


                    }
                    else//如果为空，则看看是否入会，入会则修改，不入会则新增
                    {
                        var q1 = db.TM_Mem_Account.Where(p => p.MemberID == mid && p.AccountType == actType).FirstOrDefault();
                        if (q1 != null) //入过会，已有账户
                        {
                            actId = q1.AccountID;
                            if (detailType == "value1")
                            {
                                q1.Value1 += num;
                            }
                            if (detailType == "value2")
                            {
                                q1.Value2 += num;
                            }
                            var entry1 = db.Entry(q1);
                            entry1.State = EntityState.Modified;
                        }
                        else//没有账户
                        {
                            TM_Mem_Account act = new TM_Mem_Account();
                            act.AccountID = Guid.NewGuid().ToString("N");
                            actId = act.AccountID;
                            act.MemberID = mid;
                            act.AccountType = actType;
                            if (detailType == "value1")
                            {
                                act.Value1 = num;
                                act.Value2 = 0;
                            }
                            if (detailType == "value2")
                            {
                                act.Value1 = 0;
                                act.Value2 = num;
                            }
                            act.Value3 = 0;

                            act.AddedDate = DateTime.Now;
                            act.AddedUser = userId.ToString();
                            act.ModifiedDate = DateTime.Now;
                            act.ModifiedUser = userId.ToString();

                            db.TM_Mem_Account.Add(act);
                        }

                        //
                        TM_Mem_AccountDetail ent = new TM_Mem_AccountDetail();
                        ent.AccountID = actId;
                        ent.AccountDetailType = detailType;
                        ent.DetailValue = num;
                        ent.SpecialDate1 = startDate;
                        ent.SpecialDate2 = endDate;

                        ent.AddedDate = DateTime.Now;
                        ent.AddedUser = userId.ToString();
                        ent.ModifiedDate = DateTime.Now;
                        ent.ModifiedUser = userId.ToString();


                        db.TM_Mem_AccountDetail.Add(ent);
                    }
                    db.SaveChanges();
                    db.Commit();
                }
                //还要记录帐户更改
                //To Do
                TL_Mem_AccountChange actchange = new TL_Mem_AccountChange();
                //var q = db.TM_Mem_Account.Where(p => p.MemberID == mid && p.AccountType == actType).FirstOrDefault();
                //
                //actchange.AccountID = q.AccountID;
                var q3 = db.TM_Mem_AccountDetail.Where(p => p.AccountID == actId && p.AccountDetailType == detailType).FirstOrDefault();
                actchange.AccountID = actId;
                actchange.AccountDetailID = q3.AccountDetailID;
                actchange.AccountChangeType = "manu";
                actchange.ChangeValue = num;
                actchange.ChangeReason = "开通账户";
                actchange.HasReverse = false;
                actchange.AddedDate = DateTime.Now;
                actchange.AddedUser = userId.ToString();
                db.TL_Mem_AccountChange.Add(actchange);
                db.SaveChanges();

                //还要更改帐户使用限制
                //To Do
                //注意有可能
                //if (actLimit != "null")
                //{
                //    List<ActLimit> limit = JsonHelper.Deserialize<List<ActLimit>>(actLimit);
                //    var q4 = db.TM_Mem_AccountDetail.Where(p => p.AccountID == actId && p.AccountDetailType == detailType).OrderByDescending(p => p.AddedDate).FirstOrDefault();
                //    for (int i = 0; i < limit.Count; i++)
                //    {
                //        var liType = limit[i].LimitType;
                //        var liValue = limit[i].LimitValue;
                //        var q2 = db.TM_Mem_AccountLimit.Where(p => p.AccountDetailID == q4.AccountDetailID && p.LimitType == liType && p.LimitValue == liValue).ToList();
                //        if (q2.Count <= 0)
                //        {
                //            TM_Mem_AccountLimit model = new TM_Mem_AccountLimit();
                //            model.AccountID = actId;
                //            model.AccountDetailID = q3.AccountDetailID;
                //            model.LimitValue = liValue;
                //            model.LimitType = liType;
                //            model.AddedDate = DateTime.Now;
                //            db.TM_Mem_AccountLimit.Add(model);
                //            db.SaveChanges();
                //        }
                //    }
                //}
                //db.SaveChanges();
                //db.Commit();

                return new Result(true, "添加成功");
            }
        }



        /// <summary>
        /// 获取会员等级
        /// </summary>
        /// <param name="optionType"></param>
        /// <param name="dataGroupId"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public static Result GetBizOptionsMemberLevel(string optionType, int? dataGroupId, bool? enable)
        {
            using (var db = new CRMEntities())
            {
                var query = from a in db.TD_SYS_BizOption
                            join b in db.V_Sys_DataGroupRelation on a.DataGroupID equals b.SubDataGroupID
                            where a.OptionType.Equals(optionType)
                            && b.DataGroupID == dataGroupId
                            select new
                            {
                                a.OptionText,
                                a.OptionValue,
                                a.Enable,
                                GroupName = b.SubDataGroupName + "-" + a.OptionText
                            };
                if (enable.HasValue)
                {
                    query = query.Where(o => o.Enable == enable);
                }
                return new Result(true, "", query.ToList());
            }
        }

        /// <summary>
        /// 获取账户详细信息
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="accType"></param>
        /// <returns></returns>
        public static Result GetMemActDetails(string mid, string accType, int userId)
        {
            using (var db = new CRMEntities())
            {
                //拿到账户id
                var q1 = db.TM_Mem_Account.Where(p => p.MemberID == mid && p.AccountType == accType).FirstOrDefault();
                if (q1 != null)
                {
                    var actId = q1.AccountID;
                    //过滤门店
                    var query = (from a in db.TM_Mem_AccountDetail
                                 join e in db.TD_SYS_BizOption.Where(p => p.OptionType == "AccountDetailType" && p.Enable == true) on a.AccountDetailType equals e.OptionValue
                                 //join p in db.TL_Mem_AccountChange on a.AccountDetailID equals p.AccountDetailID
                                 where a.AccountID == actId //&& a.DetailValue > 0
                                 select new
                                 {
                                     a.AccountID,
                                     a.AccountDetailType,
                                     DetailTypeText = e.OptionText,
                                     a.AccountDetailID,
                                     a.DetailValue,
                                     a.SpecialDate1,
                                     a.SpecialDate2,
                                     //p.ChangeReason,
                                     //p.AccountChangeType,
                                 }).ToList();

                    return new Result(true, "", new List<object> { query });
                }
                return new Result(false, "没有账户");
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
        public static Result SaveAddActDetailInfo1(string actId, string detailType, decimal num, DateTime? startDate, DateTime? endDate, int userId, string actType, string mid, int groupID)
        {
            using (CRMEntities db = new CRMEntities())
            {
                //如果一个账户的日期和限制一模一样，则不允许添加此账户
                //if (startDate != null && endDate != null)//冻结情况,解冻日期和有效截至日期的都有，则看限制
                //{ }
                //if (startDate != null && endDate == null)//冻结情况
                //{ }
                //if (startDate == null && endDate == null)//可用情况，并且没有有效期
                //{ }
                //if (startDate == null && endDate != null)//可用情况，并且有有效期
                //{ }
                //List<ActLimit> limit = JsonHelper.Deserialize<List<ActLimit>>(actLimit);

                string st1 = "";

                //var qqq = from a in db.TM_Mem_AccountDetail.ToList()
                //          where a.AccountID == actId && (startDate != null ? a.SpecialDate1 == startDate : a.SpecialDate1.Equals(startDate)) && (endDate != null ? a.SpecialDate2 == endDate : a.SpecialDate2.Equals(endDate))
                //          select a;
                var qq = (from a in db.TM_Mem_AccountDetail
                          where a.AccountID == actId && a.SpecialDate1 == startDate && a.SpecialDate2 == endDate
                          select a).ToList();
                if (qq.Count > 0)//存在一样的记录，更新数据
                {
                    return new Result(false, "已经存在限制日期都相同的数据，不能新增，请前往编辑页面");
                }
                string memberID = "";
                using (db.BeginTransaction())
                {
                    if (!string.IsNullOrEmpty(actId))//已经入会
                    {
                        //还要更改帐户总额
                        //To Do
                        var q1 = db.TM_Mem_Account.Where(p => p.AccountID == actId).FirstOrDefault();
                        memberID = q1.MemberID;
                        if (detailType == "value1")
                        {
                            q1.Value1 = q1.Value1 + num;

                            TM_Mem_AccountDetail ent = new TM_Mem_AccountDetail();
                            ent.AccountDetailID = Guid.NewGuid().ToString("N");
                            ent.AccountID = actId;
                            ent.AccountDetailType = detailType;
                            ent.DetailValue = num;
                            ent.SpecialDate1 = startDate;
                            ent.SpecialDate2 = endDate;

                            ent.AddedDate = DateTime.Now;
                            ent.AddedUser = userId.ToString();
                            ent.ModifiedDate = DateTime.Now;
                            ent.ModifiedUser = userId.ToString();

                            db.TM_Mem_AccountDetail.Add(ent);
                        }
                        if (detailType == "value2")
                        {
                            q1.Value2 = q1.Value2 + num;

                            TM_Mem_AccountDetail ent = new TM_Mem_AccountDetail();
                            ent.AccountDetailID = Guid.NewGuid().ToString("N");
                            ent.AccountID = actId;
                            ent.AccountDetailType = detailType;
                            ent.DetailValue = num;
                            ent.SpecialDate1 = startDate;
                            ent.SpecialDate2 = endDate;

                            ent.AddedDate = DateTime.Now;
                            ent.AddedUser = userId.ToString();
                            ent.ModifiedDate = DateTime.Now;
                            ent.ModifiedUser = userId.ToString();

                            db.TM_Mem_AccountDetail.Add(ent);
                        }
                        var entry1 = db.Entry(q1);
                        entry1.State = EntityState.Modified;

                    }
                    else//如果为空，则看看是否入会，入会则修改，不入会则新增
                    {
                        var q1 = db.TM_Mem_Account.Where(p => p.MemberID == mid && p.AccountType == actType).FirstOrDefault();
                        if (q1 != null) //入过会，已有账户
                        {
                            actId = q1.AccountID;
                            if (detailType == "value1")
                            {
                                q1.Value1 += num;
                            }
                            if (detailType == "value2")
                            {
                                q1.Value2 += num;
                            }
                            var entry1 = db.Entry(q1);
                            entry1.State = EntityState.Modified;
                        }
                        else//没有账户
                        {
                            TM_Mem_Account act = new TM_Mem_Account();
                            act.AccountID = Guid.NewGuid().ToString("N");
                            actId = act.AccountID;
                            act.MemberID = mid;
                            act.AccountType = actType;
                            if (detailType == "value1")
                            {
                                act.Value1 = num;
                                act.Value2 = 0;
                            }
                            if (detailType == "value2")
                            {
                                act.Value1 = 0;
                                act.Value2 = num;
                            }
                            act.Value3 = 0;

                            act.AddedDate = DateTime.Now;
                            act.AddedUser = userId.ToString();
                            act.ModifiedDate = DateTime.Now;
                            act.ModifiedUser = userId.ToString();

                            db.TM_Mem_Account.Add(act);
                        }
                        //db.SaveChanges();
                        //db.Configuration.ValidateOnSaveEnabled = false;
                        //int count = db.SaveChanges();
                        //db.Configuration.ValidateOnSaveEnabled = true;
                        //actId = db.TM_Mem_Account.Where(p => p.MemberID == mid).FirstOrDefault().AccountID;

                        //
                        TM_Mem_AccountDetail ent = new TM_Mem_AccountDetail();
                        ent.AccountDetailID = Guid.NewGuid().ToString("N");
                        ent.AccountID = actId;
                        ent.AccountDetailType = detailType;
                        ent.DetailValue = num;
                        ent.SpecialDate1 = startDate;
                        ent.SpecialDate2 = endDate;

                        ent.AddedDate = DateTime.Now;
                        ent.AddedUser = userId.ToString();
                        ent.ModifiedDate = DateTime.Now;
                        ent.ModifiedUser = userId.ToString();


                        db.TM_Mem_AccountDetail.Add(ent);
                    }
                    db.SaveChanges();

                    db.Commit();

                    //还要记录帐户更改
                    //To Do
                    TL_Mem_AccountChange actchange = new TL_Mem_AccountChange();
                    //var q = db.TM_Mem_Account.Where(p => p.MemberID == mid && p.AccountType == actType).FirstOrDefault();
                    //
                    //actchange.AccountID = q.AccountID;
                    var q3 = db.TM_Mem_AccountDetail.Where(p => p.AccountID == actId && p.AccountDetailType == detailType).OrderByDescending(p => p.AddedDate).FirstOrDefault();
                    var q4 = db.TM_Mem_Account.Where(p => p.AccountID == actId).FirstOrDefault();
                    actchange.AccountID = actId;
                    actchange.AccountDetailID = q3.AccountDetailID;
                    actchange.MemberID = q4.MemberID;
                    actchange.AccountChangeType = "manu";
                    actchange.ChangeValue = num;
                    actchange.ChangeReason = "开通账户";
                    actchange.HasReverse = false;
                    actchange.AddedDate = DateTime.Now;
                    actchange.AddedUser = userId.ToString();
                    db.TL_Mem_AccountChange.Add(actchange);
                    db.SaveChanges();
                    //处理实时积分维度  2815 -累计获取积分 2782 -半年内累计获取积分
                    MemberSubdivision.ComputeDimension(new List<string> { memberID }, DateTime.Now, new List<int> { 2815, 2782 });

                    //V_M_TM_Mem_Trade_charge charge = new V_M_TM_Mem_Trade_charge()
                    //{
                    //    DataGroupID = groupID,
                    //    TradeCode = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString(),
                    //    TradeType = "charge",
                    //    MemberID = mid,
                    //    NeedLoyCompute = true,
                    //    ChargeValueTrade = num,
                    //    IsInvoiceCharge = true,
                    //    ChargeTypeTrade = actType,
                    //    RechargeTimeTrade = DateTime.Now,
                    //    StoreCodeRechargeTrade = "",
                    //    AddedDate = DateTime.Now,
                    //    AddedUser = userId.ToString(),
                    //    ModifiedDate = DateTime.Now,
                    //    ModifiedUser = userId.ToString()
                    //};
                    //dynamic t = db.AddViewRow<V_M_TM_Mem_Trade_charge, TM_Mem_Trade>(charge);
                    //db.SaveChanges();
                    //还要更改帐户使用限制
                    //To Do
                    //注意有可能
                    //if (actLimit != "null")
                    //{

                    //    var q4 = db.TM_Mem_AccountDetail.Where(p => p.AccountID == actId && p.AccountDetailType == detailType).OrderByDescending(p => p.AddedDate).FirstOrDefault();
                    //    for (int i = 0; i < limit.Count; i++)
                    //    {
                    //        var liType = limit[i].LimitType;
                    //        var liValue = limit[i].LimitValue;
                    //        var q2 = db.TM_Mem_AccountLimit.Where(p => p.AccountDetailID == q4.AccountDetailID && p.LimitType == liType && p.LimitValue == liValue).ToList();
                    //        if (q2.Count <= 0)
                    //        {
                    //            TM_Mem_AccountLimit model = new TM_Mem_AccountLimit();
                    //            model.AccountID = actId;
                    //            model.AccountDetailID = q3.AccountDetailID;
                    //            model.LimitValue = liValue;
                    //            model.LimitType = liType;
                    //            model.AddedDate = DateTime.Now;
                    //            db.TM_Mem_AccountLimit.Add(model);
                    //            db.SaveChanges();
                    //        }
                    //    }

                    //}
                    //db.SaveChanges();
                    //db.Commit();
                }


                return new Result(true, "添加成功");
            }
        }

        public static Result GetMemActDetails1(string mid, string accType)
        {
            using (var db = new CRMEntities())
            {
                //拿到账户id
                var q1 = db.TM_Mem_Account.Where(p => p.MemberID == mid && p.AccountType == accType).FirstOrDefault();
                if (q1 != null)
                {
                    var actId = q1.AccountID;
                    List<AccountLimit> limits = new List<AccountLimit>();
                    //根据账户id获取明细id列表
                    var q2 = db.TM_Mem_AccountDetail.Where(p => p.AccountID == actId && p.DetailValue > 0).ToList();
                    if (q2 != null)
                    {
                        string detailId;
                        for (int i = 0; i < q2.Count; i++)
                        {
                            detailId = q2[i].AccountDetailID;
                            //获取限制列表
                            // var q3 = db.TM_Mem_AccountLimit.Where(p => p.AccountDetailID == detailId).ToList();
                            var q3 = (from a in db.TM_Mem_AccountLimit
                                      //join b in db.TD_SYS_Store on a.LimitValue equals b.StoreCodeBase into ba
                                      //from baa in ba.DefaultIfEmpty()
                                      //join c in db.V_M_TM_SYS_BaseData_brand on a.LimitValue equals c.BrandCode into ca
                                      //from caa in ca.DefaultIfEmpty()
                                      join o in db.TD_SYS_BizOption on a.LimitType equals o.OptionValue
                                      where a.AccountDetailID == detailId && o.OptionType == "AccountLimitType"
                                      orderby a.AccountLimitID
                                      select new
                                      {
                                          a.LimitValue,
                                          a.LimitType,
                                          LimitTypeText = o.OptionText,
                                          //StoreName = baa.StoreNameBase,
                                          //caa.BrandName,
                                      }).ToList();

                            string str = "";
                            //foreach (var item in q3)
                            //{
                            //    if (item.LimitType == "store")
                            //    {
                            //        str += item.LimitTypeText + ":" + item.StoreName + "; ";
                            //    }
                            //    else if (item.LimitType == "brand")
                            //    {
                            //        str += item.LimitTypeText + ":" + item.BrandName + "; ";
                            //    }
                            //    else
                            //    {
                            //        str += item.LimitTypeText + ":" + item.LimitValue + "; ";
                            //    }
                            //}
                            limits.Add(new AccountLimit { ActDetailID = detailId, LimitText = str });
                        }

                    }

                    var query = from q in q2
                                join ltemp in limits on q.AccountDetailID equals ltemp.ActDetailID into lt
                                from l in lt.DefaultIfEmpty()
                                join o in db.TD_SYS_BizOption on q.AccountDetailType equals o.OptionValue
                                //join p in db.TL_Mem_AccountChange on q.AccountDetailID equals p.AccountDetailID
                                where o.OptionType == "AccountDetailType" && o.Enable == true
                                select new
                                {
                                    q.AccountID,
                                    q.AccountDetailType,
                                    DetailTypeText = o.OptionText,
                                    q.AccountDetailID,
                                    q.DetailValue,
                                    q.SpecialDate1,
                                    q.SpecialDate2,
                                    //p.ChangeReason,
                                    //p.AccountChangeType,
                                    AccountLimit = l == null ? "" : l.LimitText ?? ""
                                };
                    return new Result(true, "", new List<object> { query.ToList() });
                }
                return new Result(false, "没有账户");
            }
        }


        public static Result BatchImportPoint(string pointStr, int userId)
        {
            using (var db = new CRMEntities())
            {
                var pointLists = JsonHelper.Deserialize<List<string>>(pointStr);

                string cardNo = "";
                string value = "";
                string type = "";
                string time = "";



                ObjectParameter successNum = new ObjectParameter("SuccessNum", typeof(int));

                ObjectParameter failNum = new ObjectParameter("FailNum", typeof(int));




                var sql = "insert into [dbo].[TE_Mem_BatchImportPoint](id,type,value,expiredtime)values";


                foreach (var point in pointLists)
                {
                    cardNo = point.Split(',')[0];
                    type = point.Split(',')[1];
                    value = point.Split(',')[2];
                    time = point.Split(',')[3];
                    sql += "('" + cardNo + "','" + type + "','" + value + "','" + time + "'),";
                }

                sql = sql.Substring(0, sql.Length - 1);

                if (pointLists.Count() != 0)
                {
                    db.Database.ExecuteSqlCommand(sql);
                }
                //var result = db.sp_CRM_MemberRetrieveCardIsUsable(Convert.ToInt32(retrieveNum), provinceCode, Convert.ToInt64(beginCardNo), MemberCardEnd);

                var result = db.sp_Loy_AccountPointManualAdjustBatch(successNum, failNum);

                List<object> listNum = new List<object>();
                listNum.Add(successNum.Value);
                listNum.Add(failNum.Value);



                return new Result(true, "添加成功", listNum);
            }
        }


        /// <summary>
        /// 获取限制车辆列表
        /// </summary>
        /// <param name="memId"></param>
        /// <returns></returns>
        //public static Result GetActLimitVehicleList(string memId)
        //{
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        var query = (from a in db.V_M_TM_Mem_SubExt_vehicle
        //                     where a.MemberID == memId
        //                     select new
        //                     {
        //                         a.MemberSubExtID,
        //                         a.VIN,
        //                         a.CarNo
        //                     }).ToList();
        //        return new Result(true, "", query);
        //    }
        //}

        #endregion

        #region 套餐购买
        public static Result SavePackageSaleData(string package, string mid, decimal total, int userId, int groupId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                if (string.IsNullOrEmpty(mid))
                {
                    return new Result(false, "请先选择会员");
                }
                //获取门店
                List<string> brand = new List<string>();
                var store = Service.GetStoreNameByUserID(userId, ref brand);//获取门店
                string[] storeCode1 = null;
                if (!string.IsNullOrEmpty(store))
                {
                    storeCode1 = store.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
                List<PackageModel> packageList = JsonHelper.Deserialize<List<PackageModel>>(package);
                //List<ActLimit> limit = JsonHelper.Deserialize<List<ActLimit>>(limitList);

                //List<string> brand = new List<string>();

                //V_M_TM_Mem_Trade_package插入数据
                //V_M_TM_Mem_Trade_package q1 = new V_M_TM_Mem_Trade_package();
                //q1.AddedDate = DateTime.Now;
                //q1.AddedUser = userId.ToString();
                //q1.TradeCode = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
                //q1.DataGroupID = groupId;
                //q1.MemberID = mid;
                //q1.BuyPackageDateTrade = DateTime.Now;
                //q1.ModifiedDate = DateTime.Now;
                //q1.ModifiedUser = userId.ToString();
                //q1.NeedLoyCompute = true;
                //q1.NoNeedLoyComputeReaseon = null;
                //q1.PackageDecTrade = "购买套餐";
                //q1.PackageEndDateTrade = DateTime.Now;
                //q1.PackageTotalPriceTrade = total;//总价格
                //q1.StoreCodePackageTrade = storeCode1 != null ? storeCode1[0] : "";//门店
                //q1.StoreNamePackageTrade = Service.GetStoreNameByCode(q1.StoreCodePackageTrade).Obj[0].ToString();
                //q1.TradeType = "package";

                //dynamic t1 = db.AddViewRow<V_M_TM_Mem_Trade_package, TM_Mem_Trade>(q1);

                db.SaveChanges();

                for (int i = 0; i < packageList.Count; i++)
                {
                    var id = packageList[i].PackageId;
                    var p1 = db.TD_SYS_Package.Where(p => p.PackageID == id).FirstOrDefault();
                    DateTime dt = new DateTime();
                    for (int j = 0; j < packageList[i].Qty; j++)
                    {
                        //TM_MEM_Package Insert
                        TM_Mem_Package tp = new TM_Mem_Package();
                        tp.MemberID = packageList[i].MemberId;
                        tp.PackageName = p1.PackageName;
                        tp.PackageDesc = p1.PackageDesc;
                        tp.IsPresented = packageList[i].IsPresented;
                        tp.ModifiedDate = DateTime.Now;
                        tp.ModifiedUser = userId.ToString();
                        tp.AddedDate = DateTime.Now;
                        tp.AddedUser = userId.ToString();

                        var now = Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString());
                        tp.StartDate = now;
                        if (p1.AppendUnit == "day")
                            tp.EndDate = now.AddDays((double)(p1.AppendQty == null ? 0 : p1.AppendQty));
                        else if (p1.AppendUnit == "month")
                            tp.EndDate = now.AddMonths(p1.AppendQty == null ? 0 : (int)p1.AppendQty);
                        else if (p1.AppendUnit == "year")
                            tp.EndDate = now.AddYears(p1.AppendQty == null ? 0 : (int)p1.AppendQty);
                        else
                            tp.EndDate = now.AddMonths(6);
                        dt = (DateTime)tp.EndDate;
                        tp.PurchasePrice1 = p1.Price1;
                        tp.Enable = true;
                        tp.PriceRelation = p1.PriceRelation;
                        tp.PurchasePrice2 = packageList[i].Price;//购买价格
                        tp.PackageID = id;
                        dynamic tt2 = db.TM_Mem_Package.Add(tp);
                        db.SaveChanges();
                        long packageInstanceID = tt2.PackageInstanceID;

                        //TM_Mem_PackageLimit Insert
                        var q3 = db.TD_SYS_PackageLimit.Where(p => p.PackageID == id).ToList();
                        if (q3.Count > 0)
                        {
                            for (int k = 0; k < q3.Count; k++)
                            {
                                TM_Mem_PackageLimit tml = new TM_Mem_PackageLimit();
                                tml.PackageInstanceID = packageInstanceID;
                                tml.LimitType = q3[k].LimitType;
                                tml.LimitValue = q3[k].LimitValue;
                                tml.AddedDate = DateTime.Now;
                                tml.AddedUser = userId.ToString();
                                var li = db.TM_Mem_PackageLimit.Where(p => p.PackageInstanceID == tml.PackageInstanceID && p.LimitType == tml.LimitType && p.LimitValue == tml.LimitValue).ToList();
                                if (li.Count <= 0)
                                {
                                    db.TM_Mem_PackageLimit.Add(tml);
                                    db.SaveChanges();
                                }
                            }
                        }

                        var pdlst = db.TD_SYS_PackageDetail.AsNoTracking().Where(p => p.PackageID == id).ToList();
                        //TM_Mem_PackageDetail Insert
                        foreach (var item2 in pdlst)
                        {
                            TM_Mem_PackageDetail tmp = new TM_Mem_PackageDetail();
                            tmp.PackageInstanceID = packageInstanceID;
                            tmp.ItemID = item2.ItemID;
                            tmp.ItemName = item2.ItemName;
                            tmp.ItemDesc = item2.ItemDesc;
                            tmp.Qty = item2.Qty;
                            tmp.OrgQty = item2.Qty;
                            tmp.StartDate = tp.StartDate;
                            tmp.EndDate = tp.EndDate;//tp.StartDate == null ? null : AppendUnitTime(item2.AppendUnit, item2.AppendQty, tp.StartDate);
                            db.TM_Mem_PackageDetail.Add(tmp);
                            db.SaveChanges();
                        }
                    }
                    //添加之后记录log
                    //TL_Mem_ValueAddedChange ent = new TL_Mem_ValueAddedChange();
                    //ent.MemberID = mid;
                    //ent.InstanceID = id;
                    //ent.Type = "package";
                    //ent.ChangeValue = packageList[i].Qty;
                    //ent.ChangeReason = "购买套餐";
                    //ent.AddedDate = DateTime.Now;
                    //ent.AddedUser = userId.ToString();
                    //db.TL_Mem_ValueAddedChange.Add(ent);
                    //db.SaveChanges();

                    //V_M_TM_Mem_TradeDetail_package_packagedetail插入数据
                    //V_M_TM_Mem_TradeDetail_package_packagedetail q2 = new V_M_TM_Mem_TradeDetail_package_packagedetail();
                    //q2.TradeDetailType = "packagedetail";
                    //q2.TradeID = t1.TradeID;
                    //q2.TradeType = "package";
                    //q2.PackageQtyDetail = packageList[i].Qty;
                    //q2.PackagePriceDetail = packageList[i].Price;//购买价格
                    //q2.PackageNameDetail = p1.PackageName;//套餐描述
                    //q2.PackageIDDetail = p1.PackageID.ToString();//套餐ID
                    //q2.PackageEndDateDetail = dt;//套餐有效结束日期
                    //q2.PackageDecDetail = p1.PackageDesc;//套餐描述
                    //dynamic t2 = db.AddViewRow<V_M_TM_Mem_TradeDetail_package_packagedetail, TM_Mem_TradeDetail>(q2);

                    db.SaveChanges();
                }


            }
            return new Result(true, "添加成功");
        }

        /// <summary>
        /// 获取套餐列表（分页）
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="groupId"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetPackageList(string pName, int groupId, int userId, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.TD_SYS_Package
                             join b in db.TD_SYS_PackageLimit on a.PackageID equals b.PackageID into bb
                             from bba in bb.DefaultIfEmpty()
                             join c in db.V_M_TM_SYS_BaseData_brand on bba.LimitValue equals c.BrandCodeBase into cc
                             from cca in cc.DefaultIfEmpty()
                             join d in db.V_M_TM_SYS_BaseData_store on bba.LimitValue equals d.StoreCode into dd
                             from dda in dd.DefaultIfEmpty()
                             where a.Enable == true && (a.EndDate == null ? true : a.EndDate > DateTime.Now) && (a.StartDate == null ? true : a.StartDate < DateTime.Now)
                             select new
                             {
                                 a.PackageID,
                                 a.PackageName,
                                 a.PackageDesc,
                                 a.Price1,
                                 a.Price2,
                                 a.Proportion,
                                 a.PriceRelation,
                                 a.AppendQty,
                                 a.AppendUnit,
                                 a.DataGroupID,
                                 a.AddedDate,
                                 a.AddedUser,
                                 a.ModifiedUser,
                                 a.ModifiedDate,
                                 a.StartDate,
                                 a.EndDate,
                                 a.Enable,
                                 //bba.LimitType,
                                 //bba.LimitValue,
                                 //cca.BrandName,//品牌名字
                                 //dda.StoreName,//门店名字
                                 limitCode = bba == null ? "" : (bba.LimitType == "store" ? dda.StoreCode : cca.BrandCodeBase),
                                 limitName = bba == null ? "" : (bba.LimitType == "store" ? dda.StoreName : cca.BrandNameBase),
                             }).ToList();
                //门店过滤一下
                List<string> brand = new List<string>();
                var store = Service.GetStoreNameByUserID(userId, ref brand);//获取门店

                if (!string.IsNullOrEmpty(store) && brand.Count <= 0)
                {
                    var storeCode1 = store.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> l = new List<string>();
                    foreach (var item in storeCode1)
                    {
                        l.Add(item);
                    }
                    query = query.Where(p => l.Contains(p.limitCode) || p.limitCode == "").ToList();
                }
                else
                {
                    query = (from a in query
                             join b in db.V_Sys_DataGroupRelation on a.DataGroupID equals b.SubDataGroupID into bb
                             from bcg in bb.DefaultIfEmpty()
                             where bcg.DataGroupID == groupId
                             select new
                             {
                                 a.PackageID,
                                 a.PackageName,
                                 a.PackageDesc,
                                 a.Price1,
                                 a.Price2,
                                 a.Proportion,
                                 a.PriceRelation,
                                 a.AppendQty,
                                 a.AppendUnit,
                                 a.DataGroupID,
                                 a.AddedDate,
                                 a.AddedUser,
                                 a.ModifiedUser,
                                 a.ModifiedDate,
                                 a.StartDate,
                                 a.EndDate,
                                 a.Enable,
                                 a.limitCode,
                                 a.limitName
                                 //a.LimitType,
                                 //a.LimitValue,
                                 //a.BrandName,
                                 //a.StoreName,
                             }).ToList();
                }
                var query1 = query.GroupBy(t => new { t.PackageID, t.PackageName, t.Price1, t.StartDate, t.EndDate })
            .Select(g => new { PackageID = g.Key.PackageID, PackageName = g.Key.PackageName, StartDate = g.Key.StartDate, EndDate = g.Key.EndDate, NUM = g.Count(), Price1 = g.Key.Price1, limitName = string.Join(",", g.Select(s => s.limitName).ToArray()) }).ToList();
                if (!string.IsNullOrEmpty(pName)) query1 = query1.Where(p => p.PackageName.Contains(pName)).ToList();

                return new Result(true, "", new List<object> { query1.ToDataTableSourceVsPage(myDp) });
            }
        }

        //AppendUnit 根据周期类型计算截至日期
        public static DateTime? AppendUnitTime(string appendUnit, int? appendQty, DateTime? datetime)
        {
            switch (appendUnit)
            {
                case "day":
                    datetime = Convert.ToDateTime(datetime).AddDays(Convert.ToDouble(appendQty));
                    break;
                case "month":
                    datetime = Convert.ToDateTime(datetime).AddMonths(Convert.ToInt16(appendQty));
                    break;
                case "year":
                    datetime = Convert.ToDateTime(datetime).AddYears(Convert.ToInt16(appendQty));
                    break;
                default:
                    break;
            }
            return datetime;
        }
        #endregion

        #region 套餐调整
        /// <summary>
        /// 获取会员套餐列表
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetMemPackageList(string mid, int userId, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from u in db.TM_Mem_Package
                             join e in db.TM_AUTH_User on u.AddedUser equals SqlFunctions.StringConvert((double)e.UserID).Trim() into eb
                             from eba in eb.DefaultIfEmpty()
                             join b in db.TD_SYS_PackageLimit on u.PackageID equals b.PackageID into bb
                             from bba in bb.DefaultIfEmpty()
                             join c in db.V_M_TM_SYS_BaseData_brand on bba.LimitValue equals c.BrandCodeBase into cc
                             from cca in cc.DefaultIfEmpty()
                             join d in db.V_M_TM_SYS_BaseData_store on bba.LimitValue equals d.StoreCode into dd
                             from dda in dd.DefaultIfEmpty()
                             where u.MemberID == mid
                             select new
                             {
                                 u.PackageInstanceID,
                                 u.PackageName,
                                 u.PackageDesc,
                                 u.PurchasePrice2,//购买价格
                                 u.AddedDate,
                                 u.StartDate,
                                 u.EndDate,
                                 u.IsPresented,
                                 eba.UserName,//操作人
                                 limitCode = bba == null ? "" : (bba.LimitType == "store" ? dda.StoreCode : cca.BrandCodeBase),
                                 limitName = bba == null ? "" : (bba.LimitType == "store" ? dda.StoreName : cca.BrandNameBase),
                             }).Distinct().ToList();
                //过滤一下门店
                //门店过滤一下
                List<string> brand = new List<string>();
                var store = Service.GetStoreNameByUserID(userId, ref brand);//获取门店

                if (!string.IsNullOrEmpty(store) && brand.Count <= 0)
                {
                    var storeCode1 = store.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> l = new List<string>();
                    foreach (var item in storeCode1)
                    {
                        l.Add(item);
                    }
                    query = query.Where(p => l.Contains(p.limitCode) || p.limitCode == "").ToList();
                }
                var query1 = query.GroupBy(t => new { t.PackageInstanceID, t.PackageName, t.PackageDesc, t.PurchasePrice2, t.AddedDate, t.IsPresented, t.UserName, t.StartDate, t.EndDate })
            .Select(g => new { PackageInstanceID = g.Key.PackageInstanceID, PackageName = g.Key.PackageName, PackageDesc = g.Key.PackageDesc, PurchasePrice2 = g.Key.PurchasePrice2, AddedDate = g.Key.AddedDate, IsPresented = g.Key.IsPresented, UserName = g.Key.UserName, StartDate = g.Key.StartDate, EndDate = g.Key.EndDate, NUM = g.Count(), limitName = string.Join(",", g.Select(s => s.limitName).ToArray()) }).ToList();

                return new Result(true, "", new List<object> { query1.ToDataTableSourceVsPage(myDp) });
            }
        }

        /// <summary>
        /// 删除会员套餐
        /// </summary>
        /// <param name="packageID"></param>
        /// <returns></returns>
        public static Result DeleteMemPackage(int packageID, string mid, int userId, int? groupId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                //获取门店
                List<string> brand = new List<string>();
                var store = Service.GetStoreNameByUserID(userId, ref brand);//获取门店
                string[] storeCode1 = null;
                if (!string.IsNullOrEmpty(store))
                {
                    storeCode1 = store.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }

                var query = db.TM_Mem_Package.Where(p => p.PackageInstanceID == packageID).FirstOrDefault();
                if (query != null)
                {
                    db.TM_Mem_Package.Remove(query);
                    db.SaveChanges();
                    //删除之后添加log
                    //TL_Mem_ValueAddedChange ent = new TL_Mem_ValueAddedChange();
                    //ent.MemberID = mid;
                    //ent.InstanceID = packageID;
                    //ent.Type = "package";
                    //ent.ChangeValue = -1;
                    //ent.ChangeReason = "套餐调整";
                    //ent.AddedDate = DateTime.Now;
                    //ent.AddedUser = userId.ToString();
                    //db.TL_Mem_ValueAddedChange.Add(ent);
                    //V_M_TM_Mem_Trade_package插入数据
                    //V_M_TM_Mem_Trade_package q1 = new V_M_TM_Mem_Trade_package();
                    //q1.AddedDate = DateTime.Now;
                    //q1.AddedUser = userId.ToString();
                    //q1.DataGroupID = (int)groupId;
                    //q1.MemberID = mid;
                    //q1.ModifiedDate = DateTime.Now;
                    //q1.ModifiedUser = userId.ToString();
                    //q1.NeedLoyCompute = true;
                    //q1.NoNeedLoyComputeReaseon = null;
                    //q1.PackageDecTrade = "套餐调整";
                    //q1.PackageEndDateTrade = DateTime.Now;
                    //q1.StoreCodePackageTrade = storeCode1 != null ? storeCode1[0] : "";//门店
                    //q1.StoreNamePackageTrade = Service.GetStoreNameByCode(q1.StoreCodePackageTrade).Obj[0].ToString();
                    //q1.TradeType = "package";

                    //dynamic t1 = db.AddViewRow<V_M_TM_Mem_Trade_package, TM_Mem_Trade>(q1);

                    db.SaveChanges();
                    //V_M_TM_Mem_TradeDetail_package_packagedetail插入数据
                    //V_M_TM_Mem_TradeDetail_package_packagedetail q2 = new V_M_TM_Mem_TradeDetail_package_packagedetail();
                    //q2.TradeDetailType = "packagedetail";
                    //q2.TradeID = t1.TradeID;
                    //q2.TradeType = "package";
                    //q2.PackageQtyDetail = 1;
                    //q2.PackagePriceDetail = query.PurchasePrice2;//购买价格
                    //q2.PackageNameDetail = query.PackageName;//套餐描述
                    //q2.PackageIDDetail = query.PackageID.ToString();//套餐ID
                    //q2.PackageEndDateDetail = query.EndDate;//套餐有效结束日期
                    //q2.PackageDecDetail = query.PackageDesc;//套餐描述
                    //dynamic t2 = db.AddViewRow<V_M_TM_Mem_TradeDetail_package_packagedetail, TM_Mem_TradeDetail>(q2);
                    db.SaveChanges();
                    return new Result(true, "删除成功");
                }
                return new Result(false, "删除失败");
            }
        }
        #endregion

        #region 优惠券添加

        /// <summary>
        /// 获取优惠券添加列表
        /// </summary>
        /// <returns></returns>
        public static Result GetCouponAddData(string dp, string couponName, int userId)
        {
            try
            {
                DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
                using (CRMEntities db = new CRMEntities())
                {

                    var query = (from a in db.TM_Act_CommunicationTemplet.AsNoTracking()
                                 join b in db.TD_SYS_BizOption on a.SubType equals b.OptionValue
                                 where a.Type == "Coupon"
                                     //&& a.DataGroupID == basedata.DataGroupID
                                 && b.OptionType == "CouponType"
                                 && (string.IsNullOrEmpty(couponName) ? true : a.Name.Contains(couponName))
                                 select new CouponModel
                                 {
                                     TempletID = a.TempletID,
                                     CouponName = a.Name,
                                     CouponType = b.OptionText,
                                     BasicContent = a.BasicContent,
                                     IsPubilc = true
                                 }).ToList();


                    foreach (var item in query)
                    {
                        var cm = JsonHelper.Deserialize<JsonCouponModel>(item.BasicContent);
                        if (cm.ispublic == true)//cm.isanonymous == true || 
                        {
                            item.IsPubilc = false;
                        }
                        var now = Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString());
                        item.StartDate = now;
                        if (cm.unit == "day")
                            item.EndDate = now.AddDays((double)(cm.offNumber == null ? 0 : cm.offNumber));
                        else if (cm.unit == "month")
                            item.EndDate = now.AddMonths(cm.offNumber == null ? 0 : (int)cm.offNumber);
                        else if (cm.unit == "year")
                            item.EndDate = now.AddYears(cm.offNumber == null ? 0 : (int)cm.offNumber);
                        else
                            item.EndDate = now.AddMonths(6);

                    }
                    var aa = (from a in query
                              //join b in db.TD_SYS_CouponLimit on a.TempletID equals b.TempletID into bb
                              //from bba in bb.DefaultIfEmpty()
                              //join c in db.V_M_TM_SYS_BaseData_brand on bba == null ? "" : bba.LimitValue equals c.BrandCode into cc
                              //from cca in cc.DefaultIfEmpty()
                              //join d in db.V_M_TM_SYS_BaseData_store on bba == null ? "" : bba.LimitValue equals d.StoreCode into dd
                              //from dda in dd.DefaultIfEmpty()
                              where a.IsPubilc == true
                              select new
                              {
                                  TempletID = a.TempletID,
                                  a.CouponType,
                                  CouponName = a.CouponName,
                                  StartDate = a.StartDate,
                                  EndDate = a.EndDate,
                                  //limitCode = bba == null ? "" : (bba.LimitType == "store" ? dda.StoreCode : cca.BrandCode),
                                  //limitName = bba == null ? "" : (bba.LimitType == "store" ? dda.StoreName : cca.BrandName),
                              }).Distinct().ToList();

                    //门店过滤一下
                    List<string> brand = new List<string>();
                    var store = Service.GetStoreNameByUserID(userId, ref brand);//获取门店

                    if (!string.IsNullOrEmpty(store) && brand.Count <= 0)
                    {
                        var storeCode1 = store.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        List<string> l = new List<string>();
                        foreach (var item in storeCode1)
                        {
                            l.Add(item);
                        }
                        //aa = aa.Where(p => l.Contains(p.limitCode) || p.limitCode == "").ToList();
                    }
                    var query1 = aa.GroupBy(t => new { t.TempletID, t.CouponName, t.StartDate, t.EndDate, t.CouponType })
                .Select(g => new { TempletID = g.Key.TempletID, CouponName = g.Key.CouponName, CouponType = g.Key.CouponType, NUM = g.Count(), StartDate = g.Key.StartDate, EndDate = g.Key.EndDate }).ToList();

                    //    var query1 = aa.GroupBy(t => new { t.TempletID, t.CouponName, t.StartDate, t.EndDate, t.CouponType })
                    //.Select(g => new { TempletID = g.Key.TempletID, CouponName = g.Key.CouponName, CouponType = g.Key.CouponType, NUM = g.Count(), StartDate = g.Key.StartDate, EndDate = g.Key.EndDate, limitName = string.Join(",", g.Select(s => s.limitName).ToArray()) }).ToList();


                    var res = new DatatablesSourceVsPage();
                    res.iDisplayStart = myDp.iDisplayStart;
                    res.iDisplayLength = myDp.iDisplayLength;
                    res.iTotalRecords = query1.Count;
                    res.aaData = query1;
                    return new Result(true, "", new List<object> { query1.ToDataTableSourceVsPage(myDp) });
                }
            }
            catch (Exception ex)
            {

                Log4netHelper.WriteErrorLog("GetCouponAddData" + ex.ToString());
                return new Result(false, ex.Message);
            }


        }

        public static Result AddCouponByCode(int? dataGroupID, string mid, int templetID, string couponCode, DateTime time, string userId, string reason)
        {
            using (CRMEntities db = new CRMEntities())
            {
                db.BeginTransaction();
                var query = (from a in db.TM_Act_CommunicationTemplet.AsNoTracking()
                             join b in db.TD_SYS_BizOption on a.SubType equals b.OptionValue
                             where a.Type == "Coupon"
                             && a.TempletID == templetID
                             && b.OptionType == "CouponType"
                             select new
                             {
                                 CouponType = a.SubType,
                                 CouponName = a.Name,
                                 BasicContent = a.BasicContent,
                                 OptionText = b.OptionText,
                                 Startdate = "",
                                 Enddate = ""
                             }).FirstOrDefault();
                if (query == null) return new Result(false, "优惠券模板编号不正确");
                JsonCouponModel jm = new JsonCouponModel();
                jm = JsonHelper.Deserialize<JsonCouponModel>(query.BasicContent);

                TM_Mem_CouponPool tmp = new TM_Mem_CouponPool();
                tmp.AddedDate = time;
                tmp.MemberID = mid;
                tmp.TempletID = templetID;
                tmp.CouponCode = couponCode;
                tmp.CouponType = query.CouponType;
                var now = Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString());

                tmp.StartDate = now;
                if (jm.unit == "day")
                    tmp.EndDate = now.AddDays((double)(jm.offNumber == null ? 0 : jm.offNumber));
                else if (jm.unit == "month")
                    tmp.EndDate = now.AddMonths(jm.offNumber == null ? 0 : (int)jm.offNumber);
                else if (jm.unit == "year")
                    tmp.EndDate = now.AddYears(jm.offNumber == null ? 0 : (int)jm.offNumber);
                else
                    tmp.EndDate = now.AddMonths(6);
                tmp.AddedDate = DateTime.Now;
                tmp.Enable = true;
                tmp.IsUsed = false;
                var cp = db.TM_Mem_CouponPool.Add(tmp);
                db.SaveChanges();
                //增加限制
                //var limits = db.TD_SYS_CouponLimit.Where(p => p.TempletID == templetID).ToList();
                //if (limits.Count > 0)
                //{
                //    for (int i = 0; i < limits.Count; i++)
                //    {
                //        TM_Mem_CouponLimit lim = new TM_Mem_CouponLimit();
                //        lim.CouponID = cp.CouponID;
                //        lim.LimitType = limits[i].LimitType;
                //        lim.LimitValue = limits[i].LimitValue;
                //        lim.AddedDate = DateTime.Now;
                //        var li = db.TM_Mem_CouponLimit.Where(p => p.CouponID == lim.CouponID && p.LimitType == lim.LimitType && p.LimitValue == lim.LimitValue).ToList();
                //        if (li.Count <= 0)
                //            db.TM_Mem_CouponLimit.Add(lim);
                //    }
                //}
                //新增日志记录表
                //TL_Mem_ValueAddedChange ent = new TL_Mem_ValueAddedChange();
                //ent.MemberID = mid;
                //ent.InstanceID = Convert.ToInt64(dataGroupID);
                //ent.Type = "Coupon";
                //ent.ChangeValue = 1;
                //ent.ChangeReason = reason;
                //ent.AddedDate = DateTime.Now;
                //ent.AddedUser = userId;
                //db.TL_Mem_ValueAddedChange.Add(ent);

                db.SaveChanges();
                db.Commit();
            }

            return new Result(true, "添加成功");
        }
        #endregion

        #region 会员权益减扣
        /// <summary>
        /// 验证委托书号
        /// </summary>
        /// <param name="tradeCode"></param>
        /// <returns></returns>
        public static Result CheckTradeCode(string mId, string tradeCode)
        {
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var q = db.V_M_TM_Mem_Trade_service.Where(p => p.MemberID == mId && p.TradeCodeService == tradeCode).ToList();
            //    if (q.Count > 0)
            //    {
            //        return new Result(true, "验证成功");
            //    }
            //    else
            //    {
            //        return new Result(false, "验证失败");
            //    }
            //}

            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取账户信息
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetAccountInfo(string mid, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.TM_Mem_Account.AsNoTracking()
                            join b in db.TD_SYS_BizOption.AsNoTracking() on a.AccountType equals b.OptionValue
                            where b.OptionType == "AccountType" && a.MemberID == mid
                            select new
                            {
                                b.OptionText,
                                a.Value1
                            };
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }
        /// <summary>
        /// 获取优惠券信息
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetCouponInfo(string mid, string storeCode, string dp)
        {
            throw new NotImplementedException();
            //DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            //var basedata = getDataGroupId(storeCode);
            //var nowdate = DateTime.Now;
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var query = from a in db.TM_Mem_CouponPool.AsNoTracking()
            //                join b in db.TM_Mem_CouponLimit.AsNoTracking() on a.CouponID equals b.CouponID into bt
            //                from c in bt.DefaultIfEmpty()
            //                join d in db.TM_Act_CommunicationTemplet.AsNoTracking() on a.TempletID equals d.TempletID
            //                where a.StartDate <= nowdate && a.EndDate >= nowdate
            //                      && a.MemberID == mid && (string.IsNullOrEmpty(c.LimitType) || (c.LimitType == "store" && c.LimitValue == storeCode) || (c.LimitType == "brand" && c.LimitValue == basedata.StoreBrandCode) || (c.LimitType == "vehicle" && c.LimitValue == ""))
            //                select new
            //                {
            //                    a.CouponID,
            //                    d.Name,
            //                    Qty = "1",
            //                    a.EndDate,

            //                };
            //    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            //}
        }
        /// <summary>
        /// 根据门店编号获取会员群组信息
        /// </summary>
        /// <param name="storeCode"></param>
        /// <returns></returns>
        public static V_M_TM_SYS_BaseData_store getDataGroupId(string storeCode)
        {
            throw new NotImplementedException();
            //using (CRMEntities db = new CRMEntities())
            //{
            //    V_M_TM_SYS_BaseData_store vs = new V_M_TM_SYS_BaseData_store();
            //    var query = db.V_M_TM_SYS_BaseData_store.AsNoTracking().Where(p => p.StoreCode == storeCode || p.StoreCodeSale == storeCode).FirstOrDefault();
            //    if (query != null)
            //    {
            //        vs = query;
            //        return vs;
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}
        }
        /// <summary>
        /// 获取套餐信息
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetPackageInfo(string mid, string storeCode, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            var nowdate = DateTime.Now;
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.TM_Mem_Package.AsNoTracking()
                            join b in db.TM_Mem_PackageDetail.AsNoTracking() on a.PackageInstanceID equals b.PackageInstanceID
                            join c in db.TM_Mem_PackageLimit.AsNoTracking() on a.PackageInstanceID equals c.PackageInstanceID into ct
                            from d in ct.DefaultIfEmpty()
                            where b.StartDate <= nowdate && b.EndDate >= nowdate && a.MemberID == mid
                            && (string.IsNullOrEmpty(d.LimitType) ? true : (d.LimitType == "store" ? d.LimitValue == storeCode : d.LimitValue == mid))
                            && b.Qty > 0
                            select new
                            {
                                a.PackageInstanceID,
                                b.PackageInstanceDetailID,
                                a.PackageName,
                                b.ItemName,
                                b.ItemDesc,
                                b.Qty,
                                b.EndDate
                            };
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }

        /// <summary>
        /// 获取公共优惠券
        /// </summary>
        /// <param name="couponCode"></param>
        /// <returns></returns>
        public static Result GetPublicCoupon(string couponCode)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_Mem_CouponPool.Where(p => p.CouponCode == couponCode && p.Enable == true && p.IsUsed == false && p.MemberID.Equals(null)).FirstOrDefault();
                if (query == null)
                {
                    return new Result(false, "不存在该券号");
                }
                else
                {
                    var query2 = db.TM_Act_CommunicationTemplet.Where(p => p.TempletID == query.TempletID).FirstOrDefault();
                    if (query2 != null)
                    {
                        return new Result(true, "", query2);

                    }
                    return new Result(false, "模板错误");
                }
            }
        }

        /// <summary>
        /// 保存会员权益减扣数据
        /// </summary>
        /// <param name="package"></param>
        /// <param name="account"></param>
        /// <param name="coupon"></param>
        /// <param name="mid"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Result SaveMemInterestDeduction(string package, string account, string coupon, string mid, int userId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                List<PackageModel> packageList = JsonHelper.Deserialize<List<PackageModel>>(package);
                List<AccountModel> accountList = JsonHelper.Deserialize<List<AccountModel>>(account);
                List<CouModel> couponList = JsonHelper.Deserialize<List<CouModel>>(coupon);
                using (db.BeginTransaction())
                {
                    //账户：账户，账户明细，账户变更
                    if (account != "null")
                    {
                        for (int i = 0; i < accountList.Count; i++)
                        {
                            //账户
                            var actType = accountList[i].AccountType;
                            var q1 = db.TM_Mem_Account.Where(p => p.MemberID == mid && p.AccountType == actType).FirstOrDefault();
                            if (q1 != null)
                            {
                                q1.Value1 = q1.Value1 - accountList[i].ChangeValue;
                                var entry1 = db.Entry(q1);
                                entry1.State = EntityState.Modified;
                                //db.SaveChanges();
                                //账户明细
                                var q2 = db.TM_Mem_AccountDetail.Where(p => p.AccountID == q1.AccountID && p.AccountDetailType == "value1").FirstOrDefault();
                                var actID = q2.AccountID;
                                var actDetailID = q2.AccountDetailID;
                                q2.DetailValue = q2.DetailValue - accountList[i].ChangeValue;
                                q2.ModifiedDate = DateTime.Now;
                                q2.ModifiedUser = userId.ToString();
                                var entry = db.Entry(q2);
                                entry.State = EntityState.Modified;
                                //db.SaveChanges();
                                //还要记录帐户更改
                                //To Do
                                TL_Mem_AccountChange ent = new TL_Mem_AccountChange();
                                ent.AccountID = q1.AccountID;
                                ent.AccountDetailID = actDetailID;
                                ent.AccountChangeType = "manu";
                                ent.ChangeValue = -accountList[i].ChangeValue;
                                ent.ChangeReason = "会员权益扣减";
                                ent.HasReverse = false;
                                ent.AddedDate = DateTime.Now;
                                ent.AddedUser = userId.ToString();
                                db.TL_Mem_AccountChange.Add(ent);
                                //db.SaveChanges();
                            }
                        }
                    }

                    //套餐：套餐明细，套餐变更
                    if (package != "null")
                    {
                        for (int i = 0; i < packageList.Count; i++)
                        {
                            var pacDetailId = packageList[i].PackageDetailId;
                            var query = db.TM_Mem_PackageDetail.Where(p => p.PackageInstanceID == pacDetailId).FirstOrDefault();
                            if (query != null)
                            {
                                query.Qty = query.Qty - packageList[i].Qty;
                                var entry = db.Entry(query);
                                entry.State = EntityState.Modified;
                                //TL_Mem_ValueAddedChapackageListnge ent = new TL_Mem_ValueAddedChange();
                                //ent.MemberID = mid;
                                //ent.InstanceID = packageID;
                                //ent.Type = "package";
                                //ent.ChangeValue = -1;
                                //ent.ChangeReason = "";
                                //ent.AddedDate = DateTime.Now;
                                //ent.AddedUser = userId.ToString();
                                //db.TL_Mem_ValueAddedChange.Add(ent);
                                //db.SaveChanges();
                            }
                        }
                    }

                    //优惠券：优惠券
                    if (coupon != "null")
                    {
                        for (int i = 0; i < couponList.Count; i++)
                        {
                            var couponId = couponList[i].CouponId;
                            var cquery = db.TM_Mem_CouponPool.Where(p => p.CouponID == couponId).FirstOrDefault();
                            if (cquery != null)
                            {
                                cquery.IsUsed = true;

                                var entry = db.Entry(cquery);
                                entry.State = EntityState.Modified;
                                //db.SaveChanges();
                            }
                        }
                    }
                    db.SaveChanges();
                    db.Commit();
                }
            }

            return new Result(true, "扣减成功");
        }

        public static Result GetActDetail(string actid)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_Mem_AccountDetail.Where(p => p.AccountID == actid);
                return new Result(true, "", new List<object> { query.ToDataTableSource() });
            }
        }

        #endregion

        #region 账户储值
        public static Result SaveStoreValue(string mid, decimal changeValue, bool isBack, int userId, string storeCode, int dataGroupId, DateTime? startDate)
        {
            using (CRMEntities db = new CRMEntities())
            {

                var q = db.TM_Mem_Account.Where(p => p.MemberID == mid && p.AccountType == "1").FirstOrDefault();
                string actId;
                if (q == null) //如果没有账户，则新建
                {
                    TM_Mem_Account act = new TM_Mem_Account();
                    act.AccountID = Guid.NewGuid().ToString("N");
                    actId = act.AccountID;
                    act.MemberID = mid;
                    act.AccountType = "1";
                    act.Value1 = 0;
                    act.Value2 = 0;
                    act.Value3 = 0;
                    act.NoBackAccount = 0;
                    act.AddedDate = DateTime.Now;
                    act.AddedUser = userId.ToString();
                    act.ModifiedDate = DateTime.Now;
                    act.ModifiedUser = userId.ToString();

                    db.TM_Mem_Account.Add(act);
                    db.SaveChanges();
                }
                else { actId = q.AccountID; }

                var q1 = db.TM_Mem_Account.Where(p => p.MemberID == mid && p.AccountType == "1").FirstOrDefault();
                //同时明细表增加记录--增加一条冻结记录
                //var q2 = db.TM_Mem_AccountDetail.Where(p => p.AccountID == actId && p.AccountDetailType == "value1").OrderByDescending(p => p.AddedDate).FirstOrDefault();

                //如果可退，可并入value1，否则存NoBackAccount
                if (isBack)
                {
                    q1.Value2 = q1.Value2 + changeValue;
                    //if (q2 != null)
                    //{
                    //    q2.DetailValue = q2.DetailValue + changeValue;
                    //}
                    //else
                    //{
                    TM_Mem_AccountDetail ent = new TM_Mem_AccountDetail();
                    ent.AccountID = q1.AccountID;
                    ent.AccountDetailType = "value2";
                    ent.DetailValue = changeValue;
                    ent.SpecialDate1 = startDate;

                    ent.AddedDate = DateTime.Now;
                    ent.AddedUser = userId.ToString();
                    ent.ModifiedDate = DateTime.Now;
                    ent.ModifiedUser = userId.ToString();
                    db.TM_Mem_AccountDetail.Add(ent);
                    db.SaveChanges();

                    var q2 = db.TM_Mem_AccountDetail.Where(p => p.AccountID == actId && p.AccountDetailType == "value2").OrderByDescending(p => p.AddedDate).FirstOrDefault();
                    //}
                    //log表中增加记录
                    TL_Mem_AccountChange actchange = new TL_Mem_AccountChange();
                    actchange.AccountID = q1.AccountID;
                    actchange.AccountDetailID = q2.AccountDetailID;
                    actchange.AccountChangeType = "manu";
                    actchange.ChangeValue = changeValue;
                    actchange.ChangeReason = "储值页面";
                    actchange.HasReverse = false;
                    actchange.AddedDate = DateTime.Now;
                    actchange.AddedUser = userId.ToString();
                    db.TL_Mem_AccountChange.Add(actchange);

                    //V_M_TM_Mem_Trade_charge
                    //V_M_TM_Mem_Trade_charge charge = new V_M_TM_Mem_Trade_charge()
                    //{
                    //    DataGroupID = dataGroupId,
                    //    TradeCode = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString(),
                    //    TradeType = "charge",
                    //    MemberID = mid,
                    //    NeedLoyCompute = true,
                    //    ChargeValueTrade = changeValue,
                    //    IsInvoiceCharge = true,
                    //    ChargeTypeTrade = "1",
                    //    RechargeTimeTrade = DateTime.Now,
                    //    StoreCodeRechargeTrade = storeCode,
                    //    StoreNameRechargeTrade = Service.GetStoreNameByCode(storeCode).Obj[0].ToString(),
                    //    AddedDate = DateTime.Now,
                    //    AddedUser = userId.ToString(),
                    //    ModifiedDate = DateTime.Now,
                    //    ModifiedUser = userId.ToString()
                    //};
                    //dynamic t = db.AddViewRow<V_M_TM_Mem_Trade_charge, TM_Mem_Trade>(charge);
                    db.SaveChanges();

                    //增加限制条件
                    TM_Mem_AccountLimit model = new TM_Mem_AccountLimit();
                    model.AccountID = actId;
                    model.AccountDetailID = q2.AccountDetailID;
                    model.LimitValue = storeCode;
                    model.LimitType = "store";
                    model.AddedDate = DateTime.Now;
                    db.TM_Mem_AccountLimit.Add(model);
                    db.SaveChanges();
                }
                else
                {
                    q1.Value1 = q1.Value1 + changeValue;
                    q1.NoBackAccount = (q1.NoBackAccount != null ? q1.NoBackAccount : 0) + changeValue;
                    //添加一个不可退明细
                    TM_Mem_AccountDetail ent = new TM_Mem_AccountDetail();
                    ent.AccountID = q1.AccountID;
                    ent.AccountDetailType = "value1";
                    ent.DetailValue = changeValue;
                    ent.SpecialDate2 = startDate;
                    ent.IsNoBack = true;
                    ent.AddedDate = DateTime.Now;
                    ent.AddedUser = userId.ToString();
                    ent.ModifiedDate = DateTime.Now;
                    ent.ModifiedUser = userId.ToString();
                    db.TM_Mem_AccountDetail.Add(ent);
                    db.SaveChanges();

                    var q3 = db.TM_Mem_AccountDetail.Where(p => p.AccountID == actId && p.AccountDetailType == "noBackAccount").OrderByDescending(p => p.AddedDate).FirstOrDefault();
                    //log表中增加记录（不可退）
                    TL_Mem_AccountChange actchange = new TL_Mem_AccountChange();

                    actchange.AccountID = q1.AccountID;
                    actchange.AccountDetailID = q3.AccountDetailID;
                    actchange.AccountChangeType = "manu";
                    actchange.ChangeValue = changeValue;
                    actchange.ChangeReason = "储值页面";
                    actchange.HasReverse = false;
                    actchange.AddedDate = DateTime.Now;
                    actchange.AddedUser = userId.ToString();
                    db.TL_Mem_AccountChange.Add(actchange);

                    //V_M_TM_Mem_Trade_charge
                    //V_M_TM_Mem_Trade_charge charge = new V_M_TM_Mem_Trade_charge()
                    //{
                    //    DataGroupID = dataGroupId,
                    //    TradeCode = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString(),
                    //    TradeType = "charge",
                    //    MemberID = mid,
                    //    NeedLoyCompute = true,
                    //    ChargeValueTrade = changeValue,
                    //    ChargeTypeTrade = "1",
                    //    IsInvoiceCharge = false,
                    //    RechargeTimeTrade = DateTime.Now,
                    //    StoreCodeRechargeTrade = storeCode,
                    //    StoreNameRechargeTrade = Service.GetStoreNameByCode(storeCode).Obj[0].ToString(),
                    //    AddedDate = DateTime.Now,
                    //    AddedUser = userId.ToString(),
                    //    ModifiedDate = DateTime.Now,
                    //    ModifiedUser = userId.ToString()
                    //};
                    //dynamic t = db.AddViewRow<V_M_TM_Mem_Trade_charge, TM_Mem_Trade>(charge);
                    db.SaveChanges();

                    //增加限制条件
                    TM_Mem_AccountLimit model = new TM_Mem_AccountLimit();
                    model.AccountID = actId;
                    model.AccountDetailID = q3.AccountDetailID;
                    model.LimitValue = storeCode;
                    model.LimitType = "store";
                    model.AddedDate = DateTime.Now;
                    db.TM_Mem_AccountLimit.Add(model);
                    db.SaveChanges();
                }
                var entry1 = db.Entry(q1);
                entry1.State = EntityState.Modified;
                //var entry2 = db.Entry(q2);
                //entry2.State = EntityState.Modified;

                db.SaveChanges();
            }

            return new Result(true, "储值成功");
        }
        public static Result SaveStoreValue1(string mid, decimal changeValue, int userId, string storeCode, int dataGroupId, DateTime? startDate)
        {
            using (CRMEntities db = new CRMEntities())
            {
                //获取门店
                //List<string> brand = new List<string>();
                //var store = Service.GetStoreNameByUserID(userId, ref brand);//获取门店
                //string[] storeCode1 = null;
                //if (!string.IsNullOrEmpty(store) && brand.Count <= 0)
                //{
                //    storeCode1 = store.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //}

                var q = db.TM_Mem_Account.Where(p => p.MemberID == mid && p.AccountType == "1").FirstOrDefault();
                string actId;
                string accountDetailID;
                decimal beforeValue;//充值前金额
                bool isLimit = false;
                if (q == null) //如果没有账户，则新建
                {
                    TM_Mem_Account act = new TM_Mem_Account();
                    act.AccountID = Guid.NewGuid().ToString("N");
                    actId = act.AccountID;
                    act.MemberID = mid;
                    act.AccountType = "1";
                    act.Value1 = 0;
                    act.Value2 = 0;
                    act.Value3 = 0;
                    act.NoBackAccount = 0;
                    act.AddedDate = DateTime.Now;
                    act.AddedUser = userId.ToString();
                    act.ModifiedDate = DateTime.Now;
                    act.ModifiedUser = userId.ToString();

                    db.TM_Mem_Account.Add(act);
                    db.SaveChanges();
                }
                else { actId = q.AccountID; }

                //Todo:如果当天存储两笔，并且解冻日期一样，限制一样，待处理？
                var qq = from a in db.TM_Mem_AccountDetail
                         join b in db.TM_Mem_AccountLimit on a.AccountDetailID equals b.AccountDetailID into ba
                         from bba in ba.DefaultIfEmpty()
                         where a.AccountID == actId
                         select new
                         {
                             a.AccountDetailType,
                             a.AccountDetailID,
                             a.SpecialDate1,
                             a.SpecialDate2,
                             IsNoBack = a.IsNoBack == null ? false : a.IsNoBack,
                             LimitType = bba == null ? null : bba.LimitType,
                             LimitValue = bba == null ? null : bba.LimitValue,
                         };
                var qq1 = qq.ToList().GroupBy(t => new { t.AccountDetailID, t.AccountDetailType, t.SpecialDate1, t.SpecialDate2, t.IsNoBack }).Select(g => new { AccountDetailID = g.Key.AccountDetailID, AccountDetailType = g.Key.AccountDetailType, SpecialDate1 = g.Key.SpecialDate1, SpecialDate2 = g.Key.SpecialDate2, IsNoBack = g.Key.IsNoBack, limitName = string.Join(",", g.Select(s => s.LimitValue).Where(s => s != null).ToArray()) }).Where(p => (p.SpecialDate1 == startDate || p.SpecialDate2 == startDate) && p.limitName == storeCode).ToList();
                var q1 = db.TM_Mem_Account.Where(p => p.MemberID == mid && p.AccountType == "1").FirstOrDefault();

                beforeValue = q1.Value1 + q1.Value2;
                if (qq1.Count == 1)//存在一样的记录，更新数据
                {
                    isLimit = true;
                    accountDetailID = qq1[0].AccountDetailID;
                    var q4 = db.TM_Mem_AccountDetail.Where(p => p.AccountDetailID == accountDetailID).FirstOrDefault();
                    q4.DetailValue += changeValue;
                    if (q4.AccountDetailType == "value1")
                    {
                        q1.Value1 += changeValue;
                        q1.NoBackAccount = (q1.NoBackAccount != null ? q1.NoBackAccount : 0) + changeValue;
                    }
                    else { q1.Value2 += changeValue; }
                    var entry4 = db.Entry(q4);
                    entry4.State = EntityState.Modified;
                }
                else if (qq1.Count == 2)//存在一样的记录，更新数据,可用和冻结各一条
                {
                    isLimit = true;

                    if (true)
                    {
                        accountDetailID = qq1.Where(p => p.AccountDetailType == "value2").FirstOrDefault().AccountDetailID;
                        q1.Value2 += changeValue;
                    }
                    else
                    {
                        accountDetailID = qq1.Where(p => p.AccountDetailType == "value1").FirstOrDefault().AccountDetailID;
                        q1.Value1 += changeValue;
                        q1.NoBackAccount = (q1.NoBackAccount != null ? q1.NoBackAccount : 0) + changeValue;
                    }
                    var q4 = db.TM_Mem_AccountDetail.Where(p => p.AccountDetailID == accountDetailID).FirstOrDefault();
                    q4.DetailValue += changeValue;
                    var entry4 = db.Entry(q4);
                    entry4.State = EntityState.Modified;
                }
                else //不存在一样的记录，添加新数据
                {
                    //如果可退，可并入value1，否则存NoBackAccount
                    if (true)
                    {
                        q1.Value2 = q1.Value2 + changeValue;

                        TM_Mem_AccountDetail ent = new TM_Mem_AccountDetail();
                        ent.AccountID = q1.AccountID;
                        ent.AccountDetailType = "value2";
                        ent.DetailValue = changeValue;
                        ent.SpecialDate1 = startDate;

                        ent.AddedDate = DateTime.Now;
                        ent.AddedUser = userId.ToString();
                        ent.ModifiedDate = DateTime.Now;
                        ent.ModifiedUser = userId.ToString();
                        db.TM_Mem_AccountDetail.Add(ent);
                        db.SaveChanges();

                        var q2 = db.TM_Mem_AccountDetail.Where(p => p.AccountID == actId && p.AccountDetailType == "value2").OrderByDescending(p => p.AddedDate).FirstOrDefault();
                        accountDetailID = q2.AccountDetailID;

                    }
                    else
                    {
                        isLimit = true;
                        q1.Value1 = q1.Value1 + changeValue;
                        q1.NoBackAccount = (q1.NoBackAccount != null ? q1.NoBackAccount : 0) + changeValue;
                        //添加一个不可退明细
                        TM_Mem_AccountDetail ent = new TM_Mem_AccountDetail();
                        ent.AccountID = q1.AccountID;
                        ent.AccountDetailType = "value1";
                        ent.DetailValue = changeValue;
                        //ent.SpecialDate2 = startDate;
                        ent.IsNoBack = true;
                        ent.AddedDate = DateTime.Now;
                        ent.AddedUser = userId.ToString();
                        ent.ModifiedDate = DateTime.Now;
                        ent.ModifiedUser = userId.ToString();
                        db.TM_Mem_AccountDetail.Add(ent);
                        db.SaveChanges();

                        var q3 = db.TM_Mem_AccountDetail.Where(p => p.AccountID == actId && p.AccountDetailType == "value1").OrderByDescending(p => p.AddedDate).FirstOrDefault();
                        accountDetailID = q3.AccountDetailID;
                    }
                }

                //V_M_TM_Mem_Trade_charge
                //V_M_TM_Mem_Trade_charge charge = new V_M_TM_Mem_Trade_charge()
                //{
                //    DataGroupID = dataGroupId,
                //    TradeCode = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString(),
                //    TradeType = "charge",
                //    MemberID = mid,
                //    NeedLoyCompute = true,
                //    ChargeValueTrade = changeValue,
                //    ChargeTypeTrade = "1",
                //    IsInvoiceCharge = false,
                //    RechargeTimeTrade = DateTime.Now,
                //    StoreCodeRechargeTrade = storeCode,//storeCode1 != null ? storeCode1[0] : "",//门店,
                //    StoreNameRechargeTrade = Service.GetStoreNameByCode(storeCode).Obj[0].ToString(),
                //    AddedDate = DateTime.Now,
                //    AddedUser = userId.ToString(),
                //    ModifiedDate = DateTime.Now,
                //    ModifiedUser = userId.ToString()
                //};
                //var tt = db.AddViewRow<V_M_TM_Mem_Trade_charge, TM_Mem_Trade>(charge);
                db.SaveChanges();

                //log表中增加记录（不可退）
                TL_Mem_AccountChange actchange = new TL_Mem_AccountChange();

                actchange.AccountID = q1.AccountID;
                actchange.AccountDetailID = accountDetailID;
                actchange.AccountChangeType = "manu";
                actchange.ChangeValue = changeValue;
                actchange.ChangeValueBefore = beforeValue;
                actchange.ChangeReason = "储值页面";
                actchange.ReferenceNo = "";//tt.TradeID.ToString();
                actchange.HasReverse = false;
                actchange.AddedDate = DateTime.Now;
                actchange.AddedUser = userId.ToString();
                db.TL_Mem_AccountChange.Add(actchange);




                //增加限制条件
                if (isLimit != true)
                {
                    TM_Mem_AccountLimit model = new TM_Mem_AccountLimit();
                    model.AccountID = actId;
                    model.AccountDetailID = accountDetailID;
                    model.LimitValue = storeCode;
                    model.LimitType = "store";
                    model.AddedDate = DateTime.Now;
                    db.TM_Mem_AccountLimit.Add(model);
                    db.SaveChanges();
                }
                var entry1 = db.Entry(q1);
                entry1.State = EntityState.Modified;
                //var entry2 = db.Entry(q2);
                //entry2.State = EntityState.Modified;

                db.SaveChanges();
            }

            return new Result(true, "储值成功");
        }
        /// <summary>
        /// 获取不可退账户金额
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public static Result GetMemIsBackAccountInfo(string mid)
        {
            using (var db = new CRMEntities())
            {
                var query = from a in db.TM_Mem_Account
                            where a.MemberID == mid && a.AccountType == "1"
                            select new { a.Value1, a.Value2, Total = a.Value1 + a.Value2, a.NoBackAccount };

                return new Result(true, "", query.ToList());
            }
        }


        //仅获取本门店的可用积分
        public static Result GetMemIsBackAccountInfo_store(string mid, string storecode)
        {
            using (var db = new CRMEntities())
            {
                var query = from a in db.TM_Mem_AccountDetail
                            join b in db.TM_Mem_Account on a.AccountID equals b.AccountID
                            join c in db.TM_Mem_AccountLimit on a.AccountDetailID equals c.AccountDetailID
                            where b.MemberID == mid && b.AccountType == "1" && c.LimitType == "store" && c.LimitValue == storecode



                            group new { a.DetailValue, b.Value2 } by a.AccountID into g
                            select new
                            {
                                Value1 = g.Sum(p => p.DetailValue),
                                Value2 = 0,
                                ID = g.Key,

                            };



                var query_nolimit = from a in db.TM_Mem_AccountDetail
                                    join b in db.TM_Mem_Account on a.AccountID equals b.AccountID

                                    where b.MemberID == mid && b.AccountType == "1" && !(from s in db.TM_Mem_AccountLimit select s.AccountDetailID).Contains(a.AccountDetailID)
                                    group new { a.DetailValue, b.Value2 } by a.AccountID into g
                                    select new
                                    {
                                        Value1 = g.Sum(p => p.DetailValue),
                                        Value2 = 0,
                                        ID = g.Key,

                                    };

                if (query_nolimit.Count() == 0)
                {
                    var query_ttl = from a in query
                                    select new
                                    {
                                        Value1 = a.Value1,
                                        Value2 = 0,
                                    };

                    return new Result(true, "", query_ttl.ToList());
                }
                else
                {
                    var query_ttl = from a in query
                                    join b in query_nolimit on a.ID equals b.ID
                                    select new
                                    {
                                        Value1 = a.Value1 + b.Value1,
                                        Value2 = 0,
                                    };
                    return new Result(true, "", query_ttl.ToList());
                }









            }
        }



        #endregion

        #region 卡变更（挂失解挂）
        /// <summary>
        /// 卡变更列表
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="cardNo"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static Result CardChangeSearch(string dp, string orderNo, string cardNo, string phone, string status, string type, DateTime startTime, DateTime endTime, string rds, int pid)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                List<string> l = new List<string>();
                if (rds != "[]")
                {
                    List<string> rid = JsonHelper.Deserialize<List<string>>(rds);
                    StringBuilder limitSql = new StringBuilder();
                    limitSql.AppendLine("select distinct MemberID from TR_Mem_MemberLimit ");
                    var limlst = db.TM_AUTH_DataLimit.Where(p => p.HierarchyType == "role" && rid.Contains(p.HierarchyValue)
                                 && (p.PageID == 9999 || p.PageID == pid)).ToList();
                    if (limlst.Count > 0)
                    {
                        string limwhere = "";
                        foreach (var item in limlst)
                        {
                            if (limwhere == "")
                                limwhere = "where " + item.RangeType + " = '" + item.RangeValue + "'";
                            else
                                limwhere += " or " + item.RangeType + " = '" + item.RangeValue + "'";
                        }
                        limitSql.AppendLine(limwhere);
                    }
                    l = db.Database.SqlQuery<string>(limitSql.ToString(), DBNull.Value).ToList();
                }
                //var queryA = from u in db.TL_Mem_CardChange
                //             join b in db.TD_SYS_BizOption.Where(p => p.OptionType == "CardChangeType") on u.ChangeType equals b.OptionValue
                //             where (string.IsNullOrEmpty(cardNo.Trim()) ? true : (u.CardNo.Contains(cardNo)))
                //                 //&& u.AddedDate.CompareTo(startTime) >= 0 && u.AddedDate.CompareTo(endTime) <= 0
                //             && u.AddedDate >= startTime && u.AddedDate <= endTime
                //             && (u.ChangeType == "3" || u.ChangeType == "4")
                //             select new { u.LogID, u.CardNo, u.ChangeType, u.ChangePlace, u.ChangeTime, u.Remark, u.AddedDate, u.AddedUser, b.OptionText };

                //var query = from u in queryA
                //            join r in db.TM_Mem_Card on u.CardNo equals r.CardNo
                //            join t in db.TM_Mem_Ext on r.MemberID equals t.MemberID
                //            join us in db.TM_AUTH_User on u.AddedUser equals SqlFunctions.StringConvert((double)us.UserID).Trim()

                //            select new { u.LogID, CardNo = u.CardNo == null ? "" : u.CardNo, t.Str_Attr_3, u.ChangeType, u.OptionText, u.ChangePlace, u.ChangeTime, AddedUser = us.UserName, u.Remark, u.AddedDate };

                var que = from a in db.TM_JPOS_CardPrepare
                          join btmp in db.TL_Mem_CardChange on a.cardNumber equals btmp.CardNo into bt
                          from b in bt.DefaultIfEmpty()
                          join ctmp in db.TM_Mem_Card on b.CardNo equals ctmp.CardNo into ct
                          from c in ct.DefaultIfEmpty()
                          join dtmp in db.V_U_TM_Mem_Info on c.MemberID equals dtmp.MemberID into dt
                          from d in dt.DefaultIfEmpty()
                          join e in db.TD_SYS_BizOption.Where(p => p.OptionType.Contains("CardChangeType")) on b.ChangeType equals e.OptionValue
                          join f in db.TM_AUTH_User on b.AddedUser equals SqlFunctions.StringConvert((double)f.UserID).Trim()

                          where a.source == "CRM"
                          select new
                          {
                              a.tranId,
                              CardNo = b.CardNo == null ? "" : b.CardNo,
                              Mobile = d.Mobile == null ? "" : d.Mobile,
                              CusAndPhone = d.CustomerName + d.Mobile,
                              d.MemberID,
                              e.OptionValue,
                              e.OptionText,
                              a.isConfirm,
                              b.AddedUser,
                              b.AddedDate,
                              f.UserName

                          };


                var que2 = from a in db.TL_Mem_CardChange
                           join b in db.TM_JPOS_CardPrepare.Where(p => p.source == "CRM") on a.CardNo equals b.cardNumber
                           join c in db.TM_Mem_Card on a.CardNo equals c.CardNo
                           join d in db.V_U_TM_Mem_Info on c.MemberID equals d.MemberID
                           join e in db.TD_SYS_BizOption.Where(p => p.OptionType.Contains("CardChangeType")) on a.ChangeType equals e.OptionValue
                           join f in db.TM_AUTH_User on a.AddedUser equals SqlFunctions.StringConvert((double)f.UserID).Trim()
                           select new
                           {
                               b.tranId,
                               CardNo = a.CardNo == null ? "" : a.CardNo,
                               Mobile = d.Mobile == null ? "" : d.Mobile,
                               CusAndPhone = d.CustomerName + d.Mobile,
                               d.MemberID,
                               e.OptionValue,
                               e.OptionText,
                               b.isConfirm,
                               a.AddedUser,
                               a.AddedDate,
                               f.UserName
                           };
                if (!string.IsNullOrEmpty(orderNo.Trim())) que = que.Where(p => p.tranId.Contains(orderNo));
                if (!string.IsNullOrEmpty(cardNo.Trim())) que = que.Where(p => p.CardNo.Contains(cardNo));
                if (!string.IsNullOrEmpty(phone.Trim())) que = que.Where(p => p.Mobile.Contains(phone));
                //if (l.Any()) que = que.Where(p => l.Contains(p.MemberID));
                que = que.Where(p => p.OptionValue == type);
                bool s = bool.Parse(status);
                que = que.Where(p => p.isConfirm == s);
                que = que.Where(p => p.AddedDate >= startTime && p.AddedDate <= endTime);
                return new Result(true, "", new List<object> { que.ToDataTableSourceVsPage(myDp) });
            }
        }

        public static Result GetCardByCardChange(string dp, string cardNo, string memName, string memMobile, string vehicleNo, string rds, int pid)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                List<int> rid = JsonHelper.Deserialize<List<int>>(rds);
                var que = from a in db.TM_Mem_Card
                          join b in db.TM_Mem_Ext on a.MemberID equals b.MemberID
                          join s in db.V_M_TM_SYS_BaseData_store.FilterDataByAuth(rid, pid, 1) on b.Str_Attr_5 equals s.StoreCode
                          where a.Lock == false && a.Active == true
                          select new { CardNo = a.CardNo == null ? "" : a.CardNo, b.MemberID, b.Str_Attr_4, b.Str_Attr_3, a.AddedDate };
                //var que = from a in db.TL_Mem_CardChange.Where(p => p.ChangeType == "3" || p.ChangeType == "4")
                //          join b in db.TD_SYS_BizOption.Where(p => p.OptionType == "CardChangeType") on a.ChangeType equals b.OptionValue
                //          join r in db.TM_Mem_Card on a.CardNo equals r.CardNo
                //          join t in db.TM_Mem_Ext on r.MemberID equals t.MemberID
                //          join us in db.TM_AUTH_User on a.AddedUser equals SqlFunctions.StringConvert((double)us.UserID).Trim()
                //          select new { a.LogID, CardNo = a.CardNo == null ? "" : a.CardNo, r.MemberID, t.Str_Attr_4, t.Str_Attr_3, a.ChangeType, b.OptionText, a.ChangePlace, a.ChangeTime, AddedUser = us.UserName, a.Remark, a.AddedDate };
                if (!string.IsNullOrEmpty(cardNo.Trim())) que = que.Where(p => p.CardNo.Contains(cardNo));
                if (!string.IsNullOrEmpty(memName.Trim())) que = que.Where(p => p.Str_Attr_3.Contains(memName));
                if (!string.IsNullOrEmpty(memMobile.Trim())) que = que.Where(p => p.Str_Attr_4.Contains(memMobile));
                if (!string.IsNullOrEmpty(vehicleNo.Trim()))
                {
                    que = from a in que
                          join c in db.V_M_TM_Mem_SubExt_vehicle on a.MemberID equals c.MemberID
                          where c.PlateNumVehicle.Contains(vehicleNo)
                          select a;
                }

                return new Result(true, "", new List<object> { que.ToDataTableSourceVsPage(myDp) });
            }
        }

        /// <summary>
        ///挂失或者解挂列表
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="cardNo"></param>
        /// <param name="name"></param>
        /// <param name="mobile"></param>
        /// <param name="vehicleNo"></param>
        /// <returns></returns>
        public static Result SearchCardList(string dp, string cardNo, string phone, string mem, string idNum, int dataGroupId, int pid, string rds)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                List<string> l = new List<string>();
                if (rds != "[]")
                {
                    List<string> rid = JsonHelper.Deserialize<List<string>>(rds);
                    StringBuilder limitSql = new StringBuilder();
                    limitSql.AppendLine("select distinct MemberID from TR_Mem_MemberLimit ");
                    var limlst = db.TM_AUTH_DataLimit.Where(p => p.HierarchyType == "role" && rid.Contains(p.HierarchyValue)
                                 && (p.PageID == 9999 || p.PageID == pid)).ToList();
                    if (limlst.Count > 0)
                    {
                        string limwhere = "";
                        foreach (var item in limlst)
                        {
                            if (limwhere == "")
                                limwhere = "where " + item.RangeType + " = '" + item.RangeValue + "'";
                            else
                                limwhere += " or " + item.RangeType + " = '" + item.RangeValue + "'";
                        }
                        limitSql.AppendLine(limwhere);
                    }
                    l = db.Database.SqlQuery<string>(limitSql.ToString(), DBNull.Value).ToList();
                }
                var query = from t in db.TM_Mem_Ext
                            join u in db.TM_Mem_Card.Where(p => p.Active == true) on t.MemberID equals u.MemberID
                            join s in db.TM_JPOS_CardPrepare.Where(p => p.source == "CRM" && p.isConfirm_Unlock == true && p.isConfirm != true) on u.CardNo equals s.cardNumber into ss
                            from s in ss.DefaultIfEmpty()
                            //where u.CardType != "2"
                            select new { u.MemberID, u.CardNo, t.Str_Attr_3, t.Str_Attr_9, t.Str_Attr_4, u.Lock, u.Active, s.isConfirm };



                if (!string.IsNullOrEmpty(cardNo.Trim())) query = query.Where(p => p.CardNo.Contains(cardNo));
                if (!string.IsNullOrEmpty(mem.Trim())) query = query.Where(p => p.MemberID.Contains(mem));
                if (!string.IsNullOrEmpty(phone.Trim())) query = query.Where(p => p.Str_Attr_4.Contains(phone));
                if (!string.IsNullOrEmpty(idNum.Trim())) query = query.Where(p => p.Str_Attr_9.Contains(idNum));
                //if (l.Any()) query = query.Where(p => l.Contains(p.MemberID));
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });

            }
        }
        //public static Result GetCardInfo(string type,string num)
        //{
        //     using (CRMEntities db = new CRMEntities())
        //     {                  

        //           var query=(from a in db.TM_Mem_Card.Where(p=>p.CardNo==num)
        //                  join b in db.V_U_TM_Mem_Info on a.MemberID equals b.MemberID
        //                  select new {a.MemberID,
        //                      a.CardNo,b.Mobile,
        //                      b.CustomerName,
        //                      b.CertificateType,
        //                      b.CertificateNo}).FirstOrDefault();

        //         if(type=="2")
        //         {
        //            query=(from a in db.TM_Mem_Card
        //                  join b in db.V_U_TM_Mem_Info on a.MemberID equals b.MemberID
        //                  where b.Mobile==num
        //                  select new {a.MemberID,
        //                      a.CardNo,b.Mobile,
        //                      b.CustomerName,
        //                      b.CertificateType,
        //                      b.CertificateNo}).FirstOrDefault(); 
        //         }
        //         else if(type=="3")
        //         {
        //             query=(from a in db.TM_Mem_Card
        //                  join b in db.V_U_TM_Mem_Info on a.MemberID equals b.MemberID
        //                  where a.MemberID==num
        //                  select new {a.MemberID,
        //                      a.CardNo,b.Mobile,
        //                      b.CustomerName,
        //                      b.CertificateType,
        //                      b.CertificateNo}).FirstOrDefault(); 
        //         }
        //         else if(type=="4")
        //         {
        //             query=(from a in db.TM_Mem_Card
        //                  join b in db.V_U_TM_Mem_Info on a.MemberID equals b.MemberID
        //                  where b.CertificateNo==num
        //                  select new {a.MemberID,
        //                      a.CardNo,b.Mobile,
        //                      b.CustomerName,
        //                      b.CertificateType,
        //                      b.CertificateNo}).FirstOrDefault(); 
        //         }
        //         return new Result(true,"",query);
        //    }
        //}
        /// <summary>
        /// 锁卡保存
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static Result LockAddCardPrepare(CardPrepare card, string type)
        {
            using (CRMEntities db = new CRMEntities())
            {
                try
                {
                    TM_JPOS_CardPrepare tmp = new TM_JPOS_CardPrepare();
                    tmp = new TM_JPOS_CardPrepare();
                    tmp.id = Guid.NewGuid().ToString("N");
                    tmp.cardNumber = card.cardNumber;
                    tmp.tranId = "No" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                        DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() +
                        DateTime.Now.Second.ToString() + Guid.NewGuid().ToString("N").Substring(0, 4);
                    if (type == "lock")
                    {
                        tmp.isConfirm = false;
                        //true 挂失 false 解挂
                        tmp.isConfirm_Unlock = true;
                    }
                    else
                    {
                        tmp.isConfirm = false;
                        tmp.isConfirm_Unlock = false;
                    }
                    tmp.lossCardInfo = card.lossCardInfo;
                    tmp.operatorStr = card.operatorStr;
                    tmp.posNo = card.posNo;
                    tmp.storeCode = card.storeCode;
                    tmp.time = DateTime.Now;
                    tmp.tranId = card.tranId;
                    tmp.tranTime = DateTime.Now;
                    tmp.cardHolder = card.cardHolder;
                    tmp.source = "CRM";
                    tmp.agent = card.agent;
                    tmp.agentCertificateNo = card.agentCertificateNo;
                    tmp.agentMobile = card.agentMobile;
                    tmp.serviceCharge = card.serviceCharge;
                    db.TM_JPOS_CardPrepare.Add(tmp);
                    db.SaveChanges();
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
                }
            }

            return new Result(true, "锁卡保存成功");
        }
        /// <summary>
        /// 锁卡审核
        /// </summary>
        /// <param name="cCard"></param>
        /// <returns></returns>
        public static Result CardLockCheck(CardChange cCard, string type)
        {
            using (CRMEntities db = new CRMEntities())
            {
                //cardPrepare修改记录
                bool isLock = true;
                if (type != "lock")
                    isLock = false;
                var cc = db.TM_JPOS_CardPrepare.FirstOrDefault(p => p.cardNumber == cCard.CardNo && p.source == "CRM" && p.isConfirm_Unlock == isLock && p.isConfirm == false);
                cc.isConfirm = true;

                var dd = db.TM_Card_CardNo.Where(i => i.CardNo == cCard.CardNo).FirstOrDefault();
                if (dd.Status == "-3")
                {
                    return new Result(false, "作废卡不能挂失或解挂");
                }
                if (type == "lock")
                {
                    dd.Status = "-1";
                }

                else
                {
                    dd.Status = "1";
                }

                //会员卡修改状态
                var u = db.TM_Mem_Card.FirstOrDefault(p => p.CardNo == cCard.CardNo);
                if (u != null)
                {
                    if (u.CardType != "1")
                    {
                        return new Result(false, "非实体卡不允许挂失");
                    }
                    u.CardNo = cCard.CardNo;
                    u.Lock = !u.Lock;
                    u.Active = !u.Active;
                }
                //cardChange表添加记录
                TL_Mem_CardChange tm = new TL_Mem_CardChange();
                tm.CardNo = cCard.CardNo;
                tm.ChangePlace = cCard.ChangePlace;
                tm.ChangeTime = DateTime.Now;
                tm.ChangeType = "2";
                tm.AddedDate = DateTime.Now;
                tm.AddedUser = cCard.AddedUser;
                tm.OldCardNo = cCard.OldCardNo;
                tm.Agent = cc.agent;
                tm.AgentCertificateNo = cc.agentCertificateNo;
                tm.AgentMobile = cc.agentMobile;
                tm.ServiceCharge = cc.serviceCharge;
                db.TL_Mem_CardChange.Add(tm);
                db.SaveChanges();
            }
            return new Result(true, "锁卡审核成功");
        }
        /// <summary>
        /// 保存并审核
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static Result LockSaveAndCheck(CardPrepare pCard, CardChange cCard, string type)
        {
            using (CRMEntities db = new CRMEntities())
            {
                TM_JPOS_CardPrepare tmp = new TM_JPOS_CardPrepare();
                tmp = new TM_JPOS_CardPrepare();
                tmp.id = Guid.NewGuid().ToString("N");
                tmp.cardNumber = pCard.cardNumber;
                tmp.tranId = "No" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                    DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() +
                    DateTime.Now.Second.ToString() + Guid.NewGuid().ToString("N").Substring(0, 4);
                if (type == "lock")
                {
                    tmp.isConfirm = true;
                    tmp.isConfirm_Unlock = true;
                }
                else
                {
                    tmp.isConfirm = true;
                    tmp.isConfirm_Unlock = false;
                }
                tmp.lossCardInfo = pCard.lossCardInfo;
                tmp.operatorStr = pCard.operatorStr;
                tmp.posNo = pCard.posNo;
                tmp.storeCode = pCard.storeCode;
                tmp.time = DateTime.Now;
                tmp.tranTime = DateTime.Now;
                tmp.cardHolder = pCard.cardHolder;
                tmp.source = "CRM";
                tmp.agent = pCard.agent;
                tmp.agentCertificateNo = pCard.agentCertificateNo;
                tmp.agentMobile = pCard.agentMobile;
                tmp.serviceCharge = pCard.serviceCharge;
                db.TM_JPOS_CardPrepare.Add(tmp);

                //会员卡修改状态
                var u = db.TM_Mem_Card.FirstOrDefault(p => p.CardNo == cCard.CardNo);
                if (u != null)
                {
                    if (u.CardType != "1")
                    {
                        return new Result(false, "非实体卡不允许挂失或解挂");
                    }
                    u.CardNo = cCard.CardNo;
                    u.Lock = !u.Lock;
                    u.Active = !u.Active;
                }

                //Update  tm_card_cardno
                var cn = db.TM_Card_CardNo.Where(i => i.CardNo == cCard.CardNo).FirstOrDefault();
                if (cn != null)
                {
                    if (cn.Status == "-3")
                    {
                        return new Result(false, "作废卡不允许挂失解挂");
                    }

                    if (type == "lock")
                    {
                        cn.Status = "-1";
                    }

                    else
                    {
                        cn.Status = "1";
                    }

                }


                //cardChange表添加记录
                TL_Mem_CardChange tm = new TL_Mem_CardChange();
                tm.CardNo = cCard.CardNo;
                tm.ChangePlace = cCard.ChangePlace;
                tm.ChangeTime = DateTime.Now;
                tm.ChangeType = "2";
                tm.AddedDate = DateTime.Now;
                tm.AddedUser = cCard.AddedUser;
                tm.OldCardNo = cCard.OldCardNo;
                tm.Agent = pCard.agent;
                tm.AgentCertificateNo = pCard.agentCertificateNo;
                tm.AgentMobile = pCard.agentMobile;
                tm.ServiceCharge = pCard.serviceCharge;
                db.TL_Mem_CardChange.Add(tm);
                db.SaveChanges();
            }
            return new Result(true, "保存并审核成功");
        }
        #region 批量挂失解挂
        /// <summary>
        /// 批量挂失解挂查询
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="cardNo"></param>
        /// <param name="phone"></param>
        /// <param name="mem"></param>
        /// <param name="idNum"></param>
        /// <param name="type"></param>
        /// <param name="dataGroupId"></param>
        /// <param name="pid"></param>
        /// <param name="rds"></param>
        /// <returns></returns>
        public static Result BulkSearchCardList(string dp, string cardNo, string phone, string mem, string idNum, string type, int dataGroupId, int pid, string rds)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                List<string> l = new List<string>();
                if (rds != "[]")
                {
                    List<string> rid = JsonHelper.Deserialize<List<string>>(rds);
                    StringBuilder limitSql = new StringBuilder();
                    limitSql.AppendLine("select distinct MemberID from TR_Mem_MemberLimit ");
                    var limlst = db.TM_AUTH_DataLimit.Where(p => p.HierarchyType == "role" && rid.Contains(p.HierarchyValue)
                                 && (p.PageID == 9999 || p.PageID == pid)).ToList();
                    if (limlst.Count > 0)
                    {
                        string limwhere = "";
                        foreach (var item in limlst)
                        {
                            if (limwhere == "")
                                limwhere = "where " + item.RangeType + " = '" + item.RangeValue + "'";
                            else
                                limwhere += " or " + item.RangeType + " = '" + item.RangeValue + "'";
                        }
                        limitSql.AppendLine(limwhere);
                    }
                    l = db.Database.SqlQuery<string>(limitSql.ToString(), DBNull.Value).ToList();
                }
                bool isActive = true;
                if (type != "lock")
                {
                    isActive = false;
                }
                var query = from t in db.TM_Mem_Ext
                            join u in db.TM_Mem_Card.Where(p => p.Active == isActive) on t.MemberID equals u.MemberID
                            join s in db.TM_JPOS_CardPrepare.Where(p => p.source == "CRM") on u.CardNo equals s.cardNumber into ss
                            from s in ss.DefaultIfEmpty()
                            select new { u.MemberID, u.CardNo, t.Str_Attr_3, t.Str_Attr_9, t.Str_Attr_4, u.Lock, u.Active, s.isConfirm, s.isConfirm_Unlock };

                //若输入车牌号
                //if (!string.IsNullOrEmpty(vehicleNo.Trim()))
                //{
                //    query = from u in query
                //            join v in db.TM_Mem_SubExt on u.MemberID equals v.MemberID
                //            where v.Str_Attr_1.Contains(vehicleNo)

                //            select new { u.MemberID, u.CardNo, u.Str_Attr_3, u.Str_Attr_4, u.Lock, u.Active };
                //}

                if (!string.IsNullOrEmpty(cardNo.Trim())) query = query.Where(p => p.CardNo.Contains(cardNo));
                if (!string.IsNullOrEmpty(mem.Trim())) query = query.Where(p => p.MemberID.Contains(mem));
                if (!string.IsNullOrEmpty(phone.Trim())) query = query.Where(p => p.Str_Attr_4.Contains(phone));
                if (!string.IsNullOrEmpty(idNum.Trim())) query = query.Where(p => p.Str_Attr_9.Contains(idNum));
                if (l.Any()) query = query.Where(p => l.Contains(p.MemberID));
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });

            }
        }
        /// <summary>
        /// 批量审核
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Result BulkCardsCheck(List<CardChange> cards, string type)
        {
            using (CRMEntities db = new CRMEntities())
            {
                foreach (var cCard in cards)
                {
                    //cardPrepare修改记录
                    var cc = db.TM_JPOS_CardPrepare.FirstOrDefault(p => p.cardNumber == cCard.CardNo && p.source == "CRM");
                    if (type == "lock")
                    {
                        cc.isConfirm = true;
                        cc.isConfirm_Unlock = null;
                    }
                    else
                    {
                        cc.isConfirm_Unlock = true;
                        cc.isConfirm = null;
                    }
                    //会员卡修改状态
                    var u = db.TM_Mem_Card.FirstOrDefault(p => p.CardNo == cCard.CardNo);
                    if (u != null)
                    {
                        if (u.CardType != "1")
                        {
                            return new Result(false, "非实体卡不允许挂失");
                        }
                        u.CardNo = cCard.CardNo;
                        u.Lock = !u.Lock;
                        u.Active = !u.Active;
                    }
                    //cardChange表添加记录
                    TL_Mem_CardChange tm = new TL_Mem_CardChange();
                    tm.CardNo = cCard.CardNo;
                    tm.ChangePlace = cCard.ChangePlace;
                    tm.ChangeTime = DateTime.Now;
                    tm.ChangeType = "2";
                    tm.AddedDate = DateTime.Now;
                    tm.AddedUser = cCard.AddedUser;
                    tm.OldCardNo = cCard.OldCardNo;
                    tm.Agent = cc.agent;
                    tm.AgentCertificateNo = cc.agentCertificateNo;
                    tm.AgentMobile = cc.agentMobile;
                    tm.ServiceCharge = cc.serviceCharge;
                    db.TL_Mem_CardChange.Add(tm);
                    db.SaveChanges();
                }
            }
            return new Result(true, "锁卡审核成功");
        }
        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Result BulkCardsPrepare(List<CardPrepare> cards, string type)
        {
            using (CRMEntities db = new CRMEntities())
            {
                try
                {
                    foreach (var card in cards)
                    {
                        bool b = false;
                        TM_JPOS_CardPrepare tmp = db.TM_JPOS_CardPrepare.FirstOrDefault(p => p.cardNumber == card.cardNumber && p.source == "CRM");
                        if (tmp == null)
                        {
                            tmp = new TM_JPOS_CardPrepare();
                            tmp.id = Guid.NewGuid().ToString("N");
                            tmp.cardNumber = card.cardNumber;
                            tmp.tranId = "No" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                                DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() +
                                DateTime.Now.Second.ToString() + Guid.NewGuid().ToString("N").Substring(0, 4);
                            b = true;
                        }
                        if (type == "lock")
                        {
                            tmp.isConfirm = false;
                        }
                        else
                        {
                            tmp.isConfirm_Unlock = false;
                        }
                        tmp.lossCardInfo = card.lossCardInfo;
                        tmp.operatorStr = card.operatorStr;
                        tmp.posNo = card.posNo;
                        tmp.storeCode = card.storeCode;
                        tmp.time = DateTime.Now;
                        tmp.tranId = card.tranId;
                        tmp.tranTime = DateTime.Now;
                        tmp.cardHolder = card.cardHolder;
                        tmp.source = "CRM";
                        tmp.agent = card.agent;
                        tmp.agentCertificateNo = card.agentCertificateNo;
                        tmp.agentMobile = card.agentMobile;
                        tmp.serviceCharge = card.serviceCharge;
                        if (b)
                        {
                            db.TM_JPOS_CardPrepare.Add(tmp);
                        }
                        db.SaveChanges();
                    }
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

                }
                return new Result(true, "批量保存成功");
            }

        }
        #endregion
        /// <summary>
        /// 保存卡变更信息
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static Result AddCardChange(CardChange card)
        {
            using (CRMEntities db = new CRMEntities())
            {
                bool loc = true;
                //更新状态
                var u = db.TM_Mem_Card.FirstOrDefault(p => p.CardNo == card.CardNo);
                if (u != null)
                {
                    if (u.CardType != "1")
                    {
                        return new Result(false, "非实体卡不允许挂失");
                    }
                    u.CardNo = card.CardNo;
                    u.Lock = !u.Lock;
                    u.Active = !u.Active;
                    loc = u.Lock;
                }
                else
                {


                    TM_Mem_Card c = new TM_Mem_Card();
                    c.CardNo = card.CardNo;
                    c.MemberID = card.MemberId;
                    c.Active = true;
                    c.Lock = true;
                    c.AddedDate = DateTime.Now;
                    c.AddedUser = card.AddedUser;

                    db.TM_Mem_Card.Add(c);
                    loc = c.Lock;
                }

                var tme = db.TM_Mem_Ext.Where(p => p.MemberID == card.MemberId).FirstOrDefault();
                if (tme != null)
                {
                    if (loc)
                    {
                        tme.Str_Attr_17 = "0";
                        tme.Str_Attr_1 = "0";
                    }
                    else
                    {
                        tme.Str_Attr_17 = "1";
                        tme.Str_Attr_1 = "1";
                    }
                }

                TL_Mem_CardChange tmp = new TL_Mem_CardChange();
                tmp.CardNo = card.CardNo;
                tmp.ChangePlace = card.ChangePlace;
                tmp.ChangeTime = DateTime.Now;
                tmp.ChangeType = card.ChangeType;
                tmp.AddedDate = DateTime.Now;
                tmp.AddedUser = card.AddedUser;
                tmp.OldCardNo = card.OldCardNo;
                tmp.Remark = card.Remark;
                db.TL_Mem_CardChange.Add(tmp);
                db.SaveChanges();
            }

            return new Result(true, "卡变更成功");
        }
        /// <summary>
        /// 获取卡解挂列表
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="cardNo"></param>
        /// <param name="name"></param>
        /// <param name="mobile"></param>
        /// <param name="vehicleNo"></param>
        /// <returns></returns>
        public static Result SearchCardUnLockList(string dp, string cardNo, string phone, string mem, string idNum, int pid, string rds)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                List<string> l = new List<string>();
                if (rds != "[]")
                {
                    List<string> rid = JsonHelper.Deserialize<List<string>>(rds);
                    StringBuilder limitSql = new StringBuilder();
                    limitSql.AppendLine("select distinct MemberID from TR_Mem_MemberLimit ");
                    var limlst = db.TM_AUTH_DataLimit.Where(p => p.HierarchyType == "role" && rid.Contains(p.HierarchyValue)
                                 && (p.PageID == 9999 || p.PageID == pid)).ToList();
                    if (limlst.Count > 0)
                    {
                        string limwhere = "";
                        foreach (var item in limlst)
                        {
                            if (limwhere == "")
                                limwhere = "where " + item.RangeType + " = '" + item.RangeValue + "'";
                            else
                                limwhere += " or " + item.RangeType + " = '" + item.RangeValue + "'";
                        }
                        limitSql.AppendLine(limwhere);
                    }
                    l = db.Database.SqlQuery<string>(limitSql.ToString(), DBNull.Value).ToList();
                }
                var query = from t in db.TM_Mem_Ext
                            join u in db.TM_Mem_Card.Where(p => p.Active == false) on t.MemberID equals u.MemberID
                            join s in db.TM_JPOS_CardPrepare.Where(p => p.source == "CRM" && p.isConfirm_Unlock == false && p.isConfirm != true) on u.CardNo equals s.cardNumber into ss
                            from s in ss.DefaultIfEmpty()
                            select new { u.MemberID, u.CardNo, t.Str_Attr_3, t.Str_Attr_9, t.Str_Attr_4, u.Lock, u.Active, s.isConfirm };

                //若输入车牌号
                //if (!string.IsNullOrEmpty(vehicleNo.Trim()))
                //{
                //    query = from u in query
                //            join v in db.TM_Mem_SubExt on u.MemberID equals v.MemberID
                //            where v.Str_Attr_1.Contains(vehicleNo)

                //            select new { u.MemberID, u.CardNo, u.Str_Attr_3, u.Str_Attr_4, u.Lock, u.Active };
                //}

                if (!string.IsNullOrEmpty(cardNo.Trim())) query = query.Where(p => p.CardNo.Contains(cardNo));
                if (!string.IsNullOrEmpty(mem.Trim())) query = query.Where(p => p.MemberID.Contains(mem));
                if (!string.IsNullOrEmpty(phone.Trim())) query = query.Where(p => p.Str_Attr_4.Contains(phone));
                if (!string.IsNullOrEmpty(idNum.Trim())) query = query.Where(p => p.Str_Attr_9.Contains(idNum));
                //if (l.Any()) query = query.Where(p => l.Contains(p.MemberID));

                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }
        /// <summary>
        /// 旧卡换新卡
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static Result SaveCardChange(CardChange card)
        {
            using (CRMEntities db = new CRMEntities())
            {
                try
                {
                    var o = db.TM_Mem_Card.Where(p => p.CardNo == card.CardNo);
                    if (o.Count() > 0)
                    {
                        return new Result(false, "新卡号已存在,请重新确认卡号");
                    }

                    var q = db.TM_Mem_Card.Where(p => p.CardNo == card.OldCardNo).FirstOrDefault();

                    TM_Mem_Card tm = new TM_Mem_Card();
                    tm.AddedDate = DateTime.Now;
                    tm.AddedUser = card.AddedUser;
                    tm.Active = true;
                    tm.CardType = q.CardType;
                    tm.Lock = false;
                    if (card.CardNo == null || card.CardNo == "")
                        return new Result(true, "换卡时新卡号不能为空");
                    tm.CardNo = card.CardNo;
                    tm.MemberID = card.MemberId;
                    db.TM_Mem_Card.Add(tm);
                    var tme = db.TM_Mem_Ext.Where(p => p.MemberID == card.MemberId).FirstOrDefault();
                    if (tme != null)
                    {
                        tme.Str_Attr_2 = card.CardNo;
                        tme.Str_Attr_17 = "1";
                    }

                    //旧卡失效

                    q.Active = false;
                    q.Lock = true;

                    //新卡激活记录
                    TL_Mem_CardChange tmp = new TL_Mem_CardChange();
                    tmp.CardNo = card.CardNo;
                    tmp.ChangePlace = card.ChangePlace;
                    tmp.ChangeTime = DateTime.Now;
                    tmp.ChangeType = "1";//激活
                    tmp.AddedDate = DateTime.Now;
                    tmp.AddedUser = card.AddedUser;
                    tmp.OldCardNo = card.OldCardNo;
                    tmp.Remark = card.Remark;
                    db.TL_Mem_CardChange.Add(tmp);
                    db.SaveChanges();
                    //旧卡失效记录
                    TL_Mem_CardChange tmp1 = new TL_Mem_CardChange();
                    tmp1.CardNo = card.OldCardNo;
                    tmp1.ChangePlace = card.ChangePlace;
                    tmp1.ChangeTime = DateTime.Now;
                    tmp1.ChangeType = "2";//失效
                    tmp1.AddedDate = DateTime.Now;
                    tmp1.AddedUser = card.AddedUser;
                    tmp1.Remark = card.Remark;
                    db.TL_Mem_CardChange.Add(tmp1);
                    db.SaveChanges();
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
                }

            }

            return new Result(true, "换卡成功");
        }

        public static Result GetMemberNameByPage1(string dp, int dataGroupId, string pageIds, string dataRoleIds, string cardNo, string memName, string memMobile, string vehicleNo)
        {
            //DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            //using (CRMEntities db = new CRMEntities())
            //{
            //    //var queryMember = from m in db.V_U_TM_Mem_Info.FilterDataByAuth(authModel.RoleIDs, authModel.CurPageID.Value, authModel.DataGroupID.Value)
            //    //                  join s in db.V_M_TM_SYS_BaseData_store.FilterDataByAuth(authModel.RoleIDs, authModel.CurPageID.Value, authModel.DataGroupID.Value, false) on m.RegisterStoreCode equals s.StoreCode
            //    //                  select new { m, s.StoreBrandCode };
            //    //var query = (from a in queryMember//.FilterDataByAuth(authModel.RoleIDs, authModel.CurPageID.Value, authModel.DataGroupID.Value)
            //    //             join c in db.V_M_TM_Mem_SubExt_vehicle on a.m.MemberID equals c.MemberID into cc
            //    //             from ccg in cc.DefaultIfEmpty()
            //    //             select new
            //    //             {
            //    //                 a.m.MemberID,
            //    //                 a.m.CustomerMobile,
            //    //                 a.m.CustomerName,
            //    //                 a.m.MemberCardNo,
            //    //                 a.m.VinKey,
            //    //                 ccg.VehicleNo,
            //    //                 ccg.VIN
            //    //             }).Distinct();
            //    //if (!string.IsNullOrEmpty(cardNo)) query = query.Where(p => p.MemberCardNo.Contains(cardNo));
            //    //if (!string.IsNullOrEmpty(memName)) query = query.Where(p => p.CustomerName.Contains(memName));
            //    //if (!string.IsNullOrEmpty(memMobile)) query = query.Where(p => p.CustomerMobile.Contains(memMobile));
            //    //if (!string.IsNullOrEmpty(vehicleNo)) query = query.Where(p => p.VehicleNo.Contains(vehicleNo));
            //    //if (!string.IsNullOrEmpty(vehicleVIN)) query = query.Where(p => p.VIN.Contains(vehicleVIN));
            //    ObjectParameter recordCount = new ObjectParameter("RecordCount", typeof(int));
            //    //var query1 = db.sp_CRM_Mem_SearchForAccountChange(dataGroupId, pageIds, dataRoleIds, cardNo, memName, memMobile, vehicleNo, "", myDp.iDisplayStart / myDp.iDisplayLength, myDp.iDisplayLength, recordCount).ToList();
            //    var query = db.sp_CRM_Mem_Search(dataGroupId, pageIds, dataRoleIds, null,
            //        cardNo, memName, memMobile, null, vehicleNo, null, null, null, null, null, null, null,
            //        0, 0, recordCount).ToList();
            //    var q1 = query.Where(p => p.DataGroupID != 1).ToList();
            //    var q2 = q1.Where(p => p.CustomerStatus == "1").ToList();
            //    //跨店授权的用户也需要添加过来（不过只能改）

            //    var q = q2.Skip(myDp.iDisplayStart).Take(myDp.iDisplayLength).ToList();
            //    var res = new DatatablesSourceVsPage();
            //    res.iDisplayStart = myDp.iDisplayStart;
            //    res.iDisplayLength = myDp.iDisplayLength;
            //    res.iTotalRecords = q2.Count();
            //    res.aaData = q;
            return new Result(true, "");

        }

        /// <summary>
        /// 批量卡挂失
        /// </summary>
        /// <param name="batchCardNo"></param>
        /// <returns></returns>
        public static Result BatchCardLock(string batchCardNo)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    string[] cardNoArray = batchCardNo.Split(',');
                    for (int i = 0; i < cardNoArray.Length; i++)
                    {
                        string cardNo = cardNoArray[i];
                        var query = db.TM_Mem_Card.Where(n => n.CardNo == cardNo).FirstOrDefault();
                        query.Lock = true;
                        query.IsReissured = false;
                        query.Status = -1;
                    }
                    db.SaveChanges();
                    return new Result(true, "批量挂失成功");
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("BatchCardLock :" + ex.ToString());
                return new Result(true, "批量挂失失败");
                throw;
            }
        }
        #endregion

        #region 储值历史打印
        public static Result GetActHistoryByPage(string dp, int dataGroupId, string memName, string memMobile, string vehicleNo, DateTime? startDate, DateTime? endDate, int userId)
        {
            throw new NotImplementedException();

            //DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var query = from a in db.TL_Mem_AccountChange.Where(p => p.ChangeReason == "储值页面")
            //                join b in db.TM_Mem_Account.Where(p => p.AccountType == "1") on a.AccountID equals b.AccountID into bb
            //                from bba in bb.DefaultIfEmpty()
            //                join c in db.V_U_TM_Mem_Info on bba.MemberID equals c.MemberID into cc
            //                from cca in cc.DefaultIfEmpty()
            //                join d in db.V_M_TM_Mem_SubExt_vehicle on bba.MemberID equals d.MemberID into dd
            //                from dda in dd.DefaultIfEmpty()
            //                join e in db.TM_AUTH_User on a.AddedUser equals SqlFunctions.StringConvert((double)e.UserID).Trim() into ee
            //                from eea in ee.DefaultIfEmpty()
            //                join h in db.V_Sys_DataGroupRelation on cca.DataGroupID equals h.SubDataGroupID into hh
            //                from hcg in hh.DefaultIfEmpty()
            //                join i in db.V_M_TM_Mem_Trade_charge on a.ReferenceNo equals SqlFunctions.StringConvert((double)i.TradeID).Trim() into ii
            //                from iia in ii.DefaultIfEmpty()
            //                where hcg.DataGroupID == dataGroupId
            //                select new
            //                {
            //                    a.ChangeValue,//变更值
            //                    a.ChangeValueBefore,
            //                    ChangeAfter = (a.ChangeValueBefore == null ? 0 : a.ChangeValueBefore) + a.ChangeValue,
            //                    a.AddedDate,
            //                    a.AddedUser,
            //                    eea.UserName,
            //                    cca.CustomerName,
            //                    cca.MemberCardNo,
            //                    cca.CustomerMobile,
            //                    cca.RegisterStoreCode,
            //                    iia.StoreCodeRechargeTrade,
            //                    cca.DataGroupID,
            //                    dda.CarNo,//车牌
            //                    dda.VIN,//车架号
            //                };
            //    //门店过滤
            //    List<string> brand = new List<string>();
            //    var store = Service.GetStoreNameByUserID(userId, ref brand);//获取门店
            //    if (!string.IsNullOrEmpty(store) && brand.Count <= 0)
            //    {
            //        var storeCode1 = store.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //        query = query.Where(p => storeCode1.Contains(p.StoreCodeRechargeTrade));
            //    }

            //    if (!string.IsNullOrEmpty(memName)) query = query.Where(p => p.CustomerName.Contains(memName));
            //    if (!string.IsNullOrEmpty(memMobile)) query = query.Where(p => p.CustomerMobile.Contains(memMobile));
            //    if (!string.IsNullOrEmpty(vehicleNo)) query = query.Where(p => p.CarNo.Contains(vehicleNo));
            //    if (startDate != null) query = query.Where(p => p.AddedDate >= startDate);
            //    if (endDate != null) query = query.Where(p => p.AddedDate <= endDate);
            //return new Result(true, "");
            //}
        }
        #endregion

        #region 套餐销售历史打印
        public static Result GetPackageSaleHistoryByPage(string dp, int dataGroupId, string memName, string memMobile, string vehicleNo, DateTime? startDate, DateTime? endDate, int userId)
        {
            throw new NotImplementedException();

            //DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var query = from a in db.V_M_TM_Mem_Trade_package.Where(p => p.TradeType == "package")
            //                join b in db.TM_AUTH_User on a.AddedUser equals SqlFunctions.StringConvert((double)b.UserID).Trim() into bb
            //                from bba in bb.DefaultIfEmpty()
            //                join c in db.V_U_TM_Mem_Info on a.MemberID equals c.MemberID into cc
            //                from cca in cc.DefaultIfEmpty()
            //                join d in db.V_M_TM_Mem_SubExt_vehicle on a.MemberID equals d.MemberID into dd
            //                from dda in dd.DefaultIfEmpty()
            //                join h in db.V_Sys_DataGroupRelation on cca.DataGroupID equals h.SubDataGroupID into hh
            //                from hcg in hh.DefaultIfEmpty()
            //                where hcg.DataGroupID == dataGroupId
            //                select new
            //                {
            //                    cca.MemberCardNo,
            //                    cca.CustomerName,
            //                    cca.CustomerMobile,
            //                    cca.RegisterStoreCode,
            //                    a.BuyPackageDateTrade,//购买日期
            //                    a.PackageDecTrade,
            //                    a.PackageTotalPriceTrade,
            //                    a.StoreCodePackageTrade,//购买套餐都有门店
            //                    a.AddedDate,
            //                    AddedUser = bba.UserName,
            //                    a.TradeID,
            //                    dda.VIN,
            //                    dda.CarNo
            //                };
            //    //门店过滤
            //    List<string> brand = new List<string>();
            //    var store = Service.GetStoreNameByUserID(userId, ref brand);//获取门店
            //    if (!string.IsNullOrEmpty(store) && brand.Count <= 0)
            //    {
            //        var storeCode1 = store.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //        query = query.Where(p => storeCode1.Contains(p.StoreCodePackageTrade));
            //    }
            //    if (!string.IsNullOrEmpty(memName)) query = query.Where(p => p.CustomerName.Contains(memName));
            //    if (!string.IsNullOrEmpty(memMobile)) query = query.Where(p => p.CustomerMobile.Contains(memMobile));

            //    if (!string.IsNullOrEmpty(vehicleNo)) query = query.Where(p => p.CarNo.Contains(vehicleNo));
            //    if (startDate != null) query = query.Where(p => p.AddedDate >= startDate);
            //    if (endDate != null) query = query.Where(p => p.AddedDate <= endDate);
            //    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            //}
        }

        public static Result GetPakcgeHistoryByTradeID(long? tid)
        {
            //using (var db = new CRMEntities())
            //{
            //    var query = from r in db.V_M_TM_Mem_TradeDetail_package_packagedetail
            //                where r.TradeID == tid
            //                select r;
            //    return new Result(true, "", new List<object> { query.ToList() });
            //}

            throw new NotImplementedException();
        }
        #endregion

        #region 会员历史金额调整

        /// <summary>
        /// 保存调整历史金额信息
        /// </summary>
        /// <param name="ajustType"></param>
        /// <param name="value"></param>
        /// <param name="memId"></param>
        /// <param name="ajustReason"></param>
        /// <param name="userId"></param>
        /// <returns></returns>

        public static Result SaveAjustAmountInfo(string memId, string ajustType, decimal value, string ajustReason, string userId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                using (db.BeginTransaction())
                {
                    var loyMemExt = db.TM_Loy_MemExt.Where(p => p.MemberID == memId).FirstOrDefault();
                    if (loyMemExt == null)
                    {
                        return new Result(false, "会员不存在");
                    }
                    if (loyMemExt.Dec_Attr_21 == null)
                    {
                        loyMemExt.Dec_Attr_21 = 0.00M;
                    }
                    if (ajustType == "add")
                        loyMemExt.Dec_Attr_21 += value;
                    else
                        loyMemExt.Dec_Attr_21 -= value;
                    TL_Mem_CousumptionAmountAdjust amountAdjust = new TL_Mem_CousumptionAmountAdjust();
                    amountAdjust.AdjustAmount = value;
                    amountAdjust.AdjustReason = ajustReason;
                    amountAdjust.AdjustUser = memId;
                    amountAdjust.AdjustDate = DateTime.Now;
                    amountAdjust.AdjustWay = ajustType;
                    db.TL_Mem_CousumptionAmountAdjust.Add(amountAdjust);
                    db.SaveChanges();
                    MemberSubdivision.ComputeDimension(new List<string> { memId }, DateTime.Now, new List<int> { 1766, 2916 });
                    db.Commit();
                }
            }
            return new Result(true, "添加成功");
        }

        //public static Result GetAjustHistoryAmountData(string memId)
        //{
        //    //   DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        var query = from a in db.TL_Mem_CousumptionAmountAdjust
        //                    where a.AdjustUser == memId
        //                    join b in db.V_S_TM_Mem_Ext on a.AdjustUser equals b.MemberID into bb
        //                    from bcg in bb.DefaultIfEmpty()
        //                    select new
        //                        {
        //                            bcg.MemberCardNo,
        //                            bcg.CustomerMobile,
        //                            bcg.CustomerName,
        //                            bcg.MemberCode,
        //                            a.ID,
        //                            a.AdjustUser,
        //                            a.AdjustAmount,
        //                            a.AdjustDate,
        //                            a.AdjustWay,
        //                            a.AdjustReason

        //                        };

        //        //if (!string.IsNullOrEmpty(cardNo)) query = query.Where(p => p.MemberCardNo.Contains(cardNo));
        //        //if (!string.IsNullOrEmpty(memNo)) query = query.Where(p => p.MemberCode.Contains(memNo));
        //        //if (!string.IsNullOrEmpty(memName)) query = query.Where(p => p.CustomerName.Contains(memName));
        //        //if (!string.IsNullOrEmpty(memMobile)) query = query.Where(p => p.CustomerMobile.Contains(memMobile));
        //        return new Result(true, "", new List<object> { query.ToList() });
        //    }
        //}


        //public static Result GetMemberAmountByMemberId(string memId)
        //{
        //    using (var db = new CRMEntities())
        //    {
        //        var query = from a in db.V_U_TM_Mem_Info
        //                    join b in db.TM_Loy_MemExt on a.MemberID equals b.MemberID into memLoyExt
        //                    from LoyExt in memLoyExt.DefaultIfEmpty()
        //                    join c in db.TD_SYS_BizOption.Where(p => p.OptionType == "CustomerLevel") on a.CustomerLevel equals c.OptionValue into bl
        //                    from bll in bl.DefaultIfEmpty()
        //                    where a.MemberID == memId
        //                    select new
        //                    {
        //                        a.CustomerName,
        //                        a.MemberID,
        //                        CustomerLevel = bll.OptionText,
        //                        a.Gender,
        //                        a.CustomerMobile,
        //                        a.MemberCode,
        //                        HistoryConsumeAmount = LoyExt.Dec_Attr_1,
        //                        TotalAdjustAmount = LoyExt.Dec_Attr_21
        //                    };
        //        return new Result(true, "", query.FirstOrDefault());
        //    }
        //}
        #endregion


        //private static List<AutoComputeTable> mmm(List<ReturnField> r, List<AutoComputeTable> act, CRMEntities db, int Grade, List<TD_SYS_TableMapping> t)
        #region 360视图自动化查询
        public static DataTable Member360AutoSearch(string input, int pageId, string rds)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    if (string.IsNullOrEmpty(input))
                    {
                        return null;
                    }
                    var r = JsonHelper.Deserialize<CommonSearch>(input);
                    List<AutoComputeTable> act = new List<AutoComputeTable>();
                    var tt = db.TD_SYS_TableMapping.Where(p => p.TableName_1 == r.TableName).ToList();
                    // var aa = mmm(r.f, act, db, 1, tt);
                    //if (listMappingBuff == null || (DateTime.Now - lastMappingBuffTime).Hours > 1)
                    //{
                    List<TD_SYS_TableMapping> listMappingBuff = db.TD_SYS_TableMapping.Where(p => p.TableName_1 == r.TableName).ToList();
                    //lastMappingBuffTime = DateTime.Now;
                    //}

                    StringBuilder ruleSql = new StringBuilder();

                    ruleSql.AppendLine("declare @sql nvarchar(max) ");
                    ruleSql.AppendLine(" DECLARE @primaryKey varchar(50)");
                    ruleSql.AppendLine(" select @primaryKey=COLUMN_NAME from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE");
                    ruleSql.AppendLine(" WHERE CONSTRAINT_NAME=(select CONSTRAINT_NAME from INFORMATION_SCHEMA.TABLE_CONSTRAINTS");
                    ruleSql.AppendLine(" WHERE CONSTRAINT_TYPE='PRIMARY KEY' AND TABLE_NAME='" + r.TableName + "')");

                    ruleSql.AppendLine("set @sql = ' ");
                    ruleSql.AppendLine(" select " + r.TableName + ".'+@primaryKey+ ' as N''主键'',");
                    for (int i = 0; i < r.f.Count; i++)
                    {
                        var f = r.f[i];
                        if (f.TableName == "TD_SYS_BizOption")
                        {
                            f.TableName += i.ToString();
                            ruleSql.AppendLine(f.TableName + ".OptionText" + "  as N''" + f.ColumnName + "'',");
                        }
                        else if (f.TableName == "TD_Sys_VehicleBasicInfo")
                        {
                            f.TableName += i.ToString();
                            ruleSql.AppendLine(f.TableName + ".Name" + "  as N''" + f.ColumnName + "'',");
                        }
                        else
                        {
                            ruleSql.AppendLine(f.TableName + "." + f.FieldAlias + "  as N''" + f.ColumnName + "'',");
                        }
                    }
                    ruleSql.Remove(ruleSql.ToString().LastIndexOf(','), 1);
                    ruleSql.AppendLine(" from " + r.TableName);

                    //权限过滤
                    if (rds != "")
                    {
                        List<string> rid = JsonHelper.Deserialize<List<string>>(rds);
                        StringBuilder limitSql = new StringBuilder();
                        limitSql.AppendLine("select distinct MemberID from TR_Mem_MemberLimit ");
                        var limlst = db.TM_AUTH_DataLimit.Where(p => p.HierarchyType == "role" && rid.Contains(p.HierarchyValue)
                                     && (p.PageID == 9999 || p.PageID == pageId)).ToList();
                        if (limlst.Count > 0)
                        {
                            string limwhere = "";
                            foreach (var item in limlst)
                            {
                                if (limwhere == "")
                                    limwhere = "where " + item.RangeType + " = ''" + item.RangeValue + "''";
                                else
                                    limwhere += " or " + item.RangeType + " = ''" + item.RangeValue + "''";
                            }
                            limitSql.AppendLine(limwhere);
                            ruleSql.AppendLine(" inner join (" + limitSql + ")l on " + r.TableName + ".MemberID = l.MemberID");
                        }
                    }

                    List<TableMapping> tlst = new List<TableMapping>();
                    //查询需要关联的表
                    foreach (var f in r.f)
                    {
                        var tn = "";
                        if (f.TableName.Contains("TD_SYS_BizOption"))
                            tn = "TD_SYS_BizOption";
                        else if (f.TableName.Contains("TD_Sys_VehicleBasicInfo"))
                            tn = "TD_Sys_VehicleBasicInfo";
                        else
                            tn = f.TableName;

                        if (tlst.Where(p => p.TableName == f.TableName).FirstOrDefault() == null && f.TableName != r.TableName)
                        {
                            var item = listMappingBuff.Where(p => p.TableName_1 == r.TableName && p.TableName_2 == tn).FirstOrDefault();
                            if (item != null)
                            {
                                TableMapping tm = new TableMapping();
                                tm.TableName = f.TableName;
                                tm.RelationAlias_1 = item.RelationAlias_1;
                                tm.RelationAlias_2 = item.RelationAlias_2;
                                listMappingBuff.Remove(item);
                                tlst.Add(tm);
                            }
                        }
                    }
                    StringBuilder whereSql = new StringBuilder();
                    whereSql.AppendLine("where 1=1  ");
                    if (r.TableName == "TM_Mem_Trade")
                        whereSql.AppendLine(" and TM_Mem_Trade.TradeType=''" + r.AliasKey + "''  ");
                    if (r.TableName == "TM_Mem_SubExt" && !string.IsNullOrEmpty(r.AliasKey))
                    {
                        whereSql.AppendLine(" and " + r.TableName + ".ExtType=''" + r.AliasKey + "'' ");
                    }
                    if (r.TableName == "TM_Mem_TradeDetail")
                    {
                        whereSql.AppendLine(" and " + r.TableName + ".TradeType=''" + r.AliasKey + "'' and " + r.TableName + ".TradeDetailType=''" + r.AliasSubKey + "''");
                    }


                    //拼接join
                    foreach (var t in tlst)
                    {
                        if (t.TableName.Contains("TD_SYS_BizOption"))
                        {
                            ruleSql.AppendLine("left join TD_SYS_BizOption as " + t.TableName + " on " + t.TableName + ".OptionValue = " + r.TableName + "." + t.RelationAlias_1);
                            whereSql.AppendLine(" and " + t.TableName + ".OptionType = " + "''" + t.RelationAlias_2 + "''");

                        }
                        else if (t.TableName.Contains("TD_Sys_VehicleBasicInfo"))
                        {
                            ruleSql.AppendLine("left join TD_Sys_VehicleBasicInfo as " + t.TableName + " on " + t.TableName + ".Code = " + r.TableName + "." + t.RelationAlias_1);
                            //whereSql.AppendLine(" and " + t.TableName + ".Code = " + "''" + t.RelationAlias_2 + "''");
                        }
                        else
                        {
                            ruleSql.AppendLine("left join " + t.TableName + " on " + t.TableName + "." + t.RelationAlias_2 + " = " + r.TableName + "." + t.RelationAlias_1);
                        }
                    }





                    //查询条件
                    foreach (var s in r.s)
                    {
                        string ep = s.TableName + "." + s.FieldAlias;
                        var v = "''" + s.Value + "''";
                        switch (s.Condition)
                        {
                            case "=":
                                ep += " = " + v;
                                break;
                            case "<>":
                                ep += " <> " + v;
                                break;
                            case ">":
                                ep += " > " + v; ;
                                break;
                            case "<":
                                ep += " < " + v;
                                break;
                            case ">=":
                                ep += " >= " + v;
                                break;
                            case "<=":
                                ep += " <= " + v;
                                break;
                            case "like":
                                ep += " like ''%" + s.Value + "%''";
                                break;
                            case "notlike":
                                ep += " not like ''%" + s.Value + "%''";
                                break;
                            case "likebegin":
                                ep += " like " + s.Value + "%''";
                                break;
                            case "likeend":
                                ep += " like ''%" + s.Value;
                                break;
                            default:
                                ep += " = " + v;
                                break;
                        }
                        whereSql.AppendLine("and  " + ep);
                    }

                    ruleSql.AppendLine(whereSql.ToString());
                    ruleSql.AppendLine("' ");
                    ruleSql.AppendLine("exec(@sql)");

                    var cmd = db.Database.Connection.CreateCommand();
                    cmd.CommandTimeout = 600;
                    cmd.CommandText = ruleSql.ToString();
                    cmd.CommandType = System.Data.CommandType.Text;




                    //temp log
                    Log4netHelper.WriteInfoLog("member360sql:" + cmd.CommandText);

                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed) db.Database.Connection.Open();
                    var rt = cmd.ExecuteReader();

                    //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    //DataTable table = new DataTable();
                    //adapter.Fill(table);

                    DataTable dt = new DataTable();
                    dt.Columns.Add("主键", System.Type.GetType("System.String"));
                    foreach (var f in r.f)
                    {
                        dt.Columns.Add(f.ColumnName, System.Type.GetType("System.String"));
                    }

                    while (rt.Read())
                    {
                        DataRow dr = dt.NewRow();
                        dr["主键"] = rt["主键"];
                        foreach (var f in r.f)
                        {
                            dr[f.ColumnName] = rt[f.ColumnName];
                        }
                        dt.Rows.Add(dr);
                    }

                    return dt;

                    //return new Result(true, "", dt);
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("Member360AutoSearch：" + ex.ToString());
                return null;
            }


        }

        //查询语句转换（Join）
        private static List<AutoComputeTable> mmm(List<ReturnField> r, List<AutoComputeTable> act, CRMEntities db, int Grade, List<TD_SYS_TableMapping> t)
        {
            //主表处理
            if (Grade == 1)
            {
                for (int i = 0; i < r.Count; i++)
                {
                    if (t[0].TableName_1 == r[i].TableName)
                    {
                        var m = new AutoComputeTable();
                        m.TableAlias = "Extent";
                        m.TableName = r[i].TableName;
                        m.FieldAlias = r[i].FieldAlias;
                        m.ColumnName = r[i].ColumnName;
                        m.Grade = Grade;
                        act.Add(m);
                        r[i].flag = true;
                    }
                }
                Grade += 1;
                var tn = t[0].TableName_1;
                var ts = db.TD_SYS_TableMapping.Where(p => p.TableName_1 == tn).ToList();
                if (ts.Count == 0)
                    return act;
                else
                    return mmm(r.Where(p => p.flag != true).ToList(), act, db, Grade, ts);
            }
            //子表处理
            else
            {
                for (int i = 0; i < r.Count; i++)
                {
                    //t:带处理的子表名称集合，r:带处理的返回值集合
                    var tnl = t.Where(p => p.TableName_2 == r[i].TableName).ToList();
                    var tn1 = tnl.Where(p => r[i].FieldAlias == p.RelationAlias_1 && p.TableName_2 == "TD_SYS_BizOption").FirstOrDefault();
                    var tn2 = tnl.Where(p => r[i].FieldAlias2 == p.RelationAlias_2 && p.TableName_2 != "TD_SYS_BizOption").FirstOrDefault();
                    //TD_SYS_BizOption处理逻辑
                    if (tn1 != null)
                    {
                        string ex = "ExtentA" + Grade.ToString() + "B" + i.ToString();
                        var m = new AutoComputeTable();
                        m.TableName = r[i].TableName;
                        m.TableAlias = ex;
                        m.TableName_S = tn1.TableName_1;
                        m.TableAlias_S = act.Where(p => p.Grade == (Grade - 1) && p.TableName == tn1.TableName_1).FirstOrDefault().TableAlias;
                        m.FieldAlias = r[i].FieldAlias;
                        m.ColumnName = r[i].ColumnName;
                        m.RelationAlias_1 = tn1.RelationAlias_1;
                        m.RelationAlias_2 = tn1.RelationAlias_2;
                        act.Add(m);
                        r[i].flag = true;
                    }
                    if (tn2 != null)
                    {
                        string ex = "ExtentA" + Grade.ToString() + "B" + i.ToString();
                        var m = new AutoComputeTable();
                        m.TableName = r[i].TableName;
                        m.TableAlias = ex;
                        m.TableName_S = tn2.TableName_1;
                        m.TableAlias_S = act.Where(p => p.Grade == (Grade - 1) && p.TableName == tn2.TableName_1).FirstOrDefault().TableAlias;
                        m.FieldAlias = r[i].FieldAlias;
                        m.ColumnName = r[i].ColumnName;
                        m.RelationAlias_1 = tn2.RelationAlias_1;
                        m.RelationAlias_2 = tn2.RelationAlias_2;
                        act.Add(m);
                        r[i].flag = true;
                    }
                }
                Grade += 1;
                List<string> tl = t.Select(o => o.TableName_2).ToList();
                var ts = db.TD_SYS_TableMapping.Where(p => tl.Contains(p.TableName_1)).ToList();
                if (ts.Count == 0)
                    return act;
                else
                    return mmm(r.Where(p => p.flag != true).ToList(), act, db, Grade, ts);
            }
        }


        public static DataTable Member360AutoSearch_bk(string input, int pageId, string rds)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    if (string.IsNullOrEmpty(input))
                    {
                        return null;
                    }
                    var r = JsonHelper.Deserialize<CommonSearch>(input);


                    //if (listMappingBuff == null || (DateTime.Now - lastMappingBuffTime).Hours > 1)
                    //{
                    List<TD_SYS_TableMapping> listMappingBuff = db.TD_SYS_TableMapping.Where(p => p.TableName_1 == r.TableName).ToList();
                    //lastMappingBuffTime = DateTime.Now;
                    //}

                    StringBuilder ruleSql = new StringBuilder();

                    ruleSql.AppendLine("declare @sql nvarchar(max) ");
                    ruleSql.AppendLine(" DECLARE @primaryKey varchar(50)");
                    ruleSql.AppendLine(" select @primaryKey=COLUMN_NAME from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE");
                    ruleSql.AppendLine(" WHERE CONSTRAINT_NAME=(select CONSTRAINT_NAME from INFORMATION_SCHEMA.TABLE_CONSTRAINTS");
                    ruleSql.AppendLine(" WHERE CONSTRAINT_TYPE='PRIMARY KEY' AND TABLE_NAME='" + r.TableName + "')");

                    ruleSql.AppendLine("set @sql = ' ");
                    ruleSql.AppendLine(" select " + r.TableName + ".'+@primaryKey+ ' as N''主键'',");
                    for (int i = 0; i < r.f.Count; i++)
                    {
                        var f = r.f[i];
                        if (f.TableName == "TD_SYS_BizOption")
                        {
                            f.TableName += i.ToString();
                            ruleSql.AppendLine(f.TableName + ".OptionText" + "  as N''" + f.ColumnName + "'',");
                        }
                        else if (f.TableName == "TD_Sys_VehicleBasicInfo")
                        {
                            f.TableName += i.ToString();
                            ruleSql.AppendLine(f.TableName + ".Name" + "  as N''" + f.ColumnName + "'',");
                        }
                        else
                        {
                            ruleSql.AppendLine(f.TableName + "." + f.FieldAlias + "  as N''" + f.ColumnName + "'',");
                        }
                    }
                    ruleSql.Remove(ruleSql.ToString().LastIndexOf(','), 1);
                    ruleSql.AppendLine(" from " + r.TableName);

                    //权限过滤
                    if (rds != "")
                    {
                        List<string> rid = JsonHelper.Deserialize<List<string>>(rds);
                        StringBuilder limitSql = new StringBuilder();
                        limitSql.AppendLine("select distinct MemberID from TR_Mem_MemberLimit ");
                        var limlst = db.TM_AUTH_DataLimit.Where(p => p.HierarchyType == "role" && rid.Contains(p.HierarchyValue)
                                     && (p.PageID == 9999 || p.PageID == pageId)).ToList();
                        if (limlst.Count > 0)
                        {
                            string limwhere = "";
                            foreach (var item in limlst)
                            {
                                if (limwhere == "")
                                    limwhere = "where " + item.RangeType + " = ''" + item.RangeValue + "''";
                                else
                                    limwhere += " or " + item.RangeType + " = ''" + item.RangeValue + "''";
                            }
                            limitSql.AppendLine(limwhere);
                            ruleSql.AppendLine(" inner join (" + limitSql + ")l on " + r.TableName + ".MemberID = l.MemberID");
                        }
                    }

                    List<TableMapping> tlst = new List<TableMapping>();
                    //查询需要关联的表
                    foreach (var f in r.f)
                    {
                        var tn = "";
                        if (f.TableName.Contains("TD_SYS_BizOption"))
                            tn = "TD_SYS_BizOption";
                        else if (f.TableName.Contains("TD_Sys_VehicleBasicInfo"))
                            tn = "TD_Sys_VehicleBasicInfo";
                        else
                            tn = f.TableName;

                        if (tlst.Where(p => p.TableName == f.TableName).FirstOrDefault() == null && f.TableName != r.TableName)
                        {
                            var item = listMappingBuff.Where(p => p.TableName_1 == r.TableName && p.TableName_2 == tn).FirstOrDefault();
                            if (item != null)
                            {
                                TableMapping tm = new TableMapping();
                                tm.TableName = f.TableName;
                                tm.RelationAlias_1 = item.RelationAlias_1;
                                tm.RelationAlias_2 = item.RelationAlias_2;
                                listMappingBuff.Remove(item);
                                tlst.Add(tm);
                            }
                        }
                    }
                    StringBuilder whereSql = new StringBuilder();
                    whereSql.AppendLine("where 1=1  ");
                    if (r.TableName == "TM_Mem_Trade")
                        whereSql.AppendLine(" and TM_Mem_Trade.TradeType=''sales''  ");
                    if (r.TableName == "TM_Mem_SubExt" && !string.IsNullOrEmpty(r.AliasKey))
                    {
                        whereSql.AppendLine(" and " + r.TableName + ".ExtType=''" + r.AliasKey + "'' ");
                    }
                    if (r.TableName == "TM_Mem_TradeDetail")
                    {
                        whereSql.AppendLine(" and " + r.TableName + ".TradeType=''" + r.AliasKey + "'' and " + r.TableName + ".TradeDetailType=''" + r.AliasSubKey + "''");
                    }
                    //拼接join
                    foreach (var t in tlst)
                    {
                        if (t.TableName.Contains("TD_SYS_BizOption"))
                        {
                            ruleSql.AppendLine("left join TD_SYS_BizOption as " + t.TableName + " on " + t.TableName + ".OptionValue = " + r.TableName + "." + t.RelationAlias_1);
                            whereSql.AppendLine(" and " + t.TableName + ".OptionType = " + "''" + t.RelationAlias_2 + "''");

                        }
                        else if (t.TableName.Contains("TD_Sys_VehicleBasicInfo"))
                        {
                            ruleSql.AppendLine("left join TD_Sys_VehicleBasicInfo as " + t.TableName + " on " + t.TableName + ".Code = " + r.TableName + "." + t.RelationAlias_1);
                            //whereSql.AppendLine(" and " + t.TableName + ".Code = " + "''" + t.RelationAlias_2 + "''");
                        }
                        else
                        {
                            ruleSql.AppendLine("left join " + t.TableName + " on " + t.TableName + "." + t.RelationAlias_2 + " = " + r.TableName + "." + t.RelationAlias_1);
                        }
                    }





                    //查询条件
                    foreach (var s in r.s)
                    {
                        string ep = s.TableName + "." + s.FieldAlias;
                        var v = "''" + s.Value + "''";
                        switch (s.Condition)
                        {
                            case "=":
                                ep += " = " + v;
                                break;
                            case "<>":
                                ep += " <> " + v;
                                break;
                            case ">":
                                ep += " > " + v; ;
                                break;
                            case "<":
                                ep += " < " + v;
                                break;
                            case ">=":
                                ep += " >= " + v;
                                break;
                            case "<=":
                                ep += " <= " + v;
                                break;
                            case "like":
                                ep += " like ''%" + s.Value + "%''";
                                break;
                            case "notlike":
                                ep += " not like ''%" + s.Value + "%''";
                                break;
                            case "likebegin":
                                ep += " like " + s.Value + "%''";
                                break;
                            case "likeend":
                                ep += " like ''%" + s.Value;
                                break;
                            default:
                                ep += " = " + v;
                                break;
                        }
                        whereSql.AppendLine("and  " + ep);
                    }

                    ruleSql.AppendLine(whereSql.ToString());
                    ruleSql.AppendLine("' ");
                    ruleSql.AppendLine("exec(@sql)");

                    var cmd = db.Database.Connection.CreateCommand();
                    cmd.CommandTimeout = 600;
                    cmd.CommandText = ruleSql.ToString();
                    cmd.CommandType = System.Data.CommandType.Text;
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed) db.Database.Connection.Open();
                    var rt = cmd.ExecuteReader();

                    //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    //DataTable table = new DataTable();
                    //adapter.Fill(table);

                    DataTable dt = new DataTable();
                    dt.Columns.Add("主键", System.Type.GetType("System.String"));
                    foreach (var f in r.f)
                    {
                        dt.Columns.Add(f.ColumnName, System.Type.GetType("System.String"));
                    }

                    while (rt.Read())
                    {
                        DataRow dr = dt.NewRow();
                        dr["主键"] = rt["主键"];
                        foreach (var f in r.f)
                        {
                            dr[f.ColumnName] = rt[f.ColumnName];
                        }
                        dt.Rows.Add(dr);
                    }

                    return dt;

                    //return new Result(true, "", dt);
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("Member360AutoSearch：" + ex.ToString());
                return null;
            }


        }
        //获取数据
        public static Result DtToDatatable(int pid, string rds, string tablename, int pageid, string blocktype, string dp, params object[] strlist)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            var input = TransInput(tablename, pageid, blocktype, strlist);
            DataTable dt = Member360AutoSearch(input, pid, rds);
            //dt转化为datatable使用
            if (dt != null)
            {
                List<object> headerInfo = new List<object>();
                foreach (DataColumn c in dt.Columns)
                {
                    headerInfo.Add(new { title = c.ColumnName, @class = "center" });
                }

                List<object> data;
                List<object> dataList = new List<object>();
                string link = string.Empty;
                foreach (DataRow drr in dt.Rows)
                {
                    data = new List<object>();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        data.Add(drr[i]);
                    }
                    dataList.Add(data);
                }

                var res = new DatatablesSourceVsPage();
                res.iDisplayStart = myDp.iDisplayStart;
                res.iDisplayLength = myDp.iDisplayLength;
                res.iTotalRecords = dt.Rows.Count;
                res.aaData = dataList.Skip(myDp.iDisplayStart).Take(myDp.iDisplayLength).ToList();
                res.header = headerInfo;
                return new Result(true, "", new List<object> { res });
            }
            else
            {
                return new Result(true, "", null);
            }
        }
        //获取表头
        public static Result DtToColumn(string tablename, int pageid, string blocktype, params object[] strlist)
        {
            var input = TransInput(tablename, pageid, blocktype, strlist);
            var r = JsonHelper.Deserialize<CommonSearch>(input);
            //DataTable dt = Member360AutoSearch(input);
            //dt转化为datatable使用
            if (r.f.Count > 0)
            {
                List<object> headerInfo = new List<object>();

                headerInfo.Add(new { title = "主键", @class = "center hide" });
                foreach (var c in r.f)
                {
                    headerInfo.Add(new { title = c.ColumnName, @class = "center" });
                }

                return new Result(true, "", new List<object> { headerInfo });
            }
            else
            {
                return new Result(true, "", null);
            }
        }

        //获取360导出表头
        public static Result Export360Column(string tablename, int pageid, string blocktype, params object[] strlist)
        {
            var input = TransInput(tablename, pageid, blocktype, strlist);
            var r = JsonHelper.Deserialize<CommonSearch>(input);
            //DataTable dt = Member360AutoSearch(input);
            //dt转化为datatable使用
            if (r.f.Count > 0)
            {
                List<object> headerInfo = new List<object>();


                foreach (var c in r.f)
                {
                    headerInfo.Add(new { Name = c.ColumnName, Code = c.Alias });
                }

                return new Result(true, "", new List<object> { headerInfo });
            }
            else
            {
                return new Result(true, "", null);
            }
        }


        //把搜索条件转化为json字符串
        public static string TransInput(string tablename, int pageid, string blocktype, params object[] strlist)
        {
            List<CommonSearchDetail> detaillist = new List<CommonSearchDetail>();
            List<ReturnField> returnlist = new List<ReturnField>();
            CommonSearchDetail detail;
            ReturnField ret;
            using (var db = new CRMEntities())
            {
                var conlist = db.TD_SYS_BizOption.Where(p => p.OptionType == "NumOperator").ToString();
                //var blocklist= db.TM_SYS_BlockSetting.Where(p => p.PageID == pageid && (string.IsNullOrEmpty(blocktype) ? true : p.BlockType == blocktype)).OrderBy(p => p.Sort).ToList();
                var blocklist = (from a in db.TM_SYS_BlockSetting.Where(p => p.PageID == pageid && p.BlockCode == "Search_Info" && (string.IsNullOrEmpty(blocktype) ? true : p.BlockType == blocktype))
                                 join b in db.TD_SYS_FieldAlias on a.FieldID equals b.AliasID
                                 select new
                                 {
                                     a.FieldID,
                                     a.PageID,
                                     a.BlockCode,
                                     a.BlockType,
                                     a.Sort,
                                     b.TableName,
                                     b.FieldName,
                                     b.FieldAlias,
                                     b.ControlType,
                                     b.ViewName,
                                 }).OrderBy(p => p.Sort).ToList();
                if (strlist.Length > 0)
                {
                    for (int i = 0; i < blocklist.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(strlist[i].ToString()))
                        {
                            detail = new CommonSearchDetail()
                            {
                                FieldAlias = blocklist[i].FieldName,
                                Value = strlist[i].ToString(),
                                TableName = blocklist[i].TableName,
                                Condition = GetCondition(blocklist[i].ControlType),


                            };
                            detaillist.Add(detail);
                        }
                    }
                }
                var retlist = (from a in db.TM_SYS_BlockSetting.Where(p => p.PageID == pageid && p.BlockCode == "Return" && (string.IsNullOrEmpty(blocktype) ? true : p.BlockType == blocktype))
                               join bba in db.TD_SYS_FieldAlias on a.FieldID equals bba.AliasID into bb
                               from b in bb.DefaultIfEmpty()
                               select new
                               {
                                   a.FieldID,
                                   a.PageID,
                                   a.BlockCode,
                                   a.BlockType,
                                   a.Sort,
                                   a.DisplayName,
                                   b.TableName,
                                   b.FieldName,
                                   b.FieldAlias,
                                   b.FieldDesc,
                                   b.ControlType,
                                   b.ViewName,
                                   b.DictTableName,
                                   b.DictTableType
                               }).OrderBy(p => p.Sort).ToList();
                //TODO:和会员相关的表有MemberID

                for (int i = 0; i < retlist.Count; i++)
                {

                    if (retlist[i].DictTableName == "TD_SYS_BizOption")
                    {
                        ret = new ReturnField()
                        {
                            FieldAlias = retlist[i].FieldName,
                            TableName = "TD_SYS_BizOption",
                            ColumnName = retlist[i].FieldDesc,
                            IsSource = "1",
                            Alias = retlist[i].FieldAlias,
                        };
                    }
                    else if (!string.IsNullOrEmpty(retlist[i].DictTableName))
                    {
                        var code = retlist[i].DictTableType.Split(',')[1];
                        ret = new ReturnField()
                        {
                            FieldAlias = code,
                            TableName = retlist[i].DictTableName,
                            ColumnName = retlist[i].FieldDesc,
                            IsSource = "1",
                            Alias = retlist[i].FieldAlias,
                        };
                    }
                    else if (retlist[i].FieldID == -1)
                    {
                        ret = new ReturnField()
                        {
                            FieldAlias = "AddedDate",
                            TableName = tablename,
                            ColumnName = retlist[i].DisplayName,
                            IsSource = "1",
                            Alias = retlist[i].FieldAlias,
                        };
                    }
                    else if (retlist[i].FieldID == -2)
                    {
                        ret = new ReturnField()
                        {
                            FieldAlias = "AddedUser",
                            TableName = tablename,
                            ColumnName = retlist[i].DisplayName,
                            IsSource = "1",
                            Alias = retlist[i].FieldAlias,
                        };
                    }
                    else
                    {
                        ret = new ReturnField()
                        {
                            FieldAlias = retlist[i].FieldName,
                            TableName = retlist[i].TableName,
                            ColumnName = retlist[i].FieldDesc,
                            IsSource = "1",
                            Alias = retlist[i].FieldAlias,
                        };
                    }

                    returnlist.Add(ret);
                }

            }

            CommonSearch search = new CommonSearch() { TableName = tablename, s = detaillist, f = returnlist };

            return JsonHelper.Serialize(search);
        }


        //获取数据
        public static Result DtToDatatableNew(int pid, string rds, string tablename, string aliaskey, string aliassubkey, int pageid, string blocktype, string dp, string con)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            var input = TransInputNew(tablename, aliaskey, aliassubkey, pageid, blocktype, con);
            DataTable dt = Member360AutoSearch(input, pid, rds);
            //dt转化为datatable使用
            if (dt != null)
            {
                DataTable dt2 = new DataTable();

                List<object> headerInfo = new List<object>();
                foreach (DataColumn c in dt.Columns)
                {
                    headerInfo.Add(new { title = c.ColumnName, @class = "center" });
                }
                if (myDp.iSortingCols > 0)
                {
                    //排序
                    DataView dv = dt.DefaultView;
                    dv.Sort = dt.Columns[Convert.ToInt32(myDp.SortCols[0])].ColumnName + " " + myDp.SortDirs[0];
                    dt2 = dv.ToTable();
                }
                else
                {
                    dt2 = dt;
                }

                List<object> data;
                List<object> dataList = new List<object>();
                string link = string.Empty;
                foreach (DataRow drr in dt2.Rows)
                {
                    data = new List<object>();
                    for (int i = 0; i < dt2.Columns.Count; i++)
                    {
                        data.Add(drr[i]);
                    }
                    dataList.Add(data);
                }

                var res = new DatatablesSourceVsPage();
                res.iDisplayStart = myDp.iDisplayStart;
                res.iDisplayLength = myDp.iDisplayLength;
                res.iTotalRecords = dt.Rows.Count;
                res.aaData = dataList.Skip(myDp.iDisplayStart).Take(myDp.iDisplayLength).ToList();
                res.header = headerInfo;
                return new Result(true, "", new List<object> { res });
            }
            else
            {
                return new Result(true, "", null);
            }
        }
        //把搜索条件转化为json字符串
        public static string TransInputNew(string tablename, string aliaskey, string aliassubkey, int pageid, string blocktype, string con)
        {
            List<CommonSearchDetail> detaillist = JsonHelper.Deserialize<List<CommonSearchDetail>>(con);
            List<ReturnField> returnlist = new List<ReturnField>();
            ReturnField ret;
            using (var db = new CRMEntities())
            {
                var retlist = (from a in db.TM_SYS_BlockSetting.Where(p => p.PageID == pageid && p.BlockCode == "Return" && (string.IsNullOrEmpty(blocktype) ? true : p.BlockType == blocktype))
                               join bba in db.TD_SYS_FieldAlias on a.FieldID equals bba.AliasID into bb
                               from b in bb.DefaultIfEmpty()
                               select new
                               {
                                   a.FieldID,
                                   a.PageID,
                                   a.BlockCode,
                                   a.BlockType,
                                   a.Sort,
                                   a.DisplayName,
                                   b.TableName,
                                   b.AliasKey,
                                   b.FieldName,
                                   b.FieldAlias,
                                   b.FieldDesc,
                                   b.ControlType,
                                   b.ViewName,
                                   b.DictTableName,
                                   b.DictTableType
                               }).OrderBy(p => p.Sort).ToList();

                //TODO:和会员相关的表有MemberID
                //ret = new ReturnField()
                //{
                //    FieldAlias = "MemberID",
                //    TableName = tablename,
                //    ColumnName = "会员主键",
                //    IsSource = "1"
                //};
                //returnlist.Add(ret);
                for (int i = 0; i < retlist.Count; i++)
                {

                    if (retlist[i].DictTableName == "TD_SYS_BizOption")
                    {
                        ret = new ReturnField()
                        {
                            FieldAlias = retlist[i].FieldName,
                            FieldAlias2 = retlist[i].DictTableType,
                            TableName = "TD_SYS_BizOption",
                            ColumnName = retlist[i].FieldDesc,
                            IsSource = "1"
                        };
                    }
                    else if (!string.IsNullOrEmpty(retlist[i].DictTableName))
                    {
                        var code1 = retlist[i].DictTableType.Split(',')[0];
                        var code = retlist[i].DictTableType.Split(',')[1];
                        ret = new ReturnField()
                        {
                            FieldAlias = code,
                            FieldAlias2 = code1,
                            TableName = retlist[i].DictTableName,
                            ColumnName = retlist[i].FieldDesc,
                            IsSource = "1"
                        };
                    }
                    else if (retlist[i].FieldID == -1)
                    {
                        ret = new ReturnField()
                        {
                            FieldAlias = "AddedDate",
                            TableName = tablename,
                            ColumnName = retlist[i].DisplayName,
                            IsSource = "1"
                        };
                    }
                    else if (retlist[i].FieldID == -2)
                    {
                        ret = new ReturnField()
                        {
                            FieldAlias = "AddedUser",
                            TableName = tablename,
                            ColumnName = retlist[i].DisplayName,
                            IsSource = "1"
                        };
                    }
                    else
                    {
                        ret = new ReturnField()
                        {
                            FieldAlias = retlist[i].FieldName,
                            TableName = retlist[i].TableName,
                            ColumnName = retlist[i].FieldDesc,
                            IsSource = "1"
                        };
                    }
                    returnlist.Add(ret);
                }

            }

            CommonSearch search = new CommonSearch() { TableName = tablename, AliasKey = aliaskey, AliasSubKey = aliassubkey, s = detaillist, f = returnlist };

            return JsonHelper.Serialize(search);
        }


        public static string GetCondition(string option)
        {
            string con = "like";
            switch (option)
            {
                case "input":
                    con = "like";
                    break;
                case "select":
                    con = "=";
                    break;
                case "date":
                    con = ">=";
                    break;
                default:
                    con = "like";
                    break;
            }
            return con;
        }
        #endregion


        #region 入会
        public static Result AddMemberData(string mem, string userid)
        {

            using (CRMEntities db = new CRMEntities())
            {

                MemberInfo meminfo = JsonHelper.Deserialize<MemberInfo>(mem);
                ObjectParameter cardN = new ObjectParameter("memberCardNo", typeof(int));
                var res = db.sp_CRM_ECMemberCardCreate(cardN);
                meminfo.CustomerNo = cardN.Value.ToString();
                string mid = Guid.NewGuid().ToString("N");
                string token = ConfigurationSettings.AppSettings["token"].ToString();
                var pass = ToolsHelper.MD5(meminfo.Mobile.Substring(5, 6) + token);

                var member1 = db.V_U_TM_Mem_Info.Where(p => p.Mobile == meminfo.Mobile).FirstOrDefault();
                if (member1 != null) { return new Result(false, "手机号码已存在"); }

                var member2 = db.V_U_TM_Mem_Info.Where(p => p.MemberCardNo == meminfo.CustomerNo).FirstOrDefault();
                if (member2 != null) { return new Result(false, "会员卡号已存在"); }

                var store = db.V_M_TM_SYS_BaseData_store.Where(p => p.StoreCode == meminfo.StoreCode).FirstOrDefault();
                if (store == null) { return new Result(false, "门店不存在,请联系系统管理员"); }
                db.BeginTransaction();

                //master  ext  account loy_MemExt
                TM_Mem_Ext ext = new TM_Mem_Ext()
                {
                    MemberID = mid,
                    Str_Attr_1 = "1",//会员状态
                    Str_Attr_2 = meminfo.CustomerNo,//会员卡号
                    Str_Attr_3 = meminfo.MemName,//会员姓名
                    Str_Attr_4 = meminfo.Mobile,//手机号
                    Str_Attr_7 = meminfo.Gender,//性别
                    Str_Attr_8 = meminfo.CertType,//证件类型
                    Str_Attr_9 = meminfo.CertNo,//证件号
                    Str_Attr_11 = meminfo.Province,//省
                    Str_Attr_12 = meminfo.City,//市
                    Str_Attr_13 = meminfo.District,//区
                    Str_Attr_18 = meminfo.Address,//地址
                    Str_Attr_15 = meminfo.MemType,//会员类型
                    Str_Attr_5 = meminfo.StoreCode,//注册门店
                    Str_Attr_20 = meminfo.ProvinceCode,//省代码
                    Str_Attr_21 = meminfo.CityCode,//市代码
                    Str_Attr_34 = "0", //是否老会员
                    Str_Attr_22 = meminfo.DistrictCode,//区代码
                    Str_Attr_40 = meminfo.Corp,
                    Str_Attr_41 = meminfo.PosNo,
                    Date_Attr_2 = string.IsNullOrEmpty(meminfo.Birthday) ? null : (DateTime?)Convert.ToDateTime(meminfo.Birthday),//生日
                    Str_Attr_52 = meminfo.Corp,
                    //Str_Attr_52 = db.V_M_TM_SYS_BaseData_store.Where(i => i.StoreCode == meminfo.StoreCode).Select(j => j.ChannelCodeStore).FirstOrDefault(),
                    Str_Attr_53 = meminfo.StoreCode,
                    Str_Attr_24 = meminfo.MakeupFrequency,
                    Str_Attr_25 = meminfo.SkinType,
                    Str_Attr_54 = meminfo.SkinCareType,
                    Str_Attr_55 = meminfo.MarriageStatus,
                    Str_Attr_56 = meminfo.Job,
                    Str_Attr_57 = meminfo.MonthlyIncome,
                    Str_Attr_58 = meminfo.KnowingFrom,
                    Str_Attr_59 = meminfo.NeededMessage,



                    AddedDate = DateTime.Now,
                    AddedUser = userid,
                    ModifiedDate = DateTime.Now,
                    ModifiedUser = userid,
                };
                db.TM_Mem_Ext.Add(ext);

                TM_Mem_Master master = new TM_Mem_Master()
                {
                    MemberID = mid,
                    MemberGrade = 1,
                    MemberLevel = "001",
                    DataGroupID = 1,
                    Str_Key_1 = meminfo.Mobile,
                    Str_Key_3 = pass,
                    AddedDate = DateTime.Now,
                    AddedUser = userid,
                    ModifiedDate = DateTime.Now,
                    ModifiedUser = userid,
                };
                db.TM_Mem_Master.Add(master);

                //会员卡表
                TM_Mem_Card c = new TM_Mem_Card();
                c.CardNo = meminfo.CustomerNo;
                c.CardType = "2";
                c.MemberID = mid;
                c.Active = true;
                c.Lock = false;
                c.AddedDate = DateTime.Now;
                c.AddedUser = userid;
                c.Status = 3;
                db.TM_Mem_Card.Add(c);

                var gid = Guid.NewGuid().ToString("N");
                var gid_acountDetail = Guid.NewGuid().ToString("N");

                TM_Mem_Account account = new TM_Mem_Account();
                account.AccountID = gid;
                account.MemberID = mid;
                account.AccountType = "3";
                account.Value1 = 0;
                account.Value2 = 0;
                account.Value3 = 0;
                account.NoBackAccount = 0;
                account.NoInvoiceAccount = 0;
                account.AddedDate = DateTime.Now;
                account.AddedUser = userid;
                account.ModifiedDate = DateTime.Now;
                account.ModifiedUser = userid;
                db.TM_Mem_Account.Add(account);

                db.SaveChanges();

                var result = MemberAuthStore(meminfo.StoreCode, userid, mid, db);
                TM_Loy_MemExt memext = new TM_Loy_MemExt()
                {
                    MemberID = mid,
                    AddedDate = DateTime.Now,
                    AddedUser = userid,
                    ModifiedDate = DateTime.Now,
                    ModifiedUser = userid,
                    Dec_Attr_23 = meminfo.TotalGold,
                };
                db.TM_Loy_MemExt.Add(memext);


                TM_Sys_SMSSendingQueue sms = new TM_Sys_SMSSendingQueue();
                sms.Mobile = meminfo.Mobile;
                sms.Message = "尊敬的会员，您的卡号为" + meminfo.CustomerNo + ",欢迎您的加入";
                sms.MemberID = mid;
                sms.IsSent = false;
                sms.AddedDate = DateTime.Now;
                sms.AddedUser = userid.ToString();
                sms.Remark = "入会";
                sms.Channel = "1";
                sms.TryTime = 0;
                db.TM_Sys_SMSSendingQueue.Add(sms);


                db.SaveChanges();
                db.Commit();
                return new Result(true, "入会成功", new MemberInfo { CustomerLevel = "v1", CustomerNo = meminfo.CustomerNo });
            }



        }


        public static Result AddMemberDataMobile(MemberInfo meminfo, List<VechileInfo> vechileList, string userid)
        {
            using (CRMEntities db = new CRMEntities())
            {
                string mid = Guid.NewGuid().ToString("N");
                var member1 = db.V_U_TM_Mem_Info.Where(p => p.Mobile == meminfo.Mobile).FirstOrDefault();
                if (member1 != null) { return new Result(false, "手机号码已存在"); }

                var member2 = db.V_U_TM_Mem_Info.Where(p => p.MemberCardNo == meminfo.CustomerNo).FirstOrDefault();
                if (member2 != null) { return new Result(false, "会员卡号已存在"); }
                db.BeginTransaction();
                //master  ext  account loy_MemExt
                TM_Mem_Ext ext = new TM_Mem_Ext()
                {
                    MemberID = mid,
                    Str_Attr_1 = "1",//会员状态
                    Str_Attr_2 = meminfo.CustomerNo,//会员卡号
                    Str_Attr_3 = meminfo.MemName,//会员姓名
                    Str_Attr_4 = meminfo.Mobile,//手机号
                    Str_Attr_6 = meminfo.Email,//邮箱
                    Str_Attr_7 = meminfo.Gender,//性别
                    Str_Attr_8 = meminfo.CertType,//证件类型
                    Str_Attr_9 = meminfo.CertNo,//证件号
                    Str_Attr_11 = meminfo.Province,//省
                    Str_Attr_12 = meminfo.City,//市
                    Str_Attr_13 = meminfo.District,//区
                    Str_Attr_18 = meminfo.Address,//现居地址
                    Str_Attr_15 = meminfo.MemType,//会员类型

                    Str_Attr_37 = meminfo.ProvinceCode,//省代码
                    Str_Attr_38 = meminfo.CityCode,//市代码
                    Str_Attr_39 = meminfo.DistrictCode,//区代码
                    Str_Attr_33 = meminfo.Wechat,//微信号
                    Str_Attr_30 = meminfo.Region,//所属区域
                    Str_Attr_31 = meminfo.Brand,//所属品牌
                    Date_Attr_2 = string.IsNullOrEmpty(meminfo.Birthday) ? null : (DateTime?)Convert.ToDateTime(meminfo.Birthday),//生日
                    AddedDate = DateTime.Now,
                    AddedUser = userid,
                    ModifiedDate = DateTime.Now,
                    ModifiedUser = userid,
                };
                db.TM_Mem_Ext.Add(ext);

                TM_Mem_Master master = new TM_Mem_Master()
                {
                    MemberID = mid,
                    MemberGrade = 1,
                    MemberLevel = "v1",
                    DataGroupID = 1,
                    Str_Key_1 = meminfo.Mobile,
                    Str_Key_3 = ToolsHelper.MD5(meminfo.Mobile.Substring(5, 6)),
                    AddedDate = DateTime.Now,
                    AddedUser = userid,
                    ModifiedDate = DateTime.Now,
                    ModifiedUser = userid,
                };
                db.TM_Mem_Master.Add(master);

                TM_Mem_Account account = new TM_Mem_Account()
                {
                    AccountID = Guid.NewGuid().ToString("N"),
                    MemberID = mid,
                    AccountType = "1",
                    Value1 = 0,
                    Value2 = 0,
                    Value3 = 0,
                    AddedDate = DateTime.Now,
                    AddedUser = userid,
                    ModifiedDate = DateTime.Now,
                    ModifiedUser = userid,
                };
                db.TM_Mem_Account.Add(account);

                //TM_Mem_AccountDetail accdetail = new TM_Mem_AccountDetail()
                //{
                //    AccountID = account.AccountID,
                //    AccountDetailType = "value1",
                //    DetailValue = 0,
                //    AddedDate = DateTime.Now,
                //    AddedUser = userid,
                //    ModifiedDate = DateTime.Now,
                //    ModifiedUser = userid,
                //};
                //db.TM_Mem_AccountDetail.Add(accdetail);

                TM_Loy_MemExt memext = new TM_Loy_MemExt()
                {
                    MemberID = mid,
                    AddedDate = DateTime.Now,
                    AddedUser = userid,
                    ModifiedDate = DateTime.Now,
                    ModifiedUser = userid,
                };
                db.TM_Loy_MemExt.Add(memext);

                if (vechileList.Count > 0)
                {
                    foreach (var vechileinfo in vechileList)
                    {
                        //车辆信息
                        if (!string.IsNullOrEmpty(vechileinfo.VechileNo))
                        {
                            V_M_TM_Mem_SubExt_vehicle veh = new V_M_TM_Mem_SubExt_vehicle()
                            {
                                ExtType = "vechile",
                                MemberID = mid,
                                VINVehicle = vechileinfo.VinNo,
                                PlateNumVehicle = vechileinfo.VechileNo,
                                BrandVehicle = vechileinfo.VechileBrand,
                                SeriesVehicle = vechileinfo.VechileSerice,
                                LevelVehicle = vechileinfo.VechileType,
                                ColorVehicle = vechileinfo.VechileColor,
                                TrimVehicle = vechileinfo.VechileInner,
                                DriveDistinct = vechileinfo.Mile,
                                IsPass = vechileinfo.IsTransfer,
                                BuyDate = string.IsNullOrEmpty(vechileinfo.BuyDate) ? null : (DateTime?)Convert.ToDateTime(vechileinfo.BuyDate),
                            };
                            dynamic t = db.AddViewRow<V_M_TM_Mem_SubExt_vehicle, TM_Mem_SubExt>(veh);
                        }
                    }
                }

                db.SaveChanges();
                db.Commit();
            }

            return new Result(true, "保存成功");
        }
        #endregion



        public static string loyaltyWebApiURL = ConfigurationSettings.AppSettings["loyaltyWebApiURL"].ToString();
        public static void ComputeLoyalty(string methodName, string memberId, string tradeId)
        {
            Log4netHelper.WriteInfoLog("ComputeLoyalty:" + methodName + "," + memberId + "," + tradeId);
            LoyShare ls = new LoyShare();
            ls.memberScript = "Select MemberID From TM_Mem_Master where MemberID ='" + memberId + "'";
            ls.searchTradeDetailSQL = "Select TradeDetailID From TM_Mem_TradeDetail where TradeID =" + tradeId;
            ls.searchTradeSQL = "Select TradeID From TM_Mem_Trade     where TradeID =" + tradeId;
            var json = JsonHelper.Serialize(ls);
            using (HttpContent httpcontent = new StringContent(json))
            {
                using (var httpClient = new HttpClient())
                {
                    var responseResult = httpClient.PostAsync(loyaltyWebApiURL, httpcontent).Result.Content.ReadAsStringAsync().Result;
                    Log4netHelper.WriteInfoLog(responseResult);
                }
            }
        }

        public static void ComputerRechargeRate(string memberid, string tradeId)
        {
            using (CRMEntities db = new CRMEntities())
            {





            }

        }

        //跨店授权
        public static bool MemberAuthStore(string storeCode, string userId, string mid, CRMEntities db)
        {
            try
            {
                var store = db.V_M_TM_SYS_BaseData_store.Where(p => p.StoreCode == storeCode).FirstOrDefault();
                var l = db.TR_Mem_MemberLimit.Where(p => p.MemberID == mid && p.store == storeCode).FirstOrDefault();
                if (l == null)
                {
                    TR_Mem_MemberLimit tml = new TR_Mem_MemberLimit();
                    tml.AddedDate = DateTime.Now;
                    tml.AddedUser = userId;
                    tml.area = store.AreaCodeStore == null ? "" : store.AreaCodeStore;
                    tml.brand = store.BrandCodeStore == null ? "" : store.BrandCodeStore;
                    tml.store = storeCode;
                    tml.MemberID = mid;
                    db.TR_Mem_MemberLimit.Add(tml);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("MemberAuthStore:" + storeCode + "|" + userId + "|" + mid);
                return false;
            }
        }





        #region 开票
        public static Result AddOrUpdateInvoiceData(string invo, string userid)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {

                    var invoiceInfo = JsonHelper.Deserialize<InvoiceInfo>(invo);
                    if (string.IsNullOrEmpty(invoiceInfo.InvoiceId))
                    {
                        if (string.IsNullOrEmpty(invoiceInfo.StoreCode))
                            return new Result(false, "请选择门店");
                        var act = db.TM_Mem_Account.Where(p => p.MemberID == invoiceInfo.MemberId && p.AccountType == "1").FirstOrDefault();
                        if (act == null) { return new Result(false, "会员账户异常,请系统管理员"); }
                        decimal total = 0;
                        var invoicelst = db.V_M_TM_Mem_Trade_invoice.Where(p => p.MemberID == invoiceInfo.MemberId && p.StatusInvoice == "1").ToList();
                        if (invoicelst != null)
                            total = invoicelst.Sum(p => p.AmountInvoice).Value;
                        if (invoiceInfo.InvoiceCash > act.NoBackAccount)
                            return new Result(false, "未开票金额不足");
                        if (invoiceInfo.InvoiceCash > (act.NoBackAccount - total))
                            return new Result(false, "未开票金额不足,请检查待审批的开票记录");
                        if (invoiceInfo.InvoiceDrawer == "门店")
                        {
                            V_M_TM_Mem_Trade_invoice invoice = new V_M_TM_Mem_Trade_invoice()
                            {
                                MemberID = invoiceInfo.MemberId,
                                DataGroupID = 1,
                                TradeCode = DateTime.Now.ToString("yyyyMMddHHssmmfff"),
                                TradeType = "invoice",
                                NeedLoyCompute = false,
                                StoreCodeInovice = invoiceInfo.StoreCode,//开票门店
                                StoreCode2Inovice = invoiceInfo.StoreCode,//开票门店
                                MobileNumberInvoice = invoiceInfo.Telephone,//手机号
                                InvoiceDate = DateTime.Now,//开票日期
                                StatusInvoice = "1",//开票审批状态
                                AmountInvoice = invoiceInfo.InvoiceCash,//开票金额
                                Memtype = invoiceInfo.MemType,//用户类型
                                InvoiceDrawer = invoiceInfo.InvoiceDrawer,//开票方
                                InvoiceType = invoiceInfo.InvoiceType,//发票类型
                                CorpName = invoiceInfo.CorpName,//企业名称
                                IdentityNo = invoiceInfo.IdentyNo,//识别号
                                CreditCode = invoiceInfo.CreditCode,//信用代码
                                CorpAddress = invoiceInfo.Address,//注册地址
                                CorpMobile = invoiceInfo.CorpMobile,//联系电话
                                DepositBank = invoiceInfo.Bank,//开户行
                                BankAccount = invoiceInfo.BankNo,//开户行账号
                                AddedDate = DateTime.Now,
                                AddedUser = userid,
                                ModifiedDate = DateTime.Now,
                                ModifiedUser = userid
                            };
                            db.AddViewRow<V_M_TM_Mem_Trade_invoice, TM_Mem_Trade>(invoice);
                            db.SaveChanges();
                        }
                        else
                        {
                            V_M_TM_Mem_Trade_invoice invoice = new V_M_TM_Mem_Trade_invoice()
                            {
                                MemberID = invoiceInfo.MemberId,
                                DataGroupID = 1,
                                TradeCode = DateTime.Now.ToString("yyyyMMddHHssmmfff"),
                                TradeType = "invoice",
                                NeedLoyCompute = false,
                                //StoreCodeInovice = invoiceInfo.StoreCode,//开票门店
                                StoreCode2Inovice = invoiceInfo.StoreCode,//开票门店
                                MobileNumberInvoice = invoiceInfo.Telephone,//手机号
                                InvoiceDate = DateTime.Now,//开票日期
                                StatusInvoice = "1",//开票审批状态
                                AmountInvoice = invoiceInfo.InvoiceCash,//开票金额
                                Memtype = invoiceInfo.MemType,//用户类型
                                InvoiceDrawer = invoiceInfo.InvoiceDrawer,//开票方
                                InvoiceType = invoiceInfo.InvoiceType,//发票类型
                                CorpName = invoiceInfo.CorpName,//企业名称
                                IdentityNo = invoiceInfo.IdentyNo,//识别号
                                CreditCode = invoiceInfo.CreditCode,//信用代码
                                CorpAddress = invoiceInfo.Address,//注册地址
                                CorpMobile = invoiceInfo.CorpMobile,//联系电话
                                DepositBank = invoiceInfo.Bank,//开户行
                                BankAccount = invoiceInfo.BankNo,//开户行账号
                                AddedDate = DateTime.Now,
                                AddedUser = userid,
                                ModifiedDate = DateTime.Now,
                                ModifiedUser = userid
                            };
                            db.AddViewRow<V_M_TM_Mem_Trade_invoice, TM_Mem_Trade>(invoice);
                            db.SaveChanges();


                        }
                        return new Result(true, "保存成功");
                    }
                    else
                    {
                        var invoiceId = Convert.ToInt32(invoiceInfo.InvoiceId);
                        var act = db.TM_Mem_Account.Where(p => p.MemberID == invoiceInfo.MemberId && p.AccountType == "1").FirstOrDefault();
                        if (act == null) { return new Result(false, "会员账户异常,请系统管理员"); }
                        decimal total = 0;
                        var invoicelst = db.V_M_TM_Mem_Trade_invoice.Where(p => p.MemberID == invoiceInfo.MemberId && p.StatusInvoice == "1" && p.TradeID != invoiceId).ToList();
                        if (invoicelst != null)
                            total = invoicelst.Sum(p => p.AmountInvoice).Value;
                        if (invoiceInfo.InvoiceCash > act.NoBackAccount)
                            return new Result(false, "未开票金额不足");
                        if (invoiceInfo.InvoiceCash > (act.NoBackAccount - total))
                            return new Result(false, "未开票金额不足,请检查待审批的开票记录");
                        var invoice = db.V_M_TM_Mem_Trade_invoice.Where(p => p.TradeID == invoiceId).FirstOrDefault();
                        //赋值
                        if (invoiceInfo.InvoiceDrawer == "门店")
                        {
                            V_M_TM_Mem_Trade_invoice newinvoice = new V_M_TM_Mem_Trade_invoice();
                            newinvoice.TradeID = invoice.TradeID;
                            newinvoice.TradeCode = invoice.TradeCode;
                            newinvoice.AddedDate = invoice.AddedDate;
                            newinvoice.DataGroupID = invoice.DataGroupID;
                            newinvoice.AddedUser = userid;
                            newinvoice.StatusInvoice = invoice.StatusInvoice;
                            newinvoice.NeedLoyCompute = invoice.NeedLoyCompute;
                            newinvoice.TradeType = invoice.TradeType;
                            newinvoice.MemberID = invoice.MemberID;
                            newinvoice.StoreCodeInovice = invoiceInfo.StoreCode;//开票门店
                            newinvoice.StoreCode2Inovice = invoiceInfo.StoreCode;//开票门店
                            newinvoice.MobileNumberInvoice = invoiceInfo.Telephone;//手机号
                            newinvoice.AmountInvoice = invoiceInfo.InvoiceCash;//开票金额
                            newinvoice.InvoiceDrawer = invoiceInfo.InvoiceDrawer;//开票方
                            newinvoice.Memtype = invoiceInfo.MemType;
                            newinvoice.InvoiceType = invoiceInfo.InvoiceType;//发票类型
                            newinvoice.CorpName = invoiceInfo.CorpName;//企业名称
                            newinvoice.IdentityNo = invoiceInfo.IdentyNo;//识别号
                            newinvoice.CreditCode = invoiceInfo.CreditCode;//信用代码
                            newinvoice.CorpAddress = invoiceInfo.Address;//注册地址
                            newinvoice.CorpMobile = invoiceInfo.CorpMobile;//联系电话
                            newinvoice.DepositBank = invoiceInfo.Bank;//开户行
                            newinvoice.BankAccount = invoiceInfo.BankNo;//开户行账号
                            newinvoice.ModifiedDate = DateTime.Now;
                            newinvoice.InvoiceDate = invoice.InvoiceDate;
                            newinvoice.ModifiedUser = userid;
                            db.UpdateViewRow<V_M_TM_Mem_Trade_invoice, TM_Mem_Trade>(newinvoice);
                            db.SaveChanges();
                            return new Result(true, "修改成功");
                        }
                        else
                        {
                            V_M_TM_Mem_Trade_invoice newinvoice = new V_M_TM_Mem_Trade_invoice();
                            newinvoice.TradeID = invoice.TradeID;
                            newinvoice.TradeCode = invoice.TradeCode;
                            newinvoice.AddedDate = invoice.AddedDate;
                            newinvoice.DataGroupID = invoice.DataGroupID;
                            newinvoice.AddedUser = userid;
                            newinvoice.StatusInvoice = invoice.StatusInvoice;
                            newinvoice.NeedLoyCompute = invoice.NeedLoyCompute;
                            newinvoice.TradeType = invoice.TradeType;
                            newinvoice.MemberID = invoice.MemberID;
                            newinvoice.StoreCodeInovice = null;
                            newinvoice.StoreCode2Inovice = invoiceInfo.StoreCode;//开票门店
                            newinvoice.MobileNumberInvoice = invoiceInfo.Telephone;//手机号
                            newinvoice.AmountInvoice = invoiceInfo.InvoiceCash;//开票金额
                            newinvoice.InvoiceDrawer = invoiceInfo.InvoiceDrawer;//开票方
                            newinvoice.Memtype = invoiceInfo.MemType;
                            newinvoice.InvoiceType = invoiceInfo.InvoiceType;//发票类型
                            newinvoice.CorpName = invoiceInfo.CorpName;//企业名称
                            newinvoice.IdentityNo = invoiceInfo.IdentyNo;//识别号
                            newinvoice.CreditCode = invoiceInfo.CreditCode;//信用代码
                            newinvoice.CorpAddress = invoiceInfo.Address;//注册地址
                            newinvoice.CorpMobile = invoiceInfo.CorpMobile;//联系电话
                            newinvoice.DepositBank = invoiceInfo.Bank;//开户行
                            newinvoice.BankAccount = invoiceInfo.BankNo;//开户行账号
                            newinvoice.ModifiedDate = DateTime.Now;
                            newinvoice.InvoiceDate = invoice.InvoiceDate;
                            newinvoice.ModifiedUser = userid;
                            db.UpdateViewRow<V_M_TM_Mem_Trade_invoice, TM_Mem_Trade>(newinvoice);
                            db.SaveChanges();
                            return new Result(true, "修改成功");

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return new Result(false, ex.ToString());
            }

        }

        public static Result EditInvoiceData(string invo, string userid)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {

                    var invoiceInfo = JsonHelper.Deserialize<InvoiceInfo>(invo);
                    var InvoiceId = int.Parse(invoiceInfo.InvoiceId);
                    if (string.IsNullOrEmpty(invoiceInfo.InvoiceId))
                    {
                        if (string.IsNullOrEmpty(invoiceInfo.StoreCode))
                            return new Result(false, "请选择门店");

                        if (string.IsNullOrEmpty(invoiceInfo.InvoiceNo))
                            return new Result(false, "请填写开票单号");

                        var i = db.V_M_TM_Mem_Trade_invoice.Where(p => p.InvoiceOrderNo == invoiceInfo.InvoiceNo && p.TradeID != InvoiceId);

                        if (i.Count() > 0)
                            return new Result(false, "该开票单号已存在");



                        var act = db.TM_Mem_Account.Where(p => p.MemberID == invoiceInfo.MemberId && p.AccountType == "1").FirstOrDefault();
                        if (act == null) { return new Result(false, "会员账户异常,请系统管理员"); }
                        decimal total = 0;
                        var invoicelst = db.V_M_TM_Mem_Trade_invoice.Where(p => p.MemberID == invoiceInfo.MemberId && p.StatusInvoice == "1").ToList();
                        if (invoicelst != null)
                            total = invoicelst.Sum(p => p.AmountInvoice).Value;
                        if (invoiceInfo.InvoiceCash > act.NoBackAccount)
                            return new Result(false, "未开票金额不足");
                        if (invoiceInfo.InvoiceCash > (act.NoBackAccount - total))
                            return new Result(false, "未开票金额不足,请检查待审批的开票记录");

                        if (invoiceInfo.InvoiceDrawer == "门店")
                        {
                            V_M_TM_Mem_Trade_invoice invoice = new V_M_TM_Mem_Trade_invoice()
                            {
                                InvoiceOrderNo = invoiceInfo.InvoiceNo,
                                MemberID = invoiceInfo.MemberId,
                                DataGroupID = 1,
                                TradeCode = DateTime.Now.ToString("yyyyMMddHHssmmfff"),
                                TradeType = "invoice",
                                NeedLoyCompute = false,
                                StoreCodeInovice = invoiceInfo.StoreCode,//开票门店
                                StoreCode2Inovice = invoiceInfo.StoreCode,//开票门店
                                MobileNumberInvoice = invoiceInfo.Telephone,//手机号
                                InvoiceDate = DateTime.Now,//开票日期
                                StatusInvoice = "1",//开票审批状态
                                AmountInvoice = invoiceInfo.InvoiceCash,//开票金额
                                Memtype = invoiceInfo.MemType,//用户类型
                                InvoiceDrawer = invoiceInfo.InvoiceDrawer,//开票方
                                InvoiceType = invoiceInfo.InvoiceType,//发票类型
                                CorpName = invoiceInfo.CorpName,//企业名称
                                IdentityNo = invoiceInfo.IdentyNo,//识别号
                                CreditCode = invoiceInfo.CreditCode,//信用代码
                                CorpAddress = invoiceInfo.Address,//注册地址
                                CorpMobile = invoiceInfo.CorpMobile,//联系电话
                                DepositBank = invoiceInfo.Bank,//开户行
                                BankAccount = invoiceInfo.BankNo,//开户行账号
                                AddedDate = DateTime.Now,
                                AddedUser = userid,
                                ModifiedDate = DateTime.Now,
                                ModifiedUser = userid
                            };
                            db.AddViewRow<V_M_TM_Mem_Trade_invoice, TM_Mem_Trade>(invoice);
                            db.SaveChanges();
                        }
                        else
                        {
                            V_M_TM_Mem_Trade_invoice invoice = new V_M_TM_Mem_Trade_invoice()
                            {
                                MemberID = invoiceInfo.MemberId,
                                DataGroupID = 1,
                                TradeCode = DateTime.Now.ToString("yyyyMMddHHssmmfff"),
                                TradeType = "invoice",
                                NeedLoyCompute = false,
                                //StoreCodeInovice = invoiceInfo.StoreCode,//开票门店
                                StoreCode2Inovice = invoiceInfo.StoreCode,//开票门店
                                MobileNumberInvoice = invoiceInfo.Telephone,//手机号
                                InvoiceDate = DateTime.Now,//开票日期
                                StatusInvoice = "1",//开票审批状态
                                AmountInvoice = invoiceInfo.InvoiceCash,//开票金额
                                Memtype = invoiceInfo.MemType,//用户类型
                                InvoiceDrawer = invoiceInfo.InvoiceDrawer,//开票方
                                InvoiceType = invoiceInfo.InvoiceType,//发票类型
                                CorpName = invoiceInfo.CorpName,//企业名称
                                IdentityNo = invoiceInfo.IdentyNo,//识别号
                                CreditCode = invoiceInfo.CreditCode,//信用代码
                                CorpAddress = invoiceInfo.Address,//注册地址
                                CorpMobile = invoiceInfo.CorpMobile,//联系电话
                                DepositBank = invoiceInfo.Bank,//开户行
                                BankAccount = invoiceInfo.BankNo,//开户行账号
                                AddedDate = DateTime.Now,
                                AddedUser = userid,
                                ModifiedDate = DateTime.Now,
                                ModifiedUser = userid
                            };
                            db.AddViewRow<V_M_TM_Mem_Trade_invoice, TM_Mem_Trade>(invoice);
                            db.SaveChanges();


                        }
                        return new Result(true, "保存成功");
                    }
                    else
                    {
                        var invoiceId = Convert.ToInt32(invoiceInfo.InvoiceId);
                        var act = db.TM_Mem_Account.Where(p => p.MemberID == invoiceInfo.MemberId && p.AccountType == "1").FirstOrDefault();
                        if (act == null) { return new Result(false, "会员账户异常,请系统管理员"); }
                        decimal total = 0;
                        var invoicelst = db.V_M_TM_Mem_Trade_invoice.Where(p => p.MemberID == invoiceInfo.MemberId && p.StatusInvoice == "1" && p.TradeID != invoiceId).ToList();
                        if (invoicelst != null)
                            total = invoicelst.Sum(p => p.AmountInvoice).Value;
                        if (invoiceInfo.InvoiceCash > act.NoBackAccount)
                            return new Result(false, "未开票金额不足");
                        if (invoiceInfo.InvoiceCash > (act.NoBackAccount - total))
                            return new Result(false, "未开票金额不足,请检查待审批的开票记录");
                        if (string.IsNullOrEmpty(invoiceInfo.InvoiceNo))
                            return new Result(false, "请填写开票单号");
                        var i = db.V_M_TM_Mem_Trade_invoice.Where(p => p.InvoiceOrderNo == invoiceInfo.InvoiceNo && p.TradeID != InvoiceId);

                        if (i.Count() > 0)
                            return new Result(false, "该开票单号已存在");


                        var invoice = db.V_M_TM_Mem_Trade_invoice.Where(p => p.TradeID == invoiceId).FirstOrDefault();
                        //赋值
                        if (invoiceInfo.InvoiceDrawer == "门店")
                        {
                            V_M_TM_Mem_Trade_invoice newinvoice = new V_M_TM_Mem_Trade_invoice();
                            newinvoice.InvoiceOrderNo = invoiceInfo.InvoiceNo;
                            newinvoice.TradeID = invoice.TradeID;
                            newinvoice.TradeCode = invoice.TradeCode;
                            newinvoice.AddedDate = invoice.AddedDate;
                            newinvoice.DataGroupID = invoice.DataGroupID;
                            newinvoice.AddedUser = userid;
                            newinvoice.StatusInvoice = invoice.StatusInvoice;
                            newinvoice.NeedLoyCompute = invoice.NeedLoyCompute;
                            newinvoice.TradeType = invoice.TradeType;
                            newinvoice.MemberID = invoice.MemberID;
                            newinvoice.StoreCodeInovice = invoiceInfo.StoreCode;//开票门店
                            newinvoice.StoreCode2Inovice = invoiceInfo.StoreCode;//开票门店
                            newinvoice.MobileNumberInvoice = invoiceInfo.Telephone;//手机号
                            newinvoice.AmountInvoice = invoiceInfo.InvoiceCash;//开票金额
                            newinvoice.InvoiceDrawer = invoiceInfo.InvoiceDrawer;//开票方
                            newinvoice.Memtype = invoiceInfo.MemType;
                            newinvoice.InvoiceType = invoiceInfo.InvoiceType;//发票类型
                            newinvoice.CorpName = invoiceInfo.CorpName;//企业名称
                            newinvoice.IdentityNo = invoiceInfo.IdentyNo;//识别号
                            newinvoice.CreditCode = invoiceInfo.CreditCode;//信用代码
                            newinvoice.CorpAddress = invoiceInfo.Address;//注册地址
                            newinvoice.CorpMobile = invoiceInfo.CorpMobile;//联系电话
                            newinvoice.DepositBank = invoiceInfo.Bank;//开户行
                            newinvoice.BankAccount = invoiceInfo.BankNo;//开户行账号
                            newinvoice.ModifiedDate = DateTime.Now;
                            newinvoice.InvoiceDate = invoice.InvoiceDate;
                            newinvoice.ModifiedUser = userid;
                            db.UpdateViewRow<V_M_TM_Mem_Trade_invoice, TM_Mem_Trade>(newinvoice);
                            db.SaveChanges();
                            return new Result(true, "修改成功");
                        }
                        else
                        {
                            V_M_TM_Mem_Trade_invoice newinvoice = new V_M_TM_Mem_Trade_invoice();
                            newinvoice.InvoiceOrderNo = invoiceInfo.InvoiceNo;
                            newinvoice.TradeID = invoice.TradeID;
                            newinvoice.TradeCode = invoice.TradeCode;
                            newinvoice.AddedDate = invoice.AddedDate;
                            newinvoice.DataGroupID = invoice.DataGroupID;
                            newinvoice.AddedUser = userid;
                            newinvoice.StatusInvoice = invoice.StatusInvoice;
                            newinvoice.NeedLoyCompute = invoice.NeedLoyCompute;
                            newinvoice.TradeType = invoice.TradeType;
                            newinvoice.MemberID = invoice.MemberID;
                            newinvoice.StoreCodeInovice = null;
                            newinvoice.StoreCode2Inovice = invoiceInfo.StoreCode;//开票门店
                            newinvoice.MobileNumberInvoice = invoiceInfo.Telephone;//手机号
                            newinvoice.AmountInvoice = invoiceInfo.InvoiceCash;//开票金额
                            newinvoice.InvoiceDrawer = invoiceInfo.InvoiceDrawer;//开票方
                            newinvoice.Memtype = invoiceInfo.MemType;
                            newinvoice.InvoiceType = invoiceInfo.InvoiceType;//发票类型
                            newinvoice.CorpName = invoiceInfo.CorpName;//企业名称
                            newinvoice.IdentityNo = invoiceInfo.IdentyNo;//识别号
                            newinvoice.CreditCode = invoiceInfo.CreditCode;//信用代码
                            newinvoice.CorpAddress = invoiceInfo.Address;//注册地址
                            newinvoice.CorpMobile = invoiceInfo.CorpMobile;//联系电话
                            newinvoice.DepositBank = invoiceInfo.Bank;//开户行
                            newinvoice.BankAccount = invoiceInfo.BankNo;//开户行账号
                            newinvoice.ModifiedDate = DateTime.Now;
                            newinvoice.InvoiceDate = invoice.InvoiceDate;
                            newinvoice.ModifiedUser = userid;
                            db.UpdateViewRow<V_M_TM_Mem_Trade_invoice, TM_Mem_Trade>(newinvoice);
                            db.SaveChanges();
                            return new Result(true, "修改成功");

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return new Result(false, ex.ToString());
            }

        }


        public static Result GetInvoiceTotal(string mid)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var act = db.TM_Mem_Account.Where(p => p.MemberID == mid && p.AccountType == "1").FirstOrDefault();
                var noInvoice = (act.NoBackAccount == null ? 0 : act.NoBackAccount);//未开票金额
                var invoice = db.V_M_TM_Mem_Trade_invoice.Where(p => p.MemberID == mid).ToList();
                var invoice1 = invoice.Where(p => p.StatusInvoice == "1").Sum(p => p.AmountInvoice == null ? 0 : p.AmountInvoice);//处于待审批状态的票
                var invoice2 = invoice.Where(p => p.StatusInvoice == "2").Sum(p => p.AmountInvoice == null ? 0 : p.AmountInvoice);//处于已审批状态的票
                ActInfo actInfo = new ActInfo()
                {
                    MemberId = mid,
                    NoInvoiceAct = noInvoice,
                    InvoiceAct = invoice2,
                };
                return new Result(true, "", actInfo);
            }
        }

        public static Result GetInvoiceData(string mid, DateTime? startDate, DateTime? endDate, string dp, string rds, int pid)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            List<int> rid = JsonHelper.Deserialize<List<int>>(rds);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.V_M_TM_Mem_Trade_invoice.Where(p => p.MemberID == mid)
                            join b in db.V_U_TM_Mem_Info on a.MemberID equals b.MemberID
                            join c in db.TD_SYS_BizOption.Where(p => p.OptionType == "MemberType") on b.CustomerStatus equals c.OptionValue into cc
                            from cca in cc.DefaultIfEmpty()
                            join stmp in db.V_M_TM_SYS_BaseData_store.FilterDataByAuth(rid, pid, 1) on a.StoreCode2Inovice equals stmp.StoreCode
                            select new
                            {
                                a.TradeID,
                                a.MemberID,
                                a.TradeCode,
                                a.InvoiceDrawer,
                                b.CustomerMobile,
                                StoreName = stmp.StoreName,
                                MobileNumberInvoice = b.Mobile,
                                a.InvoiceDate,//开票日期
                                a.StatusInvoice,//审批状态
                                a.AmountInvoice,
                                b.MemberCardNo,//会员卡号
                                Memtype = cca.OptionText,//个人还是企业
                                a.AddedDate,
                            };
                DateTime dt = Convert.ToDateTime(endDate).AddDays(1);
                if (startDate != null) { query = query.Where(p => p.AddedDate >= startDate); }
                if (endDate != null) { query = query.Where(p => p.AddedDate <= dt); }
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }
        public static Result GetInvoiceEditInfo(long tid)
        {
            using (var db = new CRMEntities())
            {
                var query = db.V_M_TM_Mem_Trade_invoice.FirstOrDefault(p => p.TradeID == tid);
                return new Result(true, "", query);
            }
        }
        #endregion

        #region 车辆管理
        public static Result UpdateVehcileData(string vechile, string userid)
        {
            VechileInfo vechileinfo = JsonHelper.Deserialize<VechileInfo>(vechile);

            using (CRMEntities db = new CRMEntities())
            {
                V_M_TM_Mem_SubExt_vehicle veh = new V_M_TM_Mem_SubExt_vehicle()
                {
                    //MemberSubExtID = vechileinfo.VechileId,
                    MemberID = vechileinfo.MemberID,
                    VINVehicle = vechileinfo.VinNo,
                    PlateNumVehicle = vechileinfo.VechileNo,
                    BrandVehicle = vechileinfo.VechileBrand,
                    SeriesVehicle = vechileinfo.VechileSerice,
                    LevelVehicle = vechileinfo.VechileType,
                    ColorVehicle = vechileinfo.VechileColor,
                    TrimVehicle = vechileinfo.VechileInner,
                    DriveDistinct = vechileinfo.Mile,
                    IsPass = vechileinfo.IsTransfer,
                    BuyDate = string.IsNullOrEmpty(vechileinfo.BuyDate) ? null : (DateTime?)Convert.ToDateTime(vechileinfo.BuyDate),
                };
                if (vechileinfo.VechileId == 0)
                {
                    var mem = db.V_U_TM_Mem_Info.Where(p => p.MemberID == vechileinfo.MemberID).FirstOrDefault();
                    var memve = db.V_M_TM_Mem_SubExt_vehicle.ToList();
                    var memveByID = db.V_M_TM_Mem_SubExt_vehicle.Where(p => p.MemberID == vechileinfo.MemberID).ToList();

                    if (mem.CustomerType2 == "0" && memveByID.Count >= 1) { return new Result(false, "用户为企业用户，只能有一辆车"); }
                    //if (memve.Where(p => p.PlateNumVehicle == vechileinfo.VechileNo).Count() > 0) { return new Result(false, "此车牌号已存在"); }
                    if (vechileinfo.IsTransfer != "1")
                    {
                        if (memve.Where(p => p.VINVehicle == vechileinfo.VinNo && p.IsPass == "0").Count() > 0) { return new Result(false, "此车架号已存在"); }
                    }
                    dynamic t = db.AddViewRow<V_M_TM_Mem_SubExt_vehicle, TM_Mem_SubExt>(veh);
                    db.SaveChanges();
                    return new Result(true, "新增成功");
                }
                else
                {
                    var memve = db.V_M_TM_Mem_SubExt_vehicle.ToList();

                    //if (memve.Where(p => p.PlateNumVehicle == vechileinfo.VechileNo).Count() > 0) { return new Result(false, "此车牌号已存在"); }
                    if (vechileinfo.IsTransfer != "1")
                    {
                        if (memve.Where(p => p.VINVehicle == vechileinfo.VinNo && p.IsPass == "0" && p.MemberSubExtID != vechileinfo.VechileId).Count() > 0) { return new Result(false, "此车架号已存在"); }
                    }
                    veh.MemberSubExtID = vechileinfo.VechileId;
                    dynamic t = db.UpdateViewRow<V_M_TM_Mem_SubExt_vehicle, TM_Mem_SubExt>(veh);
                    db.SaveChanges();
                    return new Result(true, "修改成功");
                }

            }

        }

        public static Result GetVehcileInfoByid(long id)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.V_M_TM_Mem_SubExt_vehicle.Where(p => p.MemberSubExtID == id).FirstOrDefault();
                return new Result(true, "", query);
            }
        }
        public static Result DeleteVehcileData(long id)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.V_M_TM_Mem_SubExt_vehicle.Where(p => p.MemberSubExtID == id).FirstOrDefault();
                if (query != null)
                {
                    dynamic t = db.DeleteViewRow<V_M_TM_Mem_SubExt_vehicle, TM_Mem_SubExt>(query);
                    db.SaveChanges();

                    return new Result(true, "删除成功");
                }
                return new Result(false, "删除失败");
            }
        }
        public static Result TransferVehcileData(long id)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_Mem_SubExt.Where(p => p.MemberSubExtID == id && p.ExtType == "vehicle").FirstOrDefault();
                if (query != null)
                {
                    query.Str_Attr_9 = "1";
                    var entry = db.Entry(query);
                    entry.State = EntityState.Modified;
                    db.SaveChanges();

                    return new Result(true, "过户成功");
                }
                return new Result(false, "过户失败");
            }
        }
        #endregion

        #region 账户调整新
        public static Result SaveAccountChange(string mid, string tid, decimal changeValue, string userId, string changereson, string changeresoncode, string remark, string storeCode)
        {
            using (CRMEntities db = new CRMEntities())
            {
                //总账户
                var query1 = db.TM_Mem_Account.Where(p => p.MemberID == mid && p.AccountType == "1").FirstOrDefault();
                if (query1 == null)
                {
                    return new Result(false, "此用户还未建立账户，请联系管理员");
                }
                //会员的账户明细
                var query2 = db.TM_Mem_AccountDetail.Where(p => p.AccountID == query1.AccountID);


                //审批完才算调整成功
                //query1.Value1 += changeValue;
                //db.Entry(query1).State = EntityState.Modified;
                //日志

                var query3 = query2.Where(p => p.SpecialDate1 == null && p.SpecialDate2 == null).FirstOrDefault();
                if (query3 != null)
                {
                    //query3.DetailValue = query3.DetailValue + changeValue;
                    //var entry = db.Entry(query3);
                    //entry.State = EntityState.Modified;

                    DateTime dt = DateTime.Now;
                    if (string.IsNullOrEmpty(tid))
                    {
                        V_M_TM_Mem_Trade_change change = new V_M_TM_Mem_Trade_change()
                        {
                            DataGroupID = 1,
                            TradeCode = dt.ToString("yyyyMMddHHssmmfff"),
                            MemberID = mid,
                            NeedLoyCompute = false,
                            ChangeStoreCode = storeCode,
                            StatusChange = "1",
                            ActChangeReason = changereson,
                            ActChangeReasonCode = changeresoncode,
                            ActChangeValue = changeValue,
                            Remark = remark,
                            AccountDetailID = query3.AccountDetailID.ToString(),
                            AccountID = query1.AccountID,
                            AddedDate = dt,
                            AddedUser = userId,
                            ModifiedDate = dt,
                            ModifiedUser = userId
                        };
                        db.AddViewRow<V_M_TM_Mem_Trade_change, TM_Mem_Trade>(change);

                        db.SaveChanges();
                    }
                    else
                    {
                        var tradeid = Convert.ToInt32(tid);
                        var cha = db.V_M_TM_Mem_Trade_change.Where(p => p.TradeID == tradeid).FirstOrDefault();

                        V_M_TM_Mem_Trade_change change = new V_M_TM_Mem_Trade_change()
                        {
                            TradeID = cha.TradeID,
                            DataGroupID = 1,
                            TradeCode = cha.TradeCode,
                            MemberID = mid,
                            NeedLoyCompute = false,
                            ChangeStoreCode = storeCode,
                            StatusChange = "1",
                            ActChangeReason = changereson,
                            ActChangeReasonCode = changeresoncode,
                            ActChangeValue = changeValue,
                            Remark = remark,
                            AccountDetailID = cha.AccountDetailID,
                            AccountID = cha.AccountID,
                            AddedDate = cha.AddedDate,
                            AddedUser = cha.AddedUser,
                            ModifiedDate = dt,
                            ModifiedUser = userId
                        };
                        db.UpdateViewRow<V_M_TM_Mem_Trade_change, TM_Mem_Trade>(change);
                        db.SaveChanges();
                    }
                }

                return new Result(true, "调整申请成功");
            }
        }

        public static Result GetAccountChangeRecord(string mid, string startDate, string endDate, string rds, int pid, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            List<int> rid = JsonHelper.Deserialize<List<int>>(rds);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.V_M_TM_Mem_Trade_change.Where(p => p.MemberID == mid)
                            join b in db.V_U_TM_Mem_Info on a.MemberID equals b.MemberID
                            join d in db.TM_AUTH_User on a.AddedUser equals SqlFunctions.StringConvert((double)d.UserID).Trim() into dc
                            from dca in dc.DefaultIfEmpty()
                            join stmp in db.V_M_TM_SYS_BaseData_store.FilterDataByAuth(rid, pid, 1) on a.ChangeStoreCode equals stmp.StoreCode
                            orderby a.AddedDate descending
                            select new
                            {
                                a.TradeID,
                                a.StatusChange,
                                a.ActChangeValue,
                                a.ActChangeReason,
                                a.AddedDate,
                                dca.UserName
                            };

                if (!string.IsNullOrEmpty(startDate))
                {
                    DateTime parseDateStart = DateTime.Parse(startDate);
                    query = query.Where(p => p.AddedDate >= parseDateStart);
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    DateTime parseDateEnd = DateTime.Parse(endDate).AddDays(1);
                    query = query.Where(p => p.AddedDate <= parseDateEnd);
                }
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }
        public static Result GetActChangeEditInfo(long tid)
        {
            using (var db = new CRMEntities())
            {
                var query = db.V_M_TM_Mem_Trade_change.FirstOrDefault(p => p.TradeID == tid);
                return new Result(true, "", query);
            }
        }
        #endregion

        #region 账户调整审核
        public static Result GetAccountChangeRecord_Finance(string cardNO, string name, string mobile, string status, string startDate, string endDate, string dp, string rds, int pid)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            List<int> rid = JsonHelper.Deserialize<List<int>>(rds);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.V_M_TM_Mem_Trade_change
                            join b in db.V_U_TM_Mem_Info on a.MemberID equals b.MemberID
                            join d in db.TM_AUTH_User on a.AddedUser equals SqlFunctions.StringConvert((double)d.UserID).Trim() into dc
                            from dca in dc.DefaultIfEmpty()
                            join stmp in db.V_M_TM_SYS_BaseData_store.FilterDataByAuth(rid, pid, 1) on a.ChangeStoreCode equals stmp.StoreCode
                            orderby a.AddedDate descending
                            select new
                            {
                                a.TradeID,
                                b.MemberCardNo,
                                b.MemberID,
                                b.CustomerName,
                                b.Mobile,
                                a.StatusChange,
                                a.ActChangeValue,
                                a.ActChangeReason,
                                a.AddedDate,
                                dca.UserName,
                                // e.PlateNumVehicle,
                            };
                if (!string.IsNullOrEmpty(cardNO)) query = query.Where(p => p.MemberCardNo.Contains(cardNO));
                if (!string.IsNullOrEmpty(name)) query = query.Where(p => p.CustomerName.Contains(name));
                if (!string.IsNullOrEmpty(mobile)) query = query.Where(p => p.Mobile.Contains(mobile));

                if (!string.IsNullOrEmpty(status)) query = query.Where(p => p.StatusChange == status);
                if (!string.IsNullOrEmpty(startDate))
                {
                    DateTime parseDateStart = DateTime.Parse(startDate);
                    query = query.Where(p => p.AddedDate >= parseDateStart);
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    DateTime parseDateEnd = DateTime.Parse(endDate).AddDays(1);
                    query = query.Where(p => p.AddedDate <= parseDateEnd);
                }
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }

        }
        public static Result InActiveActChangeById(long tId, string userId)
        {

            DateTime dt = DateTime.Now;
            using (CRMEntities db = new CRMEntities())
            {
                var trade = db.TM_Mem_Trade.Where(p => p.TradeID == tId).FirstOrDefault();
                if (trade == null)
                    return new Result(false, "账户调整单异常,请联系管理员");
                var am = trade.Dec_Attr_1 == null ? 0 : Convert.ToDecimal(trade.Dec_Attr_1);
                //修改审批状态
                trade.Str_Attr_5 = "2";
                var account = db.TM_Mem_Account.Where(p => p.MemberID == trade.MemberID).FirstOrDefault();
                if (account == null)
                    return new Result(false, "会员账户异常,请联系管理员");
                var detail = db.TM_Mem_AccountDetail.Where(p => p.AccountID == account.AccountID && p.SpecialDate1 == null && p.SpecialDate2 == null).FirstOrDefault();
                if (detail != null)
                {
                    detail.DetailValue += am;
                    var entry = db.Entry(detail);
                    entry.State = EntityState.Modified;
                }
                account.Value1 += am;
                db.Entry(account).State = EntityState.Modified;

                //审批完之后往日志表中插入数据
                TL_Mem_AccountChange entt = new TL_Mem_AccountChange();
                entt.MemberID = trade.MemberID;
                entt.AccountID = trade.Str_Attr_1;
                entt.AccountDetailID = trade.Str_Attr_2;
                entt.AccountChangeType = "manu";
                entt.ChangeValue = am;
                entt.ChangeReason = trade.Str_Attr_3;
                entt.AddedDate = DateTime.Now;
                entt.Remark = trade.Str_Attr_4;
                entt.AddedUser = userId.ToString();
                db.TL_Mem_AccountChange.Add(entt);
                db.SaveChanges();
                return new Result(true, "审批成功");
            }
        }

        #endregion

        #region 老会员审批
        public static Result UpdateOldMemStatus(string mid, Decimal? total, Decimal? noinvoice, Decimal? recharge, Decimal? send, string userid)
        {
            using (CRMEntities db = new CRMEntities())
            {
                db.BeginTransaction();
                if (total != null && noinvoice != null && recharge != null && send != null)
                {
                    //account主表
                    var act = db.TM_Mem_Account.Where(p => p.MemberID == mid && p.AccountType == "1").FirstOrDefault();
                    act.Value1 = Convert.ToDecimal(recharge) + Convert.ToDecimal(send);
                    act.NoInvoiceAccount = Convert.ToDecimal(noinvoice);
                    act.NoBackAccount = Convert.ToDecimal(noinvoice);
                    act.ModifiedDate = DateTime.Now;
                    act.ModifiedUser = userid;
                    db.Entry(act).State = EntityState.Modified;

                    //accountdetail主表明细表
                    var actdetail = db.TM_Mem_AccountDetail.Where(p => p.AccountID == act.AccountID && p.AccountDetailType == "value1").FirstOrDefault();
                    actdetail.DetailValue = Convert.ToDecimal(recharge) + Convert.ToDecimal(send);
                    actdetail.ModifiedDate = DateTime.Now;
                    actdetail.ModifiedUser = userid;
                    db.Entry(actdetail).State = EntityState.Modified;

                    DateTime t = DateTime.Now;

                    var actchange = db.TL_Mem_AccountChange.Where(p => p.MemberID == mid && p.AccountChangeType == "init").ToList();
                    var changerecharge = actchange.Where(p => p.ChangeReason == "老会员现金").FirstOrDefault();
                    if (changerecharge != null)
                    {
                        changerecharge.ChangeValue = Convert.ToDecimal(recharge);
                        db.Entry(changerecharge).State = EntityState.Modified;
                    }
                    else
                    {
                        TL_Mem_AccountChange tl = new TL_Mem_AccountChange();
                        tl.AccountChangeType = "init";
                        tl.AccountDetailID = actdetail.AccountDetailID;
                        tl.MemberID = mid;
                        tl.HasReverse = false;
                        tl.ChangeValue = Convert.ToDecimal(recharge);
                        tl.AccountID = act.AccountID;
                        tl.AddedDate = t;
                        tl.AddedUser = userid;
                        tl.ChangeReason = "老会员现金";
                        db.TL_Mem_AccountChange.Add(tl);
                    }


                    var changesend = actchange.Where(p => p.ChangeReason == "老会员赠送").FirstOrDefault();
                    if (changesend != null)
                    {
                        changesend.ChangeValue = Convert.ToDecimal(send);
                        db.Entry(changesend).State = EntityState.Modified;
                    }
                    else
                    {
                        TL_Mem_AccountChange tl2 = new TL_Mem_AccountChange();
                        tl2.AccountChangeType = "init";
                        tl2.AccountDetailID = actdetail.AccountDetailID;
                        tl2.MemberID = mid;
                        tl2.HasReverse = false;
                        tl2.ChangeValue = Convert.ToDecimal(send);
                        tl2.AccountID = act.AccountID; ;
                        tl2.AddedDate = t;
                        tl2.AddedUser = userid;
                        tl2.ChangeReason = "老会员赠送";
                        db.TL_Mem_AccountChange.Add(tl2);
                    }


                    var memext = db.TM_Loy_MemExt.Where(p => p.MemberID == mid).FirstOrDefault();
                    memext.Dec_Attr_23 = total;
                    db.Entry(memext).State = EntityState.Modified;
                    db.SaveChanges();

                }
                var ext = db.TM_Mem_Ext.Where(p => p.MemberID == mid).FirstOrDefault();
                if (ext != null)
                {
                    ext.Str_Attr_1 = "1";
                    db.Entry(ext).State = EntityState.Modified;
                    db.SaveChanges();
                    db.Commit();
                    return new Result(true, "审批成功");
                }
                else
                {
                    return new Result(false, "审批失败");
                }
            }

        }

        public static Result GetOldMembersByPage(string dp, string memNo, string memName, string memMobile, string status, string startDate, string endDate, string rds, int pid)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {

                if (rds == "[]")
                {
                    List<string> rid = JsonHelper.Deserialize<List<string>>(rds);
                    StringBuilder limitSql = new StringBuilder();
                    limitSql.AppendLine("select distinct MemberID from TR_Mem_MemberLimit ");
                    var limlst = db.TM_AUTH_DataLimit.Where(p => p.HierarchyType == "role" && rid.Contains(p.HierarchyValue)
                                 && (p.PageID == 9999 || p.PageID == pid)).ToList();
                    if (limlst.Count > 0)
                    {
                        string limwhere = "";
                        foreach (var item in limlst)
                        {
                            if (limwhere == "")
                                limwhere = "where " + item.RangeType + " = '" + item.RangeValue + "'";
                            else
                                limwhere += " or " + item.RangeType + " = '" + item.RangeValue + "'";
                        }
                        limitSql.AppendLine(limwhere);
                    }
                    var l = db.Database.SqlQuery<string>(limitSql.ToString(), DBNull.Value).ToList();

                    var query = (from a in db.V_U_TM_Mem_Info.Where(p => p.StrMem_bk_5 == "1")
                                 join b in db.TM_Mem_Account on a.MemberID equals b.MemberID
                                 join o in db.TD_SYS_BizOption.Where(p => p.OptionType == "CustomerStatus") on a.CustomerStatus equals o.OptionValue
                                 join level in db.V_M_TM_SYS_BaseData_customerlevel on a.CustomerLevel equals level.CustomerLevelBase
                                 join actmp1 in db.TL_Mem_AccountChange.Where(p => p.AccountChangeType == "init" && p.ChangeReason == "老会员现金") on a.MemberID equals actmp1.MemberID into act1
                                 from ac1 in act1.DefaultIfEmpty()
                                 join actmp2 in db.TL_Mem_AccountChange.Where(p => p.AccountChangeType == "init" && p.ChangeReason == "老会员赠送") on a.MemberID equals actmp2.MemberID into act2
                                 from ac2 in act2.DefaultIfEmpty()
                                 where l.Contains(a.MemberID)
                                 select new
                                 {
                                     MemberCardNo = a.MemberCardNo,
                                     CustomerName = a.CustomerName,
                                     CustomerMobile = a.Mobile,
                                     StatusChange = a.CustomerStatus,
                                     CustomerStatus = o.OptionText,
                                     MemberID = a.MemberID,
                                     CustomerLevel = level.CustomerLevelNameBase,
                                     NoInvoiceAccount = b.NoInvoiceAccount,
                                     OldTotalValue = a.DecLoy_bk_3,
                                     OverRechargeValue = ac1.ChangeValue == null ? 0 : ac1.ChangeValue,
                                     OverSendValue = ac2.ChangeValue == null ? 0 : ac2.ChangeValue,
                                     a.AddedDate
                                 }).ToList();
                    if (!string.IsNullOrEmpty(memNo)) query = query.Where(p => p.MemberCardNo.Contains(memNo)).ToList();
                    if (!string.IsNullOrEmpty(memName)) query = query.Where(p => p.CustomerName.Contains(memName)).ToList();
                    if (!string.IsNullOrEmpty(memMobile)) query = query.Where(p => p.CustomerMobile.Contains(memMobile)).ToList();
                    if (!string.IsNullOrEmpty(status)) query = query.Where(p => p.StatusChange == status).ToList();
                    if (!string.IsNullOrEmpty(startDate))
                    {
                        DateTime parseDateStart = DateTime.Parse(startDate);
                        query = query.Where(p => p.AddedDate >= parseDateStart).ToList();
                    }
                    if (!string.IsNullOrEmpty(endDate))
                    {
                        DateTime parseDateEnd = DateTime.Parse(endDate).AddDays(1);
                        query = query.Where(p => p.AddedDate <= parseDateEnd).ToList();
                    }
                    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });

                }
                else
                {
                    var query = (from a in db.V_U_TM_Mem_Info.Where(p => p.StrMem_bk_5 == "1")
                                 join b in db.TM_Mem_Account on a.MemberID equals b.MemberID
                                 join o in db.TD_SYS_BizOption.Where(p => p.OptionType == "CustomerStatus") on a.CustomerStatus equals o.OptionValue
                                 join level in db.V_M_TM_SYS_BaseData_customerlevel on a.CustomerLevel equals level.CustomerLevelBase
                                 join actmp1 in db.TL_Mem_AccountChange.Where(p => p.AccountChangeType == "init" && p.ChangeReason == "老会员现金") on a.MemberID equals actmp1.MemberID into act1
                                 from ac1 in act1.DefaultIfEmpty()
                                 join actmp2 in db.TL_Mem_AccountChange.Where(p => p.AccountChangeType == "init" && p.ChangeReason == "老会员赠送") on a.MemberID equals actmp2.MemberID into act2
                                 from ac2 in act2.DefaultIfEmpty()
                                 select new
                                 {
                                     MemberCardNo = a.MemberCardNo,
                                     CustomerName = a.CustomerName,
                                     CustomerMobile = a.Mobile,
                                     StatusChange = a.CustomerStatus,
                                     CustomerStatus = o.OptionText,
                                     MemberID = a.MemberID,
                                     CustomerLevel = level.CustomerLevelNameBase,
                                     NoInvoiceAccount = b.NoInvoiceAccount,
                                     OldTotalValue = a.DecLoy_bk_3,
                                     OverRechargeValue = ac1.ChangeValue == null ? 0 : ac1.ChangeValue,
                                     OverSendValue = ac2.ChangeValue == null ? 0 : ac2.ChangeValue,
                                     a.AddedDate
                                 }).ToList();
                    if (!string.IsNullOrEmpty(memNo)) query = query.Where(p => p.MemberCardNo.Contains(memNo)).ToList();
                    if (!string.IsNullOrEmpty(memName)) query = query.Where(p => p.CustomerName.Contains(memName)).ToList();
                    if (!string.IsNullOrEmpty(memMobile)) query = query.Where(p => p.CustomerMobile.Contains(memMobile)).ToList();
                    if (!string.IsNullOrEmpty(status)) query = query.Where(p => p.StatusChange == status).ToList();
                    if (!string.IsNullOrEmpty(startDate))
                    {
                        DateTime parseDateStart = DateTime.Parse(startDate);
                        query = query.Where(p => p.AddedDate >= parseDateStart).ToList();
                    }
                    if (!string.IsNullOrEmpty(endDate))
                    {
                        DateTime parseDateEnd = DateTime.Parse(endDate).AddDays(1);
                        query = query.Where(p => p.AddedDate <= parseDateEnd).ToList();
                    }
                    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });



                }


            }
        }


        public static Result GetOldMemActInfo(string mid)
        {
            using (var db = new CRMEntities())
            {
                var total = db.TM_Loy_MemExt.Where(p => p.MemberID == mid).FirstOrDefault();//累计充值金额
                var act = db.TM_Mem_Account.Where(p => p.MemberID == mid && p.AccountType == "1").FirstOrDefault();//未开票金额，剩余赠送金币和赠送金币
                var actchange = db.TL_Mem_AccountChange.Where(p => p.MemberID == mid && p.AccountChangeType == "init").ToList();
                var recharge = actchange.Where(p => p.ChangeReason == "老会员现金").FirstOrDefault();
                var send = actchange.Where(p => p.ChangeReason == "老会员赠送").FirstOrDefault();
                ActInfo actinfo = new ActInfo()
                {
                    TotalAct = total.Dec_Attr_23,
                    NoInvoiceAct = act.NoBackAccount == null ? 0 : act.NoBackAccount,
                    RechargeAct = recharge.ChangeValue,
                    SendAct = send.ChangeValue,
                };
                return new Result(true, "", actinfo);
            }
        }
        #endregion

        #region 开票财务审核
        public static Result GetInvoice_Finance(string startDate, string endDate, string status, string dp, string rds, int pid)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                List<int> rid = JsonHelper.Deserialize<List<int>>(rds);

                var query = from a in db.V_M_TM_Mem_Trade_invoice//.Where(p => p.StatusInvoice == "1")
                            join b in db.V_U_TM_Mem_Info on a.MemberID equals b.MemberID
                            join c in db.TD_SYS_BizOption.Where(p => p.OptionType == "MemberType") on b.CustomerStatus equals c.OptionValue into cc
                            from cca in cc.DefaultIfEmpty()
                            //join e in db.V_M_TM_Mem_SubExt_vehicle on a.MemberID equals e.MemberID
                            join f in db.V_M_TM_SYS_BaseData_store.FilterDataByAuth(rid, pid, 1) on a.StoreCodeInovice equals f.StoreCode
                            select new
                            {
                                a.InvoiceDrawer,
                                b.CustomerName,
                                b.CustomerMobile,
                                a.TradeID,
                                a.MemberID,
                                a.TradeCode,
                                StoreName = f.StoreName,
                                a.MobileNumberInvoice,
                                a.InvoiceDate,//开票日期
                                a.StatusInvoice,//审批状态
                                a.AmountInvoice,
                                b.MemberCardNo,//会员卡号
                                Memtype = cca.OptionText,//个人还是企业
                                a.AddedDate,
                                a.InvoiceOrderNo,
                                //e.PlateNumVehicle,
                            };
                if (!string.IsNullOrEmpty(startDate))
                {
                    DateTime parseDateStart = DateTime.Parse(startDate);
                    query = query.Where(p => p.AddedDate >= parseDateStart);
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    DateTime parseDateEnd = DateTime.Parse(endDate).AddDays(1);
                    query = query.Where(p => p.AddedDate <= parseDateEnd);
                }
                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(p => p.StatusInvoice == status);
                }
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });

            }
        }
        public static Result GetInvoiceGroup_Finance(string startDate, string endDate, string status, string dp, string rds, int pid)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                List<int> rid = JsonHelper.Deserialize<List<int>>(rds);

                var query = from a in db.V_M_TM_Mem_Trade_invoice//.Where(p => p.StatusInvoice == "1")
                            join b in db.V_U_TM_Mem_Info on a.MemberID equals b.MemberID
                            join c in db.TD_SYS_BizOption.Where(p => p.OptionType == "MemberType") on b.CustomerStatus equals c.OptionValue into cc
                            from cca in cc.DefaultIfEmpty()
                            //join e in db.V_M_TM_Mem_SubExt_vehicle on a.MemberID equals e.MemberID
                            join f in db.V_M_TM_SYS_BaseData_store.FilterDataByAuth(rid, pid, 1) on a.StoreCode2Inovice equals f.StoreCode
                            where a.InvoiceDrawer == "集团"
                            select new
                            {
                                a.InvoiceDrawer,
                                b.CustomerName,
                                b.CustomerMobile,
                                a.TradeID,
                                a.MemberID,
                                a.TradeCode,
                                StoreName = f.StoreName,
                                a.MobileNumberInvoice,
                                a.InvoiceDate,//开票日期
                                a.StatusInvoice,//审批状态
                                a.AmountInvoice,
                                b.MemberCardNo,//会员卡号
                                Memtype = cca.OptionText,//个人还是企业
                                a.AddedDate,
                                a.InvoiceOrderNo,
                                // e.PlateNumVehicle,
                            };
                if (!string.IsNullOrEmpty(startDate))
                {
                    DateTime parseDateStart = DateTime.Parse(startDate);
                    query = query.Where(p => p.AddedDate >= parseDateStart);
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    DateTime parseDateEnd = DateTime.Parse(endDate).AddDays(1);
                    query = query.Where(p => p.AddedDate <= parseDateEnd);
                }
                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(p => p.StatusInvoice == status);
                }
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });

            }
        }

        public static Result UpdateInvoiceStatus(long tid)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var ext = db.TM_Mem_Trade.Where(p => p.TradeID == tid).FirstOrDefault();
                var acc = db.TM_Mem_Account.Where(p => p.MemberID == ext.MemberID).FirstOrDefault();
                if (acc.NoBackAccount < ext.Dec_Attr_1)
                    return new Result(false, "开票金额大于未开票金额,请检查");
                acc.NoBackAccount -= ext.Dec_Attr_1;
                if (string.IsNullOrEmpty(ext.Str_Attr_14))
                    return new Result(false, "请在编辑中填写开票单号");
                if (ext != null)
                {
                    ext.Str_Attr_15 = "2";
                    db.SaveChanges();
                    return new Result(true, "审批成功");
                }
                else
                {
                    return new Result(false, "审批失败");
                }
            }

        }

        #endregion


        #region 体验券核销
        public static Result GetCouponListByMid(string mid, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var dbRes = from a in db.TM_Mem_CouponPool
                            join b in db.TM_Act_CommunicationTemplet on a.TempletID equals b.TempletID
                            orderby a.AddedDate
                            select new
                            {
                                a.CouponID,
                                a.CouponCode,
                                a.TempletID,
                                a.StartDate,
                                a.EndDate,
                                a.Enable,
                                a.IsUsed,
                                a.Counts,
                                b.Name
                            };
                return new Result(true, "", new List<object> { dbRes.ToDataTableSourceVsPage(myDp) });
            }
        }

        public static Result UsingCoupon(long cid)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var ext = db.TM_Mem_CouponPool.Where(p => p.CouponID == cid).FirstOrDefault();
                if (ext != null && ext.IsUsed == false)
                {
                    ext.IsUsed = true;
                    db.SaveChanges();
                    return new Result(true, "使用成功");
                }
                else
                {
                    return new Result(false, "使用失败");
                }
            }

        }

        #endregion

        #region 导出360列表
        public static Result ExportMember360List(int pid, string rds, string tablename, string aliaskey, string aliassubkey, int pageid, string blocktype, string dp, string con)
        {

            var input = TransInputNew(tablename, aliaskey, aliassubkey, pageid, blocktype, con);
            DataTable dt = Member360AutoSearch(input, pid, rds);
            //dt转化为datatable使用
            if (dt != null)
            {
                DataTable dt2 = new DataTable();


                dt2 = dt;


                List<object> list = new List<object>();

                //遍历DataTable中所有的数据行
                foreach (DataRow dr in dt2.Rows)
                {
                    //类所在的namespace
                    Type type = typeof(Arvato.CRM.Model.ExportMember360Model);
                    var t = Activator.CreateInstance(type);

                    System.Reflection.PropertyInfo[] propertys = t.GetType().GetProperties();

                    for (var i = 0; i < propertys.Count(); i++)
                    {
                        object value = dr[i + 1];

                        Type tmpType = Nullable.GetUnderlyingType(propertys[i].PropertyType) ?? propertys[i].PropertyType;
                        object safeValue = (value == null) ? null : Convert.ChangeType(value, tmpType);

                        //如果非空，则赋给对象的属性  PropertyInfo
                        if (safeValue != DBNull.Value)
                        {
                            propertys[i].SetValue(t, safeValue, null);
                        }

                    }

                    //对象添加到泛型集合中
                    list.Add(t);
                }



                return new Result(true, "", new List<object> { list });
            }
            else
            {
                return new Result(true, "", null);
            }
        }

        #endregion

        #region 异常购物
        /// <summary>
        /// 列表页
        /// </summary>
        /// <param name="tradeId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static Result AbnormalTradeList(string tradeId, string page)
        {
            DatatablesParameter PageSize = JsonHelper.Deserialize<DatatablesParameter>(page);
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_Mem_AbnormalTrade.Select(n => new { Id = n.ID, TradeId = n.TradeID, Mobile = n.Mobile, StoreName = from t in db.V_M_TM_SYS_BaseData_store where t.StoreCode == n.StoreCode select t.StoreName, CustomerName = from v in db.V_M_TM_SYS_BaseData_store where n.CustomerName == v.ChannelCodeStore select v.ChannelNameStore, MemberID = n.MemberID, CreateTime = n.AddedDate });
                if (string.IsNullOrWhiteSpace(tradeId) == false)
                {
                    long tradeIDParam = Convert.ToInt64(tradeId);
                    query = query.Where(n => n.TradeId == tradeIDParam);
                }
                return new Result(true, "", query.ToDataTableSourceVsPage(PageSize));
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <returns></returns>
        public static Result AbnormalTradeToExcel()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var list = db.TM_Mem_AbnormalTrade.ToList();
                return new Result(true, "", list);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Result DeleteTrade(string jsonParam, int userId)
        {
            List<DeleteAbnormalTrade> lt = JsonHelper.Deserialize<List<DeleteAbnormalTrade>>(jsonParam);
            using (CRMEntities db = new CRMEntities())
            {
                foreach (var item in lt)
                {
                    long id = Convert.ToInt64(item.Id);
                    var query = db.TM_Mem_AbnormalTrade.Where(n => n.ID == id).FirstOrDefault();
                    db.TM_Mem_AbnormalTrade.Remove(query);
                }
                try
                {
                    db.SaveChanges();
                    return new Result(true, "删除成功");
                }
                catch (Exception ex)
                {
                    Log4netHelper.WriteErrorLog("DeleteTrade:" + ex.Message.ToString());
                    return new Result(false, "删除失败");
                    throw;
                }
            }

        }
        #endregion
    }
}
