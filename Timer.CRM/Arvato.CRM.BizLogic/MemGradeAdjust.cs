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
    public class MemGradeAdjust
    {
        #region 会员等级调整列表
        public static Result GetMemGradeAdjust(int dataGroupId, string strRole, string dp, string vipCode, string name, string mobileNO, string plateNO)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            List<int> roleid = JsonHelper.Deserialize<List<int>>(strRole);
            using (CRMEntities db = new CRMEntities())
            {
                var vgrouprelationId = db.V_Sys_DataGroupRelation.Where(t => t.DataGroupID == dataGroupId).Select(t => t.SubDataGroupID).ToList();

                var query = from a in db.V_S_TM_Mem_Ext
                            join c in db.V_S_TM_Mem_Master on a.MemberID equals c.MemberID
                            join b in db.TM_SYS_DataGroup on c.DataGroupID equals b.DataGroupID
                            join d in db.TD_SYS_BizOption.Where(o => o.DataGroupID == dataGroupId) on c.CustomerLevel equals d.OptionValue
                            where vgrouprelationId.Contains(c.DataGroupID)
                            select new
                            {
                                b.DataGroupName,
                                a.MemberID,
                                a.MemberCardNo,
                                a.CustomerName,
                                a.CustomerMobile,
                                d.OptionText,
                                a.MemberLevelStartDate,
                                a.MemberLevelEndDate
                            };

                if (!string.IsNullOrWhiteSpace(vipCode))
                {
                    query = query.Where(p => p.MemberCardNo.Contains(vipCode));
                }
                if (!string.IsNullOrWhiteSpace(name))
                {
                    query = query.Where(p => p.CustomerName.Contains(name));
                }
                if (!string.IsNullOrWhiteSpace(mobileNO))
                {
                    query = query.Where(p => p.CustomerMobile.Contains(mobileNO));
                }

                //if (!string.IsNullOrWhiteSpace(plateNO))
                //{
                //    var query_MemID = from m in db.V_M_TM_Mem_SubExt_vehicle.Where(p => p.CarNo.Contains(plateNO))
                //                      select new
                //                      {
                //                          m.MemberID
                //                      };
                //    var query_res = query_MemID.Distinct().ToList();
                //    List<string> lst = new List<string>();
                //    foreach (var item in query_res)
                //    {
                //        lst.Add(item.MemberID);
                //    }

                //    query = query.Where(p => lst.Contains(p.MemberID));
                //}

                var res = query.Distinct();
                if (roleid != null)
                {
                    List<string> rlist = roleid.Select(r => r.ToString()).ToList();

                    //var queryLimit = (from l in db.TM_AUTH_DataLimit
                    //                  where l.HierarchyType == "role" && l.RangeType == "store" && rlist.Contains(l.HierarchyValue)
                    //                  select l.RangeValue).Union(
                    //                 from l in db.TM_AUTH_DataLimit
                    //                 join s in db.V_M_TM_SYS_BaseData_store on l.RangeValue equals s.StoreBrandCode
                    //                 where l.HierarchyType == "role" && l.RangeType == "brand" && rlist.Contains(l.HierarchyValue)
                    //                 select s.StoreCode
                    //                 );

                    //List<string> storeList = queryLimit.ToList();
                    //if (storeList != null && storeList.Count > 0)
                    //{
                    //    var result = from a in query
                    //                 join b in db.TR_Mem_StoreCode on a.MemberID equals b.MemberID
                    //                 where queryLimit.Contains(b.StoreCode)
                    //                 select new
                    //                 {
                    //                     a.DataGroupName,
                    //                     a.MemberID,
                    //                     a.MemberCardNo,
                    //                     a.CustomerName,
                    //                     a.CustomerMobile,
                    //                     a.OptionText,
                    //                     a.MemberLevelStartDate,
                    //                     a.MemberLevelEndDate
                    //                 };
                    //    return new Result(true, "", new List<object> { result.ToDataTableSourceVsPage(myDp) });
                    //}
                }
                return new Result(true, "", new List<object> { res.ToDataTableSourceVsPage(myDp) });
            }
        }
        #endregion

        public static Result GetItemById(string itemId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.V_S_TM_Mem_Ext
                            join c in db.V_S_TM_Mem_Master on a.MemberID equals c.MemberID
                            join d in db.TD_SYS_BizOption on c.CustomerLevel equals d.OptionValue
                            where a.MemberID == itemId
                            select new
                            {
                                a.MemberCardNo,
                                a.CustomerName,
                                a.CustomerMobile,
                                d.OptionText,
                                c.CustomerLevel,
                                MemberLevelStartDate = a.MemberLevelStartDate == null ? SqlFunctions.GetDate() : a.MemberLevelStartDate,
                                MemberLevelEndDate = a.MemberLevelEndDate == null ? SqlFunctions.DateAdd("year", 1, SqlFunctions.GetDate()) : a.MemberLevelEndDate,
                            };
                var res = query.FirstOrDefault();

                return new Result(true, "", new List<object> { res });
            }
        }

        public static Result UpdateVipLevelData(string befGrade, DateTime? befStartDate, DateTime? befEndDate, string vipCode, string vipLevel, string updateUser, DateTime? startDate, DateTime? endDate, string reason)
        {
            using (CRMEntities db = new CRMEntities())
            {
                try
                {
                    //更新会员卡号起始日期
                    var query = db.TM_Mem_Ext.Where(p => p.MemberID == vipCode).FirstOrDefault();
                    if (query != null)
                    {
                        query.Date_Attr_62 = startDate;
                        query.Date_Attr_63 = endDate;
                        query.ModifiedDate = DateTime.Now;
                        query.ModifiedUser = updateUser;
                        db.Entry(query).State = EntityState.Modified;
                    }
                    else
                    {
                        return new Result(false, "会员等级调整失败");
                    }

                    //更新会员等级
                    var queryLevel = db.TM_Mem_Master.Where(p => p.MemberID == vipCode).FirstOrDefault();
                    if (queryLevel != null)
                    {
                        queryLevel.MemberLevel = vipLevel;
                        queryLevel.ModifiedDate = DateTime.Now;
                        queryLevel.ModifiedUser = updateUser;
                        db.Entry(queryLevel).State = EntityState.Modified;
                    }
                    else
                    {
                        return new Result(false, "会员等级调整失败");
                    }

                    //添加日志记录信息
                    Arvato.CRM.EF.TL_Mem_LevelChange entity = new TL_Mem_LevelChange();
                    entity.MemberID = vipCode;
                    entity.ChangeLevelFrom = befGrade;
                    entity.ChangeLevelTo = vipLevel;
                    if (befStartDate != null)
                        entity.StartDateFrom = befStartDate;
                    entity.StartDateTo = startDate;
                    if (befEndDate != null)
                        entity.EndDateFrom = befEndDate;
                    entity.EndDateTo = endDate;
                    entity.LevelChangeType = "manu";
                    entity.ChangeReason = reason;
                    entity.AddedDate = DateTime.Now;
                    entity.AddedUser = updateUser;

                    db.TL_Mem_LevelChange.Add(entity);
                    db.SaveChanges();
                    return new Result(true, "会员等级调整成功");
                }
                catch (Exception ex)
                {
                    return new Result(false, ex.Message);
                }
            }
        }
    }
}
