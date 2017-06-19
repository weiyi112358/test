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
    public static class Purchases
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
        public static Result CustomizeCardList(string oddId, string status, string agent, string modifyBy, string destineNumber, string page)
        {
            DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);
            using (CRMEntities db = new CRMEntities())
            {
                //var query = from n in db.TM_Card_Produce orderby n.CreateTime descending select new { OddId = n.OddId, Status = n.Status, DestineNumber = n.DestineNumber, ReserveNumber = n.ReserveNumber, Agent = from k in db.V_M_TM_SYS_BaseData_supplier where n.Agent==k.SupplierCode select k.SupplierName, CreateBy = from m in db.TM_AUTH_User where m.UserID == n.CreateBy select m.UserName, CreateTime = n.CreateTime, OddIdNo = n.OddIdNo };
                var query=db.TM_Card_Produce.OrderByDescending(m=>m.CreateTime)
                    .Select(n=>new{
                        OddId = n.OddId, 
                        Status = n.Status, 
                        DestineNumber = n.DestineNumber, 
                        ReserveNumber = n.ReserveNumber, 
                        AgentCode=n.Agent,
                        Agent= (from k in db.V_M_TM_SYS_BaseData_supplier where n.Agent==k.SupplierCode select k.SupplierName).FirstOrDefault(), 
                        CreateBy =( from m in db.TM_AUTH_User where m.UserID == n.CreateBy select m.UserName).FirstOrDefault(),
                        CreateTime = n.CreateTime, 
                        OddIdNo = n.OddIdNo});
                if (string.IsNullOrEmpty(oddId) == false)
                {
                    long oddIdno = Convert.ToInt64(oddId);
                    query = query.Where(n => n.OddIdNo == oddIdno);
                }
                if (string.IsNullOrEmpty(status) == false)
                {
                    query = query.Where(n => n.Status == status);
                }
                if (string.IsNullOrEmpty(agent) == false)
                {
                    query = query.Where(n => n.AgentCode == agent);
                }
                if (string.IsNullOrEmpty(modifyBy) == false)
                {
                    DateTime time = Convert.ToDateTime(modifyBy);
                    DateTime afterTime = time.AddHours(+24);
                    query = query.Where(n => n.CreateTime >= time && n.CreateTime < afterTime);
                }
                if (string.IsNullOrEmpty(destineNumber) == false)
                {
                    int num = Convert.ToInt32(destineNumber);
                    query = query.Where(n => n.DestineNumber == num);
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
                var query = from n in db.V_M_TM_SYS_BaseData_company select new { CompanyCode = n.CompanyCode, CompanyName = n.CompanyName,ComPanyProvinceCode=n.CompanyProvinceCode };
                return new Result(true, "", query.ToDataTableSource());
            }
        }

        /// <summary>
        /// 加载省份
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Result LoadProviceCode(string name)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var provinceName = db.V_M_TM_SYS_BaseData_company.Where(n => n.CompanyCode == name).Select(m => new { Code = m.CompanyProvinceCode });
                return new Result(true, "", provinceName.ToDataTableSource());
            }
        }

        /// <summary>
        /// 添加定卡
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <param name="destineNumber"></param>
        /// <param name="status"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static Result AddCard(string jsonParam, string destineNumber, string status, int userName)
        {
            List<PurchasesCard> lt = JsonHelper.Deserialize<List<PurchasesCard>>(jsonParam);         
            using (CRMEntities db = new CRMEntities())
            {
                string id = ToolsHelper.GuidtoString(Guid.NewGuid());
                string acceptingUnit = "";
                string agent = "";
                int mathRule = 0;
                int cardCount=0;
                long oddIdNo = 0;
                DateTime nowTime = DateTime.Today;
                DateTime afterTime = nowTime.AddDays(+1);
                var list = (from n in db.TM_Card_Produce where n.CreateTime >= nowTime && n.CreateTime < afterTime orderby n.OddIdNo descending select n.OddIdNo).ToList();
                if (list == null || list.Count==0)
                {
                    oddIdNo =Distribution.GetOddIdNoByRole();
                }
                else
                {
                    oddIdNo = list[0].Value + 1;
                }

                foreach (var item in lt)
                {
                    TM_Card_ProduceDetail NewDetail = new TM_Card_ProduceDetail()
                    {
                        Id = ToolsHelper.GuidtoString(Guid.NewGuid()),
                        BeginCardNo = item.BeginCardNo,
                        EndCardNo = item.EndCardNo,
                        CardNum = Convert.ToInt32(item.CardNum),
                        CardTypeCode = item.Code,
                        CreateBy = userName,
                        CreateTime = DateTime.Now,
                        OddId = id
                    };
                    cardCount += Convert.ToInt32(item.CardNum);
                    acceptingUnit = item.AcceptingUnit;
                    agent = item.Agent;
                    mathRule = Convert.ToInt32(item.MathRule);
                    db.TM_Card_ProduceDetail.Add(NewDetail);
                };
                TM_Card_Produce NewCard = new TM_Card_Produce()
                {
                    OddId = id,
                    AcceptingUnit = acceptingUnit,
                    Agent = agent,
                    DestineNumber = cardCount,
                    ReserveNumber = 0,
                    Status = status,
                    MathRule = mathRule,
                    CreateBy = userName,
                    CreateTime = DateTime.Now,
                    OddIdNo=oddIdNo
                };
                db.TM_Card_Produce.Add(NewCard);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                    return new Result(false, msg);
                }
            }
            return new Result(true);
        }

        /// <summary>
        ///  自动生成卡号
        /// </summary>
        /// <param name="provinceCode"></param>
        /// <param name="cardNum"></param>
        /// <returns></returns>
        public static Result GetCardNo(string provinceCode, string cardNum)
        {
            using (var db = new CRMEntities())
            {
                ObjectParameter MemberCardStart = new ObjectParameter("MemberCardStart", typeof(string));
                ObjectParameter MemberCardEnd = new ObjectParameter("MemberCardEnd", typeof(string));
                ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
                var result = db.sp_CRM_MemberCardCreate(Convert.ToInt32(cardNum), provinceCode, MemberCardStart, MemberCardEnd);
                if (Convert.ToInt64(MemberCardStart.Value) == 0)
                {
                    return new Result(false, "");
                }
                else if (Convert.ToInt64(MemberCardEnd.Value) == 0)
                {
                    return new Result(false, "");
                }
                else
                {
                    string msg = MemberCardStart.Value + "," + MemberCardEnd.Value;
                    return new Result(true, msg);
                }
            }
        }

        //public static Result GetCardNo(string provinceCode, string cardNum)
        //{
        //    string beginCardNo = "";
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        string sql="";
        //        SqlParameter[] param=new SqlParameter[2];
        //        param[0]=new SqlParameter("@ProvinceCode",provinceCode);
        //        param[1]=new SqlParameter("@CardNum ",cardNum);
        //        PurchasesNewCard NewCard = new PurchasesNewCard();
        //        NewCard = db.Database.SqlQuery<PurchasesNewCard>(sql, param).FirstOrDefault();
        //    }

        //    string res = "";

        //    return new Result(true, res);
        //}


        /// <summary>
        /// 手动生成卡号
        /// </summary>
        /// <param name="beginCardNo"></param>
        /// <param name="provinceCode"></param>
        /// <param name="cardNum"></param>
        /// <returns></returns>
        public static Result GetCardNoManual(string beginCardNo, string provinceCode, string cardNum)
        {
            using (var db = new CRMEntities())
            {
                ObjectParameter MemberCardEnd = new ObjectParameter("MemberCardEnd", typeof(string));
                ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
                var result = db.sp_CRM_MemberCreateCardIsUsable(Convert.ToInt32(cardNum), provinceCode, Convert.ToInt64(beginCardNo), MemberCardEnd);
                if (Convert.ToInt64(MemberCardEnd.Value) == 0)
                {
                    return new Result(false, "");
                }
                else
                {
                    return new Result(true, MemberCardEnd.Value.ToString());
                }

            }
        }



        //public static Result GetCardNoManual(string beginCardNo, string provinceCode, string cardNum)
        //{
        //    long startNo =Convert.ToInt64(beginCardNo.Trim());
        //    long start = 0;
        //    if (!long.TryParse(beginCardNo.Trim(), out start))
        //    {
        //        return new Result(false, "参数为非有效数字");
        //    }

        //    //将开始字符串转换为标准的9进制字符串
        //    StringBuilder sb = new StringBuilder();
        //    foreach (var c in beginCardNo.Trim())
        //    {
        //        int num = Convert.ToInt32(c.ToString());
        //        if (num == 4)
        //            return new Result(false, "参数中含有非法数字");
        //        else if (num > 4)
        //            sb.Append(num - 1);
        //        else
        //            sb.Append(num);
        //    }
        //    int count=Convert.ToInt32(cardNum);
        //    string startStr9 = sb.ToString();
        //    long startLong10 = Convert9To10(startStr9);
        //    long endLong10 = startLong10 + count;
        //    string endStr9 = Convert10To9(endLong10);

        //    //将9进制字符串转换为没有4的字符串
        //    sb.Clear();
        //    foreach (char c in endStr9)
        //    {
        //        int num = Convert.ToInt32(c.ToString());
        //        if (num >= 4)
        //            sb.Append(num + 1);
        //        else
        //            sb.Append(num);
        //    }
        //    string res = sb.ToString();

        //    return new Result(true, res);
        //}



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
            string s = ""; ;
            while (num10 > 0)
            {
                s = (num10 % 9) + s;
                num10 /= 9;
            }
            return s;
        }





        #endregion


        #region 生成盒号
        public static Result BoxNoList(string boxNo, string status, string modifyby,string page,int user)
        {
            using (CRMEntities db=new CRMEntities())
            {
                DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);
                var query = from n in db.Tm_Card_CardBox select new { BoxNo = n.BoxNo, Purpose=n.BoxPurposeId,CardNumIn=n.CardNumIn,CreateBy=from k in db.TM_AUTH_User where k.UserID==n.Createby select k.UserName,CreateTime=n.CreateTime,Status=n.AcceptingUnit,CardTypeName=from q in db.V_M_TM_SYS_BaseData_cardType where q.CardTypeCodeBase=="1" select q.CardTypeNameBase,AcceptingUnit=n.AcceptingUnit };
                if (string.IsNullOrEmpty(boxNo) == false)
                {              
                    query = query.Where(n => n.BoxNo== boxNo);
                }
                if (string.IsNullOrEmpty(status) == false)
                {
                    if (status=="0")
                    {
                        query = query.Where(n => n.AcceptingUnit == null);
                    }
                    else
                    {
                        query = query.Where(n => n.AcceptingUnit != null);
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
                        var query = db.Tm_Card_CardBox.Where(n => n.BoxNo == temp1).FirstOrDefault();
                        if (query != null)
                        {            
                            i--;
                            beginCardBoxNo++;
                            continue;
                        }
                        Tm_Card_CardBox NewBox = new Tm_Card_CardBox()
                        {
                            BoxId = ToolsHelper.GuidtoString(Guid.NewGuid()),
                            BoxNo=temp1,
                            CardNumIn = 0,
                            Createby = user,
                            CreateTime = DateTime.Now
                        };
                        db.Tm_Card_CardBox.Add(NewBox);
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
        #endregion


        #region 收卡
        /// <summary>
        /// 收卡列表
        /// </summary>
        /// <param name="retrieveId"></param>
        /// <param name="status"></param>
        /// <param name="agent"></param>
        /// <param name="modifyBy"></param>
        /// <param name="destineNumber"></param>
        /// <param name="oddId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static Result RetrieveCardList(string retrieveId, string status, string agent, string modifyBy, string destineNumber, string oddId, string page)
        {
            DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_Retrieve join m in db.V_M_TM_SYS_BaseData_supplier on n.Agent equals m.SupplierCode into d from ds in d.DefaultIfEmpty() orderby n.CreateTime descending select new { RetrieveId = n.RetrieveId, Status = n.Status, ReserveNumber = n.ReserveNumber,OddId=from p in db.TM_Card_Produce where n.OddId==p.OddId select p.OddIdNo,AgentCode=n.Agent, Agent = ds.SupplierName, CreateBy = from m in db.TM_AUTH_User where m.UserID == n.CreateBy select m.UserName, CreateTime = n.CreateTime,OddIdNo=n.OddIdNo };
                if (string.IsNullOrEmpty(retrieveId) == false)
                {
                    long oddIdNo = Convert.ToInt64(retrieveId);
                    query = query.Where(n => n.OddIdNo == oddIdNo);
                }
                if (string.IsNullOrEmpty(status) == false)
                {
                    query = query.Where(n => n.Status == status);
                }
                if (string.IsNullOrEmpty(agent) == false)
                {
                    query = query.Where(n => n.AgentCode == agent);
                }
                if (string.IsNullOrEmpty(modifyBy) == false)
                {
                    DateTime time = Convert.ToDateTime(modifyBy);
                    DateTime afterTime = time.AddHours(+24);
                    query = query.Where(n => n.CreateTime >= time && n.CreateTime < afterTime);
                }
                if (string.IsNullOrEmpty(destineNumber) == false)
                {
                    int num = Convert.ToInt32(destineNumber);
                    query = query.Where(n => n.ReserveNumber == num);
                }
                if (string.IsNullOrEmpty(oddId) == false)
                {
                    long oddIdNo = Convert.ToInt64(oddId);
                    query = query.Where(n => n.OddIdNo == oddIdNo);
                }
                return new Result(true, "", query.ToDataTableSourceVsPage(pageInfo));
            }
        }

        /// <summary>
        /// 添加收卡
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <param name="status"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static Result RetrieveCard(string jsonParam, string status, int userName)
        {
            List<RetrieveCard> lt = JsonHelper.Deserialize<List<RetrieveCard>>(jsonParam);
            using (CRMEntities db = new CRMEntities())
            {          
                int reserveNumber = 0;
                string retrieveId = ToolsHelper.GuidtoString(Guid.NewGuid());
                string agent = "";
                string oddId = ToolsHelper.GuidtoString(Guid.NewGuid());
                long oddIdNo = 0;
                DateTime nowTime = DateTime.Today;
                DateTime afterTime = nowTime.AddDays(+1);
                var list = (from n in db.TM_Card_Retrieve where n.CreateTime >= nowTime && n.CreateTime < afterTime orderby n.OddIdNo descending select n.OddIdNo).ToList();
                if (list == null || list.Count == 0)
                {
                    oddIdNo = Distribution.GetOddIdNoByRole();
                }
                else
                {
                    oddIdNo = list[0].Value + 1;
                }
                foreach (var item in lt)
                {
                    TM_Card_RetrieveDetail NewDetail = new TM_Card_RetrieveDetail()
                    {
                        Id = ToolsHelper.GuidtoString(Guid.NewGuid()),
                        BeginCardNo = item.BeginCardNo,
                        EndCardNo = item.EndCardNo,
                        RetrieveId = retrieveId,
                        CreateBy = userName,
                        CreateTime = DateTime.Now,
                        RetrieveTypeId = Convert.ToInt32(item.Code)
                    };
                    db.TM_Card_RetrieveDetail.Add(NewDetail);
                    reserveNumber += Convert.ToInt32(item.RetrieveNum);
                    agent = item.Agent;
                    oddId = item.OddId;
                }
                TM_Card_Retrieve NewCard = new TM_Card_Retrieve()
                {
                    RetrieveId = retrieveId,
                    Agent = agent,
                    OddId = oddId,
                    ReserveNumber = reserveNumber,
                    Status = status,             
                    CreateBy = userName,
                    CreateTime = DateTime.Now,
                    OddIdNo=oddIdNo
                };
                db.TM_Card_Retrieve.Add(NewCard);
                var query = db.TM_Card_Produce.Where(n => n.OddId == oddId).FirstOrDefault();
                query.ReserveNumber += reserveNumber;
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

        /// <summary>
        /// 加载定卡单单号
        /// </summary>
        /// <returns></returns>
        public static Result GetOddIdList()
        {
            using (CRMEntities db = new CRMEntities())
            {
                //var list = db.TM_Card_Produce.Where(m => m.Status == "1").Select(n => new { OddId = n.OddId }).ToList();
                var list = (from n in db.TM_Card_Produce join m in db.TM_Card_Retrieve on n.OddId equals m.OddId into d from ds in d.DefaultIfEmpty() where n.Status == "1"&&n.DestineNumber>n.ReserveNumber orderby n.CreateTime descending select  new { OddId = n.OddId,OddIdNo=n.OddIdNo }).Distinct();
                return new Result(true, "", list.ToDataTableSource());
            }
        }

        /// <summary>
        /// 加载供应商
        /// </summary>
        /// <param name="oddId"></param>
        /// <returns></returns>
        public static Result GetAgentList(string oddId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var list = from n in db.TM_Card_Produce
                           join m in db.TM_Card_ProduceDetail on n.OddId equals m.OddId into d
                           from dc in d.DefaultIfEmpty()
                           join k in db.V_M_TM_SYS_BaseData_cardType on dc.CardTypeCode equals k.CardTypeCodeBase into s
                           from ds in s.DefaultIfEmpty()
                           where n.OddId == oddId
                           select new { Agent = n.Agent, AgentName=from m in db.V_M_TM_SYS_BaseData_supplier where m.SupplierCode==n.Agent select m.SupplierName,TypeText = ds.CardTypeNameBase, TypeId = dc.CardTypeCode };
                return new Result(true, "", list.ToDataTableSource());
            }
        }


        public static Result GetProvince(string oddId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var obj = from n in db.TM_Card_Retrieve join m in db.TM_Card_Produce on n.OddId equals m.OddId into d from dc in d.DefaultIfEmpty() join k in db.V_M_TM_SYS_BaseData_company on dc.AcceptingUnit equals k.CompanyCode into s from ds in s.DefaultIfEmpty() where n.OddId == oddId select new { Province = ds.CompanyProvinceCode};
                return new Result(true, "", obj.ToDataTableSource());
            }

        }

        /// <summary>
        /// 判断卡号是否可收
        /// </summary>
        /// <param name="retrieveNum"></param>
        /// <param name="beginCardNo"></param>
        /// <param name="oddId"></param>
        /// <returns></returns>
        public static Result CheckBeginCardNo(string retrieveNum, string beginCardNo,string oddId)
        {
            using (var db = new CRMEntities())
            {               
                string provinceCode = beginCardNo.Substring(2, 2);
                ObjectParameter MemberCardEnd = new ObjectParameter("MemberCardEnd", typeof(long));
                ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 300;
                var result = db.sp_CRM_MemberRetrieveCardIsUsable(Convert.ToInt32(retrieveNum), provinceCode, Convert.ToInt64(beginCardNo), MemberCardEnd);
                if (Convert.ToInt64(MemberCardEnd.Value)==0)
                {
                    return new Result(false, "您输入的收卡数量和起始卡号没有匹配数据");
                }
                string sql = "select CardTypeCode from [dbo].[TM_Card_ProduceDetail] as n where Convert(bigint,n.BeginCardNo)<=@BeginCardNo and Convert(bigint,n.EndCardNo)>=@BeginCardNo";
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@BeginCardNo", Convert.ToInt64(beginCardNo));
                param[1] = new SqlParameter("@OddId", oddId);
                var cardType = db.Database.SqlQuery<CardType>(sql, param).FirstOrDefault();         
            
                var query = (from n in db.TM_Card_Produce join m in db.TM_Card_ProduceDetail on n.OddId equals m.OddId into d from ds in d.DefaultIfEmpty() where n.OddId == oddId && ds.CardTypeCode == cardType.CardTypeCode select new { Status = n.Status, DestineNumber = n.DestineNumber, ReserveNumber = n.ReserveNumber, MaxBeginCardNo = ds.BeginCardNo, MaxEndCardNo = ds.EndCardNo }).FirstOrDefault();
                if (query==null)
                {
                    return new Result(false, "收卡的起始卡号和定卡单号没有匹配数据");
                }
                int num = query.DestineNumber - query.ReserveNumber;
                if (Convert.ToInt32(retrieveNum) > num)
                {
                    return new Result(false, "收卡数量大于剩余定卡数量");
                };
                long paramBeginCardNo = Convert.ToInt64(beginCardNo);
                if (Convert.ToInt64(query.MaxBeginCardNo) > paramBeginCardNo || Convert.ToInt64(query.MaxEndCardNo) < paramBeginCardNo)
                {
                    return new Result(false, "收卡的起始卡号和定卡单号没有匹配数据");
                };                
                return new Result(true, MemberCardEnd.Value.ToString());
            }

        }



      
        #endregion


        
    }
}
