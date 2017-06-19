using Arvato.CRM.EF;
using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Trigger
{
    public class AccountTrigger : BaseTrigger
    {
        private ExtraAccount Ext;

        public AccountTrigger(ExtraAccount ext)
        {
            TriggerType = Trigger.TriggerType.Account;
            Ext = ext;
            ExtraData = Utility.JsonHelper.Serialize(ext);
        }

        public override void Start()
        {
            if (MemberIDs == null)
            {
                MemberIDs = Ext.db.Database.SqlQuery<TM_Mem_Master>(MemberScript).Select(o => o.MemberID).ToList();
            }
            using (CRMEntities db = Ext.db ?? new CRMEntities())
            {
                #region 可以新建账户
                for (int i = 0; i < Ext.AccountIDs.Count; i++)
                {
                    var actId = Ext.AccountIDs[i];
                    db.BeginTransaction();
                    if (!string.IsNullOrEmpty(actId))
                    {
                        //还要更改帐户总额
                        //To Do
                        var q1 = db.TM_Mem_Account.Where(p => p.AccountID == actId).FirstOrDefault();

                        if (Ext.AccountDetailType == "value1")
                        {
                            q1.Value1 = q1.Value1 + Ext.ChangeValue;

                            var qq1 = db.TM_Mem_AccountDetail.Where(p => p.AccountID == actId && p.AccountDetailType == Ext.AccountDetailType).FirstOrDefault();
                            if (qq1 != null)
                            {
                                qq1.DetailValue = qq1.DetailValue + Ext.ChangeValue;
                                var entryq1 = db.Entry(qq1);
                                entryq1.State = EntityState.Modified;
                            }
                            else
                            {
                                TM_Mem_AccountDetail ent = new TM_Mem_AccountDetail();
                                ent.AccountID = actId;
                                ent.AccountDetailType = Ext.AccountDetailType;
                                ent.DetailValue = Ext.ChangeValue;
                                //ent.SpecialDate1 = DateTime.Now;
                                //ent.SpecialDate2 = DateTime.Now;

                                ent.AddedDate = DateTime.Now;
                                ent.AddedUser = Ext.AuthID.ToString();
                                ent.ModifiedDate = DateTime.Now;
                                ent.ModifiedUser = Ext.AuthID.ToString();

                                db.TM_Mem_AccountDetail.Add(ent);
                            }
                        }
                        if (Ext.AccountDetailType == "value2")
                        {
                            q1.Value2 = q1.Value2 + Ext.ChangeValue;

                            TM_Mem_AccountDetail ent = new TM_Mem_AccountDetail();
                            ent.AccountID = actId;
                            ent.AccountDetailType = Ext.AccountDetailType;
                            ent.DetailValue = Ext.ChangeValue;
                            //ent.SpecialDate1 = DateTime.Now;
                            //ent.SpecialDate2 = DateTime.Now;

                            ent.AddedDate = DateTime.Now;
                            ent.AddedUser = Ext.AuthID.ToString();
                            ent.ModifiedDate = DateTime.Now;
                            ent.ModifiedUser = Ext.AuthID.ToString();

                            db.TM_Mem_AccountDetail.Add(ent);
                        }
                        var entry1 = db.Entry(q1);
                        entry1.State = EntityState.Modified;


                    }
                    else
                    {
                        TM_Mem_Account act = new TM_Mem_Account();
                        act.AccountID = Guid.NewGuid().ToString("N");
                        actId = act.AccountID;
                        act.MemberID = MemberIDs[0];
                        act.AccountType = Ext.AccountType;
                        if (Ext.AccountDetailType == "value1")
                        {
                            act.Value1 = Ext.ChangeValue;
                            act.Value2 = 0;
                        }
                        if (Ext.AccountDetailType == "value2")
                        {
                            act.Value1 = 0;
                            act.Value2 = Ext.ChangeValue;
                        }
                        act.Value3 = 0;

                        act.AddedDate = DateTime.Now;
                        act.AddedUser = Ext.AuthID.ToString();
                        act.ModifiedDate = DateTime.Now;
                        act.ModifiedUser = Ext.AuthID.ToString();

                        db.TM_Mem_Account.Add(act);
                        //db.SaveChanges();
                        //db.Configuration.ValidateOnSaveEnabled = false;
                        //int count = db.SaveChanges();
                        //db.Configuration.ValidateOnSaveEnabled = true;
                        //actId = db.TM_Mem_Account.Where(p => p.MemberID == mid).FirstOrDefault().AccountID;

                        //
                        TM_Mem_AccountDetail ent = new TM_Mem_AccountDetail();
                        ent.AccountID = actId;
                        ent.AccountDetailType = Ext.AccountDetailType;
                        ent.DetailValue = Ext.ChangeValue;
                        // ent.SpecialDate1 = DateTime.Now;
                        // ent.SpecialDate2 = DateTime.Now;

                        ent.AddedDate = DateTime.Now;
                        ent.AddedUser = Ext.AuthID.ToString();
                        ent.ModifiedDate = DateTime.Now;
                        ent.ModifiedUser = Ext.AuthID.ToString();


                        db.TM_Mem_AccountDetail.Add(ent);
                    }

                    //还要更改帐户使用限制
                    //To Do


                    db.SaveChanges();
                    db.Commit();
                }
            }

                #endregion

            #region 不能新建账户
            //for (int i = 0; i < Ext.AccountDetailIDs.Count; i++)
            //{
            //    db.BeginTransaction();

            //    var did = (long)Convert.ToInt32(Ext.AccountDetailIDs[i]);
            //    var query = db.TM_Mem_AccountDetail.Where(p => p.AccountDetailID == did).FirstOrDefault();
            //    var actID = query.AccountID;
            //    if (query != null)
            //    {

            //        if (Ext.Operation == "add")
            //        {
            //            query.DetailValue = query.DetailValue + Ext.ChangeValue;
            //        }
            //        if (Ext.Operation == "sub")
            //        {
            //            query.DetailValue = query.DetailValue - Ext.ChangeValue;

            //        }
            //        if (query.DetailValue <= 0)
            //        {
            //            db.TM_Mem_AccountDetail.Remove(query);
            //            //db.SaveChanges();
            //            //return new Result(true, "此帐户金额减至0，自动删除");
            //        }
            //        else
            //        {
            //            query.AccountDetailType = Ext.AccountDetailType;
            //            query.SpecialDate1 = DateTime.Now;
            //            query.SpecialDate2 = DateTime.Now;
            //            query.ModifiedDate = DateTime.Now;
            //            query.ModifiedUser = Ext.AuthID.ToString();

            //            var entry = db.Entry(query);
            //            entry.State = EntityState.Modified;
            //        }
            //        //db.SaveChanges();

            //        //还要更改帐户总额
            //        //To Do
            //        var q1 = db.TM_Mem_Account.Where(p => p.AccountID == actID).FirstOrDefault();

            //        if (query.AccountDetailType == "value1" && Ext.Operation == "add")
            //        {
            //            q1.Value1 = q1.Value1 + Ext.ChangeValue;
            //        }
            //        if (query.AccountDetailType == "value1" && Ext.Operation == "sub")
            //        {
            //            q1.Value1 = (q1.Value1 - Ext.ChangeValue) < 0 ? 0 : (q1.Value1 - Ext.ChangeValue);
            //        }
            //        if (query.AccountDetailType == "value2" && Ext.Operation == "add")
            //        {
            //            q1.Value2 = q1.Value2 + Ext.ChangeValue;
            //        }
            //        if (query.AccountDetailType == "value2" && Ext.Operation == "sub")
            //        {
            //            q1.Value2 = (q1.Value2 - Ext.ChangeValue) < 0 ? 0 : (q1.Value2 - Ext.ChangeValue);
            //        }
            //        var entry1 = db.Entry(q1);
            //        entry1.State = EntityState.Modified;
            //        //db.SaveChanges();

            //        //还要记录帐户更改
            //        //To Do
            //        TL_Mem_AccountChange ent = new TL_Mem_AccountChange();
            //        ent.AccountID = actID;
            //        ent.AccountDetailID = did;
            //        ent.AccountChangeType = "manu";
            //        ent.ChangeValue = Ext.ChangeValue;
            //        ent.ChangeReason = "";
            //        ent.AddedDate = DateTime.Now;
            //        ent.AddedUser = Ext.AuthID;
            //        db.TL_Mem_AccountChange.Add(ent);
            //        // db.SaveChanges();
            //        db.SaveChanges();
            //        db.Commit();
            //        Callback(new Result(true, "调整成功"));
            //    }

            //    //for (int j = 0; j < MemberIDs.Count; j++)
            //    //{
            //    //    UpdateActDetail(db, j);
            //    //}

            //    //Log
            //    base.Log(db);
            //}
            #endregion


            //根据执行情况判断调用回调函数
            //To Do
        }
        //更新账户明细
        private void UpdateActDetail(CRMEntities db, int j)
        {
            //账户调整代码
            var query1 = db.TM_Mem_Account.Where(p => p.MemberID == MemberIDs[j] && p.AccountType == Ext.AccountType).FirstOrDefault();
            //会员某个类型的账户明细
            var query2 = db.TM_Mem_AccountDetail.Where(p => p.AccountID == query1.AccountID);

            //调整一个账户增加的时候判断限制，判断时间，如果没有就要新增
            if (Ext.Operation == "add")
            {
                if (Ext.AccountDetailType == "value1")//如果是可用类型的话，直接操作
                {
                    var query3 = query2.ToList();
                    for (int i = 0; i < query3.Count; i++)
                    {
                        if (query3[i].AccountDetailType == Ext.AccountDetailType)
                        {
                            query3[i].DetailValue = query3[i].DetailValue + Ext.ChangeValue;
                            var entry = db.Entry(query3[i]);
                            entry.State = EntityState.Modified;
                            db.SaveChanges();

                            //更改账户总额
                            UpdateActSum(db, query1.AccountID, Ext.Operation, Ext.AccountDetailType);
                            Callback(new Result(true, "修改成功"));
                        }
                    }
                }
                if (Ext.AccountDetailType == "value2")//如果是冻结类型的话，找出时间最长远的
                {
                    var query4 = query2.Where(p => p.AccountDetailType == "value2").OrderByDescending(p => p.SpecialDate1).ToList();
                    query4[0].DetailValue = query4[0].DetailValue + Ext.ChangeValue;
                    var entry = db.Entry(query4[0]);
                    entry.State = EntityState.Modified;
                    db.SaveChanges();

                    //更改账户总额
                    UpdateActSum(db, query1.AccountID, Ext.Operation, Ext.AccountDetailType);
                    Callback(new Result(true, "修改成功"));
                }
            }
            if (Ext.Operation == "sub")
            {
                if (Ext.AccountDetailType == "value1")//如果是可用类型的话，直接操作
                {
                    var query3 = query2.ToList();
                    for (int i = 0; i < query3.Count; i++)
                    {
                        if (query3[i].AccountDetailType == Ext.AccountDetailType)
                        {
                            query3[i].DetailValue = query3[i].DetailValue - Ext.ChangeValue;
                            var entry = db.Entry(query3[i]);
                            entry.State = EntityState.Modified;
                            db.SaveChanges();

                            //更改账户总额
                            UpdateActSum(db, query1.AccountID, Ext.Operation, Ext.AccountDetailType);

                            Callback(new Result(true, "修改成功"));
                        }
                    }
                }
                //减少的时候判断时间，从快过期的账户减，然后多出来的话，从第二个快过期的减。
                if (Ext.AccountDetailType == "value2")//如果是冻结类型的话，找出时间最长远的
                {
                    SubActOperation(Ext.ChangeValue, db, query2);
                    //更改账户总额
                    UpdateActSum(db, query1.AccountID, Ext.Operation, Ext.AccountDetailType);
                }
            }
        }
        //更新账户明细的同时还需要更新账户总额
        private void UpdateActSum(CRMEntities db, string accountId, string oper, string type)
        {
            var query = db.TM_Mem_Account.Where(p => p.AccountID == accountId).FirstOrDefault();
            if (query != null)
            {
                if (type == "value1" && oper == "add")
                {
                    query.Value1 = query.Value1 + Ext.ChangeValue;
                }
                if (type == "value1" && oper == "sub")
                {
                    query.Value1 = query.Value1 - Ext.ChangeValue;
                }
                if (type == "value2" && oper == "add")
                {
                    query.Value2 = query.Value2 + Ext.ChangeValue;
                }
                if (type == "value2" && oper == "sub")
                {
                    query.Value2 = query.Value2 - Ext.ChangeValue;
                }
                var entry = db.Entry(query);
                entry.State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        //递归减去积分现金积点
        private void SubActOperation(decimal changeValue, CRMEntities db, IQueryable<TM_Mem_AccountDetail> query2)
        {
            var query4 = query2.Where(p => p.AccountDetailType == "value2").OrderBy(p => p.SpecialDate1).ToList();
            for (int i = 0; i < query4.Count; i++)
            {
                if ((query4[i].DetailValue - changeValue) > 0)
                {
                    query4[i].DetailValue = query4[i].DetailValue - changeValue;
                    var entry = db.Entry(query4[i]);
                    entry.State = EntityState.Modified;
                    db.SaveChanges();
                    Callback(new Result(true, "修改成功"));
                    break;
                }
                else //if ((query4[i].DetailValue - changeValue) == 0)
                {
                    changeValue = changeValue - query4[i].DetailValue;
                    db.TM_Mem_AccountDetail.Remove(query4[i]);
                    db.SaveChanges();
                    SubActOperation(changeValue, db, query2);
                    Callback(new Result(true, "修改成功"));

                }
                break;
            }
        }
    }



    /// <summary>
    /// 账户触发器扩展结构体
    /// </summary>
    public struct ExtraAccount
    {
        public CRMEntities db { get; set; }
        public List<string> AccountIDs { get; set; }//账户编号，若存在则忽略会员编号
        public string AccountType { get; set; }//账户类型
        public List<string> AccountDetailIDs { get; set; }//账户明细编号
        public string AccountDetailType { get; set; }//账户明细类型
        public decimal ChangeValue { get; set; }//变动值
        public string Operation { get; set; }//操作
        public string AuthID { get; set; }//账户名
        public string Remark { get; set; }//调整备注
        public DateTime SpecialDate1 { get; set; }
        public DateTime SpecialDate2 { get; set; }
    }
}
