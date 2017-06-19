using Arvato.CRM.EF;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arvato.CRM.Model;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace Arvato.CRM.BizLogic
{
   public static class PurchasesNew
   {

       #region 定卡
       /// <summary>
       /// 定卡列表
       /// </summary>
       /// <param name="oddId"></param>
       /// <param name="status"></param>
       /// <param name="agent"></param>
       /// <param name="modifyBy"></param>
       /// <param name="destineNumber"></param>
       /// <param name="page"></param>
       /// <returns></returns>
       public static Result CustomizeCardList(string oddIdNo, string status, string agent, string createTime, string boxNumIn, string cardNumIn, string isRetrieve,string page)
       {
           DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);
           using (CRMEntities db = new CRMEntities())
           {          
               var query = db.TM_Card_ProduceNew.OrderByDescending(m => m.CreateTime)
                   .Select(n => new
                   {
                       OddId = n.CustomizeOddId,
                       Status = n.Status,
                       StatusName = (from s in db.TD_SYS_BizOption where s.OptionValue == n.Status && s.OptionType == "ApproveStatus" select s.OptionText).FirstOrDefault(),
                       AgentCode = n.Agent,
                       Agent = (from k in db.V_M_TM_SYS_BaseData_supplier where n.Agent == k.SupplierCode select k.SupplierName).FirstOrDefault(),
                       CreateBy = (from m in db.TM_AUTH_User where m.UserID == n.CreateBy select m.UserName).FirstOrDefault(),
                       CreateTime = n.CreateTime,
                       OddIdNo = n.CustomizeOddIdNo,
                       IsRetrieve=db.TM_Card_RetrieveNew.Where(m=>m.CustomizeOddId==n.CustomizeOddId).FirstOrDefault(),
                       //旧版
                       //AcceptingUnit = n.CompanyCode == "" ? db.V_M_TM_SYS_BaseData_store.Where(g => g.StoreCode == n.StoreCode).Select(m => m.StoreName).FirstOrDefault() : db.V_M_TM_SYS_BaseData_channel.Where(c => c.ChannelCodeBase == n.CompanyCode).Select(f => f.ChannelNameBase).FirstOrDefault(),
                       AcceptingUnit=n.StoreCode,
                       BoxNumIn = n.BoxNumIn,
                       CardNumIn = n.CardNumIn
                   });
               if (string.IsNullOrEmpty(oddIdNo) == false)
               {
                   //long number = Convert.ToInt64(oddIdNo);
                   query = query.Where(n =>SqlFunctions.StringConvert((decimal)n.OddIdNo,14).Contains(oddIdNo));
               }
               if (string.IsNullOrEmpty(status) == false)
               {
                   query = query.Where(n => n.Status == status);
               }
               if (string.IsNullOrEmpty(agent) == false)
               {
                   query = query.Where(n => n.AgentCode == agent);
               }
               if (string.IsNullOrEmpty(createTime) == false)
               {
                   DateTime time = Convert.ToDateTime(createTime);
                   DateTime afterTime = time.AddHours(+24);
                   query = query.Where(n => n.CreateTime >= time && n.CreateTime < afterTime);
               }
               if (string.IsNullOrEmpty(boxNumIn) == false)
               {
                   int num = Convert.ToInt32(boxNumIn);
                   query = query.Where(n => n.BoxNumIn == num);
               }
               if (string.IsNullOrEmpty(cardNumIn) == false)
               {
                   int num = Convert.ToInt32(cardNumIn);
                   query = query.Where(n => n.CardNumIn == num);
               }
               if (string.IsNullOrEmpty(isRetrieve) == false)
               {
                   if (isRetrieve == "0")
                   {
                       query = query.Where(n => n.IsRetrieve == null);
                   }
                   else
                   {
                       query = query.Where(n => n.IsRetrieve != null);
                   }
               }         
               return new Result(true, "", query.ToDataTableSourceVsPage(pageInfo));
           }
       }

       /// <summary>
       /// 加载供应商
       /// </summary>
       /// <returns></returns>
       public static Result LoadAgent()
       {
           using (CRMEntities db = new CRMEntities())
           {
               var query = from n in db.V_M_TM_SYS_BaseData_supplier where n.SupplierStatus==1 select new { AgentCode = n.SupplierCode, AgentName = n.SupplierName };
               return new Result(true, "", query.ToDataTableSource());
           }
       }

       /// <summary>
       /// 加载分公司
       /// </summary>
       /// <returns></returns>
       public static Result LoadCompany()
       {
           using (CRMEntities db = new CRMEntities())
           {
               //var query = (from n in db.V_M_TM_SYS_BaseData_channel join m in db.V_M_TM_SYS_BaseData_store on n.ChannelCodeBase equals m.ChannelCodeStore into d from dc in d.DefaultIfEmpty() select new { CompanyCode = n.ChannelCodeBase, CompanyName = n.ChannelNameBase, CompanyProvinceCode =dc.ProvinceCodeStore}).Distinct();
               var query = (from n in db.V_M_TM_SYS_BaseData_channel select new { CompanyCode = n.ChannelCodeBase, CompanyName = n.ChannelNameBase, CompanyProvinceCode = (from k in db.V_M_TM_SYS_BaseData_store where n.ChannelCodeBase == k.ChannelCodeStore select k.ProvinceCodeStore).FirstOrDefault() }).Distinct();
               return new Result(true, "", query.ToDataTableSource());
           }
       }

       /// <summary>
       /// 加载门店
       /// </summary>
       /// <param name="companyCode"></param>
       /// <returns></returns>
       public static Result LoadStore(string storeType)
       {
           using (CRMEntities db = new CRMEntities())
           {
               var list = from n in db.V_M_TM_SYS_BaseData_store select new { StoreCode = n.StoreCode, StoreName = n.StoreName, StoreProvinceCode = n.ProvinceCodeStore, StoreType = n.StoreType };
               if (string.IsNullOrWhiteSpace(storeType) == false)
               {
                   list = list.Where(n => n.StoreType == storeType);
               }
               return new Result(true, "", list.ToDataTableSource());
           }
       }

       /// <summary>
       /// 加载卡类型
       /// </summary>
       /// <returns></returns>
       public static Result LoadCardType()
       {
           using (CRMEntities db = new CRMEntities())
           {
               var query = from n in db.V_M_TM_SYS_BaseData_cardType where n.CardTypeCodeBase != "2" select new { CardCode = n.CardTypeCodeBase, CardName = n.CardTypeNameBase };
               return new Result(true, "", query.ToDataTableSource());
           }
       }

       /// <summary>
       /// 加载下拉框
       /// </summary>
       /// <param name="optionType"></param>
       /// <returns></returns>
       public static Result LoadBizOption(string optionType)
       {
           using (CRMEntities db = new CRMEntities())
           {          
               var query = from n in db.TD_SYS_BizOption where n.OptionType == optionType select new { OptionValue = n.OptionValue, OptionText = n.OptionText };
               return new Result(true, "", query.ToDataTableSource());
           }
       }

       /// <summary>
       /// 盒号列表
       /// </summary>
       /// <param name="boxNo"></param>
       /// <param name="status"></param>
       /// <param name="modifyby"></param>
       /// <param name="page"></param>
       /// <param name="user"></param>
       /// <returns></returns>
       public static Result BoxNoList(string boxNo, string status, string modifyby, string page, int user)
       {
           using (CRMEntities db = new CRMEntities())
           {
               DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);
               var query = from n in db.Tm_Card_CardBoxNew select new { BoxNo = n.BoxNo, Purpose = n.BoxPurposeId, CardNumIn = n.CardNumIn, CreateBy = from k in db.TM_AUTH_User where k.UserID == n.Createby select k.UserName, CreateTime = n.CreateTime, Status = n.CustomizeOddId, CardTypeName = from q in db.V_M_TM_SYS_BaseData_cardType where q.CardTypeCodeBase == "1" select q.CardTypeNameBase, AcceptingUnit = n.AcceptingUnit};
               if (string.IsNullOrEmpty(boxNo) == false)
               {
                   query = query.Where(n => n.BoxNo == boxNo);
               }
               if (string.IsNullOrEmpty(status) == false)
               {
                   if (status == "0")
                   {
                       query = query.Where(n => n.Status == null);
                   }
                   else
                   {
                       query = query.Where(n => n.Status != null);
                   }
               }
               if (string.IsNullOrEmpty(modifyby) == false)
               {
                   DateTime time = Convert.ToDateTime(modifyby);
                   DateTime afterTime = time.AddHours(+24);
                   query = query.Where(n => n.CreateTime >= time && n.CreateTime < afterTime);
               }
               return new Result(true, "", query.ToDataTableSourceVsPage(pageInfo));
           }
       }

       /// <summary>
       /// 新增盒号
       /// </summary>
       /// <param name="jsonParam"></param>
       /// <param name="user"></param>
       /// <returns></returns>
       public static Result AddBoxNo(string jsonParam, int user)
       {
           try
           {
               using (CRMEntities db = new CRMEntities())
               {
                   List<AddEmptyBoxNo> lt = JsonHelper.Deserialize<List<AddEmptyBoxNo>>(jsonParam);
                   long beginCardBoxNo = Convert.ToInt64(lt[0].BeginCardBoxNo);
                   int count = Convert.ToInt32(lt[0].CardBoxNum);
                   for (long i = 0; i < count; i++)
                   {
                       string temp1 = beginCardBoxNo.ToString();
                       var query = db.Tm_Card_CardBoxNew.Where(n => n.BoxNo == temp1).FirstOrDefault();
                       if (query!=null)
                       {
                           i--;
                           beginCardBoxNo++;
                           continue;
                       }
                       Tm_Card_CardBoxNew NewBox = new Tm_Card_CardBoxNew()
                       {
                           BoxId = ToolsHelper.GuidtoString(Guid.NewGuid()),
                           BoxNo = temp1,
                           CardNumIn = 0,
                           Createby = user,
                           CreateTime = DateTime.Now,
                           BoxPurposeId=Convert.ToInt32(lt[0].Purpose),
                       };
                       db.Tm_Card_CardBoxNew.Add(NewBox);
                       beginCardBoxNo++;
                   }
                   db.SaveChanges();
                   return new Result(true);
               }
           }
           catch (Exception ex)
           {
               Log4netHelper.WriteErrorLog(ex.Message.ToString());
               throw;
           }
       }

       /// <summary>
       /// 查询空盒
       /// </summary>
       /// <param name="boxNoInput"></param>
       /// <param name="user"></param>
       /// <param name="page"></param>
       /// <returns></returns>
       public static Result GetEmptyBoxNo(string boxNoInput, int user, string page)
       {
           using (CRMEntities db = new CRMEntities())
           {
               DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);
               var list = from n in db.Tm_Card_CardBoxNew where n.CustomizeOddId == null orderby n.BoxNo select new { BoxNo = n.BoxNo };
               if (string.IsNullOrEmpty(boxNoInput) == false)
               {
                   list = list.Where(n => n.BoxNo.Contains(boxNoInput));
               }
               return new Result(true, "", list.ToDataTableSourceVsPage(pageInfo));
           }
       }

       /// <summary>
       /// 新增定卡
       /// </summary>
       /// <returns></returns>
       public static Result AddCustomize(string jsonParams, string status, int userId)
       {
           using (CRMEntities db = new CRMEntities())
           {
               try
               {
                   List<CustomizeNewCard> NewCardParams = JsonHelper.Deserialize<List<CustomizeNewCard>>(jsonParams);
                   string customizeNewCard = ToolsHelper.GuidtoString(Guid.NewGuid());
                   //旧版
                   //if (GetEndNo(Convert.ToInt64(NewCardParams[0].BeginCardNo), Convert.ToInt32(NewCardParams[0].DestineNum)) == 0)
                   //{
                   //    return new Result(false, "起始卡号不能含有数字4");
                   //}
                   //流水号
                   long oddIdNo = 0;
                   DateTime nowTime = DateTime.Today;
                   DateTime afterTime = nowTime.AddDays(+1);
                   var list = (from n in db.TM_Card_ProduceNew where n.CreateTime >= nowTime && n.CreateTime < afterTime orderby n.CustomizeOddIdNo descending select n.CustomizeOddIdNo).ToList();
                   if (list == null || list.Count == 0)
                   {
                       oddIdNo = Distribution.GetOddIdNoByRole();
                   }
                   else
                   {
                       oddIdNo = list[0].Value + 1;
                   };

                   int destineNum = Convert.ToInt32(NewCardParams[0].DestineNum);
                   long beginCardNo = Convert.ToInt64(NewCardParams[0].BeginCardNo);
                   long endCardNo = Convert.ToInt64(NewCardParams[0].EndCardNo);

                   //计算盒数量
                   int boxNum = GetBoxNum(destineNum);
                   //获得省份code
                   string provinceCode = NewCardParams[0].BeginCardNo.ToString().Substring(2, 2);
                   //Insert
                   TM_Card_ProduceNew NewCard = new TM_Card_ProduceNew()
                   {
                       CustomizeOddId = customizeNewCard,
                       CustomizeOddIdNo = oddIdNo,
                       Status = status,
                       CompanyCode = NewCardParams[0].CompanyCode,
                       StoreCode = NewCardParams[0].StoreCode,
                       Agent = NewCardParams[0].Agent,
                       BeginCardNo = NewCardParams[0].BeginCardNo,
                       EndCardNo = NewCardParams[0].EndCardNo,
                       BoxNumIn = boxNum,
                       MathRule = Convert.ToInt32(NewCardParams[0].MathRule),
                       CardNumIn = Convert.ToInt32(NewCardParams[0].DestineNum),
                       RetrieveBoxNumber = 0,
                       CardTypeCode = NewCardParams[0].CardType,
                       CreateBy = userId,
                       CreateTime = DateTime.Now,
                   };
                   db.TM_Card_ProduceNew.Add(NewCard);
                   //自动生成盒号
                   long prevBeginCardNo = 0;
                   if (string.IsNullOrEmpty(NewCardParams[0].CardNoArray))
                   {
                       var resultBox = db.sp_CRM_BoxNoCreateNew(boxNum, provinceCode, beginCardNo, endCardNo, customizeNewCard, NewCardParams[0].MathRule);
                     
                       //根据盒号生成卡号
                       var boxList = db.Tm_Card_CardBoxNew.Where(n => n.CustomizeOddId == customizeNewCard);
                           foreach (var item in boxList)
                           {
		                    var resultCard = db.sp_CRM_MemberCardNoInsertNew(Convert.ToInt64(item.BeginCardNo),Convert.ToInt64( item.EndCardNo), item.BoxNo, NewCardParams[0].MathRule);
                            item.BoxPurposeId = Convert.ToInt32(NewCardParams[0].BoxPurpose);
                            item.IsOut = false;
                            item.IsReturn = false;
                           }

                 
                   }
                   else//手动生成盒号
                   {
                       string[] boxList = NewCardParams[0].CardNoArray.Substring(0, NewCardParams[0].CardNoArray.Length - 1).Split(',');
                       for (int i = 0; i < boxList.Length; i++)
                       {
                           int count = 0;
                           if (destineNum <= 0)
                           {
                               break;
                           }
                           string paramBoxNo=boxList[i];
                           var query = db.Tm_Card_CardBoxNew.Where(n => n.BoxNo == paramBoxNo).FirstOrDefault();
                           query.IsOut = false;
                           query.IsReturn = false;
                           query.BoxPurposeId = Convert.ToInt32(NewCardParams[0].BoxPurpose);
                           query.CustomizeOddId = customizeNewCard;
                           int cardNumIn=destineNum > 250 ? 250 : destineNum;
                           if (i == 0)
                           {

                               long firstEndTemp = GetEndNo(beginCardNo, cardNumIn);
                               //盒卡关联
                               query.BeginCardNo = beginCardNo.ToString();
                               query.EndCardNo = firstEndTemp.ToString();
                               query.CardNumIn = cardNumIn;
                               var resultCard = db.sp_CRM_MemberCardNoInsertNew(beginCardNo, firstEndTemp, boxList[i], NewCardParams[0].MathRule);
                               //旧版
                               //prevBeginCardNo = GetEndNo(firstEndTemp, 2);
                               prevBeginCardNo = firstEndTemp + 1;
                               count++;
                           }
                           else
                           {
                               long endTemp = GetEndNo(prevBeginCardNo, cardNumIn);
                               //盒卡关联
                               query.BeginCardNo = prevBeginCardNo.ToString();
                               query.EndCardNo = endTemp.ToString();
                               query.CardNumIn = cardNumIn;                       
                               var resultCard = db.sp_CRM_MemberCardNoInsertNew(prevBeginCardNo, endTemp, boxList[i], NewCardParams[0].MathRule);
                               //旧版
                               //prevBeginCardNo = GetEndNo(endTemp, 2);       
                               prevBeginCardNo = endTemp + 1;
                           }
                              destineNum -= 250;          
                       }
                   };
        
                   db.SaveChanges();
                   return new Result(true, "新增定卡成功");
               }
               catch (Exception ex)
               {
                   Log4netHelper.WriteErrorLog("AddCustomize ：" + ex.ToString());
                   return new Result(false, "新增定卡失败");           
                   throw;
               }
           }
       }

       /// <summary>
       /// 自动生成卡号
       /// </summary>
       /// <param name="cardNum"></param>
       /// <returns></returns>
       public static Result GetAutoCard(string cardNum, string provinceCode, string mathRule)
       {
           //旧版
           //int cardCount=Convert.ToInt32(cardNum);
           //long paramBeginCardNo = Convert.ToInt64(string.Format("21{0}00000000", provinceCode));
           //if (provinceCode=="21")
           //{
           //    paramBeginCardNo = 212100033065;
           //}
           //long paramEndCardNo = GetEndNo(paramBeginCardNo, cardCount);
           //string startWith = string.Format("21{0}", provinceCode);

           int cardCount = Convert.ToInt32(cardNum);
           long paramBeginCardNo = Convert.ToInt64("211000000000");         
           long paramEndCardNo = GetEndNo(paramBeginCardNo, cardCount);
           string startWith = "2110";
           using (CRMEntities db=new CRMEntities())
           {
               var list = db.TM_Card_ProduceNew.Where(m => m.BeginCardNo.StartsWith(startWith)).Select(n => new { BeginCardNo = n.BeginCardNo, EndCardNo = n.EndCardNo }).OrderBy(m => m.BeginCardNo);               
               foreach (var item in list)
               {
                   long itemBegin = Convert.ToInt64(item.BeginCardNo);
                   long itemEnd = Convert.ToInt64(item.EndCardNo);
                   //旧版
                   //if (itemBegin <= paramBeginCardNo && itemEnd >= paramBeginCardNo)
                   //{
                   //    paramBeginCardNo = GetEndNo(itemEnd, 2);
                   //    paramEndCardNo = GetEndNo(paramBeginCardNo, cardCount);
                   //    continue;
                   //}
                   //else if (itemBegin <= paramEndCardNo && itemEnd >= paramEndCardNo)
                   //{
                   //    paramBeginCardNo = GetEndNo(itemEnd, 2);
                   //    paramEndCardNo = GetEndNo(paramBeginCardNo, cardCount);
                   //    continue;
                   //}
                   //else if (itemBegin >= paramBeginCardNo && itemEnd <= paramEndCardNo)
                   //{
                   //    paramBeginCardNo = GetEndNo(itemEnd, 2);
                   //    paramEndCardNo = GetEndNo(paramBeginCardNo, cardCount);
                   //    continue;
                   //}
                   if (itemBegin <= paramBeginCardNo && itemEnd >= paramBeginCardNo)
                   {
                       paramBeginCardNo = itemEnd + 1;
                       paramEndCardNo = GetEndNo(paramBeginCardNo, cardCount);
                       continue;
                   }
                   else if (itemBegin <= paramEndCardNo && itemEnd >= paramEndCardNo)
                   {
                       paramBeginCardNo = itemEnd + 1;
                       paramEndCardNo = GetEndNo(paramBeginCardNo, cardCount);
                       continue;
                   }
                   else if (itemBegin >= paramBeginCardNo && itemEnd <= paramEndCardNo)
                   {
                       paramBeginCardNo = itemEnd + 1;
                       paramEndCardNo = GetEndNo(paramBeginCardNo, cardCount);
                       continue;
                   }
               }
               string result = string.Format("{0},{1}", paramBeginCardNo, paramEndCardNo);
               return new Result(true,result);
           }
       }

       /// <summary>
       /// 手动获得卡号
       /// </summary>
       /// <param name="beginCardNo"></param>
       /// <param name="cardNum"></param>
       /// <returns></returns>
       public static Result GetManualCard(string beginCardNo, string cardNum, string provinceCode, string mathRule)
       {
           using (CRMEntities db=new CRMEntities())
           {
               long paramBeginCardNo=Convert.ToInt64(beginCardNo);
               if (GetEndNo(paramBeginCardNo,2) == 0)
               {
                   return new Result(false, "起始卡号不能含有数字4");
               };
               if (provinceCode=="21"&&paramBeginCardNo<212100033065 )
               {
                   return new Result(false, "编号为21的省份起始卡号不能小于212100033065");
               };
               long paramEndCardNo=GetEndNo(Convert.ToInt64(beginCardNo),Convert.ToInt32(cardNum));
               //旧版
               //string startWith = string.Format("21{0}", provinceCode);
               string startWith = "2110";
               var list = db.TM_Card_ProduceNew.Where(c=>c.BeginCardNo.StartsWith(startWith)).Select(n => new { BeginCardNo = n.BeginCardNo, EndCardNo = n.EndCardNo }).OrderBy(m=>m.BeginCardNo);
               foreach (var item in list)
               {
                   long itemBegin = Convert.ToInt64(item.BeginCardNo);
                   long itemEnd = Convert.ToInt64(item.EndCardNo);
                   if (itemBegin <= paramBeginCardNo && itemEnd >= paramBeginCardNo)
                   {
                       return new Result(false, "起始号段已被使用");
                   }
                   else if (itemBegin <= paramEndCardNo && itemEnd >= paramEndCardNo)
                   {
                       return new Result(false, "截止号段已被使用");
                   }
                   else if (itemBegin >= paramBeginCardNo && itemEnd <= paramEndCardNo)
                   {
                       return new Result(false, "号段中某部分已被使用");
                   }
               }
               return new Result(true,paramEndCardNo.ToString());
           }
       }

       /// <summary>
       /// 详情页基本信息
       /// </summary>
       /// <param name="customizeOddId"></param>
       /// <returns></returns>
       public static Result CustomizeDetailsParams(string customizeOddId,string page)
       {
           DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);       
           using (CRMEntities db=new CRMEntities())
           {
               var query = from n in db.TM_Card_ProduceNew
                           join m in db.Tm_Card_CardBoxNew on n.CustomizeOddId equals m.CustomizeOddId into d
                           from ds in d.DefaultIfEmpty()
                           where n.CustomizeOddId == customizeOddId
                           select new
                               {
                                 
                                   Status = n.Status,
                                   StatusName = (from s in db.TD_SYS_BizOption where s.OptionValue == n.Status && s.OptionType == "ApproveStatus" select s.OptionText).FirstOrDefault(),
                                   //AgentCode = n.Agent,
                                   //Agent = (from k in db.V_M_TM_SYS_BaseData_supplier where n.Agent == k.SupplierCode select k.SupplierName).FirstOrDefault(),
                                   //CreateBy = (from m in db.TM_AUTH_User where m.UserID == n.CreateBy select m.UserName).FirstOrDefault(),
                                   //CreateTime = n.CreateTime,
                                   OddIdNo = n.CustomizeOddIdNo,
                                   //BoxNumIn = n.BoxNumIn,
                                   //CardNumIn = n.CardNumIn,
                                   BoxNo=ds.BoxNo,
                                   BeginCardNo=ds.BeginCardNo,
                                   EndCardNo=ds.EndCardNo,
                                   //AcceptingUnit=n.AcceptingUnit
                               };
               return new Result(true, "", query.ToDataTableSourceVsPage(pageInfo));
           }
       }

       /// <summary>
       ///  导出Excel
       /// </summary>
       /// <param name="customizeOddId"></param>
       /// <returns></returns>
       public static Result CustomizeToExcel(string customizeOddId)
       {
           using (CRMEntities db = new CRMEntities())
           {
               string sql = "select ROW_NUMBER() over(order by CreateTime) as Sequence,BoxNo,BeginCardNo,EndCardNo from [dbo].[Tm_Card_CardBoxNew] where CustomizeOddId=@CustomizeOddId";
               SqlParameter sqlParams = new SqlParameter("@CustomizeOddId", customizeOddId);
               var list = db.Database.SqlQuery<CustomizeToExcel>(sql, sqlParams).ToList();
               return new Result(true, "",list);          
           }
       }

       /// <summary>
       /// 审批
       /// </summary>
       /// <param name="customizeOddId"></param>
       /// <param name="status"></param>
       /// <returns></returns>
       public static Result ChangeStatus(string customizeOddId, string status)
       {
           try
           {
               using (CRMEntities db = new CRMEntities())
               {
                   var query = db.TM_Card_ProduceNew.Where(n => n.CustomizeOddId == customizeOddId).FirstOrDefault();
                   query.Status = status;
                   db.SaveChanges();
                   return new Result(true, "操作成功");
               }
           }
           catch (Exception ex)
           {
               Log4netHelper.WriteErrorLog("ChangeStatus :" + ex.Message.ToString());
               return new Result(false, "操作失败");
               throw;
           }
          
       }

       /// <summary>
       /// 定卡列表导出Excel
       /// </summary>
       /// <param name="txtExcelOddIdNo"></param>
       /// <param name="txtExcelAgent"></param>
       /// <param name="txtExcelStatus"></param>
       /// <param name="txtExcelBoxNumIn"></param>
       /// <param name="txtExcelCardNumIn"></param>
       /// <param name="txtExcelCreateTime"></param>
       /// <returns></returns>
       public static Result CustomizeListToExcel(string txtExcelOddIdNo, string txtExcelAgent, string txtExcelStatus, string txtExcelBoxNumIn, string txtExcelCardNumIn, string txtExcelCreateTime, string txtExcelIsRetrieve)
       {
           using (CRMEntities db = new CRMEntities())
           {
               var query = db.TM_Card_ProduceNew.OrderByDescending(m => m.CreateTime)
                   .Select(n => new
                   {                    
                       Status = n.Status,
                       StatusName = (from s in db.TD_SYS_BizOption where s.OptionValue == n.Status && s.OptionType == "ApproveStatus" select s.OptionText).FirstOrDefault(),             
                       AgentCode = n.Agent,
                       Agent = (from k in db.V_M_TM_SYS_BaseData_supplier where n.Agent == k.SupplierCode select k.SupplierName).FirstOrDefault(),
                       CreateBy = (from m in db.TM_AUTH_User where m.UserID == n.CreateBy select m.UserName).FirstOrDefault(),
                       CreateTime = n.CreateTime,
                       OddIdNo = n.CustomizeOddIdNo,
                       IsRetrieve = db.TM_Card_RetrieveNew.Where(m => m.CustomizeOddId == n.CustomizeOddId).FirstOrDefault(),
                       AcceptingUnit = n.CompanyCode == "" ? db.V_M_TM_SYS_BaseData_store.Where(g => g.StoreCode == n.StoreCode).Select(m => m.StoreName).FirstOrDefault() : db.V_M_TM_SYS_BaseData_channel.Where(c => c.ChannelCodeBase == n.CompanyCode).Select(f => f.ChannelNameBase).FirstOrDefault(),
                       BoxNumIn = n.BoxNumIn,
                       CardNumIn = n.CardNumIn
                   });
               if (string.IsNullOrEmpty(txtExcelOddIdNo) == false)
               {
                   long number = Convert.ToInt64(txtExcelOddIdNo);
                   query = query.Where(n => n.OddIdNo == number);
               }
               if (string.IsNullOrEmpty(txtExcelStatus) == false)
               {
                   query = query.Where(n => n.Status == txtExcelStatus);
               }
               if (string.IsNullOrEmpty(txtExcelAgent) == false)
               {
                   query = query.Where(n => n.AgentCode == txtExcelAgent);
               }
               if (string.IsNullOrEmpty(txtExcelCreateTime) == false)
               {
                   DateTime time = Convert.ToDateTime(txtExcelCreateTime);
                   DateTime afterTime = time.AddHours(+24);
                   query = query.Where(n => n.CreateTime >= time && n.CreateTime < afterTime);
               }
               if (string.IsNullOrEmpty(txtExcelBoxNumIn) == false)
               {
                   int num = Convert.ToInt32(txtExcelBoxNumIn);
                   query = query.Where(n => n.BoxNumIn == num);
               }
               if (string.IsNullOrEmpty(txtExcelCardNumIn) == false)
               {
                   int num = Convert.ToInt32(txtExcelCardNumIn);
                   query = query.Where(n => n.CardNumIn == num);
               }
               if (string.IsNullOrEmpty(txtExcelIsRetrieve) == false)
               {
                   if (txtExcelIsRetrieve == "0")
                   {
                       query = query.Where(n => n.IsRetrieve == null);
                   }
                   else
                   {
                       query = query.Where(n => n.IsRetrieve != null);
                   }
               }         
               return new Result(true, "", query.ToList());
           }
       }
       #endregion

       #region 收卡
       /// <summary>
       /// 收卡列表
       /// </summary>
       /// <param name="retrieveOddIdNo"></param>
       /// <param name="customizeOddIdNo"></param>
       /// <param name="status"></param>
       /// <param name="agent"></param>
       /// <param name="createTime"></param>
       /// <param name="page"></param>
       /// <returns></returns>
       public static Result RetrieveCardList(string retrieveOddIdNo, string customizeOddIdNo, string status, string agent, string createTime, string reserveBoxNumber, string page)
       {
           DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);
           using (CRMEntities db = new CRMEntities())
           {
               var query = from n in db.TM_Card_RetrieveNew orderby n.CreateTime descending select new { 
                   RetrieveOddId = n.RetrieveOddId, 
                   Status = n.Status,
                   StatusName = (from s in db.TD_SYS_BizOption where s.OptionValue == n.Status && s.OptionType == "ApproveStatus" select s.OptionText).FirstOrDefault(),
                   ReserveCardNumber = n.ReserveCardNumber, 
                   AgentCode = n.Agent, 
                   AgentName = (from k in db.V_M_TM_SYS_BaseData_supplier where n.Agent == k.SupplierCode select k.SupplierName).FirstOrDefault(),                              CreateBy = from m in db.TM_AUTH_User where m.UserID == n.CreateBy select m.UserName,
                   CreateTime = n.CreateTime,
                   RetrieveOddIdNo = n.RetrieveOddIdNo, 
                   CustomizeOddIdNo = (from p in db.TM_Card_ProduceNew where p.CustomizeOddId == n.CustomizeOddId select p.CustomizeOddIdNo).FirstOrDefault(), ReserveBoxNumber = n.ReserveBoxNumber };
               if (string.IsNullOrEmpty(retrieveOddIdNo) == false)
               {
                   //long oddIdNo = Convert.ToInt64(retrieveOddIdNo);
                   query = query.Where(n => SqlFunctions.StringConvert((decimal)n.RetrieveOddIdNo,14).Contains(retrieveOddIdNo));
               }
               if (string.IsNullOrEmpty(status) == false)
               {
                   query = query.Where(n => n.Status == status);
               }
               if (string.IsNullOrEmpty(agent) == false)
               {
                   query = query.Where(n => n.AgentCode == agent);
               }
               if (string.IsNullOrEmpty(createTime) == false)
               {
                   DateTime time = Convert.ToDateTime(createTime);
                   DateTime afterTime = time.AddHours(+24);
                   query = query.Where(n => n.CreateTime >= time && n.CreateTime < afterTime);
               }
               if (string.IsNullOrEmpty(reserveBoxNumber) == false)
               {
                   int num = Convert.ToInt32(reserveBoxNumber);
                   query = query.Where(n => n.ReserveBoxNumber == num);
               }
               if (string.IsNullOrEmpty(customizeOddIdNo) == false)
               {
                   long oddIdNo = Convert.ToInt64(customizeOddIdNo);
                   query = query.Where(n => n.CustomizeOddIdNo == oddIdNo);
               }
               return new Result(true, "", query.ToDataTableSourceVsPage(pageInfo));
           }
       }

       /// <summary>
       /// 加载定卡单号
       /// </summary>
       /// <returns></returns>
       public static Result loadCustomizeOddIdNo()
       {
           using (CRMEntities db=new CRMEntities())
           {
               var list = db.TM_Card_ProduceNew.Where(n => n.Status == "1" && n.RetrieveBoxNumber < n.BoxNumIn).Select(m => new { CustomizeOddId = m.CustomizeOddId, CustomizeOddIdNo = m.CustomizeOddIdNo,CreateTime=m.CreateTime }).OrderByDescending(t=>t.CreateTime);
               return new Result(true,"",list.ToDataTableSource());
           }
       }

       /// <summary>
       /// 新增收卡
       /// </summary>
       /// <param name="customizeOddId"></param>
       /// <param name="status"></param>
       /// <param name="userId"></param>
       /// <returns></returns>
       public static Result AddRetrieve(string customizeOddId,string status,int userId)
       {
           try
           {
               using (CRMEntities db = new CRMEntities())
               {
                   var query = db.TM_Card_ProduceNew.Where(n => n.CustomizeOddId == customizeOddId).FirstOrDefault();
                   query.RetrieveBoxNumber = query.BoxNumIn;
                   //收卡单号
                   string retrieveOddId = ToolsHelper.GuidtoString(Guid.NewGuid());
                   //流水号
                   long oddIdNo = 0;
                   DateTime nowTime = DateTime.Today;
                   DateTime afterTime = nowTime.AddDays(+1);
                   var list = (from n in db.TM_Card_RetrieveNew where n.CreateTime >= nowTime && n.CreateTime < afterTime orderby n.RetrieveOddIdNo descending select n.RetrieveOddIdNo).ToList();
                   if (list == null || list.Count == 0)
                   {
                       oddIdNo = Distribution.GetOddIdNoByRole();
                   }
                   else
                   {
                       oddIdNo = list[0].Value + 1;
                   };
                   TM_Card_RetrieveNew NewRetrieve = new TM_Card_RetrieveNew()
                   {
                       RetrieveOddId = retrieveOddId,
                       Status=status,
                       Agent = query.Agent,
                       CustomizeOddId = query.CustomizeOddId,
                       RetrieveOddIdNo = oddIdNo,
                       ReserveBoxNumber = query.BoxNumIn,
                       ReserveCardNumber = query.CardNumIn,
                       CompanyCode=query.CompanyCode,
                       StoreCode=query.StoreCode,
                       CreateBy = userId,
                       CreateTime = DateTime.Now,
                       Remark = query.Remark
                   };
                   db.TM_Card_RetrieveNew.Add(NewRetrieve);
                   var boxList = from n in db.TM_Card_ProduceNew join m in db.Tm_Card_CardBoxNew on n.CustomizeOddId equals m.CustomizeOddId into d from ds in d.DefaultIfEmpty() where n.CustomizeOddId == customizeOddId select new { BeginCardNo = ds.BeginCardNo, EndCardNo = ds.EndCardNo, BoxNo = ds.BoxNo,CardNumber=ds.CardNumIn };
                   foreach (var item in boxList)
                   {
                       TM_Card_RetrieveDetailNew NewRetrieveDetail = new TM_Card_RetrieveDetailNew()
                       {
                           RetrieveDetailId = ToolsHelper.GuidtoString(Guid.NewGuid()),
                           Status = status,
                           BeginCardNo =item.BeginCardNo,
                           EndCardNo = item.EndCardNo,
                           BoxNo=item.BoxNo,
                           CardNumber = item.CardNumber,
                           RetrieveOddId = retrieveOddId,
                           CardTypeCode=query.CardTypeCode
                       };
                       db.TM_Card_RetrieveDetailNew.Add(NewRetrieveDetail);
                   }           
                   db.SaveChanges();
                   return new Result(true, "新增收卡成功");
               }
           }
           catch (Exception ex)
           {
               Log4netHelper.WriteErrorLog("AddRetrieve :" + ex.Message.ToString());
               throw;
           }       
       }

       /// <summary>
       /// 详情列表
       /// </summary>
       /// <param name="customizeOddId"></param>
       /// <param name="page"></param>
       /// <returns></returns>
       public static Result RetrieveDetailsParams(string retrieveOddId, string page)
       {
           DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);
           using (CRMEntities db = new CRMEntities())
           {
               var query = from n in db.TM_Card_RetrieveNew join m in db.TM_Card_RetrieveDetailNew on n.RetrieveOddId equals m.RetrieveOddId into d from ds in d.DefaultIfEmpty() orderby n.CreateTime descending select new {RetrieveOddId=n.RetrieveOddId,Status = ds.Status, StatusName = (from s in db.TD_SYS_BizOption where s.OptionValue == ds.Status && s.OptionType == "ApproveStatus" select s.OptionText).FirstOrDefault(), OddIdNo = n.RetrieveOddIdNo, ReserveBoxNumber = n.ReserveBoxNumber,BeginCardNo=ds.BeginCardNo,EndCardNo=ds.EndCardNo,BoxNumber=ds.BoxNo};
               if (string.IsNullOrEmpty(retrieveOddId)==false)
               {
                   query = query.Where(n => n.RetrieveOddId== retrieveOddId);
               }
               return new Result(true, "", query.ToDataTableSourceVsPage(pageInfo));
           }
       }

       /// <summary>
       /// 审核
       /// </summary>
       /// <param name="retrieveOddId"></param>
       /// <param name="status"></param>
       /// <returns></returns>
       public static Result ChangeStatusRetrieve(string retrieveOddId,string status)
       {
           try
           {
               using (CRMEntities db = new CRMEntities())
               {
                   var query = db.TM_Card_RetrieveNew.Where(n => n.RetrieveOddId == retrieveOddId).FirstOrDefault();
                   query.Status = status;
                   var list = db.TM_Card_RetrieveDetailNew.Where(n => n.RetrieveOddId == retrieveOddId).ToList();
                   foreach (var item in list)
                   {
                       item.Status = status;
                   };                 
                   db.SaveChanges();
                   return new Result(true, "操作成功");
               }
           }
           catch (Exception ex)
           {
               Log4netHelper.WriteErrorLog("ChangeStatus :" + ex.Message.ToString());
               return new Result(false, "操作失败");
               throw;
           }
       }

       /// <summary>
       /// 收卡列表导出Excel
       /// </summary>
       /// <param name="txtExcelRetrieveOddIdNo"></param>
       /// <param name="txtExcelAgent"></param>
       /// <param name="txtExcelStatus"></param>
       /// <param name="txtExcelCustomizeOddIdNo"></param>
       /// <param name="txtExcelReserveBox"></param>
       /// <param name="txtExcelCreateTime"></param>
       /// <returns></returns>
       public static Result RetrieveListToExcel(string txtExcelRetrieveOddIdNo, string txtExcelAgent, string txtExcelStatus, string txtExcelCustomizeOddIdNo, string txtExcelReserveBox, string txtExcelCreateTime)
       {
           using (CRMEntities db = new CRMEntities())
           {
               var query = from n in db.TM_Card_RetrieveNew orderby n.CreateTime descending select new { RetrieveOddId = n.RetrieveOddId, Status = n.Status, StatusName = (from s in db.TD_SYS_BizOption where s.OptionValue == n.Status && s.OptionType == "ApproveStatus" select s.OptionText).FirstOrDefault(), ReserveCardNumber = n.ReserveCardNumber, AgentCode = n.Agent, AgentName = (from k in db.V_M_TM_SYS_BaseData_supplier where n.Agent == k.SupplierCode select k.SupplierName).FirstOrDefault(), CreateBy = from m in db.TM_AUTH_User where m.UserID == n.CreateBy select m.UserName, CreateTime = n.CreateTime, RetrieveOddIdNo = n.RetrieveOddIdNo, CustomizeOddIdNo = (from p in db.TM_Card_ProduceNew where p.CustomizeOddId == n.CustomizeOddId select p.CustomizeOddIdNo).FirstOrDefault(), ReserveBoxNumber = n.ReserveBoxNumber,ReserveCardNo=n.ReserveCardNumber };
               if (string.IsNullOrEmpty(txtExcelRetrieveOddIdNo) == false)
               {
                   long oddIdNo = Convert.ToInt64(txtExcelRetrieveOddIdNo);
                   query = query.Where(n => n.RetrieveOddIdNo == oddIdNo);
               }
               if (string.IsNullOrEmpty(txtExcelStatus) == false)
               {
                   query = query.Where(n => n.Status == txtExcelStatus);
               }
               if (string.IsNullOrEmpty(txtExcelAgent) == false)
               {
                   query = query.Where(n => n.AgentCode == txtExcelAgent);
               }
               if (string.IsNullOrEmpty(txtExcelCreateTime) == false)
               {
                   DateTime time = Convert.ToDateTime(txtExcelCreateTime);
                   DateTime afterTime = time.AddHours(+24);
                   query = query.Where(n => n.CreateTime >= time && n.CreateTime < afterTime);
               }
               if (string.IsNullOrEmpty(txtExcelReserveBox) == false)
               {
                   int num = Convert.ToInt32(txtExcelReserveBox);
                   query = query.Where(n => n.ReserveBoxNumber == num);
               }
               if (string.IsNullOrEmpty(txtExcelCustomizeOddIdNo) == false)
               {
                   long oddIdNo = Convert.ToInt64(txtExcelCustomizeOddIdNo);
                   query = query.Where(n => n.CustomizeOddIdNo == oddIdNo);
               }
               return new Result(true, "", query.ToList());
           }
       }
       #endregion

       #region 卡号算法
       /// <summary>
       /// 获取不带4的结束卡号（顺序）   Count-1
       /// </summary>
       /// <param name="startNo">开始卡号</param>
       /// <param name="count">数量</param>
       /// <returns></returns>
       //internal static long GetEndNo(long beginNo, int count)
       //{
       //    string startNo = beginNo.ToString();
       //    long result = 0;
       //    bool isContinued = true;

       //    //将开始字符串转换为标准的9进制字符串
       //    StringBuilder sb = new StringBuilder();
       //    foreach (var c in startNo)
       //    {
       //        int num = Convert.ToInt32(c.ToString());
       //        //非法数字
       //        if (num == 4)
       //        {
       //            isContinued = false;
       //            break;
       //        }
       //        else if (num > 4)
       //            sb.Append(num - 1);
       //        else
       //            sb.Append(num);
       //    }
       //    if (isContinued)
       //    {
       //        string startStr9 = sb.ToString();
       //        long startLong10 = Convert9To10(startStr9);
       //        long endLong10 = startLong10 + (count - 1);
       //        string endStr9 = Convert10To9(endLong10);

       //        //将9进制字符串转换为没有4的字符串
       //        sb.Clear();
       //        foreach (char c in endStr9)
       //        {
       //            int num = Convert.ToInt32(c.ToString());
       //            if (num >= 4)
       //                sb.Append(num + 1);
       //            else
       //                sb.Append(num);
       //        }
       //        string res = sb.ToString();
       //        result = Convert.ToInt64(res);
       //    }
       //    return result;
       //}

       internal static long GetEndNo(long beginNo, int count)
       {
           return beginNo + count - 1;
       }

       /// <summary>
       /// 九进制数据转十进制
       /// </summary>
       /// <param name="num9">九进制数</param>
       /// <returns></returns>
       private static long Convert9To10(string num9)
       {
           long res = 0;
           long step = 1;
           for (int i = num9.Length - 1; i >= 0; i--)
           {
               res += Convert.ToInt32(num9[i].ToString()) * step;
               step *= 9;
           }
           return res;
       }

       /// <summary>
       /// 十进制数据转九进制数据
       /// </summary>
       /// <param name="num10">十进制数</param>
       /// <returns></returns>
       private static string Convert10To9(long num10)
       {
           string s = ""; ;
           while (num10 > 0)
           {
               s = (num10 % 9) + s;
               num10 /= 9;
           }
           return s;
       }

       /// <summary>
       /// 计算盒数量
       /// </summary>
       /// <param name="destineNum"></param>
       /// <returns></returns>
       private static int GetBoxNum(int destineNum)
       {
           int result = 0;
           if (destineNum<250)
           {
               result = 1;
           }
           else
           {
               result = destineNum % 250 == 0 ? destineNum / 250 : destineNum / 250 + 1;
           }
           return result;
       }

   
       #endregion
   }
}
