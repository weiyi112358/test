using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.BizLogic
{
    public static class TmpCommunication
    {
        /// <summary>
        /// 获取沟通模板树图
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Result GetTmpDataList(string sessionStr, string type, string key)
        {
            using (CRMEntities db = new CRMEntities())
            {
                //List<TM_Act_CommunicationTemplet> query = db.TM_Act_CommunicationTemplet.ToList();//获取数据列表
                //获取一级目录
                var query1 = db.TD_SYS_BizOption.Where(p => p.OptionType == "CommunicationType" && p.Enable == true).OrderBy(p => p.Sort).ToList();
                var query = from c in db.TM_Act_CommunicationTemplet
                            where c.IsSysInit == null || c.IsSysInit == false
                            select new
                            {
                                c.TempletID,
                                c.Type,
                                c.Name,
                                c.Category,
                            };
                //获取二级目录--可扩展，
                var query2 = query.Where(q => q.Type == type).ToList();
                if (!string.IsNullOrEmpty(key)) query2 = query2.Where(p => p.Name.Contains(key)).ToList();
                //把数据列表转化为所要的数据源
                List<TreeDataSource> nodes = new List<TreeDataSource>();
                TreeDataSource node = null;
                foreach (var item in query1)
                {
                    node = new TreeDataSource();
                    node.nodeId = item.OptionValue;
                    node.nodePId = null;
                    node.nodeGrade = 0;
                    node.nodeName = item.OptionText;
                    nodes.Add(node);
                }
                foreach (var item in query2)
                {
                    node = new TreeDataSource();
                    node.nodeId = item.TempletID.ToString();
                    node.nodePId = item.Category;
                    node.nodeGrade = 1;
                    node.nodeName = item.Name;
                    nodes.Add(node);
                }
                Dictionary<string, object> dictSession = JsonHelper.Deserialize<Dictionary<string, object>>(sessionStr);
                AuthModel authModel = JsonHelper.Deserialize<AuthModel>(dictSession["auth"].ToString());
                int dataGroupID = authModel.DataGroupID == null ? 0 : (int)authModel.DataGroupID;
                var qGroupPersonalize = db.TM_SYS_Class.Where(c => c.ClassType.Trim().Equals("2") && c.UserID == authModel.UserID
                   && c.DataGroupID == authModel.DataGroupID).ToList();
                foreach (var g in qGroupPersonalize)
                {
                    var qNode = nodes.Where(p => p.nodePId != null && p.nodePId.Trim() == g.ClassID.ToString()).FirstOrDefault();
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
                //var levelOne = query.Where(o => o.DataGroupGrade == 0).ToList();
                var result = new List<TreeNode>();
                foreach (var cate in levelOne)
                {
                    //var node = Arvato.CRM.BizLogic.Service.CreateTreeNode(query, cate.DataGroupID);
                    var res = Arvato.CRM.BizLogic.Service.CreateTreeNodeData(nodes, cate.nodeId);
                    if (node != null && res.children != null)
                    {
                        result.Add(res);
                    }
                }
                return new Result(true, "", result);
            }
        }


        /// <summary>
        /// 根据主键ID获取单条沟通模板信息
        /// </summary>
        /// <param name="tmpId"></param>
        /// <returns></returns>
        public static Result GetTmpDataById(int tmpId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_Act_CommunicationTemplet.Where(p => p.TempletID == tmpId).FirstOrDefault();
                return new Result(true, "", query);
            }
        }
        /// <summary>
        /// 根据主键ID删除单条沟通模板信息
        /// </summary>
        /// <param name="tmpId"></param>
        /// <returns></returns>
        public static Result DeleteTmpDataById(int tmpId)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    var query = db.TM_Act_CommunicationTemplet.Where(p => p.TempletID == tmpId).FirstOrDefault();
                    var querytemplete = db.TM_Mem_CouponPool.Where(p => p.TempletID == tmpId).FirstOrDefault();
                    if (querytemplete != null)
                    {
                        return new Result(false, "该模板已被应用于优惠券，不能删除！");
                    }
                    if (query != null)
                    {
                        if (query.IsSysInit == true)
                        {
                            return new Result(false, "系统初始化短信不能删除！");
                        }
                        var couponLimit = db.TM_CRM_CouponLimit.Where(l => l.TempletID == tmpId);
                        if (couponLimit.Count() > 0)
                        {
                            foreach (var obj in couponLimit)
                            {
                                db.TM_CRM_CouponLimit.Remove(obj);
                            }
                        }
                        db.TM_Act_CommunicationTemplet.Remove(query);
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
        /// 增加或者更新沟通短信模板信息
        /// </summary>
        /// <param name="templet"></param>
        /// <returns></returns>
        public static Result AddOrUpdateTmpMessageData(TmpCommunicationModel templet)
        {
            using (CRMEntities db = new CRMEntities())
            {
                TM_Act_CommunicationTemplet ent = new TM_Act_CommunicationTemplet();
                ent.Name = templet.Name;
                ent.Category = templet.Category;
                ent.Enable = templet.Enable;
                ent.BasicContent = templet.BasicContent;
                ent.ReferenceNo = templet.ReferenceNo;
                ent.Remark = templet.Remark;
                ent.DataGroupID = (int)templet.DataGroupID;
                //ent.SchemaId = templet.SchemaId;
                ent.BusinessType = templet.BusinessType;
                if (templet.TempletID != null)//修改
                {
                    ent.TempletID = (int)templet.TempletID;
                    var qyy = db.TM_Act_CommunicationTemplet.Where(p => p.Name == ent.Name && p.Type == "SMS" && p.TempletID != ent.TempletID).ToList();
                    if (qyy.Count >= 1)
                    {
                        return new Result(false, "模板名称与其他模板重名");
                    }
                    var query = db.TM_Act_CommunicationTemplet.Where(p => p.TempletID == ent.TempletID && p.DataGroupID == ent.DataGroupID).FirstOrDefault();
                    if (query != null)
                    {
                        query.Name = ent.Name;
                        query.Category = ent.Category;
                        query.Enable = ent.Enable;
                        query.BasicContent = ent.BasicContent;
                        query.Remark = ent.Remark;
                        query.ReferenceNo = ent.ReferenceNo;
                        query.ModifiedDate = DateTime.Now;
                        query.ModifiedUser = templet.ModifiedUser;
                        var entry = db.Entry(query);
                        entry.State = EntityState.Modified;
                        db.SaveChanges();
                        return new Result(true, "修改成功", query.TempletID);
                    }
                    return new Result(false, "没有权限修改此模板");
                }
                var q = db.TM_Act_CommunicationTemplet.Where(p => p.Name == ent.Name && p.Type == "SMS").FirstOrDefault();
                if (q != null)
                {
                    return new Result(false, "此模板已存在");
                }
                ent.Type = "SMS";
                ent.Topic = "短信";
                ent.AddedUser = templet.AddedUser;
                ent.AddedDate = DateTime.Now;
                ent.ModifiedDate = DateTime.Now;
                ent.ModifiedUser = templet.ModifiedUser;
                ent.Status = 0;
                db.TM_Act_CommunicationTemplet.Add(ent);
                db.SaveChanges();
                var qy = db.TM_Act_CommunicationTemplet.Where(p => p.Name == ent.Name && p.Type == "SMS").FirstOrDefault();
                return new Result(true, "添加成功", qy.TempletID);
            }
        }

        /// <summary>
        /// 获取群组沟通模板类型
        /// </summary>
        /// <returns></returns>
        public static Result GetTmpCatg()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TD_SYS_BizOption.Where(p => p.OptionType == "CommunicationType" && p.Enable == true).OrderBy(p => p.Sort).ToList();

                return new Result(true, "", query);
            }
        }

        /// <summary>
        /// 获取短信类型 车享平台
        /// </summary>
        /// <returns></returns>
        public static Result GetTmpCXBusinessType()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TD_SYS_BizOption.Where(p => p.OptionType == "SMSBusinessType" && p.Enable == true).OrderBy(p => p.Sort).ToList();
                var query2 = db.TD_SYS_BizOption.Where(p => p.OptionType == "SMSBusinessType").OrderBy(p => p.Sort).ToList();

                return new Result(true, "", query);
            }
        }
        /// <summary>
        /// 获取问卷列表
        /// </summary>
        /// <returns></returns>
        public static Result GetQuestionList()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_Act_CommunicationTemplet.Where(p => p.Type == "Question").OrderBy(p => p.AddedDate).ToList();

                return new Result(true, "", query);
            }
        }

        /// <summary>
        /// 获取个性化元素列表
        /// </summary>
        /// <returns></returns>
        public static Result GetElementsList()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TD_SYS_FieldAlias.Where(p => p.IsCommunicationTemplet == true).OrderBy(p => p.AddedDate).ToList();

                return new Result(true, "", query);
            }
        }


        /// <summary>
        /// 增加或者更新邮件模板信息
        /// </summary>
        /// <param name="templet"></param>
        /// <returns></returns>
        public static Result AddOrUpdateTmpMailData(TmpCommunicationModel templet)
        {
            using (CRMEntities db = new CRMEntities())
            {
                TM_Act_CommunicationTemplet ent = new TM_Act_CommunicationTemplet();
                ent.Name = templet.Name;
                ent.Topic = templet.Topic;
                ent.Category = templet.Category;
                ent.Enable = templet.Enable;
                ent.BasicContent = templet.BasicContent;
                ent.ReferenceNo = templet.ReferenceNo;
                ent.Remark = templet.Remark;
                ent.DataGroupID = (int)templet.DataGroupID;
                if (templet.TempletID != null)//修改
                {
                    ent.TempletID = (int)templet.TempletID;
                    var qyy = db.TM_Act_CommunicationTemplet.Where(p => p.Name == ent.Name && p.Type == "Mail" && p.TempletID != ent.TempletID).ToList();
                    if (qyy.Count >= 1)
                    {
                        return new Result(false, "模板名称与其他模板重名");
                    }
                    var query = db.TM_Act_CommunicationTemplet.Where(p => p.TempletID == ent.TempletID && p.DataGroupID == ent.DataGroupID).FirstOrDefault();
                    if (query != null)
                    {
                        query.Name = ent.Name;
                        query.Topic = ent.Topic;
                        query.Category = ent.Category;
                        query.Enable = ent.Enable;
                        query.BasicContent = ent.BasicContent;
                        query.ReferenceNo = ent.ReferenceNo;
                        query.Remark = ent.Remark;
                        query.ModifiedDate = DateTime.Now;
                        query.ModifiedUser = templet.ModifiedUser;
                        var entry = db.Entry(query);
                        entry.State = EntityState.Modified;
                        db.SaveChanges();
                        return new Result(true, "修改成功", query.TempletID);
                    }
                    return new Result(false, "没有权限修改此模板");
                }
                var q = db.TM_Act_CommunicationTemplet.Where(p => p.Name == ent.Name && p.Type == "Mail").FirstOrDefault();
                if (q != null)
                {
                    return new Result(false, "此模板已存在");
                }
                ent.Type = "Mail";
                ent.AddedUser = templet.AddedUser;
                ent.AddedDate = DateTime.Now;
                ent.ModifiedDate = DateTime.Now;
                ent.ModifiedUser = templet.ModifiedUser;
                ent.Status = 0;
                db.TM_Act_CommunicationTemplet.Add(ent);
                db.SaveChanges();
                var qy = db.TM_Act_CommunicationTemplet.Where(p => p.Name == ent.Name && p.Type == "Mail").FirstOrDefault();
                return new Result(true, "添加成功", qy.TempletID);
            }
        }


        /// <summary>
        /// 增加或者更新微信模板信息
        /// </summary>
        /// <param name="templet"></param>
        /// <returns></returns>
        public static Result AddOrUpdateTmpWeChatData(TmpCommunicationModel templet)
        {
            using (CRMEntities db = new CRMEntities())
            {
                TM_Act_CommunicationTemplet ent = new TM_Act_CommunicationTemplet();
                ent.Name = templet.Name;
                ent.SubType = templet.SubType;
                ent.Category = templet.Category;
                ent.Enable = templet.Enable;
                ent.BasicContent = string.IsNullOrEmpty(templet.BasicContent) ? "" : templet.BasicContent;
                ent.ReferenceNo = templet.ReferenceNo;
                ent.Remark = templet.Remark;
                ent.DataGroupID = (int)templet.DataGroupID;
                ent.Topic = "WeChat";

                if (templet.TempletID != null)//修改
                {
                    ent.TempletID = (int)templet.TempletID;
                    var query = db.TM_Act_CommunicationTemplet.Where(p => p.TempletID == ent.TempletID && p.DataGroupID == ent.DataGroupID).FirstOrDefault();
                    if (query != null)
                    {
                        query.Name = ent.Name;
                        query.Topic = ent.Topic;
                        query.Category = ent.Category;
                        query.Enable = ent.Enable;
                        query.BasicContent = ent.BasicContent;
                        query.ReferenceNo = ent.ReferenceNo;
                        query.Remark = ent.Remark;
                        query.ModifiedDate = DateTime.Now;
                        query.ModifiedUser = templet.ModifiedUser;
                        var entry = db.Entry(query);
                        entry.State = EntityState.Modified;
                        db.SaveChanges();
                        return new Result(true, "修改成功");
                    }
                    return new Result(false, "没有权限修改此模板");
                }
                var q = db.TM_Act_CommunicationTemplet.Where(p => p.Name == ent.Name && p.Type == "WeChat").FirstOrDefault();
                if (q != null)
                {
                    return new Result(false, "此模板已存在");
                }
                ent.Type = "WeChat";
                ent.AddedUser = templet.AddedUser;
                ent.AddedDate = DateTime.Now;
                ent.ModifiedDate = DateTime.Now;
                ent.ModifiedUser = templet.ModifiedUser;
                ent.Status = 0;
                db.TM_Act_CommunicationTemplet.Add(ent);
                db.SaveChanges();
                return new Result(true, "添加成功");
            }
        }

        /// <summary>
        /// 增加或者更新优惠券模板信息
        /// </summary>
        /// <param name="templet"></param>
        /// <returns></returns>
        public static Result AddOrUpdateTmpCouponData(TmpCommunicationModel templet, string couponLimit)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    List<ActLimit> limit = JsonHelper.Deserialize<List<ActLimit>>(couponLimit);
                    TM_Act_CommunicationTemplet ent = new TM_Act_CommunicationTemplet();
                    ent.Name = templet.Name;
                    ent.Topic = templet.Topic;
                    ent.Category = templet.Category;
                    ent.Enable = templet.Enable;
                    ent.BasicContent = templet.BasicContent;
                    ent.InternalSetPrice = templet.InternalSetPrice;
                    ent.SalePrice = templet.SalePrice;
                    ent.Proportion = templet.Proportion;
                    ent.ReferenceNo = templet.ReferenceNo;
                    ent.Remark = templet.Remark;
                    ent.SubType = templet.SubType;
                    ent.MaxSetPrice = templet.MaxSetPrice;
                    ent.LimitType = templet.LimitType;
                    ent.LimitTypeKey = templet.LimitTypeKey;
                    ent.DataGroupID = (int)templet.DataGroupID;
                    ent.IDOSPackageMapping = JsonHelper.Serialize(templet.IDOSPackageMapping);
                    if (templet.TempletID != null)//修改
                    {
                        ent.TempletID = (int)templet.TempletID;
                        var qyy = db.TM_Act_CommunicationTemplet.Where(p => p.Name == ent.Name && p.Type == "Coupon" && p.TempletID != ent.TempletID).ToList();
                        if (qyy.Count >= 1)
                        {
                            return new Result(false, "模板名称与其他模板重名");
                        }
                        var query = db.TM_Act_CommunicationTemplet.Where(p => p.TempletID == ent.TempletID && p.DataGroupID == ent.DataGroupID).FirstOrDefault();
                        if (query != null)
                        {
                            query.Name = ent.Name;
                            query.Topic = ent.Topic;
                            query.Category = ent.Category;
                            query.Enable = ent.Enable;
                            query.BasicContent = ent.BasicContent;
                            query.InternalSetPrice = ent.InternalSetPrice;
                            query.SalePrice = ent.SalePrice;
                            query.IDOSPackageMapping = ent.IDOSPackageMapping;
                            query.Proportion = ent.Proportion;
                            query.ReferenceNo = ent.ReferenceNo;
                            query.Remark = ent.Remark;
                            query.SubType = ent.SubType;
                            query.MaxSetPrice = ent.MaxSetPrice;
                            query.LimitType = ent.LimitType;
                            query.LimitTypeKey = ent.LimitTypeKey;
                            query.ModifiedDate = DateTime.Now;
                            query.ModifiedUser = templet.ModifiedUser;

                            query.IsSync = "0";
                            query.RunTimes = 0;

                            var entry = db.Entry(query);
                            entry.State = EntityState.Modified;
                            db.SaveChanges();
                            //修改限制
                            if (couponLimit != "null")
                            {
                                var queryCouponLimit = db.TM_CRM_CouponLimit.Where(p => p.TempletID == query.TempletID).ToList();
                                if (queryCouponLimit.Count > 0)
                                {
                                    for (int i = 0; i < queryCouponLimit.Count; i++)
                                    {
                                        db.TM_CRM_CouponLimit.Remove(queryCouponLimit[i]);
                                        db.SaveChanges();
                                    }
                                }
                                for (int k = 0; k < limit.Count; k++)
                                {
                                    TM_CRM_CouponLimit tempObject = new TM_CRM_CouponLimit();
                                    tempObject.TempletID = query.TempletID;
                                    tempObject.LimitType = limit[k].LimitType;
                                    tempObject.LimitValue = limit[k].LimitValue;
                                    db.TM_CRM_CouponLimit.Add(tempObject);
                                    db.SaveChanges();
                                }
                            }

                            return new Result(true, "修改成功", query.TempletID);
                        }
                        return new Result(false, "没有权限修改此模板");
                    }
                    var q = db.TM_Act_CommunicationTemplet.Where(p => p.Name == ent.Name && p.Type == "Coupon").FirstOrDefault();
                    if (q != null)
                    {
                        return new Result(false, "此模板已存在");
                    }
                    ent.Type = "Coupon";
                    ent.AddedUser = templet.AddedUser;
                    ent.AddedDate = DateTime.Now;
                    ent.ModifiedDate = DateTime.Now;
                    ent.ModifiedUser = templet.ModifiedUser;
                    ent.Status = 0;
                    ent.IsSync = "0";
                    ent.RunTimes = 0;
                    db.TM_Act_CommunicationTemplet.Add(ent);
                    db.SaveChanges();
                    //添加限制
                    var qy = db.TM_Act_CommunicationTemplet.Where(p => p.Name == ent.Name && p.Type == "Coupon").FirstOrDefault();
                    if (!string.IsNullOrEmpty(couponLimit))
                    {
                        for (int index = 0; index < limit.Count; index++)
                        {
                            TM_CRM_CouponLimit tempObject = new TM_CRM_CouponLimit();
                            tempObject.TempletID = qy.TempletID;
                            tempObject.LimitType = limit[index].LimitType;
                            tempObject.LimitValue = limit[index].LimitValue;
                            db.TM_CRM_CouponLimit.Add(tempObject);
                            db.SaveChanges();
                        }
                    }
                    return new Result(true, "添加成功", qy.TempletID);
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        //<summary>
        //获取优惠券限制
        //</summary>
        //<param name="couponId"></param>
        //<returns></returns>
        public static Result GetTmpCouponLimit(int templetID)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_CRM_CouponLimit.Where(p => p.TempletID == templetID);
                return new Result(true, "", query.ToList());
            }
        }


        /// <summary>
        /// 获取条目列表
        /// </summary>
        /// <returns></returns>
        //public static Result GetItemList(int dataGroupId, string optType)
        //{
        //    throw new NotImplementedException();
        //using (CRMEntities db = new CRMEntities())
        //{
        //    var query = from a in db.V_M_TM_SYS_BaseData_item
        //                join c in db.V_Sys_DataGroupRelation on a.DataGroupID equals c.SubDataGroupID into cb
        //                from ccg in cb.DefaultIfEmpty()
        //                where a.ItemEnable == true && ccg.DataGroupID == dataGroupId
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
        //                };
        //    if (!string.IsNullOrEmpty(optType)) query = query.Where(p => p.ItemType == optType);
        //    return new Result(true, "", query.ToList());
        //}
        //}
    }
}
