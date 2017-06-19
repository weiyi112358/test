using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Arvato.CRM.BizLogic
{
    public static class BaseData
    {
        #region DataGroup数据群组

        /// <summary>
        /// 获取数据群组信息（分页）
        /// </summary>
        /// <param name="groupGrade"></param>
        /// <param name="groupName"></param>
        /// <param name="addDate"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetDataGroup(int? groupGrade, string groupName, DateTime? addDate, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from u in db.TM_SYS_DataGroup
                            join b in db.TM_SYS_DataGroup on u.ParentDataGroupID equals b.DataGroupID into bb
                            from dept in bb.DefaultIfEmpty()
                            select new
                            {
                                u.DataGroupID,
                                u.DataGroupGrade,
                                u.DataGroupName,
                                u.AddedDate,
                                u.ParentDataGroupID,
                                FDataGroupName = dept.DataGroupName
                            };
                if (groupGrade != null) query = query.Where(p => p.DataGroupGrade == groupGrade);
                if (!string.IsNullOrEmpty(groupName)) query = query.Where(p => p.DataGroupName.Contains(groupName));
                if (addDate != null) query = query.Where(p => SqlFunctions.DateDiff("day", p.AddedDate, addDate) == 0);
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }

        /// <summary>
        /// 根据主键ID获取单条数据群组信息
        /// </summary>
        /// <param name="dgId"></param>
        /// <returns></returns>
        public static Result GetDataGroupById(int dgId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_SYS_DataGroup.Where(p => p.DataGroupID == dgId).FirstOrDefault();
                return new Result(true, "", query);
            }
        }

        /// <summary>
        /// 根据主键ID删除单条数据群组信息
        /// </summary>
        /// <param name="dgId"></param>
        /// <returns></returns>
        public static Result DeleteDataGroupById(int dgId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_SYS_DataGroup.Where(p => p.DataGroupID == dgId).FirstOrDefault();
                if (query != null)
                {
                    var q = db.TM_SYS_DataGroup.Where(p => p.ParentDataGroupID == dgId).ToList();
                    if (q.Count <= 0)
                    {
                        db.TM_SYS_DataGroup.Remove(query);
                        db.SaveChanges();
                        return new Result(true, "删除成功");
                    }
                    return new Result(false, "有子节点不允许删除，请先删除子节点");
                }
                return new Result(false, "删除失败");
            }
        }

        /// <summary>
        /// 获取数据群组信息（树图）
        /// </summary>
        /// <returns></returns>
        public static Result GetDataGroupList()
        {
            using (CRMEntities db = new CRMEntities())
            {
                List<TM_SYS_DataGroup> query = db.TM_SYS_DataGroup.ToList();//获取数据列表
                //把数据列表转化为所要的数据源
                List<TreeDataSource> nodes = new List<TreeDataSource>();
                TreeDataSource node = null;
                foreach (var item in query)
                {
                    node = new TreeDataSource();
                    node.nodeId = item.DataGroupID.ToString();
                    node.nodePId = item.ParentDataGroupID.ToString();
                    node.nodeGrade = item.DataGroupGrade;
                    node.nodeName = item.DataGroupName;
                    nodes.Add(node);
                }
                var levelOne = nodes.Where(o => o.nodeGrade == 0).ToList();
                //var levelOne = query.Where(o => o.DataGroupGrade == 0).ToList();
                var result = new List<TreeNode>();
                foreach (var cate in levelOne)
                {
                    //var node = Arvato.CRM.BizLogic.Service.CreateTreeNode(query, cate.DataGroupID);
                    var res = Arvato.CRM.BizLogic.Service.CreateTreeNodeData(nodes, cate.nodeId.ToString());
                    if (node != null)
                    {
                        result.Add(res);
                    }
                }
                return new Result(true, "", result);
            }
        }

        /// <summary>
        /// 增加或者更新群组信息
        /// </summary>
        /// <param name="dataGroupName"></param>
        /// <param name="dataGroupGrade"></param>
        /// <param name="pDataGroupID"></param>
        /// <param name="dgId"></param>
        /// <returns></returns>
        public static Result AddOrUpdateDataGroup(string dataGroupName, int dataGroupGrade, int? pDataGroupID, int? dgId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                TM_SYS_DataGroup ent = new TM_SYS_DataGroup();
                ent.DataGroupName = dataGroupName;
                ent.DataGroupGrade = (short)(dataGroupGrade);
                ent.ParentDataGroupID = pDataGroupID;
                ent.AddedDate = DateTime.Now;

                if (dgId != null)//修改
                {
                    ent.DataGroupID = (int)dgId;
                    var query = db.TM_SYS_DataGroup.Where(p => p.DataGroupID == ent.DataGroupID).FirstOrDefault();
                    if (query != null)
                    {

                        query.DataGroupName = dataGroupName;
                        query.DataGroupGrade = (short)(dataGroupGrade);
                        query.ParentDataGroupID = pDataGroupID;
                        //query.AddedDate = DateTime.Now;
                        var entry = db.Entry(query);
                        entry.State = EntityState.Modified;
                        db.SaveChanges();
                        return new Result(true, "修改成功");
                    }
                }
                db.TM_SYS_DataGroup.Add(ent);
                db.SaveChanges();
                return new Result(true, "添加成功");
            }
        }

        /// <summary>
        /// 获取群组等级列表
        /// </summary>
        /// <returns></returns>
        public static Result GetDataGroupGrade()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.TM_SYS_DataGroup
                             where a.DataGroupGrade != 0
                             select a.DataGroupGrade).Distinct().ToList();
                return new Result(true, "", query);
            }
        }

        /// <summary>
        /// 获取所属父群组列表
        /// </summary>
        /// <param name="dataGroupGrade"></param>
        /// <returns></returns>
        public static Result GetParentDataGroup(int? dataGroupGrade)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_SYS_DataGroup.Where(p => p.DataGroupGrade == (dataGroupGrade - 1)).ToList();
                return new Result(true, "", query);
            }
        }

        #endregion

        #region 维度数据/会员属性
        /// <summary>
        /// 获取维度数据列表
        /// </summary>
        /// <param name="fieldDesc"></param>
        /// <param name="fieldAlias"></param>
        /// <param name="fieldType"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetDimensionList(string fieldDesc, string fieldAlias, string fieldType, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from u in db.TD_SYS_FieldAlias
                            join b in db.TD_SYS_BizOption on u.FieldType equals b.OptionValue into bb
                            from dept in bb.DefaultIfEmpty()
                            select new
                            {
                                u.FieldDesc,
                                u.FieldAlias,
                                u.FieldName,
                                u.FieldType,
                                u.IsFilterByLoyActionLeft,
                                u.IsFilterByLoyActionRight,
                                u.IsFilterByLoyRule,
                                u.IsFilterBySubdivision,
                                u.IsCommunicationTemplet,
                                u.ComputeScript,
                                u.RunType,
                                u.ModifiedDate,
                                u.Reg,
                                u.TableName,
                                u.DictTableType,
                                u.DictTableName,
                                u.ControlType,
                                u.AliasType,
                                u.AliasKey,
                                u.AliasID,
                                u.AddedDate,
                                AliasKey1 = dept.OptionText,//维度展示
                                OptionType = dept.OptionType
                                //AliasKey2=depc.OptionText//属性展示
                            };
                if (!string.IsNullOrEmpty(fieldDesc)) query = query.Where(p => p.FieldDesc.Contains(fieldDesc));
                if (!string.IsNullOrEmpty(fieldAlias)) query = query.Where(p => p.FieldAlias.Contains(fieldAlias));
                if (fieldType != "-1") query = query.Where(p => p.FieldType.Contains(fieldType));
                query = query.Where(p => p.ComputeScript != null && p.OptionType == "DBFieldType");
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }



        /// <summary>
        /// 增加或者修改维度数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Result AddorUpdateDimensionData(FieldAliasModel model)
        {
            using (CRMEntities db = new CRMEntities())
            {
                TD_SYS_FieldAlias ent = new TD_SYS_FieldAlias();


                if (model.AliasID != null)//修改
                {
                    ent.AliasID = (int)model.AliasID;
                    //ent.ModifiedDate = DateTime.Now;
                    var query = db.TD_SYS_FieldAlias.Where(p => p.AliasID == ent.AliasID).FirstOrDefault();
                    if (query != null)
                    {
                        query.ParameterCount = model.ParameterCount;

                        query.FieldType = model.FieldType;//x
                        query.FieldAlias = model.FieldAlias;//x

                        query.FieldDesc = model.FieldDesc;//x
                        query.DictTableName = model.DictTableName;//x
                        query.DictTableType = model.DictTableType;//x

                        query.IsFilterByLoyRule = model.IsFilterByLoyRule;//x
                        query.IsFilterBySubdivision = model.IsFilterBySubdivision;//x
                        query.ControlType = model.ControlType;//x

                        query.Reg = model.Reg;//x
                        query.ComputeScript = model.ComputeScript;//x
                        query.RunType = model.RunType;//x
                        query.IsDynamicAlias = model.IsDynamic;
                        query.ModifiedDate = DateTime.Now;
                        var entry = db.Entry(query);
                        entry.State = EntityState.Modified;
                        //删除原有的动态参数
                        var querypara = from p in db.TD_SYS_FieldAliasParameter
                                        where p.AliasID == model.AliasID
                                        select p;
                        if (querypara.Count() != 0)
                        {
                            foreach (var item in querypara)
                            {
                                db.TD_SYS_FieldAliasParameter.Remove(item);
                            }
                        }
                        if (model.IsDynamic)//如果是动态字段添加动态字段
                        {
                            List<FieldAliasParameter> paras = model.FieldPara;
                            for (int i = 0; i < paras.Count(); i++)
                            {
                                TD_SYS_FieldAliasParameter para = new TD_SYS_FieldAliasParameter();
                                para.AliasID = Convert.ToInt32(model.AliasID);
                                para.ControlType = paras[i].ControlType;
                                para.DictTableName = paras[i].DictTableName;
                                para.DictTableType = paras[i].DictTableType;
                                para.FieldType = paras[i].FieldType;
                                para.ParaIndex = short.Parse(paras[i].ParaIndex);
                                para.ParameterName = paras[i].ParameterName;
                                para.Reg = paras[i].Reg == null ? "" : paras[i].Reg;
                                para.UIIndex = paras[i].UIIndex;
                                para.IsRequired = paras[i].IsRequired;
                                db.TD_SYS_FieldAliasParameter.Add(para);
                            }

                        }
                        db.SaveChanges();
                        return new Result(true, "修改成功");
                    }
                }

                if (!model.IsDynamic)
                {
                    dynamic avaiField = db.sp_Sys_ReturnAvaiAliasColumn("TM_Loy_MemExt", model.FieldType, "MemberExt", model.AliasKey, model.AliasSubKey).FirstOrDefault();

                    if (avaiField == null) return new Result(false, "没有可分配的预留字段");
                    ent.TableName = "TM_Loy_MemExt";
                    ent.FieldName = avaiField;
                    ent.AliasType = "MemberExt";
                }
                else
                {
                    //dynamic avaiField = db.sp_Sys_ReturnAvaiAliasColumn("TE_Mem_DynamicDimension", model.FieldType, "MemberExt", model.AliasKey, model.AliasSubKey).FirstOrDefault();

                    //if (avaiField == null) return new Result(false, "没有可分配的预留字段");
                    ent.TableName = "TE_Mem_DynamicDimension";
                    ent.FieldName = model.FieldAlias;
                    ent.AliasType = "MemberExt";
                    ent.ParameterCount = model.ParameterCount;

                }

                var q = db.TD_SYS_FieldAlias.Where(p => p.FieldAlias == model.FieldAlias).FirstOrDefault();
                if (q != null)
                {
                    return new Result(false, "此字段已维护,请重新输入字段别名");
                }

                ent.FieldType = model.FieldType;//x
                ent.FieldAlias = model.FieldAlias;//x

                //null
                ent.FieldDesc = model.FieldDesc;//x
                ent.DictTableName = model.DictTableName;//x
                ent.DictTableType = model.DictTableType;//x

                //not null
                ent.IsFilterByLoyRule = model.IsFilterByLoyRule;//x
                ent.IsFilterBySubdivision = model.IsFilterBySubdivision;//x
                ent.ControlType = model.ControlType;//x

                //null
                ent.Reg = model.Reg;//x
                ent.ComputeScript = model.ComputeScript;//x
                ent.RunType = model.RunType;//x

                ent.IsFilterByLoyActionLeft = false;
                ent.IsFilterByLoyActionRight = false;
                ent.IsCommunicationTemplet = false;
                ent.IsDynamicAlias = model.IsDynamic;
                ent.AddedDate = DateTime.Now;//not null
                ent.ModifiedDate = DateTime.Now;
                dynamic master = db.TD_SYS_FieldAlias.Add(ent);
                db.SaveChanges();
                if (model.IsDynamic)//如果是动态字段则添加动态参数
                {
                    List<FieldAliasParameter> paras = model.FieldPara;
                    for (int i = 0; i < paras.Count(); i++)
                    {
                        TD_SYS_FieldAliasParameter para = new TD_SYS_FieldAliasParameter();
                        para.AliasID = master.AliasID;
                        para.ControlType = paras[i].ControlType;
                        para.DictTableName = paras[i].DictTableName;
                        para.DictTableType = paras[i].DictTableType;
                        para.FieldType = paras[i].FieldType;
                        para.ParaIndex = short.Parse(paras[i].ParaIndex);
                        para.ParameterName = paras[i].ParameterName;
                        para.Reg = paras[i].Reg == null ? "" : paras[i].Reg;
                        para.UIIndex = paras[i].UIIndex;
                        para.IsRequired = paras[i].IsRequired;
                        db.TD_SYS_FieldAliasParameter.Add(para);
                    }
                    db.SaveChanges();
                }
                return new Result(true, "添加成功");
            }
        }


        /// <summary>
        /// 根据维度ID获取动态参数
        /// </summary>
        /// <param name="AliasID"></param>
        /// <returns></returns>
        public static Result GetParaList(int AliasID)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from p in db.TD_SYS_FieldAliasParameter
                            where p.AliasID == AliasID
                            select p;
                return new Result(true, "", query.ToList());
            }
        }

        ///// <summary>
        ///// 为维度数据添加动态参数
        ///// </summary>
        ///// <param name="paras"></param>
        ///// <param name="AliasID"></param>
        ///// <returns></returns>
        //public static Result AddorUpdateParaData(List<FieldAliasParameter> paras, string AliasID)
        //{
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        if (AliasID != null)
        //        {
        //            var query = from p in db.TD_SYS_FieldAliasParameter
        //                        where p.AliasID == int.Parse(AliasID)
        //                        select p;
        //            if (query.Count() != 0)
        //            {
        //                foreach (var item in query)
        //                {
        //                    db.TD_SYS_FieldAliasParameter.Remove(item);
        //                }
        //            }
        //        }

        //        for (int i = 0; i < paras.Count(); i++)
        //        {
        //            TD_SYS_FieldAliasParameter para = new TD_SYS_FieldAliasParameter();
        //            para.AliasID = int.Parse(AliasID);
        //            para.ControlType = paras[i].ControlType;
        //            para.DictTableName = paras[i].DictTableName;
        //            para.DictTableType = paras[i].DictTableType;
        //            para.FieldType = paras[i].FieldType;
        //            para.ParaIndex = short.Parse(paras[i].ParaIndex);
        //            para.ParameterName = paras[i].ParameterName;
        //            para.Reg = paras[i].Reg;
        //            para.UIIndex = paras[i].UIIndex;
        //            db.TD_SYS_FieldAliasParameter.Add(para);
        //        }

        //    }

        //    return new Result(true, "添加成功");
        //}

        /// <summary>
        /// 获取系统属性列表
        /// </summary>
        /// <param name="fieldDesc"></param>
        /// <param name="fieldAlias"></param>
        /// <param name="fieldType"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetAttributeList(string fieldDesc, string fieldAlias, string fieldType, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from u in db.TD_SYS_FieldAlias.Where(p => p.AliasKey != "campaign")
                            join c in db.TD_SYS_BizOption on u.FieldType equals c.OptionValue into cc
                            from depc in cc.DefaultIfEmpty()
                            select new
                            {
                                u.FieldDesc,
                                u.FieldAlias,
                                u.FieldName,
                                u.FieldType,
                                u.IsFilterByLoyActionLeft,
                                u.IsFilterByLoyActionRight,
                                u.IsFilterByLoyRule,
                                u.IsFilterBySubdivision,
                                u.IsCommunicationTemplet,
                                u.ComputeScript,
                                u.RunType,
                                u.ModifiedDate,
                                u.Reg,
                                u.TableName,
                                u.DictTableType,
                                u.DictTableName,
                                u.ControlType,
                                u.AliasType,
                                u.AliasKey,
                                u.AliasSubKey,
                                u.AliasID,
                                u.AddedDate,
                                //AliasKey1 = dept.OptionText,//维度展示
                                AliasKey2 = depc.OptionText,//属性展示
                                OptionType = depc.OptionType
                            };
                if (!string.IsNullOrEmpty(fieldDesc)) query = query.Where(p => p.FieldDesc.Contains(fieldDesc));
                if (!string.IsNullOrEmpty(fieldAlias)) query = query.Where(p => p.FieldAlias.Contains(fieldAlias));
                if (fieldType != "-1") query = query.Where(p => p.FieldType.Contains(fieldType));
                query = query.Where(p => p.ComputeScript == null && p.OptionType == "DBFieldType");
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }

        public static Result GetCampaignAttributeList(string fieldDesc, string fieldAlias, string fieldType, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from u in db.TD_SYS_FieldAlias.Where(p => p.AliasKey == "campaign")
                            join c in db.TD_SYS_BizOption on u.FieldType equals c.OptionValue into cc
                            from depc in cc.DefaultIfEmpty()
                            select new
                            {
                                u.FieldDesc,
                                u.FieldAlias,
                                u.FieldName,
                                u.FieldType,
                                u.IsFilterByLoyActionLeft,
                                u.IsFilterByLoyActionRight,
                                u.IsFilterByLoyRule,
                                u.IsFilterBySubdivision,
                                u.IsCommunicationTemplet,
                                u.ComputeScript,
                                u.RunType,
                                u.ModifiedDate,
                                u.Reg,
                                u.TableName,
                                u.DictTableType,
                                u.DictTableName,
                                u.ControlType,
                                u.AliasType,
                                u.AliasKey,
                                u.AliasSubKey,
                                u.AliasID,
                                u.AddedDate,
                                //AliasKey1 = dept.OptionText,//维度展示
                                AliasKey2 = depc.OptionText,//属性展示
                                OptionType = depc.OptionType
                            };
                if (!string.IsNullOrEmpty(fieldDesc)) query = query.Where(p => p.FieldDesc.Contains(fieldDesc));
                if (!string.IsNullOrEmpty(fieldAlias)) query = query.Where(p => p.FieldAlias.Contains(fieldAlias));
                if (fieldType != "-1") query = query.Where(p => p.FieldType.Contains(fieldType));
                query = query.Where(p => p.ComputeScript == null && p.OptionType == "DBFieldType");
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }

        public static Result GetCampaginOptionDataList(string optType)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.V_M_TM_SYS_BaseData_campaign
                             where a.CampType == optType
                             select a).ToList();
                return new Result(true, "", query);
            }
        }
        /// <summary>
        /// 增加或者修改属性数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Result AddorUpdateAttributeData(FieldAliasModel model)
        {
            using (CRMEntities db = new CRMEntities())
            {
                TD_SYS_FieldAlias ent = new TD_SYS_FieldAlias();


                if (model.AliasID != null)//修改
                {
                    ent.AliasID = (int)model.AliasID;
                    var query = db.TD_SYS_FieldAlias.Where(p => p.AliasID == ent.AliasID).FirstOrDefault();
                    if (query != null)
                    {
                        //not null
                        query.TableName = model.TableName;
                        //query.FieldName = avaiField;
                        query.AliasType = model.AliasType;//basedata

                        //not null
                        query.FieldType = model.FieldType;//x
                        query.FieldAlias = model.FieldAlias;//x

                        //null
                        query.AliasKey = model.AliasKey;//x
                        query.AliasSubKey = model.AliasSubKey;//x
                        query.FieldDesc = model.FieldDesc;//x
                        query.DictTableName = model.DictTableName;//x
                        query.DictTableType = model.DictTableType;//x

                        //not null
                        query.IsFilterByLoyRule = model.IsFilterByLoyRule;//x
                        query.IsFilterBySubdivision = model.IsFilterBySubdivision;//x
                        query.IsCommunicationTemplet = model.IsCommunicationTemplet;
                        query.ControlType = model.ControlType;//x

                        //null
                        query.Reg = model.Reg;//x
                        query.RunType = model.RunType;//x
                        //query = ent;
                        query.ModifiedDate = DateTime.Now;//null
                        var entry = db.Entry(query);
                        entry.State = EntityState.Modified;
                        db.SaveChanges();
                        return new Result(true, "修改成功");
                    }
                }
                dynamic avaiField = db.sp_Sys_ReturnAvaiAliasColumn(model.TableName, model.FieldType, model.AliasType, model.AliasKey, model.AliasSubKey).FirstOrDefault();

                if (avaiField == null) return new Result(false, "没有可分配的预留字段");

                var q = db.TD_SYS_FieldAlias.Where(p => p.FieldAlias == model.FieldAlias).FirstOrDefault();
                if (q != null)
                {
                    return new Result(true, "此字段已维护,请重新输入字段别名");
                }
                //not null
                ent.TableName = model.TableName;
                ent.FieldName = avaiField;
                ent.AliasType = model.AliasType;//basedata

                //not null
                ent.FieldType = model.FieldType;//x
                ent.FieldAlias = model.FieldAlias;//x

                //null
                ent.AliasKey = model.AliasKey;//x
                ent.AliasSubKey = model.AliasSubKey;//x
                ent.FieldDesc = model.FieldDesc;//x
                ent.DictTableName = model.DictTableName;//x
                ent.DictTableType = model.DictTableType;//x

                //not null
                ent.IsFilterByLoyRule = model.IsFilterByLoyRule;//x
                ent.IsFilterBySubdivision = model.IsFilterBySubdivision;//x
                ent.IsCommunicationTemplet = model.IsCommunicationTemplet;
                ent.ControlType = model.ControlType;//x

                //null
                ent.Reg = model.Reg;//x
                ent.RunType = model.RunType;//x

                ent.IsFilterByLoyActionLeft = false;
                ent.IsFilterByLoyActionRight = false;
                ent.AddedDate = DateTime.Now;//not null
                ent.ModifiedDate = DateTime.Now;
                db.TD_SYS_FieldAlias.Add(ent);
                db.SaveChanges();
                return new Result(true, "添加成功");
            }
        }

        /// <summary>
        /// 根据主键ID删除维度数据
        /// </summary>
        /// <param name="aliasId"></param>
        /// <returns></returns>
        public static Result DeleteDimensionById(int aliasId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TD_SYS_FieldAlias.Where(p => p.AliasID == aliasId).FirstOrDefault();
                if (query != null)
                {
                    db.TD_SYS_FieldAlias.Remove(query);
                    db.SaveChanges();
                    return new Result(true, "删除成功");
                }
                return new Result(false, "删除失败");
            }
        }

        /// <summary>
        /// 根据主键ID获取维度数据
        /// </summary>
        /// <param name="aliasId"></param>
        /// <returns></returns>
        public static Result GetDimensionById(int aliasId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TD_SYS_FieldAlias.Where(p => p.AliasID == aliasId).FirstOrDefault();
                var q = from a in db.TD_SYS_FieldAlias
                        join b in db.TD_SYS_BizOption.Where(p => p.OptionType == "MemberTradeDetail") on new { ak = a.AliasKey, ask = a.AliasType } equals new { ak = b.OptionValue, ask = b.OptionType } into bb
                        from bba in bb.DefaultIfEmpty()
                        where a.AliasID == aliasId
                        select new
                        {
                            a.AliasID,
                            a.FieldAlias,
                            a.FieldDesc,
                            a.FieldType,
                            a.FieldName,
                            a.ControlType,
                            a.IsCommunicationTemplet,
                            a.DictTableName,
                            a.DictTableType,
                            a.Reg,
                            a.IsFilterBySubdivision,
                            a.IsFilterByLoyRule,
                            a.TableName,
                            a.AliasType,
                            a.AliasKey,
                            a.AliasSubKey,
                            a.RunType,
                            a.ComputeScript,
                            bba.ReferenceOptionType,
                            a.ParameterCount,
                            a.IsDynamicAlias
                        };
                return new Result(true, "", q.FirstOrDefault());
            }
        }
        #endregion

        #region 条目管理
        /// <summary>
        /// 获取条目列表（分页）
        /// </summary>
        /// <param name="itemCode"></param>
        /// <param name="itemName"></param>
        /// <param name="itemClass"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetItemData(long? itemCode, string itemName, string itemClass, bool itemEnable, int groupId, string dp)
        {
            throw new NotImplementedException();
            //DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var q = from a in db.TD_SYS_BizOption
            //            where a.OptionType == "ItemType"
            //            select a;
            //    var query = from a in db.V_M_TM_SYS_BaseData_item
            //                join b in q on a.ItemType equals b.OptionValue into bq
            //                from bqq in bq.DefaultIfEmpty()
            //                join c in db.V_Sys_DataGroupRelation on a.DataGroupID equals c.SubDataGroupID into cb
            //                from ccg in cb.DefaultIfEmpty()
            //                where a.ItemEnable == itemEnable && ccg.DataGroupID == groupId
            //                select new
            //                {
            //                    a.BaseDataID,
            //                    a.BaseDataType,
            //                    a.DataGroupID,
            //                    a.ItemName,
            //                    a.ItemDesc,
            //                    a.ItemType,
            //                    a.ItemEnable,
            //                    a.ItemAddedTime,
            //                    bqq.OptionText
            //                };
            //    if (itemCode != null) query = query.Where(p => p.BaseDataID == itemCode);
            //    if (!string.IsNullOrEmpty(itemName)) query = query.Where(p => p.ItemName.Contains(itemName));
            //    if (!string.IsNullOrEmpty(itemClass)) query = query.Where(p => p.ItemType == itemClass);

            //    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            //}
        }

        /// <summary>
        /// 新增条目
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemName"></param>
        /// <param name="itemType"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static Result AddItemData(long itemId, string itemName, string itemType, string remark, bool enable, int dataGroupID)
        {
            throw new NotImplementedException();
            //using (CRMEntities db = new CRMEntities())
            //{
            //    V_M_TM_SYS_BaseData_item ent = new V_M_TM_SYS_BaseData_item();
            //    //ent.BaseDataID = itemId;
            //    ent.DataGroupID = dataGroupID;
            //    ent.ItemName = itemName;
            //    ent.ItemType = itemType;
            //    ent.ItemDesc = remark;
            //    ent.ItemEnable = enable;
            //    ent.ItemAddedTime = DateTime.Now;
            //    var q = db.V_M_TM_SYS_BaseData_item.Where(p => p.ItemType == ent.ItemType && p.ItemName == ent.ItemName).FirstOrDefault();
            //    if (q != null) { return new Result(false, "此条目已存在"); }

            //    dynamic t = db.AddViewRow<V_M_TM_SYS_BaseData_item, TM_SYS_BaseData>(ent);

            //    db.SaveChanges();

            //    return new Result(true, "添加成功", t.BaseDataID);
            //}
        }

        /// <summary>
        /// 根据id获取单条条目信息
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static Result GetItemById(long itemId)
        {
            throw new NotImplementedException();
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var query = db.V_M_TM_SYS_BaseData_item.Where(p => p.BaseDataID == itemId).FirstOrDefault();
            //    return new Result(true, "", query);
            //}
        }
        /// <summary>
        /// 修改条目
        /// </summary>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static Result UpdateItemData(ItemModel model)
        {
            throw new NotImplementedException();
            //using (CRMEntities db = new CRMEntities())
            //{


            //    //var query = db.V_M_TM_SYS_BaseData_item.Where(p => p.BaseDataID == itemId).FirstOrDefault();
            //    //if (query != null)
            //    //{
            //    //V_M_TM_SYS_BaseData_item ttt = new V_M_TM_SYS_BaseData_item
            //    //{
            //    //    BaseDataID = (long)model.BaseDataID,
            //    //    BaseDataType = model.BaseDataType,
            //    //    DataGroupID = model.DataGroupID,
            //    //    ItemAddedTime = model.ItemAddedTime,
            //    //    ItemDesc = model.ItemDesc,
            //    //    ItemEnable = model.ItemEnable,
            //    //    ItemName = model.ItemName,
            //    //    ItemType = model.ItemType
            //    //};

            //    //dynamic t = db.UpdateViewRow<V_M_TM_SYS_BaseData_item, TM_SYS_BaseData>(ttt);
            //    //db.SaveChanges();
            //    //return new Result(true, "修改成功");
            //    // }
            //    // return new Result(true, "修改失败");
            //}
        }
        /// <summary>
        /// 删除条目
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static Result DeleteItemById(long itemId)
        {
            throw new NotImplementedException();
            //using (CRMEntities db = new CRMEntities())
            //{
            //    //todo:删除校验
            //    var q1 = db.TD_SYS_PackageDetail.Where(p => p.ItemID == itemId).FirstOrDefault();
            //    if (q1 != null)
            //    {
            //        return new Result(false, "此条目在套餐管理里被引用，不能被删除");
            //    }
            //    var query = db.V_M_TM_SYS_BaseData_item.Where(p => p.BaseDataID == itemId).FirstOrDefault();
            //    if (query != null)
            //    {
            //        dynamic t = db.DeleteViewRow<V_M_TM_SYS_BaseData_item, TM_SYS_BaseData>(query);
            //        //db.V_M_TM_SYS_BaseData_item.Remove(query);
            //        db.SaveChanges();
            //        return new Result(true, "删除成功");
            //    }
            //    return new Result(false, "删除失败");
            //}
        }
        /// <summary>
        /// 激活条目
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static Result ActiveItemById(long itemId)
        {
            throw new NotImplementedException();
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var query = db.V_M_TM_SYS_BaseData_item.Where(p => p.BaseDataID == itemId).FirstOrDefault();
            //    if (query != null)
            //    {
            //        V_M_TM_SYS_BaseData_item ttt = new V_M_TM_SYS_BaseData_item
            //        {
            //            BaseDataID = query.BaseDataID,
            //            BaseDataType = query.BaseDataType,
            //            DataGroupID = query.DataGroupID,
            //            ItemAddedTime = query.ItemAddedTime,
            //            ItemDesc = query.ItemDesc,
            //            ItemEnable = true,
            //            ItemName = query.ItemName,
            //            ItemType = query.ItemType
            //        };

            //        dynamic t = db.UpdateViewRow<V_M_TM_SYS_BaseData_item, TM_SYS_BaseData>(ttt);

            //        db.SaveChanges();
            //        return new Result(true, "启用成功");
            //    }
            //    return new Result(false, "启用失败");
            //}
        }
        /// <summary>
        /// 禁用条目
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static Result InActiveItemById(long itemId)
        {
            throw new NotImplementedException();
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var q1 = db.TD_SYS_PackageDetail.Where(p => p.ItemID == itemId).FirstOrDefault();
            //    if (q1 != null)
            //    {
            //        return new Result(false, "此条目在套餐管理里被引用，不能被禁用");
            //    }
            //    var query = db.V_M_TM_SYS_BaseData_item.Where(p => p.BaseDataID == itemId).FirstOrDefault();
            //    if (query != null)
            //    {
            //        V_M_TM_SYS_BaseData_item ttt = new V_M_TM_SYS_BaseData_item
            //        {
            //            BaseDataID = query.BaseDataID,
            //            BaseDataType = query.BaseDataType,
            //            DataGroupID = query.DataGroupID,
            //            ItemAddedTime = query.ItemAddedTime,
            //            ItemDesc = query.ItemDesc,
            //            ItemEnable = false,
            //            ItemName = query.ItemName,
            //            ItemType = query.ItemType
            //        };

            //        dynamic t = db.UpdateViewRow<V_M_TM_SYS_BaseData_item, TM_SYS_BaseData>(ttt);

            //        db.SaveChanges();
            //        return new Result(true, "禁用成功");
            //    }
            //    return new Result(false, "禁用失败");
            //}
        }
        #endregion

        #region 门店管理
        /// <summary>
        /// 获取门店数据
        /// </summary>
        /// <param name="storeCode"></param>
        /// <param name="storeName"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetStoreData(string storeCode, string storeName, int? datagroupId, int groupId, string dp)
        {
            //DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var query = from a in db.V_M_TM_SYS_BaseData_store
            //                join b in db.V_Sys_DataGroupRelation on a.DataGroupID equals b.SubDataGroupID into bb
            //                from bcg in bb.DefaultIfEmpty()
            //                join c in db.V_M_TM_SYS_BaseData_brand on a.StoreBrandCode equals c.BrandCode
            //                where bcg.DataGroupID == groupId
            //                select new
            //                {
            //                    a.BaseDataID,
            //                    a.BaseDataType,
            //                    a.DataGroupID,
            //                    bcg.SubDataGroupName,
            //                    a.StoreName,
            //                    a.StoreCode,
            //                    a.StoreAddress,
            //                    a.StoreCodeSale,
            //                    c.BrandName,
            //                };
            //    if (datagroupId != null) query = query.Where(p => p.DataGroupID == datagroupId);
            //    if (!string.IsNullOrEmpty(storeCode)) query = query.Where(p => p.StoreCode.Contains(storeCode));
            //    if (!string.IsNullOrEmpty(storeName)) query = query.Where(p => p.StoreName.Contains(storeName));

            //    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            //}

            throw new NotImplementedException();
        }
        /// <summary>
        /// 增加门店信息
        /// </summary>
        /// <param name="storeCode"></param>
        /// <param name="storeName"></param>
        /// <param name="storeClass"></param>
        /// <param name="address"></param>
        /// <param name="dataGroupID"></param>
        /// <returns></returns>
        public static Result AddStoreData(string storeCode, string storeName, int storeDataGroup, string address, string storeBrand, string storeCodeSale, string printer, int dataGroupID, string code, string storeFullName)
        {
            using (CRMEntities db = new CRMEntities())
            {
                V_M_TM_SYS_BaseData_store ent = new V_M_TM_SYS_BaseData_store();
                //ent.BaseDataID = itemId;
                ent.DataGroupID = storeDataGroup;
                ent.BaseDataType = "store";
                ent.StoreFullName = storeFullName;
                ent.StoreCode = storeCode;
                ent.StoreAddress = address;
                ent.StoreName = storeName;
                //ent.StoreBrandCode = storeBrand;
                //ent.StoreCodeSale = storeCodeSale;
                //ent.Printer = printer;
                //ent.SerialCode = code;

                var q1 = db.V_M_TM_SYS_BaseData_store.Where(p => p.StoreName == ent.StoreName).FirstOrDefault();
                if (q1 != null) { return new Result(false, "此门店信息已存在"); }
                var q2 = db.V_M_TM_SYS_BaseData_store.Where(p => p.StoreCode == ent.StoreCode).FirstOrDefault();
                if (q2 != null) { return new Result(false, "此门店代码已存在"); }

                dynamic t = db.AddViewRow<V_M_TM_SYS_BaseData_store, TM_SYS_BaseData>(ent);

                db.SaveChanges();

                return new Result(true, "添加成功", t.BaseDataID);
            }
        }

        /// <summary>
        /// 根据Id获取门店信息
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public static Result GetStoreById(long storeId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_store.Where(p => p.BaseDataID == storeId).FirstOrDefault();
                return new Result(true, "", query);
            }
        }
        /// <summary>
        /// 根据id删除门店信息
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public static Result DeleteStoreById(long storeId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_store.Where(p => p.BaseDataID == storeId).FirstOrDefault();
                if (query != null)
                {
                    dynamic t = db.DeleteViewRow<V_M_TM_SYS_BaseData_store, TM_SYS_BaseData>(query);
                    //db.V_M_TM_SYS_BaseData_store.Remove(query);
                    db.SaveChanges();
                    return new Result(true, "删除成功");
                }
                return new Result(false, "删除失败");
            }
        }
        /// <summary>
        /// 修改门店信息
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static Result UpdateStoreData(StoreModel model)
        {
            using (CRMEntities db = new CRMEntities())
            {
                //var query = db.V_M_TM_SYS_BaseData_store.Where(p => p.BaseDataID == storeId).FirstOrDefault();
                //if (query != null)
                //{
                V_M_TM_SYS_BaseData_store ttt = new V_M_TM_SYS_BaseData_store
                {
                    BaseDataID = (long)model.BaseDataID,
                    BaseDataType = model.BaseDataType,
                    DataGroupID = (int)model.DataGroupID,
                    StoreFullName = model.StoreFullName,
                    StoreName = model.StoreName,
                    StoreCode = model.StoreCode,
                    StoreAddress = model.StoreAddress,
                    //StoreBrandCode = model.StoreBrandCode,
                    //StoreCodeSale = model.StoreCodeSale,
                    //Printer = model.Printer,
                    //SerialCode = model.SerialCode,
                };
                dynamic t = db.UpdateViewRow<V_M_TM_SYS_BaseData_store, TM_SYS_BaseData>(ttt);

                db.SaveChanges();
                return new Result(true, "修改成功");
                //}
                //return new Result(false, "修改失败");
            }
        }

        /// <summary>
        /// 获取门店所属群组列表
        /// </summary>
        /// <returns></returns>
        public static Result GetStroeGroupList(int dataGroupID)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.V_Sys_DataGroupRelation
                             where a.DataGroupID == dataGroupID
                             orderby a.SubDataGroupID
                             select new
                             {
                                 a.SubDataGroupID,
                                 a.SubDataGroupName
                             }).ToList();
                return new Result(true, "", query);
            }
        }
        #endregion

        #region 车辆品牌维护
        /// <summary>
        /// 获取车辆品牌列表（分页）
        /// </summary>
        /// <param name="brandName"></param>
        /// <param name="groupId"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetBrandData(string brandName, int groupId, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.V_M_TM_SYS_BaseData_brand
                            join b in db.V_Sys_DataGroupRelation on a.DataGroupID equals b.SubDataGroupID into bb
                            from bcg in bb.DefaultIfEmpty()
                            where bcg.DataGroupID == groupId
                            select new
                            {
                                a.BaseDataID,
                                a.BaseDataType,
                                a.DataGroupID,
                                bcg.SubDataGroupName,
                                a.BrandNameBase,
                                a.BrandCodeBase,
                                a.EnableBrand
                            };
                if (!string.IsNullOrEmpty(brandName)) query = query.Where(p => p.BrandNameBase.Contains(brandName));

                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }

        /// <summary>
        /// 增加品牌信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Result AddBrandData(Brand model)
        {
            using (CRMEntities db = new CRMEntities())
            {
                V_M_TM_SYS_BaseData_brand ent = new V_M_TM_SYS_BaseData_brand();

                ent.DataGroupID = (int)model.DataGroupID;
                ent.BaseDataType = "brand";
                ent.BrandNameBase = model.BrandName;
                ent.BrandCodeBase = model.BrandCode;
                ent.EnableBrand = "1";
                var q = db.V_M_TM_SYS_BaseData_brand.Where(p => p.BrandNameBase == model.BrandName).FirstOrDefault();
                if (q != null) { return new Result(false, "此品牌信息已存在"); }
                var q1 = db.V_M_TM_SYS_BaseData_brand.Where(p => p.BrandCodeBase == model.BrandCode).FirstOrDefault();
                if (q1 != null) { return new Result(false, "此品牌代码已使用"); }
                dynamic t = db.AddViewRow<V_M_TM_SYS_BaseData_brand, TM_SYS_BaseData>(ent);

                db.SaveChanges();

                return new Result(true, "添加成功", t.BaseDataID);
            }
        }

        /// <summary>
        /// 编辑车辆品牌信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Result UpdateBrandData(Brand model)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var q = db.V_M_TM_SYS_BaseData_brand.Where(p => p.BrandNameBase == model.BrandName && p.BaseDataID != model.BaseDataID).FirstOrDefault();
                if (q != null) { return new Result(false, "此品牌信息已存在"); }
                var q1 = db.V_M_TM_SYS_BaseData_brand.Where(p => p.BrandCodeBase == model.BrandCode && p.BaseDataID != model.BaseDataID).FirstOrDefault();
                if (q1 != null) { return new Result(false, "此品牌代码已使用"); }
                V_M_TM_SYS_BaseData_brand ttt = new V_M_TM_SYS_BaseData_brand
                {
                    BaseDataID = (long)model.BaseDataID,
                    BaseDataType = model.BaseDataType,
                    DataGroupID = (int)model.DataGroupID,
                    BrandNameBase = model.BrandName,
                    BrandCodeBase = model.BrandCode,
                    EnableBrand = model.EnableBrand
                };
                dynamic t = db.UpdateViewRow<V_M_TM_SYS_BaseData_brand, TM_SYS_BaseData>(ttt);
                db.SaveChanges();
                return new Result(true, "修改成功");
            }
        }
        /// <summary>
        /// 根据ID获取品牌信息
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public static Result GetBrandById(long brandId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_brand.Where(p => p.BaseDataID == brandId).FirstOrDefault();
                return new Result(true, "", query);
            }
        }
        /// <summary>
        /// 根据ID删除品牌信息
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public static Result DeleteBrandById(long brandId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_brand.Where(p => p.BaseDataID == brandId).FirstOrDefault();

                if (query != null)
                {
                    dynamic t = db.DeleteViewRow<V_M_TM_SYS_BaseData_brand, TM_SYS_BaseData>(query);
                    db.SaveChanges();
                    return new Result(true, "删除成功");
                }
                return new Result(false, "删除失败");
            }
        }

        /// <summary>
        /// 根据ID 控制是否启用
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="currentEnable"></param>
        /// <returns></returns>
        public static Result EnableBrandById(long brandId, string currentEnable)
        {
            using (var db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_brand.FirstOrDefault(p => p.BaseDataID == brandId);
                if (query == null) return new Result(false, "操作失败");
                var ttt = new V_M_TM_SYS_BaseData_brand
                {
                    EnableBrand = (currentEnable == "1" ? "0" : "1"),
                    BaseDataID = query.BaseDataID,
                    BaseDataType = query.BaseDataType,
                    BrandCodeBase = query.BrandCodeBase,
                    BrandNameBase = query.BrandNameBase,
                    DataGroupID = query.DataGroupID
                };
                dynamic t = db.UpdateViewRow<V_M_TM_SYS_BaseData_brand, TM_SYS_BaseData>(ttt);
                db.SaveChanges();
                return new Result(true, "操作成功");
            }
        }
        #endregion

        #region 区域维护
        /// <summary>
        /// 获取区域列表（分页）
        /// </summary>
        /// <param name="areaName"></param>
        /// <param name="groupId"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetAreaData(string areaName, int groupId, string dp)
        {
            var myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (var db = new CRMEntities())
            {
                var query = from a in db.V_M_TM_SYS_BaseData_area
                            join b in db.V_Sys_DataGroupRelation on a.DataGroupID equals b.SubDataGroupID into bb
                            from bcg in bb.DefaultIfEmpty()
                            where bcg.DataGroupID == groupId
                            select new
                            {
                                a.BaseDataID,
                                a.BaseDataType,
                                a.DataGroupID,
                                bcg.SubDataGroupName,
                                a.AreaNameBase,
                                a.AreaCodeBase
                            };
                if (!string.IsNullOrEmpty(areaName)) query = query.Where(p => p.AreaNameBase.Contains(areaName));

                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }

        /// <summary>
        /// 增加区域信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Result AddAreaData(Area model)
        {
            using (var db = new CRMEntities())
            {
                var ent = new V_M_TM_SYS_BaseData_area
                {
                    DataGroupID = (int)model.DataGroupID,
                    BaseDataType = "Area",
                    AreaNameBase = model.AreaNameBase,
                    AreaCodeBase = model.AreaCodeBase
                };

                var q = db.V_M_TM_SYS_BaseData_area.FirstOrDefault(p => p.AreaNameBase == model.AreaNameBase);
                if (q != null) { return new Result(false, "此区域信息已存在"); }
                var q1 = db.V_M_TM_SYS_BaseData_area.FirstOrDefault(p => p.AreaCodeBase == model.AreaCodeBase);
                if (q1 != null) { return new Result(false, "此区域代码已使用"); }
                dynamic t = db.AddViewRow<V_M_TM_SYS_BaseData_area, TM_SYS_BaseData>(ent);

                db.SaveChanges();

                return new Result(true, "添加成功", t.BaseDataID);
            }
        }

        /// <summary>
        /// 编辑区域信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Result UpdateAreaData(Area model)
        {
            using (var db = new CRMEntities())
            {
                var q = db.V_M_TM_SYS_BaseData_area.FirstOrDefault(p => p.AreaNameBase == model.AreaNameBase && p.BaseDataID != model.BaseDataID);
                if (q != null) { return new Result(false, "此区域信息已存在"); }
                var q1 = db.V_M_TM_SYS_BaseData_area.FirstOrDefault(p => p.AreaCodeBase == model.AreaCodeBase && p.BaseDataID != model.BaseDataID);
                if (q1 != null) { return new Result(false, "此区域代码已使用"); }
                var ttt = new V_M_TM_SYS_BaseData_area
                {
                    BaseDataID = (long)model.BaseDataID,
                    BaseDataType = model.BaseDataType,
                    DataGroupID = (int)model.DataGroupID,
                    AreaNameBase = model.AreaNameBase,
                    AreaCodeBase = model.AreaCodeBase
                };
                dynamic t = db.UpdateViewRow<V_M_TM_SYS_BaseData_area, TM_SYS_BaseData>(ttt);
                db.SaveChanges();
                return new Result(true, "修改成功");
            }
        }
        /// <summary>
        /// 根据ID获取区域信息
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public static Result GetAreaById(long areaId)
        {
            using (var db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_area.FirstOrDefault(p => p.BaseDataID == areaId);
                return new Result(true, "", query);
            }
        }
        #endregion

        #region 车辆款式管理
        /// <summary>
        /// 获取车型数据（分页）
        /// </summary>
        /// <param name="vehicleBrand"></param>
        /// <param name="vehicleSeries"></param>
        /// <param name="vehicleType"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetVehicleData(string vehicleBrand, string vehicleSeries, string vehicleType, string dp)
        {

            throw new NotImplementedException();
            //DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var query = from a in db.V_M_TM_SYS_BaseData_vehicle
            //                select a;
            //    if (!string.IsNullOrEmpty(vehicleBrand)) query = query.Where(p => p.BrandCodeVehicle == vehicleBrand);
            //    if (!string.IsNullOrEmpty(vehicleSeries)) query = query.Where(p => p.SeriesCodeVehicle == vehicleSeries);
            //    if (!string.IsNullOrEmpty(vehicleType)) query = query.Where(p => p.LevelCodeVehicle == vehicleType);
            //    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            //}
        }





        #endregion

        #region 车辆基础信息维护
        /// <summary>
        /// 加载车辆品牌信息
        /// </summary>
        /// <param name="brandName"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetVehicleBrandInfo(string brandName, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var q = db.TD_Sys_VehicleBasicInfo.Where(p => p.Grade == 1);
                if (!string.IsNullOrEmpty(brandName)) q = q.Where(p => p.Name.Contains(brandName));
                return new Result(true, "", new List<object> { q.ToDataTableSourceVsPage(myDp) });
            }
        }
        /// <summary>
        /// 加载车辆车系信息
        /// </summary>
        /// <param name="vehicleBrand"></param>
        /// <param name="seriesName"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetVehicleSeriesInfo(string vehicleBrand, string seriesName, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var q = from a in db.TD_Sys_VehicleBasicInfo
                        join btemp in db.TD_Sys_VehicleBasicInfo.Where(p => p.Grade == 1) on a.ParentCode equals btemp.Code into bt
                        from b in bt.DefaultIfEmpty()
                        where a.Grade == 2
                        select new
                        {
                            a.Code,
                            a.Grade,
                            a.ID,
                            a.Name,
                            a.Sort,
                            a.Type,
                            BrandCode = b.Code,
                            BrandName = b.Name
                        };

                if (!string.IsNullOrEmpty(vehicleBrand)) q = q.Where(p => p.BrandCode == vehicleBrand);
                if (!string.IsNullOrEmpty(seriesName)) q = q.Where(p => p.Name.Contains(seriesName));

                return new Result(true, "", new List<object> { q.ToDataTableSourceVsPage(myDp) });
            }
        }
        /// <summary>
        /// 加载车型信息
        /// </summary>
        /// <param name="vehicleBrand"></param>
        /// <param name="vehicleSeries"></param>
        /// <param name="typeName"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetVehicleTypeInfo(string vehicleBrand, string vehicleSeries, string typeName, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var q = from a in db.TD_Sys_VehicleBasicInfo
                        join btemp in db.TD_Sys_VehicleBasicInfo.Where(p => p.Grade == 2) on a.ParentCode equals btemp.Code into bt
                        from b in bt.DefaultIfEmpty()
                        join ctemp in db.TD_Sys_VehicleBasicInfo.Where(p => p.Grade == 1) on b.ParentCode equals ctemp.Code into ct
                        from c in ct.DefaultIfEmpty()
                        where a.Grade == 3
                        select new
                        {
                            a.Code,
                            a.Grade,
                            a.ID,
                            a.Name,
                            a.Sort,
                            a.Type,
                            SeriesCode = b.Code,
                            SeriesName = b.Name,
                            BrandCode = c.Code,
                            BrandName = c.Name
                        };

                if (!string.IsNullOrEmpty(vehicleBrand)) q = q.Where(p => p.BrandCode == vehicleBrand);
                if (!string.IsNullOrEmpty(vehicleSeries)) q = q.Where(p => p.SeriesCode == vehicleSeries);
                if (!string.IsNullOrEmpty(typeName)) q = q.Where(p => p.Name.Contains(typeName));

                return new Result(true, "", new List<object> { q.ToDataTableSourceVsPage(myDp) });
            }
        }

        /// <summary>
        /// 获取车辆品牌列表
        /// </summary>
        /// <returns></returns>
        public static Result GetVehicleBrandList()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TD_Sys_VehicleBasicInfo.Where(p => p.Grade == 1).OrderBy(p => p.Name).ToList();
                return new Result(true, "", query);
            }
        }
        /// <summary>
        /// 获取车系列表
        /// </summary>
        /// <param name="dataGroupId"></param>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public static Result GetVehicleSeriesList(string brandId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TD_Sys_VehicleBasicInfo.Where(p => p.Grade == 2 && p.ParentCode == brandId).ToList();
                return new Result(true, "", query);
            }
        }
        /// <summary>
        /// 获取车型列表
        /// </summary>
        /// <param name="dataGroupId"></param>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public static Result GetVehicleLevelList(string seriesId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TD_Sys_VehicleBasicInfo.Where(p => p.Grade == 3 && p.ParentCode == seriesId).ToList();
                return new Result(true, "", query);
            }
        }

        public static Result GetVehicleSiblingList(string code)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.TD_Sys_VehicleBasicInfo
                            join b in db.TD_Sys_VehicleBasicInfo.Where(p => p.Code == code) on a.ParentCode equals b.ParentCode
                            select a;
                return new Result(true, "", query.ToList());
            }
        }

        /// <summary>
        /// 根据id删除车型信息
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public static Result DeleteVehicleById(int vehicleId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TD_Sys_VehicleBasicInfo.Where(p => p.ID == vehicleId).FirstOrDefault();
                if (query != null)
                {
                    var q = db.TD_Sys_VehicleBasicInfo.Where(p => p.ParentCode == query.Code).ToList();
                    if (q.Count <= 0)
                    {
                        db.TD_Sys_VehicleBasicInfo.Remove(query);
                        db.SaveChanges();
                        return new Result(true, "删除成功");
                    }
                    else
                    {
                        return new Result(true, "有关联信息，不允许删除");
                    }
                }
                return new Result(false, "删除失败");
            }
        }
        /// <summary>
        /// 新增信息
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public static Result AddVehicleData(Vehicle vehicle)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var q = db.TD_Sys_VehicleBasicInfo.Where(p => (p.Name == vehicle.Name || p.Code == vehicle.Code)).FirstOrDefault();
                if (q != null) return new Result(false, "数据库中存在相同的名称或者代码");
                TD_Sys_VehicleBasicInfo ent = new TD_Sys_VehicleBasicInfo();
                ent.Name = vehicle.Name;
                ent.Code = vehicle.Code;
                ent.Grade = vehicle.Grade;
                ent.ParentCode = vehicle.ParentCode;
                ent.Type = "";
                ent.Sort = 1;

                db.TD_Sys_VehicleBasicInfo.Add(ent);
                db.SaveChanges();
                return new Result(true, "添加成功");
            }
        }
        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public static Result UpdateVehicleData(Vehicle vehicle)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TD_Sys_VehicleBasicInfo.Where(p => p.ID == vehicle.ID).FirstOrDefault();
                if (query != null)
                {
                    var q = db.TD_Sys_VehicleBasicInfo.Where(p => p.ID != vehicle.ID && (p.Name == vehicle.Name || p.Code == vehicle.Code)).FirstOrDefault();
                    if (q != null) return new Result(false, "数据库中存在相同的名称或者代码");
                    query.Name = vehicle.Name;
                    query.Code = vehicle.Code;
                    query.ParentCode = vehicle.ParentCode;
                    query.Grade = vehicle.Grade;
                    var entry = db.Entry(query);
                    entry.State = EntityState.Modified;
                    db.SaveChanges();
                    return new Result(true, "修改成功");
                }
                return new Result(false, "修改失败");
            }
        }
        /// <summary>
        /// 根据id获取车型信息
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public static Result GetVehicleById(int vehicleId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TD_Sys_VehicleBasicInfo.Where(p => p.ID == vehicleId).FirstOrDefault();
                return new Result(true, "", query);
            }
        }
        #endregion

        #region 企业信息管理
        /// <summary>
        /// 获取企业信息（分页）
        /// </summary>
        /// <param name="corpName"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetCorpData(string corpName, string dp)
        {
            throw new NotImplementedException();
            //DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var query = from a in db.V_M_TM_SYS_BaseData_corp
            //                select a;
            //    if (!string.IsNullOrEmpty(corpName)) query = query.Where(p => p.CorpName.Contains(corpName));
            //    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            //}
        }

        /// <summary>
        /// 增加企业信息
        /// </summary>
        /// <param name="corpName"></param>
        /// <param name="corpContract"></param>
        /// <param name="corpPhone"></param>
        /// <param name="corpAddress"></param>
        /// <param name="dataGroupID"></param>
        /// <returns></returns>
        public static Result AddCorpData(CorpModel model)
        {
            throw new NotImplementedException();
            //using (CRMEntities db = new CRMEntities())
            //{
            //    V_M_TM_SYS_BaseData_corp ent = new V_M_TM_SYS_BaseData_corp();

            //    ent.DataGroupID = (int)model.DataGroupID;
            //    ent.BaseDataType = "corp";
            //    ent.CorpName = model.CorpName;
            //    ent.CorpContract = model.CorpContract;
            //    ent.CorpPhoneNo = model.CorpPhoneNo;
            //    ent.CorpAddress = model.CorpAddress;
            //    var q = db.V_M_TM_SYS_BaseData_corp.Where(p => p.CorpName == model.CorpName).FirstOrDefault();
            //    if (q != null) { return new Result(false, "此企业信息已存在"); }

            //    dynamic t = db.AddViewRow<V_M_TM_SYS_BaseData_corp, TM_SYS_BaseData>(ent);

            //    db.SaveChanges();

            //    return new Result(true, "添加成功", t.BaseDataID);
            //}
        }
        /// <summary>
        /// 编辑企业信息
        /// </summary>
        /// <param name="corpId"></param>
        /// <param name="corpName"></param>
        /// <param name="corpContract"></param>
        /// <param name="corpPhone"></param>
        /// <param name="corpAddress"></param>
        /// <returns></returns>
        public static Result UpdateCorpData(CorpModel model)
        {
            throw new NotImplementedException();
            //using (CRMEntities db = new CRMEntities())
            //{
            //    //var query = db.V_M_TM_SYS_BaseData_corp.Where(p => p.BaseDataID == corpId).FirstOrDefault();
            //    //if (query != null)
            //    //{
            //    V_M_TM_SYS_BaseData_corp ttt = new V_M_TM_SYS_BaseData_corp
            //    {
            //        BaseDataID = (long)model.BaseDataID,
            //        BaseDataType = model.BaseDataType,
            //        DataGroupID = (int)model.DataGroupID,
            //        CorpName = model.CorpName,
            //        CorpContract = model.CorpContract,
            //        CorpPhoneNo = model.CorpPhoneNo,
            //        CorpAddress = model.CorpAddress,
            //    };
            //    dynamic t = db.UpdateViewRow<V_M_TM_SYS_BaseData_corp, TM_SYS_BaseData>(ttt);
            //    db.SaveChanges();
            //    return new Result(true, "修改成功");
            //    //}
            //    //return new Result(false, "修改失败");
            //}
        }
        /// <summary>
        /// 根据ID获取企业信息
        /// </summary>
        /// <param name="corpId"></param>
        /// <returns></returns>
        public static Result GetCorpById(long corpId)
        {
            throw new NotImplementedException();
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var query = db.V_M_TM_SYS_BaseData_corp.Where(p => p.BaseDataID == corpId).FirstOrDefault();
            //    return new Result(true, "", query);
            //}
        }
        /// <summary>
        /// 根据ID删除企业信息
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public static Result DeleteCorpById(long corpId)
        {
            throw new NotImplementedException();
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var query = db.V_M_TM_SYS_BaseData_corp.Where(p => p.BaseDataID == corpId).FirstOrDefault();
            //    if (query != null)
            //    {
            //        dynamic t = db.DeleteViewRow<V_M_TM_SYS_BaseData_corp, TM_SYS_BaseData>(query);
            //        db.SaveChanges();
            //        return new Result(true, "删除成功");
            //    }
            //    return new Result(false, "删除失败");
            //}
        }
        #endregion

        #region 门店设定管理
        /// <summary>
        /// 获取门店设定信息（分页）
        /// </summary>
        /// <param name="storeCode"></param>
        /// <param name="groupId"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        //public static Result GetStoreSettingData(string storeCode, int groupId, string dp)
        //{
        //    DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        var query = from a in db.V_M_TM_SYS_BaseData_storeSetting
        //                    join c in db.V_M_TM_SYS_BaseData_store on a.StoreSettingStoreCode equals c.StoreCode into cc
        //                    from ccg in cc.DefaultIfEmpty()
        //                    join b in db.V_Sys_DataGroupRelation on a.DataGroupID equals b.SubDataGroupID into bb
        //                    from bcg in bb.DefaultIfEmpty()
        //                    where bcg.DataGroupID == groupId
        //                    select new
        //                    {
        //                        a.BaseDataID,
        //                        a.BaseDataType,
        //                        a.DataGroupID,
        //                        bcg.SubDataGroupName,
        //                        ccg.StoreName,
        //                        a.StoreSettingStoreCode,
        //                        a.PointCashPec,
        //                        a.OrderMaxPoint
        //                    };
        //        if (!string.IsNullOrEmpty(storeCode)) query = query.Where(p => p.StoreSettingStoreCode == storeCode);

        //        return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
        //    }
        //}

        ///// <summary>
        ///// 增加门店设定
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public static Result AddStoreSettingData(StoreSetting model)
        //{
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        V_M_TM_SYS_BaseData_storeSetting ent = new V_M_TM_SYS_BaseData_storeSetting();

        //        ent.DataGroupID = (int)model.DataGroupID;
        //        ent.BaseDataType = "storeSetting";
        //        ent.StoreSettingStoreCode = model.StoreSettingStoreCode;
        //        ent.OrderMaxPoint = model.OrderMaxPoint;
        //        ent.PointCashPec = model.PointCashPec;
        //        var q = db.V_M_TM_SYS_BaseData_storeSetting.Where(p => p.StoreSettingStoreCode == model.StoreSettingStoreCode).FirstOrDefault();
        //        if (q != null) { return new Result(false, "此门店设定已维护"); }

        //        dynamic t = db.AddViewRow<V_M_TM_SYS_BaseData_storeSetting, TM_SYS_BaseData>(ent);

        //        db.SaveChanges();

        //        return new Result(true, "添加成功", t.BaseDataID);
        //    }
        //}
        ///// <summary>
        ///// 编辑门店设定
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public static Result UpdateStoreSettingData(StoreSetting model)
        //{
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        var dataGroupID = db.V_M_TM_SYS_BaseData_store.Where(p => p.StoreCode == model.StoreSettingStoreCode).FirstOrDefault().DataGroupID;
        //        V_M_TM_SYS_BaseData_storeSetting ttt = new V_M_TM_SYS_BaseData_storeSetting
        //        {
        //            BaseDataID = (long)model.BaseDataID,
        //            BaseDataType = model.BaseDataType,
        //            //2015 07 20 根据当前门店编号设定dataGroupID
        //            DataGroupID = dataGroupID,
        //            StoreSettingStoreCode = model.StoreSettingStoreCode,
        //            OrderMaxPoint = model.OrderMaxPoint,
        //            PointCashPec = model.PointCashPec,
        //        };
        //        dynamic t = db.UpdateViewRow<V_M_TM_SYS_BaseData_storeSetting, TM_SYS_BaseData>(ttt);
        //        db.SaveChanges();
        //        return new Result(true, "修改成功");
        //    }
        //}
        /// <summary>
        /// 根据ID获取门店设定
        /// </summary>
        /// <param name="settingId"></param>
        /// <returns></returns>
        //public static Result GetStoreSettingById(long settingId)
        //{
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        var query = db.V_M_TM_SYS_BaseData_storeSetting.Where(p => p.BaseDataID == settingId).FirstOrDefault();
        //        return new Result(true, "", query);
        //    }
        //}
        /// <summary>
        /// 根据ID删除门店设定
        /// </summary>
        /// <param name="settingId"></param>
        /// <returns></returns>
        //public static Result DeleteStoreSettingById(long settingId)
        //{
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        var query = db.V_M_TM_SYS_BaseData_storeSetting.Where(p => p.BaseDataID == settingId).FirstOrDefault();
        //        if (query != null)
        //        {
        //            dynamic t = db.DeleteViewRow<V_M_TM_SYS_BaseData_storeSetting, TM_SYS_BaseData>(query);
        //            db.SaveChanges();
        //            return new Result(true, "删除成功");
        //        }
        //        return new Result(false, "删除失败");
        //    }
        //}
        /// <summary>
        /// 获取门店列表
        /// </summary>
        /// <param name="dataGroupId"></param>
        /// <returns></returns>
        public static Result GetStoreList(int dataGroupId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.V_M_TM_SYS_BaseData_store
                             join b in db.V_Sys_DataGroupRelation on a.DataGroupID equals b.SubDataGroupID into bb
                             from bcg in bb.DefaultIfEmpty()
                             where bcg.DataGroupID == dataGroupId
                             select new
                             {
                                 a.BaseDataID,
                                 a.DataGroupID,
                                 a.BaseDataType,
                                 a.StoreName,
                                 a.StoreCode
                             }).ToList();
                return new Result(true, "", query);
            }
        }
        public static Result GetStoreLimitList(int dataGroupId, int userId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.V_M_TM_SYS_BaseData_store
                             select new
                             {
                                 a.BaseDataID,
                                 a.DataGroupID,
                                 a.BaseDataType,
                                 a.StoreName,
                                 a.StoreCode
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
                    query = query.Where(p => l.Contains(p.StoreCode)).ToList();
                }
                else
                {
                    query = (from a in query
                             join b in db.V_Sys_DataGroupRelation on a.DataGroupID equals b.SubDataGroupID into bb
                             from bcg in bb.DefaultIfEmpty()
                             where bcg.DataGroupID == dataGroupId
                             select new
                             {
                                 a.BaseDataID,
                                 a.DataGroupID,
                                 a.BaseDataType,
                                 a.StoreName,
                                 a.StoreCode
                             }).ToList();
                }
                return new Result(true, "", query);
            }
        }
        #endregion

        #region 套餐管理
        /// <summary>
        /// 获取套餐列表（分页）
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="groupId"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetPackageList(string pName, string enable, int groupId, int userId, string dp)
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
                var query1 = query.GroupBy(t => new { t.PackageID, t.PackageName, t.Price1, t.Enable, t.StartDate, t.EndDate, t.AddedDate })
            .Select(g => new { PackageID = g.Key.PackageID, PackageName = g.Key.PackageName, NUM = g.Count(), Price1 = g.Key.Price1, Enable = g.Key.Enable, StartDate = g.Key.StartDate, EndDate = g.Key.EndDate, AddedDate = g.Key.AddedDate, limitCode = string.Join(",", g.Select(s => s.limitCode).ToArray()), limitName = string.Join(",", g.Select(s => s.limitName).ToArray()) }).ToList();

                if (!string.IsNullOrEmpty(enable)) query1 = query1.Where(p => p.Enable == (enable == "1" ? true : false)).ToList();
                if (!string.IsNullOrEmpty(pName)) query1 = query1.Where(p => p.PackageName.Contains(pName)).ToList();

                return new Result(true, "", new List<object> { query1.ToDataTableSourceVsPage(myDp) });
            }
        }

        /// <summary>
        /// 添加套餐信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Result AddPackageData(Package model, string detaillimit)
        {
            using (CRMEntities db = new CRMEntities())
            {
                //db.BeginTransaction();
                TD_SYS_Package ent = new TD_SYS_Package();

                var q = db.TD_SYS_Package.Where(p => p.PackageName == model.PackageName).FirstOrDefault();
                if (q != null)
                {
                    return new Result(false, "此套餐名字已存在,请重新输入套餐名字");
                }
                ent.DataGroupID = model.DataGroupID;
                ent.PackageName = model.PackageName;
                ent.PackageDesc = model.PackageDesc;
                ent.StartDate = model.StartDate;
                ent.EndDate = model.EndDate == null ? model.EndDate : Convert.ToDateTime(model.EndDate).AddDays(1);
                ent.AppendQty = model.AppendQty;
                ent.AppendUnit = model.AppendUnit;
                ent.Price1 = model.Price1;
                ent.Price2 = model.Price2;
                ent.Proportion = model.Proportion;
                ent.MaxSetPrice = model.MaxSetPrice;
                ent.PriceRelation = model.PriceRelation;
                ent.AddedDate = DateTime.Now;
                ent.AddedUser = model.AddedUser;
                ent.Enable = model.Enable;
                db.TD_SYS_Package.Add(ent);
                db.SaveChanges();

                var q1 = db.TD_SYS_Package.Where(p => p.PackageName == model.PackageName).FirstOrDefault();
                if (detaillimit != "null")
                {
                    List<ActLimit> limit = JsonHelper.Deserialize<List<ActLimit>>(detaillimit);

                    for (int i = 0; i < limit.Count; i++)
                    {
                        var liType = limit[i].LimitType;
                        var q2 = db.TD_SYS_PackageLimit.Where(p => p.PackageID == model.PackageID && p.LimitType == liType).ToList();
                        if (q2.Count <= 0)
                        {
                            TD_SYS_PackageLimit l = new TD_SYS_PackageLimit();
                            l.PackageID = q1.PackageID;
                            l.LimitType = liType;
                            l.LimitValue = limit[i].LimitValue;
                            l.AddedDate = DateTime.Now;
                            l.AddedUser = model.AddedUser;
                            db.TD_SYS_PackageLimit.Add(l);
                        }
                    }
                }
                db.SaveChanges();
                return new Result(true, "添加成功");
            }
        }

        /// <summary>
        /// 修改套餐信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Result UpdatePackageData(Package model, string detaillimit)
        {
            using (CRMEntities db = new CRMEntities())
            {
                db.BeginTransaction();
                var query = db.TD_SYS_Package.Where(p => p.PackageID == model.PackageID).FirstOrDefault();
                if (query != null)
                {
                    query.PackageName = model.PackageName;
                    query.PackageDesc = model.PackageDesc;
                    query.StartDate = model.StartDate;
                    query.EndDate = model.EndDate;
                    query.AppendQty = model.AppendQty;
                    query.AppendUnit = model.AppendUnit;
                    query.Price1 = model.Price1;
                    query.Price2 = model.Price2;
                    query.Proportion = model.Proportion;
                    query.MaxSetPrice = model.MaxSetPrice;
                    query.PriceRelation = model.PriceRelation;
                    query.ModifiedDate = DateTime.Now;
                    query.ModifiedUser = model.ModifiedUser;
                    query.Enable = model.Enable;
                    var entry = db.Entry(query);
                    entry.State = EntityState.Modified;
                }

                //更改帐户限制
                if (detaillimit != "null")
                {
                    var q3 = db.TD_SYS_PackageLimit.Where(p => p.PackageID == model.PackageID).ToList();
                    if (q3.Count > 0)
                    {
                        for (int i = 0; i < q3.Count; i++)
                        {
                            db.TD_SYS_PackageLimit.Remove(q3[i]);

                        }
                        //db.SaveChanges();
                    }
                    List<ActLimit> limit = JsonHelper.Deserialize<List<ActLimit>>(detaillimit);
                    for (int i = 0; i < limit.Count; i++)
                    {
                        var liType = limit[i].LimitType;
                        TD_SYS_PackageLimit l = new TD_SYS_PackageLimit();
                        l.PackageID = model.PackageID;
                        l.LimitType = liType;
                        l.LimitValue = limit[i].LimitValue;
                        l.AddedDate = DateTime.Now;
                        l.AddedUser = model.ModifiedUser;
                        db.TD_SYS_PackageLimit.Add(l);
                    }
                }
                else //删除以前的限制
                {
                    var q3 = db.TD_SYS_PackageLimit.Where(p => p.PackageID == model.PackageID).ToList();
                    if (q3.Count > 0)
                    {
                        foreach (var item in q3)
                        {
                            db.TD_SYS_PackageLimit.Remove(item);
                            //db.SaveChanges();
                        }
                    }
                }
                db.SaveChanges();
                db.Commit();
                return new Result(true, "修改成功");
            }
        }

        /// <summary>
        /// 根据id获取单条套餐信息
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public static Result GetPackageById(int? packageId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                //var query = db.TD_SYS_Package.Where(p => p.PackageID == packageId).FirstOrDefault();
                //return new Result(true, "", query);
                var query = from a in db.TD_SYS_Package
                            join b in db.TD_SYS_PackageLimit on a.PackageID equals b.PackageID into bg
                            from bgg in bg.DefaultIfEmpty()
                            where a.PackageID == packageId
                            select new
                            {
                                a.PackageID,
                                a.PackageName,
                                a.PackageDesc,
                                a.Price1,
                                a.Price2,
                                a.Proportion,
                                a.MaxSetPrice,
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
                                LimitType = bgg.LimitType ?? "",
                                LimitValue = bgg.LimitValue ?? "",
                            };
                return new Result(true, "", query.ToList());
            }
        }
        /// <summary>
        /// 根据id删除套餐信息
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public static Result DeletePackageById(int packageId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                db.BeginTransaction();
                var query = db.TD_SYS_Package.Where(p => p.PackageID == packageId).FirstOrDefault();
                if (query != null)
                {
                    if (query.Enable)
                    {
                        return new Result(false, "套餐已激活，不允许删除");
                    }
                    db.TD_SYS_Package.Remove(query);
                    //db.SaveChanges();
                    //return new Result(true, "删除成功");
                }

                //删除套餐信息的同时也要删除明细信息
                var detail = db.TD_SYS_PackageDetail.Where(p => p.PackageID == packageId).ToList();
                if (detail.Count > 0)
                {
                    foreach (var item in detail)
                    {
                        db.TD_SYS_PackageDetail.Remove(item);
                        //db.SaveChanges();
                    }
                }
                //删除套餐的限制信息
                var limit = db.TD_SYS_PackageLimit.Where(p => p.PackageID == packageId).ToList();
                if (limit.Count > 0)
                {
                    foreach (var item in limit)
                    {
                        db.TD_SYS_PackageLimit.Remove(item);
                        //db.SaveChanges();
                    }
                }
                db.SaveChanges();
                db.Commit();
                return new Result(true, "删除成功");

            }
        }

        /// <summary>
        /// 获取某套餐明细列表
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetPackageDetailList(int pId, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.TD_SYS_PackageDetail
                            where a.PackageID == pId
                            select new
                            {
                                a.PackageID,
                                a.PackageDetailID,
                                a.ItemDesc,
                                a.ItemID,
                                a.ItemName,
                                a.Qty,
                                a.AppendQty,
                                a.AppendUnit,
                                a.StartDate,
                                a.EndDate,
                            };

                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }
        public static Result GetPackageDetailList1(int pId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.TD_SYS_PackageDetail
                            where a.PackageID == pId
                            select new
                            {
                                a.PackageID,
                                a.PackageDetailID,
                                a.ItemDesc,
                                a.ItemID,
                                a.ItemName,
                                a.Qty,
                                a.AppendQty,
                                a.AppendUnit,
                                a.StartDate,
                                a.EndDate,
                            };

                return new Result(true, "", query.ToList());
            }
        }
        /// <summary>
        /// 根据id获取某条明细信息
        /// </summary>
        /// <param name="packageDetailId"></param>
        /// <returns></returns>
        public static Result GetPackageDetailById(int packageDetailId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TD_SYS_PackageDetail.Where(p => p.PackageDetailID == packageDetailId).FirstOrDefault();
                return new Result(true, "", query);
            }
        }

        /// <summary>
        /// 根据id删除套餐明细信息
        /// </summary>
        /// <param name="packageDetailId"></param>
        /// <returns></returns>
        public static Result DeletePackageDetailById(int packageDetailId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TD_SYS_PackageDetail.Where(p => p.PackageDetailID == packageDetailId).FirstOrDefault();
                if (query != null)
                {
                    db.TD_SYS_PackageDetail.Remove(query);
                    db.SaveChanges();
                    return new Result(true, "删除成功");
                }
                return new Result(false, "删除失败");
            }
        }

        /// <summary>
        /// 添加套餐明细信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Result AddPackageDetailData(PackageDetail model)
        {
            using (CRMEntities db = new CRMEntities())
            {
                TD_SYS_PackageDetail ent = new TD_SYS_PackageDetail();

                var q = db.TD_SYS_PackageDetail.Where(p => p.ItemID == model.ItemID && p.PackageID == model.PackageID).FirstOrDefault();
                if (q != null)
                {
                    return new Result(false, "此条目在此套餐明细已存在,请重新选择条目");
                }
                ent.PackageID = model.PackageID;
                ent.Proportion = model.Proportion;
                ent.MaxSetPrice = model.MaxSetPrice;
                ent.ItemID = model.ItemID;
                ent.ItemName = model.ItemName;
                ent.ItemDesc = model.ItemDesc;
                ent.Qty = model.Qty;
                ent.IDOSPackageMapping = model.IDOSPackageMapping;
                //ent.StartDate = model.StartDate;
                //ent.EndDate = model.EndDate;
                //ent.AppendQty = model.AppendQty;
                //ent.AppendUnit = model.AppendUnit;
                db.TD_SYS_PackageDetail.Add(ent);
                db.SaveChanges();
                return new Result(true, "添加成功");
            }
        }

        /// <summary>
        /// 更新套餐明细信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Result UpdatePackageDetailData(PackageDetail model)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var q = db.TD_SYS_PackageDetail.Where(p => p.ItemID == model.ItemID && p.PackageID == model.PackageID && p.PackageDetailID != model.PackageDetailID).FirstOrDefault();
                if (q != null)
                {
                    return new Result(false, "此条目在此套餐明细已存在,请重新选择条目");
                }
                var query = db.TD_SYS_PackageDetail.Where(p => p.PackageDetailID == model.PackageDetailID).FirstOrDefault();
                if (query != null)
                {
                    query.ItemID = model.ItemID;
                    query.ItemName = model.ItemName;
                    query.ItemDesc = model.ItemDesc;
                    query.IDOSPackageMapping = model.IDOSPackageMapping;
                    query.Qty = model.Qty;
                    //query.StartDate = model.StartDate;
                    //query.EndDate = model.EndDate;
                    //query.AppendQty = model.AppendQty;
                    //query.AppendUnit = model.AppendUnit;

                    var entry = db.Entry(query);
                    entry.State = EntityState.Modified;
                }

                db.SaveChanges();
                return new Result(true, "修改成功");
            }
        }

        /// <summary>
        /// 激活套餐
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public static Result ActivePackageById(int packageId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TD_SYS_Package.Where(p => p.PackageID == packageId).FirstOrDefault();
                if (query != null)
                {
                    query.Enable = true;

                    var entry = db.Entry(query);
                    entry.State = EntityState.Modified;

                    db.SaveChanges();
                    return new Result(true, "激活成功");
                }
                return new Result(false, "激活失败");
            }
        }

        /// <summary>
        /// 禁用套餐
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public static Result InActivePackageById(int packageId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TD_SYS_Package.Where(p => p.PackageID == packageId).FirstOrDefault();
                if (query != null)
                {
                    query.Enable = false;

                    var entry = db.Entry(query);
                    entry.State = EntityState.Modified;

                    db.SaveChanges();
                    return new Result(true, "禁用成功");
                }
                return new Result(false, "禁用失败");
            }
        }

        /// <summary>
        /// 获取IDOS套餐列表
        /// </summary>
        /// <returns></returns>
        public static Result GetIDOSPackageList(int groupId, int userID, string stores)
        {
            using (CRMEntities db = new CRMEntities())
            {
                string store = "";

                List<string> brand = new List<string>();
                //根据用户名拿到用户ID,然后查看用户的数据角色ID,再然后从数据限制表中找到限制品牌或者门店
                var q1 = db.TR_AUTH_UserRole.Where(a => a.UserID == userID).ToList();
                if (q1 != null)
                {
                    var roleIds = new List<int>();
                    foreach (var item in q1)
                    {
                        var id = item.RoleID;
                        var q2 = db.TM_AUTH_Role.Where(p => p.RoleID == id && p.RoleType == "data").FirstOrDefault();
                        if (q2 != null)
                            roleIds.Add(q2.RoleID);
                    }
                    if (roleIds.Count > 0)
                    {
                        List<Rang> q3 = null;
                        foreach (var item in roleIds)
                        {
                            var temp = item.ToString();
                            q3 = (from a in db.TM_AUTH_DataLimit
                                  where a.HierarchyValue == temp
                                  select new Rang
                                  {
                                      RangeType = a.RangeType,
                                      RangeValue = a.RangeValue
                                  }).Distinct().ToList();
                        }
                        if (q3.Count > 0)
                        {
                            for (int i = 0; i < q3.Count; i++)
                            {
                                if (q3[i].RangeType == "store")
                                {
                                    store += q3[i].RangeValue + ",";
                                }
                                else if (q3[i].RangeType == "brand")
                                {
                                    brand.Add(q3[i].RangeValue);
                                }
                            }
                            if (brand != null)
                            {
                                List<V_M_TM_SYS_BaseData_store> list = new List<V_M_TM_SYS_BaseData_store>();
                                foreach (var item in brand)
                                {
                                    var temp = item.ToString();
                                    //list = db.V_M_TM_SYS_BaseData_store.Where(p => p.StoreBrandCode == temp).ToList();
                                }
                                if (list.Count > 0)
                                {
                                    foreach (var item in list)
                                    {
                                        store += item.StoreCode + ",";
                                    }
                                }
                            }
                        }
                    }
                }
                string[] storeCode = store.Split(',');
                //var query = (from a in db.TD_IDOS_Package
                //             join c in db.V_M_TM_SYS_BaseData_store on a.StoreCode equals c.StoreCode
                //             join b in db.V_Sys_DataGroupRelation on a.DataGroupID equals b.SubDataGroupID into bb
                //             from bcg in bb.DefaultIfEmpty()
                //             where bcg.DataGroupID == groupId && storeCode.Contains(a.StoreCode)
                //             select new
                //             {
                //                 a.ID,
                //                 a.IDOSPackageCode,
                //                 a.PackageDesc,
                //                 a.StoreCode,
                //                 c.StoreName
                //             }).ToList();
                string[] s = JsonHelper.Deserialize<string[]>(stores);
                //if (s != null)
                //{
                //    if (s.Length > 0 && !string.IsNullOrEmpty(s[0]) && s[0] != "null") query = query.Where(p => s.Contains(p.StoreCode)).ToList();
                //}
                //return new Result(true, "", query);

                throw new NotImplementedException();
            }
        }
        private class Rang
        {
            public string RangeType { get; set; }
            public string RangeValue { get; set; }
        }
        #endregion

        #region 优惠券管理

        public static Result BatchInsertDiscrepantIndustryCouponPool1(string templet, string couponList)
        {
            try
            {
                TM_Mem_CouponPool couponTemplet = JsonHelper.Deserialize<TM_Mem_CouponPool>(templet);

                using (CRMEntities db = new CRMEntities())
                {
                    db.Configuration.AutoDetectChangesEnabled = false;
                    db.Configuration.ValidateOnSaveEnabled = false;

                    if (!string.IsNullOrEmpty(couponList))
                    {
                        DateTime time = DateTime.Now;
                        string batchNo = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString();
                        List<string> list = JsonHelper.Deserialize<List<string>>(couponList);
                        //List<TM_Mem_CouponPool> addList = new List<TM_Mem_CouponPool>();
                        int count = list.Count;
                        for (var index = 0; index < list.Count(); index++)
                        {
                            TM_Mem_CouponPool tempObj = new TM_Mem_CouponPool();
                            tempObj.CouponCode = list[index];
                            tempObj.TempletID = couponTemplet.TempletID;
                            tempObj.CouponType = couponTemplet.CouponType;
                            tempObj.BatchNo = batchNo;
                            tempObj.StartDate = couponTemplet.StartDate;
                            tempObj.EndDate = couponTemplet.EndDate;
                            tempObj.Enable = couponTemplet.Enable;
                            tempObj.IsUsed = couponTemplet.IsUsed;
                            tempObj.Counts = count;
                            tempObj.AddedDate = time;
                            tempObj.CreateDate = time;
                            db.TM_Mem_CouponPool.Add(tempObj);
                        }
                        db.SaveChanges();
                        return new Result(true, "新增成功", couponTemplet.BatchNo);
                    }
                    return new Result(false, "请导入优惠券数据");
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public static Result BatchInsertDiscrepantIndustryCouponPool(string templet, string couponList)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    TM_Mem_CouponPool couponTemplet = JsonHelper.Deserialize<TM_Mem_CouponPool>(templet);
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[]{  
                            new DataColumn("CouponCode",typeof(string)),  
                            new DataColumn("TempletID",typeof(int)),  
                            new DataColumn("CouponType",typeof(string)),  
                            new DataColumn("BatchNo",typeof(string)),  
                            new DataColumn("StartDate",typeof(string)),  
                            new DataColumn("EndDate",typeof(string)),  
                            new DataColumn("Enable",typeof(bool)),  
                            new DataColumn("IsUsed",typeof(bool)), 
                            new DataColumn("AddedDate",typeof(DateTime)),
                            new DataColumn("Counts",typeof(int)),
                            new DataColumn("CreateDate",typeof(DateTime))});
                    DateTime time = DateTime.Now;
                    string batchNo = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString();
                    List<string> list = JsonHelper.Deserialize<List<string>>(couponList);
                    List<TM_Mem_CouponPool> addList = new List<TM_Mem_CouponPool>();
                    int count = list.Count;
                    for (var index = 0; index < list.Count(); index++)
                    {
                        DataRow tempRow = dt.NewRow();

                        //TM_Mem_CouponPool tempObj = new TM_Mem_CouponPool();
                        tempRow[0] = list[index];
                        tempRow[1] = couponTemplet.TempletID;
                        tempRow[2] = couponTemplet.CouponType;
                        tempRow[3] = batchNo;
                        tempRow[4] = couponTemplet.StartDate;
                        tempRow[5] = couponTemplet.EndDate;
                        tempRow[6] = couponTemplet.Enable;
                        tempRow[7] = couponTemplet.IsUsed;
                        tempRow[8] = time;
                        tempRow[9] = count;
                        tempRow[10] = time;
                        dt.Rows.Add(tempRow);
                    }
                    using (System.Data.SqlClient.SqlBulkCopy sqlbulkcopy = new System.Data.SqlClient.SqlBulkCopy(
                        db.Database.Connection.ConnectionString,
                        System.Data.SqlClient.SqlBulkCopyOptions.UseInternalTransaction))
                    {
                        sqlbulkcopy.DestinationTableName = "TM_Mem_CouponPool";
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                        }
                        sqlbulkcopy.BulkCopyTimeout = 600;
                        sqlbulkcopy.WriteToServer(dt);
                        return new Result(true, "新增成功", couponTemplet.BatchNo);
                    }
                }
            }
            catch (System.Exception ex)
            {
                return new Result(false, ex.Message);
            }

        }


        public static Result AddPublicCouponData(string prefix, string couponLength, string couponName, string couponCounts, DateTime addDate, int TempletID)
        {
            using (CRMEntities db = new CRMEntities())
            {
                int c;
                var b = int.TryParse(couponLength, out c);
                if (!b) { return new Result(false, "随机数输入格式不正确！"); }
                if (b && c == 0) { return new Result(false, "随机数长度不能为0！"); }
                if (c > 10) return new Result(false, "长度不能大于10");
                if (Convert.ToInt32(couponCounts) > 100000) return new Result(false, "数量不能大于100000");
                if (couponCounts.Length > Convert.ToInt32(couponLength)) return new Result(false, "随机数长度必须大于数量位数");
                int length = Convert.ToInt32(couponLength);//长度
                int RmNum = Convert.ToInt32(couponCounts);
                //判断该该位数的数据加前缀是否在数据库中存在
                //var couponpool = db.TM_Mem_CouponPool.ToList();
                var distinctCoupon = db.TM_Mem_CouponPool.Where(p => p.CouponCode.StartsWith(prefix) && p.CouponCode.Length == length + prefix.Length);
                if (distinctCoupon.Count() > 0)
                    return new Result(false, "该前缀后缀加随机数长度已使用，为避免重复请更换一个前缀、后缀或随机数长度");
                Random rm = new Random();
                //int iMax = 0;
                //string strMax = "";
                //for (int i = 0; i < length; i++)
                //{
                //    strMax += "9";
                //}
                //iMax = Convert.ToInt32(strMax);
                //存放待选择数
                //List<int> lstContains = new List<int>();

                //for (int i = 0; i < 5 * RmNum; i++)//5表示随机率，1/5
                //{
                //    lstContains.Add(i + 1);
                //}
                #region 获取随机码
                List<string> couponCode = new List<string>();
                couponCode = GetCouponCode(GetCodes(RmNum));
                #endregion
                //int index_Get = 0;//随机取索引
                //int value_Get = 0;//随机取值

                var batchNo = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var query = db.TM_Act_CommunicationTemplet.Where(p => p.TempletID == TempletID).FirstOrDefault();
                var couponModel = new JsonCouponModel();
                if (query != null)
                {
                    couponModel = JsonHelper.Deserialize<JsonCouponModel>(query.BasicContent);
                }

                //先拿到此模板的限制，然后再添加到公共优惠券池中
                //var limit = db.TD_SYS_CouponLimit.Where(p => p.TempletID == TempletID).Distinct().ToList();

                StringBuilder sb = new StringBuilder();
                sb.Append("begin declare @id numeric(18,0)  ");
                db.BeginTransaction(0, 3, 0);
                for (int i = 0; i < couponCode.Count; i++)
                {
                    string strCouponCode = couponCode[i];
                    TM_Mem_CouponPool tmc = new TM_Mem_CouponPool();
                    tmc.AddedDate = addDate;
                    tmc.CreateDate = addDate;
                    tmc.CouponType = query.SubType;
                    tmc.CouponCode = prefix + strCouponCode;
                    tmc.TempletID = TempletID;

                    if (couponModel.startdate != null)
                    {
                        if (couponModel.offNumber == null)
                        {
                            tmc.StartDate = couponModel.startdate;
                        }
                        else
                        {
                            if (couponModel.unit == "day")
                                tmc.StartDate = ((DateTime)couponModel.startdate).AddDays((double)(couponModel.offNumber == null ? 0 : couponModel.offNumber));
                            else if (couponModel.unit == "month")
                                tmc.StartDate = ((DateTime)couponModel.startdate).AddMonths(couponModel.offNumber == null ? 0 : (int)couponModel.offNumber);
                            else if (couponModel.unit == "year")
                                tmc.StartDate = ((DateTime)couponModel.startdate).AddYears(couponModel.offNumber == null ? 0 : (int)couponModel.offNumber);
                        }
                        tmc.EndDate = couponModel.enddate == null ? couponModel.enddate : couponModel.enddate.Value.Date.AddDays(1).AddSeconds(-1);
                    }
                    else
                    {
                        tmc.StartDate = couponModel.startdate;
                        tmc.EndDate = couponModel.enddate == null ? couponModel.enddate : couponModel.enddate.Value.Date.AddDays(1).AddSeconds(-1);
                    }

                    tmc.Enable = true;
                    tmc.IsUsed = false;
                    tmc.BatchNo = batchNo;

                    if (tmc.StartDate != null && tmc.EndDate != null)
                    {
                        sb.AppendFormat(@"INSERT INTO [TM_Mem_CouponPool]([AddedDate],[CouponType],[CouponCode],[TempletID],[StartDate],[EndDate],[Enable],[IsUsed],[BatchNo],[Counts],[CreateDate]) 
                                            VALUES('{0}','{1}','{2}','{3}','{4}','{5}',{6},{7},'{8}','{9}','{10}') SELECT  @id= @@IDENTITY;",
                        tmc.AddedDate.ToString("yyyy-MM-dd HH:mm:ss"), tmc.CouponType, tmc.CouponCode, tmc.TempletID,
                        tmc.StartDate.HasValue ? tmc.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                        tmc.EndDate.HasValue ? tmc.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                        1, 0, tmc.BatchNo, RmNum, tmc.AddedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else if (tmc.StartDate == null && tmc.EndDate != null)
                    {
                        sb.AppendFormat(@"INSERT INTO [TM_Mem_CouponPool]([AddedDate],[CouponType],[CouponCode],[TempletID],[Enable],[IsUsed],[BatchNo],[EndDate],[Counts],[CreateDate]) 
                                            VALUES('{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}','{8}','{9}') SELECT  @id= @@IDENTITY;",
                        tmc.AddedDate.ToString("yyyy-MM-dd HH:mm:ss"), tmc.CouponType, tmc.CouponCode, tmc.TempletID, 1, 0, tmc.BatchNo,
                        tmc.EndDate.HasValue ? tmc.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : null, RmNum, tmc.AddedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else if (tmc.StartDate != null && tmc.EndDate == null)
                    {
                        sb.AppendFormat(@"INSERT INTO [TM_Mem_CouponPool]([AddedDate],[CouponType],[CouponCode],[TempletID],[Enable],[IsUsed],[BatchNo],[StartDate],[Counts],[CreateDate]) 
                                            VALUES('{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}','{8}','{9}') SELECT  @id= @@IDENTITY;",
                        tmc.AddedDate.ToString("yyyy-MM-dd HH:mm:ss"), tmc.CouponType, tmc.CouponCode, tmc.TempletID, 1, 0, tmc.BatchNo,
                        tmc.StartDate.HasValue ? tmc.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : null, RmNum, tmc.AddedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        sb.AppendFormat(@"INSERT INTO [TM_Mem_CouponPool]([AddedDate],[CouponType],[CouponCode],[TempletID],[Enable],[IsUsed],[BatchNo],[Counts],[CreateDate]) 
                                            VALUES('{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}','{8}') SELECT  @id= @@IDENTITY;",
                        tmc.AddedDate.ToString("yyyy-MM-dd HH:mm:ss"), tmc.CouponType, tmc.CouponCode, tmc.TempletID, 1, 0, tmc.BatchNo, RmNum, tmc.AddedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    }

                }
                sb.Append("end  ");
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandTimeout = 240;
                cmd.CommandText = sb.ToString();
                cmd.CommandType = System.Data.CommandType.Text;
                if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                {
                    db.Database.Connection.Open();
                }
                cmd.ExecuteNonQuery();
                db.Commit();

                db.SaveChanges();
                return new Result(true, "添加成功");
            }
        }

        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="RmNum"></param>
        /// <returns></returns>
        public static Hashtable GetCodes(int RmNum)
        {
            Hashtable hashtable = new Hashtable();
            Random rm = new Random();
            for (int i = 0; hashtable.Count < RmNum; i++)
            {
                int nValue = rm.Next(65537, 1048575);
                if (!hashtable.ContainsValue(nValue) && nValue != 0)
                {
                    hashtable.Add(nValue, nValue);
                }
            }
            return hashtable;
        }
        /// <summary>
        /// 拿到5位随机码
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<string> GetCouponCode(Hashtable table)
        {
            List<string> codeList = new List<string>();
            foreach (DictionaryEntry item in table)
            {
                string code = DecToHex(item.Value.ToString());
                codeList.Add(code);
            }
            return codeList;
        }
        /// <summary>
        /// 10进制转16进制
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string DecToHex(string x)
        {
            if (string.IsNullOrEmpty(x))
            {
                return "0";
            }
            string z = null;
            int X = Convert.ToInt32(x);
            Stack a = new Stack();
            int i = 0;
            while (X > 0)
            {
                a.Push(Convert.ToString(X % 16));
                X = X / 16;
                i++;
            }
            while (a.Count != 0)
                z += ToHex(Convert.ToString(a.Pop()));
            if (string.IsNullOrEmpty(z))
            {
                z = "0";
            }
            string Randomcode = "";
            for (int m = 0; m < z.Length; m++)
            {
                string s = z[m].ToString();
                if (s == "1")
                {
                    s = "G";
                }
                if (s == "0")
                {
                    s = "H";
                }
                Randomcode += s;
            }
            return Randomcode;
        }
        private static string ToHex(string x)
        {
            switch (x)
            {
                case "10":
                    return "A";
                case "11":
                    return "B";
                case "12":
                    return "C";
                case "13":
                    return "D";
                case "14":
                    return "E";
                case "15":
                    return "F";
                default:
                    return x;
            }
        }

        /// <summary>
        /// 获取优惠券列表
        /// </summary>
        /// <returns></returns>
        public static Result GetCouponList(int dataGroupId, int userId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                List<JsonCouponModel> lst = new List<JsonCouponModel>();
                var query = (from a in db.TM_Act_CommunicationTemplet
                             where a.Type == "Coupon" && a.DataGroupID == dataGroupId
                             select new
                             {
                                 Name = a.Name,
                                 a.DataGroupID,
                                 TempletID = a.TempletID,
                                 BasicContent = a.BasicContent
                             }).ToList();
                foreach (var item in query)
                {
                    var CouponModel = JsonHelper.Deserialize<JsonCouponModel>(item.BasicContent);
                    CouponModel.TempletID = item.TempletID;
                    if (CouponModel.ispublic == true && CouponModel.isOthers == false)
                    {
                        lst.Add(CouponModel);
                    }
                }
                var query2 = from a in query
                             join b in lst on a.TempletID equals b.TempletID
                             select new
                             {
                                 a.DataGroupID,
                                 CouponName = a.Name,
                                 CouponID = a.TempletID,
                             };

                return new Result(true, "", query2);
            }
        }

        /// <summary>
        /// 获取优惠券列表
        /// </summary>
        /// <returns></returns>
        public static Result GetDiscrepantIndustryCouponTemplet(int dataGroupID)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.TM_Act_CommunicationTemplet
                            where a.Type == "Coupon"
                               && a.DataGroupID == dataGroupID
                               && a.BasicContent.Contains(",\"isOthers\":true,")
                            select a;

                return new Result(true, "", new List<object> { query.ToList() });
            }
        }

        public static Result GetDiscrepantIndustryCoupon(string batchNo, string templetID, int dataGroupID, string pageInfo)
        {
            DatatablesParameter page = JsonHelper.Deserialize<DatatablesParameter>(pageInfo);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.TM_Mem_CouponPool
                            join b in db.TM_Act_CommunicationTemplet.Where(o => o.BasicContent.Contains(",\"isOthers\":true,"))
                            on a.TempletID equals b.TempletID
                            join o in db.TD_SYS_BizOption.Where(o => o.OptionType == "CouponType") on b.SubType equals o.OptionValue
                            group a by new { a.BatchNo, b.Name, a.CreateDate, b.TempletID, b.DataGroupID, a.StartDate, a.EndDate, o.OptionText, a.Counts } into g
                            where (string.IsNullOrEmpty(batchNo) ? true : g.Key.BatchNo.Contains(batchNo))
                                && !string.IsNullOrEmpty(g.Key.BatchNo)
                                && (string.IsNullOrEmpty(templetID) ? true : SqlFunctions.StringConvert((decimal)g.Key.TempletID).Trim().Equals(templetID.Trim()))
                                && g.Key.DataGroupID == dataGroupID
                            select new
                            {
                                CouponId = g.Key.TempletID,
                                CouponName = g.Key.Name,
                                CouponType = g.Key.OptionText,
                                BatchNo = g.Key.BatchNo,
                                AddedDate = g.Key.CreateDate,
                                StartDate = g.Key.StartDate,
                                EndDate = g.Key.EndDate,
                                CouponCounts = g.Key.Counts
                            };

                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(page) });
            }
        }

        private static int Count(int m, int n)
        {
            int result;
            if (n == 0)
            {
                return 1;
            }
            result = m * Count(m, n - 1);
            return result;
        }

        public static List<int> GetCouponList1(int dataGroupId, int userId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                List<int> couList = new List<int>();
                List<JsonCouponModel> lst = new List<JsonCouponModel>();
                var query = (from a in db.TM_Act_CommunicationTemplet
                             where a.Type == "Coupon" && a.DataGroupID == dataGroupId
                             select new
                             {
                                 Name = a.Name,
                                 a.DataGroupID,
                                 TempletID = a.TempletID,
                                 BasicContent = a.BasicContent
                             }).ToList();
                foreach (var item in query)
                {
                    var CouponModel = JsonHelper.Deserialize<JsonCouponModel>(item.BasicContent);
                    CouponModel.TempletID = item.TempletID;
                    if (CouponModel.ispublic == true)
                        lst.Add(CouponModel);
                }
                var query2 = from a in query
                             join b in lst on a.TempletID equals b.TempletID
                             //join c in db.TD_SYS_CouponLimit on a.TempletID equals c.TempletID into bb
                             //from bba in bb.DefaultIfEmpty()
                             select new
                             {
                                 a.DataGroupID,
                                 CouponName = a.Name,
                                 CouponID = a.TempletID,
                                 //LimitValue = bba == null ? "" : bba.LimitValue,
                             };
                //门店过滤
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
                    query2 = query2.Distinct().ToList();
                    //query2 = query2.Where(p => l.Contains(p.LimitValue) || p.LimitValue == "").Distinct().ToList();
                }
                else
                {
                    query2 = from a in query2
                             join b in db.V_Sys_DataGroupRelation on a.DataGroupID equals b.SubDataGroupID into bb
                             from bcg in bb.DefaultIfEmpty()
                             where bcg.DataGroupID == dataGroupId
                             select new
                             {
                                 a.DataGroupID,
                                 CouponName = a.CouponName,
                                 CouponID = a.CouponID,
                                 //a.LimitValue
                             };
                }
                var qq1 = query2.ToList();
                for (int i = 0; i < qq1.Count; i++)
                {
                    couList.Add(qq1[i].CouponID);
                }
                return couList;
            }
        }

        /// <summary>
        /// 获取公共优惠券数据
        /// </summary>
        /// <param name="storeCode"></param>
        /// <param name="storeName"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetCouponData(string dp, int dataGroupID, string batchNo, string templetID, string templetName, string status, int userId)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                int TempletID = -1;
                if (templetID != null && templetID != "")
                {
                    TempletID = Convert.ToInt32(templetID);
                }
                bool innerstatus = true;
                switch (status)
                {
                    case "0": innerstatus = false; break;
                    case "1": innerstatus = true; break;
                }

                var query = from a in db.TM_Mem_CouponPool
                            join b in db.TM_Act_CommunicationTemplet on a.TempletID equals b.TempletID
                            join o in db.TD_SYS_BizOption.Where(o => o.OptionType == "CouponType") on b.SubType equals o.OptionValue
                            group a by new { a.BatchNo, b.Name, a.CreateDate, b.TempletID, b.DataGroupID, a.StartDate, a.EndDate, o.OptionText, a.Counts, a.Enable } into g
                            where (string.IsNullOrEmpty(batchNo) ? true : g.Key.BatchNo.Contains(batchNo))
                            && !string.IsNullOrEmpty(g.Key.BatchNo)
                            && (TempletID == -1 ? true : g.Key.TempletID == TempletID)
                            && g.Key.DataGroupID == dataGroupID
                            && (status == "2" ? true : g.Key.Enable == innerstatus)
                            && (string.IsNullOrEmpty(templetName) ? true : g.Key.Name.Contains(templetName))
                            select new
                             {
                                 Enable = g.Key.Enable,
                                 CouponId = g.Key.TempletID,
                                 CouponName = g.Key.Name,
                                 CouponType = g.Key.OptionText,
                                 BatchNo = g.Key.BatchNo,
                                 AddedDate = g.Key.CreateDate,
                                 StartDate = g.Key.StartDate,
                                 EndDate = g.Key.EndDate,
                                 CouponCounts = g.Key.Counts
                             };

                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }
        /// <summary>
        /// 获取公共优惠券详细数据
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetCouponDetailData(string dp, string batchNo, int dataGroupID)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.TM_Mem_CouponPool
                            join b in db.TM_Act_CommunicationTemplet on a.TempletID equals b.TempletID
                            join c in db.TM_Mem_Master on a.MemberID equals c.MemberID
                            into temp
                            from list in temp.DefaultIfEmpty()
                            where a.BatchNo.Trim().Equals(batchNo.Trim())
                                && b.DataGroupID == dataGroupID
                            select new
                            {
                                CouponID = a.CouponID,
                                CouponCode = a.CouponCode,
                                IsUsed = a.IsUsed,
                                TempletName = b.Name,
                                Mobile = list.Str_Key_1
                            };
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }
        /// <summary>
        /// 导出公共优惠券
        /// </summary>
        /// <param name="batchNo"></param>
        /// <param name="dataGroupID"></param>
        /// <returns></returns>
        public static Result ExportPublicCoupon(string batchNo, int dataGroupID)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.TM_Mem_CouponPool
                            join b in db.TM_Act_CommunicationTemplet on a.TempletID equals b.TempletID
                            join c in db.TM_Mem_Master on a.MemberID equals c.MemberID
                            into temp
                            from list in temp.DefaultIfEmpty()
                            where a.BatchNo.Trim().Equals(batchNo.Trim()) && b.DataGroupID == dataGroupID
                            select new
                            {
                                CouponID = a.CouponID,
                                CouponCode = a.CouponCode,
                                IsUsed = a.IsUsed,
                                TempletName = b.Name,
                                Mobile = list.Str_Key_1
                            };
                return new Result(true, "", query.ToList());
            }
        }

        /// <summary>
        /// 激活规则
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public static Result ActiveCoupon(string batchNo)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var querys = db.TM_Mem_CouponPool.Where(p => p.BatchNo == batchNo);
                foreach (var query in querys)
                {
                    if (query != null)
                    {
                        query.Enable = true;
                    }
                }
                db.SaveChanges();
                return new Result(true, "激活成功");
            }
        }
        /// <summary>
        /// 禁用规则
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public static Result InActiveCoupon(string batchNo)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var querys = db.TM_Mem_CouponPool.Where(p => p.BatchNo == batchNo);
                foreach (var query in querys)
                {
                    if (query != null)
                    {
                        query.Enable = false;
                    }
                }
                db.SaveChanges();
                return new Result(true, "禁用成功");
            }
        }

        //public static Result ForbiddenCoupon(string batchNo, bool status)
        //{
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        var editStudent = db.TM_Mem_CouponPool.Where<TM_Mem_CouponPool>(s => s.BatchNo == batchNo);
        //        foreach (var i in editStudent)
        //        {
        //            i.Enable = status;                  
        //        }
        //        db.SaveChanges();
        //        return new Result (true,"");
        //    }

        //}


        #endregion

        #region 维修类型维护
        /// <summary>
        /// 获取维修类型数据（分页）
        /// </summary>
        /// <param name="storeCode"></param>
        /// <param name="groupId"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetRepairTypeData(string repairName, string storeCode, int groupId, string dp)
        {
            throw new NotImplementedException();

            //DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var query = from a in db.TM_Loy_RepairType
            //                join b in db.V_M_TM_SYS_BaseData_store on a.StoreCode equals b.StoreCode into ba
            //                from baa in ba.DefaultIfEmpty()
            //                select new
            //                {
            //                    a.StoreCode,
            //                    a.RepairTypeName,
            //                    a.RepairTypeCode,
            //                    a.IsApplyToLoyStatus,
            //                    a.IsApplyToLoyPoint,
            //                    a.IsApplyToLoyDimension,
            //                    a.ID,
            //                    baa.StoreName
            //                };
            //    if (!string.IsNullOrEmpty(repairName)) query = query.Where(p => p.RepairTypeName == repairName);
            //    if (!string.IsNullOrEmpty(storeCode)) query = query.Where(p => p.StoreCode == storeCode);

            //    return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            //}
        }

        /// <summary>
        /// 添加维修类型数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Result AddRepairTypeData(RepairModel model)
        {
            throw new NotImplementedException();

            //using (CRMEntities db = new CRMEntities())
            //{
            //    var q = db.TM_Loy_RepairType.Where(p => p.RepairTypeName == model.RepairTypeName).FirstOrDefault();
            //    if (q != null) { return new Result(false, "此维修类型已维护"); }
            //    //List<ActLimit> store = JsonHelper.Deserialize<List<ActLimit>>(model.StoreCode);
            //    if (model.StoreCode.Count > 0)
            //    {
            //        for (int i = 0; i < model.StoreCode.Count; i++)
            //        {
            //            TM_Loy_RepairType ent = new TM_Loy_RepairType();

            //            ent.StoreCode = model.StoreCode[i].LimitValue;
            //            ent.RepairTypeCode = GetStrSpellCode(model.RepairTypeName);
            //            ent.RepairTypeName = model.RepairTypeName;
            //            ent.IsApplyToLoyPoint = model.IsApplyToLoyPoint;
            //            ent.IsApplyToLoyDimension = model.IsApplyToLoyDimension;
            //            ent.IsApplyToLoyStatus = model.IsApplyToLoyStatus;

            //            db.TM_Loy_RepairType.Add(ent);
            //            db.SaveChanges();
            //        }

            //    }
            //    return new Result(true, "添加成功");
            //}
        }
        private static string GetStrSpellCode(string str)
        {
            string l = "";
            for (int i = 0; i < str.Length; i++)
            {
                l += GetCharSpellCode(str[i].ToString());
            }
            return l;
        }
        ///   <summary> 
        ///   得到一个汉字的拼音第一个字母，如果是一个英文字母则直接返回大写字母 
        ///   </summary> 
        ///   <param   name="CnChar">单个汉字</param> 
        ///   <returns>单个大写字母</returns> 
        private static string GetCharSpellCode(string CnChar)
        {
            long iCnChar;
            byte[] ZW = System.Text.Encoding.Default.GetBytes(CnChar);
            //如果是字母，则直接返回 
            if (ZW.Length == 1)
            {
                return CnChar.ToUpper();
            }
            else
            {
                //   get   the     array   of   byte   from   the   single   char    
                int i1 = (short)(ZW[0]);
                int i2 = (short)(ZW[1]);
                iCnChar = i1 * 256 + i2;
            }
            #region table   of   the   constant   list
            //expresstion 
            //table   of   the   constant   list 
            // 'A';           //45217..45252 
            // 'B';           //45253..45760 
            // 'C';           //45761..46317 
            // 'D';           //46318..46825 
            // 'E';           //46826..47009 
            // 'F';           //47010..47296 
            // 'G';           //47297..47613 
            // 'H';           //47614..48118 
            // 'J';           //48119..49061 
            // 'K';           //49062..49323 
            // 'L';           //49324..49895 
            // 'M';           //49896..50370 
            // 'N';           //50371..50613 
            // 'O';           //50614..50621 
            // 'P';           //50622..50905 
            // 'Q';           //50906..51386 
            // 'R';           //51387..51445 
            // 'S';           //51446..52217 
            // 'T';           //52218..52697 
            //没有U,V 
            // 'W';           //52698..52979 
            // 'X';           //52980..53640 
            // 'Y';           //53689..54480 
            // 'Z';           //54481..55289 
            #endregion
            //   iCnChar match     the   constant 
            if ((iCnChar >= 45217) && (iCnChar <= 45252))
            {
                return "A";
            }
            else if ((iCnChar >= 45253) && (iCnChar <= 45760))
            {
                return "B";
            }
            else if ((iCnChar >= 45761) && (iCnChar <= 46317))
            {
                return "C";
            }
            else if ((iCnChar >= 46318) && (iCnChar <= 46825))
            {
                return "D";
            }
            else if ((iCnChar >= 46826) && (iCnChar <= 47009))
            {
                return "E";
            }
            else if ((iCnChar >= 47010) && (iCnChar <= 47296))
            {
                return "F";
            }
            else if ((iCnChar >= 47297) && (iCnChar <= 47613))
            {
                return "G";
            }
            else if ((iCnChar >= 47614) && (iCnChar <= 48118))
            {
                return "H";
            }
            else if ((iCnChar >= 48119) && (iCnChar <= 49061))
            {
                return "J";
            }
            else if ((iCnChar >= 49062) && (iCnChar <= 49323))
            {
                return "K";
            }
            else if ((iCnChar >= 49324) && (iCnChar <= 49895))
            {
                return "L";
            }
            else if ((iCnChar >= 49896) && (iCnChar <= 50370))
            {
                return "M";
            }
            else if ((iCnChar >= 50371) && (iCnChar <= 50613))
            {
                return "N";
            }
            else if ((iCnChar >= 50614) && (iCnChar <= 50621))
            {
                return "O";
            }
            else if ((iCnChar >= 50622) && (iCnChar <= 50905))
            {
                return "P";
            }
            else if ((iCnChar >= 50906) && (iCnChar <= .51386))
            {
                return "Q";
            }
            else if ((iCnChar >= 51387) && (iCnChar <= 51445))
            {
                return "R";
            }
            else if ((iCnChar >= 51446) && (iCnChar <= 52217))
            {
                return "S";
            }
            else if ((iCnChar >= 52218) && (iCnChar <= 52697))
            {
                return "T";
            }
            else if ((iCnChar >= 52698) && (iCnChar <= 52979))
            {
                return "W";
            }
            else if ((iCnChar >= 52980) && (iCnChar <= 53640))
            {
                return "X";
            }
            else if ((iCnChar >= 53689) && (iCnChar <= 54480))
            {
                return "Y";
            }
            else if ((iCnChar >= 54481) && (iCnChar <= 55289))
            {
                return "Z";
            }
            else return ("?");
        }
        /// <summary>
        /// 更新维修类型数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Result UpdateRepairTypeData(RepairModel model)
        {
            throw new NotImplementedException();
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var q = db.TM_Loy_RepairType.Where(p => p.ID == model.ID).FirstOrDefault();
            //    if (q != null)
            //    {
            //        q.IsApplyToLoyPoint = model.IsApplyToLoyPoint;
            //        q.IsApplyToLoyDimension = model.IsApplyToLoyDimension;
            //        q.IsApplyToLoyStatus = model.IsApplyToLoyStatus;

            //        var entry = db.Entry(q);
            //        entry.State = EntityState.Modified;
            //        db.SaveChanges();
            //    }


            //    return new Result(true, "修改成功");
            //}
        }
        /// <summary>
        /// 根据ID获取维修类型
        /// </summary>
        /// <param name="repairTypeId"></param>
        /// <returns></returns>
        public static Result GetRepairTypeById(int repairTypeId)
        {
            throw new NotImplementedException();
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var query = db.TM_Loy_RepairType.Where(p => p.ID == repairTypeId).FirstOrDefault();
            //    return new Result(true, "", query);
            //}
        }
        /// <summary>
        /// 根据ID删除维修类型
        /// </summary>
        /// <param name="repairTypeId"></param>
        /// <returns></returns>
        public static Result DeleteRepairTypeById(int repairTypeId)
        {
            throw new NotImplementedException();
            //using (CRMEntities db = new CRMEntities())
            //{
            //    var query = db.TM_Loy_RepairType.Where(p => p.ID == repairTypeId).FirstOrDefault();
            //    if (query != null)
            //    {
            //        db.TM_Loy_RepairType.Remove(query);
            //        db.SaveChanges();
            //        return new Result(true, "删除成功");
            //    }
            //    return new Result(false, "删除失败");
            //}
        }
        #endregion

        #region 门店维护
        public static Result GetStoreMaintenanceData(string StoreName, string ChannelCodeStore, string AddressStore, int groupId, string storeCode, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.V_M_TM_SYS_BaseData_store
                            // where a.DataGroupID == groupId
                            select new
                            {
                                a.BaseDataID,
                                a.BaseDataType,
                                a.DataGroupID,
                                a.StoreName,
                                a.StoreAddress,
                                a.StoreCode,
                                a.StoreFullName,
                                a.StoreTel,
                                a.StoreType,
                                a.ProvinceStore,
                                a.ProvinceCodeStore,
                                a.AddressStore,
                                a.AreaCodeStore,
                                a.AreaNameStore,
                                a.ChannelCodeStore,
                                a.ChannelNameStore,
                                a.CityCodeStore,
                                a.CityStore,
                                a.OneLineFlag,
                                a.BrandStore
                            };

                if (!string.IsNullOrEmpty(StoreName)) query = query.Where(p => p.StoreCode.Contains(StoreName));
                if (!string.IsNullOrEmpty(ChannelCodeStore)) query = query.Where(p => p.ChannelCodeStore.Contains(ChannelCodeStore));
                if (!string.IsNullOrEmpty(AddressStore)) query = query.Where(p => p.StoreAddress.Contains(AddressStore));
                if (!string.IsNullOrWhiteSpace(groupId.ToString())) query = query.Where(p => p.DataGroupID == groupId);      
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }

        /// <summary>
        /// 新增门店
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Result AddStoreMaintenanceData(StoreModel model)
        {
            using (CRMEntities db = new CRMEntities())
            {
                V_M_TM_SYS_BaseData_store ent = new V_M_TM_SYS_BaseData_store();

                ent.DataGroupID = (int)model.DataGroupID;
                ent.BaseDataType = "store";
                ent.StoreCode = model.StoreCode;//门店编码
                ent.StoreName = model.StoreName;//门店名称
                ent.StoreAddress = model.StoreAddress;//门店地址
                ent.StoreFullName = model.StoreFullName;//门店全称
                ent.StoreType = model.StoreType;//门店类型
                ent.StoreTel = model.StoreTel;
                ent.AreaNameStore = model.AreaNameStore;
                ent.AreaCodeStore = model.AreaCodeStore;
                ent.ProvinceCodeStore = model.ProvinceCodeStore;
                ent.ProvinceStore = model.ProvinceStore;
                ent.CityCodeStore = model.CityCodeStore;
                ent.CityStore = model.CityStore;
                ent.BrandStore = model.BrandStore;
                ent.BrandCodeStore = model.StoreBrandCode;
                ent.OneLineFlag = "1";//是否营业
                var q = db.V_M_TM_SYS_BaseData_store.Where(p => p.StoreCode == model.StoreCode).FirstOrDefault();
                if (q != null) { return new Result(false, "此门店代码已使用"); }
                var q1 = db.V_M_TM_SYS_BaseData_store.Where(p => p.StoreName == model.StoreName).FirstOrDefault();
                if (q1 != null) { return new Result(false, "此品门店名称已存在"); }
                dynamic t = db.AddViewRow<V_M_TM_SYS_BaseData_store, TM_SYS_BaseData>(ent);

                db.SaveChanges();

                return new Result(true, "添加成功", null);
            }
        }

        

        /// <summary>
        /// 修改门店
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Result UpdateStoreMaintenanceData(StoreModel model)
        {
            using (CRMEntities db = new CRMEntities())
            {
                //var q = db.V_M_TM_SYS_BaseData_store.Where(p => p.StoreCode == model.StoreCode).FirstOrDefault();
                //if (q != null) { return new Result(false, "此门店代码已使用"); }
                //var q1 = db.V_M_TM_SYS_BaseData_store.Where(p => p.StoreName == model.StoreName).FirstOrDefault();
                //if (q1 != null) { return new Result(false, "此品门店名称已存在"); }

                V_M_TM_SYS_BaseData_store store = new V_M_TM_SYS_BaseData_store
                {
                    BaseDataID = (int)model.BaseDataID,
                    DataGroupID = (int)model.DataGroupID,
                    StoreCode = model.StoreCode,//门店编码
                    StoreName = model.StoreName,//门店名称
                    StoreAddress = model.StoreAddress,//门店地址
                    StoreFullName = model.StoreFullName,//门店全称
                    StoreType = model.StoreType,//门店类型
                    StoreTel = model.StoreTel,
                    AreaNameStore = model.AreaNameStore,
                    AreaCodeStore = model.AreaCodeStore,

                    ProvinceCodeStore = model.ProvinceCodeStore,
                    ProvinceStore = model.ProvinceStore,
                    CityCodeStore = model.CityCodeStore,
                    CityStore = model.CityStore,
                    BrandCodeStore = model.StoreBrandCode,
                    BrandStore = model.BrandStore,
                    OneLineFlag = "1"//是否营业
                };
                dynamic t = db.UpdateViewRow<V_M_TM_SYS_BaseData_store, TM_SYS_BaseData>(store);
                db.SaveChanges();
                return new Result(true, "修改成功");
            }
        }

        /// <summary>
        /// 删除门店
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public static Result DeleteStoreMaintenanceById(long storeId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_store.Where(p => p.BaseDataID == storeId).FirstOrDefault();

                if (query != null)
                {
                    dynamic t = db.DeleteViewRow<V_M_TM_SYS_BaseData_store, TM_SYS_BaseData>(query);
                    db.SaveChanges();
                    return new Result(true, "删除成功");
                }
                return new Result(false, "删除失败");
            }
        }

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <returns></returns>
        public static Result GetProvince()
        {
            using (var db = new CRMEntities())
            {
                var query = from a in db.TD_SYS_Region
                            where a.RegionGrade == 1
                            select new
                            {
                                OptionValue = a.RegionID,
                                OptionText = a.NameZH,
                            };
                return new Result(true, "", query.ToList());
            }
        }

        public static Result GetCity(string ParentRegionID)
        {
            using (var db = new CRMEntities())
            {
                var query = from a in db.TD_SYS_Region
                            where a.RegionGrade == 2
                            select new
                            {
                                OptionValue = a.RegionID,
                                OptionText = a.NameZH,
                                a.ParentRegionID
                            };
                query = query.Where(q => q.ParentRegionID == ParentRegionID);
                return new Result(true, "", query.ToList());
            }
        }

        /// <summary>
        /// 获取门店品牌
        /// </summary>
        /// <returns></returns>
        public static Result GetStoreBrand(int dataGroupId)
        {
            using (var db = new CRMEntities())
            {
                var query = from a in db.V_M_TM_SYS_BaseData_brand
                            where a.DataGroupID == dataGroupId
                            select new
                            {
                                OptionValue = a.BrandCodeBase,
                                OptionText = a.BrandNameBase,
                            };
                return new Result(true, "", query.ToList());
            }
        }

        /// <summary>
        /// 根据id查找门店
        /// </summary>
        /// <param name="storeid"></param>
        /// <returns></returns>
        public static Result GetStoreMaintenanceById(long storeid)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_store.Where(p => p.BaseDataID == storeid).FirstOrDefault();
                return new Result(true, "", query);
            }
        }



        public static Result StoreMaintenanceToExcel(string hideDrpStoreClass, string hideStoreName, string hideChannelCodeStore, string hideAddressStore, string hideStoreCode)
        {
            using (CRMEntities db=new CRMEntities())
            {
                var query = from a in db.V_M_TM_SYS_BaseData_store
                            // where a.DataGroupID == groupId
                            select new
                            {
                                a.BaseDataID,
                                a.BaseDataType,
                                a.DataGroupID,
                                a.StoreName,
                                a.StoreAddress,
                                a.StoreCode,
                                a.StoreFullName,
                                a.StoreTel,
                                a.StoreType,
                                a.ProvinceStore,
                                a.ProvinceCodeStore,
                                a.AddressStore,
                                a.AreaCodeStore,
                                a.AreaNameStore,
                                a.ChannelCodeStore,
                                a.ChannelNameStore,
                                a.CityCodeStore,
                                a.CityStore,
                                a.OneLineFlag,
                                a.BrandStore
                            };
          
                if (!string.IsNullOrEmpty(hideStoreName)) query = query.Where(p => p.StoreCode.Contains(hideStoreName));
                if (!string.IsNullOrEmpty(hideChannelCodeStore)) query = query.Where(p => p.ChannelCodeStore.Contains(hideChannelCodeStore));
                if (!string.IsNullOrEmpty(hideAddressStore)) query = query.Where(p => p.StoreAddress.Contains(hideAddressStore));
                if (!string.IsNullOrWhiteSpace(hideDrpStoreClass.ToString()))
                {
                    int groupID = Convert.ToInt32(hideDrpStoreClass);
                    query = query.Where(p => p.DataGroupID == groupID);
                }
                return new Result(true, "", new List<object> { query.ToList() });
            }
        
        }
        #endregion

        /// <summary>
        /// 获取品牌列表
        /// </summary>
        /// <param name="datagroupId"></param>
        /// <returns></returns>
        public static Result GetBrandList(int datagroupId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var vgrouprelationId = db.V_Sys_DataGroupRelation.Where(t => t.DataGroupID == datagroupId).Select(t => t.SubDataGroupID).ToList();
                var query = db.V_M_TM_SYS_BaseData_brand.Where(p => vgrouprelationId.Contains(p.DataGroupID)).ToList();
                return new Result(true, "", query);
            }
        }
        public static Result GetAllDataGroupList()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_SYS_DataGroup.Where(p => p.DataGroupGrade > 1).ToList();
                return new Result(true, "", query);
            }
        }
        public static Result GetStoreListByGroupID(string rids, int pageid, int datagroupid)
        {
            List<int> rid = JsonHelper.Deserialize<List<int>>(rids);
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.V_M_TM_SYS_BaseData_store.FilterDataByAuth(rid, pageid, datagroupid)
                             //where (a.AreaCodeStore == groupId || a.BrandCodeStore == groupId)
                             select new
                             {
                                 a.StoreName,
                                 a.StoreCode
                             }).ToList();
                return new Result(true, "", query);
            }
        }
        public static Result GetStoreByCode(string storecode)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.V_M_TM_SYS_BaseData_store
                             where a.StoreCode == storecode
                             select new
                             {
                                 a.AreaCodeStore,
                                 a.BrandCodeStore
                             }).FirstOrDefault();
                return new Result(true, "", query);
            }
        }
        public static Result GetKpiList(string KPIname, string KPItype, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from p in db.TM_CRM_KPI
                            select p;
                query = string.IsNullOrEmpty(KPIname) ? query : query.Where(p => p.KPIName.Contains(KPIname));
                query = string.IsNullOrEmpty(KPItype) ? query : query.Where(p => p.KPIType == KPItype);
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }

        public static Result GetCustomerLevel()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.V_M_TM_SYS_BaseData_customerlevel
                             select a).ToList();
                return new Result(true, "", query);

            }

        }

        /// <summary>
        /// 根据主键ID删除KPI数据
        /// </summary>
        /// <param name="aliasId"></param>
        /// <returns></returns>
        public static Result DeleteKPIById(int KPIId)
        {
            using (CRMEntities db = new CRMEntities())
            {

                var query = db.TM_CRM_KPI.Where(p => p.KPIID == KPIId).FirstOrDefault();
                if (query != null)
                {
                    if (query.KPIType != "1")//如果不是全局KPI，判断该KPI是否已经使用
                    {
                        var queryISUsed = db.TM_CRM_KPITarget.Where(p => p.KPIID == KPIId);
                        if (queryISUsed.Count() > 0)
                        {
                            return new Result(false, "该KPI已经使用，无法删除该KPI！");
                        }
                    }
                    db.TM_CRM_KPI.Remove(query);
                    if (query.KPIType == "1")//如果是全局KPI则删除TM_CRM_KPITarget中该KPI数据
                    {
                        var targetquery = db.TM_CRM_KPITarget.Where(p => p.KPIID == KPIId).FirstOrDefault();
                        if (targetquery != null)
                        {
                            db.TM_CRM_KPITarget.Remove(targetquery);
                        }
                    }
                    db.SaveChanges();
                    return new Result(true, "删除成功");
                }
                return new Result(false, "删除失败");
            }
        }

        /// <summary>
        /// 根据主键ID获取KPI数据
        /// </summary>
        /// <param name="aliasId"></param>
        /// <returns></returns>
        public static Result GetKPIById(int KPIId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_CRM_KPI.Where(p => p.KPIID == KPIId).FirstOrDefault();
                return new Result(true, "", query);
            }
        }

        public static Result SetKPIEnable(int KPIID, bool Enable)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_CRM_KPI.Where(p => p.KPIID == KPIID).FirstOrDefault();
                query.Enable = Enable;
                var entry = db.Entry(query);
                entry.State = EntityState.Modified;
                db.SaveChanges();
                return new Result(true, Enable ? "启用成功" : "禁用成功");
            }
        }

        public static Result AddorUpdateKPI(KPIModel model, string user)
        {
            try
            {
                TM_AUTH_User auth_user = JsonHelper.Deserialize<TM_AUTH_User>(user);

                using (CRMEntities db = new CRMEntities())
                {
                    string LoginName = db.TM_AUTH_User.Where(p => p.UserID == auth_user.UserID).FirstOrDefault().LoginName;
                    TM_CRM_KPI ent = new TM_CRM_KPI();


                    if (model.KPIID != 0)//修改
                    {
                        ent.KPIID = (int)model.KPIID;
                        var query = db.TM_CRM_KPI.Where(p => p.KPIID == ent.KPIID).FirstOrDefault();
                        if (query != null)
                        {
                            query.KPIName = model.KPIName;
                            query.KPIType = model.KPIType;


                            query.ComputeScript = model.ComputeScript;
                            query.Unit = model.Unit;


                            query.AddedDate = model.AddedDate;
                            query.AddedUser = model.AddedUser;
                            query.ModifiedDate = DateTime.Now;
                            query.ModifiedUser = LoginName;
                            query.TargetValueType = model.TargetValueType;

                            query.DataGroupID = 1;

                            var entry = db.Entry(query);
                            entry.State = EntityState.Modified;
                            db.SaveChanges();

                            return new Result(true, "修改成功");
                        }
                    }

                    ent.KPIName = model.KPIName;
                    ent.KPIType = model.KPIType;

                    ent.ComputeScript = model.ComputeScript;
                    ent.Unit = model.Unit;

                    ent.AddedUser = LoginName;
                    ent.ModifiedUser = "";
                    ent.TargetValueType = model.TargetValueType;

                    ent.DataGroupID = 1;
                    ent.AddedDate = DateTime.Now;//not null
                    ent.ModifiedDate = DateTime.Now;
                    ent.Enable = true;
                    dynamic master = db.TM_CRM_KPI.Add(ent);

                    db.SaveChanges();
                    if (ent.KPIType == "1")
                    {
                        TM_CRM_KPITarget target = new TM_CRM_KPITarget();
                        target.KPIID = master.KPIID;
                        target.KPIType = "1";
                        target.KPITypeValue = "";
                        db.TM_CRM_KPITarget.Add(target);
                        db.SaveChanges();
                    }
                    return new Result(true, "添加成功");
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        #region  基础分类信息维护相关

        public static Result GetSysClass(int? classID, string className, string classType, int dataGroupID, int? roleID, int? userID)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from c in db.TM_SYS_Class
                            join dr in db.V_Sys_DataGroupRelation.Where(d => d.DataGroupID == dataGroupID) on c.DataGroupID equals dr.SubDataGroupID
                            select c;

                if (roleID.HasValue)
                {
                    query = from q in query
                            join ur in db.TR_AUTH_UserRole.Where(o => o.RoleID == roleID.Value) on q.UserID equals ur.UserID
                            select q;
                }

                if (classID.HasValue)
                {
                    query = query.Where(c => c.ClassID == classID.Value);
                }

                if (!string.IsNullOrEmpty(className))
                {
                    query = query.Where(c => c.ClassName.Trim().Equals(className.Trim()));
                }

                if (!string.IsNullOrEmpty(classType))
                {
                    query = query.Where(c => c.ClassType.Trim().Equals(classType.Trim()));
                }

                if (userID.HasValue)
                {
                    query = query.Where(c => c.UserID == userID.Value);
                }

                return new Result(true, "", new List<object> { query.ToList() });
            }
        }

        public static Result GetSysClassForPage(int? classID, string className, string classType, int dataGroupID, int? roleID, int? userID, string page)
        {
            DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);

            using (CRMEntities db = new CRMEntities())
            {
                var query = from c in db.TM_SYS_Class
                            join dr in db.V_Sys_DataGroupRelation.Where(d => d.DataGroupID == dataGroupID) on c.DataGroupID equals dr.SubDataGroupID
                            select c;

                if (roleID.HasValue)
                {
                    query = from q in query
                            join ur in db.TR_AUTH_UserRole.Where(o => o.RoleID == roleID.Value) on q.UserID equals ur.UserID
                            select q;
                }

                if (classID.HasValue)
                {
                    query = query.Where(c => c.ClassID == classID.Value);
                }

                if (!string.IsNullOrEmpty(className))
                {
                    query = query.Where(c => c.ClassName.Trim().Equals(className.Trim()));
                }

                if (!string.IsNullOrEmpty(classType))
                {
                    query = query.Where(c => c.ClassType.Trim().Equals(classType.Trim()));
                }

                if (roleID.HasValue)
                {
                    query = query.Where(c => c.RoleID == roleID.Value);
                }

                if (userID.HasValue)
                {
                    query = query.Where(c => c.UserID == userID.Value);
                }

                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(pageInfo) });
            }
        }


        public static Result InsertSysClass(string classInfo)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    TM_SYS_Class sysClass = JsonHelper.Deserialize<TM_SYS_Class>(classInfo);

                    if (sysClass != null)
                    {
                        sysClass.AddedDate = DateTime.Now;
                        db.Entry<TM_SYS_Class>(sysClass).State = EntityState.Added;
                        db.SaveChanges();
                        return new Result(true, "", sysClass.ClassID);
                    }
                    return new Result(false, "新增失败", null);
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        public static Result UpdateSysClass(string classInfo)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    TM_SYS_Class sysClass = JsonHelper.Deserialize<TM_SYS_Class>(classInfo);

                    if (sysClass != null && sysClass.ClassID > 0)
                    {
                        sysClass.ModifiedDate = DateTime.Now;
                        db.TM_SYS_Class.Attach(sysClass);
                        db.Entry<TM_SYS_Class>(sysClass).State = EntityState.Modified;
                        db.SaveChanges();
                        return new Result(true, "", sysClass.ClassID);
                    }

                    return new Result(false, "更新失败", null);
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        //public static Result GetTypeAList()
        //{
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        //var query = db.V_M_TM_SYS_BaseData_typeA.Where(p => p.EnableTypeA == "1").ToList();
        //        var query = (from a in db.V_M_TM_SYS_BaseData_typeA
        //                     join b in db.V_M_TM_SYS_BaseData_typeB.Where(p => p.PageType == "1") on a.TypeACodeBase equals b.ParentIDTypeA
        //                     group a by new { a.TypeACodeBase, a.TypeANameBase } into ag
        //                     select new
        //                     {
        //                         TypeACodeBase = ag.Key.TypeACodeBase,
        //                         TypeANameBase = ag.Key.TypeANameBase
        //                     }).ToList();


        //        return new Result(true, "", query);
        //    }
        //}
  

        #endregion

        #region 渠道管理
        /// <summary>
        /// 获取渠道数据
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="groupId"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static Result GetChannelData(string channelName, int groupId, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.V_M_TM_SYS_BaseData_channel
                            join b in db.V_Sys_DataGroupRelation on a.DataGroupID equals b.SubDataGroupID into bb
                            from bcg in bb.DefaultIfEmpty()
                            where bcg.DataGroupID == groupId
                            select new
                            {
                                a.BaseDataID,
                                a.BaseDataType,
                                a.DataGroupID,
                                bcg.SubDataGroupName,
                                a.ChannelNameBase,
                                a.ChannelCodeBase
                                //,
                                //a.ChannelIsEnableBase
                            };

                if (!string.IsNullOrEmpty(channelName)) query = query.Where(p => p.ChannelNameBase.Contains(channelName));
                if (!string.IsNullOrWhiteSpace(groupId.ToString())) query = query.Where(p => p.DataGroupID == groupId);
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }

        /// <summary>
        /// 新增渠道
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Result AddChannelData(ChannelModel model)
        {
            using (CRMEntities db = new CRMEntities())
            {
                V_M_TM_SYS_BaseData_channel ent = new V_M_TM_SYS_BaseData_channel();
                //ent.ChannelIsEnableBase = "1";
                ent.DataGroupID = (int)model.DataGroupID;
                ent.BaseDataType = "store";
                ent.ChannelNameBase = model.ChannelName;//渠道名称
                var querys = from a in db.V_M_TM_SYS_BaseData_channel
                             where a.DataGroupID == ent.DataGroupID
                             select new
                             {
                                 a.BaseDataID,
                                 a.ChannelCodeBase,
                                 a.ChannelNameBase
                             };

                var query = querys.Where(p => p.ChannelNameBase == model.ChannelName).FirstOrDefault();
                if (query != null) { return new Result(false, "此品渠道名称已存在"); }
                int maxcode = querys.Select(p => p.ChannelCodeBase).ToList().ConvertAll(p => int.Parse(p)).Max();
                ent.ChannelCodeBase = (maxcode + 1).ToString();//渠道编码
                db.AddViewRow<V_M_TM_SYS_BaseData_channel, TM_SYS_BaseData>(ent);
                db.SaveChanges();
                return new Result(true, "添加成功", null);
            }
        }

        /// <summary>
        /// 修改渠道
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Result UpdateChannelData(ChannelModel model)
        {
            using (CRMEntities db = new CRMEntities())
            {
                V_M_TM_SYS_BaseData_channel channel = new V_M_TM_SYS_BaseData_channel()
                {
                    BaseDataID = (int)model.BaseDataID,
                    DataGroupID = (int)model.DataGroupID,
                    BaseDataType = model.BaseDataType,
                    ChannelNameBase = model.ChannelName,
                    ChannelCodeBase = model.ChannelCode
                    //,
                    //ChannelIsEnableBase = model.ChannelIsEnableBase
                };
                db.UpdateViewRow<V_M_TM_SYS_BaseData_channel, TM_SYS_BaseData>(channel);
                db.SaveChanges();
                return new Result(true, "修改成功");
            }
        }

        /// <summary>
        /// 根据ID获取渠道信息
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public static Result GetChannelById(long channelId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_channel.Where(p => p.BaseDataID == channelId).FirstOrDefault();
                return new Result(true, "", query);
            }

        }
        /// <summary>
        /// 禁用/启用渠道
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        public static Result UpdateChannelEnableById(long channelId, string isEnable)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var tm_Channel = db.V_M_TM_SYS_BaseData_channel.Where(p => p.BaseDataID == channelId).FirstOrDefault();
                if (tm_Channel == null)
                {
                    return new Result(false, "渠道不存在");
                }
                V_M_TM_SYS_BaseData_channel channel = new V_M_TM_SYS_BaseData_channel()
                {
                    BaseDataID = tm_Channel.BaseDataID,
                    DataGroupID = tm_Channel.DataGroupID,
                    BaseDataType = tm_Channel.BaseDataType,
                    ChannelNameBase = tm_Channel.ChannelNameBase,
                    ChannelCodeBase = tm_Channel.ChannelCodeBase,
                    ChannelIsEnableBase = isEnable
                };
                db.UpdateViewRow<V_M_TM_SYS_BaseData_channel, TM_SYS_BaseData>(channel);
                db.SaveChanges();
                return new Result(true, "保存成功");
            }
        }

        /// <summary>
        /// 删除渠道
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public static Result DeleteChannelById(long channelId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_channel.Where(p => p.BaseDataID == channelId).FirstOrDefault();

                if (query != null)
                {
                    db.DeleteViewRow<V_M_TM_SYS_BaseData_channel, TM_SYS_BaseData>(query);
                    db.SaveChanges();
                    return new Result(true, "删除成功");
                }
                return new Result(false, "删除失败");
            }
        }

        #endregion

       



       

        #region SysOption配置
        /// <summary>
        /// 获取SysOption 选择Type
        /// </summary>
        /// <returns></returns>
        public static Result GetSysOptionTypes()
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var result = db.TD_SYS_BizOption.Where(i => i.OptionType == "SysOptionType").OrderBy(i => i.Sort);
                    return new Result(true, "", result.ToList());
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        /// <summary>
        /// 获取SysOption 选择值
        /// </summary>
        /// <returns></returns>
        public static Result GetSysOptionValues(string type, string type2)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var result = type == "3" ? db.TD_SYS_FieldAlias.Where(i => i.DictTableName != null && i.DictTableType != null) : db.TD_SYS_FieldAlias;
                    if (!string.IsNullOrEmpty(type2))
                        if (type2.ToUpper() == "SOURCE")
                        {
                            result = result.Where(i => i.IsDegreeSource == true);
                        }
                        else if (type2.ToUpper() == "TARGET")
                        {
                            result = result.Where(i => i.IsDegreeTarge == true);
                        }
                    return new Result(true, "", result.ToList());
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        public static Result DeleteSysOption(int optionId)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    db.TD_SYS_FieldAliasDegree.Remove(db.TD_SYS_FieldAliasDegree.First(i => i.AliasDegeeID == optionId));
                    db.SaveChanges();
                    return new Result(true, "删除成功", null);
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        public static Result SaveSysOption(SysOption model, string user)
        {
            try
            {
                var authUser = JsonHelper.Deserialize<TM_AUTH_User>(user);
                using (var db = new CRMEntities())
                {
                    var result = false;
                    var loginName = db.TM_AUTH_User.First(p => p.UserID == authUser.UserID).LoginName;
                    if (model.AliasDegeeID > 0)
                    {
                        var item = db.TD_SYS_FieldAliasDegree.First(i => i.AliasDegeeID == model.AliasDegeeID);
                        item.AliasID = model.AliasID;
                        item.AliasID2 = model.AliasID2;
                        item.BasicContent = model.BasicContent;
                        item.Sort = model.Sort;
                        item.ValueDegeeType = model.ValueDegeeType;
                        item.ModifiedDate = DateTime.Now;
                        item.ModifiedUser = loginName;
                    }
                    else
                    {
                        var info = db.TD_SYS_FieldAliasDegree.FirstOrDefault(i => i.AliasID == model.AliasID);
                        if (info != null) return new Result(false, "该变量名已存在配置.");
                        var item = new TD_SYS_FieldAliasDegree()
                        {
                            AliasID = model.AliasID,
                            AliasID2 = model.AliasID2,
                            BasicContent = model.BasicContent,
                            Sort = model.Sort,
                            ValueDegeeType = model.ValueDegeeType,
                            ModifiedDate = DateTime.Now,
                            ModifiedUser = loginName,
                            AddedDate = DateTime.Now,
                            AddedUser = loginName,
                            Tablename = ""
                        };
                        db.TD_SYS_FieldAliasDegree.Add(item);
                    }
                    result = db.SaveChanges() > 0;
                    return new Result(result, result ? "操作成功" : "操作失败");
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        public static Result GetOptionData(string optionType, string optionText, string dp)
        {
            try
            {
                DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
                using (var db = new CRMEntities())
                {
                    var dbRes = db.TD_SYS_FieldAliasDegree.DefaultIfEmpty();
                    if (!string.IsNullOrEmpty(optionType)) dbRes = dbRes.Where(i => i.ValueDegeeType == optionType);
                    if (!string.IsNullOrEmpty(optionText))
                    {
                        var aliasId = Convert.ToInt32(optionText);
                        dbRes = dbRes.Where(i => i.AliasID == aliasId);
                    }
                    var res = dbRes
                        .Join(db.TD_SYS_BizOption.Where(i => i.OptionType == "SysOptionType"), a => a.ValueDegeeType, ar => ar.OptionValue, (a, ar) => new
                        {
                            AliasID = a.AliasID,
                            AliasID2 = a.AliasID2,
                            AliasDegeeID = a.AliasDegeeID,
                            Sort = a.Sort,
                            OptionValue = ar.OptionValue,
                            OptionText = ar.OptionText
                        })
                        .Join(db.TD_SYS_FieldAlias, b => b.AliasID, br => br.AliasID, (b, br) => new
                        {
                            AliasID2 = b.AliasID2,
                            AliasID = b.AliasID,
                            AliasDegeeID = b.AliasDegeeID,
                            Sort = b.Sort,
                            OptionValue = b.OptionValue,
                            OptionText = b.OptionText,
                            FieldDesc = br.FieldDesc
                        })
                         .Join(db.TD_SYS_FieldAlias, c => c.AliasID2, cr => cr.AliasID, (c, cr) => new
                        {
                            AliasID2 = c.AliasID2,
                            AliasID = c.AliasID,
                            AliasDegeeID = c.AliasDegeeID,
                            Sort = c.Sort,
                            OptionValue = c.OptionValue,
                            OptionText = c.OptionText,
                            FieldDesc = c.FieldDesc,
                            FieldDesc2 = cr.FieldDesc
                        })
                       .OrderByDescending(i => i.Sort);
                    return new Result(true, "", new List<object> { res.ToDataTableSourceVsPage(myDp) });
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        public static Result GetOptionById(int optionId)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var item = db.TD_SYS_FieldAliasDegree.FirstOrDefault(i => i.AliasDegeeID == optionId);
                    return new Result(item != null, "", item);
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        #endregion


        #region  价值度模型
        public static Result GetAliasDegree()
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var item = db.TD_SYS_DegreeModel;
                    return new Result(true, "", item.ToList());
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        public static Result GetDegreeModelList(string degreeName, string dp)
        {
            try
            {
                DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
                using (var db = new CRMEntities())
                {
                    var dbRes = db.TD_SYS_DegreeModel.DefaultIfEmpty();
                    //if (!string.IsNullOrEmpty(optionType)) dbRes = dbRes.Where(i => i.ValueDegeeType == optionType);
                    //if (!string.IsNullOrEmpty(optionText))
                    //{
                    //    var aliasId = Convert.ToInt32(optionText);
                    //    dbRes = dbRes.Where(i => i.AliasID == aliasId);
                    //}
                    if (!string.IsNullOrEmpty(degreeName)) dbRes = dbRes.Where(i => i.Name.Contains(degreeName));
                    var res = dbRes

                       .OrderByDescending(i => i.AddedDate);
                    return new Result(true, "", new List<object> { res.ToDataTableSourceVsPage(myDp) });
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        public static Result GetTargetData()
        {
            try
            {
                using (var db = new CRMEntities())
                {




                    var dbRes = from a in db.TD_SYS_FieldAliasDegree
                                join b in db.TD_SYS_FieldAlias on a.AliasID2 equals b.AliasID
                                select new
                                {
                                    a.AliasID2,
                                    b.FieldDesc,
                                };
                    return new Result(true, "", dbRes.ToList());
                }
            }

            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        public static Result SaveModelDegree(DegreeOption model, string user)
        {
            try
            {
                var authUser = JsonHelper.Deserialize<TM_AUTH_User>(user);
                using (var db = new CRMEntities())
                {
                    var result = false;
                    var loginName = db.TM_AUTH_User.First(p => p.UserID == authUser.UserID).LoginName;
                    if (model.DegreeModelID > 0)
                    {
                        var item = db.TD_SYS_DegreeModel.First(i => i.DegreeModelID == model.DegreeModelID);
                        item.Name = model.Name;
                        item.BasicContent = model.BasicContent;
                        item.ModifiedDate = DateTime.Now;
                        item.ModifiedUser = loginName;
                    }
                    else
                    {
                        var info = db.TD_SYS_DegreeModel.FirstOrDefault(i => i.Name == model.Name && i.DegreeModelID != model.DegreeModelID);
                        if (info != null) return new Result(false, "该名称已存在.");
                        var item = new TD_SYS_DegreeModel()
                        {
                            Name = model.Name,
                            BasicContent = model.BasicContent,
                            ModifiedDate = DateTime.Now,
                            ModifiedUser = loginName,
                            AddedDate = DateTime.Now,
                            AddedUser = loginName,

                        };
                        db.TD_SYS_DegreeModel.Add(item);
                    }
                    result = db.SaveChanges() > 0;
                    return new Result(result, result ? "操作成功" : "操作失败");
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        public static Result GetDegreeModelById(int degreeId)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var item = db.TD_SYS_DegreeModel.FirstOrDefault(i => i.DegreeModelID == degreeId);
                    return new Result(item != null, "", item);
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        public static Result DeleteDegreeModel(int degreeId)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    db.TD_SYS_DegreeModel.Remove(db.TD_SYS_DegreeModel.First(i => i.DegreeModelID == degreeId));
                    db.SaveChanges();
                    return new Result(true, "删除成功", null);
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message, null);
            }
        }

        #endregion

        #region 业务部门管理
        /// <summary>
        /// 根据主键ID删除信息
        /// </summary>
        /// <param name="tmpId"></param>
        /// <returns></returns>
        public static Result DeleteBsiDptDataById(int tmpId)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    var query = db.TD_SYS_BusinessDepartment.Where(p => p.DepartmentID == tmpId).FirstOrDefault();
                    if (query != null)
                    {
                        var child = db.TD_SYS_BusinessDepartment.Where(p => p.ParentID == query.DepartmentID).ToList();
                        if (child.Any()) return new Result(false, "此部门下含有子部门不允许删除");
                        var role = db.TR_AUTH_UserBusinessDepartment.Where(p => p.DepartmentID == query.DepartmentID).ToList();
                        if (role.Any()) return new Result(false, "此部门已被分配角色不允许删除");
                        db.TD_SYS_BusinessDepartment.Remove(query);
                        db.SaveChanges();
                        return new Result(true, "删除成功");
                    }

                    return new Result(false, "删除失败");
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        /// <summary>
        /// 增加或者更新信息
        /// </summary>
        /// <param name="templet"></param>
        /// <returns></returns>
        public static Result AddOrUpdateBsiDptData(BsiDpt templet)
        {
            using (CRMEntities db = new CRMEntities())
            {
                TD_SYS_BusinessDepartment ent = new TD_SYS_BusinessDepartment();
                ent.AddedDate = DateTime.Now;

                var pq = db.TD_SYS_BusinessDepartment.Where(p => p.DepartmentID == templet.ParentBisDpt).FirstOrDefault();
                if (pq == null) ent.DepartmentGrade = 1; else ent.DepartmentGrade = pq.DepartmentGrade + 1;

                if (templet.ID != null)//修改
                {
                    ent.DepartmentID = (int)templet.ID;
                    var qyy = db.TD_SYS_BusinessDepartment.Where(p => p.DepartmentName == templet.BisDptName && p.DepartmentID != ent.DepartmentID).ToList();
                    if (qyy.Count >= 1)
                    {
                        return new Result(false, "业务部门名称已存在");
                    }
                    var query = db.TD_SYS_BusinessDepartment.Where(p => p.DepartmentID == ent.DepartmentID).FirstOrDefault();
                    if (query != null)
                    {
                        query.DepartmentName = templet.BisDptName;
                        query.ParentID = templet.ParentBisDpt;
                        query.DepartmentGrade = ent.DepartmentGrade;
                        var entry = db.Entry(query);
                        entry.State = EntityState.Modified;
                        db.SaveChanges();
                        return new Result(true, "修改成功", query.DepartmentID);
                    }
                }
                var q = db.TD_SYS_BusinessDepartment.Where(p => p.DepartmentName == templet.BisDptName).FirstOrDefault();
                if (q != null)
                {
                    return new Result(false, "业务部门名称已存在");
                }
                ent.DepartmentName = templet.BisDptName;
                ent.ParentID = templet.ParentBisDpt;
                db.TD_SYS_BusinessDepartment.Add(ent);
                db.SaveChanges();
                var qq = db.TD_SYS_BusinessDepartment.Where(p => p.DepartmentName == templet.BisDptName).FirstOrDefault();
                return new Result(true, "添加成功", qq.DepartmentID);
            }
        }
        /// <summary>
        /// 获取沟通模板树图
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Result GetBsiDptDataList(string key)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TD_SYS_BusinessDepartment.ToList();//获取数据列表
                if (!string.IsNullOrEmpty(key)) query = query.Where(p => p.DepartmentName.Contains(key)).ToList();
                //把数据列表转化为所要的数据源
                List<TreeDataSource> nodes = new List<TreeDataSource>();
                TreeDataSource node = null;
                foreach (var item in query)
                {
                    node = new TreeDataSource();
                    node.nodeId = item.DepartmentID.ToString();
                    node.nodePId = item.ParentID.ToString();
                    node.nodeGrade = item.DepartmentGrade;
                    node.nodeName = item.DepartmentName;
                    nodes.Add(node);
                }
                var levelOne = nodes.Where(o => o.nodeGrade == 1).ToList();
                //var levelOne = query.Where(o => o.DataGroupGrade == 0).ToList();
                var result = new List<TreeNode>();
                foreach (var cate in levelOne)
                {
                    //var node = Arvato.CRM.BizLogic.Service.CreateTreeNode(query, cate.DataGroupID);
                    var res = Arvato.CRM.BizLogic.Service.CreateTreeNodeData(nodes, cate.nodeId.ToString());
                    if (node != null)
                    {
                        result.Add(res);
                    }
                }
                return new Result(true, "", result);
            }
        }

        /// <summary>
        /// 根据主键ID获取单条信息
        /// </summary>
        /// <param name="tmpId"></param>
        /// <returns></returns>
        public static Result GetBsiDptDataById(int tmpId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TD_SYS_BusinessDepartment.Where(p => p.DepartmentID == tmpId).FirstOrDefault();
                return new Result(true, "", query);
            }
        }

        public static Result GetBsiDptGrade()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.TD_SYS_BusinessDepartment
                             select a.DepartmentGrade).Distinct().ToList();
                return new Result(true, "", query);
            }
        }

        /// <summary>
        /// 获取所属父列表
        /// </summary>
        /// <param name="dataGroupGrade"></param>
        /// <returns></returns>
        public static Result GetParentBsiDpt()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TD_SYS_BusinessDepartment.OrderBy(p => p.DepartmentGrade).ToList();
                return new Result(true, "", query);
            }
        }

        public static Result LoadDepartmentByUser(int userID)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.TR_AUTH_UserBusinessDepartment
                            join b in db.TD_SYS_BusinessDepartment on a.DepartmentID equals b.DepartmentID
                            where a.UserID == userID
                            select new
                            {
                                a.UserID,
                                b.DepartmentID,
                                b.DepartmentName,

                            };
                return new Result(true, "", query.ToList());

                //db.TR_AUTH_UserBusinessDepartment.Where(p => p.UserID == userID);
            }
        }

        #endregion

        #region 类别管理
        public static Result GetCampaignCatgList(string parentCatg, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.V_M_TM_SYS_BaseData_campaign
                            select new
                            {
                                a.BaseDataID,
                                a.BaseDataType,
                                a.DataGroupID,
                                a.CampType,
                                a.CampAttrName,
                                a.CampGrade,
                                a.AliaskeyBase,
                                a.AliasSubkeyBase
                            };
                if (!string.IsNullOrEmpty(parentCatg)) query = query.Where(p => p.CampType == parentCatg);

                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }
        public static Result AddCampaignData(Campaign cam)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var q = db.V_M_TM_SYS_BaseData_campaign.Where(p => p.AliasSubkeyBase == cam.AliasSubkeyBase).FirstOrDefault();
                if (q != null) return new Result(false, "数据库中存在相同的后缀");
                V_M_TM_SYS_BaseData_campaign ent = new V_M_TM_SYS_BaseData_campaign();
                ent.AliaskeyBase = cam.AliaskeyBase;
                ent.AliasSubkeyBase = cam.AliasSubkeyBase;
                ent.CampGrade = cam.CampGrade;
                ent.CampAttrName = cam.CampAttrName;
                ent.CampType = cam.CampType;
                ent.DataGroupID=1;
                ent.BaseDataType="campaign";

                db.AddViewRow<V_M_TM_SYS_BaseData_campaign, TM_SYS_BaseData>(ent);
                db.SaveChanges();
                return new Result(true, "添加成功");
            }
        }
        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public static Result UpdateCampaignData(Campaign cam)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_campaign.Where(p => p.BaseDataID == cam.ID).FirstOrDefault();
                if (query != null)
                {
                    var q = db.V_M_TM_SYS_BaseData_campaign.Where(p => p.BaseDataID != cam.ID && p.AliasSubkeyBase == cam.AliasSubkeyBase).FirstOrDefault();
                    if (q != null) return new Result(false, "数据库中存在相同的后缀名");


                    V_M_TM_SYS_BaseData_campaign ent = new V_M_TM_SYS_BaseData_campaign();
                    ent.AliaskeyBase = cam.AliaskeyBase;
                    ent.AliasSubkeyBase = cam.AliasSubkeyBase;
                    ent.CampGrade = cam.CampGrade;
                    ent.CampAttrName = cam.CampAttrName;
                    ent.CampType = cam.CampType;
                    ent.BaseDataID = cam.ID;

                    db.UpdateViewRow<V_M_TM_SYS_BaseData_campaign, TM_SYS_BaseData>(ent);

                    db.SaveChanges();
                    return new Result(true, "修改成功");
                }
                return new Result(false, "修改失败");
            }
        }

        public static Result GetCampaignById(long camid)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_campaign.Where(p => p.BaseDataID == camid).FirstOrDefault();
                return new Result(true, "", query);
            }
        }
        /// <summary>
        /// 根据ID删除品牌信息
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public static Result DeleteCampaignById(long camid)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_campaign.Where(p => p.BaseDataID == camid).FirstOrDefault();

                if (query != null)
                {
                    dynamic t = db.DeleteViewRow<V_M_TM_SYS_BaseData_campaign, TM_SYS_BaseData>(query);
                    db.SaveChanges();
                    return new Result(true, "删除成功");
                }
                return new Result(false, "删除失败");
            }
        }

        public static Result GetCampaignParentCatg(int? grade)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_campaign.Where(p=>p.CampGrade==grade).ToList();

                return new Result(true, "", query);
            }
        }
        #endregion

        #region 供应商
        public static Result GetSupplierData(string dp, string SupplierCode, string SupplierName)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.V_M_TM_SYS_BaseData_supplier.Where(n => n.SupplierStatus == 1).Select(t => new { t.BaseDataID, t.SupplierCode, t.SupplierName, t.SupplierPhone, t.SupplierAddress, t.SupplierFax, t.SupplierContactPerson, AddedUser = db.TM_AUTH_User.Where(p => SqlFunctions.StringConvert((decimal)p.UserID) == t.AddedUser).Select(m => m.UserName),t.AddedDate });
                if (!string.IsNullOrWhiteSpace(SupplierCode))
                {
                    query = query.Where(t => t.SupplierCode.Contains(SupplierCode));
                }
                if (!string.IsNullOrWhiteSpace(SupplierName))
                {
                    query = query.Where(t => t.SupplierName.Contains(SupplierName));
                }
                return new Result(true, "", query.ToDataTableSourceVsPage(myDp));
            }
        }
        
        //public static Result AddSupplier(string SupplierCode, string SupplierName, string SupplierPhone, string SupplierAddress, string SupplierFax, string SupplierContactPerson)
        //{
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        V_M_TM_SYS_BaseData_supplier supplier = new V_M_TM_SYS_BaseData_supplier();
        //        supplier.SupplierCode = SupplierCode;
        //        supplier.SupplierName = SupplierName;
        //        supplier.SupplierPhone = SupplierPhone;
        //        supplier.SupplierAddress = SupplierAddress;
        //        supplier.SupplierFax = SupplierFax;
        //        supplier.SupplierContactPerson = SupplierContactPerson;
        //        //db.V_M_TM_SYS_BaseData_supplier.Add(supplier);
        //        db.AddViewRow<V_M_TM_SYS_BaseData_supplier, TM_SYS_BaseData>(supplier);
        //        try
        //        {
        //            db.SaveChanges();
        //            return new Result(true);
        //        }
        //        catch (Exception ex)
        //        {                    
        //            return new Result(false, "添加供应商失败");
        //        }
        //    }
        //}


        public static Result AddSupplierData(Supplier model,string addUserID)
        {
            using (CRMEntities db = new CRMEntities())
            {
                V_M_TM_SYS_BaseData_supplier supplier = new V_M_TM_SYS_BaseData_supplier();
                supplier.SupplierCode = model.SupplierCode;
                supplier.SupplierName = model.SupplierName;
                supplier.SupplierPhone = model.SupplierPhone;
                supplier.SupplierAddress = model.SupplierAddress;
                supplier.SupplierFax = model.SupplierFax;
                supplier.SupplierContactPerson = model.SupplierContactPerson;
                supplier.AddedUser = addUserID;
                supplier.AddedDate = DateTime.Now;
                supplier.SupplierStatus = 1;
                //db.V_M_TM_SYS_BaseData_supplier.Add(supplier);
                db.AddViewRow<V_M_TM_SYS_BaseData_supplier, TM_SYS_BaseData>(supplier);
                try
                {
                    db.SaveChanges();
                    return new Result(true,"");
                }
                catch (Exception ex)
                {
                    return new Result(false, "添加供应商失败");
                }
            }
            }

        public static Result GetSupplierByID(long? id)
        {
            using (CRMEntities db = new CRMEntities())
            {
                if (id != null)
                {
                    var query = db.V_M_TM_SYS_BaseData_supplier.FirstOrDefault(t => t.BaseDataID == id);
                    return new Result(true, "", query);
                }
                else
                {
                    return new Result(false, "查找条件为空");
                }
            }
        }

        public static Result UpdateSupplier(Supplier supplier,string updateuserid)
        {
            if (supplier == null)
            {
                return new Result(false, "更新的数据为空");
            }
            using (CRMEntities db = new CRMEntities())
            {
                V_M_TM_SYS_BaseData_supplier oldsupplier = db.V_M_TM_SYS_BaseData_supplier.FirstOrDefault(t => t.BaseDataID == supplier.BaseDateID);
                V_M_TM_SYS_BaseData_supplier newsupplier =new V_M_TM_SYS_BaseData_supplier();
                newsupplier.BaseDataID = supplier.BaseDateID.Value;
                newsupplier.SupplierCode = supplier.SupplierCode;
                newsupplier.SupplierName = supplier.SupplierName;
                newsupplier.SupplierPhone = supplier.SupplierPhone;
                newsupplier.SupplierFax = supplier.SupplierFax;
                newsupplier.SupplierAddress = supplier.SupplierAddress;
                newsupplier.SupplierContactPerson = supplier.SupplierContactPerson;
                newsupplier.BaseDataType = oldsupplier.BaseDataType;
                newsupplier.DataGroupID = oldsupplier.DataGroupID;
                newsupplier.AddedDate = oldsupplier.AddedDate;
                newsupplier.AddedUser = oldsupplier.AddedUser;
                newsupplier.SupplierDescription = oldsupplier.SupplierDescription;
                newsupplier.SupplierFullName = oldsupplier.SupplierFullName;
                newsupplier.ModifiedUser = updateuserid;
                newsupplier.ModifiedDate = DateTime.Now;
                newsupplier.SupplierOrgType = oldsupplier.SupplierOrgType;
                newsupplier.SupplierStatus = oldsupplier.SupplierStatus;
                

                db.UpdateViewRow<V_M_TM_SYS_BaseData_supplier, TM_SYS_BaseData>(newsupplier);
                try
                {
                    db.SaveChanges();
                    return new Result(true, "");
                }
                catch (Exception ex)
                {
                    return new Result(false, "更新失败");
                }
                
            }
        }


        public static Result StopChannel(string basedataID,int userID)
        {
            using (CRMEntities db=new CRMEntities())
            {
                try
                {
                    long id = Convert.ToInt64(basedataID);
                    var query = db.V_M_TM_SYS_BaseData_supplier.Where(n => n.BaseDataID == id).FirstOrDefault();                
                    if (query != null)
                    {
                        V_M_TM_SYS_BaseData_supplier supplier = new V_M_TM_SYS_BaseData_supplier()
                        {
                            BaseDataID = query.BaseDataID,
                            BaseDataType = query.BaseDataType,
                            DataGroupID = query.DataGroupID,
                            SupplierCode = query.SupplierCode,
                            SupplierName = query.SupplierName,
                            SupplierOrgType = query.SupplierOrgType,
                            SupplierStatus = 2,
                            AddedDate = query.AddedDate,
                            AddedUser = query.AddedUser,
                            ModifiedDate = DateTime.Now,
                            ModifiedUser = userID.ToString(),
                            SupplierAddress = query.SupplierAddress,
                            SupplierContactPerson = query.SupplierContactPerson,
                            SupplierCreateUser = query.SupplierCreateUser,
                            SupplierDescription = query.SupplierDescription,
                            SupplierFax = query.SupplierFax,
                            SupplierFullName = query.SupplierFullName,
                            SupplierPhone = query.SupplierPhone
                        };
                        db.UpdateViewRow<V_M_TM_SYS_BaseData_supplier, TM_SYS_BaseData>(supplier);
                        db.SaveChanges();
                        return new Result(true);
                    }
                    else
                    {
                        return new Result(false);
                    }
                }
                catch (Exception ex)
                {
                    Log4netHelper.WriteErrorLog("StopChannel：" + ex.ToString());
                    throw;
                }           
            }   
        }
        #endregion


        #region 卡片类型管理

        #region 获取卡片数据
        /// <summary>
        /// 获取卡片数据
        /// </summary>
        /// <param name="CarName"></param>
        /// <param name="CarStatu"></param>
        /// <returns></returns>
        public static Result GetCarTypeData(string CarName, string CarCode, string dp)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using(var db=new CRMEntities())
	        {
                var query = from i in db.V_M_TM_SYS_BaseData_cardType select i;
                query = string.IsNullOrWhiteSpace(CarName) ? query : query.Where(a => a.CardTypeNameBase == CarName);
                query = string.IsNullOrWhiteSpace(CarCode) ? query : query.Where(a => a.CardTypeCodeBase == CarCode);

                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
	        }
            
        }
        #endregion

        /// <summary>
        /// 根据Id 获取卡片类型数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Result GetCarTypeById(int id) {
            using (var db = new CRMEntities())
            {
                var query = (from i in db.V_M_TM_SYS_BaseData_cardType.Where(a => a.BaseDataID == id) select i).FirstOrDefault();
                return new Result(true, "", query);
            }
        }
        /// <summary>
        /// 修改卡片类型
        /// </summary>
        /// <param name="id"></param>
        /// <param name="carName"></param>
        /// <param name="carCode"></param>
        /// <param name="status"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Result UpdateCarTypeData(int id,string carName,string carCode,string user){

            var authUser = JsonHelper.Deserialize<TM_AUTH_User>(user);
                
            using (var db = new CRMEntities())
            {
                try
                {
                    var loginName = db.TM_AUTH_User.First(p => p.UserID == authUser.UserID).LoginName;
                    var query = (from i in db.V_M_TM_SYS_BaseData_cardType.Where(a => a.BaseDataID == id) select i).FirstOrDefault();
                    if (query == null) return new Result(false, "修改失败");
                    var queryName = (from i in db.V_M_TM_SYS_BaseData_cardType.Where(a => a.CardTypeNameBase == carName && a.BaseDataID != id) select i).FirstOrDefault();
                    if (queryName != null) return new Result(false, "卡片类型名称已存在!");
                    var queryCode = (from i in db.V_M_TM_SYS_BaseData_cardType.Where(a => a.CardTypeCodeBase == carCode && a.BaseDataID != id) select i).FirstOrDefault();
                    if (queryCode != null) return new Result(false, "卡片类型代码已存在!");
                    var basedata = (from i in db.TM_SYS_BaseData where i.BaseDataID == id select i).FirstOrDefault();
                    basedata.Str_Attr_1 = carName;
                    basedata.Str_Attr_2 = carCode;
                    basedata.ModifiedUser = loginName;
                  
                    basedata.ModifiedDate = DateTime.Now;
                    //db.TM_SYS_BaseData.
                    db.SaveChanges();
                    return new Result(true, "修改成功");
                }
                catch (Exception ex)
                {
                    return new Result(false, "修改失败");
                   
                }
            }
        }
        /// <summary>
        /// 添加卡片类型
        /// </summary>
        /// <param name="carName"></param>
        /// <param name="carCode"></param>
        /// <param name="status"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Result AddCarTypeData(string carName, string carCode, string user)
        {

            var authUser = JsonHelper.Deserialize<TM_AUTH_User>(user);
            using (var db = new CRMEntities())
            {
                try
                {
                    var loginName = db.TM_AUTH_User.First(p => p.UserID == authUser.UserID).LoginName;
                    var query = (from i in db.V_M_TM_SYS_BaseData_cardType.Where(a => a.CardTypeNameBase == carName) select i).FirstOrDefault();
                    if (query != null) return new Result(false, "卡片类型名称已经存在!");
                    var query1 = (from i in db.V_M_TM_SYS_BaseData_cardType.Where(a => a.CardTypeCodeBase == carCode) select i).FirstOrDefault();
                    if (query1 != null) return new Result(false, "卡片类型代码已经存在!");
                    V_M_TM_SYS_BaseData_cardType ent = new V_M_TM_SYS_BaseData_cardType();
                    ent.CardTypeNameBase = carName;
                    ent.CardTypeCodeBase = carCode;
                    ent.AddedUser = loginName;
                    //ent.CardTypeStatus = status;
                    ent.DataGroupID = 1;
                    ent.BaseDataType = "cardType";
                       
                    ent.AddedDate = DateTime.Now;
                    dynamic t = db.AddViewRow<V_M_TM_SYS_BaseData_cardType, TM_SYS_BaseData>(ent);
                    db.SaveChanges();
                    return new Result(true, "添加成功");
                }
                catch (Exception ex)
                {
                    return new Result(false, "添加失败");

                }
            }
        }

        public static Result DeleteCarType(long Id)
        {
            using (var db = new CRMEntities())
            {
                try
                {
                    var query = (from i in db.V_M_TM_SYS_BaseData_cardType.Where(a =>a.BaseDataID == Id) select i).FirstOrDefault();
                    if (query == null) return new Result(false, "卡片类型不存在!");
                    V_M_TM_SYS_BaseData_cardType ent = new V_M_TM_SYS_BaseData_cardType();
                    ent.BaseDataID = Id;
                    dynamic t = db.DeleteViewRow<V_M_TM_SYS_BaseData_cardType, TM_SYS_BaseData>(ent);
                    db.SaveChanges();
                    return new Result(true, "删除成功");
                }
                catch (Exception ex)
                {
                    return new Result(false, "删除失败");

                }
            }
        }
        #endregion
    }
}
