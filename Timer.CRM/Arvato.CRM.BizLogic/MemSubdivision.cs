using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.BizLogic
{
    public static class MemSubdivision
    {
        /// <summary>
        /// 查找会员细分
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Result GetMemberSubdivisionByKey(string sessionStr, string key)
        {
            Dictionary<string, object> dictSession = JsonHelper.Deserialize<Dictionary<string, object>>(sessionStr);
            AuthModel authModel = JsonHelper.Deserialize<AuthModel>(dictSession["auth"].ToString());
            int dataGroupID = authModel.DataGroupID == null ? 0 : (int)authModel.DataGroupID;
            //List<string> brand = new List<string>();
            //var store = Service.GetStoreNameByUserID(authModel.UserID, ref brand);//获取门店
            //List<string> storeLst = new List<string>();
            //string[] arrStore = store.Split(',');
            //if (arrStore != null && arrStore.Length > 0)
            //{
            //    foreach (string s in arrStore)
            //    {
            //        if (!string.IsNullOrEmpty(s))
            //            storeLst.Add(s);
            //    }
            //}
            //string key1 = "";
            //if (key != null && key.Length == 6 && key.Substring(4, 1) == "0")
            //{
            //    key1 = key.Substring(0, 4) + key.Substring(5, 1);
            //}
            //bool isBrand = false;
            //bool hasStore = false;
            //if (brand.Count > 0)
            //{
            //    isBrand = true;
            //}
            //if (storeLst != null && storeLst.Count > 0)
            //{
            //    hasStore = true;
            //}
            using (CRMEntities db = new CRMEntities())
            {
                //var query = from ms in db.TM_Mem_Subdivision.Where(o => isBrand == true ? true : (hasStore == true ? storeLst.Contains(o.StoreCode) : true))
                //(string.IsNullOrEmpty(store) ? true : SqlFunctions.CharIndex(o.StoreCode, store) > 0))//.FilterDataByAuth(authModel.RoleIDs, (int)authModel.CurPageID, (int)authModel.DataGroupID)
                //var query = from ms in db.TM_Mem_Subdivision
                //            join b in db.V_Sys_DataGroupRelation.Where(p => p.DataGroupID == dataGroupID) on ms.DataGroupID equals b.SubDataGroupID
                //            where string.IsNullOrEmpty(key) ? true : ms.SubdivisionName.Contains(key)
                //            || ms.SubdivisionDesc.Contains(key)
                //            || (SqlFunctions.DateName("YY", ms.AddedDate) + SqlFunctions.DateName("MM", ms.AddedDate) + SqlFunctions.DateName("DD", ms.AddedDate)).Contains(key)
                //            where string.IsNullOrEmpty(key1) ? true : (SqlFunctions.DateName("YY", ms.AddedDate) + SqlFunctions.DateName("MM", ms.AddedDate) + SqlFunctions.DateName("DD", ms.AddedDate)).Contains(key1)
                //            orderby ms.AddedDate descending
                //            select ms;
                var query = from ms in db.TM_Mem_Subdivision
                            join b in db.V_Sys_DataGroupRelation.Where(p => p.DataGroupID == dataGroupID) on ms.DataGroupID equals b.SubDataGroupID
                            where string.IsNullOrEmpty(key) ? true : ms.SubdivisionName.Contains(key)
                            orderby ms.AddedDate descending
                            select ms;
                //把数据列表转化为所要的数据源
                List<TreeDataSource> nodes = new List<TreeDataSource>();
                TreeDataSource node = null;
                foreach (var item in query)
                {
                    node = new TreeDataSource();
                    node.nodeId = item.SubdivisionID.ToString();
                    node.nodePId = item.SubdivisionType.Trim();//item.DataGroupID.ToString();
                    node.nodeGrade = item.DataGroupID;
                    node.nodeName = item.SubdivisionName.Trim();
                    nodes.Add(node);
                }
                //var qGroup = db.TM_SYS_DataGroup.Select(p => new { p.DataGroupID, p.DataGroupName }).ToList();
                var qGroup = db.TD_SYS_BizOption.Where(p => p.OptionType == "SubdivisionType").ToList();
                foreach (var g in qGroup)
                {
                    var qNode = nodes.Where(p => p.nodePId.Trim() == g.OptionValue.Trim()).FirstOrDefault();
                    if (qNode != null)
                    {
                        node = new TreeDataSource();
                        node.nodeId = g.OptionValue;//g.DataGroupID.ToString();
                        node.nodePId = "0";
                        node.nodeGrade = 0;
                        node.nodeName = g.OptionText;
                        nodes.Add(node);
                    }
                }

                var qGroupPersonalize = db.TM_SYS_Class.Where(c => c.ClassType.Trim().Equals("1") && c.UserID == authModel.UserID
                    && c.DataGroupID == authModel.DataGroupID).ToList();
                foreach (var g in qGroupPersonalize)
                {
                    var qNode = nodes.Where(p => p.nodePId.Trim() == g.ClassID.ToString()).FirstOrDefault();
                    if (qNode != null)
                    {
                        node = new TreeDataSource();
                        node.nodeId = g.ClassID.ToString();//g.DataGroupID.ToString();
                        node.nodePId = "0";
                        node.nodeGrade = 0;
                        node.nodeName = g.ClassName.Trim();
                        nodes.Add(node);
                    }
                }

                var levelOne = nodes.Where(o => o.nodeGrade == 0).ToList();

                var result = new List<TreeNode>();
                foreach (var cate in levelOne)
                {
                    var res = Arvato.CRM.BizLogic.Service.CreateTreeNodeData(nodes, cate.nodeId);
                    if (node != null)
                    {
                        result.Add(res);
                    }
                }

                return new Result(true, "", result);
            }
        }

        #region 根据ID查找会员细分
        /// <summary>
        /// 根据ID查找会员细分
        /// </summary>
        /// <param name="msid"></param>
        /// <returns></returns>
        public static Result GetMemberSubdivisionById(Guid msid)
        {
            using (var db = new CRMEntities())
            {
                var query = from ms in db.TM_Mem_Subdivision
                            join i in db.TM_Mem_SubdivisionInstance
                            on ms.CurSubdivisionInstanceID equals i.SubdivisionInstanceID
                            into ms_ljoin_i
                            from msi in ms_ljoin_i.DefaultIfEmpty()
                            where ms.SubdivisionID == msid
                            orderby msi.AddedDate descending
                            select new
                            {
                                SubdivisionID = ms.SubdivisionID,
                                SubdivisionName = ms.SubdivisionName,
                                CurSubdivisionInstanceID = ms.CurSubdivisionInstanceID,
                                Enable = ms.Enable,
                                Condition = ms.Condition,
                                DataGroupID = ms.DataGroupID,
                                AddedDate = ms.AddedDate,
                                AddedUser = ms.AddedUser,
                                SubdivisionDesc = ms.SubdivisionDesc,
                                Schedule = ms.Schedule,
                                SubdivisionType = ms.SubdivisionType,
                                SubDevDataType = ms.SubDevDataType,
                                ComputerTime = msi.ComputerTime,
                                LastComputerDate = msi.LastComputerDate
                                //StoreCode = ms.StoreCode
                            };
                return new Result(true, "", query.FirstOrDefault());
            }
        }
        #endregion

        /// <summary>
        /// 获取细分会员类型
        /// </summary>
        /// <returns></returns>
        public static Result GetSubdivisionType()
        {
            using (var db = new CRMEntities())
            {
                var query = db.TD_SYS_BizOption.Where(p => p.OptionType == "SubdivisionType").OrderBy(p => p.Sort).ToList();
                return new Result(true, "", query);
            }
        }

        #region 添加会员细分
        /// <summary>
        /// 添加会员细分
        /// </summary>
        /// <param name="memsubd"></param>
        /// <param name="importObj"></param>
        /// <returns></returns>
        public static Result AddMemberSubdivision(TM_Mem_Subdivision memsubd, string importObj)
        {
            using (var db = new CRMEntities())
            {
                try
                {
                    db.BeginTransaction();
                    var ms = db.TM_Mem_Subdivision.Where(s => s.SubdivisionName == memsubd.SubdivisionName).FirstOrDefault();
                    if (ms != null)
                    {
                        return new Result(false, "名称已存在");
                    }

                    var retImportMsg = "";
                    if (memsubd.SubDevDataType == "2")
                    {
                        if (!string.IsNullOrEmpty(importObj))
                        {
                            Result retImport = ImportMemberSubdivision(memsubd.SubdivisionID, importObj);
                            if (retImport.IsPass)
                            {
                                Dictionary<string, string> dict = JsonHelper.Deserialize<Dictionary<string, string>>(retImport.Obj[0].ToString());
                                retImportMsg = " 成功导入" + dict["Count"] + "条数据";
                                memsubd.CurSubdivisionInstanceID = new Guid(dict["InstanceId"].ToString());
                            }
                            else
                            {
                                return new Result(false, retImport.MSG);
                            }
                        }

                    }

                    memsubd.ModifiedDate = DateTime.Now;
                    db.TM_Mem_Subdivision.Add(memsubd);

                    db.SaveChanges();
                    db.Commit();
                    return new Result(true, "新增会员细分成功！" + retImportMsg, memsubd.SubdivisionID);
                }
                catch (Exception ex)
                {
                    return new Result(false, ex.Message);
                }
            }
        }
        #endregion

        #region 编辑会员细分

        /// <summary>
        /// 编辑或添加会员细分
        /// </summary>
        /// <param name="memsubdStr"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Result EditOrAddMemberSubdivision(string memsubdStr, string userId, string importObj)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    TM_Mem_Subdivision memsubd = JsonHelper.Deserialize<TM_Mem_Subdivision>(memsubdStr);
                    //TM_Mem_Subdivision s;
                    //if (memsubd.SubdivisionID == Guid.Empty)
                    //{
                    //    s = new TM_Mem_Subdivision();
                    //    s.AddedUser = userId;
                    //    s.AddedDate = DateTime.Now;
                    //}
                    //else
                    //{
                    //    //var qSubd= from ms in db.TM_Mem_Subdivision
                    //    var result = GetMemberSubdivisionById(memsubd.SubdivisionID);
                    //    s = JsonHelper.Deserialize<TM_Mem_Subdivision>(result.Obj[0].ToString());
                    //}
                    //s.ModifiedUser = userId;
                    //s.ModifiedDate = DateTime.Now;

                    Result res = new Result(); ;
                    if (memsubd.SubdivisionID == Guid.Empty)
                    {
                        memsubd.AddedUser = userId;
                        memsubd.AddedDate = DateTime.Now;
                        memsubd.ModifiedUser = userId;
                        memsubd.ModifiedDate = DateTime.Now;
                        memsubd.SubdivisionID = Guid.NewGuid();
                        memsubd.Condition = string.IsNullOrEmpty(memsubd.Condition) ? "" : memsubd.Condition;
                        res = AddMemberSubdivision(memsubd, importObj);
                    }
                    else
                    {
                        memsubd.ModifiedUser = userId;
                        memsubd.ModifiedDate = DateTime.Now;
                        memsubd.Condition = string.IsNullOrEmpty(memsubd.Condition) ? "" : memsubd.Condition;
                        res = EditMemberSubdivision(memsubd);
                    }
                    return res;

                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }
        /// <summary>
        /// 编辑会员细分
        /// </summary>
        /// <param name="memsubd"></param>
        /// <returns></returns>
        public static Result EditMemberSubdivision(TM_Mem_Subdivision memsubd)
        {
            using (var db = new CRMEntities())
            {
                try
                {
                    var ms = db.TM_Mem_Subdivision.Where(s => s.SubdivisionName == memsubd.SubdivisionName && s.SubdivisionID != memsubd.SubdivisionID).FirstOrDefault();
                    if (ms != null)
                    {
                        return new Result(false, "名称已存在");
                    }
                    ms = db.TM_Mem_Subdivision.Where(s => s.SubdivisionID == memsubd.SubdivisionID).FirstOrDefault();
                    if (ms == null)
                    {
                        return new Result(false, "找不到记录");
                    }
                    if (ms.CurSubdivisionInstanceID != null)
                    {
                        ms.SubdivisionType = memsubd.SubdivisionType;
                        db.Entry(ms).State = System.Data.EntityState.Modified;
                        db.SaveChanges();
                        return new Result(true, "更新会员细分成功！", memsubd.SubdivisionID);
                        //return new Result(false, "已经执行过的会员细分不能编辑");
                    }

                    ms.SubdivisionName = memsubd.SubdivisionName;
                    ms.SubdivisionDesc = memsubd.SubdivisionDesc;
                    ms.Schedule = memsubd.Schedule;
                    ms.Enable = memsubd.Enable;
                    ms.ModifiedUser = memsubd.ModifiedUser;
                    ms.ModifiedDate = DateTime.Now;
                    ms.DataGroupID = memsubd.DataGroupID;
                    ms.SubdivisionType = memsubd.SubdivisionType;
                    ms.Condition = memsubd.Condition;
                    db.Entry(ms).State = System.Data.EntityState.Modified;
                    db.SaveChanges();
                    return new Result(true, "更新会员细分成功！", memsubd.SubdivisionID);
                }
                catch (Exception ex)
                {
                    return new Result(false, ex.Message);
                }
            }
        }
        #endregion

        #region 删除会员细分
        /// <summary>
        /// 删除会员细分
        /// </summary>
        /// <param name="memsubd"></param>
        /// <returns></returns>
        public static Result DeleMemberSubdivision(Guid msid)
        {
            using (var db = new CRMEntities())
            {
                try
                {
                    TM_Mem_Subdivision d = db.TM_Mem_Subdivision.FirstOrDefault(s => s.SubdivisionID == msid);
                    if (d != null)
                    {
                        db.BeginTransaction();
                        var queryActivity = from a in db.TM_Act_Subdivision
                                            where a.SubdivisionID == d.SubdivisionID
                                            select a;
                        if (queryActivity.Count() > 0)
                        {
                            return new Result(false, "该会员细分已经被市场活动调用，不能删除！");
                        }
                        var qInstance = db.TM_Mem_SubdivisionInstance.Where(o => o.SubdivisionID == msid).ToList();
                        if (qInstance.Count > 0)
                        {
                            foreach (var q in qInstance)
                            {
                                db.Database.ExecuteSqlCommand(string.Format(" DROP Table [{0}]", q.TableName));
                                db.TM_Mem_SubdivisionInstance.Remove(q);
                            }
                        }

                        db.TM_Mem_Subdivision.Remove(d);
                        db.SaveChanges();
                        db.Commit();
                        return new Result(true, "删除成功!");
                    }
                    else
                    {
                        return new Result(false, "删除失败，没有找到此编号的会员细分");
                    }
                }
                catch (Exception ex)
                {
                    return new Result(false, ex.Message);
                }
            }
        }
        #endregion

        #region 激活/取消激活会员细分
        /// <summary>
        /// 激活/取消激活会员细分
        /// </summary>
        /// <param name="msId"></param>
        /// <param name="isEnable"></param>
        /// <param name="modifiedUser"></param>
        /// <returns></returns>
        public static Result EnableMemberSubdivision(Guid msId, bool isEnable, string modifiedUser)
        {
            using (var db = new CRMEntities())
            {
                try
                {
                    var s = db.TM_Mem_Subdivision.Where(a => a.SubdivisionID == msId).FirstOrDefault();
                    if (s != null)
                    {
                        if (s.SubDevDataType == "2")//人工导入细分
                        {
                            return new Result(false, "人工导入细分无需激活！", "");
                        }
                        if (string.IsNullOrEmpty(s.Condition))
                        {
                            return new Result(false, "请先配置会员细分规则，才能激活！", "");
                        }
                        s.Enable = isEnable;
                        s.ModifiedUser = modifiedUser;
                        s.ModifiedDate = DateTime.Now;
                        db.Entry(s).State = System.Data.EntityState.Modified;
                        db.SaveChanges();
                        return new Result(true, "操作成功！", isEnable);
                    }
                    else
                    {
                        return new Result(false, "找不到记录");
                    }
                }
                catch (Exception ex)
                {
                    return new Result(false, ex.Message);
                }
            }
        }
        #endregion

        #region 细分规则相关

        /// <summary>
        /// 获取字典表数据通用方法
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Result GetCommonOptionData(string type)
        {
            using (var db = new CRMEntities())
            {
                var query = db.TD_SYS_BizOption.Where(p => p.OptionType == type && p.Enable).OrderBy(p => p.Sort).ToList();
                return new Result(true, "", query);
            }
        }


        #region 获取所有会员细分规则模板
        public static Result GetMemSubdFilterAll()
        {
            using (var db = new CRMEntities())
            {
                var query = from t in db.TM_Mem_Subdivision orderby t.AddedDate descending select t;
                return new Result(true, "", query.ToList());
            }
        }
        #endregion

        #region 获取会员细分过滤条件所有左值相关参数
        /// <summary>
        /// 获取会员细分过滤条件所有左值相关参数
        /// </summary>
        /// <returns></returns>
        public static Result GetSubdLeftValuesAll()
        {
            using (var db = new CRMEntities())
            {
                var queryF = from f in db.TD_SYS_FieldAlias
                             where f.IsFilterBySubdivision
                             orderby f.FieldDesc
                             select new
                             {
                                 f.AliasID,
                                 f.AliasKey,
                                 f.AliasType,
                                 f.ControlType,
                                 f.FieldAlias,
                                 f.FieldDesc,
                                 f.FieldType,
                                 f.DictTableName,
                                 f.DictTableType,
                                 f.TableName,
                                 f.Reg,
                                 f.ParameterCount,
                                 f.IsDynamicAlias
                             };

                var queryP = from p in db.TD_SYS_FieldAliasParameter
                             join f in db.TD_SYS_FieldAlias on p.AliasID equals f.AliasID
                             where f.IsFilterBySubdivision
                             select new
                             {
                                 p.ParaID,
                                 p.AliasID,
                                 f.FieldAlias,
                                 p.Reg,
                                 p.ParaIndex,
                                 p.FieldType,
                                 p.ControlType,
                                 p.DictTableName,
                                 p.DictTableType,
                                 p.ParameterName,
                                 p.UIIndex,
                                 p.IsRequired,
                                 p.GroupType,
                                 p.flag
                             };

                var poList = db.TD_SYS_FieldAliasParameter.Select(o => new
                {
                    o.DictTableName,
                    o.DictTableType
                }).Distinct().ToList();


                StringBuilder poListStr = new StringBuilder();
                poListStr.Append("[");
                StringBuilder strjson = new StringBuilder();
                foreach (var po in poList)
                {
                    poListStr.Append("{");
                    strjson = new StringBuilder();
                    poListStr.Append(string.IsNullOrEmpty(po.DictTableName) ? "" : po.DictTableName + po.DictTableType.Replace(",", "") + ":[");
                    List<OptionItem> vlist;
                    var res = GetRightSelectData(po.DictTableName, po.DictTableType, -1);
                    if (res.IsPass)
                    {
                        vlist = JsonHelper.Deserialize<List<OptionItem>>(JsonHelper.Serialize(res.Obj[0]));
                        foreach (var v in vlist)
                        {
                            strjson.Append(",{sv:'" + v.OptionValue + "',st:'" + v.OptionText + "'}");
                        }
                        if (strjson.Length > 0)
                        {
                            poListStr.Append(strjson.ToString().Substring(1));
                        }
                        poListStr.Append("]");
                    }
                    else
                    {
                        if (res.Obj != null)
                        {
                            poListStr.Append("]");
                        }
                    }
                    poListStr.Append("},");
                }
                poListStr.Append("]");

                return new Result(true, "", new List<object> { queryF.ToList(), queryP.ToList(), poListStr.ToString() });
            }
        }
        #endregion

        #region 会员细分规则右值

        /// <summary>
        /// 根据左值获取右值下拉数据
        /// </summary>
        /// <param name="table"></param>
        /// <param name="dictType"></param>
        /// <returns></returns>
        public static Result GetRightSelectData(string table, string dictType, int groupId)
        {
            using (var db = new CRMEntities())
            {
                StringBuilder sbsql = new StringBuilder();
                if (table == "TD_SYS_BizOption")
                {
                    if (dictType == "CustomerLevel")
                    {
                        sbsql.AppendFormat("select * from {0} where OptionType='{1}' and DataGroupID={2}", table, dictType, groupId);
                    }
                    else
                    {
                        sbsql.AppendFormat("select * from {0} where OptionType='{1}'", table, dictType);
                    }
                    var q = db.Database.SqlQuery<OptionItem>(sbsql.ToString(), DBNull.Value);
                    return new Result(true, "", q.ToList());
                }
                else
                {
                    if (!string.IsNullOrEmpty(table) && !string.IsNullOrEmpty(dictType))
                    {
                        sbsql.AppendFormat("select cast({1} as nvarchar(100)) as OptionValue,{2} as OptionText from {0} ", table, dictType.Split(',')[0], dictType.Split(',')[1]);
                        var q = db.Database.SqlQuery<OptionItem>(sbsql.ToString(), DBNull.Value).ToList();
                        foreach (var o in q)
                        {
                            o.OptionText = Utility.JsonHelper.JsonCharFilter(o.OptionText);
                        }
                        return new Result(true, "", q);
                    }
                    else
                    {
                        return new Result(false, "参数错误", null);
                    }
                }
            }
        }

        public static Result GetRightSelectDataByLimit(string fa, int groupId, string rid, int pageId)
        {
            using (var db = new CRMEntities())
            {
                List<string> lrid = JsonHelper.Deserialize<List<string>>(rid);
                var alias = db.TD_SYS_FieldAlias.Where(p => p.FieldAlias == fa).FirstOrDefault();
                StringBuilder sbsql = new StringBuilder();
                if (alias.DictTableName == "TD_SYS_BizOption")
                {
                    if (alias.DictTableType == "CustomerLevel")
                    {
                        sbsql.AppendFormat("select * from {0} where OptionType='{1}' and DataGroupID={2}", alias.DictTableName, alias.DictTableType, groupId);
                    }
                    else
                    {
                        sbsql.AppendFormat("select * from {0} where OptionType='{1}'", alias.DictTableName, alias.DictTableType);
                    }
                    var q = db.Database.SqlQuery<OptionItem>(sbsql.ToString(), DBNull.Value);
                    return new Result(true, "", q.ToList());
                }
                else
                {
                    if (!string.IsNullOrEmpty(alias.DictTableName) && !string.IsNullOrEmpty(alias.DictTableType))
                    {
                        var dl = db.TM_AUTH_DataLimit.Where(p => (p.PageID == pageId || p.PageID == 9999) && lrid.Contains(p.HierarchyValue)).ToList();
                        var datalimit = alias.DictTableName.Split('_').LastOrDefault();
                        var fieldalias = db.TD_SYS_FieldAlias.Where(p => p.DataLimitType == datalimit && p.AliasKey == datalimit).ToList();
                        sbsql.AppendFormat("select distinct cast({1} as nvarchar(100)) as OptionValue,{2} as OptionText from {0} ", alias.DictTableName, alias.DictTableType.Split(',')[0], alias.DictTableType.Split(',')[1]);
                        string where = "";

                        foreach (var d in dl)
                        {
                            foreach (var f in fieldalias)
                            {
                                if (d.RangeType == f.DataLimitType)
                                {
                                    if (where == "")
                                        where += "where " + f.FieldAlias + " = '" + d.RangeValue + "'";
                                    else
                                        where += " or " + f.FieldAlias + " = '" + d.RangeValue + "'";
                                }
                            }
                        }
                        sbsql.AppendLine(where);
                        var q = db.Database.SqlQuery<OptionItem>(sbsql.ToString(), DBNull.Value).ToList();
                        foreach (var o in q)
                        {
                            o.OptionText = Utility.JsonHelper.JsonCharFilter(o.OptionText);
                        }
                        return new Result(true, "", q);
                    }
                    else
                    {
                        return new Result(false, "参数错误", null);
                    }
                }
            }
        }


        /// <summary>
        /// 根据会员细分过滤条件左值获取右值项
        /// </summary>
        /// <param name="rightValCfgs"></param>
        /// <returns></returns>
        public static Result GetMemSubdRightValues(string rightValStr, int groupId)
        {
            List<MemSubdRValTypeModel> rightValCfgs = JsonHelper.Deserialize<List<MemSubdRValTypeModel>>(rightValStr);
            if (rightValCfgs == null)
                return new Result(false, "", null);
            StringBuilder strRightVal = new StringBuilder();
            StringBuilder strjson = new StringBuilder();
            foreach (var c in rightValCfgs)
            {
                strjson = new StringBuilder();
                if (strRightVal.Length == 0)
                {
                    strRightVal.Append(c.FieldAlias + ":[");
                }
                else
                {
                    strRightVal.Append("," + c.FieldAlias + ":[");
                }

                if (c.TableName == "TD_SYS_BizOption")
                {
                    List<TD_SYS_BizOption> vlist;
                    var res = GetRightSelectData(c.TableName, c.TableType, groupId);

                    if (res.IsPass)
                    {
                        vlist = JsonHelper.Deserialize<List<TD_SYS_BizOption>>(JsonHelper.Serialize(res.Obj[0]));
                        foreach (var v in vlist)
                        {
                            strjson.Append(",{sv:'" + v.OptionValue + "',st:'" + v.OptionText + "'}");
                        }
                        if (strjson.Length > 0)
                        {
                            strRightVal.Append(strjson.ToString().Substring(1));
                        }
                        strRightVal.Append("]");
                    }
                    else
                    {
                        strRightVal.Append("]");
                    }

                }
                else
                {
                    List<OptionItem> vlist;
                    var res = GetRightSelectData(c.TableName, c.TableType, groupId);
                    if (res.IsPass)
                    {
                        vlist = JsonHelper.Deserialize<List<OptionItem>>(JsonHelper.Serialize(res.Obj[0]));
                        foreach (var v in vlist)
                        {
                            strjson.Append(",{sv:'" + v.OptionValue + "',st:'" + v.OptionText + "'}");
                        }
                        if (strjson.Length > 0)
                        {
                            strRightVal.Append(strjson.ToString().Substring(1));
                        }
                        strRightVal.Append("]");
                    }
                    else
                    {
                        strRightVal.Append("]");
                    }
                }
            }
            return new Result(true, "", strRightVal.ToString());
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
        /// <param name="registerStore"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetMemberSubdResultByPage(Guid currSubdId, string memCard, string memName, string mobile, string registerStoreCode, string table, string subDevDataType, string dp)
        {
            using (var db = new CRMEntities())
            {
                int amount = 0;
                string Condition = "";
                string TableName = "";
                string curSubdIdStr = "";
                if (currSubdId != null)
                {
                    curSubdIdStr = currSubdId.ToString();
                    if (subDevDataType == "2")
                        curSubdIdStr = curSubdIdStr.Replace("-", "");
                }
                if (string.IsNullOrEmpty(table))
                {
                    TableName = "TM_Mem_SubdivideResult_" + curSubdIdStr;
                }
                else
                    TableName = table;
                DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
                var res = new DatatablesSourceVsPage();
                List<MemSubdivisionResultModel> lst = new List<MemSubdivisionResultModel>();
                if (db.Database.SqlQuery<int>("select count(1) from sysobjects where name = '" + TableName + "' and type = 'U'").FirstOrDefault() == 1)
                {
                    string strSQL = "select top " + myDp.iDisplayLength + " MSRID,MemberID,CustomerName,isnull(CustomerMobile,'') CustomerMobile,CustomerEmail,MemberCardNo,Gender,RegisterDate,RegisterStoreCode,StoreName "
                        + " from (select row_number() over (order by MSRID) as RowIndex,a.*,b.MemberCardNo,b.CustomerName,b.Gender,b.CustomerMobile,b.CustomerEmail,b.RegisterDate,b.RegisterStoreCode,c.StoreName "
                        + "  from [" + TableName + "] a join dbo.V_S_TM_Mem_Ext b on a.MemberID = b.MemberID "
                        + " left join dbo.V_M_TM_SYS_BaseData_store c on b.RegisterStoreCode = c.StoreCode "
                        + " where 1=1 ";

                    if (!string.IsNullOrEmpty(memCard))
                    {
                        Condition += " and b.MemberCardNo like '%" + memCard + "%'";
                    }
                    if (!string.IsNullOrEmpty(memName))
                    {
                        Condition += " and b.CustomerName like '%" + memName + "%'";
                    }
                    if (!string.IsNullOrEmpty(mobile))
                    {
                        Condition += " and b.CustomerMobile like '%" + mobile + "%'";
                    }
                    if (!string.IsNullOrEmpty(registerStoreCode))
                    {
                        Condition += " and b.RegisterStoreCode ='" + registerStoreCode + "'";
                    }
                    strSQL += Condition + " ) as t where RowIndex>" + myDp.iDisplayStart; ;
                    lst = db.Database.SqlQuery<MemSubdivisionResultModel>(strSQL).ToList();

                    amount = db.Database.SqlQuery<int>("select count(1) from [" + TableName + "] a join dbo.V_S_TM_Mem_Ext b "
                            + " on a.MemberID = b.MemberID left join dbo.V_M_TM_SYS_BaseData_store c "
                            + " on b.RegisterStoreCode = c.StoreCode where 1=1 " + Condition).FirstOrDefault();

                    res.iDisplayStart = myDp.iDisplayStart;
                    res.iDisplayLength = myDp.iDisplayLength;
                    res.iTotalRecords = amount;
                    res.aaData = lst;
                }
                return new Result(true, "", new List<object> { res });
            }
        }

        /// <summary>
        /// 根据会员细分获取动态表数据
        /// </summary>
        /// <param name="subdId"></param>
        /// <returns></returns>
        public static Result GetSubdDynamicTable(Guid subdId)
        {
            using (var db = new CRMEntities())
            {
                var query = db.TM_Mem_SubdivisionInstance.Where(p => p.SubdivisionID == subdId).OrderByDescending(p => p.LastComputerDate).Take(10).ToList();
                if (query != null && query.Count > 0)
                    return new Result(true, "", query);
                else
                    return new Result(false, "", null);
            }
        }

        #region 会员细分导出——新版

        /// <summary>
        /// 加载会员细分导出弹窗的下拉框
        /// </summary>
        /// <returns></returns>
        public static Result loadSubExportSelect()
        {
            using (var db = new CRMEntities())
            {
                var query = db.TD_SYS_FieldAlias.Where(i => i.AliasType == "MemberExt").Select(i => new { Code = i.AliasID, Name = i.FieldDesc });
                return new Result(true, "", query.ToList());
            }
        }

        /// <summary>
        /// 加载会员细分导出弹窗的表格
        /// </summary>
        /// <returns></returns>
        public static Result loadSubExportTable(Guid SubdivisionID, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (var db = new CRMEntities())
            {
                var query = from a in db.TM_Mem_SubdivisionExport
                            join b in db.TD_SYS_FieldAlias on a.FieldAliasID equals b.AliasID into ba
                            from b in ba.DefaultIfEmpty()
                            where a.SubdivisionID == SubdivisionID
                            select new
                            {
                                ID = a.ID,
                                Code = b.FieldAlias,
                                Name = b.FieldDesc
                            };
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }

        /// <summary>
        /// 会员细分导出框添加数据
        /// </summary>
        /// <param name="FieldAliasID"></param>
        /// <param name="SubdivisionID"></param>
        /// <returns></returns>
        public static Result AddSubExport(int FieldAliasID, Guid SubdivisionID, int UserId)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var queryCount = db.TM_Mem_SubdivisionExport.Count(i => i.FieldAliasID == FieldAliasID && i.SubdivisionID == SubdivisionID);
                    if (queryCount > 0)
                    {
                        return new Result(false, "该列已经存在");
                    }
                    TM_Mem_SubdivisionExport se = new TM_Mem_SubdivisionExport();
                    se.ID = Guid.NewGuid().ToString("N");
                    se.FieldAliasID = FieldAliasID;
                    se.SubdivisionID = SubdivisionID;
                    se.AddedUser = UserId.ToString();
                    se.AddedDate = DateTime.Now;
                    se.ModifiedUser = UserId.ToString();
                    se.ModifiedDate = DateTime.Now;
                    db.TM_Mem_SubdivisionExport.Add(se);
                    db.SaveChanges();
                    return new Result(true, "添加成功");
                }
            }
            catch (Exception ex)
            {
                return new Result(false, "添加失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 会员细分导出框删除数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Result deleteSubExportCol(string ID, int UserId)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var query = db.TM_Mem_SubdivisionExport.Find(ID);
                    db.TM_Mem_SubdivisionExport.Remove(query);
                    db.SaveChanges();
                    return new Result(true, "删除成功");
                }
            }
            catch
            {

                return new Result(false, "删除失败");
            }
        }
       
        public static Result ExportMemberSubdResult(Guid currSubdId, string memCard, string memName, string mobile, string registerStoreCode, string dynamicTable, string subDevDataType)
        {
            using (var db = new CRMEntities())
            {
                string Condition = "";
                string TableName = "";
                string curSubdIdStr = "";
                if (currSubdId != null)
                {
                    curSubdIdStr = currSubdId.ToString();
                    if (subDevDataType == "2")
                        curSubdIdStr = curSubdIdStr.Replace("-", "");
                }
                if (string.IsNullOrEmpty(dynamicTable))
                {
                    TableName = "TM_Mem_SubdivideResult_" + curSubdIdStr;
                }
                else
                    TableName = dynamicTable;

                List<V_U_TM_Mem_Info> lst = new List<V_U_TM_Mem_Info>();
                if (db.Database.SqlQuery<int>("select count(1) from sysobjects where name = '" + TableName + "' and type = 'U'").FirstOrDefault() == 1)
                {
                    string strSQL = "select * from [" + TableName + "] a left join dbo.V_U_TM_Mem_Info b on a.MemberID = b.MemberID where 1=1";
                    if (!string.IsNullOrEmpty(memCard))
                    {
                        Condition += " and b.MemberCardNo like '%" + memCard + "%'";
                    }
                    if (!string.IsNullOrEmpty(memName))
                    {
                        Condition += " and b.CustomerName like '%" + memName + "%'";
                    }
                    if (!string.IsNullOrEmpty(mobile))
                    {
                        Condition += " and b.CustomerMobile like '%" + mobile + "%'";
                    }
                    if (!string.IsNullOrEmpty(registerStoreCode))
                    {
                        Condition += " and b.RegisterStoreCode ='" + registerStoreCode + "'";
                    }
                    strSQL += Condition;
                    lst = db.Database.SqlQuery<V_U_TM_Mem_Info>(strSQL).ToList();
                }
                if (lst == null || lst.Count == 0)
                    return new Result(false, "", null);
                return new Result(true, "", lst);
            }
        }

        public static Result loadSubExportColumn(Guid currSubdId)
        {
            using (var db = new CRMEntities())
            {
                var query = from a in db.TM_Mem_SubdivisionExport
                            join b in db.TD_SYS_FieldAlias on a.FieldAliasID equals b.AliasID into ba
                            from b in ba.DefaultIfEmpty()
                            where a.SubdivisionID == currSubdId
                            select new
                            {
                                ID = a.ID,
                                Code = b.FieldAlias,
                                Name = b.FieldDesc
                            };
                return new Result(true, "", query.ToList());
            }

        }

        #endregion

        #region 会员细分导出——旧版



        /// <summary>
        /// 会员细分结果导出
        /// </summary>
        /// <param name="currSubdId"></param>
        /// <param name="memCard"></param>
        /// <param name="memName"></param>
        /// <param name="mobile"></param>
        /// <param name="registerStoreCode"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        //public static Result ExportMemberSubdResult(Guid currSubdId, string memCard, string memName, string mobile, string registerStoreCode, string dynamicTable, string subDevDataType)
        //{
        //    using (var db = new CRMEntities())
        //    {
        //        string Condition = "";
        //        string TableName = "";
        //        string curSubdIdStr = "";
        //        if (currSubdId != null)
        //        {
        //            curSubdIdStr = currSubdId.ToString();
        //            if (subDevDataType == "2")
        //                curSubdIdStr = curSubdIdStr.Replace("-", "");
        //        }
        //        if (string.IsNullOrEmpty(dynamicTable))
        //        {
        //            TableName = "TM_Mem_SubdivideResult_" + curSubdIdStr;
        //        }
        //        else
        //            TableName = dynamicTable;

        //        List<MemSubdivisionResultModel> lst = new List<MemSubdivisionResultModel>();
        //        if (db.Database.SqlQuery<int>("select count(1) from sysobjects where name = '" + TableName + "' and type = 'U'").FirstOrDefault() == 1)
        //        {
        //            string strSQL = "select a.MemberID,b.CustomerName,b.CustomerMobile,b.CustomerEmail,b.MemberCardNo,Gender,RegisterDate,RegisterStoreCode,StoreName "
        //                //+ " from (select row_number() over (order by MSRID) as RowIndex,a.*,b.MemberCardNo,b.CustomerName,b.Gender,b.CustomerMobile,b.CustomerEmail,b.RegisterDate,b.RegisterStoreCode,c.StoreName "
        //                + "  from [" + TableName + "] a left join dbo.V_S_TM_Mem_Ext b on a.MemberID = b.MemberID "
        //                + " left join dbo.V_M_TM_SYS_BaseData_store c on b.RegisterStoreCode = c.StoreCode where 1=1 ";

        //            if (!string.IsNullOrEmpty(memCard))
        //            {
        //                Condition += " and b.MemberCardNo like '%" + memCard + "%'";
        //            }
        //            if (!string.IsNullOrEmpty(memName))
        //            {
        //                Condition += " and b.CustomerName like '%" + memName + "%'";
        //            }
        //            if (!string.IsNullOrEmpty(mobile))
        //            {
        //                Condition += " and b.CustomerMobile like '%" + mobile + "%'";
        //            }
        //            if (!string.IsNullOrEmpty(registerStoreCode))
        //            {
        //                Condition += " and b.RegisterStoreCode ='" + registerStoreCode + "'";
        //            }
        //            strSQL += Condition;
        //            lst = db.Database.SqlQuery<MemSubdivisionResultModel>(strSQL).ToList();
        //        }
        //        if (lst == null || lst.Count == 0)
        //            return new Result(false, "", null);
        //        return new Result(true, "", lst);
        //    }
        //}
        #endregion

        /// <summary>
        /// 手动导入会员细分
        /// </summary>
        /// <param name="curSubdId"></param>
        /// <param name="lstMemSub"></param>
        /// <returns></returns>
        public static Result ImportMemberSubdivision(Guid curSubdId, string importObj)
        {
            try
            {
                List<string> lstImport = JsonHelper.Deserialize<List<string>>(importObj);
                using (CRMEntities db = new CRMEntities())
                {
                    db.BeginTransaction();
                    if (lstImport == null || lstImport.Count == 0)
                        return new Result(false, "不存在需要导入的数据！");
                    var qMember = db.V_S_TM_Mem_Ext.Where(p => lstImport.Contains(p.MemberID)).ToList();
                    if (qMember == null || qMember.Count == 0)
                        return new Result(false, "不存在会员数据！");
                    var sb = new StringBuilder();
                    Guid instanceId = Guid.NewGuid();
                    string instanceIdStr = instanceId.ToString().Replace("-", "");
                    string tableName = "TM_Mem_SubdivideResult_" + instanceIdStr;
                    if (db.Database.SqlQuery<int>("select count(1) from TM_Mem_SubdivisionInstance where SubdivisionID = '" + curSubdId + "'").FirstOrDefault() == 0)
                    //if (db.Database.SqlQuery<int>("select count(1) from sysobjects where name = '" + tableName + "' and type = 'U'").FirstOrDefault() == 0)
                    {
                        sb.AppendLine(string.Format("CREATE TABLE [{0}](", tableName));
                        sb.AppendLine("[MSRID] [bigint] IDENTITY(1,1) NOT NULL,");
                        sb.AppendLine("[MemberID] [char](32) NOT NULL,");
                        sb.AppendLine("[ParentMemberID] [char](32) NULL,");
                        sb.AppendLine("[DataGroupID] [int] NOT NULL,");
                        sb.AppendLine("[MemberGrade] [smallint] NOT NULL,");
                        sb.AppendLine(string.Format("CONSTRAINT [PK_TM_Mem_SubdivideResult_{0}] PRIMARY KEY CLUSTERED ", instanceIdStr));
                        sb.AppendLine("(");
                        sb.AppendLine("[MSRID] ASC");
                        sb.AppendLine(")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                        sb.AppendLine(") ON [PRIMARY]");

                        sb.AppendFormat(" CREATE TABLE #TempSubdResult([MemberID] [char](32) NOT NULL)");

                        if (lstImport != null && lstImport.Count > 0)
                        {
                            foreach (string s in lstImport)
                            {
                                sb.AppendFormat(" Insert into #TempSubdResult([MemberID])");
                                sb.AppendFormat(" Select '{0}' ", s);
                            }

                            sb.AppendFormat(" Insert into {0}([MemberID],[ParentMemberID],[DataGroupID],[MemberGrade])", tableName);
                            sb.AppendFormat(" Select B.MemberID,B.ParentMemberID,B.DataGroupID,B.MemberGrade From #TempSubdResult A join [dbo].[V_S_TM_Mem_Master] B on A.MemberID=B.MemberID ");

                            sb.AppendFormat(" Insert into TM_Mem_SubdivisionInstance([SubdivisionInstanceID],[SubdivisionID],[TableName],[ComputerTime],[LastComputerDate],[AddedDate])");
                            sb.AppendFormat(" Select '{2}','{0}','{1}',0,getdate(),getdate() ", curSubdId.ToString(), tableName, instanceId.ToString());
                            sb.AppendFormat(" Update TM_Mem_Subdivision Set CurSubdivisionInstanceID='{1}' Where SubdivisionID='{0}'", curSubdId.ToString(), instanceId.ToString());

                            db.Database.ExecuteSqlCommand(sb.ToString());
                        }

                        db.Commit();
                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        dict.Add("Count", qMember.Count.ToString());
                        dict.Add("InstanceId", instanceId.ToString());
                        //dynamic dy = new { Count = qMember.Count, instanceId = instanceId };
                        return new Result(true, "", JsonHelper.Serialize(dict));
                    }
                    else
                    {
                        return new Result(false, "已存在细分结果！");
                    }
                }
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        #endregion

        #region 统计分析图表数据源
        /// <summary>
        /// 获取会员细分统计分析图表数据源
        /// </summary>
        /// <param name="subdId"></param>
        /// <returns></returns>
        public static Result GetSubdStatiscicalResult(Guid subdId)
        {
            using (var db = new CRMEntities())
            {
                var query = (from q in db.TM_Mem_SubdivisionInstance
                             where q.SubdivisionID == subdId
                             group q by new { cDate = q.LastComputerDate, tableName = q.TableName } into g
                             select new
                             {
                                 ComputeDate = g.Key.cDate,
                                 TableName = g.Key.tableName,
                                 ComputeCount = g.Count()
                             }).ToList();

                IDictionary<string, int> tableNames = new Dictionary<string, int>();
                int cnt = 0;
                int rows = 24;

                if (query.Count() > rows)
                {
                    query = (from q in
                                 (from q in query
                                  orderby q.ComputeDate descending
                                  select q).Take(rows)
                             orderby q.ComputeDate ascending
                             select q).ToList();
                }

                foreach (var item in query)
                {
                    var q = db.Database.SqlQuery<int>("select count(-1) from [" + item.TableName + "]", DBNull.Value).ToList();

                    if (item.ComputeCount > 1)//table数量大于1
                    {
                        if (q != null && q.Count > 0)
                        {
                            cnt += q[0];
                            tableNames.Add(item.TableName, cnt);
                        }
                    }
                    else
                    {
                        if (q != null && q.Count > 0)
                        {
                            tableNames.Add(item.TableName, q[0]);
                        }
                        cnt = 0;
                        rows++;
                    }
                }
                var qResult = from q in query
                              join t in tableNames on q.TableName equals t.Key
                              select new
                              {
                                  ComputeDate = q.ComputeDate,
                                  ComputeCount = t.Value
                              };

                if (qResult != null && qResult.Count() > 0)
                    return new Result(true, "", qResult);
                else
                    return new Result(false, "", null);
            }
        }
        #endregion
    }
}
