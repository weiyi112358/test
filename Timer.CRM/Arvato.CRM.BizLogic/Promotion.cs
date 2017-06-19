using Arvato.CRM.EF;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;

namespace Arvato.CRM.BizLogic
{
    public static class Promotion
    {
        ///// <summary>
        ///// 获取促销活动
        ///// </summary>
        ///// <returns></returns>
        //public static Result GetPromotion(string promotionname, string dp)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    promotionname = string.IsNullOrEmpty(promotionname) ? "" : promotionname;
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        var querydata = from p in db.V_M_TM_SYS_BaseData_promotion//获取促销活动
        //                        where p.PromotionName.Contains(promotionname) 
        //                        orderby p.BaseDataID descending
        //                        select p;
        //        return new Result(true, "", new List<object> { querydata.ToDataTableSourceVsPage(myDp) });
        //    }
        //}

        ///// <summary>
        ///// 获取促销活动
        ///// </summary>
        ///// <returns></returns>
        //public static Result GetPromotions(long? baseDataID, string promotionID, string promotionCode, string promotionName, string promotionIsEnd,
        //    DateTime? startDate, DateTime? endDate, int dataGroupID)
        //{
        //    try
        //    {
        //        using (CRMEntities db = new CRMEntities())
        //        {
        //            var query = from p in db.V_M_TM_SYS_BaseData_promotion//获取促销活动
        //                        orderby p.BaseDataID descending
        //                        where p.DataGroupID == dataGroupID
        //                        select p;
        //            if (baseDataID.HasValue)
        //            {
        //                query = query.Where(p => p.BaseDataID == baseDataID.Value);
        //            }
        //            if (string.IsNullOrEmpty(promotionID))
        //            {
        //                query = query.Where(p => p.PromotionID.Trim().Equals(promotionID.Trim()));
        //            }
        //            if (string.IsNullOrEmpty(promotionCode))
        //            {
        //                query = query.Where(p => p.PromotionCode.Trim().Equals(promotionCode.Trim()));
        //            }
        //            if (string.IsNullOrEmpty(promotionName))
        //            {
        //                query = query.Where(p => p.PromotionName.Trim().Contains(promotionName.Trim()));
        //            }
        //            if (string.IsNullOrEmpty(promotionIsEnd))
        //            {
        //                query = query.Where(p => p.PromotionIsEnd.Trim().Equals(promotionIsEnd.Trim()));
        //            }
        //            if (startDate.HasValue)
        //            {
        //                query = query.Where(p => p.StartDatePromotion.HasValue ? (p.StartDatePromotion.Value >= startDate.Value) : true);
        //            }
        //            if (endDate.HasValue)
        //            {
        //                query = query.Where(p => p.EndDatePromotion.HasValue ? (p.EndDatePromotion.Value <= endDate.Value) : true);
        //            }
        //            return new Result(true, "", new List<object> { query.ToList() });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Result(false, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// 获取当前可用的促销活动
        ///// </summary>
        ///// <returns></returns>
        //public static Result GetAvaiablePromotion(int dataGroupID)
        //{
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        var query = from p in db.V_M_TM_SYS_BaseData_promotion
        //                    orderby p.BaseDataID descending
        //                    where p.PromotionIsEnd.Trim().Equals("0")
        //                        && (p.StartDatePromotion.HasValue ? DateTime.Now >= p.StartDatePromotion.Value : true)
        //                        && (p.EndDatePromotion.HasValue ? DateTime.Now <= p.EndDatePromotion.Value : true)
        //                        && p.DataGroupID == dataGroupID
        //                    select p;
        //        return new Result(true, "", new List<object> { query.ToList() });
        //    }
        //}

        //public static Result GetPromotionWithSysCommonByKey(string key, string type, string promotionIsEnd, bool isValidDate, int dataGroupID,string dp)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    try
        //    {
        //        using (CRMEntities db = new CRMEntities())
        //        {
        //            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(key))
        //            {
        //                return new Result(false, "填写信息不完整");
        //            }

        //            switch (type.Trim())
        //            {
        //                case "1"://会员细分
        //                    var querySubdivison = from p in db.V_M_TM_SYS_BaseData_promotion
        //                                          join c in db.TR_SYS_Common
        //                                          on new
        //                                          {
        //                                              RelationType = "1",
        //                                              RelationDataType = "2",
        //                                              PrmotionID = p.BaseDataID,
        //                                              Key = new Guid(key.Trim())
        //                                          } equals new
        //                                           {
        //                                               RelationType = c.RelationType.Trim(),
        //                                               RelationDataType = c.RelationDataType.Trim(),
        //                                               PrmotionID = c.RelationBigintValue1,
        //                                               Key = c.RelationGuidValue1
        //                                           }
        //                                          into pcTemp
        //                                          from pc in pcTemp.DefaultIfEmpty()
        //                                          join s in db.TM_Mem_Subdivision
        //                                          on new
        //                                          {
        //                                              RelationType = pc.RelationType.Trim(),
        //                                              RelationDataType = pc.RelationDataType.Trim(),
        //                                              SubdivisionID = pc.RelationGuidValue1,
        //                                              DataGroupID = dataGroupID
        //                                          } equals new
        //                                          {
        //                                              RelationType = "1",
        //                                              RelationDataType = "2",
        //                                              SubdivisionID = s.SubdivisionID,
        //                                              DataGroupID = s.DataGroupID
        //                                          }
        //                                          into psTemp
        //                                          from ps in psTemp.DefaultIfEmpty()
        //                                          orderby p.BaseDataID descending
        //                                          where p.DataGroupID == dataGroupID
        //                                          select new
        //                                          {
        //                                              p.BaseDataID,
        //                                              p.BaseDataType,
        //                                              p.DataGroupID,
        //                                              p.StartDatePromotion,
        //                                              p.EndDatePromotion,
        //                                              p.PromotionBillType,
        //                                              p.PromotionCode,
        //                                              p.PromotionID,
        //                                              p.PromotionIsEnd,
        //                                              p.PromotionName,
        //                                              p.PromotionRemark,
        //                                              p.PromotionType,
        //                                              StartDate = pc.StartDate,
        //                                              EndDate = pc.EndDate,
        //                                              Name = ps.SubdivisionName,
        //                                              Key = ps == null ? new Guid("00000000-0000-0000-0000-000000000000") : ps.SubdivisionID
        //                                          };
        //                    if (string.IsNullOrEmpty(promotionIsEnd))
        //                    {
        //                        querySubdivison = querySubdivison.Where(p => p.PromotionIsEnd.Trim().Equals(promotionIsEnd.Trim()));
        //                    }
        //                    if (isValidDate)
        //                    {
        //                        querySubdivison = querySubdivison.Where(p => (p.StartDatePromotion.HasValue ? p.StartDatePromotion <= DateTime.Now : true)
        //                            && (p.EndDatePromotion.HasValue ? p.EndDatePromotion >= DateTime.Now : true));
        //                    }
        //                    return new Result(true, "", new List<object> { querySubdivison.ToDataTableSource() });
        //                case "2"://商业计划
        //                    var queryBusinessPlan = from p in db.V_M_TM_SYS_BaseData_promotion
        //                                            join c in db.TR_SYS_Common
        //                                            on new
        //                                            {
        //                                                RelationType = "2",
        //                                                RelationDataType = "3",
        //                                                PrmotionID = p.BaseDataID,
        //                                                Key = key.Trim()
        //                                            } equals new
        //                                            {
        //                                                RelationType = c.RelationType.Trim(),
        //                                                RelationDataType = c.RelationDataType.Trim(),
        //                                                PrmotionID = c.RelationBigintValue1,
        //                                                Key = c.ReferenceStringValue1.Trim()
        //                                            }
        //                                            into pcTemp
        //                                            from pc in pcTemp.DefaultIfEmpty()
        //                                            join b in db.TM_CRM_BusinessPlan
        //                                            on new
        //                                            {
        //                                                RelationType = pc.RelationType.Trim(),
        //                                                RelationDataType = pc.RelationDataType.Trim(),
        //                                                BusinessPlanID = pc.ReferenceStringValue1.Trim(),
        //                                                DataGroupID = dataGroupID
        //                                            } equals new
        //                                            {
        //                                                RelationType = "2",
        //                                                RelationDataType = "3",
        //                                                BusinessPlanID = b.BusinessPlanID.Trim(),
        //                                                DataGroupID = b.DataGroupID
        //                                            }
        //                                            into pbTemp
        //                                            from pb in pbTemp.DefaultIfEmpty()
        //                                            orderby p.BaseDataID descending
        //                                            where p.DataGroupID == dataGroupID
        //                                            select new
        //                                            {
        //                                                p.BaseDataID,
        //                                                p.BaseDataType,
        //                                                p.DataGroupID,
        //                                                p.StartDatePromotion,
        //                                                p.EndDatePromotion,
        //                                                p.PromotionBillType,
        //                                                p.PromotionCode,
        //                                                p.PromotionID,
        //                                                p.PromotionIsEnd,
        //                                                p.PromotionName,
        //                                                p.PromotionRemark,
        //                                                p.PromotionType,
        //                                                StartDate = pc.StartDate,
        //                                                EndDate = pc.EndDate,
        //                                                Name = pb.BusinessPlanName,
        //                                                Key = pb.BusinessPlanID
        //                                            };
        //                    if (string.IsNullOrEmpty(promotionIsEnd))
        //                    {
        //                        queryBusinessPlan = queryBusinessPlan.Where(p => p.PromotionIsEnd.Trim().Equals(promotionIsEnd.Trim()));
        //                    }
        //                    if (isValidDate)
        //                    {
        //                        queryBusinessPlan = queryBusinessPlan.Where(p => (p.StartDatePromotion.HasValue ? p.StartDatePromotion <= DateTime.Now : true)
        //                            && (p.EndDatePromotion.HasValue ? p.EndDatePromotion >= DateTime.Now : true));
        //                    }
        //                    return new Result(true, "", new List<object> { queryBusinessPlan.ToDataTableSource() });
        //                case "3"://市场活动
        //                    var queryActivity = from p in db.V_M_TM_SYS_BaseData_promotion
        //                                        join c in db.TR_SYS_Common
        //                                        on new
        //                                        {
        //                                            RelationType = "3",
        //                                            RelationDataType = "1",
        //                                            PrmotionID = p.BaseDataID,
        //                                            Key = key.Trim()
        //                                        } equals new
        //                                        {
        //                                            RelationType = c.RelationType.Trim(),
        //                                            RelationDataType = c.RelationDataType.Trim(),
        //                                            PrmotionID = c.RelationBigintValue1,
        //                                            Key = SqlFunctions.StringConvert((double)c.RelationBigintValue2).Trim()
        //                                        }
        //                                        into pcTemp
        //                                        from pc in pcTemp.DefaultIfEmpty()
        //                                        join a in db.TM_Act_Master
        //                                        on new
        //                                        {
        //                                            RelationType = pc.RelationType.Trim(),
        //                                            RelationDataType = pc.RelationDataType.Trim(),
        //                                            ActivityID = pc.RelationBigintValue2,
        //                                            DataGroupID = dataGroupID
        //                                        } equals new
        //                                        {
        //                                            RelationType = "3",
        //                                            RelationDataType = "1",
        //                                            ActivityID = (long)a.ActivityID,
        //                                            DataGroupID = a.DataGroupID
        //                                        }
        //                                        into paTemp
        //                                        from pa in paTemp.DefaultIfEmpty()
        //                                        orderby p.BaseDataID descending
        //                                        where p.DataGroupID == dataGroupID
        //                                        select new
        //                                        {
        //                                            p.BaseDataID,
        //                                            p.BaseDataType,
        //                                            p.DataGroupID,
        //                                            p.StartDatePromotion,
        //                                            p.EndDatePromotion,
        //                                            p.PromotionBillType,
        //                                            p.PromotionCode,
        //                                            p.PromotionID,
        //                                            p.PromotionIsEnd,
        //                                            p.PromotionName,
        //                                            p.PromotionRemark,
        //                                            p.PromotionType,
        //                                            StartDate = pc.StartDate,
        //                                            EndDate = pc.EndDate,
        //                                            Name = pa.ActivityName,
        //                                            Key = pa == null ? -1 : pa.ActivityID
        //                                        };
        //                    if (string.IsNullOrEmpty(promotionIsEnd))
        //                    {
        //                        queryActivity = queryActivity.Where(p => p.PromotionIsEnd.Trim().Equals(promotionIsEnd.Trim()));
        //                    }
        //                    if (isValidDate)
        //                    {
        //                        queryActivity = queryActivity.Where(p => (p.StartDatePromotion.HasValue? p.StartDatePromotion<= DateTime.Now :true)
        //                            && (p.EndDatePromotion.HasValue ? p.EndDatePromotion >= DateTime.Now : true));
        //                    }
        //                    return new Result(true, "", new List<object> { queryActivity.ToDataTableSource() });
        //                default:
        //                    return new Result(false, "无法识别的绑定促销类型");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Result(false, ex.Message);
        //    }
        //}

        //public static Result GetPromotionWithSysCommonByPromotionID(long promotionID, string type, int dataGroupID)
        //{
        //    try
        //    {
        //        using (CRMEntities db = new CRMEntities())
        //        {
        //            if (string.IsNullOrEmpty(type))
        //            {
        //                return new Result(false, "填写信息不完整");
        //            }

        //            switch (type.Trim())
        //            {
        //                case "1"://会员细分
        //                    var querySubdivison = from p in db.V_M_TM_SYS_BaseData_promotion
        //                                          join c in db.TR_SYS_Common
        //                                          on new
        //                                          {
        //                                              RelationType = "1",
        //                                              RelationDataType = "2",
        //                                              PrmotionID = p.BaseDataID
        //                                          } equals new
        //                                          {
        //                                              RelationType = c.RelationType.Trim(),
        //                                              RelationDataType = c.RelationDataType.Trim(),
        //                                              PrmotionID = c.RelationBigintValue1
        //                                          }
        //                                          into pcTemp
        //                                          from pc in pcTemp.DefaultIfEmpty()
        //                                          join s in db.TM_Mem_Subdivision
        //                                          on new
        //                                          {
        //                                              RelationType = pc.RelationType.Trim(),
        //                                              RelationDataType = pc.RelationDataType.Trim(),
        //                                              SubdivisionID = pc.RelationGuidValue1,
        //                                              DataGroupID = dataGroupID
        //                                          } equals new
        //                                          {
        //                                              RelationType = "1",
        //                                              RelationDataType = "2",
        //                                              SubdivisionID = s.SubdivisionID,
        //                                              DataGroupID = s.DataGroupID
        //                                          }
        //                                          into psTemp
        //                                          from ps in psTemp.DefaultIfEmpty()
        //                                          orderby p.BaseDataID descending
        //                                          where p.BaseDataID == promotionID
        //                                            && p.DataGroupID == dataGroupID
        //                                          select new
        //                                          {
        //                                              p.BaseDataID,
        //                                              p.BaseDataType,
        //                                              p.DataGroupID,
        //                                              p.StartDatePromotion,
        //                                              p.EndDatePromotion,
        //                                              p.PromotionBillType,
        //                                              p.PromotionCode,
        //                                              p.PromotionID,
        //                                              p.PromotionIsEnd,
        //                                              p.PromotionName,
        //                                              p.PromotionRemark,
        //                                              p.PromotionType,
        //                                              StartDate = pc.StartDate,
        //                                              EndDate = pc.EndDate,
        //                                              Name = ps.SubdivisionName,
        //                                              Key = ps == null ? new Guid("00000000-0000-0000-0000-000000000000") : ps.SubdivisionID
        //                                          };
        //                    return new Result(true, "", new List<object>(querySubdivison.ToList()));
        //                case "2"://商业计划
        //                    var queryBusinessPlan = from p in db.V_M_TM_SYS_BaseData_promotion
        //                                            join c in db.TR_SYS_Common
        //                                            on new
        //                                            {
        //                                                RelationType = "2",
        //                                                RelationDataType = "3",
        //                                                PrmotionID = p.BaseDataID
        //                                            } equals new
        //                                            {
        //                                                RelationType = c.RelationType.Trim(),
        //                                                RelationDataType = c.RelationDataType.Trim(),
        //                                                PrmotionID = c.RelationBigintValue1
        //                                            }
        //                                            into pcTemp
        //                                            from pc in pcTemp.DefaultIfEmpty()
        //                                            join b in db.TM_CRM_BusinessPlan
        //                                            on new
        //                                            {
        //                                                RelationType = pc.RelationType.Trim(),
        //                                                RelationDataType = pc.RelationDataType.Trim(),
        //                                                BusinessPlanID = pc.ReferenceStringValue1.Trim(),
        //                                                DataGroupID = dataGroupID
        //                                            } equals new
        //                                            {
        //                                                RelationType = "2",
        //                                                RelationDataType = "3",
        //                                                BusinessPlanID = b.BusinessPlanID.Trim(),
        //                                                DataGroupID = b.DataGroupID
        //                                            }
        //                                            into pbTemp
        //                                            from pb in pbTemp.DefaultIfEmpty()
        //                                            orderby p.BaseDataID descending
        //                                            where p.BaseDataID == promotionID
        //                                                && p.DataGroupID == dataGroupID
        //                                            select new
        //                                            {
        //                                                p.BaseDataID,
        //                                                p.BaseDataType,
        //                                                p.DataGroupID,
        //                                                p.StartDatePromotion,
        //                                                p.EndDatePromotion,
        //                                                p.PromotionBillType,
        //                                                p.PromotionCode,
        //                                                p.PromotionID,
        //                                                p.PromotionIsEnd,
        //                                                p.PromotionName,
        //                                                p.PromotionRemark,
        //                                                p.PromotionType,
        //                                                StartDate = pc.StartDate,
        //                                                EndDate = pc.EndDate,
        //                                                Name = pb.BusinessPlanName,
        //                                                Key = pb.BusinessPlanID
        //                                            };

        //                    return new Result(true, "", new List<object>(queryBusinessPlan.ToList()));
        //                case "3"://市场活动
        //                    var queryActivity = from p in db.V_M_TM_SYS_BaseData_promotion
        //                                        join c in db.TR_SYS_Common
        //                                        on new
        //                                        {
        //                                            RelationType = "3",
        //                                            RelationDataType = "1",
        //                                            PrmotionID = p.BaseDataID
        //                                        } equals new
        //                                        {
        //                                            RelationType = c.RelationType.Trim(),
        //                                            RelationDataType = c.RelationDataType.Trim(),
        //                                            PrmotionID = c.RelationBigintValue1
        //                                        }
        //                                        into pcTemp
        //                                        from pc in pcTemp.DefaultIfEmpty()
        //                                        join a in db.TM_Act_Master
        //                                        on new
        //                                        {
        //                                            RelationType = pc.RelationType.Trim(),
        //                                            RelationDataType = pc.RelationDataType.Trim(),
        //                                            ActivityID = pc.RelationBigintValue2,
        //                                            DataGroupID = dataGroupID
        //                                        } equals new
        //                                        {
        //                                            RelationType = "3",
        //                                            RelationDataType = "1",
        //                                            ActivityID = (long)a.ActivityID,
        //                                            DataGroupID = a.DataGroupID
        //                                        }
        //                                        into paTemp
        //                                        from pa in paTemp.DefaultIfEmpty()
        //                                        orderby p.BaseDataID descending
        //                                        where p.BaseDataID == promotionID
        //                                            && p.DataGroupID == dataGroupID
        //                                        select new
        //                                        {
        //                                            p.BaseDataID,
        //                                            p.BaseDataType,
        //                                            p.DataGroupID,
        //                                            p.StartDatePromotion,
        //                                            p.EndDatePromotion,
        //                                            p.PromotionBillType,
        //                                            p.PromotionCode,
        //                                            p.PromotionID,
        //                                            p.PromotionIsEnd,
        //                                            p.PromotionName,
        //                                            p.PromotionRemark,
        //                                            p.PromotionType,
        //                                            StartDate = pc.StartDate,
        //                                            EndDate = pc.EndDate,
        //                                            Name = pa.ActivityName,
        //                                            Key = pa == null ? -1 : pa.ActivityID
        //                                        };
        //                    return new Result(true, "", new List<object>(queryActivity.ToList()));
        //                default:
        //                    return new Result(false, "无法识别的绑定促销类型");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Result(false, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// 获取促销活动
        ///// </summary>
        ///// <returns></returns>
        //public static Result GetPromotionsForPage(long? baseDataID, string promotionID, string promotionCode, string promotionName, string promotionIsEnd,
        //    DateTime? startDate, DateTime? endDate, int dataGroupID, string pageInfo)
        //{
        //    try
        //    {
        //        DatatablesParameter page = JsonHelper.Deserialize<DatatablesParameter>(pageInfo);

        //        using (CRMEntities db = new CRMEntities())
        //        {
        //            var query = from p in db.V_M_TM_SYS_BaseData_promotion//获取促销活动
        //                        orderby p.BaseDataID descending
        //                        where p.DataGroupID == dataGroupID
        //                        select p;
        //            if (baseDataID.HasValue)
        //            {
        //                query = query.Where(p => p.BaseDataID == baseDataID.Value);
        //            }
        //            if (string.IsNullOrEmpty(promotionID))
        //            {
        //                query = query.Where(p => p.PromotionID.Trim().Equals(promotionID.Trim()));
        //            }
        //            if (string.IsNullOrEmpty(promotionCode))
        //            {
        //                query = query.Where(p => p.PromotionCode.Trim().Equals(promotionCode.Trim()));
        //            }
        //            if (string.IsNullOrEmpty(promotionName))
        //            {
        //                query = query.Where(p => p.PromotionName.Trim().Contains(promotionName.Trim()));
        //            }
        //            if (string.IsNullOrEmpty(promotionIsEnd))
        //            {
        //                query = query.Where(p => p.PromotionIsEnd.Trim().Equals(promotionIsEnd.Trim()));
        //            }
        //            if (startDate.HasValue)
        //            {
        //                query = query.Where(p => p.StartDatePromotion.HasValue ? (p.StartDatePromotion.Value >= startDate.Value) : true);
        //            }
        //            if (endDate.HasValue)
        //            {
        //                query = query.Where(p => p.EndDatePromotion.HasValue ? (p.EndDatePromotion.Value <= endDate.Value) : true);
        //            }
        //            return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(page) });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Result(false, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// 获取当前可用的促销活动分页
        ///// </summary>
        ///// <returns></returns>
        //public static Result GetAvaiablePromotionForPage(int dataGroupID, string pageInfo)
        //{
        //    try
        //    {
        //        DatatablesParameter page = JsonHelper.Deserialize<DatatablesParameter>(pageInfo);
        //        using (CRMEntities db = new CRMEntities())
        //        {
        //            var query = from p in db.V_M_TM_SYS_BaseData_promotion
        //                        orderby p.BaseDataID descending
        //                        where p.PromotionIsEnd.Trim().Equals("0")
        //                            && (p.StartDatePromotion.HasValue ? DateTime.Now >= p.StartDatePromotion.Value : true)
        //                            && (p.EndDatePromotion.HasValue ? DateTime.Now <= p.EndDatePromotion.Value : true)
        //                            && p.DataGroupID == dataGroupID
        //                        select p;
        //            return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(page) });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Result(false, ex.Message);
        //    }
        //}

        ///// <summary>
        ///// 获取会员细分
        ///// </summary>
        ///// <returns></returns>
        //public static Result GetMemSubdivision(string subdivisionname, int DataGroupID)
        //{
        //    subdivisionname = string.IsNullOrEmpty(subdivisionname) ? "" : subdivisionname;
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        var querydata = from p in db.TM_Mem_Subdivision//获取会员细分
        //                        where p.SubdivisionName.Contains(subdivisionname) && p.DataGroupID == DataGroupID
        //                        join q in db.TM_Mem_SubdivisionInstance
        //                        on p.SubdivisionID equals q.SubdivisionID into temp
        //                        from o in temp.DefaultIfEmpty()
        //                        select new { SubdivisionID = p.SubdivisionID, SubdivisionName = p.SubdivisionName, MemCount = o.MemberCount, AddedDate = p.AddedDate };
        //        return new Result(true, "", new List<object> { querydata.ToDataTableSource() });
        //    }
        //}

        ///// <summary>
        ///// 获取商业计划
        ///// </summary>
        ///// <param name="busplanname"></param>
        ///// <returns></returns>
        //public static Result GetBusinessPlan(string busplanname, int DataGroupID)
        //{
        //    busplanname = string.IsNullOrEmpty(busplanname) ? "" : busplanname;
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        var querydata = from p in db.TM_CRM_BusinessPlan//商业计划
        //                        where p.BusinessPlanName.Contains(busplanname) && p.DataGroupID == DataGroupID && p.Status == "1"
        //                        select new { BusinessPlanID = p.BusinessPlanID, BusinessPlanName = p.BusinessPlanName, AddedDate = p.AddedDate };
        //        return new Result(true, "", new List<object> { querydata.ToDataTableSource() });
        //    }
        //}

        ///// <summary>
        ///// 获取市场活动
        ///// </summary>
        ///// <param name="busplanname"></param>
        ///// <returns></returns>
        //public static Result GetMarkActive(string markactname, int DataGroupID)
        //{
        //    markactname = string.IsNullOrEmpty(markactname) ? "" : markactname;
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        var querydata = from p in db.TM_Act_Master//市场活动
        //                        where p.ActivityName.Contains(markactname) && p.DataGroupID == DataGroupID && p.Status == 0
        //                        select new { ActivityID = p.ActivityID, ActivityName = p.ActivityName, AddedDate = p.AddedDate };
        //        return new Result(true, "", new List<object> { querydata.ToDataTableSource() });
        //    }
        //}

        ///// <summary>
        ///// 获取所选活动所选取的商业计划
        ///// </summary>
        ///// <param name="promotionID"></param>
        ///// <returns>活动ID</returns>
        //public static Result GetSysCommonChecked(int promotionID, string type)
        //{

        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        var querydata = from p in db.TR_SYS_Common
        //                        where p.RelationType == type && p.RelationBigintValue1 == promotionID
        //                        select p;
        //        return new Result(true, "", new List<object> { querydata.ToDataTableSource() });
        //    }
        //}

        //public static Result GetBusinessPlanApproved(int promotionID, string dp)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        var querydata = from p in db.TR_SYS_Common
        //                        join bp in db.TM_CRM_BusinessPlan on p.ReferenceStringValue1 equals bp.BusinessPlanID into bptemp
        //                        from q in bptemp.DefaultIfEmpty()
        //                        where p.RelationBigintValue1 == promotionID && q.Status == "2"
        //                        select new
        //                        {
        //                            q.BusinessPlanName,
        //                        };
        //        return new Result(true, "", new List<object> { querydata.ToDataTableSourceVsPage(myDp) });
        //    }
        //}

        //public static Result GetMarkApproved(int promotionID, string dp)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        var querydata = from p in db.TR_SYS_Common
        //                        join bp in db.TM_Act_Master on p.RelationBigintValue2 equals bp.ActivityID into bptemp
        //                        from q in bptemp.DefaultIfEmpty()
        //                        where p.RelationBigintValue1 == promotionID && q.Status == 99
        //                        select new
        //                        {
        //                            q.ActivityName,
        //                        };
        //        return new Result(true, "", new List<object> { querydata.ToDataTableSourceVsPage(myDp) });
        //    }
        //}

        //public static Result GetSysCommonMarkByID(int actID) {
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        //var querydata = from p in db.TR_SYS_Common
        //        //                where p.RelationType=="3"&&p.RelationBigintValue1==actID
        //        //                select p;
        //        var querydata = from p in db.V_M_TM_SYS_BaseData_promotion
        //                         join o in db.TR_SYS_Common on p.BaseDataID equals o.RelationBigintValue1 into temp
        //                         from q in temp.DefaultIfEmpty()
        //                        orderby p.BaseDataID descending
        //                         where q.RelationBigintValue2 == actID
        //                         select p;
        //        return new Result(true, "", new List<object> { querydata.ToList() });
        //    }
        //}

        //public static Result GetSysCommonBusPlanByID(string planID)
        //{
        //    using (CRMEntities db = new CRMEntities())
        //    {
             
        //        var querydata = from p in db.V_M_TM_SYS_BaseData_promotion
        //                        join o in db.TR_SYS_Common on p.BaseDataID equals o.RelationBigintValue1 into temp
        //                        from q in temp.DefaultIfEmpty()
        //                        orderby p.BaseDataID descending
        //                        where q.ReferenceStringValue1 == planID
        //                        select p;
        //        return new Result(true, "", new List<object> { querydata.ToList() });
        //    }
        //}


        //public static Result GetPosPromotionByID(string promotionID) {
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        var promotion = from p in db.V_M_TM_SYS_BaseData_promotion
        //                        orderby p.BaseDataID descending
        //                        where p.PromotionID == promotionID
        //                        select p;
        //        return new Result(true,"",promotion.FirstOrDefault());
        //    }
        //}

        //public static Result GetSysCommonByKey(string key, string type, int dataGroupID)
        //{
        //    try
        //    {
        //        using (CRMEntities db = new CRMEntities())
        //        {
        //            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(key))
        //            {
        //                return new Result(false, "填写信息不完整");
        //            }

        //            switch (type.Trim())
        //            {
        //                case "1"://会员细分
        //                    var querySubdivison = from c in db.TR_SYS_Common
        //                                          join s in db.TM_Mem_Subdivision
        //                                          on new
        //                                          {
        //                                              RelationType = c.RelationType.Trim(),
        //                                              RelationDataType = c.RelationDataType.Trim(),
        //                                              SubdivisionID = c.RelationGuidValue1
        //                                          } equals new
        //                                          {
        //                                              RelationType = "1",
        //                                              RelationDataType = "2",
        //                                              SubdivisionID = s.SubdivisionID
        //                                          }
        //                                          join p in db.V_M_TM_SYS_BaseData_promotion
        //                                          on new
        //                                          {
        //                                              RelationType = c.RelationType.Trim(),
        //                                              RelationDataType = c.RelationDataType.Trim(),
        //                                              PrmotionID = c.RelationBigintValue1
        //                                          } equals new
        //                                          {
        //                                              RelationType = "1",
        //                                              RelationDataType = "2",
        //                                              PrmotionID = p.BaseDataID
        //                                          }
        //                                          where c.RelationGuidValue1.Equals(new Guid(key.Trim()))
        //                                              && p.DataGroupID == dataGroupID
        //                                              && s.DataGroupID == dataGroupID
        //                                          select c;

        //                    return new Result(true, "", new List<object> { querySubdivison.ToList() });
        //                case "2"://商业计划
        //                    var queryBusinessPlan = from c in db.TR_SYS_Common
        //                                            join b in db.TM_CRM_BusinessPlan
        //                                            on new
        //                                            {
        //                                                RelationType = c.RelationType.Trim(),
        //                                                RelationDataType = c.RelationDataType.Trim(),
        //                                                BusinessPlanID = c.ReferenceStringValue1.Trim()
        //                                            } equals new
        //                                            {
        //                                                RelationType = "2",
        //                                                RelationDataType = "3",
        //                                                BusinessPlanID = b.BusinessPlanID.Trim()
        //                                            }
        //                                            join p in db.V_M_TM_SYS_BaseData_promotion
        //                                            on new
        //                                            {
        //                                                RelationType = c.RelationType.Trim(),
        //                                                RelationDataType = c.RelationDataType.Trim(),
        //                                                PrmotionID = c.RelationBigintValue1
        //                                            } equals new
        //                                            {
        //                                                RelationType = "2",
        //                                                RelationDataType = "3",
        //                                                PrmotionID = p.BaseDataID
        //                                            }
        //                                            where c.ReferenceStringValue1.Trim().Equals(key.Trim())
        //                                                && p.DataGroupID == dataGroupID
        //                                                && b.DataGroupID == dataGroupID
        //                                            select c;
        //                    return new Result(true, "", new List<object> { queryBusinessPlan.ToList() });
        //                case "3"://市场活动
        //                    var queryActivity = from c in db.TR_SYS_Common
        //                                        join a in db.TM_Act_Master
        //                                        on new
        //                                        {
        //                                            RelationType = c.RelationType.Trim(),
        //                                            RelationDataType = c.RelationDataType.Trim(),
        //                                            ActivityID = c.RelationBigintValue2
        //                                        } equals new
        //                                        {
        //                                            RelationType = "3",
        //                                            RelationDataType = "1",
        //                                            ActivityID = (long)a.ActivityID
        //                                        }
        //                                        join p in db.V_M_TM_SYS_BaseData_promotion
        //                                        on new
        //                                        {
        //                                            RelationType = c.RelationType.Trim(),
        //                                            RelationDataType = c.RelationDataType.Trim(),
        //                                            PrmotionID = c.RelationBigintValue1
        //                                        } equals new
        //                                        {
        //                                            RelationType = "3",
        //                                            RelationDataType = "1",
        //                                            PrmotionID = p.BaseDataID
        //                                        }
        //                                        where SqlFunctions.StringConvert((double)c.RelationBigintValue2).Trim().Equals(key.Trim())
        //                                            && p.DataGroupID == dataGroupID
        //                                            && a.DataGroupID == dataGroupID
        //                                        select c;
        //                    return new Result(true, "", new List<object> { queryActivity.ToList() });
        //                default:
        //                    return new Result(false, "无法识别的绑定促销类型");

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Result(false, ex.Message);
        //    }
        //}

        //public static Result GetSysCommonByPromotionID(long promotionID, string type, int dataGroupID)
        //{
        //    try
        //    {
        //        using (CRMEntities db = new CRMEntities())
        //        {
        //            if (string.IsNullOrEmpty(type))
        //            {
        //                return new Result(false, "填写信息不完整");
        //            }

        //            switch (type.Trim())
        //            {
        //                case "1"://会员细分
        //                    var querySubdivison = from c in db.TR_SYS_Common
        //                                          join s in db.TM_Mem_Subdivision
        //                                          on new
        //                                          {
        //                                              RelationType = c.RelationType.Trim(),
        //                                              RelationDataType = c.RelationDataType.Trim(),
        //                                              SubdivisionID = c.RelationGuidValue1
        //                                          } equals new
        //                                          {
        //                                              RelationType = "1",
        //                                              RelationDataType = "2",
        //                                              SubdivisionID = s.SubdivisionID
        //                                          }
        //                                          join p in db.V_M_TM_SYS_BaseData_promotion
        //                                          on new
        //                                          {
        //                                              RelationType = c.RelationType.Trim(),
        //                                              RelationDataType = c.RelationDataType.Trim(),
        //                                              PrmotionID = c.RelationBigintValue1
        //                                          } equals new
        //                                          {
        //                                              RelationType = "1",
        //                                              RelationDataType = "2",
        //                                              PrmotionID = p.BaseDataID
        //                                          }
        //                                          where p.BaseDataID == promotionID
        //                                              && p.DataGroupID == dataGroupID
        //                                              && s.DataGroupID == dataGroupID
        //                                          select c;
        //                    return new Result(true, "", new List<object>(querySubdivison.ToList()));
        //                case "2"://商业计划
        //                    var queryBusinessPlan = from c in db.TR_SYS_Common
        //                                            join b in db.TM_CRM_BusinessPlan
        //                                            on new
        //                                            {
        //                                                RelationType = c.RelationType.Trim(),
        //                                                RelationDataType = c.RelationDataType.Trim(),
        //                                                BusinessPlanID = c.ReferenceStringValue1.Trim()
        //                                            } equals new
        //                                            {
        //                                                RelationType = "2",
        //                                                RelationDataType = "3",
        //                                                BusinessPlanID = b.BusinessPlanID.Trim()
        //                                            }
        //                                            join p in db.V_M_TM_SYS_BaseData_promotion
        //                                            on new
        //                                            {
        //                                                RelationType = c.RelationType.Trim(),
        //                                                RelationDataType = c.RelationDataType.Trim(),
        //                                                PrmotionID = c.RelationBigintValue1
        //                                            } equals new
        //                                            {
        //                                                RelationType = "2",
        //                                                RelationDataType = "3",
        //                                                PrmotionID = p.BaseDataID
        //                                            }
        //                                            where p.BaseDataID == promotionID
        //                                                && p.DataGroupID == dataGroupID
        //                                                && b.DataGroupID == dataGroupID
        //                                            select c;

        //                    return new Result(true, "", new List<object>(queryBusinessPlan.ToList()));
        //                case "3"://市场活动
        //                    var queryActivity = from c in db.TR_SYS_Common
        //                                        join a in db.TM_Act_Master
        //                                        on new
        //                                        {
        //                                            RelationType = c.RelationType.Trim(),
        //                                            RelationDataType = c.RelationDataType.Trim(),
        //                                            ActivityID = c.RelationBigintValue2
        //                                        } equals new
        //                                        {
        //                                            RelationType = "3",
        //                                            RelationDataType = "1",
        //                                            ActivityID = (long)a.ActivityID
        //                                        }
        //                                        join p in db.V_M_TM_SYS_BaseData_promotion
        //                                        on new
        //                                        {
        //                                            RelationType = c.RelationType.Trim(),
        //                                            RelationDataType = c.RelationDataType.Trim(),
        //                                            PrmotionID = c.RelationBigintValue1
        //                                        } equals new
        //                                        {
        //                                            RelationType = "3",
        //                                            RelationDataType = "1",
        //                                            PrmotionID = p.BaseDataID
        //                                        }
        //                                        where p.BaseDataID == promotionID
        //                                            && p.DataGroupID == dataGroupID
        //                                            && a.DataGroupID == dataGroupID
        //                                        select c;
        //                    return new Result(true, "", new List<object>(queryActivity.ToList()));
        //                default:
        //                    return new Result(false, "无法识别的绑定促销类型");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Result(false, ex.Message);
        //    }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="promotionID">促销活动ID</param>
        ///// <param name="sysCommons"></param>
        ///// <param name="type">1：会员细分；2：商业计划；3：市场活动</param>
        ///// <returns></returns>
        //public static Result SaveSYSCommon(int promotionID, string sysCommons, string type)
        //{
        //    try
        //    {
        //        using (CRMEntities db = new CRMEntities())
        //        {
        //            DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(TR_SYS_Common));
        //            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(sysCommons));
        //            List<TR_SYS_Common> syscommomlist = JSONStringToList<TR_SYS_Common>(sysCommons);
        //            db.BeginTransaction();
        //            var sysCommon = from p in db.TR_SYS_Common//获取会员细分
        //                            where p.RelationType == type && p.RelationBigintValue1 == promotionID
        //                            select p;
        //            switch (type)
        //            {
        //                case "1":
        //                    if (sysCommon.Count() > 0)
        //                    {
        //                        foreach (var item in sysCommon)
        //                        {
        //                            db.TR_SYS_Common.Remove(item);
        //                        }
        //                    }
        //                    break;
        //                case "2":
        //                    var bcc = from com in sysCommon
        //                              join p in db.TM_CRM_BusinessPlan on com.ReferenceStringValue1 equals p.BusinessPlanID into temp
        //                              from q in temp.DefaultIfEmpty()
        //                              where q.Status == "1"
        //                              select new
        //                              {
        //                                  com.RelationType,
        //                                  com.RelationBigintValue1,
        //                                  com.RelationBigintValue2,
        //                                  com.RelationGuidValue1,
        //                                  com.RelationGuidValue2
        //                              };
        //                    var bc = from com1 in sysCommon
        //                             join com2 in bcc
        //                                 on new
        //                                 {
        //                                     com1.RelationType,
        //                                     com1.RelationBigintValue1,
        //                                     com1.RelationBigintValue2,
        //                                     com1.RelationGuidValue1,
        //                                     com1.RelationGuidValue2
        //                                 } equals new
        //                                 {
        //                                     com2.RelationType,
        //                                     com2.RelationBigintValue1,
        //                                     com2.RelationBigintValue2,
        //                                     com2.RelationGuidValue1,
        //                                     com2.RelationGuidValue2
        //                                 }
        //                             select com1;
        //                    if (bc.Count() > 0)
        //                    {
        //                        foreach (var item in bc)
        //                        {
        //                            db.TR_SYS_Common.Remove(item);
        //                        }
        //                    }
        //                    break;
        //                case "3":
        //                    var ccc = from com in sysCommon
        //                              join p in db.TM_Act_Master on com.RelationBigintValue2 equals p.ActivityID into temp
        //                              from q in temp.DefaultIfEmpty()
        //                              where q.Status == 0
        //                              select new
        //                              {
        //                                  com.RelationType,
        //                                  com.RelationBigintValue1,
        //                                  com.RelationBigintValue2,
        //                                  com.RelationGuidValue1,
        //                                  com.RelationGuidValue2
        //                              };
        //                    var cc = from com1 in sysCommon
        //                             join com2 in ccc
        //                                 on new
        //                                 {
        //                                     com1.RelationType,
        //                                     com1.RelationBigintValue1,
        //                                     com1.RelationBigintValue2,
        //                                     com1.RelationGuidValue1,
        //                                     com1.RelationGuidValue2
        //                                 } equals new
        //                                 {
        //                                     com2.RelationType,
        //                                     com2.RelationBigintValue1,
        //                                     com2.RelationBigintValue2,
        //                                     com2.RelationGuidValue1,
        //                                     com2.RelationGuidValue2
        //                                 }
        //                             select com1;
        //                    if (cc.Count() > 0)
        //                    {
        //                        foreach (var item in cc)
        //                        {
        //                            db.TR_SYS_Common.Remove(item);
        //                        }
        //                    }
        //                    break;
        //                default:
        //                    break;
        //            }

        //            for (int i = 0; i < syscommomlist.Count; i++)
        //            {
        //                syscommomlist[i].RelationType = type;
        //                syscommomlist[i].RelationDataType = type == "1" ? "2" : type == "2" ? "3" : "1";
        //                syscommomlist[i].RelationGuidValue2 = Guid.NewGuid();
        //                db.TR_SYS_Common.Add(syscommomlist[i]);
        //            }
        //            db.SaveChanges();
        //            db.Commit();
        //            return new Result(true, "", "保存成功");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Result(false, "", ex.Message);
        //    }
        //}

        //public static Result InsertBactchPromotionSysCommon(string commonType, string sysCommonList, string key, int dataGroupID)
        //{
        //    try
        //    {
        //        List<TR_SYS_Common> list = JsonHelper.Deserialize<List<TR_SYS_Common>>(sysCommonList);

        //        using (CRMEntities db = new CRMEntities())
        //        {
        //            if (string.IsNullOrEmpty(commonType) || string.IsNullOrEmpty(sysCommonList) || string.IsNullOrEmpty(key))
        //            {
        //                return new Result(false, "填写信息不完整");
        //            }

        //            switch (commonType.Trim())
        //            {
        //                case "1"://会员细分
        //                    var querySubdivison = from c in db.TR_SYS_Common
        //                                          join s in db.TM_Mem_Subdivision
        //                                          on new
        //                                          {
        //                                              RelationType = c.RelationType.Trim(),
        //                                              RelationDataType = c.RelationDataType.Trim(),
        //                                              SubdivisionID = c.RelationGuidValue1
        //                                          } equals new
        //                                          {
        //                                              RelationType = "1",
        //                                              RelationDataType = "2",
        //                                              SubdivisionID = s.SubdivisionID
        //                                          }
        //                                          join p in db.V_M_TM_SYS_BaseData_promotion
        //                                          on new
        //                                          {
        //                                              RelationType = c.RelationType.Trim(),
        //                                              RelationDataType = c.RelationDataType.Trim(),
        //                                              PrmotionID = c.RelationBigintValue1
        //                                          } equals new
        //                                          {
        //                                              RelationType = "1",
        //                                              RelationDataType = "2",
        //                                              PrmotionID = p.BaseDataID
        //                                          }
        //                                          where c.RelationGuidValue1.Equals(new Guid(key.Trim()))
        //                                              && p.PromotionIsEnd.Trim().Equals("0")
        //                                              && (p.StartDatePromotion.HasValue ? DateTime.Now >= p.StartDatePromotion.Value : true)
        //                                              && (p.EndDatePromotion.HasValue ? DateTime.Now <= p.EndDatePromotion.Value : true)
        //                                              && p.DataGroupID == dataGroupID
        //                                              && s.DataGroupID == dataGroupID
        //                                          select c;
        //                    if (querySubdivison.Count() > 0)
        //                    {
        //                        foreach (var obj in querySubdivison)
        //                        {
        //                            db.TR_SYS_Common.Remove(obj);
        //                        }
        //                    }

        //                    for (var index = 0; index < list.Count; index++)
        //                    {
        //                        TR_SYS_Common tempObject = new TR_SYS_Common();
        //                        tempObject.RelationType = "1";
        //                        tempObject.RelationDataType = "2";
        //                        tempObject.RelationBigintValue1 = list[index].RelationBigintValue1;
        //                        tempObject.RelationGuidValue1 = new Guid(key.Trim());
        //                        tempObject.RelationGuidValue2 = Guid.NewGuid();
        //                        tempObject.StartDate = list[index].StartDate;
        //                        tempObject.EndDate = list[index].EndDate;
        //                        db.TR_SYS_Common.Add(tempObject);
        //                    }
        //                    db.SaveChanges();
        //                    break;
        //                case "2"://商业计划
        //                    var queryValidPlan = from b in db.TM_CRM_BusinessPlan
        //                                         where b.BusinessPlanID.Trim().Equals(key.Trim())
        //                                         && b.Status.Trim().Equals("1")
        //                                         && b.DataGroupID == dataGroupID
        //                                         select b;
        //                    if (queryValidPlan.Count() == 0)
        //                    {
        //                        return new Result(false, "查找不到有效的商业计划");
        //                    }
        //                    var queryBusinessPlan = from c in db.TR_SYS_Common
        //                                            join b in db.TM_CRM_BusinessPlan
        //                                            on new
        //                                            {
        //                                                RelationType = c.RelationType.Trim(),
        //                                                RelationDataType = c.RelationDataType.Trim(),
        //                                                BusinessPlanID = c.ReferenceStringValue1.Trim()
        //                                            } equals new
        //                                            {
        //                                                RelationType = "2",
        //                                                RelationDataType = "3",
        //                                                BusinessPlanID = b.BusinessPlanID.Trim()
        //                                            }
        //                                            //join p in db.V_M_TM_SYS_BaseData_promotion
        //                                            //on new
        //                                            //{
        //                                            //    RelationType = c.RelationType.Trim(),
        //                                            //    RelationDataType = c.RelationDataType.Trim(),
        //                                            //    PrmotionID = c.RelationBigintValue1
        //                                            //} equals new
        //                                            //{
        //                                            //    RelationType = "2",
        //                                            //    RelationDataType = "3",
        //                                            //    PrmotionID = p.BaseDataID
        //                                            //}
        //                                            where c.ReferenceStringValue1.Trim().Equals(key.Trim())
        //                                                //&& p.PromotionIsEnd.Trim().Equals("0")
        //                                                //&& (p.StartDatePromotion.HasValue ? DateTime.Now >= p.StartDatePromotion.Value : true)
        //                                                //&& (p.EndDatePromotion.HasValue ? DateTime.Now <= p.EndDatePromotion.Value : true)
        //                                                //&& p.DataGroupID == dataGroupID
        //                                                && b.Status.Trim().Equals("1")
        //                                                && b.DataGroupID == dataGroupID
        //                                            select c;
        //                    if (queryBusinessPlan.Count() > 0)
        //                    {
        //                        foreach (var obj in queryBusinessPlan)
        //                        {
        //                            db.TR_SYS_Common.Remove(obj);
        //                        }
        //                    }

        //                    for (var index = 0; index < list.Count; index++)
        //                    {
        //                        TR_SYS_Common tempObject = new TR_SYS_Common();
        //                        tempObject.RelationType = "2";
        //                        tempObject.RelationDataType = "3";
        //                        tempObject.RelationBigintValue1 = list[index].RelationBigintValue1;
        //                        tempObject.ReferenceStringValue1 = key.Trim();
        //                        tempObject.RelationGuidValue2 = Guid.NewGuid();
        //                        tempObject.StartDate = list[index].StartDate;
        //                        tempObject.EndDate = list[index].EndDate;
        //                        db.TR_SYS_Common.Add(tempObject);
        //                    }
        //                    db.SaveChanges();
        //                    break;
        //                case "3"://市场活动
        //                    var queryValidActivity = from a in db.TM_Act_Master
        //                                             where SqlFunctions.StringConvert((double)a.ActivityID).Trim().Equals(key.Trim())
        //                                                 && a.Status == 0
        //                                                 && a.DataGroupID == dataGroupID
        //                                             select a;
        //                    if (queryValidActivity.Count() == 0)
        //                    {
        //                        return new Result(false, "查找不到有效的市场活动");
        //                    }
        //                    var queryActivity = from c in db.TR_SYS_Common
        //                                        join a in db.TM_Act_Master
        //                                        on new
        //                                        {
        //                                            RelationType = c.RelationType.Trim(),
        //                                            RelationDataType = c.RelationDataType.Trim(),
        //                                            ActivityID = c.RelationBigintValue2
        //                                        } equals new
        //                                        {
        //                                            RelationType = "3",
        //                                            RelationDataType = "1",
        //                                            ActivityID = (long)a.ActivityID
        //                                        }
        //                                        //join p in db.V_M_TM_SYS_BaseData_promotion
        //                                        //on new
        //                                        //{
        //                                        //    RelationType = c.RelationType.Trim(),
        //                                        //    RelationDataType = c.RelationDataType.Trim(),
        //                                        //    PrmotionID = c.RelationBigintValue1
        //                                        //} equals new
        //                                        //{
        //                                        //    RelationType = "3",
        //                                        //    RelationDataType = "1",
        //                                        //    PrmotionID = p.BaseDataID
        //                                        //}
        //                                        where SqlFunctions.StringConvert((double)c.RelationBigintValue2).Trim().Equals(key.Trim())
        //                                            //&& p.PromotionIsEnd.Trim().Equals("0")
        //                                            //&& (p.StartDatePromotion.HasValue ? DateTime.Now >= p.StartDatePromotion.Value : true)
        //                                            //&& (p.EndDatePromotion.HasValue ? DateTime.Now <= p.EndDatePromotion.Value : true)
        //                                            //&& p.DataGroupID == dataGroupID
        //                                            && a.Status == 0
        //                                            && a.DataGroupID == dataGroupID
        //                                        select c;
        //                    if (queryActivity.Count() > 0)
        //                    {
        //                        foreach (var obj in queryActivity)
        //                        {
        //                            db.TR_SYS_Common.Remove(obj);
        //                        }
        //                    }

        //                    for (var index = 0; index < list.Count; index++)
        //                    {
        //                        TR_SYS_Common tempObject = new TR_SYS_Common();
        //                        tempObject.RelationType = "3";
        //                        tempObject.RelationDataType = "1";
        //                        tempObject.RelationBigintValue1 = list[index].RelationBigintValue1;
        //                        tempObject.RelationBigintValue2 = long.Parse(key.Trim());
        //                        tempObject.RelationGuidValue2 = Guid.NewGuid();
        //                        tempObject.StartDate = list[index].StartDate;
        //                        tempObject.EndDate = list[index].EndDate;
        //                        db.TR_SYS_Common.Add(tempObject);
        //                    }
        //                    db.SaveChanges();
        //                    break;
        //                default:
        //                    return new Result(false, "无法识别的绑定促销类型");

        //            }
        //            return new Result(true, "", new List<object> { key.Trim() });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Result(false, ex.Message);
        //    }
        //}

        //public static List<T> JSONStringToList<T>(this string JsonStr)
        //{
        //    JavaScriptSerializer Serializer = new JavaScriptSerializer();
        //    List<T> objs = Serializer.Deserialize<List<T>>(JsonStr);
        //    return objs;
        //}
    }
}
