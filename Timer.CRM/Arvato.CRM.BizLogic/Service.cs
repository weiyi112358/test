using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arvato.CRM.BizLogic
{
    public static class Service
    {

        /// <summary>
        /// 递归创建数据群组树图
        /// </summary>
        /// <param name="dgInfo"></param>
        /// <param name="dgId"></param>
        /// <returns></returns>
        //public static TreeNode CreateTreeNode(List<TM_SYS_DataGroup> dgInfo, int dgId)
        //{
        //    var dg = dgInfo.Where(o => o.DataGroupID == dgId).FirstOrDefault();
        //    if (dg == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        TreeNode subTree = new TreeNode();
        //        subTree.key = dg.DataGroupID.ToString();
        //        subTree.title = dg.DataGroupName;
        //        subTree.isFolder = true;

        //        List<TM_SYS_DataGroup> childDgs = dgInfo.Where(o => o.ParentDataGroupID == dgId).ToList();
        //        List<TreeNode> childNodes = new List<TreeNode>();
        //        foreach (var item in childDgs)
        //        {
        //            var child = CreateTreeNode(dgInfo, item.DataGroupID);
        //            if (child != null)
        //            {
        //                childNodes.Add(child);
        //            }
        //        }
        //        if (childNodes.Count < 1)
        //        {
        //            subTree.isLazy = false;
        //        }
        //        else
        //        {
        //            subTree.children = childNodes;
        //        }
        //        return subTree;
        //    }
        //}

        /// <summary>
        /// 创建数据树图通用版
        /// </summary>
        /// <param name="nodes">节点模板集合</param>
        /// <param name="id">节点id</param>
        /// <param name="isLazy">是否延迟加载</param>
        /// <returns></returns>
        public static TreeNode CreateTreeNodeData(List<TreeDataSource> nodes, string id, bool isLazy = false)
        {
            var dg = nodes.Where(o => o.nodeId == id).FirstOrDefault();
            if (dg == null)
            {
                return null;
            }
            else
            {
                TreeNode subTree = new TreeNode();
                subTree.key = dg.nodeId.ToString();
                subTree.title = dg.nodeName;
                subTree.isFolder = true;

                List<TreeDataSource> childDgs = nodes.Where(o => o.nodePId == id && o.nodeId != id).ToList();
                if (childDgs == null || childDgs.Count == 0)
                    subTree.isFolder = false;

                List<TreeNode> childNodes = new List<TreeNode>();
                foreach (var item in childDgs)
                {
                    var child = CreateTreeNodeData(nodes, item.nodeId, isLazy);
                    if (child != null)
                    {
                        childNodes.Add(child);
                    }
                }
                if (childNodes.Count < 1)
                {
                    subTree.isLazy = isLazy;
                }
                else
                {
                    subTree.children = childNodes;
                }
                return subTree;
            }
        }

        /// <summary>
        /// 获取字段类型数据列表
        /// </summary>
        /// <returns></returns>
        public static Result GetFieldTypeList()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.TD_SYS_BizOption
                             where a.OptionType == "DBFieldType" && a.Enable == true
                             orderby a.Sort ascending
                             select a).ToList();
                return new Result(true, "", query);
            }
        }

        public static Result GetAllVipType(int iGroupID)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.TD_SYS_BizOption
                             where a.OptionType == "CustomerLevel" && a.Enable == true && a.DataGroupID == iGroupID
                             orderby a.Sort ascending
                             select a).ToList();
                return new Result(true, "", query);
            }
        }

        /// <summary>
        /// 获取会员属性关系列表
        /// </summary>
        /// <returns></returns>
        public static Result GetRelationshipList()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.TD_SYS_BizOption
                             where a.OptionType == "DynamicTable" && a.Enable == true
                             orderby a.Sort ascending
                             select a).ToList();
                return new Result(true, "", query);
            }
        }

        /// <summary>
        /// 获取运行类型列表
        /// </summary>
        /// <returns></returns>
        public static Result GetRunTypeList()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.TD_SYS_BizOption
                             where a.OptionType == "RunType" && a.Enable == true
                             orderby a.Sort ascending
                             select a).ToList();
                return new Result(true, "", query);
            }
        }

        /// <summary>
        /// 获取option表关于系统属性的数据
        /// （字段类型数据“DBFieldType”）/会员属性关系“DynamicTable”/交易类型“MemberTrade”/会员子属性“MemberSubExt”/销售明细类型“SalseDetailType”
        /// </summary>
        /// <param name="optType">字段类型</param>
        /// <returns></returns>
        public static Result GetOptionDataList(string optType)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.TD_SYS_BizOption
                             where a.OptionType == optType && a.Enable == true
                             orderby a.Sort ascending
                             select a).ToList();
                return new Result(true, "", query);
            }
        }

        /// <summary>
        /// 获取下级所有DataGroup（包括自己）
        /// </summary>
        /// <param name="parentId">数据群组ID</param>
        /// <returns></returns>
        public static Result GetDataGroupsByParentId(int? parentId)
        {
            using (var db = new CRMEntities())
            {
                var groups = from a in db.V_Sys_DataGroupRelation
                             select a;
                if (parentId.HasValue)
                {
                    groups = groups.Where(o => o.DataGroupID == parentId.Value);
                }
                return new Result(true, "", groups.ToList());
            }
        }

        /// <summary>
        /// 获取下级所有DataGroup（包括自己）
        /// </summary>
        /// <param name="parentId">数据群组ID</param>
        /// <returns></returns>
        public static Result GetDataGroupsExceptRootByParentId(int? parentId)
        {
            using (var db = new CRMEntities())
            {
                var groups = from a in db.V_Sys_DataGroupRelation
                             where a.SubDataGroupGrade > 0
                             select a;
                if (parentId.HasValue)
                {
                    groups = groups.Where(o => o.DataGroupID == parentId.Value);
                }
                return new Result(true, "", groups.ToList());
            }
        }

        /// <summary>
        /// 根据数据群组查找其和其下级的所有角色
        /// </summary>
        /// <param name="dataGroupID">数据群组ID</param>
        /// <param name="roleType">角色类型（page或data或""）</param>
        /// <returns></returns>
        public static Result GetAllRolesByDataGroupId(int? dataGroupID, string roleType)
        {
            using (var db = new CRMEntities())
            {
                var roles = from a in db.TM_AUTH_Role
                            where a.Enable == true
                            select a;
                if (!string.IsNullOrEmpty(roleType))
                {
                    roles = roles.Where(o => o.RoleType == roleType);
                }
                if (dataGroupID.HasValue)
                {
                    roles = from a in roles
                            join b in db.V_Sys_DataGroupRelation on a.DataGroupID equals b.SubDataGroupID
                            where b.DataGroupID == dataGroupID.Value
                            select a;
                }

                return new Result(true, "", roles.ToList());
            }
        }


        /// <summary>
        /// 根据其创建用户查找其子用户的角色
        /// </summary>
        /// <param name="dataGroupID">数据群组ID</param>
        /// <param name="roleType">角色类型（page或data或""）</param>
        /// <param name="addeduser">该用户创建者id</param>
        /// <returns></returns>
        public static Result GetRolesByAddedUser(int? dataGroupID, string roleType, string addedUser)
        {
            using (var db = new CRMEntities())
            {
                var roles = from a in db.TM_AUTH_Role
                            where a.Enable == true
                            select a;
                if (!string.IsNullOrEmpty(roleType))
                {
                    roles = roles.Where(o => o.RoleType == roleType);
                }
                if (dataGroupID.HasValue)
                {
                    roles = from a in roles
                            join b in db.V_Sys_DataGroupRelation on a.DataGroupID equals b.SubDataGroupID
                            where b.DataGroupID == dataGroupID.Value
                            select a;
                }
                if (!string.IsNullOrEmpty(addedUser))
                {
                    roles = roles.Where(o => o.AddedUser == addedUser);
                }

                return new Result(true, "", roles.ToList());
            }
        }

        /// <summary>
        /// 根据数据群组查找其和其下级的所有门店
        /// </summary>
        /// <param name="dataGroupID">数据群组ID</param>
        /// <returns></returns>
        public static Result GetAllStoresByDataGroupId(int? dataGroupID)
        {
            using (var db = new CRMEntities())
            {
                var stores = from a in db.V_M_TM_SYS_BaseData_store
                             select a;
                if (dataGroupID.HasValue)
                {
                    stores = from a in stores
                             join b in db.V_Sys_DataGroupRelation on a.DataGroupID equals b.SubDataGroupID
                             where b.DataGroupID == dataGroupID.Value
                             select a;
                }

                return new Result(true, "", stores.ToList());
            }
        }

        /// <summary>
        /// 根据UserID查找角色
        /// </summary>
        /// <param name="userId">UserID</param>
        /// <returns></returns>
        public static Result GetRolesByUserId(int userId)
        {
            using (var db = new CRMEntities())
            {
                var roles = from a in db.TM_AUTH_Role
                            join b in db.TR_AUTH_UserRole on a.RoleID equals b.RoleID
                            where b.UserID == userId
                            select a;
                return new Result(true, "", roles.ToList());
            }
        }

        /// <summary>
        /// 根据UserID查找角色
        /// </summary>
        /// <param name="userId">UserID</param>
        /// <returns></returns>
        public static Result GetSubRolesByUserId(int userId)
        {
            using (var db = new CRMEntities())
            {
                //var roles = from a in db.TM_AUTH_Role
                //            join b in db.TR_AUTH_UserRole on a.RoleID equals b.RoleID
                //            join c in db.TM_AUTH_User on SqlFunctions.StringConvert((double)b.UserID) equals c.AddedUser
                //            where c.UserID ==userId
                //            select a;

                var roles = from a in db.TR_AUTH_UserRole
                            join b in db.TM_AUTH_User on SqlFunctions.StringConvert((double)a.UserID).Trim() equals b.AddedUser
                            join c in db.TM_AUTH_Role on a.RoleID equals c.RoleID
                            where b.UserID == userId
                            select new
                            {
                                a.RoleID,
                                c.RoleType,
                                c.RoleName

                            };




                return new Result(true, "", roles.ToList());
            }
        }
        public static Result GetBusiByUserId(int userId)
        {
            using (var db = new CRMEntities())
            {
                var roles = from a in db.TR_AUTH_UserBusinessDepartment
                            where a.UserID == userId
                            select new
                            {
                                a.DepartmentID
                            };

                return new Result(true, "", roles.ToList());
            }
        }
        /// <summary>
        /// 根据UserID加载角色
        /// </summary>
        /// <param name="userId">UserID</param>
        /// <returns></returns>
        public static Result LoadRolesByUserId(int userId)
        {
            using (var db = new CRMEntities())
            {
                //var roles = from a in db.TM_AUTH_Role
                //            join b in db.TR_AUTH_UserRole on a.RoleID equals b.RoleID
                //            join c in db.TM_AUTH_User on SqlFunctions.StringConvert((double)b.UserID) equals c.AddedUser
                //            where c.UserID ==userId
                //            select a;

                var roles = from a in db.TR_AUTH_UserRole
                            join b in db.TM_AUTH_User on a.UserID equals b.UserID
                            join c in db.TM_AUTH_Role on a.RoleID equals c.RoleID
                            where b.UserID == userId
                            select new
                            {
                                a.RoleID,
                                c.RoleType,
                                c.RoleName

                            };

                return new Result(true, "", roles.ToList());
            }
        }

        /// <summary>
        /// 根据类型获取BizOption的值
        /// </summary>
        /// <param name="optionType">Option类型</param>
        /// <returns></returns>
        public static Result GetBizOptionsByType(string optionType, bool? enable)
        {
            using (var db = new CRMEntities())
            {
                var query = from a in db.TD_SYS_BizOption
                            where a.OptionType.Equals(optionType)
                            select a;
                if (enable.HasValue)
                {
                    query = query.Where(o => o.Enable == enable);
                }
                return new Result(true, "", query.ToList());
            }
        }

        /// <summary>
        /// 获取所有门店渠道
        /// </summary>
        /// <returns></returns>
        public static Result GetStoreChannels()
        {
            using (var db = new CRMEntities())
            {
                var query = from a in db.V_M_TM_SYS_BaseData_channel
                            select new
                            {
                                OptionValue = a.ChannelCodeBase,
                                OptionText = a.ChannelNameBase
                            };
                return new Result(true, "", query.ToList());
            }
        }


        //获取颜色
        //public static Result GetColors()
        //{
        //    using (var db = new CRMEntities())
        //    {
        //        var query = from a in db.V_M_TM_SYS_BaseData_colors
        //                    orderby a.ColorName
        //                    select new
        //                    {
        //                        OptionValue = a.ColorCode,
        //                        OptionText = a.ColorName
        //                    };
        //        return new Result(true, "", query.ToList());
        //    }
        //}
        //获取区域
        public static Result GetAreas()
        {
            using (var db = new CRMEntities())
            {
                var query = from a in db.V_M_TM_SYS_BaseData_area
                            select new
                            {
                                OptionValue = a.AreaCodeBase,
                                OptionText = a.AreaNameBase
                            };
                return new Result(true, "", query.ToList());
            }
        }
        //获取品牌
        public static Result GetBrands()
        {
            using (var db = new CRMEntities())
            {
                var query = from a in db.V_M_TM_SYS_BaseData_brand
                            select new
                            {
                                OptionValue = a.BrandCodeBase,
                                OptionText = a.BrandNameBase
                            };
                return new Result(true, "", query.ToList());
            }
        }
       
        /// <summary>
        /// 获取可用的门店渠道
        /// </summary>
        /// <returns></returns>
        public static Result GetEnableStoreChannels()
        {
            using (var db = new CRMEntities())
            {
                var query = from a in db.V_M_TM_SYS_BaseData_channel
                            where a.ChannelIsEnableBase == "1"
                            select new
                            {
                                OptionValue = a.ChannelCodeBase,
                                OptionText = a.ChannelNameBase
                            };
                return new Result(true, "", query.ToList());
            }
        }

        /// <summary>
        /// 获取大区
        /// </summary>
        /// <returns></returns>
        public static Result GetStoreArea()
        {
            using (var db = new CRMEntities())
            {
                var query = from a in db.V_M_TM_SYS_BaseData_store
                            group a by new
                                {
                                    a.AreaCodeStore,
                                    a.AreaNameStore
                                } into g
                            select new
                            {
                                OptionValue = g.Key.AreaCodeStore,
                                OptionText = g.Key.AreaNameStore
                            };
                return new Result(true, "", query.ToList());
            }
        }



        public static Result GetDataLimitTypes(bool? enable)
        {
            using (var db = new CRMEntities())
            {
                var query = db.TD_SYS_BizOption.Where(p => p.OptionType == "DataLimitType");
                if (enable.HasValue) query = query.Where(p => p.Enable == enable);

                //var types = query.ToList();

                var query1 = from a in db.TM_SYS_BaseData
                             join b in db.V_Sys_DataGroupRelation on a.DataGroupID equals b.SubDataGroupID
                             join q in query on a.BaseDataType equals q.OptionValue
                             where b.DataGroupID == 1
                             select new { a.Str_Attr_1, a.Str_Attr_2, q.OptionValue, q.OptionText };
                var res = query1.ToList()
                    .GroupBy(p => new { p.OptionText, p.OptionValue })
                    .Select(p => new
                    {
                        p.Key.OptionText,
                        p.Key.OptionValue,
                        Children = p.Select(q => new { q.Str_Attr_1, q.Str_Attr_2 })
                    });
                return new Result(true, "", res.ToList());
            }
        }

        /// <summary>
        /// 根据类型和DataGroupID获取BizOption的值
        /// </summary>
        /// <param name="optionType">Option类型</param>
        /// <param name="optionType">DataGroupID</param>
        /// <returns></returns>
        public static Result GetBizOptionsByTypeAndDataGroup(string optionType, int? dataGroupId, bool? enable)
        {
            using (var db = new CRMEntities())
            {
                var query = from a in db.TD_SYS_BizOption
                            where a.OptionType.Equals(optionType)
                            && a.DataGroupID == dataGroupId
                            select a;
                if (enable.HasValue)
                {
                    query = query.Where(o => o.Enable == enable);
                }
                return new Result(true, "", query.ToList());
            }
        }



        /// <summary>
        /// 查询一条数据的修改记录
        /// </summary>
        /// <param name="mainId">数据ID</param>
        /// <param name="mainType">数据类型</param>
        /// <returns></returns>
        public static Result GetLogsByMainIdAndType(string dp, string mainId, string mainType, DateTime? start, DateTime? end)
        {

            throw new NotImplementedException();
            //DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            //using (var db = new CRMEntities())
            //{
            //    var query = from c in db.TL_SYS_ValueChange
            //                join a in db.TD_SYS_FieldAlias on c.FieldAlias equals a.FieldAlias
            //                join u in db.TM_AUTH_User on c.AddedUser equals SqlFunctions.StringConvert((double)u.UserID).Trim()
            //                join t in db.TD_SYS_BizOption.Where(o => o.OptionType == "ValueChangeType") on c.ChangeType equals t.OptionValue
            //                where a.AliasType == mainType
            //                select new
            //                {
            //                    c.LogID,
            //                    c.ReferenceKey,
            //                    c.AddedDate,
            //                    c.AddedUser,
            //                    AddedUserName = u.UserName ?? "",
            //                    c.ChangeType,
            //                    ChangeTypeText = t.OptionText ?? "",
            //                    c.Remark,
            //                    c.ChangeValueTo,
            //                    c.FieldAlias,
            //                    FieldName = a.FieldDesc ?? "",
            //                    c.OrgValue
            //                };
            //    if (start != null)
            //    {
            //        query = query.Where(o => o.AddedDate >= start);
            //    }
            //    if (end != null)
            //    {
            //        query = query.Where(o => o.AddedDate <= end);
            //    }
            //    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            //}
        }
        /// <summary>
        /// 获取一家门店
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Result GetStoreName(int userId)
        {
            var store = "";

            List<string> brand = new List<string>();
            var str = GetStoreNameByUserID(userId, ref brand);
            if (!string.IsNullOrEmpty(str))
            {
                var storeCode1 = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                store = storeCode1[0].ToString();
                using (var db = new CRMEntities())
                {
                    var q = db.V_M_TM_SYS_BaseData_store.Where(p => p.StoreCode == store).FirstOrDefault();
                    store = q == null ? "" : q.StoreName;
                }
            }
            return new Result(true, "", store);
        }

        public static Result GetStoreNameByCode(string storecode)
        {
            var store = "";
            if (!string.IsNullOrEmpty(storecode))
            {
                using (var db = new CRMEntities())
                {
                    var q = db.V_M_TM_SYS_BaseData_store.Where(p => p.StoreCode == storecode).FirstOrDefault();
                    store = q == null ? "" : q.StoreName;
                }
            }
            return new Result(true, "", store);
        }
        /// <summary>
        /// 获取默认打印机
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Result GetPrinterName(int userId)
        {
            throw new NotImplementedException();
            //var printer = "";
            //List<string> brand = new List<string>();
            //var str = GetStoreNameByUserID(userId, ref brand);
            //if (!string.IsNullOrEmpty(str))
            //{
            //    var storeCode1 = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //    var storeCode = storeCode1[0].ToString();
            //    using (var db = new CRMEntities())
            //    {
            //        var printer1 = db.V_M_TM_SYS_BaseData_store.Where(p => p.StoreCode == storeCode).FirstOrDefault();
            //        if (printer1 != null)
            //        {
            //            printer = printer1.Printer;
            //        }
            //    }
            //}
            //return new Result(true, "", printer);
        }
        /// <summary>
        /// 根据登陆用户id获取门店名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetStoreNameByUserID(int userID, ref List<string> brandList)
        {
            using (CRMEntities db = new CRMEntities())
            {
                string store = "";
                List<string> brand = new List<string>();
                //根据用户名拿到用户ID,然后查看用户的数据角色ID,再然后从数据限制表中找到限制品牌或者门店
                var q1 = db.TR_AUTH_UserRole.Where(a => a.UserID == userID).ToList();
                var q22 = db.TM_AUTH_Role.ToList();
                var q33 = db.TM_AUTH_DataLimit.ToList();
                var q44 = db.V_M_TM_SYS_BaseData_store.ToList();
                if (q1 != null)
                {
                    var roleIds = new List<int>();
                    foreach (var item in q1)
                    {
                        var id = item.RoleID;
                        var q2 = q22.Where(p => p.RoleID == id && p.RoleType == "data").FirstOrDefault();
                        if (q2 != null)
                            roleIds.Add(q2.RoleID);
                    }
                    if (roleIds.Count > 0)
                    {
                        List<Rang> q3 = null;
                        foreach (var item in roleIds)
                        {
                            var temp = item.ToString();
                            q3 = q33.Where(a => a.HierarchyValue == temp).Select(a => new Rang { RangeType = a.RangeType, RangeValue = a.RangeValue }).ToList();
                            //q3 = (from a in db.TM_AUTH_DataLimit
                            //      where a.HierarchyValue == temp
                            //      select new Rang
                            //      {
                            //          RangeType = a.RangeType,
                            //          RangeValue = a.RangeValue
                            //      }).Distinct().ToList();
                        }
                        if (q3.Count > 0)
                        {
                            for (int i = 0; i < q3.Count; i++)
                            {
                                if (q3[i].RangeType == "store")
                                {
                                    if (store.IndexOf(q3[i].RangeValue) < 0)
                                        store += q3[i].RangeValue + ",";
                                }
                                else if (q3[i].RangeType == "brand")
                                {
                                    brand.Add(q3[i].RangeValue);
                                }
                            }
                            if (brand.Count > 0)
                            {
                                brandList = brand;
                                List<V_M_TM_SYS_BaseData_store> list = new List<V_M_TM_SYS_BaseData_store>();
                                foreach (var item in brand)
                                {
                                    var temp = item.ToString();
                                    //list = q44.Where(p => p.StoreBrandCode == temp).ToList();
                                    if (list.Count > 0)
                                    {
                                        foreach (var item1 in list)
                                        {
                                            if (store.IndexOf(item1.StoreCode) < 0)
                                                store += item1.StoreCode + ",";
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                //string[] storeCode = store.Split(',');
                return store;
            }

        }

        /// <summary>
        /// 获取登录用户在某个群组下的权限门店
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static Result GetAuthStoreByGroupUserID(int groupId, int userID)
        {
            List<string> brandList = new List<string>();
            var stores = GetStoreNameByUserID(userID, ref brandList);
            //List<string> storeLst = new List<string>();
            //if (!string.IsNullOrEmpty(stores))
            //    stores = stores.Remove(stores.Length - 1, 1);
            using (CRMEntities db = new CRMEntities())
            {
                var qAuthStores = (from a in db.V_M_TM_SYS_BaseData_store.Where(o => string.IsNullOrEmpty(stores) ? true : SqlFunctions.CharIndex(o.StoreCode, stores) > 0)
                                   join b in db.V_Sys_DataGroupRelation on a.DataGroupID equals b.SubDataGroupID
                                   where b.DataGroupID == groupId
                                   select a).ToList();
                return new Result(true, "", qAuthStores);
            }

        }
        private class Rang
        {
            public string RangeType { get; set; }
            public string RangeValue { get; set; }
        }

        internal static Result SendSms(string memberId, string mobile, int templateId, object para, string message)
        {
            string defaultUser = "sys";
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    StringBuilder sb = new StringBuilder();
                    List<Dictionary<string, string>> lstPara = (List<Dictionary<string, string>>)para;
                    sb.Append("{");
                    for (int i = 0; i < lstPara.Count; i++)
                    {
                        Dictionary<string, string> dict = lstPara[i];
                        sb.AppendFormat("\"{0\"}:\"{1}\"", dict["name"], dict["value"]);
                        if (i < lstPara.Count - 1)
                            sb.AppendFormat(",");
                    }
                    sb.Append("}");
                    var ent = new TM_Sys_SMSSendingQueue
                    {
                        Mobile = mobile,
                        Message = message,
                        MemberID = memberId,
                        AddedDate = DateTime.Now,
                        AddedUser = defaultUser,
                        Remark = "sys",
                        MsgPara = sb.ToString(),//JsonHelper.Serialize(para),
                        TempletID = templateId,
                        IsSent = false,
                    };
                    db.TM_Sys_SMSSendingQueue.Add(ent);
                    db.SaveChanges();

                    return new Result(true, "发送成功！");
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }


        internal static Result SendSmsStr(string memberId, string mobile, int templateId, string para, string message)
        {
            string defaultUser = "sys";
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    var ent = new TM_Sys_SMSSendingQueue
                    {
                        Mobile = mobile,
                        Message = message,
                        MemberID = memberId,
                        AddedDate = DateTime.Now,
                        AddedUser = defaultUser,
                        Remark = "sys",
                        MsgPara = para,
                        TempletID = templateId,
                        IsSent = false,
                    };
                    db.TM_Sys_SMSSendingQueue.Add(ent);
                    db.SaveChanges();

                    return new Result(true, "发送成功！");
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }
        /// <summary>
        /// 发送短信 系统固化短信
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="mobile"></param>
        /// <param name="subType"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static Result SendMsgBySysInit(string memberId, string mobile, string subType, int groupId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var qTemp = db.TM_Act_CommunicationTemplet.Where(o => o.IsSysInit == true && o.Type == "SMS" && o.SubType == subType && o.DataGroupID == groupId).FirstOrDefault();
                string sendMsg = "";
                int templateId = 0;
                if (qTemp != null)
                {
                    sendMsg = qTemp.BasicContent;
                    templateId = qTemp.TempletID;
                }
                Regex reg = new Regex(@"{[a-zA-Z \u4e00-\u9fa5]+}");
                MatchCollection mc = reg.Matches(sendMsg);
                List<Dictionary<string, string>> lstDict = new List<Dictionary<string, string>>();
                for (int i = 0; i < mc.Count; i++)
                {
                    string strRgx = mc[i].Value.ToString().TrimStart('{').TrimEnd('}');
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    var qAlias = db.TD_SYS_FieldAlias.Where(o => o.FieldDesc == strRgx).FirstOrDefault();
                    if (qAlias != null)
                    {
                        dict["name"] = qAlias.FieldAlias;
                        if (qAlias.ControlType == "date")
                        {
                            DateTime qSqlRet = db.Database.SqlQuery<DateTime>(string.Format("select {2} from {0} where memberId='{1}'", qAlias.TableName, memberId, qAlias.FieldName)).FirstOrDefault();
                            dict["value"] = qSqlRet.ToShortDateString();
                        }
                        else
                        {
                            string qSql = db.Database.SqlQuery<string>(string.Format("select Convert(varchar,{2}) from {0} where memberId='{1}'", qAlias.TableName, memberId, qAlias.FieldName)).FirstOrDefault();
                            dict["value"] = qSql;
                        }
                        lstDict.Add(dict);
                    }
                }
                if (lstDict != null && lstDict.Count > 0)
                {
                    Result ret = SendSms(memberId, mobile, templateId, lstDict, sendMsg);
                    return ret;
                }
                else
                    return new Result(false, "参数不正确，不能发送短信！");
            }
        }


        /// <summary>
        /// 发送短信 短信模板
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="mobile"></param>
        /// <param name="templateId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static Result SendMsgBySetting(string memberId, string mobile, int templateId, int groupId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var qTemp = db.TM_Act_CommunicationTemplet.Where(o => o.Type == "SMS" && o.TempletID == templateId && o.DataGroupID == groupId).FirstOrDefault();
                string sendMsg = "";
                if (qTemp != null)
                {
                    sendMsg = qTemp.BasicContent;
                    templateId = qTemp.TempletID;
                }
                Regex reg = new Regex(@"{[a-zA-Z \u4e00-\u9fa5]+}");
                MatchCollection mc = reg.Matches(sendMsg);
                List<Dictionary<string, string>> lstDict = new List<Dictionary<string, string>>();
                for (int i = 0; i < mc.Count; i++)
                {
                    string strRgx = mc[i].Value.ToString().TrimStart('{').TrimEnd('}');
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    var qAlias = db.TD_SYS_FieldAlias.Where(o => o.FieldDesc == strRgx).FirstOrDefault();
                    if (qAlias != null)
                    {
                        dict["name"] = qAlias.FieldAlias;
                        if (qAlias.ControlType == "date")
                        {
                            DateTime qSqlRet = db.Database.SqlQuery<DateTime>(string.Format("select {2} from {0} where memberId='{1}'", qAlias.TableName, memberId, qAlias.FieldName)).FirstOrDefault();
                            dict["value"] = qSqlRet.ToShortDateString();
                        }
                        else
                        {
                            string qSql = db.Database.SqlQuery<string>(string.Format("select Convert(varchar,{2}) from {0} where memberId='{1}'", qAlias.TableName, memberId, qAlias.FieldName)).FirstOrDefault();
                            dict["value"] = qSql;
                        }
                        lstDict.Add(dict);
                    }
                }
                if (lstDict != null && lstDict.Count > 0)
                {
                    Result ret = SendSms(memberId, mobile, templateId, lstDict, sendMsg);
                    return ret;
                }
                else
                    return new Result(false, "参数不正确，不能发送短信！");
            }
        }


        /// <summary>
        /// 会员积点转让短信
        /// </summary>
        /// <param name="memberIdFrom"></param>
        /// <param name="memberIdTo"></param>
        /// <param name="transDate"></param>
        /// <param name="pointAmount"></param>
        /// <param name="subType"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static Result SendMsgPointTrans(string memberIdFrom, string memberIdTo, DateTime transDate, decimal pointAmount, int groupId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var qTemp = db.TM_Act_CommunicationTemplet.Where(o => o.IsSysInit == true && o.Type == "SMS" && o.SubType == "pointTrans" && o.DataGroupID == groupId).FirstOrDefault();
                string sendMsg = "";
                int templateId = 0;
                if (qTemp != null)
                {
                    sendMsg = qTemp.BasicContent;
                    templateId = qTemp.TempletID;
                }
                string mobileFrom = "";
                string mobileTo = "";
                string cardNoFrom = "";
                string cardNoTo = "";
                string memNameFrom = "";
                string memNameTo = "";
                var qMemFrom = db.TM_Mem_Ext.Where(o => o.MemberID == memberIdFrom).FirstOrDefault();
                if (qMemFrom != null)
                {
                    mobileFrom = qMemFrom.Str_Attr_4 == null ? "" : qMemFrom.Str_Attr_4;
                    cardNoFrom = qMemFrom.Str_Attr_2 == null ? "" : qMemFrom.Str_Attr_2;
                    memNameFrom = qMemFrom.Str_Attr_3 == null ? "" : qMemFrom.Str_Attr_3;
                }
                var qMemTo = db.TM_Mem_Ext.Where(o => o.MemberID == memberIdTo).FirstOrDefault();
                if (qMemTo != null)
                {
                    mobileTo = qMemTo.Str_Attr_4 == null ? "" : qMemTo.Str_Attr_4;
                    cardNoTo = qMemTo.Str_Attr_2 == null ? "" : qMemTo.Str_Attr_2;
                    memNameTo = qMemTo.Str_Attr_3 == null ? "" : qMemTo.Str_Attr_3;
                }
                string para = "";
                StringBuilder sbPara = new StringBuilder();
                sbPara.Append("{");
                sbPara.Append("\"pointAmount\":\"" + pointAmount + "\",");
                sbPara.Append("\"transDate\":\"" + transDate.ToString() + "\",");
                sbPara.Append("\"fromName\":\"" + memNameFrom + "\",");
                sbPara.Append("\"fromCardNo\":\"" + cardNoFrom + "\",");
                sbPara.Append("\"toName\":\"" + memNameTo + "\",");
                sbPara.Append("\"toCardNo\":\"" + cardNoTo + "\"");
                //sbPara.AppendFormat("{\"pointAmount\":\"{0}\"}", pointAmount);
                //sbPara.AppendFormat("{\"transDate\":\"{0}\"}", transDate);
                //sbPara.AppendFormat("{\"fromName\":\"{0}\"}", memNameFrom);
                //sbPara.AppendFormat("{\"fromCardNo\":\"{0}\"}", cardNoFrom);
                //sbPara.AppendFormat("{\"toName\":\"{0}\"}", memNameTo);
                //sbPara.AppendFormat("{\"toCardNo\":\"{0}\"}", cardNoTo);
                sbPara.Append("}");
                //"{\"" + key + "\":\"" + value + "\"}";
                Result ret = SendSmsStr(memberIdFrom, mobileFrom, templateId, sbPara.ToString(), sendMsg);
                //Result ret2 = SendSmsStr(memberIdTo, mobileTo, templateId, sbPara.ToString(), sendMsg);
                return ret;
            }
        }

        /// <summary>
        /// 报表用到的City下拉框
        /// </summary>
        /// <returns></returns>
        public static Result GetReportCity()
        {
            using (var db = new CRMEntities())
            {
                var query = db.TD_SYS_Region.Where(p => p.RegionGrade == 2 && p.Enable == true);
                return new Result(true, "", query.ToList());
            }
        }



        public static Result GetBaseDataChannel(int dataGroupId)
        {
            using (var db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_channel.Where(p => p.DataGroupID == dataGroupId);
                return new Result(true, "", query.ToList());
            }
        }

        public static Result GetMemberGrade()
        {
            using (var db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_customerlevel.Where(p => p.BrandCodeCustomerLevel == "Kidsland");
                return new Result(true, "", query.ToList());
            }
        }


    }
}
