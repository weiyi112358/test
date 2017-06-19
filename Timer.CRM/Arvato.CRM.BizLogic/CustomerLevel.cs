using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;

namespace Arvato.CRM.BizLogic
{
    public static class CustomerLevel
    {
        public static Result GetCustomersLevels(string dataParamersJson, string customerLevel, string brand, int dataGroupId)
        {
            DatatablesParameter dataParameters = JsonHelper.Deserialize<DatatablesParameter>(dataParamersJson);
            using (CRMEntities db = new CRMEntities())
            {
                var vgrouprelationId = db.V_Sys_DataGroupRelation.Where(t => t.DataGroupID == dataGroupId).Select(t => t.SubDataGroupID).ToList();
                var query = from custom in db.V_M_TM_SYS_BaseData_customerlevel
                            where vgrouprelationId.Contains(custom.DataGroupID)
                            orderby custom.DataGroupID ascending
                            select new
                            {
                                custom.BaseDataID,
                                custom.DataGroupID,
                                custom.CustomerLevelNameBase,
                                custom.BrandNameCustomerLevel,
                                custom.BrandCodeCustomerLevel,
                                custom.CustomerLevelBase,
                                custom.MaxIntergral,
                                custom.RateCustomerLevel,
                                custom.RateMaxUse,
                                SubDataGroupName = db.TM_SYS_DataGroup.FirstOrDefault(t => t.DataGroupID == custom.DataGroupID) != null
                                  ? db.TM_SYS_DataGroup.FirstOrDefault(t => t.DataGroupID == custom.DataGroupID).DataGroupName : ""
                            };
                if (customerLevel != "0")
                {
                    query = query.Where(m => m.CustomerLevelBase == customerLevel);
                }
                if (brand != "0")
                {
                    query = query.Where(m => m.BrandCodeCustomerLevel == brand);
                }

                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(dataParameters) });
            }

        }


        public static Result GetBizOptionCustomersLevel(string customerLevel, int dataGroupId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from bo in db.TD_SYS_BizOption
                            where bo.OptionType == customerLevel && bo.DataGroupID == dataGroupId
                            select bo;
                return new Result(true, "", query.ToList());
            }

        }

        public static Result GetCustomersLevelList(int dataGroupId)
        {
            using (var db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_customerlevel.Where(bo => bo.DataGroupID == dataGroupId).Select(bo => new
                    {
                        OptionValue = bo.CustomerLevelBase,
                        OptionText = bo.CustomerLevelNameBase
                    }).ToList();
                return new Result(true, "", query);
            }

        }

        /// <summary>
        /// 添加修改会员等级
        /// </summary>
        /// <param name="customerLevel"></param>
        /// <param name="userId"></param>
        /// <param name="groupDataId"></param>
        /// <returns></returns>
        public static Result EditCustomerLevel(CustomerLevelModel customerLevel, string userId, int groupDataId)
        {
            using (var db = new CRMEntities())
            {
                var customerLevelExist = db.V_M_TM_SYS_BaseData_customerlevel.FirstOrDefault(t => t.CustomerLevelBase == customerLevel.CustomerLevel && t.DataGroupID == groupDataId && t.BrandCodeCustomerLevel == customerLevel.BrandCodeCustomerLevel);
                if (customerLevel.ID > 0)
                {//修改
                    if (customerLevelExist != null && customerLevel.ID != customerLevelExist.BaseDataID)
                    {
                        return new Result(false, "会员等级已存在，无法重复插入");
                    }
                    var dbCustomerLevel = db.V_M_TM_SYS_BaseData_customerlevel.FirstOrDefault(p => p.BaseDataID == customerLevel.ID);
                    if (dbCustomerLevel != null)
                    {
                        var dbCustomerLevel1 = new V_M_TM_SYS_BaseData_customerlevel
                        {
                            BaseDataID = customerLevel.ID,
                            BrandCodeCustomerLevel = customerLevel.BrandCodeCustomerLevel,
                            BrandNameCustomerLevel = customerLevel.BrandNameCustomerLevel,
                            CustomerLevelNameBase = customerLevel.CustomerLevelName,
                            DataGroupID = groupDataId,
                            CustomerLevelBase = customerLevel.CustomerLevel,
                            MaxIntergral = customerLevel.MaxIntergral,
                            RateMaxUse = customerLevel.RateMaxUse,
                            RateCustomerLevel = customerLevel.Rate,
                            BaseDataType = dbCustomerLevel.BaseDataType,
                            LevelUpInt =  dbCustomerLevel.LevelUpInt
                        };
                        dynamic t = db.UpdateViewRow<V_M_TM_SYS_BaseData_customerlevel, TM_SYS_BaseData>(dbCustomerLevel1);

                        db.SaveChanges();
                        return new Result(true, "修改会员等级成功");
                    }
                    else
                    {
                        return new Result(false, "会员等级未找到");
                    }
                }
                else
                {//添加
                    if (customerLevelExist != null)
                    {
                        return new Result(false, "会员等级已存在，无法重复插入");
                    }
                    V_M_TM_SYS_BaseData_customerlevel tdCustomerLevel = new V_M_TM_SYS_BaseData_customerlevel
                        {

                            BrandCodeCustomerLevel = customerLevel.BrandCodeCustomerLevel,
                            BrandNameCustomerLevel = customerLevel.BrandNameCustomerLevel,
                            CustomerLevelNameBase = customerLevel.CustomerLevelName,
                            DataGroupID = groupDataId,
                            CustomerLevelBase = customerLevel.CustomerLevel,
                            RateMaxUse = customerLevel.RateMaxUse,
                            MaxIntergral = customerLevel.MaxIntergral,
                            RateCustomerLevel = customerLevel.Rate,
                        };
                    dynamic t = db.AddViewRow<V_M_TM_SYS_BaseData_customerlevel, TM_SYS_BaseData>(tdCustomerLevel);
                    db.SaveChanges();
                    return new Result(true, "新增会员等级成功");
                }

            }
        }

        public static Result GetCustomerLevelById(int customerLevelId)
        {
            using (var db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_customerlevel.FirstOrDefault(i=>i.BaseDataID == customerLevelId);
                return new Result(true, "", query);
            }
        }

        public static Result DeleteCustomerLevelById(int customerLevelId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_customerlevel.Where(t => t.BaseDataID == customerLevelId).FirstOrDefault();
                if (query != null)
                {
                    dynamic t = db.DeleteViewRow<V_M_TM_SYS_BaseData_customerlevel, TM_SYS_BaseData>(query);

                    db.SaveChanges();
                    return new Result(true, "删除成功");
                }
                return new Result(true, "删除失败");
            }
        }
    }
}
