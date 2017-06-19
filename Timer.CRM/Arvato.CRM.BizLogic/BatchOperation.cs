using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.SqlClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.BizLogic
{
   public static class BatchOperation
    {
       #region 获取批量操作列表
       public static Result LoadOperationList(BatchOperationModel model, string dp)
       {
           var myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
           try
           {
               using (var db = new CRMEntities())
               {
                   var userLst = db.TM_AUTH_User.Select(v => new { UserID = v.UserID, UserName = v.UserName }).ToList();
                   var userDic = new Dictionary<string, string>();
                   userLst.ForEach(v => userDic[v.UserID.ToString()] = v.UserName);
                   var query = db.TM_Card_BatchOperation.Where(p=>1==1);
                   if (!string.IsNullOrWhiteSpace(model.Status)) query = query.Where(i => i.Status == model.Status);
                   if (model.OddNumber != null)
                   {
                       string oddNumer = model.OddNumber.ToString();
                       query = query.Where(i => SqlFunctions.StringConvert((decimal)i.OddNumber, 14).Contains(oddNumer));
                   };
                   if (!string.IsNullOrWhiteSpace(model.OperationType)) query = query.Where(i => i.OperationType == model.OperationType);
                   if (model.ModifyTimeBegin != null) query = query.Where(i =>i.ModifiedDate>=model.ModifyTimeBegin);
                   if (model.ModifyTimeEnd != null) query = query.Where(i => i.ModifiedDate <= model.ModifyTimeEnd);
                  var  res= query.ToList().Select(i => new
                       {
                           i.OperationID,
                           i.OddNumber,
                           i.OperationType,
                           i.Remark,
                           i.ModifiedDate,
                           ModifiedUser = userDic[i.ModifiedUser],
                           i.Status,
                           i.StoreCode,
                           i.StoreName,
                           i.CompanyCode,
                           i.CompanyName,
                           i.AddedDate,
                           i.AddedUser

                       });
                   if (!string.IsNullOrWhiteSpace(model.ModifiedUser))
	                {
                        res = res.Where(p => p.ModifiedUser.Contains(model.ModifiedUser));
	                }
                   
                  return new Result(true, "", new List<object> { res.ToList().ToDataTableSourceVsPage(myDp) });
               }
           }
           catch
           {
               return new Result(false, "", new List<object> { new List<CouponSendRuleModel>().ToDataTableSourceVsPage(myDp) });
           }
       }
       #endregion

       #region 获取筛选条件
       public static Result LoadWhere(string Array, string dp)
       {
           var myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
           List<OperationWhere> where = JsonHelper.Deserialize<List<OperationWhere>>(Array);
           List<OperationWhere> newWhere = new List<OperationWhere>();
           for (int i = 0; i < where.Count; i++)
           {
               OperationWhere ow = new OperationWhere();
               ow.CardTypeName = where[i].CardTypeName;
               ow.CardType = where[i].CardType;
               ow.sort = i;
               ow.beginCard = where[i].beginCard;
               ow.endCard = where[i].endCard;
               ow.CardStatus = where[i].CardStatus;
               if (!string.IsNullOrWhiteSpace(where[i].CompanyCode))
               {
                   ow.CompanyCode = where[i].CompanyCode;
                   ow.CompanyName = where[i].CompanyName;
               }
               if (!string.IsNullOrWhiteSpace(where[i].StoreCode))
               {
                   ow.StoreCode = where[i].StoreCode;
                   ow.StoreName = where[i].StoreName;
               }
               
              
               newWhere.Add(ow);
           }

           return new Result(true, "", new List<object> { newWhere.ToDataTableSourceVsPage(myDp) });
            
       }

       public static Result GetWhereById(string Id, string dp)
       {
           var myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
           using (var db = new CRMEntities())
           {
               var wheres = db.TM_Card_OperationCondition.Where(p => p.OperationID == Id).ToList();

               List<OperationWhere> newWhere = new List<OperationWhere>();
               for (int i = 0; i < wheres.Count; i++)
               {
                   OperationWhere ow = new OperationWhere();
                   ow.sort = i;
                   ow.beginCard = wheres[i].BeginCard;
                   ow.endCard = wheres[i].EndCard;
                   ow.CardStatus = wheres[i].CardStatu;
                   if (!string.IsNullOrWhiteSpace(wheres[i].CardStore))
                   {
                       var store = (from a in db.V_M_TM_SYS_BaseData_store
                                    join b in db.V_M_TM_SYS_BaseData_company
                                    on a.ChannelCodeStore equals b.CompanyCode
                                    where a.StoreCode == wheres[i].CardStore
                                    select new
                                    {
                                        StoreCode = a.StoreCode,
                                        StoreName = a.StoreName,
                                        CompanyCode = b.CompanyCode,
                                        CompanyName = b.CompanyName
                                    }).FirstOrDefault();
                       ow.CompanyCode = store.CompanyCode;
                       ow.CompanyName = store.CompanyName;
                       ow.StoreCode = store.StoreCode;
                       ow.StoreName = store.StoreName;
                   }


                   newWhere.Add(ow);
               }

               return new Result(true, "", new List<object> { newWhere.ToDataTableSourceVsPage(myDp) });
           }
       }
       #endregion

       #region 加载卡列表
       public static Result LoadCardList(string Array, string OperationType, string dp)
       {
           var myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
           List<OperationWhere> where = JsonHelper.Deserialize<List<OperationWhere>>(Array);
           try
           {
               using (var db=new CRMEntities())
               {
                     string sql = @"select t.CardNo,
                           t.Status,
                            t.StoreCode,
                            t.IsSalesStatus,
                            t.IsUsed,
                            t.CardType,
                            CardTypeName=b.CardTypeNameBase
                            from [dbo].[Tm_Card_CardNo] as t inner join V_M_TM_SYS_BaseData_cardType as b 
                            on t.CardType=b.CardTypeCodeBase
                              ";
                     for (int i = 0; i < where.Count; i++)
                     {
                         var str = " or ";
                         if (i==0)
                         {
                             str = " where ";
                         }
                         sql += str + " ( 1=1   ";
                         //if (OperationType=="0")
                         //{
                         //    sql += " and  t.Status= '0' and  t.Status != -3 ";
                         //}
                         //if (OperationType == "2")
                         //{
                         //    sql += " and  t.Status != -3 ";
                         //}
                         //if (OperationType == "3")
                         //{
                         //    sql += " and  t.Status= '-2' and  t.Status != -3 ";
                         //}
                         if (!string.IsNullOrWhiteSpace(where[i].CardType))
                         {
                             sql += " and  t.CardType= '" + where[i].CardType + "'";
                         }
                         if (where[i].beginCard!=null)
                         {
                             sql += " and  t.CardNo>= '" + where[i].beginCard+"'";
                         }
                         if (where[i].endCard != null)
                         {
                             sql += " and  t.CardNo<='" + where[i].endCard+"' ";
                         }
                         if (!string.IsNullOrWhiteSpace(where[i].CardStatus))
                         {
                             sql += " and  t.Status= '" + where[i].CardStatus+"' ";
                         }
                         if (where[i].StoreCode != null)
                         {
                             sql += " and  t.StoreCode='" + where[i].StoreCode+"' ";
                         }
                         sql += " )  ";
                     }
                     var CardInfo = db.Database.SqlQuery<OperationCardModel>(sql, new SqlParameter("","1")).ToList();
                     return new Result(true, "", new List<object> { CardInfo.ToDataTableSourceVsPage(myDp) });

               }
           }
           catch (Exception)
           {
               
               return new Result(false, "", new List<object> { new List<TM_Card_CardNo>().ToDataTableSourceVsPage(myDp) });
           }
           //SqlParameter param = new SqlParameter("@BoxNo", boxNo);
           //string sql = "select t.BoxNo,PurposeId=(select k.OptionValue from [dbo].[TD_SYS_BizOption] as k where OptionType='CardPurpose' and k.OptionValue=t.BoxPurposeId ),Purpose=(select k.OptionText from [dbo].[TD_SYS_BizOption] as k where OptionType='CardPurpose' and k.OptionValue=t.BoxPurposeId  ),CardTypeName=(select z.CardTypeNameBase from [dbo].[V_M_TM_SYS_BaseData_cardType] as z left join [dbo].[TM_Card_ProduceDetail] as y on z.CardTypeCodeBase=y.CardTypeCode where y.BeginCardNo=t.BeginCardNo ),CardTypeCode=(select d.CardTypeCode from [dbo].[TM_Card_ProduceDetail] as d where d.BeginCardNo<=t.BeginCardNo),CardNumIn=t.CardNumIn,BeginCardNo=t.BeginCardNo,EndCardNo=t.EndCardNo,AcceptingUnit=t.AcceptingUnit from [dbo].[Tm_Card_CardBox] as t where t.boxno=@BoxNo";
         
       }

       public static Result LoadCardListById(string ID, string dp)
       {
           var myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
           try
           {
               using (var db = new CRMEntities())
               {
                   var query = (from a in db.TM_Card_OperationCard
                                join b in db.TM_Card_CardNo

                                on a.CardNo equals b.CardNo
                                join c in db.V_M_TM_SYS_BaseData_cardType
                                on a.CardType equals c.CardTypeCodeBase
                                select new
                                {
                                    CardTypeName=c.CardTypeNameBase,
                                    OperationID = a.OperationID,
                                    CardNo = a.CardNo,
                                    Statu = b.Status,
                                    IsSalesStatus = b.IsSalesStatus,
                                    IsUsed = b.IsUsed
                                }).Where(p => p.OperationID==ID);
                 //  var query = db.TM_Card_OperationCard.Where(p=>p.OperationID==ID);

                   return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
               }
           }
           catch
           {
               return new Result(false, "", new List<object> { new List<CouponSendRuleModel>().ToDataTableSourceVsPage(myDp) });
           }
       }
       #endregion

       #region 删除批量操作

       public static Result DeleteOperationById(string Id, int User)
       {
           try
           {

               using (var db = new CRMEntities())
               {
                   var dt = DateTime.Now;
                   var query = db.TM_Card_BatchOperation.Find(Id);
                   db.TM_Card_BatchOperation.Remove(query);
                   db.SaveChanges();
                   return new Result(true, "删除成功");
               }
           }
           catch
           {
               return new Result(false, "删除失败");
           }
       }

       #endregion

       #region 添加批量操作

       public static Result AddOperation(BatchOperationModel model, int User)
       {
           using (var db = new CRMEntities())
           {
               try
               {

                   db.BeginTransaction();
                   var dt = DateTime.Now;
                   long oddnumber = 0;
                   DateTime nowTime = DateTime.Today;
                   DateTime afterTime = nowTime.AddDays(+1);
                   var odd = (from n in db.TM_Card_BatchOperation where n.AddedDate >= nowTime && n.AddedDate < afterTime orderby n.OddNumber descending select n.OddNumber).ToList();
                   if (odd == null||odd.Count==0 )
                   {
                       oddnumber = GetOddNoByRole();
                   }
                   else
                   {
                       oddnumber = odd[0] + 1;
                   }
                   TM_Card_BatchOperation list = new TM_Card_BatchOperation();
                   list.OperationID = Guid.NewGuid().ToString("N");
                   list.AddedDate = DateTime.Now;
                   list.AddedUser = User.ToString();
                   list.ModifiedDate = DateTime.Now;
                   list.ModifiedUser = User.ToString();
                   list.OddNumber = oddnumber;
                   list.OperationType = model.OperationType;
                   list.Remark = model.Remark;
                   list.Status = "0";
                   if (model.OperationType == "1")
                   {
                       list.StoreCode = model.StoreCode;
                       list.StoreName = model.StoreName;
                       list.CompanyCode = model.CompanyCode;
                       list.CompanyName = model.CompanyName;
                   }
                   db.TM_Card_BatchOperation.Add(list);


                   List<OperationWhere> where = JsonHelper.Deserialize<List<OperationWhere>>(model.Where);
                   foreach (OperationWhere item in where)
                   {
                       TM_Card_OperationCondition oc = new TM_Card_OperationCondition();
                       oc.ConditionID = Guid.NewGuid().ToString("N");
                       oc.OperationID = list.OperationID;
                       oc.CardType = "1";
                       oc.CardStore = item.StoreCode;
                       oc.CardStatu = item.CardStatus;
                       oc.BeginCard = item.beginCard;
                       oc.EndCard = item.endCard;
                       db.TM_Card_OperationCondition.Add(oc);
                       
                   }

                   db.SaveChanges();
                   db.Commit();
                   return new Result(true, "添加成功");

               }
               catch
               {
                   db.Rollback();
                   return new Result(false, "添加失败");
               }
           }
       }
       #endregion

       #region 编辑批量操作

       public static Result UpdateOperation(BatchOperationModel model, int User)
       {
           using (var db = new CRMEntities())
           {
               try
               {

                   db.BeginTransaction();
                   var dt = DateTime.Now;
                   TM_Card_BatchOperation list = db.TM_Card_BatchOperation.Find(model.OperationID);
                   list.ModifiedDate = DateTime.Now;
                   list.ModifiedUser = User.ToString();
                   list.OperationType = model.OperationType;
                   list.Remark = model.Remark;
                   list.Status = "0";
                   if (model.OperationType == "1")
                   {
                       list.StoreCode = model.StoreCode;
                       list.StoreName = model.StoreName;
                       list.CompanyCode = model.CompanyCode;
                       list.CompanyName = model.CompanyName;
                   }

                   var Clist=db.TM_Card_OperationCondition.Where(p => p.OperationID == model.OperationID).ToList();
                   foreach (var item in Clist)
                   {
                       db.TM_Card_OperationCondition.Remove(item);
                   }

                   List<OperationWhere> where = JsonHelper.Deserialize<List<OperationWhere>>(model.Where);
                   foreach (OperationWhere item in where)
                   {
                       TM_Card_OperationCondition oc = new TM_Card_OperationCondition();
                       oc.ConditionID = Guid.NewGuid().ToString("N");
                       oc.OperationID = list.OperationID;
                       oc.CardType = "2";
                       oc.CardStore = item.StoreCode;
                       oc.CardStatu = item.CardStatus;
                       oc.BeginCard = item.beginCard;
                       oc.EndCard = item.endCard;
                       db.TM_Card_OperationCondition.Add(oc);

                   }
                   db.Configuration.ValidateOnSaveEnabled = false;
                   db.SaveChanges();
                   db.Configuration.ValidateOnSaveEnabled = true;
                   db.Commit();
                   return new Result(true, "编辑成功");


               }
               catch
               {

                   return new Result(false, "编辑失败");
               }
           }
       }


       #endregion

       #region 根据Id获取批量操作信息

       public static Result GetOperationById(string id)
       {
           try
           {
               using (var db = new CRMEntities())
               {
                   var query = db.TM_Card_BatchOperation.Find(id);
                   var info = new BatchOperationModel();
                   List<OperationWhere> wherelist = new List<OperationWhere>();
                   var cond = (from a in db.TM_Card_OperationCondition
                              join b in db.V_M_TM_SYS_BaseData_cardType
                              on a.CardType equals b.CardTypeCodeBase
                              where a.OperationID==id
                              select new {
                                  CardStore = a.CardStore,
                                  BeginCard = a.BeginCard,
                                  EndCard = a.EndCard,
                                  CardStatu = a.CardStatu,
                                  CardType=a.CardType,
                                  CardTypeName=b.CardTypeNameBase,                           
                              }).ToList();
                       
                       //db.TM_Card_OperationCondition.Where(p=>p.OperationID==id).ToList();
                   foreach (var item in cond)
                   {
                       var ow = new OperationWhere();
                       if (!string.IsNullOrWhiteSpace(item.CardStore))
                       {
                           var store = (from a in db.V_M_TM_SYS_BaseData_store
                                        join b in db.V_M_TM_SYS_BaseData_channel
                                        on a.ChannelCodeStore equals b.ChannelCodeBase
                                        where a.StoreCode == item.CardStore
                                        select new
                                        {
                                            StoreCode = a.StoreCode,
                                            StoreName = a.StoreName,
                                            CompanyCode = b.ChannelCodeBase,
                                            CompanyName = b.ChannelNameBase
                                        }).FirstOrDefault();
                           ow.StoreCode = item.CardStore;
                           ow.StoreName = store.StoreName;
                           ow.CompanyCode = store.CompanyCode;
                           ow.CompanyName = store.CompanyName;
                       }
                       ow.CardType = item.CardType;
                       ow.CardTypeName = item.CardTypeName;
                       ow.beginCard = item.BeginCard;
                       ow.endCard = item.EndCard;
                       
                       ow.CardStatus = item.CardStatu;
                       wherelist.Add(ow);
                       
                   }
                   var CreateUser = Convert.ToInt32(query.AddedUser);
                   var UpdateUser = Convert.ToInt32(query.ModifiedUser);
                   info.OperationID = query.OperationID;
                   info.OddNumber = query.OddNumber;
                   info.ModifiedDate = query.ModifiedDate;
                   info.ModifiedUser = db.TM_AUTH_User.Where(p=>p.UserID==UpdateUser).Select(p=>p.UserName).FirstOrDefault();
                   info.OperationType = query.OperationType;
                   info.OperationWhere = wherelist;
                   info.Remark = query.Remark;
                   info.Status = query.Status;
                   info.AddedDate = query.AddedDate;
                   info.AddedUser = db.TM_AUTH_User.Where(p => p.UserID == CreateUser).Select(p => p.UserName).FirstOrDefault();
                   info.CompanyCode = query.CompanyCode;
                   info.CompanyName = query.CompanyName;
                   info.StoreCode = query.StoreCode;
                   info.StoreName = query.StoreName;
                   return new Result(true, "", info);
               }
           }
           catch
           {
               return new Result(false);
           }
       }
       #endregion

       #region 改变批量操作的审核状态
       public static Result ApproveOperationById(string Id, string active, int User)
       {
           try
           {
               using (var db = new CRMEntities())
               {
                   var dt = DateTime.Now;
                   var query = db.TM_Card_BatchOperation.Find(Id);
                   query.Status = active;
                   query.ModifiedDate = dt;
                   query.ModifiedUser = User.ToString();
                   var wherelist=db.TM_Card_OperationCondition.Where(p=>p.OperationID==Id).ToList();
                   string sql = @"select *
                            from [dbo].[Tm_Card_CardNo] as t
                              ";
                   for (int i = 0; i < wherelist.Count; i++)
                   {
                       var str = " or ";
                       if (i == 0)
                       {
                           str = " where ";
                       }
                       sql += str + " ( 1=1 ";
                      
                       if (wherelist[i].BeginCard != null)
                       {
                           sql += " and  t.CardNo>= '" + wherelist[i].BeginCard + "'";
                       }
                       if (wherelist[i].EndCard != null)
                       {
                           sql += " and  t.CardNo<='" + wherelist[i].EndCard + "' ";
                       }
                       if (!string.IsNullOrWhiteSpace(wherelist[i].CardStatu))
                       {
                           sql += " and  t.Status= '" + wherelist[i].CardStatu + "' ";
                       }
                       if (wherelist[i].CardStore != null)
                       {
                           sql += " and  t.StoreCode='" + wherelist[i].CardStore + "' ";
                       }
                       sql += " ) ";
                   }
                   var CardInfo = db.Database.SqlQuery<TM_Card_CardNo>(sql, new SqlParameter("", "1")).ToList();
                   if (CardInfo.Count > 1000)
                   {
                       return new Result(false, "审核失败：批量操作筛选的卡不能超过1000张");
                   }
                   var cou = 0;
                   foreach (var item in CardInfo)
                   {
                       //if (query.OperationType == "0")
                       //{
                       //    sql += " and  t.Status= '1' and  t.Status != -3 ";
                       //}
                       //if (query.OperationType == "2")
                       //{
                       //    sql += " and  t.Status != -3 ";
                       //}
                       //if (query.OperationType == "3")
                       //{
                       //    sql += " and  t.Status= '-2' and  t.Status != -3 ";
                       //}
                       var changeCard=db.TM_Card_CardNo.Where(p=>p.CardNo==item.CardNo).FirstOrDefault();
                       
                       //卡激活
                       if (query.OperationType=="0")
                       {
                           if (changeCard.Status == "1" || changeCard.Status == "0")
                           {
                               changeCard.Status = "2";
                               TM_Card_OperationCard card = new TM_Card_OperationCard();
                               card.OperationCardID = Guid.NewGuid().ToString("N");
                               card.OperationID = query.OperationID;
                               card.CardNo = item.CardNo;
                               card.CardType = item.CardType;
                               db.TM_Card_OperationCard.Add(card);
                               cou++;
                           }

                       }
                       //卡转移
                       else if(query.OperationType=="1")
                       {
                           if (changeCard.Status != "-3")
                           {
                               changeCard.StoreCode = query.StoreCode;
                               TM_Card_OperationCard card = new TM_Card_OperationCard();
                               card.OperationCardID = Guid.NewGuid().ToString("N");
                               card.OperationID = query.OperationID;
                               card.CardNo = item.CardNo;
                               card.CardType = item.CardType;
                               db.TM_Card_OperationCard.Add(card);
                               cou++;
                           }
                          
                       }
                       //卡冻结
                       else if (query.OperationType == "2")
                       {
                           if (changeCard.Status == "2")
                           {
                               changeCard.Status = "-2";
                               TM_Card_OperationCard card = new TM_Card_OperationCard();
                               card.OperationCardID = Guid.NewGuid().ToString("N");
                               card.OperationID = query.OperationID;
                               card.CardNo = item.CardNo;
                               card.CardType = item.CardType;
                               db.TM_Card_OperationCard.Add(card);
                               cou++;
                           }
                       }
                       //卡解冻
                       else if (query.OperationType == "3")
                       {
                           if (changeCard.Status == "-2")
                           {
                               changeCard.Status = "2";
                               TM_Card_OperationCard card = new TM_Card_OperationCard();
                               card.OperationCardID = Guid.NewGuid().ToString("N");
                               card.OperationID = query.OperationID;
                               card.CardNo = item.CardNo;
                               card.CardType = item.CardType;
                               db.TM_Card_OperationCard.Add(card);
                               cou++;
                           }
                       }
                       //卡作废
                       else if (query.OperationType == "4")
                       {
                           cou++;
                               changeCard.Status = "-3";
                               TM_Card_OperationCard card = new TM_Card_OperationCard();
                               card.OperationCardID = Guid.NewGuid().ToString("N");
                               card.OperationID = query.OperationID;
                               card.CardNo = item.CardNo;
                               card.CardType = item.CardType;
                               db.TM_Card_OperationCard.Add(card);

                       }
                   }
                   if (CardInfo.Count==0)
                   {
                       return new Result(false, "审核失败：没有符合筛选条件的卡");
                   }
                  
                   if (cou == 0)
                   {
                       var kst = "";
                       if (query.OperationType=="0")
                           kst = "为【已核对】或【已发卡】";
                       else if(query.OperationType == "1")
                            kst = "不为【作废】";
                       else if (query.OperationType == "2")
                           kst = "为【使用中】";
                       else if (query.OperationType == "3")
                           kst = "【冻结】";
                       return new Result(false, "审核失败：没有符合筛选条件的卡,请确认筛选的卡号状态"+kst);
                   }
                   db.SaveChanges();

                   var msg = "审核成功，批量操作" + cou + "张卡号成功，\n"+(CardInfo.Count-cou)+"张卡号失败";
                   if (active == "2")
                       msg = "撤销成功";
                   return new Result(true, msg);
               }
           }
           catch
           {
               return new Result(false, "操作失败：数据不存在");
           }
       }

       #endregion

       #region 获取卡类型列表
       public static Result GetCardType()
       {
           using (CRMEntities db = new CRMEntities())
           {


               var query = db.V_M_TM_SYS_BaseData_cardType.Select(p => new { p.CardTypeCodeBase,p.CardTypeNameBase });

               return new Result(true, "", new List<object> { query.ToList() });
           }
       }
       #endregion

       #region 单号生成规则
       public static long GetOddNoByRole()
       {
           string date = DateTime.Now.ToString("yyyy-MM-dd");
           date = date.Remove(4, 1).Remove(6, 1);
           long result = Convert.ToInt64(string.Format("{0}{1}", date, "000001"));
           return result;
       }

       #endregion

        #region
       //public Result ImportExcelBatch(string strList)
       //{
       //    try
       //    {
       //        List<string> cardInfo = JsonHelper.Deserialize<List<string>>(strList);
       //        string mappingGuid=ToolsHelper.GuidtoString(Guid.NewGuid());
       //        string storeCode = "";
       //        string storeName = "";
       //        using (CRMEntities db = new CRMEntities())
       //        {
       //            foreach (var item in cardInfo)
       //            {
       //                string[] strArray = item.Split(',');
       //                storeCode = strArray[1];
       //                storeName = strArray[2];
       //                string guid = ToolsHelper.GuidtoString(Guid.NewGuid());
       //                TM_Card_OperationCard newCard = new TM_Card_OperationCard()
       //                {
       //                    OperationCardID = guid,
       //                    OperationID = mappingGuid,
       //                    CardNo = strArray[0],
       //                    CardType = "1"
       //                };
       //            }
       //        }
       //        return new Result(true);
       //    }
       //    catch (Exception ex)
       //    {
       //        Log4netHelper.WriteErrorLog("ImportExcelBatch :" + ex.ToString());
       //        throw;
       //    }


       //}
        #endregion

    }
}
