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
using System.Data.SqlClient;

namespace Arvato.CRM.BizLogic
{
    public static class Distribution
    {
        #region 卡申请
        /// <summary>
        /// 条件查询卡
        /// </summary>
        /// <param name="Card"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static Result ApplyCard(string id, string applyNumber, string deliverNumber, string approveNumber, string status, string executeStatus, string modifyBy, string page, int? dataGroupID = null)
        {
            DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_Apply
                            join m in db.V_M_TM_SYS_BaseData_store on n.AcceptingUnit equals m.StoreCode into d
                            from dc in d.DefaultIfEmpty()
                            orderby n.CreateTime descending
                            select new
                            {
                                Id = n.Id,
                                ExecuteStatus = n.ExecuteStatus,
                                Status = n.Status,                         
                                //AcceptingUnit = dc.StoreName,
                                ApplyNumber = n.ApplyNumber,
                                ApproveNumber = n.ApproveNumber,
                                DeliverNumber = n.DeliverNumber,
                                CreateBy = from h in db.TM_AUTH_User where h.UserID == n.CreateBy select h.UserName,
                                Channel = (from k in db.V_M_TM_SYS_BaseData_store join v in db.TM_Card_ApplyDetail on k.StoreCode equals v.Accepting select k.ChannelNameStore).FirstOrDefault(),                    
                                CreateTime = n.CreateTime,
                                OddIdNo = n.OddIdNo
                            };
                if (string.IsNullOrEmpty(id) == false)
                {
                    //long oddIdNo = Convert.ToInt64(id);
                    query = query.Where(n => SqlFunctions.StringConvert((decimal)n.OddIdNo, 14).Contains(id));
                }
                if (string.IsNullOrEmpty(executeStatus) == false)
                {
                    query = query.Where(n => n.ExecuteStatus == executeStatus);
                }
                if (string.IsNullOrEmpty(status) == false)
                {
                    query = query.Where(n => n.Status == status);
                }
                if (string.IsNullOrEmpty(applyNumber) == false)
                {
                    int num = Convert.ToInt32(applyNumber);
                    query = query.Where(n => n.ApplyNumber == num);
                }
                if (string.IsNullOrEmpty(deliverNumber) == false)
                {
                    int num = Convert.ToInt32(deliverNumber);
                    query = query.Where(n => n.DeliverNumber == num);
                }
                if (string.IsNullOrEmpty(approveNumber) == false)
                {
                    int num = Convert.ToInt32(approveNumber);
                    query = query.Where(n => n.ApproveNumber == num);
                }
                if (string.IsNullOrEmpty(modifyBy) == false)
                {
                    DateTime time = Convert.ToDateTime(modifyBy);
                    DateTime afterTime = time.AddHours(+24);
                    query = query.Where(n => n.CreateTime >= time && n.CreateTime < afterTime);
                }
                return new Result(true, "", query.ToDataTableSourceVsPage(pageInfo));
            }
        }


        /// <summary>
        /// 加载门店
        /// </summary>
        /// <returns></returns>
        public static Result LoadApplyStore()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var list = db.V_M_TM_SYS_BaseData_store.Select(n => new { StoreCode = n.StoreCode, StoreName = n.StoreName });
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
                var query = from n in db.V_M_TM_SYS_BaseData_cardType where n.CardTypeCodeBase != "2" select new { Code = n.CardTypeCodeBase, Name = n.CardTypeNameBase };
                return new Result(true, "", query.ToDataTableSource());
            }
        }

        /// <summary>
        /// 加载用途
        /// </summary>
        /// <returns></returns>
        public static Result LoadPurpose()
        {
            string type = "CardPurpose";
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TD_SYS_BizOption where n.OptionType == type select new { OptionValue = n.OptionValue, OptionText = n.OptionText };
                return new Result(true, "", query.ToDataTableSource());
            }
        }

        /// <summary>
        /// 添加卡组
        /// </summary>
        /// <param name="param"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static Result AddCard(string param, int userName)
        {
            List<ApplyCard> lt = JsonHelper.Deserialize<List<ApplyCard>>(param);
            using (CRMEntities db = new CRMEntities())
            {
                string id = ToolsHelper.GuidtoString(Guid.NewGuid());
                string arriveTime = "";
                int mathRule = 0;
                int applyNum = 0;
                //string acceptingUnit = "";
                long oddIdNo = 0;
                DateTime nowTime = DateTime.Today;
                DateTime afterTime = nowTime.AddDays(+1);
                var list = (from n in db.TM_Card_Apply where n.CreateTime >= nowTime && n.CreateTime < afterTime orderby n.OddIdNo descending select n.OddIdNo).ToList();
                if (list == null || list.Count == 0)
                {
                    oddIdNo = GetOddIdNoByRole();
                }
                else
                {
                    oddIdNo = list[0].Value + 1;
                }
                foreach (var item in lt)
                {
                    TM_Card_ApplyDetail NewDetail = new TM_Card_ApplyDetail()
                    {
                        Id = ToolsHelper.GuidtoString(Guid.NewGuid()),
                        ApplyNumber = Convert.ToInt32(item.ApplyNumber),
                        Purpose = item.Purpose,
                        CreateBy = userName,
                        CreateTime = DateTime.Now,
                        OddId = id,
                        CardTypeId = Convert.ToInt32(item.Code),
                        Accepting=item.AcceptingUnit
                    };
                    applyNum += Convert.ToInt32(item.ApplyNumber);
                    //acceptingUnit = item.AcceptingUnit;
                    arriveTime = item.ArriveTime;
                    db.TM_Card_ApplyDetail.Add(NewDetail);
                }
                TM_Card_Apply NewCard = new TM_Card_Apply()
                {
                    Id = id,
                    ExecuteStatus = "0",
                    AcceptingUnit = "",
                    ApplyNumber = applyNum,
                    Status = "0",
                    ApproveNumber = 0,
                    MathRule = mathRule,
                    ArriveTime = Convert.ToDateTime(arriveTime),
                    DeliverNumber = 0,
                    CreateBy = userName,
                    CreateTime = DateTime.Now,
                    OddIdNo = oddIdNo
                };
                db.TM_Card_Apply.Add(NewCard);
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
        /// 条件查询导出Excel
        /// </summary>
        /// <param name="txtExcelOddNumbers"></param>
        /// <param name="txtExcelExecuteStatus"></param>
        /// <param name="txtExcelStatus"></param>
        /// <param name="txtExcelApplyNumber"></param>
        /// <param name="txtExcelApproveNumber"></param>
        /// <param name="txtExcelDeliverNumber"></param>
        /// <param name="txtExcelCreateTime"></param>
        /// <returns></returns>
        public static Result ApplyCardToExcel(string txtExcelOddNumbers, string txtExcelExecuteStatus, string txtExcelStatus, string txtExcelApplyNumber, string txtExcelApproveNumber, string txtExcelDeliverNumber, string txtExcelCreateTime)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_Apply
                            //join m in db.V_M_TM_SYS_BaseData_store on n.AcceptingUnit equals m.StoreCode into d
                            //from dc in d.DefaultIfEmpty()
                            orderby n.CreateTime descending
                            select new
                            {                           
                                ExecuteStatus = (from p in db.TD_SYS_BizOption where p.OptionType == "DeliveryStatus" && p.OptionValue == n.ExecuteStatus select p.OptionText).FirstOrDefault(),
                                Status = (from r in db.TD_SYS_BizOption where r.OptionType == "ApproveStatus" && r.OptionValue == n.Status select r.OptionText).FirstOrDefault(),
                                //AcceptingUnit = (from k in db.V_M_TM_SYS_BaseData_store where n.AcceptingUnit == k.StoreCode select k.StoreName).FirstOrDefault(),

                                Channel = (from k in db.V_M_TM_SYS_BaseData_store join v in db.TM_Card_ApplyDetail on k.StoreCode equals v.Accepting select k.ChannelNameStore).FirstOrDefault(),                                
                                ApplyNumber = n.ApplyNumber,
                                ApproveNumber = n.ApproveNumber,
                                DeliverNumber = n.DeliverNumber,
                                CreateBy = (from h in db.TM_AUTH_User where h.UserID == n.CreateBy select h.UserName).FirstOrDefault(),
                                CreateTime = n.CreateTime,
                                OddIdNo = n.OddIdNo
                            };
                if (string.IsNullOrEmpty(txtExcelOddNumbers) == false)
                {
                    long oddIdNo = Convert.ToInt64(txtExcelOddNumbers);
                    query = query.Where(n => n.OddIdNo == oddIdNo);
                }
                if (string.IsNullOrEmpty(txtExcelExecuteStatus) == false)
                {
                    query = query.Where(n => n.ExecuteStatus == txtExcelExecuteStatus);
                }
                if (string.IsNullOrEmpty(txtExcelStatus) == false)
                {
                    query = query.Where(n => n.Status == txtExcelStatus);
                }
                if (string.IsNullOrEmpty(txtExcelApplyNumber) == false)
                {
                    int num = Convert.ToInt32(txtExcelApplyNumber);
                    query = query.Where(n => n.ApplyNumber == num);
                }
                if (string.IsNullOrEmpty(txtExcelDeliverNumber) == false)
                {
                    int num = Convert.ToInt32(txtExcelDeliverNumber);
                    query = query.Where(n => n.DeliverNumber == num);
                }
                if (string.IsNullOrEmpty(txtExcelApproveNumber) == false)
                {
                    int num = Convert.ToInt32(txtExcelApproveNumber);
                    query = query.Where(n => n.ApproveNumber == num);
                }
                if (string.IsNullOrEmpty(txtExcelCreateTime) == false)
                {
                    DateTime time = Convert.ToDateTime(txtExcelCreateTime);
                    DateTime afterTime = time.AddHours(+24);
                    query = query.Where(n => n.CreateTime >= time && n.CreateTime < afterTime);
                }
                return new Result(true, "", query.ToList());
            }
        }
        #endregion

        #region 卡申请批准
        /// <summary>
        /// 审核查询
        /// </summary>
        /// <param name="oddNumbers"></param>
        /// <param name="applyNumber"></param>
        /// <param name="deliverNumber"></param>
        /// <param name="modifyBy"></param>
        /// <param name="status"></param>
        /// <param name="executeStatus"></param>
        /// <param name="page"></param>
        /// <param name="dataGroupID"></param>
        /// <returns></returns>
        public static Result GetVerifyList(string id, string applyNumber, string deliverNumber, string modifyBy, string status, string executeStatus, string acceptingUnit, string page, int? dataGroupID = null)
        {
            DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_Apply
                            join m in db.V_M_TM_SYS_BaseData_store on n.AcceptingUnit equals m.StoreCode into d
                            from dc in d.DefaultIfEmpty()
                            orderby n.CreateTime descending
                            select new
                            {
                                Id = n.Id,
                                ExecuteStatus = n.ExecuteStatus,
                                Status = n.Status,
                                AcceptingCode=n.AcceptingUnit,
                                AcceptingUnit = (from k in db.V_M_TM_SYS_BaseData_store join v in db.TM_Card_ApplyDetail on k.StoreCode equals v.Accepting select k.ChannelNameStore).FirstOrDefault(),
                                ApplyNumber = n.ApplyNumber,
                                ApproveNumber = n.ApproveNumber,
                                DeliverNumber = n.DeliverNumber,
                                CreateBy = from h in db.TM_AUTH_User where h.UserID == n.CreateBy select h.UserName,
                                CreateTime = n.CreateTime,
                                OddIdNo = n.OddIdNo
                            };
                if (string.IsNullOrEmpty(id) == false)
                {
                    //    long Id = Convert.ToInt64(id);
                    query = query.Where(n => SqlFunctions.StringConvert((decimal)n.OddIdNo, 14).Contains(id));
                }
                if (string.IsNullOrEmpty(executeStatus) == false)
                {
                    query = query.Where(n => n.ExecuteStatus == executeStatus);
                }
                if (string.IsNullOrEmpty(status) == false)
                {
                    query = query.Where(n => n.Status == status);
                }
                if (string.IsNullOrEmpty(applyNumber) == false)
                {
                    int num = Convert.ToInt32(applyNumber);
                    query = query.Where(n => n.ApplyNumber == num);
                }
                if (string.IsNullOrEmpty(deliverNumber) == false)
                {
                    int num = Convert.ToInt32(deliverNumber);
                    query = query.Where(n => n.DeliverNumber == num);
                }
                if (string.IsNullOrEmpty(modifyBy) == false)
                {
                    DateTime time = Convert.ToDateTime(modifyBy);
                    DateTime beforeTime = time.AddDays(-1);
                    DateTime afterTime = time.AddDays(+1);
                    query = query.Where(n => n.CreateTime > beforeTime && n.CreateTime < afterTime);
                }
                if (string.IsNullOrEmpty(acceptingUnit) == false)
                {
                    query = query.Where(n => n.AcceptingCode == acceptingUnit);
                }
                return new Result(true, "", query.ToDataTableSourceVsPage(pageInfo));
            }
        }


        /// <summary>
        /// 执行审核
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static Result ExecuteVerify(string jsonParam, int userName)
        {
            List<ExecuteVerify> lt = JsonHelper.Deserialize<List<ExecuteVerify>>(jsonParam);
            using (CRMEntities db = new CRMEntities())
            {
                foreach (var item in lt)
                {
                    var current = db.TM_Card_Apply.Where(n => n.Id == item.Id).FirstOrDefault();
                    current.Status = item.Status;
                    current.ApproveNumber = current.ApplyNumber;
                    current.ModifyBy = userName;
                    current.ModifyTime = DateTime.Now;
                }
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.ToString();
                    return new Result(false, msg);
                }
                return new Result(true);
            }
        }

        #endregion

        #region 总部卡领出
        /// <summary>
        /// 卡领出列表
        /// </summary>
        /// <param name="oddId"></param>
        /// <param name="status"></param>
        /// <param name="acceptingUnit"></param>
        /// <param name="boxNum"></param>
        /// <param name="modifyBy"></param>
        /// <param name="page"></param>
        /// <param name="dataGroupID"></param>
        /// <returns></returns>
        public static Result GetCardOutTitleList(string oddId, string status, string acceptingUnit, string boxNum, string modifyBy, string page, int? dataGroupID = null)
        {
            DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_CardOutTitle join m in db.V_M_TM_SYS_BaseData_channel on n.AcceptintUnit equals m.ChannelCodeBase into d from ds in d.DefaultIfEmpty() orderby n.CreateTime descending select new { OddId = n.OddId, Status = n.Status, SendingUnit = n.SendingUnit, AcceptintUnit = ds.ChannelNameBase, AcceptUnitCode = ds.ChannelNameBase, BoxNumber = n.BoxNumber, CreateBy = from h in db.TM_AUTH_User where h.UserID == n.CreateBy select h.UserName, CreateTime = n.CreateTime, OddIdNo = n.OddIdNo };
                if (string.IsNullOrEmpty(oddId) == false)
                {
                    long oddIdNo = Convert.ToInt64(oddId);
                    query = query.Where(n => n.OddIdNo == oddIdNo);
                }
                if (string.IsNullOrEmpty(boxNum) == false)
                {
                    int box = Convert.ToInt32(boxNum);
                    query = query.Where(n => n.BoxNumber == box);
                }
                if (string.IsNullOrEmpty(acceptingUnit) == false)
                {
                    query = query.Where(n => n.AcceptUnitCode == acceptingUnit);
                }
                if (string.IsNullOrEmpty(status) == false)
                {
                    query = query.Where(n => n.Status == status);
                }
                if (string.IsNullOrEmpty(modifyBy) == false)
                {
                    DateTime time = Convert.ToDateTime(modifyBy);
                    DateTime afterTime = time.AddHours(+24);
                    query = query.Where(n => n.CreateTime >= time && n.CreateTime < afterTime);
                }
                return new Result(true, "", query.ToDataTableSourceVsPage(pageInfo));
            }
        }

        /// <summary>
        ///  加载总部卡领出订单单号
        /// </summary>
        /// <returns></returns>
        public static Result GetOddIdList()
        {
            using (CRMEntities db = new CRMEntities())
            {
                long oddIdNo = 0;
                DateTime nowTime = DateTime.Today;
                DateTime afterTime = nowTime.AddDays(+1);
                var list = (from n in db.TM_Card_CardOutTitle where n.CreateTime >= nowTime && n.CreateTime < afterTime orderby n.OddIdNo descending select n.OddIdNo).ToList();
                if (list == null || list.Count == 0)
                {
                    oddIdNo = Distribution.GetOddIdNoByRole();
                }
                else
                {
                    oddIdNo = list[0].Value + 1;
                }
                return new Result(true, oddIdNo.ToString());
            }
        }

        /// <summary>
        /// 查询可领出盒号
        /// </summary>
        /// <param name="acceptingUnit"></param>
        /// <returns></returns>
        public static Result ChooseOddId(string acceptingUnit)
        {
            using (CRMEntities db = new CRMEntities())
            {
                //var list = db.Tm_Card_CardBox.Where(n => n.AcceptingUnit == acceptingUnit&&(n.IsOut==null || n.IsOut==false)).OrderBy(m=>m.BoxNo).Select(m => new { BoxNo = m.BoxNo });
                var list = (from n in db.Tm_Card_CardBox join m in db.TM_Card_Made on n.BatchNo equals m.BatchNo into d from ds in d.DefaultIfEmpty() where n.AcceptingUnit == acceptingUnit && n.IsOut == false && ds.Status == "1" select new { BoxNo = n.BoxNo }).Distinct();
                return new Result(true, "", list.ToDataTableSource());
            }
        }


        /// <summary>
        /// 卡申请单号条件查询盒号
        /// </summary>
        /// <param name="oddId"></param>
        /// <returns></returns>
        public static Result ChooseOddId1(string oddId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_Card_Apply.Where(n => n.Id == oddId).Select(m => m.AcceptingUnit).FirstOrDefault();
                var list = (from n in db.Tm_Card_CardBox join m in db.TM_Card_Made on n.BatchNo equals m.BatchNo into d from ds in d.DefaultIfEmpty() where n.IsOut == false && ds.Status == "1" && n.AcceptingUnit == query select new { BoxNo = n.BoxNo, AcceptingUnit = n.AcceptingUnit, ApplyId = oddId }).Distinct();
                return new Result(true, "", list.ToDataTableSource());
            }
        }

        /// <summary>
        /// 添加总部卡领出
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <param name="status"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Result AddTitleCard(string jsonParam, string status, int userId)
        {
            List<CardOutTitleInfo> lt = JsonHelper.Deserialize<List<CardOutTitleInfo>>(jsonParam);
            using (CRMEntities db = new CRMEntities())
            {
                string applyId = lt[0].ApplyId;
                int boxNum = 0;
                string titleId = ToolsHelper.GuidtoString(Guid.NewGuid());
                foreach (var item in lt)
                {
                    TM_Card_CardOutTitleDetail NewDetail = new TM_Card_CardOutTitleDetail()
                    {
                        Id = ToolsHelper.GuidtoString(Guid.NewGuid()),
                        BoxId = item.BoxNo,
                        CardNum = Convert.ToInt32(item.CardNumIn),
                        CardTypeCode = item.CardTypeCode,
                        CreateBy = userId,
                        CreateTime = DateTime.Now,
                        Purpose = item.Purpose,
                        OddId = titleId,
                        IsOut = false
                    };
                    boxNum++;
                    db.TM_Card_CardOutTitleDetail.Add(NewDetail);
                    var boxNo = db.Tm_Card_CardBox.Where(n => n.BoxNo == item.BoxNo).FirstOrDefault();
                    boxNo.IsOut = true;
                    boxNo.IsReturn = false;
                    boxNo.BoxInAddress = lt[0].AcceptingUnit;
                    var CardNoList = db.TM_Card_CardNo.Where(n => n.BoxNo == boxNo.BoxNo).ToList();
                    foreach (var Card in CardNoList)
                    {
                        Card.Status = "1";
                    }
                    if (string.IsNullOrEmpty(item.ApplyId) == false)
                    {
                        var ApplyQuery = db.TM_Card_Apply.Where(n => n.Id == applyId).FirstOrDefault();
                        int numIn = Convert.ToInt32(item.CardNumIn);
                        if (ApplyQuery.DeliverNumber + numIn > ApplyQuery.ApproveNumber)
                        {
                            return new Result(false, "领出数量超过审批数量");
                        }
                        ApplyQuery.DeliverNumber += Convert.ToInt32(item.CardNumIn);
                    }
                }
                long oddIdNo = 0;
                DateTime nowTime = DateTime.Today;
                DateTime afterTime = nowTime.AddDays(+1);
                var list = (from n in db.TM_Card_CardOutTitle where n.CreateTime >= nowTime && n.CreateTime < afterTime orderby n.OddIdNo descending select n.OddIdNo).ToList();
                if (list == null || list.Count == 0)
                {
                    oddIdNo = Distribution.GetOddIdNoByRole();
                }
                else
                {
                    oddIdNo = list[0].Value + 1;
                }
                TM_Card_CardOutTitle CardOutTitle = new TM_Card_CardOutTitle()
                {
                    AcceptintUnit = lt[0].AcceptingUnit,
                    BoxNumber = boxNum,
                    OddId = titleId,
                    Status = status,
                    SendingUnit = lt[0].SendingUnit,
                    CreateBy = userId,
                    CreateTime = DateTime.Now,
                    IsOddId = lt[0].IsOddId == "0" ? false : true,
                    OddIdNo = oddIdNo
                };
                db.TM_Card_CardOutTitle.Add(CardOutTitle);

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                    return new Result(false, "生成卡领出失败");
                }
                return new Result(true);
            }
        }

        /// <summary>
        /// 筛选盒信息
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        public static Result GetBoxInfo(string boxNo)
        {
            using (CRMEntities db = new CRMEntities())
            {
                try
                {
                    SqlParameter param = new SqlParameter("@BoxNo", boxNo);
                    //老版本
                    //string sql = "select t.BoxNo,PurposeId=(select k.OptionValue from [dbo].[TD_SYS_BizOption] as k where OptionType='CardPurpose' and k.OptionValue=t.BoxPurposeId ),Purpose=(select k.OptionText from [dbo].[TD_SYS_BizOption] as k where OptionType='CardPurpose' and k.OptionValue=t.BoxPurposeId  ),CardTypeName=(select z.CardTypeNameBase from [dbo].[V_M_TM_SYS_BaseData_cardType] as z where z.CardTypeCodeBase=1),CardTypeCode=1,CardNumIn=t.CardNumIn,BeginCardNo=t.BeginCardNo,EndCardNo=t.EndCardNo,AcceptingUnit=t.AcceptingUnit from [dbo].[Tm_Card_CardBox] as t where t.boxno=@BoxNo";

                    //新版本
                    string sql = @"select t.BoxNo,PurposeId=(select k.OptionValue from [dbo].[TD_SYS_BizOption] as k where OptionType='CardPurpose' and k.OptionValue=t.BoxPurposeId ),Purpose=(select k.OptionText from [dbo].[TD_SYS_BizOption] as k where OptionType='CardPurpose' and k.OptionValue=t.BoxPurposeId  ),CardTypeName=(select z.CardTypeNameBase from [dbo].[V_M_TM_SYS_BaseData_cardType] as z where z.CardTypeCodeBase=1),CardTypeCode=1,CardNumIn=(select count(0) from [dbo].[TM_Card_CardNo] where status!=2 and boxno=t.boxno),BeginCardNo=t.BeginCardNo,EndCardNo=t.EndCardNo,AcceptingUnit=t.boxinaddress from [dbo].[Tm_Card_CardBoxNew] as t where t.boxno=@BoxNo";
                    var BoxInfo = db.Database.SqlQuery<QueryBoxInfo>(sql, param).ToList();
                    return new Result(true, "", BoxInfo.ToDataTableSource());
                }
                catch (Exception ex)
                {
                    Log4netHelper.WriteErrorLog("GetBoxInfo : " + ex.ToString());
                    throw;
                }

            }
        }

        /// <summary>
        /// 查询申请单单号
        /// </summary>
        /// <returns></returns>
        public static Result LoadApplyId()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var list = db.TM_Card_Apply.Select(n => new { OddId = n.Id, Status = n.Status, OddIdNo = n.OddIdNo });
                return new Result(true, "", list.ToDataTableSource());
            }
        }

        #endregion

        #region 卡领入
        /// <summary>
        /// 卡领入列表
        /// </summary>
        /// <param name="oddId"></param>
        /// <param name="boxNum"></param>
        /// <param name="status"></param>
        /// <param name="modifyBy"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static Result GetCardInList(string oddId, string boxNum, string status, string modifyBy, string page)
        {
            DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_CardOutTitle join m in db.V_M_TM_SYS_BaseData_channel on n.AcceptintUnit equals m.ChannelCodeBase into d from ds in d.DefaultIfEmpty() orderby n.CreateTime descending select new { OddId = n.OddId, Status = n.Status, SendingUnit = n.SendingUnit, AcceptintUnit = ds.ChannelNameBase, AcceptingUnitCode = ds.ChannelNameBase, BoxNumber = n.BoxNumber, CreateBy = from h in db.TM_AUTH_User where h.UserID == n.CreateBy select h.UserName, CreateTime = n.CreateTime, OddIdNo = n.OddIdNo };
                if (string.IsNullOrEmpty(oddId) == false)
                {
                    long oddIdNo = Convert.ToInt64(oddId);
                    query = query.Where(n => n.OddIdNo == oddIdNo);
                }
                if (string.IsNullOrEmpty(boxNum) == false)
                {
                    int num = Convert.ToInt32(boxNum);
                    query = query.Where(n => n.BoxNumber == num);
                }
                if (string.IsNullOrEmpty(status) == false)
                {
                    query = query.Where(n => n.Status == status);
                }
                if (string.IsNullOrEmpty(modifyBy) == false)
                {
                    DateTime time = Convert.ToDateTime(modifyBy);
                    DateTime afterTime = time.AddHours(+24);
                    query = query.Where(n => n.CreateTime >= time && n.CreateTime < afterTime);
                }
                return new Result(true, "", query.ToDataTableSourceVsPage(pageInfo));

            }
        }



        #endregion

        #region 分公司卡领出
        /// <summary>
        /// 分公司卡领出列表
        /// </summary>
        /// <param name="oddId"></param>
        /// <param name="status"></param>
        /// <param name="acceptingUnit"></param>
        /// <param name="boxNum"></param>
        /// <param name="modifyBy"></param>
        /// <param name="page"></param>
        /// <param name="dataGroupID"></param>
        /// <returns></returns>
        public static Result GetCardOutBranchList(string oddId, string status, string acceptingUnit, string boxNum, string modifyBy, string boxNo, string page, int? dataGroupID = null)
        {
            try
            {
                DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);
                using (CRMEntities db = new CRMEntities())
                {
                    var query = from n in db.TM_Card_CardOutBranch select new { OddId = n.OddId, Status = n.Status, SendingUnit = from p in db.V_M_TM_SYS_BaseData_channel where p.ChannelCodeBase == n.SendingUnit select p.ChannelNameBase, BoxNumber = n.BoxNumber, CreateBy = from h in db.TM_AUTH_User where h.UserID == n.CreateBy select h.UserName, CreateTime = n.CreateTime, OddIdNo = n.OddIdNo };
                    if (string.IsNullOrEmpty(oddId) == false)
                    {
                        //long oddIdNo = Convert.ToInt64(oddId);
                        query = query.Where(n => SqlFunctions.StringConvert((decimal)n.OddIdNo, 14).Contains(oddId));
                    }
                    if (string.IsNullOrEmpty(boxNum) == false)
                    {
                        int box = Convert.ToInt32(boxNum);
                        query = query.Where(n => n.BoxNumber == box);
                    }
                    //if (string.IsNullOrEmpty(acceptingUnit) == false)
                    //{
                    //    query = query.Where(n => n.AcceptingUnitCode == acceptingUnit);
                    //}
                    if (string.IsNullOrEmpty(status) == false)
                    {
                        query = query.Where(n => n.Status == status);
                    }
                    if (string.IsNullOrEmpty(modifyBy) == false)
                    {
                        DateTime time = Convert.ToDateTime(modifyBy);
                        DateTime afterTime = time.AddHours(+24);
                        query = query.Where(n => n.CreateTime >= time && n.CreateTime < afterTime);
                    }
                    if (string.IsNullOrEmpty(boxNo) == false)
                    {
                        var tempOddID = (from n in db.TM_Card_CardOutBranchDetail where n.BoxId == boxNo select n.OddId).FirstOrDefault();
                        query = query.Where(n => n.OddId == tempOddID);
                    }
                    return new Result(true, "", query.ToDataTableSourceVsPage(pageInfo));
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("GetCardOutBranchList: " + ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// 查询订单列表
        /// </summary>
        /// <returns></returns>
        public static Result GetBranchOddIdList()
        {
            using (CRMEntities db = new CRMEntities())
            {
                var list = db.TM_Card_CardOutTitle.Select(n => new { OddId = n.OddId });
                return new Result(true, "", list.ToDataTableSource());
            }
        }

        /// <summary>
        /// 门店code条件查询盒号
        /// </summary>
        /// <param name="acceptingUnit"></param>
        /// <returns></returns>
        public static Result ChooseBranchOddId(string acceptingUnit)
        {
            using (CRMEntities db = new CRMEntities())
            {
                //var list = (from n in db.TM_Card_CardOutTitleDetail join m in db.TM_Card_CardOutTitle on n.OddId equals m.OddId into d from ds in d.DefaultIfEmpty() where ds.AcceptintUnit == acceptingUnit && n.IsOut == false && ds.Status == "1" select new { BoxNo = n.BoxId }).Distinct();
                //return new Result(true, "", list.ToDataTableSource());
                var query = (from n in db.TM_Card_CardOutTitle join m in db.TM_Card_CardOutTitleDetail on n.OddId equals m.OddId into d from dc in d.DefaultIfEmpty() join g in db.Tm_Card_CardBoxNew on dc.BoxId equals g.BoxNo select new { BoxNo = g.BoxNo, AcceptingUnit = n.AcceptintUnit, IsReturn = g.IsReturn, BoxInAddress = g.BoxInAddress, IsOut = g.IsOut }).Distinct();
                query = query.Where(n => (n.IsReturn == false ? n.AcceptingUnit == acceptingUnit : n.IsOut == false) && n.BoxInAddress == acceptingUnit);
                return new Result(true, "", query.ToDataTableSource());
            }
        }

        public static Result ChooseBranchOddId1(string oddId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var list = (from n in db.TM_Card_CardOutTitleDetail join m in db.TM_Card_CardOutTitle on n.OddId equals m.OddId into d from ds in d.DefaultIfEmpty() where ds.OddId == oddId && n.IsOut == false && ds.Status == "1" select new { BoxNo = n.BoxId }).Distinct();
                return new Result(true, "", list.ToDataTableSource());
            }
        }



        /// <summary>
        /// 添加分公司卡领出
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <param name="status"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Result AddBranchCard(string jsonParam, string status, int userId)
        {
            try
            {
                List<CardOutBranchInfo> lt = JsonHelper.Deserialize<List<CardOutBranchInfo>>(jsonParam);
                using (CRMEntities db = new CRMEntities())
                {
                    string branchId = ToolsHelper.GuidtoString(Guid.NewGuid());
                    int boxNum = 0;
                    foreach (var item in lt)
                    {
                        //总部卡领出详情表的IsOut赋值
                        var boxNo = db.TM_Card_CardOutTitleDetail.Where(n => n.BoxId == item.BoxNo).FirstOrDefault();
                        boxNo.IsOut = true;

                        //盒表赋值
                        var boxNo1 = db.Tm_Card_CardBoxNew.Where(m => m.BoxNo == item.BoxNo).FirstOrDefault();
                        boxNo1.IsReturn = false;
                        boxNo1.IsOut = true;
                        boxNo1.BoxInAddress = item.AcceptingShoppe;

                        //卡表赋值
                        var cardInfoList = db.TM_Card_CardNo.Where(n => n.BoxNo == item.BoxNo);
                        int reCardNum = 0;
                        foreach (var card in cardInfoList)
                        {
                            if ((card.Status == "1" && card.IsSalesStatus == 0) || (card.Status == "0" && card.IsSalesStatus == 0))
                            {
                                reCardNum++;
                                card.Status = "1";
                                card.IsReturn = false;
                                card.StoreCode = item.AcceptingShoppe;
                            }
                        }

                        TM_Card_CardOutBranchDetail NewDetail = new TM_Card_CardOutBranchDetail()
                        {
                            Id = ToolsHelper.GuidtoString(Guid.NewGuid()),
                            BoxId = item.BoxNo,
                            //CardNum = Convert.ToInt32(item.CardNumIn),
                            CardNum = reCardNum,
                            CardTypeCode = item.CardTypeCode,
                            CreateBy = userId,
                            CreateTime = DateTime.Now,
                            Purpose = item.Purpose,
                            OddId = branchId,
                            IsOut = false,
                            AcceptingStore = item.AcceptingShoppe,
                        };
                        boxNum++;
                        db.TM_Card_CardOutBranchDetail.Add(NewDetail);
                    }

                    long oddIdNo = 0;
                    DateTime nowTime = DateTime.Today;
                    DateTime afterTime = nowTime.AddDays(+1);
                    var list = (from n in db.TM_Card_CardOutBranch where n.CreateTime >= nowTime && n.CreateTime < afterTime orderby n.OddIdNo descending select n.OddIdNo).ToList();
                    if (list == null || list.Count == 0)
                    {
                        oddIdNo = Distribution.GetOddIdNoByRole();
                    }
                    else
                    {
                        oddIdNo = list[0].Value + 1;
                    }
                    TM_Card_CardOutBranch CardOutTitle = new TM_Card_CardOutBranch()
                    {
                        AcceptintUnit = "",
                        BoxNumber = boxNum,
                        OddId = branchId,
                        Status = status,
                        SendingUnit = lt[0].SendingUnit,
                        CreateBy = userId,
                        CreateTime = DateTime.Now,
                        IsOddId = lt[0].IsOddId == "0" ? false : true,
                        OddIdNo = oddIdNo
                    };
                    db.TM_Card_CardOutBranch.Add(CardOutTitle);
                    db.SaveChanges();
                    return new Result(true);
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("AddBranchCard :" + ex.ToString());
                return new Result(false, "新增分公司卡领出失败");
                throw;
            }

        }

        /// <summary>
        /// 加载分公司卡领出单号
        /// </summary>
        /// <returns></returns>
        public static Result GetOddIdNo()
        {
            using (CRMEntities db = new CRMEntities())
            {
                long oddIdNo = 0;
                DateTime nowTime = DateTime.Today;
                DateTime afterTime = nowTime.AddDays(+1);
                var list = (from n in db.TM_Card_CardOutTitle where n.CreateTime >= nowTime && n.CreateTime < afterTime orderby n.OddIdNo descending select n.OddIdNo).ToList();
                if (list == null || list.Count == 0)
                {
                    oddIdNo = Distribution.GetOddIdNoByRole();
                }
                else
                {
                    oddIdNo = list[0].Value + 1;
                }
                return new Result(true, oddIdNo.ToString());
            }
        }

        public static Result CardOutBranchListToExcel(string txtExcelOddIdNo, string txtExcelBoxNum, string txtExcelStatus, string txtExcelAcceptingUnit, string txtExcelCreateTime, string txtExcelBoxNo)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_CardOutBranch orderby n.CreateTime descending select new { OddId = n.OddId, Status = n.Status, StatusName = (from s in db.TD_SYS_BizOption where s.OptionValue == n.Status && s.OptionType == "ApproveStatus" select s.OptionText).FirstOrDefault(), SendingUnit = (from p in db.V_M_TM_SYS_BaseData_channel where p.ChannelCodeBase == n.SendingUnit select p.ChannelNameBase).FirstOrDefault(), AcceptingUnit = (from k in db.V_M_TM_SYS_BaseData_store where k.StoreCode == n.AcceptintUnit select k.StoreName).FirstOrDefault(), AcceptingUnitCode = n.AcceptintUnit, BoxNumber = n.BoxNumber, CreateBy = from h in db.TM_AUTH_User where h.UserID == n.CreateBy select h.UserName, CreateTime = n.CreateTime, OddIdNo = n.OddIdNo, BoxNo = (from m in db.TM_Card_CardOutBranchDetail where n.OddId == m.OddId select m.BoxId).FirstOrDefault() };
                if (string.IsNullOrEmpty(txtExcelOddIdNo) == false)
                {
                    long oddIdNo = Convert.ToInt64(txtExcelOddIdNo);
                    query = query.Where(n => n.OddIdNo == oddIdNo);
                }
                if (string.IsNullOrEmpty(txtExcelBoxNum) == false)
                {
                    int box = Convert.ToInt32(txtExcelBoxNum);
                    query = query.Where(n => n.BoxNumber == box);
                }
                if (string.IsNullOrEmpty(txtExcelAcceptingUnit) == false)
                {
                    query = query.Where(n => n.AcceptingUnitCode == txtExcelAcceptingUnit);
                }
                if (string.IsNullOrEmpty(txtExcelStatus) == false)
                {
                    query = query.Where(n => n.Status == txtExcelStatus);
                }
                if (string.IsNullOrEmpty(txtExcelCreateTime) == false)
                {
                    DateTime time = Convert.ToDateTime(txtExcelCreateTime);
                    DateTime afterTime = time.AddHours(+24);
                    query = query.Where(n => n.CreateTime >= time && n.CreateTime < afterTime);
                }
                if (string.IsNullOrEmpty(txtExcelBoxNo) == false)
                {
                    query = query.Where(n => n.BoxNo == txtExcelBoxNo);
                }
                return new Result(true, "", query.ToList());
            }
        }

        #endregion

        #region 卡中心详情页

        public static Result ApplyCardPage(string queryId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_Apply
                            where n.Id == queryId
                            orderby n.CreateTime descending
                            select new
                            {
                                Id = n.Id,
                                ExecuteStatus = n.ExecuteStatus,
                                Status = n.Status,
                                ArriveTime = n.ArriveTime,
                                AcceptingUnit = from k in db.V_M_TM_SYS_BaseData_store where k.StoreCode == n.AcceptingUnit select k.StoreName,
                                ApplyNumber = n.ApplyNumber,
                                ApproveNumber = n.ApplyNumber,
                                DeliverNumber = n.DeliverNumber,
                                CreateBy = from h in db.TM_AUTH_User where h.UserID == n.CreateBy select h.UserName,
                                CreateTime = n.CreateTime,
                                OddIdNo = n.OddIdNo
                            };
                return new Result(true, "", query.ToDataTableSource());
            }
        }

        public static Result ApplyCardDetailPage(string queryId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                SqlParameter param = new SqlParameter("@OddId", queryId);
                string sql = @"select CardTypeName=(select z.CardTypeNameBase from V_M_TM_SYS_BaseData_cardType as z where z.CardTypeCodeBase=Convert(varchar,t.CardTypeId)),Purpose=(select b.OptionText from [dbo].[TD_SYS_BizOption]as b where b.OptionType='CardPurpose' and OptionValue=t.Purpose ),t.ApplyNumber,StoreName= (select  StoreName from [dbo].[V_M_TM_SYS_BaseData_store] u where u.StoreCode=t.Accepting) ,p.ApproveNumber from  [dbo].[TM_Card_ApplyDetail] as t left join [dbo].[TM_Card_Apply] as p on p.Id=t.OddId  
 where t.OddId=@OddId";
                var list = db.Database.SqlQuery<ApplyDetailInfo>(sql, param).ToList();
                return new Result(true, "", list.ToDataTableSource());
            }
        }


        public static Result CustomizePage(string queryId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_Produce join m in db.V_M_TM_SYS_BaseData_supplier on n.Agent equals m.SupplierCode into d from ds in d.DefaultIfEmpty() where n.OddId == queryId select new { OddId = n.OddId, Status = n.Status, AcceptingUnit = from k in db.V_M_TM_SYS_BaseData_channel where k.ChannelCodeBase == n.AcceptingUnit select k.ChannelNameBase, DestineNumber = n.DestineNumber, ReserveNumber = n.ReserveNumber, Agent = ds.SupplierName, CreateBy = from m in db.TM_AUTH_User where m.UserID == n.CreateBy select m.UserName, CreateTime = n.CreateTime, OddIdNo = n.OddIdNo };
                return new Result(true, "", query.ToDataTableSource());
            }

        }

        public static Result CustomizeDetailPage(string queryId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var list = from n in db.TM_Card_ProduceDetail join m in db.TM_Card_Produce on n.OddId equals m.OddId into d from ds in d.DefaultIfEmpty() where n.OddId == queryId select new { CardTypeName = from k in db.V_M_TM_SYS_BaseData_cardType where k.CardTypeCodeBase == n.CardTypeCode select k.CardTypeNameBase, CardNumIn = n.CardNum, BeginCardNo = n.BeginCardNo, EndCardNo = n.EndCardNo, RetrieveNum = ds.ReserveNumber };
                return new Result(true, "", list.ToDataTableSource());
            }
        }

        public static Result RetrievePage(string queryId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var list = from n in db.TM_Card_Retrieve join m in db.TM_Card_ProduceDetail on n.OddId equals m.OddId into d from ds in d.DefaultIfEmpty() where n.RetrieveId == queryId select new { RetrieveId = n.RetrieveId, OddId = from t in db.TM_Card_Produce where t.OddId == n.OddId select t.OddIdNo, Agent = from h in db.V_M_TM_SYS_BaseData_supplier where n.Agent == h.SupplierCode select h.SupplierName, RetrieveNum = n.ReserveNumber, Status = n.Status, CreateBy = from m in db.TM_AUTH_User where m.UserID == n.CreateBy select m.UserName, CreateTime = n.CreateTime, OddIdNo = n.OddIdNo };
                return new Result(true, "", list.ToDataTableSource());
            }
        }

        public static Result RetrieveDetailPage(string queryId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var list = from n in db.TM_Card_RetrieveDetail join m in db.TM_Card_Retrieve on n.RetrieveId equals m.RetrieveId into d from ds in d.DefaultIfEmpty() where n.RetrieveId == queryId select new { CardTypeName = from k in db.V_M_TM_SYS_BaseData_cardType where k.CardTypeCodeBase == "1" select k.CardTypeNameBase, BeginCardNo = n.BeginCardNo, EndCardNo = n.EndCardNo, RetrieveNum = ds.ReserveNumber };
                return new Result(true, "", list.ToDataTableSource());
            }
        }

        public static Result BatchProductionPage(string queryId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_Made
                            where n.CardId == queryId
                            select new
                            {
                                OddId = n.CardId,
                                Status = n.Status,
                                CardTypeName = from k in db.V_M_TM_SYS_BaseData_cardType
                                               where k.CardTypeCodeBase == n.CardTypeCode
                                               select k.CardTypeNameBase,
                                MathRule = n.MathRule,
                                Production = n.Production,
                                BeginCardNo = n.BeginCardNum,
                                EndCardNo = n.EndCardNum,
                                ArriveTime = n.ArriveTime,
                                Purpose = from t in db.Tm_Card_CardBox where t.BatchNo == n.BatchNo select new { BoxPurpose = t.BoxPurposeId },
                                BoxNo = from s in db.Tm_Card_CardBox where s.BatchNo == n.BatchNo select new { BoxNoes = s.BoxNo },
                                IsExecute = n.IsExecute,
                                LastBoxNo = db.Tm_Card_CardBox.Where(i => i.BeginCardNo == n.BeginCardNum).Max(i => i.BoxNo),
                                LastCardNo = db.Tm_Card_CardBox.Where(i => i.BeginCardNo == n.BeginCardNum).Max(i => i.EndCardNo),
                                CreateBy = from m in db.TM_AUTH_User where m.UserID == n.CreateBy select m.UserName,
                                CreateTime = n.CreateTime,
                                OddIdNo = n.OddIdNo
                            };
                return new Result(true, "", query.ToDataTableSource());
            }
        }

        public static Result BoxAndCard(string queryId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var list = from n in db.TM_Card_Made join m in db.Tm_Card_CardBox on n.BatchNo equals m.BatchNo into d from dc in d.DefaultIfEmpty() where n.CardId == queryId select new { BoxNo = dc.BoxNo, BeginCardNo = dc.BeginCardNo, EndCardNo = dc.EndCardNo };
                return new Result(true, "", list.ToDataTableSource());
            }
        }

        public static Result CardOutTitlePage(string queryId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_CardOutTitle join m in db.V_M_TM_SYS_BaseData_channel on n.AcceptintUnit equals m.ChannelCodeBase into d from ds in d.DefaultIfEmpty() where n.OddId == queryId select new { OddId = n.OddId, Status = n.Status, SendingUnit = n.SendingUnit, AcceptintUnit = ds.ChannelNameBase, BoxNumber = n.BoxNumber, CreateBy = from h in db.TM_AUTH_User where h.UserID == n.CreateBy select h.UserName, CreateTime = n.CreateTime, IsOddId = n.IsOddId, OddIdNo = n.OddIdNo };
                return new Result(true, "", query.ToDataTableSource());
            }
        }

        public static Result CardOutTitleDetailPage(string queryId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_CardOutTitleDetail join m in db.TM_Card_CardOutTitle on n.OddId equals m.OddId into d from dc in d.DefaultIfEmpty() where n.OddId == queryId select new { CardTypeName = from k in db.V_M_TM_SYS_BaseData_cardType where k.CardTypeCodeBase == "1" select k.CardTypeNameBase, Purpose = from u in db.TD_SYS_BizOption where u.OptionType == "CardPurpose" && u.OptionValue == n.Purpose select u.OptionText, CardNumIn = n.CardNum, BoxNumber = dc.BoxNumber, CardSum = db.TM_Card_CardOutTitleDetail.Where(g => g.OddId == queryId).Sum(q => q.CardNum) };
                return new Result(true, "", query.ToDataTableSource());
            }
        }

        public static Result CardOutTitleDetailPage1(string queryId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from n in db.TM_Card_CardOutTitle join m in db.TM_Card_CardOutTitleDetail on n.OddId equals m.OddId where n.OddId == queryId select new { BoxNo = m.BoxId }).ToList();
                var list = new List<CardTitleOutBoxInfo>();
                foreach (var item in query)
                {
                    var result = (from n in db.Tm_Card_CardBoxNew where n.BoxNo == item.BoxNo select new { BoxNo = n.BoxNo, BeginCardNo = n.BeginCardNo, EndCardNo = n.EndCardNo }).FirstOrDefault();
                    list.Add(new CardTitleOutBoxInfo() { BoxNo = result.BoxNo, BeginCardNo = result.BeginCardNo, EndCardNo = result.EndCardNo });
                }
                return new Result(true, "", list.ToDataTableSource());
            }
        }


        public static Result CardOutBranchPage(string queryId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_CardOutBranch join m in db.V_M_TM_SYS_BaseData_channel on n.AcceptintUnit equals m.ChannelCodeBase into d from ds in d.DefaultIfEmpty() where n.OddId == queryId select new { OddId = n.OddId, Status = n.Status, SendingUnit = from k in db.V_M_TM_SYS_BaseData_channel where k.ChannelCodeBase == n.SendingUnit select k.ChannelNameBase, AcceptintUnit = from h in db.V_M_TM_SYS_BaseData_store where h.StoreCode == n.AcceptintUnit select h.StoreName, CreateBy = from h in db.TM_AUTH_User where h.UserID == n.CreateBy select h.UserName, CreateTime = n.CreateTime, OddIdNo = n.OddIdNo };
                return new Result(true, "", query.ToDataTableSource());
            }
        }

        public static Result CardOutBranchDetailPage(string queryId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from n in db.TM_Card_CardOutBranchDetail join m in db.TM_Card_CardOutBranch on n.OddId equals m.OddId into d from dc in d.DefaultIfEmpty() where n.OddId == queryId select new { CardTypeName = from k in db.V_M_TM_SYS_BaseData_cardType where k.CardTypeCodeBase == "1" select k.CardTypeNameBase, Purpose = from u in db.TD_SYS_BizOption where u.OptionType == "CardPurpose" && u.OptionValue == n.Purpose select u.OptionText, CardNumIn = n.CardNum, BoxNumber = dc.BoxNumber, CardSum = db.TM_Card_CardOutBranchDetail.Where(g => g.OddId == queryId).Sum(q => q.CardNum) });
                return new Result(true, "", query.ToDataTableSource());
            }
        }

        public static Result CardOutBranchDetailPage1(string queryId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from n in db.TM_Card_CardOutBranch join m in db.TM_Card_CardOutBranchDetail on n.OddId equals m.OddId where n.OddId == queryId select new { BoxNo = m.BoxId }).Distinct().ToList();
                var list = new List<CardTitleOutBoxInfo>();
                foreach (var item in query)
                {
                    var result = (from n in db.Tm_Card_CardBoxNew where n.BoxNo == item.BoxNo select new { BoxNo = n.BoxNo, BeginCardNo = n.BeginCardNo, EndCardNo = n.EndCardNo, StoreName = (from p in db.V_M_TM_SYS_BaseData_store join b in db.TM_Card_CardOutBranchDetail on p.StoreCode equals b.AcceptingStore into d from ds in d.DefaultIfEmpty() where ds.BoxId == item.BoxNo select p.StoreName).FirstOrDefault() }).FirstOrDefault();
                    list.Add(new CardTitleOutBoxInfo() { BoxNo = result.BoxNo, BeginCardNo = result.BeginCardNo, EndCardNo = result.EndCardNo, StoreName = result.StoreName });
                }
                return new Result(true, "", list.ToDataTableSource());
            }
        }

        public static Result VerifyTrue(string key, string queryId, string status)
        {
            using (CRMEntities db = new CRMEntities())
            {
                db.BeginTransaction();
                if (key == "VerifyCard")
                {
                    var query = db.TM_Card_Apply.Where(n => n.Id == queryId).FirstOrDefault();
                    if (query.Status != "0")
                    {
                        return new Result(false, "当前单号已审批过");
                    }
                    query.Status = status;
                }
                else if (key == "Customize")
                {
                    var query = db.TM_Card_Produce.Where(n => n.OddId == queryId).FirstOrDefault();
                    if (query.Status != "0")
                    {
                        return new Result(false, "当前单号已审批过");
                    }
                    query.Status = status;
                }
                else if (key == "Retrieve")
                {
                    var query = db.TM_Card_Retrieve.Where(n => n.RetrieveId == queryId).FirstOrDefault();
                    if (query.Status != "0")
                    {
                        return new Result(false, "当前单号已审批过");
                    }
                    query.Status = status;
                }
                else if (key == "BatchProduction")
                {
                    var query = db.TM_Card_Made.Where(n => n.CardId == queryId).FirstOrDefault();
                    if (query.Status != "0")
                    {
                        return new Result(false, "当前单号已审批过");
                    }
                    query.Status = status;
                }
                else if (key == "CardOutTitle")
                {
                    var query = db.TM_Card_CardOutTitle.Where(n => n.OddId == queryId).FirstOrDefault();
                    if (query.Status != "0")
                    {
                        return new Result(false, "当前单号已审批过");
                    }
                    query.Status = status;
                }
                else if (key == "CardOutBranch")
                {
                    var query = db.TM_Card_CardOutBranch.Where(n => n.OddId == queryId).FirstOrDefault();
                    if (query.Status != "0")
                    {
                        return new Result(false, "当前单号已审批过");
                    }
                    query.Status = status;
                }
                else if (key == "CardRepeal")
                {
                    var query = db.TM_Card_CardRepeal.Where(n => n.OddId == queryId).FirstOrDefault();
                    if (query.Status != "0")
                    {
                        return new Result(false, "当前单号已审批过");
                    }
                    query.Status = status;
                    if (status == "1")
                    {
                        var detial = (from a in db.TM_Card_CardRepealDetail
                                      join r in db.Tm_Card_CardBoxNew
                                      on a.BoxId equals r.BoxNo into d
                                      from ds in d.DefaultIfEmpty()
                                      where a.OddId == queryId
                                      select new
                                      {
                                          a.BoxId,
                                          ds.BoxInAddress
                                      }).ToList();
                        var isreturn = detial.Count(p => p.BoxInAddress != query.SendingUnit);
                        if (isreturn > 0)
                        {
                            return new Result(false, "当前单号中存在已退领/已领出的盒号，不可审批");
                        }
                        //db.TM_Card_CardRepealDetail.Where(n => n.OddId == queryId).ToList();
                        foreach (var item in detial)
                        {

                            SqlParameter param = new SqlParameter("@BoxNo", item.BoxId);
                            string sql = @"update TM_Card_CardNo set isreturn=1 from (select * from [dbo].[Tm_Card_CardBoxNew] where boxno=@BoxNo )
									 as b where TM_Card_CardNo.cardNO>=b.BeginCardNo and TM_Card_CardNo.cardNo<=b.EndCardNo and (TM_Card_CardNo.isUsed=0 or TM_Card_CardNo.isused=null) and  TM_Card_CardNo.status!=2;";
                            sql += @"update Tm_Card_CardBoxNew set IsReturn=1,BoxInAddress='" + query.AcceptintUnit + "' where BoxNo=@BoxNo";
                            db.Database.ExecuteSqlCommand(sql, param);
                        }
                    }

                }
                else
                {
                    return new Result(false);
                }
                try
                {
                    db.SaveChanges();
                    db.Commit();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                    db.Rollback();
                    return new Result(false, msg);
                }
                return new Result(true);
            }
        }


        public static Result CardRepealPage(string queryId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_CardRepeal
                            where n.OddId == queryId
                            select new
                            {
                                OddId = n.OddId,
                                OddIdNo = n.OddIdNo,
                                Status = n.Status,
                                SendingUnit = n.SendingUnit,
                                SendingUnitBranch = from h in db.V_M_TM_SYS_BaseData_channel where h.ChannelCodeBase == n.SendingUnit select h.ChannelNameBase,
                                SendingUnitStore = from h in db.V_M_TM_SYS_BaseData_store where h.StoreCode == n.SendingUnit select h.StoreName,
                                AcceptintUnit = (from p in db.TD_SYS_BizOption where p.OptionType == "DepartMent" && p.OptionValue == n.AcceptintUnit select p.OptionText).FirstOrDefault(),
                                AcceptintUnitStore = from h in db.V_M_TM_SYS_BaseData_channel where h.ChannelCodeBase == n.AcceptintUnit select h.ChannelNameBase,
                                AcceptintUnitGroup = from h in db.V_M_TM_SYS_BaseData_supplier where h.SupplierCode == n.AcceptintUnit select h.SupplierName,
                                BoxNumber = n.BoxNumber,
                                CardNumber = n.CardNumber,
                                CreateBy = from h in db.TM_AUTH_User where h.UserID == n.CreateBy select h.UserName,
                                CreateTime = n.CreateTime,
                                RepealType = n.RepealType,
                            };
                return new Result(true, "", query.ToDataTableSource());
            }
        }

        public static Result CardRepealDetailPage(string queryId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_CardRepealDetail
                            where n.OddId == queryId
                            select new
                            {
                                CardTypeName = from k in db.V_M_TM_SYS_BaseData_cardType where k.CardTypeCodeBase == "1" select k.CardTypeNameBase,
                                Purpose = from u in db.TD_SYS_BizOption where u.OptionType == "CardPurpose" && u.OptionValue == n.Purpose select u.OptionText,
                                CardNum = n.CardNum,
                                BoxNo = n.BoxId
                            };
                return new Result(true, "", query.ToDataTableSource());
            }
        }

        public static Result CardRepealDetailPage1(string queryId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from n in db.TM_Card_CardRepealDetail
                             where n.OddId == queryId
                             select new { BoxNo = n.BoxId }).ToList();
                var list = new List<CardTitleOutBoxInfo>();
                foreach (var item in query)
                {
                    var result = (from n in db.Tm_Card_CardBoxNew where n.BoxNo == item.BoxNo select new { BoxNo = n.BoxNo, BeginCardNo = n.BeginCardNo, EndCardNo = n.EndCardNo }).FirstOrDefault();
                    list.Add(new CardTitleOutBoxInfo() { BoxNo = result.BoxNo, BeginCardNo = result.BeginCardNo, EndCardNo = result.EndCardNo });
                }
                return new Result(true, "", list.ToDataTableSource());
            }
        }
        #endregion

        #region 分公司卡退领
        public static Result GetCardRepealBranchList(string oddId, string status, string boxNo, string modifyBy, string page, int? dataGroupID = null)
        {
            try
            {
                DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);
                using (CRMEntities db = new CRMEntities())
                {
                    var query = from n in db.TM_Card_CardRepeal
                                join m in db.V_M_TM_SYS_BaseData_channel
                                    on n.SendingUnit equals m.ChannelCodeBase
                                    into d
                                from ds in d.DefaultIfEmpty()
                                select new
                                {
                                    OddId = n.OddId,
                                    Status = n.Status,
                                    SendingUnit = n.SendingUnit,
                                    SendingUnitName = ds.ChannelNameBase,
                                    AcceptintUnit = (from s in db.TD_SYS_BizOption where s.OptionType == "DepartMent" && s.OptionValue == "1" select s.OptionText).FirstOrDefault(),
                                    BoxNumber = n.BoxNumber,
                                    CardNumber = n.CardNumber,
                                    CreateBy = from h in db.TM_AUTH_User where h.UserID == n.CreateBy select h.UserName,
                                    CreateTime = n.CreateTime,
                                    RepealType = n.RepealType,
                                    OddIdNo = n.OddIdNo,
                                };
                    query = query.Where(p => p.RepealType == 1);
                    if (string.IsNullOrEmpty(oddId) == false)
                    {
                        //long oddIdNo = Convert.ToInt64(oddId);
                        query = query.Where(n => SqlFunctions.StringConvert((decimal)n.OddIdNo, 14).Contains(oddId));
                    }
                    if (string.IsNullOrEmpty(boxNo) == false)
                    {
                        var tempOddID = (from n in db.TM_Card_CardRepealDetail where n.BoxId == boxNo select n.OddId).FirstOrDefault();
                        query = query.Where(n => n.OddId == tempOddID);
                    }
                    if (string.IsNullOrEmpty(status) == false)
                    {
                        query = query.Where(n => n.Status == status);
                    }
                    if (string.IsNullOrEmpty(modifyBy) == false)
                    {
                        DateTime time = Convert.ToDateTime(modifyBy);
                        DateTime afterTime = time.AddHours(+24);
                        query = query.Where(n => n.CreateTime >= time && n.CreateTime < afterTime);
                    }
                    return new Result(true, "", query.ToDataTableSourceVsPage(pageInfo));
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("GetCardRepealBranchList : " + ex.ToString());
                throw;
            }

        }


        public static Result GetBoxReturnInfo(string boxNo)
        {
            using (CRMEntities db = new CRMEntities())
            {
                SqlParameter param = new SqlParameter("@BoxNo", boxNo);
                //string sql = "select t.BoxNo,PurposeId=(select k.OptionValue from [dbo].[TD_SYS_BizOption] as k where OptionType='CardPurpose' and k.OptionValue=t.BoxPurposeId ),Purpose=(select k.OptionText from [dbo].[TD_SYS_BizOption] as k where OptionType='CardPurpose' and k.OptionValue=t.BoxPurposeId  ),CardTypeName=(select z.CardTypeNameBase from [dbo].[V_M_TM_SYS_BaseData_cardType] as z left join [dbo].[TM_Card_ProduceDetail] as y on z.CardTypeCodeBase=y.CardTypeCode where y.BeginCardNo=t.BeginCardNo ),CardTypeCode=(select d.CardTypeCode from [dbo].[TM_Card_ProduceDetail] as d where d.BeginCardNo<=t.BeginCardNo),CardNumIn=t.CardNumIn,BeginCardNo=t.BeginCardNo,EndCardNo=t.EndCardNo,AcceptingUnit=t.AcceptingUnit from [dbo].[Tm_Card_CardBox] as t where t.boxno=@BoxNo";
                string sql = @"select t.BoxNo,
                            PurposeId=(select k.OptionValue from [dbo].[TD_SYS_BizOption] as k where OptionType='CardPurpose' and k.OptionValue=t.BoxPurposeId),
                            Purpose=(select k.OptionText from [dbo].[TD_SYS_BizOption] as k where OptionType='CardPurpose' and k.OptionValue=t.BoxPurposeId  ),
                            CardTypeName=(select z.CardTypeNameBase from [dbo].[V_M_TM_SYS_BaseData_cardType] as z where z.CardTypeCodeBase=1),
                            CanReturnCardNumber=(select count(0) from [dbo].[TM_Card_CardNo] where cardNO>=t.BeginCardNo and cardNo<=t.EndCardNo and (isUsed=0 or isused=null) and (Status='1' and IsSalesStatus=0 )),
                            CardTypeCode=1,
                            CardNumIn=t.CardNumIn,
                            BeginCardNo=t.BeginCardNo,
                            EndCardNo=t.EndCardNo,
                            AcceptingUnit=t.AcceptingUnit 
                            from [dbo].[Tm_Card_CardBoxNew] as t
                            where t.boxno=@BoxNo";
                var BoxInfo = db.Database.SqlQuery<QueryBoxInfo>(sql, param).ToList();
                return new Result(true, "", BoxInfo.ToDataTableSource());
            }
        }


        public static Result CardRepealBranchListToExcel(string txtExcelOddIdNo, string txtExcelBoxNum, string txtExcelStatus, string txtExcelCreateTime)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_CardRepeal
                            orderby n.CreateTime descending
                            select new
                            {
                                OddId = n.OddId,
                                Status = n.Status,
                                StatusName = (from s in db.TD_SYS_BizOption where s.OptionValue == n.Status && s.OptionType == "ApproveStatus" select s.OptionText).FirstOrDefault(),
                                SendingUnit = n.SendingUnit,
                                SendingUnitName = (from k in db.V_M_TM_SYS_BaseData_channel where k.ChannelCodeBase == n.SendingUnit select k.ChannelNameBase).FirstOrDefault(),
                                AcceptingUnit = n.AcceptintUnit,
                                AcceptingName = (from s in db.TD_SYS_BizOption where s.OptionType == "DepartMent" && s.OptionValue == "1" select s.OptionText).FirstOrDefault(),
                                BoxNumber = n.BoxNumber,
                                CardNumber = n.CardNumber,
                                CreateBy = (from h in db.TM_AUTH_User where h.UserID == n.CreateBy select h.UserName).FirstOrDefault(),
                                CreateTime = n.CreateTime,
                                RepealType = n.RepealType,
                                OddIdNo = n.OddIdNo
                            };
                query = query.Where(p => p.RepealType == 1);
                if (string.IsNullOrEmpty(txtExcelOddIdNo) == false)
                {
                    long oddIdNo = Convert.ToInt64(txtExcelOddIdNo);
                    query = query.Where(n => n.OddIdNo == oddIdNo);
                }
                if (string.IsNullOrEmpty(txtExcelBoxNum) == false)
                {
                    int box = Convert.ToInt32(txtExcelBoxNum);
                    query = query.Where(n => n.BoxNumber == box);
                }
                if (string.IsNullOrEmpty(txtExcelStatus) == false)
                {
                    query = query.Where(n => n.Status == txtExcelStatus);
                }
                if (string.IsNullOrEmpty(txtExcelCreateTime) == false)
                {
                    //var userId = from n in db.TM_AUTH_User where n.UserName == txtExcelCreateTime select n.UserID;
                    query = query.Where(n => n.CreateBy == txtExcelCreateTime);
                }
                return new Result(true, "", query.ToList());
            }
        }

        //老版本
        //public static Result ChooseBox(string acceptingUnit)
        //{
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        var list = db.Tm_Card_CardBox.Where(n => n.BoxInAddress == acceptingUnit && n.IsOut == true).OrderBy(m => m.BoxNo).Select(m => new { BoxNo = m.BoxNo });
        //        return new Result(true, "", list.ToDataTableSource());
        //    }
        //}


        public static Result ChooseBox(string acceptingUnit)
        {
            using (CRMEntities db = new CRMEntities())
            {
                //var list = db.Tm_Card_CardBoxNew.Where(n => n.BoxInAddress == acceptingUnit && db.TM_Card_CardNo.Where(m => m.Status == "1" && m.IsUsed == false && m.IsSalesStatus == 0 && m.BoxNo == n.BoxNo && string.IsNullOrEmpty(m.StoreCode) == false).Count() > 0).OrderBy(m => m.BoxNo).Select(m => new { BoxNo = m.BoxNo });
                List<RepealBoxNo> boxNoList = new List<RepealBoxNo>();
                var queryBoxNo = db.Tm_Card_CardBoxNew.Where(n => n.BoxInAddress == acceptingUnit);
                foreach (var box in queryBoxNo)
                {
                    var count = db.TM_Card_CardNo.Where(m => m.Status == "1" && m.IsUsed == false && m.IsSalesStatus == 0 && m.BoxNo == box.BoxNo && string.IsNullOrEmpty(m.StoreCode) == false).Count();
                    if (count > 0)
                    {
                        boxNoList.Add(new RepealBoxNo { BoxNo = box.BoxNo });
                    }
                }
                return new Result(true, "", boxNoList.ToDataTableSource());
            }
        }

        public static Result AddCardRepeal(string jsonParam, string status, int repealtype, int userId)
        {
            List<CardRepealInfo> lt = JsonHelper.Deserialize<List<CardRepealInfo>>(jsonParam);
            using (CRMEntities db = new CRMEntities())
            {
                //db.BeginTransaction();
                try
                {
                    string Id = ToolsHelper.GuidtoString(Guid.NewGuid());
                    long oddIdNo = 0;
                    DateTime nowTime = DateTime.Today;
                    DateTime afterTime = nowTime.AddDays(+1);
                    var list = (from n in db.TM_Card_CardRepeal where n.CreateTime >= nowTime && n.CreateTime < afterTime orderby n.OddIdNo descending select n.OddIdNo).ToList();
                    if (list == null || list.Count == 0)
                    {
                        oddIdNo = GetOddIdNoByRole();
                    }
                    else
                    {
                        oddIdNo = list[0].Value + 1;
                    }
                    TM_Card_CardRepeal CardRepeal = new TM_Card_CardRepeal()
                    {
                        AcceptintUnit = lt[0].SelReturnUnit == "1" ? lt[0].AcceptingUnit : "1",
                        BoxNumber = lt.Count(),
                        OddId = Id,
                        Status = status,
                        SendingUnit = lt[0].SendingUnit,
                        CreateBy = userId,
                        CreateTime = DateTime.Now,
                        CardNumber = lt.Sum(p => p.CanReturnCardNumber),
                        RepealType = repealtype,
                        OddIdNo = oddIdNo
                    };
                    db.TM_Card_CardRepeal.Add(CardRepeal);
                    foreach (var item in lt)
                    {
                        TM_Card_CardRepealDetail NewDetail = new TM_Card_CardRepealDetail()
                        {
                            Id = ToolsHelper.GuidtoString(Guid.NewGuid()),
                            BoxId = item.BoxNo,
                            CardNum = Convert.ToInt32(item.CanReturnCardNumber),
                            CardTypeCode = item.CardTypeCode,
                            CreateBy = userId,
                            CreateTime = DateTime.Now,
                            Purpose = item.Purpose,
                            OddId = Id
                        };
                        db.TM_Card_CardRepealDetail.Add(NewDetail);
                        if (status == "1")
                        {
                            string accepting = item.SelReturnUnit == "1" ? item.AcceptingUnit : "1";
                            SqlParameter param = new SqlParameter("@BoxNo", item.BoxNo);
                            string sql = @"update TM_Card_CardNo set isreturn=1 from (select * from [dbo].[Tm_Card_CardBoxNew] where boxno=@BoxNo )
									 as b where TM_Card_CardNo.cardNO>=b.BeginCardNo and TM_Card_CardNo.cardNo<=b.EndCardNo and (TM_Card_CardNo.isUsed=0 or TM_Card_CardNo.isused=null) and  TM_Card_CardNo.status!=2;";
                            sql += @"update Tm_Card_CardBoxNew set IsReturn=1,IsOut=0,BoxInAddress='" + accepting + "' where BoxNo=@BoxNo ";
                            db.Database.ExecuteSqlCommand(sql, param);

                        }
                        var cardList = db.TM_Card_CardNo.Where(n => n.BoxNo == item.BoxNo);
                        foreach (var card in cardList)
                        {
                            if (card.Status == "1" && card.IsSalesStatus == 0 && card.IsUsed == false)
                            {
                                card.StoreCode = "";
                            }
                        }
                    }
                    db.SaveChanges();
                    //db.Commit();
                }
                catch (Exception ex)
                {
                    string msg = ex.ToString();
                    Log4netHelper.WriteErrorLog("CardRepeal :" + ex.ToString());
                    db.Rollback();
                    return new Result(false, msg);
                }
                return new Result(true);
            }
        }
        #endregion

        #region 门店卡退领
        public static Result GetCardRepealStoreList(string oddId, string status, string boxNo, string modifyBy, string page, int? dataGroupID = null)
        {
            try
            {
                DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);
                using (CRMEntities db = new CRMEntities())
                {
                    var query = from n in db.TM_Card_CardRepeal
                                join m in db.V_M_TM_SYS_BaseData_channel
                                on n.AcceptintUnit equals m.ChannelCodeBase
                                into d
                                from ds in d.DefaultIfEmpty()
                                join s in db.V_M_TM_SYS_BaseData_store
                                on n.SendingUnit equals s.StoreCode
                                into a
                                from sa in a.DefaultIfEmpty()
                                select new
                                {
                                    OddId = n.OddId,
                                    Status = n.Status,
                                    SendingUnit = n.SendingUnit,
                                    SendingUnitName = sa.StoreName,
                                    AcceptintUnit = n.AcceptintUnit == "1" ? (from k in db.TD_SYS_BizOption where k.OptionType == "DepartMent" && k.OptionValue == "1" select k.OptionText).FirstOrDefault() : ds.ChannelNameBase,
                                    CardNumber = n.CardNumber,
                                    BoxNumber = n.BoxNumber,
                                    CreateBy = from h in db.TM_AUTH_User where h.UserID == n.CreateBy select h.UserName,
                                    CreateTime = n.CreateTime,
                                    RepealType = n.RepealType,
                                    OddIdNo = n.OddIdNo,
                                };
                    query = query.Where(p => p.RepealType == 0);
                    if (string.IsNullOrEmpty(oddId) == false)
                    {
                        //query = query.Where(n => n.OddId == oddId);
                        query = query.Where(n => SqlFunctions.StringConvert((decimal)n.OddIdNo, 14).Contains(oddId));
                    }
                    if (string.IsNullOrEmpty(boxNo) == false)
                    {
                        var tempOddID = (from n in db.TM_Card_CardRepealDetail where n.BoxId == boxNo select n.OddId).FirstOrDefault();
                        query = query.Where(n => n.OddId == tempOddID);
                    }
                    if (string.IsNullOrEmpty(status) == false)
                    {
                        query = query.Where(n => n.Status == status);
                    }
                    if (string.IsNullOrEmpty(modifyBy) == false)
                    {
                        DateTime time = Convert.ToDateTime(modifyBy);
                        DateTime afterTime = time.AddHours(+24);
                        query = query.Where(n => n.CreateTime >= time && n.CreateTime < afterTime);
                    }
                    return new Result(true, "", query.ToDataTableSourceVsPage(pageInfo));
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("GetCardRepealStoreList :" + ex.ToString());
                throw;
            }

        }

        public static Result CardRepealStoreListToExcel(string txtExcelOddIdNo, string txtExcelBoxNum, string txtExcelStatus, string txtExcelCreateTime)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_CardRepeal
                            orderby n.CreateTime descending
                            select new
                            {
                                OddId = n.OddId,
                                Status = n.Status,
                                StatusName = (from s in db.TD_SYS_BizOption where s.OptionValue == n.Status && s.OptionType == "ApproveStatus" select s.OptionText).FirstOrDefault(),
                                SendingUnit = n.SendingUnit,
                                SendingUnitName = (from k in db.V_M_TM_SYS_BaseData_store where k.StoreCode == n.SendingUnit select k.StoreName).FirstOrDefault(),
                                AcceptingUnit = n.AcceptintUnit,
                                AcceptingName = (from c in db.V_M_TM_SYS_BaseData_channel where c.ChannelCodeBase == n.AcceptintUnit select c.ChannelNameBase).FirstOrDefault() != null ? (from c in db.V_M_TM_SYS_BaseData_channel where c.ChannelCodeBase == n.AcceptintUnit select c.ChannelNameBase).FirstOrDefault() : (from s in db.TD_SYS_BizOption where s.OptionType == "DepartMent" && s.OptionValue == "1" select s.OptionText).FirstOrDefault(),
                                BoxNumber = n.BoxNumber,
                                CardNumber = n.CardNumber,
                                CreateBy = (from h in db.TM_AUTH_User where h.UserID == n.CreateBy select h.UserName).FirstOrDefault(),
                                CreateTime = n.CreateTime,
                                RepealType = n.RepealType,
                                OddIdNo = n.OddIdNo
                            };
                query = query.Where(p => p.RepealType == 0);
                if (string.IsNullOrEmpty(txtExcelOddIdNo) == false)
                {
                    query = query.Where(n => n.OddId == txtExcelOddIdNo);
                }
                if (string.IsNullOrEmpty(txtExcelBoxNum) == false)
                {
                    int box = Convert.ToInt32(txtExcelBoxNum);
                    query = query.Where(n => n.BoxNumber == box);
                }
                if (string.IsNullOrEmpty(txtExcelStatus) == false)
                {
                    query = query.Where(n => n.Status == txtExcelStatus);
                }
                if (string.IsNullOrEmpty(txtExcelCreateTime) == false)
                {
                    //var userId = (from n in db.TM_AUTH_User where n.UserName == txtExcelCreateTime select n.UserID);
                    query = query.Where(n => n.CreateBy == txtExcelCreateTime);
                }
                return new Result(true, "", query.ToList());
            }
        }
        #endregion

        #region 总部卡退领
        public static Result GetCardRepealGroupList(string oddId, string status, string boxNo, string modifyBy, string page, int? dataGroupID = null)
        {
            DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_CardRepeal
                            join m in db.V_M_TM_SYS_BaseData_supplier
                            on n.AcceptintUnit equals m.SupplierCode
                            into d
                            from ds in d.DefaultIfEmpty()
                            select new
                            {
                                OddId = n.OddId,
                                Status = n.Status,
                                SendingUnit = (from k in db.TD_SYS_BizOption where k.OptionType == "DepartMent" && k.OptionValue == n.SendingUnit select k.OptionText).FirstOrDefault(),
                                AcceptintUnit = n.AcceptintUnit,
                                AcceptintUnitName = ds.SupplierName,
                                CardNumber = n.CardNumber,
                                BoxNumber = n.BoxNumber,
                                CreateBy = from h in db.TM_AUTH_User where h.UserID == n.CreateBy select h.UserName,
                                CreateTime = n.CreateTime,
                                RepealType = n.RepealType,
                                OddIdNo = n.OddIdNo
                            };

                query = query.Where(p => p.RepealType == 2);
                if (string.IsNullOrEmpty(oddId) == false)
                {

                    query = query.Where(n => SqlFunctions.StringConvert((decimal)n.OddIdNo, 14).Contains(oddId));
                }
                if (string.IsNullOrEmpty(boxNo) == false)
                {
                    var tempOddID = (from n in db.TM_Card_CardRepealDetail where n.BoxId == boxNo select n.OddId).FirstOrDefault();
                    query = query.Where(n => n.OddId == tempOddID);
                }
                if (string.IsNullOrEmpty(status) == false)
                {
                    query = query.Where(n => n.Status == status);
                }
                if (string.IsNullOrEmpty(modifyBy) == false)
                {
                    DateTime time = Convert.ToDateTime(modifyBy);
                    DateTime afterTime = time.AddHours(+24);
                    query = query.Where(n => n.CreateTime >= time && n.CreateTime < afterTime);
                }
                return new Result(true, "", query.ToDataTableSourceVsPage(pageInfo));
            }
        }

        public static Result ChooseBox2(string acceptingUnit, string sendingUnit)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var BoxInfo = from n in db.Tm_Card_CardBoxNew join m in db.TM_Card_ProduceNew on n.CustomizeOddId equals m.CustomizeOddId into d from dc in d.DefaultIfEmpty() where (n.BoxInAddress == sendingUnit || n.BoxInAddress == null) && dc.Agent == acceptingUnit select new { BoxNo = n.BoxNo };
                //var BoxInfo = db.Tm_Card_CardBoxNew.Where(n=>n.BoxInAddress == sendingUnit || n.BoxInAddress == null);
                return new Result(true, "", BoxInfo.ToDataTableSource());
            }
        }

        public static Result CardRepealTitleListToExcel(string txtExcelOddIdNo, string txtExcelBoxNum, string txtExcelStatus, string txtExcelCreateTime)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_CardRepeal
                            orderby n.CreateTime descending
                            select new
                            {
                                OddId = n.OddId,
                                Status = n.Status,
                                StatusName = (from s in db.TD_SYS_BizOption where s.OptionValue == n.Status && s.OptionType == "ApproveStatus" select s.OptionText).FirstOrDefault(),
                                SendingUnit = (from s in db.TD_SYS_BizOption where s.OptionType == "DepartMent" && s.OptionValue == "1" select s.OptionText).FirstOrDefault(),
                                AcceptingUnit = n.AcceptintUnit,
                                AcceptingUnitName = (from k in db.V_M_TM_SYS_BaseData_supplier where k.SupplierCode == n.AcceptintUnit select k.SupplierName).FirstOrDefault(),
                                CardNumber = n.CardNumber,
                                BoxNumber = n.BoxNumber,
                                CreateBy = (from h in db.TM_AUTH_User where h.UserID == n.CreateBy select h.UserName).FirstOrDefault(),
                                CreateTime = n.CreateTime,
                                RepealType = n.RepealType,
                                OddIdNo = n.OddIdNo
                            };
                query = query.Where(p => p.RepealType == 2);
                if (string.IsNullOrEmpty(txtExcelOddIdNo) == false)
                {
                    query = query.Where(n => n.OddId == txtExcelOddIdNo);
                }
                if (string.IsNullOrEmpty(txtExcelBoxNum) == false)
                {
                    int box = Convert.ToInt32(txtExcelBoxNum);
                    query = query.Where(n => n.BoxNumber == box);
                }
                if (string.IsNullOrEmpty(txtExcelStatus) == false)
                {
                    query = query.Where(n => n.Status == txtExcelStatus);
                }
                if (string.IsNullOrEmpty(txtExcelCreateTime) == false)
                {
                    //var userId = from n in db.TM_AUTH_User where n.UserName == modifyBy select n.UserID;
                    query = query.Where(n => n.CreateBy == txtExcelCreateTime);
                }
                return new Result(true, "", query.ToList());
            }
        }
        #endregion

        #region 总部卡领出(新)

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="oddId"></param>
        /// <param name="status"></param>
        /// <param name="acceptingUnit"></param>
        /// <param name="boxNum"></param>
        /// <param name="queryCreateTime"></param>
        /// <param name="page"></param>
        /// <param name="dataGroupID"></param>
        /// <returns></returns>
        public static Result GetCardOutTitleNewList(string oddId, string status, string acceptingUnit, string boxNum, string createTime, string boxNo, string store, string page, int? dataGroupID = null)
        {
            DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_CardOutTitle
                            //旧版
                            //join m in db.V_M_TM_SYS_BaseData_channel on n.AcceptintUnit equals m.ChannelCodeBase into d
                            //from ds in d.DefaultIfEmpty()
                            orderby n.CreateTime descending
                            select new
                            {
                                OddId = n.OddId,
                                Status = n.Status,
                                StatusName = (from s in db.TD_SYS_BizOption where s.OptionValue == n.Status && s.OptionType == "ApproveStatus" select s.OptionText).FirstOrDefault(),
                                SendingUnit = n.SendingUnit,
                                //旧版
                                //AcceptingUnit = ds.ChannelNameBase != null ? ds.ChannelNameBase : (from t in db.V_M_TM_SYS_BaseData_store where n.AcceptintUnit == t.StoreCode select t.StoreName).FirstOrDefault(),
                                AcceptingUnit = (from t in db.TM_Card_CardOutTitle where t.OddId == n.OddId && t.AcceptintUnit == n.AcceptintUnit select t.OddId).FirstOrDefault() != null ? (from t in db.TM_Card_CardOutTitle where t.OddId == n.OddId && t.AcceptintUnit == n.AcceptintUnit select t.OddId).FirstOrDefault() : (from t in db.TM_Card_CardOutTitleDetail where t.OddId == n.OddId select t.AcceptingUnit).FirstOrDefault(),
                                //旧版
                                //AcceptUnitCode = ds.ChannelCodeBase,
                                AcceptUnitCode = "",
                                BoxNumber = n.BoxNumber,
                                CreateBy = (from h in db.TM_AUTH_User where h.UserID == n.CreateBy select h.UserName).FirstOrDefault(),
                                CreateTime = n.CreateTime,
                                OddIdNo = n.OddIdNo
                            };

                if (string.IsNullOrEmpty(oddId) == false)
                {
                    //long oddIdNo = Convert.ToInt64(oddId);
                    query = query.Where(n => SqlFunctions.StringConvert((decimal)n.OddIdNo, 14).Contains(oddId));
                }
                if (string.IsNullOrEmpty(boxNum) == false)
                {
                    int box = Convert.ToInt32(boxNum);
                    query = query.Where(n => n.BoxNumber == box);
                }
                if (string.IsNullOrEmpty(acceptingUnit) == false)
                {
                    //旧版
                    //query = query.Where(n => n.AcceptUnitCode == acceptingUnit);

                    //var list = db.TM_Card_CardOutTitleDetail.Where(m => m.AcceptingUnit == acceptingUnit).Select(m => m.OddId).Distinct();           
                    var list = (from n in db.TM_Card_CardOutTitleDetail join m in db.V_M_TM_SYS_BaseData_store on n.AcceptingUnit equals m.StoreCode into d from ds in d.DefaultIfEmpty() where ds.ChannelCodeStore == acceptingUnit select n.OddId).Distinct();
                    List<CardTitleOutQuery> cardList = new List<CardTitleOutQuery>();
                    foreach (var oddid in list)
                    {
                        foreach (var q in query)
                        {
                            if (oddid == q.OddId)
                            {
                                cardList.Add(new CardTitleOutQuery() { OddId = q.OddId, OddIdNo = q.OddIdNo.ToString(), AcceptUnitCode = q.AcceptUnitCode, BoxNumber = q.BoxNumber.ToString(), CreateBy = q.CreateBy.ToString(), CreateTime = q.CreateTime.ToString(), SendingUnit = q.SendingUnit, Status = q.Status, StatusName = q.StatusName });
                            }
                        }
                    }
                    return new Result(true, "", cardList.ToDataTableSourceVsPage(pageInfo));
                }
                if (string.IsNullOrEmpty(status) == false)
                {
                    query = query.Where(n => n.Status == status);
                }
                if (string.IsNullOrEmpty(createTime) == false)
                {
                    DateTime time = Convert.ToDateTime(createTime);
                    DateTime afterTime = time.AddHours(+24);
                    query = query.Where(n => n.CreateTime >= time && n.CreateTime < afterTime);
                }
                if (string.IsNullOrEmpty(boxNo) == false)
                {
                    var tempOddID = (from n in db.TM_Card_CardOutTitleDetail where n.BoxId == boxNo select n.OddId).FirstOrDefault();
                    query = query.Where(n => n.OddId == tempOddID);
                }
                if (string.IsNullOrEmpty(store) == false)
                {
                    //旧版
                    //var name= (from f in db.V_M_TM_SYS_BaseData_store where f.StoreCode==store select f.StoreName).FirstOrDefault();
                    //query = query.Where(n => n.AcceptingUnit == name);
                    var list = db.TM_Card_CardOutTitleDetail.Where(m => m.AcceptingUnit == store).Select(m => m.OddId).Distinct();
                    List<CardTitleOutQuery> cardList = new List<CardTitleOutQuery>();
                    foreach (var oddid in list)
                    {
                        foreach (var q in query)
                        {
                            if (oddid == q.OddId)
                            {
                                cardList.Add(new CardTitleOutQuery() { OddId = q.OddId, OddIdNo = q.OddIdNo.ToString(), AcceptUnitCode = q.AcceptUnitCode, BoxNumber = q.BoxNumber.ToString(), CreateBy = q.CreateBy.ToString(), CreateTime = q.CreateTime.ToString(), SendingUnit = q.SendingUnit, Status = q.Status, StatusName = q.StatusName });
                            }
                        }
                    }
                    return new Result(true, "", cardList.ToDataTableSourceVsPage(pageInfo));
                }
                return new Result(true, "", query.ToDataTableSourceVsPage(pageInfo));
            }
        }

        /// <summary>
        /// 详情页
        /// </summary>
        /// <param name="oddId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static Result CardOutTitleDetailsParams(string oddId, string page)
        {
            DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(page);
            using (CRMEntities db = new CRMEntities())
            {
                var list = from n in db.TM_Card_CardOutTitleDetail join m in db.TM_Card_CardOutTitle on n.OddId equals m.OddId into d from ds in d.DefaultIfEmpty() select new { OddId = n.OddId, OddIdNo = ds.OddIdNo, BoxNo = n.BoxId, Purpose = n.Purpose, PurposeName = from v in db.TD_SYS_BizOption where v.OptionType == "CardPurpose" && n.Purpose == v.OptionValue select v.OptionText, CardTypeCode = n.CardTypeCode, CardTypeName = from u in db.V_M_TM_SYS_BaseData_cardType where u.CardTypeCodeBase == n.CardTypeCode select u.CardTypeNameBase, IsOut = n.IsOut, CardNumIn = n.CardNum, AcceptingUnit = from p in db.V_M_TM_SYS_BaseData_store where p.StoreCode == n.AcceptingUnit select p.StoreName };
                if (string.IsNullOrEmpty(oddId) == false)
                {
                    list = list.Where(n => n.OddId == oddId);
                }
                return new Result(true, "", list.ToDataTableSourceVsPage(pageInfo));
            }
        }

        /// <summary>
        /// 加载门店
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public static Result LoadStore(string companyCode)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var list = from n in db.V_M_TM_SYS_BaseData_store select new { ShoppeCode = n.StoreCode, ShoppeName = n.StoreName, StoreType = n.StoreType, ChannelCodeStore = n.ChannelCodeStore };
                if (string.IsNullOrEmpty(companyCode) == false)
                {
                    if (companyCode == "直营店")
                    {
                        list = list.Where(n => n.StoreType == companyCode);
                    }
                    else
                    {
                        list = list.Where(n => n.ChannelCodeStore == companyCode);
                    }
                }
                return new Result(true, "", list.ToDataTableSource());
            }
        }

        /// <summary>
        /// 筛选盒号
        /// </summary>
        /// <param name="acceptingUnit"></param>
        /// <returns></returns>
        public static Result ChooseBoxNoByTitle(string company, string store)
        {
            string address = string.IsNullOrEmpty(company) == false ? company : store;
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from n in db.TM_Card_RetrieveNew join m in db.TM_Card_RetrieveDetailNew on n.RetrieveOddId equals m.RetrieveOddId into d from dc in d.DefaultIfEmpty() join g in db.Tm_Card_CardBoxNew on dc.BoxNo equals g.BoxNo select new { BoxNo = dc.BoxNo, CompanyCode = n.CompanyCode, StoreCode = n.StoreCode, IsReturn = g.IsReturn, BoxInAddress = g.BoxInAddress, IsOut = g.IsOut }).Distinct();
                if (string.IsNullOrWhiteSpace(company) && string.IsNullOrWhiteSpace(store))
                {
                    query = query.Where(n => (n.IsReturn == false ? true : n.IsOut == false) && (n.BoxInAddress == null || n.BoxInAddress == "1"));
                }
                if (string.IsNullOrWhiteSpace(company) == false)
                {
                    query = query.Where(n => (n.IsReturn == false ? n.CompanyCode == address : n.IsOut == false) && (n.BoxInAddress == null || n.BoxInAddress == "1"));
                }
                if (string.IsNullOrWhiteSpace(store) == false)
                {
                    query = query.Where(n => (n.IsReturn == false ? n.StoreCode == address : n.IsOut == false) && (n.BoxInAddress == null || n.BoxInAddress == "1"));
                }
                return new Result(true, "", query.ToDataTableSource());

            }
        }

        /// <summary>
        /// 门店code查询申请单号
        /// </summary>
        /// <param name="storeCode"></param>
        /// <returns></returns>
        public static Result LoadApplyOddId(string storeCode)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var list = db.TM_Card_Apply.Where(n => n.AcceptingUnit == storeCode).Select(m => new { ApplyOddId = m.Id, ApplyOddIdNo = m.OddIdNo });
                return new Result(true, "", list.ToDataTableSource());
            }
        }

        /// <summary>
        /// 盒号查询盒信息
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        public static Result GetBoxInfoNew(string boxNo)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.Tm_Card_CardBoxNew join k in db.TM_Card_RetrieveDetailNew on n.BoxNo equals k.BoxNo into p from dp in p.DefaultIfEmpty() where n.BoxNo == boxNo select new { BoxNo = n.BoxNo, CardTypeCode = dp.CardTypeCode, CardTypeName = from j in db.V_M_TM_SYS_BaseData_cardType where dp.CardTypeCode == j.CardTypeCodeBase select j.CardTypeNameBase, Purpose = n.BoxPurposeId, CardNumIn = db.TM_Card_CardNo.Where(z => ((z.Status == "1" && z.IsSalesStatus == 0) || z.Status == "0" && z.IsSalesStatus == 0) && z.BoxNo == n.BoxNo).Count() };
                return new Result(true, "", query.ToDataTableSource());
            }

        }

        /// <summary>
        /// 盒卡关联信息
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        public static Result GetBoxCardInfoNew(string boxNo)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_Card_RetrieveDetailNew.Where(n => n.BoxNo == boxNo).Select(m => new { BoxNo = m.BoxNo, BeginCardNo = m.BeginCardNo, EndCardNo = m.EndCardNo });
                return new Result(true, "", query.ToDataTableSource());
            }
        }

        /// <summary>
        /// 新增总部卡领出(旧版)
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        //public static Result AddNewCardOutTitle(string jsonParam, string status,int userId)
        //{
        //    try
        //    {
        //        List<CardOutTitleInfoNew> lt = JsonHelper.Deserialize<List<CardOutTitleInfoNew>>(jsonParam);
        //        using (CRMEntities db = new CRMEntities())
        //        {
        //            string applyId = lt[0].ApplyOddId;
        //            int boxNum = 0;
        //            string titleId = ToolsHelper.GuidtoString(Guid.NewGuid());
        //            //生成流水单号
        //            long oddIdNo = 0;
        //            DateTime nowTime = DateTime.Today;
        //            DateTime afterTime = nowTime.AddDays(+1);
        //            var list = (from n in db.TM_Card_CardOutTitle where n.CreateTime >= nowTime && n.CreateTime < afterTime orderby n.OddIdNo descending select n.OddIdNo).ToList();
        //            if (list == null || list.Count == 0)
        //            {
        //                oddIdNo = Distribution.GetOddIdNoByRole();
        //            }
        //            else
        //            {
        //                oddIdNo = list[0].Value + 1;
        //            }


        //            //领出到分公司
        //            if (string.IsNullOrWhiteSpace(lt[0].Company)==false)
        //            {
        //                foreach (var item in lt)
        //                {
        //                    var boxNo = db.Tm_Card_CardBoxNew.Where(n => n.BoxNo == item.BoxNo).FirstOrDefault();
        //                    boxNo.IsOut = true;
        //                    boxNo.IsReturn = false;
        //                    boxNo.BoxInAddress = string.IsNullOrEmpty(item.Company) ? item.Store : item.Company;
        //                    var CardNoList = db.TM_Card_CardNo.Where(n => n.BoxNo == boxNo.BoxNo);
        //                    int reCardNum = 0;
        //                    foreach (var Card in CardNoList)
        //                    {

        //                        if (Card.Status=="2")
        //                        {
        //                            continue;
        //                        }
        //                        Card.Status = "1";
        //                        reCardNum++;
        //                    }
        //                    var rev = db.TM_Card_RetrieveDetailNew.Where(n => n.BoxNo == item.BoxNo).FirstOrDefault();
        //                    rev.TakeOutBoxNumber = 1;

        //                    TM_Card_CardOutTitleDetail NewDetail = new TM_Card_CardOutTitleDetail()
        //                    {
        //                        Id = ToolsHelper.GuidtoString(Guid.NewGuid()),
        //                        BoxId = item.BoxNo,
        //                        //CardNum = Convert.ToInt32(item.CardNumIn),
        //                        CardNum = reCardNum,
        //                        CardTypeCode = item.CardTypeCode,
        //                        CreateBy = userId,
        //                        CreateTime = DateTime.Now,
        //                        Purpose = item.Purpose,
        //                        OddId = titleId,
        //                        IsOut = false,
        //                    };
        //                    boxNum++;
        //                    db.TM_Card_CardOutTitleDetail.Add(NewDetail);                       
        //                }

        //                TM_Card_CardOutTitle CardOutTitle = new TM_Card_CardOutTitle()
        //                {
        //                    AcceptintUnit = lt[0].Company,
        //                    BoxNumber = boxNum,
        //                    OddId = titleId,
        //                    Status = status,
        //                    SendingUnit = lt[0].SendUnit,
        //                    CreateBy = userId,
        //                    CreateTime = DateTime.Now,
        //                    IsOddId = false,
        //                    OddIdNo = oddIdNo
        //                };
        //                db.TM_Card_CardOutTitle.Add(CardOutTitle);
        //            }
        //            //直接领出到门店
        //            else
        //            {
        //                foreach (var item in lt)
        //                {                    
        //                    //盒表赋值
        //                    var boxNo = db.Tm_Card_CardBoxNew.Where(n => n.BoxNo == item.BoxNo).FirstOrDefault();
        //                    boxNo.IsOut = true;
        //                    boxNo.IsReturn = false;
        //                    boxNo.BoxInAddress = item.Store;
        //                    //卡表赋值
        //                    var CardNoList = db.TM_Card_CardNo.Where(n => n.BoxNo == boxNo.BoxNo);
        //                    int reCardNum = 0;
        //                    foreach (var Card in CardNoList)
        //                    {
        //                        if (Card.Status=="2")
        //                        {
        //                            continue;
        //                        }
        //                        reCardNum++;
        //                        Card.Status = "1";
        //                        Card.StoreCode = item.Store;
        //                    }
        //                    var rev = db.TM_Card_RetrieveDetailNew.Where(n => n.BoxNo == item.BoxNo).FirstOrDefault();
        //                    rev.TakeOutBoxNumber = 1;
        //                    //判断有没有关联申请单
        //                    if (string.IsNullOrWhiteSpace(applyId)==false)
        //                    {
        //                        var ApplyQuery = db.TM_Card_Apply.Where(n => n.Id == applyId).FirstOrDefault();
        //                        if (ApplyQuery.AcceptingUnit != item.Store)
        //                        {
        //                            return new Result(false, "订单门店与领出门店不符");
        //                        }
        //                        int numIn = Convert.ToInt32(item.CardNumIn);
        //                        if (ApplyQuery.DeliverNumber + numIn > ApplyQuery.ApproveNumber)
        //                        {
        //                            return new Result(false, "领出数量超过审批数量");
        //                        }
        //                        if (ApplyQuery.DeliverNumber + numIn < ApplyQuery.DeliverNumber)
        //                        {
        //                            ApplyQuery.Status = "2";
        //                        }
        //                        if (ApplyQuery.DeliverNumber + numIn == ApplyQuery.DeliverNumber)
        //                        {
        //                            ApplyQuery.Status = "1";
        //                        }
        //                        ApplyQuery.DeliverNumber += Convert.ToInt32(item.CardNumIn);
        //                    }

        //                    TM_Card_CardOutTitleDetail NewDetail = new TM_Card_CardOutTitleDetail()
        //                    {
        //                        Id = ToolsHelper.GuidtoString(Guid.NewGuid()),
        //                        BoxId = item.BoxNo,
        //                        //CardNum = Convert.ToInt32(item.CardNumIn),
        //                        CardNum = reCardNum,
        //                        CardTypeCode = item.CardTypeCode,
        //                        CreateBy = userId,
        //                        CreateTime = DateTime.Now,
        //                        Purpose = item.Purpose,
        //                        OddId = titleId,
        //                        IsOut = false
        //                    };
        //                    boxNum++;
        //                    db.TM_Card_CardOutTitleDetail.Add(NewDetail);
        //                }

        //                TM_Card_CardOutTitle CardOutTitle = new TM_Card_CardOutTitle()
        //                {
        //                    AcceptintUnit = lt[0].Store,
        //                    BoxNumber = boxNum,
        //                    OddId = titleId,
        //                    Status = status,
        //                    SendingUnit = lt[0].SendUnit,
        //                    CreateBy = userId,
        //                    CreateTime = DateTime.Now,
        //                    IsOddId = true,
        //                    OddIdNo = oddIdNo
        //                };
        //                db.TM_Card_CardOutTitle.Add(CardOutTitle);
        //            }        
        //                db.SaveChanges();                       
        //            return new Result(true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log4netHelper.WriteErrorLog("AddNewCardOutTitle :" + ex.ToString());
        //        return new Result(false, "生成卡领出失败");
        //        throw;
        //    }       
        //}


        public static Result AddNewCardOutTitle(string jsonParam, string status, int userId)
        {
            try
            {
                List<CardOutTitleInfoNew> lt = JsonHelper.Deserialize<List<CardOutTitleInfoNew>>(jsonParam);
                using (CRMEntities db = new CRMEntities())
                {
                    string applyId = lt[0].ApplyOddId;
                    int boxNum = 0;
                    string titleId = ToolsHelper.GuidtoString(Guid.NewGuid());
                    //生成流水单号
                    long oddIdNo = 0;
                    DateTime nowTime = DateTime.Today;
                    DateTime afterTime = nowTime.AddDays(+1);
                    var list = (from n in db.TM_Card_CardOutTitle where n.CreateTime >= nowTime && n.CreateTime < afterTime orderby n.OddIdNo descending select n.OddIdNo).ToList();
                    if (list == null || list.Count == 0)
                    {
                        oddIdNo = Distribution.GetOddIdNoByRole();
                    }
                    else
                    {
                        oddIdNo = list[0].Value + 1;
                    }


                    //领出到分公司
                    if (string.IsNullOrWhiteSpace(lt[0].Company) == false)
                    {
                        foreach (var item in lt)
                        {
                            var boxNo = db.Tm_Card_CardBoxNew.Where(n => n.BoxNo == item.BoxNo).FirstOrDefault();
                            boxNo.IsOut = true;
                            boxNo.IsReturn = false;
                            boxNo.BoxInAddress = string.IsNullOrEmpty(item.Company) ? item.Store : item.Company;
                            var CardNoList = db.TM_Card_CardNo.Where(n => n.BoxNo == boxNo.BoxNo);
                            int reCardNum = 0;
                            foreach (var Card in CardNoList)
                            {
                                if ((Card.Status == "1" && Card.IsSalesStatus == 0) || (Card.Status == "0" && Card.IsSalesStatus == 0))
                                {
                                    reCardNum++;
                                    Card.Status = "1";
                                    Card.IsReturn = false;

                                }
                            }
                            var rev = db.TM_Card_RetrieveDetailNew.Where(n => n.BoxNo == item.BoxNo).FirstOrDefault();
                            rev.TakeOutBoxNumber = 1;

                            TM_Card_CardOutTitleDetail NewDetail = new TM_Card_CardOutTitleDetail()
                            {
                                Id = ToolsHelper.GuidtoString(Guid.NewGuid()),
                                BoxId = item.BoxNo,
                                //CardNum = Convert.ToInt32(item.CardNumIn),
                                CardNum = reCardNum,
                                CardTypeCode = item.CardTypeCode,
                                CreateBy = userId,
                                CreateTime = DateTime.Now,
                                Purpose = item.Purpose,
                                OddId = titleId,
                                IsOut = false,
                                AcceptingUnit = item.Store,
                                SendingUnit = item.SendUnit
                            };
                            boxNum++;
                            db.TM_Card_CardOutTitleDetail.Add(NewDetail);
                        }

                        TM_Card_CardOutTitle CardOutTitle = new TM_Card_CardOutTitle()
                        {
                            AcceptintUnit = lt[0].Store,
                            BoxNumber = boxNum,
                            OddId = titleId,
                            Status = status,
                            SendingUnit = lt[0].SendUnit,
                            CreateBy = userId,
                            CreateTime = DateTime.Now,
                            IsOddId = false,
                            OddIdNo = oddIdNo
                        };
                        db.TM_Card_CardOutTitle.Add(CardOutTitle);
                    }
                    //直接领出到门店
                    else
                    {
                        foreach (var item in lt)
                        {
                            //盒表赋值
                            var boxNo = db.Tm_Card_CardBoxNew.Where(n => n.BoxNo == item.BoxNo).FirstOrDefault();
                            boxNo.IsOut = true;
                            boxNo.IsReturn = false;
                            boxNo.BoxInAddress = item.Store;
                            //卡表赋值
                            var CardNoList = db.TM_Card_CardNo.Where(n => n.BoxNo == boxNo.BoxNo);
                            int reCardNum = 0;
                            foreach (var Card in CardNoList)
                            {
                                if ((Card.Status == "1" && Card.IsSalesStatus == 0) || (Card.Status == "0" && Card.IsSalesStatus == 0))
                                {
                                    reCardNum++;
                                    Card.Status = "1";
                                    Card.IsReturn = false;
                                    Card.StoreCode = item.Store;
                                }
                            }
                            var rev = db.TM_Card_RetrieveDetailNew.Where(n => n.BoxNo == item.BoxNo).FirstOrDefault();
                            rev.TakeOutBoxNumber = 1;
                            //判断有没有关联申请单
                            if (string.IsNullOrWhiteSpace(applyId) == false)
                            {
                                var ApplyQuery = db.TM_Card_Apply.Where(n => n.Id == applyId).FirstOrDefault();
                                if (ApplyQuery.AcceptingUnit != item.Store)
                                {
                                    return new Result(false, "订单门店与领出门店不符");
                                }
                                int numIn = Convert.ToInt32(item.CardNumIn);
                                if (ApplyQuery.DeliverNumber + numIn > ApplyQuery.ApproveNumber)
                                {
                                    return new Result(false, "领出数量超过审批数量");
                                }
                                if (ApplyQuery.DeliverNumber + numIn < ApplyQuery.DeliverNumber)
                                {
                                    ApplyQuery.Status = "2";
                                }
                                if (ApplyQuery.DeliverNumber + numIn == ApplyQuery.DeliverNumber)
                                {
                                    ApplyQuery.Status = "1";
                                }
                                ApplyQuery.DeliverNumber += Convert.ToInt32(item.CardNumIn);
                            }

                            TM_Card_CardOutTitleDetail NewDetail = new TM_Card_CardOutTitleDetail()
                            {
                                Id = ToolsHelper.GuidtoString(Guid.NewGuid()),
                                BoxId = item.BoxNo,
                                //CardNum = Convert.ToInt32(item.CardNumIn),
                                CardNum = reCardNum,
                                CardTypeCode = item.CardTypeCode,
                                CreateBy = userId,
                                CreateTime = DateTime.Now,
                                Purpose = item.Purpose,
                                OddId = titleId,
                                IsOut = false,
                                AcceptingUnit = item.Store,
                                SendingUnit = item.SendUnit
                            };
                            boxNum++;
                            db.TM_Card_CardOutTitleDetail.Add(NewDetail);
                        }

                        TM_Card_CardOutTitle CardOutTitle = new TM_Card_CardOutTitle()
                        {
                            AcceptintUnit = "",
                            BoxNumber = boxNum,
                            OddId = titleId,
                            Status = status,
                            SendingUnit = lt[0].SendUnit,
                            CreateBy = userId,
                            CreateTime = DateTime.Now,
                            IsOddId = true,
                            OddIdNo = oddIdNo
                        };
                        db.TM_Card_CardOutTitle.Add(CardOutTitle);
                    }
                    db.SaveChanges();
                    return new Result(true);
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("AddNewCardOutTitle :" + ex.ToString());
                return new Result(false, "生成卡领出失败");
                throw;
            }
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="oddId"></param>
        /// <param name="status"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static Result ChangeStatusCardOutTitle(string oddId, string status, int userID)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_Card_CardOutTitle.Where(n => n.OddId == oddId).FirstOrDefault();
                query.Status = status;
                query.ModifyBy = userID;
                try
                {
                    db.SaveChanges();
                    return new Result(true, "操作成功");
                }
                catch (Exception ex)
                {
                    Log4netHelper.WriteErrorLog("ChangeStatusCardOutTitle：" + ex.Message.ToString());
                    return new Result(false, "", "操作失败");
                    throw;
                }
            }
        }

        /// <summary>
        /// 总部卡领出列表导出Excel
        /// </summary>
        /// <param name="txtExcelOddIdNo"></param>
        /// <param name="txtExcelBoxNum"></param>
        /// <param name="txtExcelStatus"></param>
        /// <param name="txtExcelAcceptingUnit"></param>
        /// <param name="txtExcelCreateTime"></param>
        /// <returns></returns>
        public static Result CardOutTitleListToExcel(string txtExcelOddIdNo, string txtExcelBoxNum, string txtExcelStatus, string txtExcelAcceptingUnit, string txtExcelCreateTime, string txtExcelBoxNo, string txtExcelStore)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from n in db.TM_Card_CardOutTitle join m in db.V_M_TM_SYS_BaseData_channel on n.AcceptintUnit equals m.ChannelCodeBase into d from ds in d.DefaultIfEmpty() orderby n.CreateTime descending select new { OddId = n.OddId, Status = n.Status, StatusName = (from s in db.TD_SYS_BizOption where s.OptionValue == n.Status && s.OptionType == "ApproveStatus" select s.OptionText).FirstOrDefault(), SendingUnit = n.SendingUnit, AcceptingUnit = ds.ChannelNameBase != null ? ds.ChannelNameBase : (from t in db.V_M_TM_SYS_BaseData_store where n.AcceptintUnit == t.StoreCode select t.StoreName).FirstOrDefault(), AcceptUnitCode = ds.ChannelCodeBase, BoxNumber = n.BoxNumber, CreateBy = from h in db.TM_AUTH_User where h.UserID == n.CreateBy select h.UserName, CreateTime = n.CreateTime, OddIdNo = n.OddIdNo, BoxNo = (from m in db.TM_Card_CardOutTitleDetail where n.OddId == m.OddId select m.BoxId).FirstOrDefault() };
                if (string.IsNullOrEmpty(txtExcelOddIdNo) == false)
                {
                    long oddIdNo = Convert.ToInt64(txtExcelOddIdNo);
                    query = query.Where(n => n.OddIdNo == oddIdNo);
                }
                if (string.IsNullOrEmpty(txtExcelBoxNum) == false)
                {
                    int box = Convert.ToInt32(txtExcelBoxNum);
                    query = query.Where(n => n.BoxNumber == box);
                }
                if (string.IsNullOrEmpty(txtExcelAcceptingUnit) == false)
                {
                    var list1 = (from n in db.TM_Card_CardOutTitleDetail join m in db.V_M_TM_SYS_BaseData_store on n.AcceptingUnit equals m.StoreCode into d from ds in d.DefaultIfEmpty() where ds.ChannelCodeStore == txtExcelAcceptingUnit select n.OddId).Distinct();
                    List<CardTitleOutQuery> cardList1 = new List<CardTitleOutQuery>();
                    foreach (var oddid in list1)
                    {
                        foreach (var q in query)
                        {
                            if (oddid == q.OddId)
                            {
                                cardList1.Add(new CardTitleOutQuery() { OddId = q.OddId, OddIdNo = q.OddIdNo.ToString(), AcceptUnitCode = q.AcceptUnitCode, BoxNumber = q.BoxNumber.ToString(), CreateBy = q.CreateBy.ToString(), CreateTime = q.CreateTime.ToString(), SendingUnit = q.SendingUnit, Status = q.Status, StatusName = q.StatusName });
                            }
                        }
                        //query = query.Where(n => n.AcceptUnitCode == txtExcelAcceptingUnit);
                    }
                    return new Result(true, "", cardList1);
                }
                if (string.IsNullOrEmpty(txtExcelStore) == false)
                {
                    //旧版
                    //var name = (from f in db.V_M_TM_SYS_BaseData_store where f.StoreCode == txtExcelStore select f.StoreName).FirstOrDefault();
                    //query = query.Where(n => n.AcceptingUnit == name);
                    var list = db.TM_Card_CardOutTitleDetail.Where(m => m.AcceptingUnit == txtExcelStore).Select(m => m.OddId).Distinct();
                    List<CardTitleOutQuery> cardList = new List<CardTitleOutQuery>();
                    foreach (var oddid in list)
                    {
                        foreach (var q in query)
                        {
                            if (oddid == q.OddId)
                            {
                                cardList.Add(new CardTitleOutQuery() { OddId = q.OddId, OddIdNo = q.OddIdNo.ToString(), AcceptUnitCode = q.AcceptUnitCode, BoxNumber = q.BoxNumber.ToString(), CreateBy = q.CreateBy.ToString(), CreateTime = q.CreateTime.ToString(), SendingUnit = q.SendingUnit, Status = q.Status, StatusName = q.StatusName });
                            }
                        }
                    }
                    return new Result(true, "", cardList);
                }
                if (string.IsNullOrEmpty(txtExcelStatus) == false)
                {
                    query = query.Where(n => n.Status == txtExcelStatus);
                }
                if (string.IsNullOrEmpty(txtExcelCreateTime) == false)
                {
                    DateTime time = Convert.ToDateTime(txtExcelCreateTime);
                    DateTime afterTime = time.AddHours(+24);
                    query = query.Where(n => n.CreateTime >= time && n.CreateTime < afterTime);
                }
                if (string.IsNullOrEmpty(txtExcelBoxNo) == false)
                {
                    query = query.Where(n => n.BoxNo == txtExcelBoxNo);
                }
                return new Result(true, "", query.ToList());
            }
        }
        #endregion


        #region 单号生成规则
        public static long GetOddIdNoByRole()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            date = date.Remove(4, 1).Remove(6, 1);
            long result = Convert.ToInt64(string.Format("{0}{1}", date, "000001"));
            return result;
        }

        #endregion
    }
}
