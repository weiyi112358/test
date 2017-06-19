using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Objects.SqlClient;
using System.Data.SqlClient;


namespace Arvato.CRM.BizLogic
{
    public class CardProduction
    {
        /// <summary>
        /// 批量制发卡列表
        /// </summary>
        /// <param name="oddId"></param>
        /// <param name="status"></param>
        /// <param name="IsExecute"></param>
        /// <param name="modifyBy"></param>
        /// <param name="beginCardNo"></param>
        /// <param name="endCardNo"></param>
        /// <param name="type"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static Result GetBatchCardList(string oddId, string status, string IsExecute, string modifyBy, string beginCardNo, string endCardNo, string type, string page)
        {
            DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_Made join m in db.V_M_TM_SYS_BaseData_cardType on n.CardTypeCode equals m.CardTypeCodeBase into d from ds in d.DefaultIfEmpty() orderby n.CreateTime descending select new { CardId = n.CardId, Status = n.Status, IsExecute = n.IsExecute, CardTypeId = ds.CardTypeNameBase, BeginCardNum = n.BeginCardNum, EndCardNum = n.EndCardNum, CreateBy = from k in db.TM_AUTH_User where k.UserID == n.CreateBy select k.UserName, CreateTime = n.CreateTime, OddIdNo = n.OddIdNo };
                if (string.IsNullOrEmpty(oddId) == false)
                {
                    long oddIdNo = Convert.ToInt64(oddId);
                    query = query.Where(n => n.OddIdNo == oddIdNo);
                }
                if (string.IsNullOrEmpty(status) == false)
                {
                    query = query.Where(n => n.Status == status);
                }
                if (string.IsNullOrEmpty(IsExecute) == false)
                {
                    query = query.Where(n => n.IsExecute == IsExecute);
                }
                if (string.IsNullOrEmpty(modifyBy) == false)
                {
                    DateTime time = Convert.ToDateTime(modifyBy);
                    DateTime afterTime = time.AddHours(+24);
                    query = query.Where(n => n.CreateTime >= time && n.CreateTime < afterTime);
                }
                if (string.IsNullOrEmpty(beginCardNo) == false)
                {
                    query = query.Where(n => n.BeginCardNum == beginCardNo);
                }
                if (string.IsNullOrEmpty(endCardNo) == false)
                {
                    query = query.Where(n => n.EndCardNum == endCardNo);
                }
                return new Result(true, "", query.ToDataTableSourceVsPage(pageInfo));
            }
        }

        /// <summary>
        /// 跳号规则
        /// </summary>
        /// <returns></returns>
        public static Result LoadMathRule()
        {
            using (CRMEntities db = new CRMEntities())
            {
                string msg = "MathRule";
                var list = db.TD_SYS_BizOption.Where(n => n.OptionType == msg).Select(m => new { OptionValue = m.OptionValue, OptionText = m.OptionText });
                return new Result(true, "", list.ToDataTableSource());
            }
        }

        /// <summary>
        /// 判断卡号是否可用
        /// </summary>
        /// <param name="beginCardNo"></param>
        /// <param name="cardNum"></param>
        /// <param name="endCardNo"></param>
        /// <returns></returns>
        public static Result GetCardInfo(string beginCardNo, string cardNum, string endCardNo)
        {
            using (CRMEntities db = new CRMEntities())
            {
                string provinceCode = beginCardNo.Substring(2, 2);
                ObjectParameter IsEnable = new ObjectParameter("IsEnable", typeof(string));
                int resultCheck = db.sp_CRM_BoxNoCreateIsUsable(Convert.ToInt32(cardNum), provinceCode, Convert.ToInt64(beginCardNo), Convert.ToInt64(endCardNo), IsEnable);
                if (IsEnable.Value.ToString() == "False")
                {
                    return new Result(false, "起始卡号没有匹配到截止卡号，请重新输入正确的信息");
                }
                long intBegin = Convert.ToInt64(beginCardNo);
                long intEng = Convert.ToInt64(endCardNo);
                string sql = "select m.[status],t.ReserveNumber from [dbo].[TM_Card_RetrieveDetail] as n left join [dbo].[TM_Card_Retrieve] as m on n.retrieveid=m.retrieveid left join [dbo].[TM_Card_Produce] as t on m.oddid=t.oddid where Convert(bigint,n.begincardno)<=@BeginCardNo and Convert(bigint,n.endcardno)>=@EndCardNo";
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@BeginCardNo", intBegin);
                param[1] = new SqlParameter("@EndCardNo", intEng);
                IsStatus Retrieve = new IsStatus();
                Retrieve = db.Database.SqlQuery<IsStatus>(sql, param).FirstOrDefault();

                if (Retrieve != null)
                {
                    if (Retrieve.Status != "1")
                    {
                        return new Result(false, "该单号状态为非审核通过");
                    }
                    if (Retrieve.ReserveNumber < Convert.ToInt64(cardNum))
                    {
                        return new Result(false, "制卡数量大于收卡数量");
                    }
                }
                return new Result(true);
            }
        }

        /// <summary>
        /// 添加自动批量制发卡
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <param name="status"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static Result AddCard(string jsonParam, string status, int userName)
        {
            try
            {
                List<CardProductions> lt = JsonHelper.Deserialize<List<CardProductions>>(jsonParam);
                using (CRMEntities db = new CRMEntities())
                {
                    
                    //生成盒号实体卡号
                    string provinceCode = lt[0].BeginCardNo.Substring(2, 2);
                    int cardNum = Convert.ToInt32(lt[0].ProductionNum);
                    long beginCardNo = Convert.ToInt64(lt[0].BeginCardNo);
                    long endCardNo = Convert.ToInt64(lt[0].EndCardNo);

                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@BeginCardNo", beginCardNo);
                    param[1] = new SqlParameter("@EndCardNo", endCardNo);
                    AcceptUnit accept = new AcceptUnit();
                    accept = db.Database.SqlQuery<AcceptUnit>("select  m.AcceptingUnit from [dbo].[TM_Card_ProduceDetail] as n left join [dbo].[TM_Card_Produce] as m on n.OddId=m.OddId where Convert(bigint,n.begincardno)<=@BeginCardNo and Convert(bigint,n.endcardno)>=@EndCardNo", param).FirstOrDefault();
                    if (accept == null)
                    {
                        return new Result(false, "起始卡号和截止卡号匹配无效，请重新输入正确的信息");
                    }
                    string boxBatchNo = ToolsHelper.GuidtoString(Guid.NewGuid());

                    //通过定卡单号查询收卡信息  1vN
                    string sql = " select n.[status],m.begincardno,m.endcardno from [dbo].[TM_Card_Retrieve] n left join [dbo].[TM_Card_RetrieveDetail] m on n.RetrieveId=m.RetrieveId where n.OddId = (select oddid from [dbo].[TM_Card_ProduceDetail] t where Convert(bigint,t.BeginCardNo)<=@BeginCardNo and Convert(bigint,t.endcardno)>=@EndCardNo)";
                    SqlParameter[] paramCard=new SqlParameter[2];
                    paramCard[0]=new SqlParameter("@BeginCardNo",beginCardNo);
                    paramCard[1]=new SqlParameter("@EndCardNo",endCardNo);
                    List<PurchasesNewCard> NewCardList = new List<PurchasesNewCard>();
                    NewCardList = db.Database.SqlQuery<PurchasesNewCard>(sql, paramCard).ToList();
                    if (NewCardList==null)
                    {
                        return new Result(false, "没有符合条件的制发号段");
                    }
                    foreach (var item in NewCardList)
                    {
                        if (item.Status != "0")
                        {
                            int result = db.sp_CRM_BoxNoCreate(provinceCode, Convert.ToInt64(item.BeginCardNo), Convert.ToInt64(item.EndCardNo), boxBatchNo,"4");
                            if (result < 0)
                            {
                                return new Result(false, "生成盒号出错");
                            }

                            int result1 = db.sp_CRM_MemberCardNoInsert(cardNum, provinceCode, Convert.ToInt64(item.BeginCardNo), Convert.ToInt64(item.EndCardNo), boxBatchNo);
                            if (result < 0)
                            {
                                return new Result(false, "生成实体卡出错");
                            }
                        }
                    }
              
                    //生成单号
                    long oddIdNo = 0;
                    DateTime nowTime = DateTime.Today;
                    DateTime afterTime = nowTime.AddDays(+1);
                    var list = (from n in db.TM_Card_Made where n.CreateTime >= nowTime && n.CreateTime < afterTime orderby n.OddIdNo descending select n.OddIdNo).ToList();
                    if (list == null || list.Count == 0)
                    {
                        oddIdNo = Distribution.GetOddIdNoByRole();
                    }
                    else
                    {
                        oddIdNo = list[0].Value + 1;
                    }

                    //补充当前批次的参数                   
                    var cardBoxList = db.Tm_Card_CardBox.Where(n => n.BatchNo == boxBatchNo).ToList();
                    foreach (var item in cardBoxList)
                    {
                        item.AcceptingUnit = accept.AcceptingUnit;
                        item.BoxPurposeId = Convert.ToInt32(lt[0].Purpose);
                        item.IsOut = false;
                    };
                    TM_Card_Made NewCard = new TM_Card_Made()
                    {
                        CardId = ToolsHelper.GuidtoString(Guid.NewGuid()),
                        ArriveTime = Convert.ToDateTime(lt[0].ArriveTime),
                        BeginCardNum = lt[0].BeginCardNo,
                        CardTypeCode = lt[0].Code,
                        EndCardNum = lt[0].EndCardNo,
                        Production = cardNum,
                        IsExecute = "0",
                        Status = status,
                        BatchNo = boxBatchNo,
                        CreateBy = userName,
                        CreateTime = DateTime.Now,
                        OddIdNo=oddIdNo
                    };
                    db.TM_Card_Made.Add(NewCard);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message.ToString();
                        return new Result(false, msg);
                    }
                    return new Result(true);
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("AddCard: " + ex.ToString());
                return new Result(false);
            }
        }

        /// <summary>
        /// 添加手动批量制发卡
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <param name="status"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static Result AddCardByManual(string jsonParam, string status, int userName)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    List<CardProductions> lt = JsonHelper.Deserialize<List<CardProductions>>(jsonParam);

                    string provinceCode = lt[0].BeginCardNo.Substring(2, 2);
                    int cardNum = Convert.ToInt32(lt[0].ProductionNum);
                    long beginCardNo = Convert.ToInt64(lt[0].BeginCardNo);
                    long endCardNo = Convert.ToInt64(lt[0].EndCardNo);

                    //查询接收单位
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@BeginCardNo", beginCardNo);
                    param[1] = new SqlParameter("@EndCardNo", endCardNo);
                    AcceptUnit accept = new AcceptUnit();
                    accept = db.Database.SqlQuery<AcceptUnit>("select  m.AcceptingUnit from [dbo].[TM_Card_ProduceDetail] as n left join [dbo].[TM_Card_Produce] as m on n.OddId=m.OddId where Convert(bigint,n.begincardno)<=@BeginCardNo and Convert(bigint,n.endcardno)>=@EndCardNo", param).FirstOrDefault();
                    if (accept == null)
                    {
                        return new Result(false, "起始卡号和截止卡号匹配无效，请重新输入正确的信息");
                    }

                    //生成单号
                    long oddIdNo = 0;
                    DateTime nowTime = DateTime.Today;
                    DateTime afterTime = nowTime.AddDays(+1);
                    var list = (from n in db.TM_Card_Made where n.CreateTime >= nowTime && n.CreateTime < afterTime orderby n.OddIdNo descending select n.OddIdNo).ToList();
                    if (list == null || list.Count == 0)
                    {
                        oddIdNo = Distribution.GetOddIdNoByRole();
                    }
                    else
                    {
                        oddIdNo = list[0].Value + 1;
                    }
                    //update盒信息
                    string boxBatchNo = ToolsHelper.GuidtoString(Guid.NewGuid());
                    string[] boxArray = lt[0].ArrayBoxNo.Substring(0, lt[0].ArrayBoxNo.Length - 1).Split(',');             
                    int logCardNum = cardNum;
                    long logBeginCardNo = beginCardNo;
                    long lastEndCardNo = 0;
                    for (int i = 0; i < boxArray.Length; i++)
                    {
                        int num = logCardNum;
                        if (num < 0)
                        {
                            break;
                        }
                        long tempBegin = 0;
                        long tempEnd = 0;
                        if (i == 0)
                        {
                            tempEnd = GetEndNo(logBeginCardNo, num);
                            if (tempEnd == 0)
                            {
                                logBeginCardNo = GetEndNo(lastEndCardNo, 1);
                                tempEnd = GetEndNo(logBeginCardNo, num);                               
                            }
                        }
                        else
                        {
                            tempBegin = lastEndCardNo + 1;
                            tempEnd = GetEndNo(lastEndCardNo + 1, num);
                            if (tempEnd == 0)
                            {
                                long rebuildBeginCardNo = GetEndNo(lastEndCardNo, 1);
                                tempEnd = GetEndNo(rebuildBeginCardNo, num);
                                tempBegin = rebuildBeginCardNo;
                            }
                        }
                        string boxNo = boxArray[i];
                        var queryBox = db.Tm_Card_CardBox.Where(n => n.BoxNo == boxNo).FirstOrDefault();
                        queryBox.BatchNo = boxBatchNo;
                        queryBox.AcceptingUnit = accept.AcceptingUnit;
                        queryBox.BeginCardNo = i == 0 ? logBeginCardNo.ToString() : tempBegin.ToString();
                        queryBox.EndCardNo = tempEnd.ToString();
                        queryBox.ModifyBy = userName;
                        queryBox.ModifyTime = DateTime.Now;
                        queryBox.IsOut = false;
                        queryBox.IsReturn = false;
                        queryBox.CardNumIn = num > 250 ? 250 : num;
                        queryBox.BoxPurposeId = Convert.ToInt32(lt[0].Purpose);
                        logCardNum -= 250;
                        lastEndCardNo = Convert.ToInt64(tempEnd);
                    }
                    TM_Card_Made NewCard = new TM_Card_Made()
                    {
                        CardId = ToolsHelper.GuidtoString(Guid.NewGuid()),
                        ArriveTime = Convert.ToDateTime(lt[0].ArriveTime),
                        BeginCardNum = lt[0].BeginCardNo,
                        CardTypeCode = lt[0].Code,
                        EndCardNum = lt[0].EndCardNo,
                        Production = cardNum,
                        IsExecute = "0",
                        Status = status,
                        BatchNo = boxBatchNo,
                        CreateBy = userName,
                        CreateTime = DateTime.Now,
                        OddIdNo = oddIdNo
                    };
                    db.TM_Card_Made.Add(NewCard);
                    db.SaveChanges();
             
                    int result = db.sp_CRM_MemberCardNoInsert(cardNum, provinceCode, beginCardNo, endCardNo, boxBatchNo);
                    if (result < 0)
                    {
                        return new Result(false, "生成实体卡出错");
                    }
                    db.SaveChanges();
                    return new Result(true);
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("AddCardByManual: " + ex.ToString()); 
                throw;
            }
        }

        /// <summary>
        /// 加载空盒号
        /// </summary>
        /// <param name="user"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static Result GetEmptyBoxNo(string boxNoInput,int user,string page)
        {
            using (CRMEntities db = new CRMEntities())
            {
                DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);          
                var list=from n in db.Tm_Card_CardBox where n.BeginCardNo==null orderby n.BoxNo select new{BoxNo = n.BoxNo};
                if (string.IsNullOrEmpty(boxNoInput)==false)
                {
                    list = list.Where(n => n.BoxNo.Contains(boxNoInput));
                }
                return new Result(true, "", list.ToDataTableSourceVsPage(pageInfo));            
            }
        }


        //SP5
        public static Result BoxIDBuild(string boxId, string productionNum, string beginCardNo, string endCardNo)
        {
            using (CRMEntities db = new CRMEntities())
            {
                string provinceCode = beginCardNo.Substring(2, 2);
                ObjectParameter MemberCardEnd = new ObjectParameter("MemberCardEnd", typeof(long));
                ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
                //int result = db.sp_CRM_BoxNoCreate(            
                if (Convert.ToInt64(MemberCardEnd.Value) == 0)
                {
                    return new Result(false, "");
                }
                return new Result(true, MemberCardEnd.Value.ToString());

            }
        }



        /// <summary>
        /// 获取不带4的结束卡号（顺序）
        /// </summary>
        /// <param name="startNo">开始卡号</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        internal static long GetEndNo(long beginNo, int count)
        {
            string startNo = beginNo.ToString();    
            long result = 0;
            bool isContinued = true; 

            //将开始字符串转换为标准的9进制字符串
            StringBuilder sb = new StringBuilder();
            foreach (var c in startNo)
            {
                int num = Convert.ToInt32(c.ToString());
                //非法数字
                if (num == 4)
                {
                    isContinued = false;
                    break;
                }
                else if (num > 4)
                    sb.Append(num - 1);
                else
                    sb.Append(num);
            }
            if (isContinued)
            {
                string startStr9 = sb.ToString();
                long startLong10 = Convert9To10(startStr9);
                long endLong10 = startLong10 + count;
                string endStr9 = Convert10To9(endLong10);

                //将9进制字符串转换为没有4的字符串
                sb.Clear();
                foreach (char c in endStr9)
                {
                    int num = Convert.ToInt32(c.ToString());
                    if (num >= 4)
                        sb.Append(num + 1);
                    else
                        sb.Append(num);
                }
                string res = sb.ToString();
                result = Convert.ToInt64(res);
            }      
            return result;
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
            string s = ""; 
            while (num10 > 0)
            {
                s = (num10 % 9) + s;
                num10 /= 9;
            }
            return s;
        }
     
    }
}
