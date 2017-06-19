using System.Data.SqlClient;
using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Model.Interface;
using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Objects.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace Timer.CRM.JposInterfaceLogic
{
    public class JposInterfaceBiz
    {
        //获取秘钥
        public static string skey = ConfigurationSettings.AppSettings["skey"].ToString();

        #region 卡信息查询
        public string QueryCard(string input)
        {
            var mr = JsonHelper.Deserialize<QueryCardInput>(input);
            QueryCardOutput qco = new QueryCardOutput();
            qco.retCode = new TRetCode();
            qco.cardInfo = new TCardInfoRec();
            qco.memberInfo = new TQueryMemberResponse();
            qco.memberInfo.memberGrade = new TMemberGrade();

            qco.memberInfo.memberGrade.ucn = new TUCN();
            qco.memberInfo.memberGrade.ucn.code = "";
            qco.memberInfo.memberGrade.ucn.name = "";
            qco.memberInfo.memberGrade.function = "";
            qco.memberInfo.memberGrade.discounts = new List<TDiscount>();

            qco.memberInfo.memberGrade.ucn = new TUCN();
            qco.memberInfo.memberGrade.discounts = new List<TDiscount>();
            qco.memberInfo.attributes = new List<TMemberAttribute>();
            qco.memberInfo.gradeAndPay = new List<TMemGradeAndRangePay>();
            qco.extCardInfo = new List<TExtCardRec>();
            //测试用返回参数
            string msg = string.Empty;
            if (mr == null)
            {
                qco.retCode.code = "-1";
                qco.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(qco);
                return msg;
            }
            if (mr.accountType < 0
                || mr.accountType > 6
                || string.IsNullOrEmpty(mr.accountAccessCode) || string.IsNullOrEmpty(mr.storeCode) || string.IsNullOrEmpty(mr.queryType) || mr.needMemberInfo == null)
            {
                qco.retCode.code = "-7030";
                qco.retCode.message = "必填项为空或有误";
                msg = JsonHelper.Serialize(qco);
                return msg;
            }
            if (!secretKey(skey, mr.signDate, mr.secret))
            {
                qco.retCode.code = "-7031";
                qco.retCode.message = "秘钥错误";
                msg = JsonHelper.Serialize(qco);
                return msg;
            }
            using (CRMEntities db = new CRMEntities())
            {
                var query_con = from a in db.TM_Mem_Card
                                join b in db.V_U_TM_Mem_Info on a.MemberID equals b.MemberID
                                join c in db.V_M_TM_SYS_BaseData_customerlevel on b.CustomerLevel equals c.CustomerLevelBase
                                join d in db.TM_Mem_Account on a.MemberID equals d.MemberID
                                select new
                                {
                                    a,
                                    b,
                                    c,
                                    d.Value1,
                                };
                var query_t = query_con;
                switch (mr.accountType)
                {
                    case 0:
                        query_t = query_con.Where(p => p.a.CardNo == mr.accountAccessCode);
                        break;
                    case 1:
                        query_t = query_con.Where(p => p.b.Mobile == mr.accountAccessCode && p.a.Active && p.a.Lock == false);
                        break;
                    case 2:
                        query_t = query_con.Where(p => p.b.MemberID == mr.accountAccessCode);
                        break;
                    case 3:
                        query_t = query_con.Where(p => p.b.CertificateNo == mr.accountAccessCode);
                        break;
                    default:
                        query_t = null;
                        break;
                }
                var card = db.TM_Card_CardNo.Where(p => (p.CardNo == mr.cardinfo.cardNumber || p.CardNo == mr.accountAccessCode) && p.StoreCode == mr.storeCode).FirstOrDefault();
                if (card == null && query_t.FirstOrDefault() == null)
                {
                    qco.retCode.code = "-002";
                    qco.retCode.message = "卡信息不存在";
                    msg = JsonHelper.Serialize(qco);
                    return msg;
                }

                //卡查询, 用手机号的查询
                if (mr.queryType == "queryCard" && card == null && query_t.FirstOrDefault().a.CardType != "2")
                {
                    card = db.TM_Card_CardNo.Where(p => p.CardNo == query_t.FirstOrDefault().a.CardNo).FirstOrDefault();
                    if (card == null)
                    {
                        qco.retCode.code = "-001";
                        qco.retCode.message = "卡信息异常";
                        msg = JsonHelper.Serialize(qco);
                        return msg;
                    }
                    if (card.Status != "2" && !(card.Status == "1" && card.IsSalesStatus == 1) && card.CardType != "2")
                    {
                        qco.retCode.code = "-001";
                        switch (card.Status)
                        {
                            case "-1":
                                qco.retCode.message = "此卡已挂失";
                                break;
                            case "-2":
                                qco.retCode.message = "此卡已冻结";
                                break;
                            case "-3":
                                qco.retCode.message = "此卡已作废";
                                break;
                            case "1":
                                if (card.IsSalesStatus == 1)
                                    qco.retCode.message = "此卡已补发";
                                else
                                    qco.retCode.message = "此卡未激活";
                                break;
                            default:
                                qco.retCode.message = "此卡不可用";
                                break;
                        }
                        msg = JsonHelper.Serialize(qco);
                        return msg;
                    }


                }
                //卡查询, 用卡号的查询
                if (mr.queryType == "queryCard" && card != null)
                {
                    if (card.Status != "2" && !(card.Status == "1" && card.IsSalesStatus == 1))
                    {
                        qco.retCode.code = "-001";
                        switch (card.Status)
                        {
                            case "-1":
                                qco.retCode.message = "此卡已挂失";
                                break;
                            case "-2":
                                qco.retCode.message = "此卡已冻结";
                                break;
                            case "-3":
                                qco.retCode.message = "此卡已作废";
                                break;
                            case "1":
                                if (card.IsSalesStatus == 1)
                                    qco.retCode.message = "此卡已补发";
                                else
                                    qco.retCode.message = "此卡未激活";
                                break;
                            default:
                                qco.retCode.message = "此卡不可用";
                                break;
                        }
                        msg = JsonHelper.Serialize(qco);
                        return msg;
                    }
                }
                if (mr.queryType == "saleCard")
                {
                    if (card.IsSalesStatus != 0)
                    {
                        qco.retCode.code = "-003";
                        qco.retCode.message = "此卡已发售";
                        msg = JsonHelper.Serialize(qco);
                        return msg;
                    }
                    if (card.Status == "0")
                    {
                        qco.retCode.code = "-003";
                        qco.retCode.message = "此卡不存在";
                        msg = JsonHelper.Serialize(qco);
                        return msg;
                    }
                    if (card.Status != "1")
                    {
                        qco.retCode.code = "-004";
                        qco.retCode.message = "此卡已使用";
                        msg = JsonHelper.Serialize(qco);
                        return msg;
                    }

                }
                if (mr.queryType == "oldCardReissue")
                {
                    if (query_t.FirstOrDefault() == null)
                    {
                        qco.retCode.code = "-001";
                        qco.retCode.message = "卡信息异常";
                        msg = JsonHelper.Serialize(qco);
                        return msg;
                    }

                    if (card.Status != "-1" && card.Status != "-2" && card.Status != "-3")
                    {
                        qco.retCode.code = "-005";
                        //qco.retCode.message = "此卡不可补发";
                        switch (card.Status)
                        {
                            case "-1":
                                qco.retCode.message = "此卡已挂失";
                                break;
                            case "-2":
                                qco.retCode.message = "此卡已冻结";
                                break;
                            case "-3":
                                qco.retCode.message = "此卡已作废";
                                break;
                            case "1":
                                if (card.IsSalesStatus == 1)
                                    qco.retCode.message = "此卡已补发";
                                else
                                    qco.retCode.message = "此卡未激活";
                                break;
                            default:
                                qco.retCode.message = "此卡不可补发";
                                break;
                        }
                        msg = JsonHelper.Serialize(qco);
                        return msg;
                    }
                }
                if (mr.queryType == "newCardReissue")
                {
                    if (card.Status != "1")
                    {
                        qco.retCode.code = "-005";
                        //qco.retCode.message = "此卡不可补发";
                        switch (card.Status)
                        {
                            case "-1":
                                qco.retCode.message = "此卡已挂失";
                                break;
                            case "-2":
                                qco.retCode.message = "此卡已冻结";
                                break;
                            case "-3":
                                qco.retCode.message = "此卡已作废";
                                break;
                            case "1":
                                if (card.IsSalesStatus == 1)
                                    qco.retCode.message = "此卡已补发";
                                else
                                    qco.retCode.message = "此卡未激活";
                                break;
                            default:
                                qco.retCode.message = "此卡不可补发";
                                break;
                        }
                        msg = JsonHelper.Serialize(qco);
                        return msg;
                    }

                }

                qco.time = mr.time;
                qco.tranTime = mr.tranTime;
                qco.tranId = mr.tranId;
                qco.operatorStr = mr.operatorStr;

                if (card != null)
                    qco.cardInfo.cardNumber = card.CardNo;
                else
                    qco.cardInfo.cardNumber = query_t.FirstOrDefault().a.CardNo;
                qco.cardInfo.media = "";
                if (query_t.FirstOrDefault() != null && query_t.FirstOrDefault().a.CardType == "2")
                {
                    qco.cardInfo.cardType = "002";
                    qco.cardInfo.cardTypeName = "电子卡";
                }
                else
                {
                    qco.cardInfo.cardType = "001";
                    qco.cardInfo.cardTypeName = "实体卡";
                }
                qco.cardInfo.balance = 0;

                qco.cardInfo.validDate = DateTime.Now.AddYears(100);
                qco.cardInfo.cardFunction = "1111000000";
                var mem = query_t.FirstOrDefault();
                if (mem != null)
                {
                    qco.cardInfo.score = query_t.FirstOrDefault().Value1;
                    qco.cardInfo.cardHolder = mem.a.MemberID;
                    if (mem.b.Birthday != null)
                    {
                        qco.birthday = (DateTime)mem.b.Birthday;
                        qco.isBirthdate = (DateTime.Now.ToString("yyyy-MM-dd") == string.Format("{0:yyyy-MM-dd}", mem.b.Birthday));
                        qco.memberInfo.birthday = (DateTime)mem.b.Birthday;
                    }
                    //qco.isMemorialDay = (DateTime.Now.ToString("yyyy-MM-dd") == string.Format("{0:yyyy-MM-dd}", query.x.d.MemorialDay)); 
                    //纪念日还没加
                    qco.memberType = mem.b.CustomerLevel;
                    qco.mobilePhoneNum = mem.b.Mobile;
                    qco.memberTypeName = mem.c.CustomerLevelNameBase;
                    qco.memberId = mem.b.MemberID;
                    qco.memberName = mem.b.CustomerName;
                    qco.memberInfo.code = mem.b.MemberID;
                    qco.memberInfo.name = mem.b.CustomerName;
                    //output.memberId =   会员外部号
                    qco.memberInfo.memberGradeName = mem.c.CustomerLevelNameBase;
                    qco.memberInfo.cellPhone = mem.b.Mobile;
                    qco.memberInfo.email = mem.b.CustomerEmail;
                    qco.memberInfo.address = mem.b.Address1;
                    qco.memberInfo.sex = mem.b.Gender;
                    qco.memberInfo.provinceCode = mem.b.ProvinceCodeExt;
                    qco.memberInfo.provinceName = mem.b.Province;
                    qco.memberInfo.cityCode = mem.b.CityCodeExt;
                    qco.memberInfo.cityName = mem.b.City;
                    qco.memberInfo.districtCode = mem.b.DistrictCodeExt;
                    qco.memberInfo.districtName = mem.b.District;
                    qco.memberInfo.memberGrade = new TMemberGrade();
                    qco.memberInfo.memberGrade.ucn = new TUCN(); // query.x.d.MemberGrade; //会员等级
                    qco.memberInfo.memberGrade.ucn.code = mem.c.CustomerLevelBase;
                    qco.memberInfo.memberGrade.ucn.name = mem.c.CustomerLevelNameBase;

                    qco.memberInfo.cardNo = mem.a.CardNo;
                    qco.memberInfo.memberGrade.function = "";
                    qco.memberInfo.memberGrade.discounts = new List<TDiscount>();
                }
                else
                {
                    qco.memberInfo = new TQueryMemberResponse();
                    qco.memberInfo.memberGrade = new TMemberGrade();
                    qco.memberInfo.memberGrade.ucn = new TUCN();
                    qco.memberInfo.memberGrade.ucn.code = "";
                    qco.memberInfo.memberGrade.ucn.name = "";
                    qco.memberInfo.memberGrade.function = "";
                    qco.memberInfo.memberGrade.discounts = new List<TDiscount>();
                    qco.memberInfo.memberGrade.ucn = new TUCN();
                    qco.memberInfo.memberGrade.discounts = new List<TDiscount>();
                    qco.memberInfo.attributes = new List<TMemberAttribute>();
                    qco.memberInfo.gradeAndPay = new List<TMemGradeAndRangePay>();
                }
            }
            qco.accountType = mr.accountType;
            qco.accountAccessCode = mr.accountAccessCode;
            qco.discount = 1;
            qco.accountAccessCode = mr.accountAccessCode;
            qco.discountRate = 1;
            qco.retCode.code = "96960";
            qco.retCode.message = "OK";
            msg = JsonHelper.Serialize(qco);
            //Log4netHelper.WriteInfoLog("3333333:" + msg);
            return msg;

        }
        #endregion

        #region 卡挂失查询
        public string QueryLossCard(string input)
        {
            var mr = JsonHelper.Deserialize<QueryLossCardInput>(input);
            string msg = string.Empty;
            QueryLossCardOutput output = new QueryLossCardOutput();
            output.retCode = new TRetCode();
            output.lossCardInfos = new List<TLossCardInfoRec>();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }
            if (mr.accountType < 0
                || mr.accountType > 6
                || string.IsNullOrEmpty(mr.accountAccessCode))
            {
                output.retCode.code = "-7030";
                output.retCode.message = "必填项为空或有误";
                msg = JsonHelper.Serialize(output);
                return msg;
            }
            if (!secretKey(skey, mr.signDate, mr.secret))
            {
                output.retCode.code = "-7031";
                output.retCode.message = "秘钥错误";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            using (CRMEntities db = new CRMEntities())
            {

                var query_card = from m in db.V_U_TM_Mem_Info
                                 join mc in db.TM_Mem_Card on m.MemberID equals mc.MemberID
                                 where mc.Lock == false && mc.Active && mc.CardType == "1"
                                 select new { m, mc };
                //已经挂失的卡不可以挂失
                if (query_card.Count() == 0)
                {
                    output.retCode.code = "407";
                    output.retCode.message = "会员卡已挂失或废除";
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
                //根据传参类型添加where条件
                switch (mr.accountType)
                {
                    case 0:
                        query_card = query_card.Where(p => p.mc.CardNo == mr.accountAccessCode);//query_con.Where(p => p.x.d.MemberCardNo == mr.accountAccessCode).FirstOrDefault();
                        break;
                    case 1:
                        query_card = query_card.Where(p => p.m.Mobile == mr.accountAccessCode);
                        break;
                    case 2:
                        query_card = query_card.Where(p => p.m.MemberID == mr.accountAccessCode);
                        break;
                    case 3:
                        query_card = query_card.Where(p => p.m.CertificateNo == mr.accountAccessCode);
                        break;
                    default:
                        query_card = null;
                        break;
                }
                //根据条件遍历返回会员卡信息
                foreach (var card in query_card.ToList())
                {
                    TLossCardInfoRec lossCard = new TLossCardInfoRec();
                    lossCard.cardInfo = new TCardInfoRec();
                    lossCard.memberName = "";
                    lossCard.memberCode = card.m.MemberID;
                    lossCard.cardStatus = "";
                    lossCard.cardInfo.cardNumber = card.mc.CardNo;
                    lossCard.cardInfo.cardHolder = card.m.CustomerName;
                    lossCard.cardInfo.media = "";
                    lossCard.cardInfo.cardType = "1";
                    lossCard.cardInfo.cardTypeName = "";
                    lossCard.cardInfo.balance = 0;
                    lossCard.cardInfo.score = 0;
                    lossCard.cardInfo.validDate = DateTime.Now.AddYears(99);
                    lossCard.cardInfo.cardFunction = "";
                    output.lossCardInfos.Add(lossCard);
                }
            }
            output.time = mr.time;
            output.tranId = mr.tranId;
            output.tranTime = mr.tranTime;
            output.accountAccessCode = mr.accountAccessCode;
            output.accountType = mr.accountType;
            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region 卡挂失prepare
        public string LossCardPrepare(string input)
        {
            var mr = JsonHelper.Deserialize<LossCardPrepareInput>(input);

            LossCardPrepareOutput output = new LossCardPrepareOutput();

            output.retCode = new TRetCode();
            output.lossCardInfos = new List<TLossCardInfoRec>();

            string msg = string.Empty;

            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            if (mr.accountType < 0
                || mr.accountType > 6
                || string.IsNullOrEmpty(mr.accountAccessCode))
            {
                output.retCode.code = "-7030";
                output.retCode.message = "必填项为空或有误";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            if (!secretKey(skey, mr.signDate, mr.secret))
            {
                output.retCode.code = "-7031";
                output.retCode.message = "秘钥错误";
                msg = JsonHelper.Serialize(output);
                return msg;
            }



            using (CRMEntities db = new CRMEntities())
            {
                string mid = Guid.NewGuid().ToString("N");


                var cardNumbers = db.TM_JPOS_CardPrepare.Where(i => i.source == "JPos" && i.isConfirm == false).Select(o => o.cardNumber).ToList();


                if (mr.lossCardInfos != null)
                {

                    if (mr.lossCardInfos.Count > 0)
                    {
                        List<TLossCardInfoRec> cards = new List<TLossCardInfoRec>();

                        foreach (var lossCardInfo in mr.lossCardInfos)
                        {
                            if (cardNumbers.Contains(lossCardInfo.cardInfo.cardNumber))
                            {
                                output.retCode.code = "-7029";
                                output.retCode.message = "CardPrepare重复";
                                msg = JsonHelper.Serialize(output);
                                return msg;
                            }
                            else
                            {

                                TM_JPOS_CardPrepare cardPrepare = new TM_JPOS_CardPrepare();
                                cardPrepare.id = mid;
                                cardPrepare.time = mr.time;
                                cardPrepare.operatorStr = mr.operatorStr;
                                cardPrepare.storeCode = mr.storeCode;
                                cardPrepare.tranId = mr.tranId;
                                cardPrepare.tranTime = mr.tranTime;
                                cardPrepare.posNo = mr.posNo;
                                cardPrepare.accountType = mr.accountType;
                                cardPrepare.accountAccessCode = mr.accountAccessCode;
                                cardPrepare.mobileCheckPsw = mr.mobileCheckPsw;
                                cardPrepare.xid = mr.xid;
                                cardPrepare.lossCardInfo = JsonHelper.Serialize(lossCardInfo);
                                cardPrepare.isConfirm = false;
                                cardPrepare.cardNumber = lossCardInfo.cardInfo.cardNumber;
                                cardPrepare.source = "JPos";
                                db.TM_JPOS_CardPrepare.Add(cardPrepare);

                                TLossCardInfoRec card = new TLossCardInfoRec();
                                card.cardInfo = new TCardInfoRec();

                                var card_db = db.TM_Mem_Card.Join(db.V_U_TM_Mem_Info, u => u.MemberID, d => d.MemberID, (u, d) => new { u, d }).GroupJoin(db.V_M_TM_SYS_BaseData_customerlevel, x => x.d.CustomerLevel, y => y.CustomerLevelBase, (x, y) => new { x, y }).Where(i => i.x.d.MemberCardNo == lossCardInfo.cardInfo.cardNumber).Select(o => o).FirstOrDefault();

                                if (card_db != null)
                                {
                                    card.memberName = card_db.x.d.CustomerName;
                                    card.memberCode = card_db.x.d.MemberID;
                                    card.fee = 0;
                                    card.cardStatus = "enable";
                                    card.cardInfo.cardNumber = card_db.x.d.MemberCardNo;
                                    card.cardInfo.media = "media";
                                    card.cardInfo.cardType = "卡类型";
                                    card.cardInfo.cardTypeName = "卡类型名称";
                                    card.cardInfo.balance = 0;
                                    card.cardInfo.score = 99;
                                    card.cardInfo.validDate = DateTime.Now.AddYears(100);
                                    card.cardInfo.cardFunction = "卡功能";
                                    card.cardInfo.cardHolder = card_db.x.d.MemberID;
                                    card.cardInfo.cost = 199;


                                    cards.Add(card);
                                }




                            }
                        }
                        output.lossCardInfos = cards;
                    }
                }

                db.SaveChanges();



            }





            output.time = mr.time;
            output.tranId = mr.tranId;
            output.tranTime = mr.tranTime;
            output.operatorStr = mr.operatorStr;
            output.accountType = mr.accountType;
            output.accountAccessCode = mr.accountAccessCode;
            output.mobileCheckPsw = mr.mobileCheckPsw;
            output.xid = mr.xid;




            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);

            return msg;
        }
        #endregion

        #region 卡挂失confirm
        public string LossCardConfirm(string input)
        {
            var mr = JsonHelper.Deserialize<LossCardConfirmInput>(input);

            LossCardConfirmOutput output = new LossCardConfirmOutput();

            output.retCode = new TRetCode();
            output.lossCardInfos = new List<TLossCardInfoRec>();

            string msg = string.Empty;

            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            if (mr.accountType < 0
                || mr.accountType > 6
                || string.IsNullOrEmpty(mr.accountAccessCode))
            {
                output.retCode.code = "-7030";
                output.retCode.message = "RequiredFieldError";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            if (!secretKey(skey, mr.signDate, mr.secret))
            {
                output.retCode.code = "-7031";
                output.retCode.message = "WrongSecret";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            using (CRMEntities db = new CRMEntities())
            {
                string mid = Guid.NewGuid().ToString("N");
                var cardNumbers = db.TM_JPOS_CardPrepare.Where(i => i.source == "JPos" && i.isConfirm == false).Select(o => o.cardNumber).ToList();
                if (mr.lossCardInfos != null)
                {
                    if (mr.lossCardInfos.Count > 0)
                    {
                        List<TLossCardInfoRec> cards = new List<TLossCardInfoRec>();
                        foreach (var lossCardInfo in mr.lossCardInfos)
                        {
                            if (cardNumbers.Contains(lossCardInfo.cardInfo.cardNumber))
                            {
                                var query = db.TM_JPOS_CardPrepare.Where(i => i.cardNumber == lossCardInfo.cardInfo.cardNumber && i.source == "JPos").FirstOrDefault();
                                if (query != null)
                                {
                                    var cardno = db.TM_Card_CardNo.Where(p => p.CardNo == query.cardNumber).FirstOrDefault();
                                    if (cardno != null)
                                    {
                                        cardno.Status = "-1";
                                    }
                                    var cardmem = db.TM_Mem_Card.Where(p => p.CardNo == query.cardNumber).FirstOrDefault();
                                    if (cardmem != null)
                                    {
                                        cardmem.Lock = true; cardmem.Active = false; cardmem.Status = -1;
                                    }
                                }
                                query.isConfirm = true;
                            }
                        }

                        db.SaveChanges();
                    }
                }
            }
            output.lossCardInfos = mr.lossCardInfos;
            output.time = mr.time;
            output.tranId = mr.tranId;
            output.tranTime = mr.tranTime;
            output.operatorStr = mr.operatorStr;
            output.xid = mr.xid;
            output.retCode.code = "96960";
            output.retCode.message = "OK";


            msg = JsonHelper.Serialize(output);

            return msg;
        }
        #endregion

        #region 查询会员
        public string QueryMember(string input)
        {
            var mr = JsonHelper.Deserialize<QueryMemberInput>(input);

            QueryMemberOutput output = new QueryMemberOutput();

            output.retCode = new TRetCode();
            output.memberGrade = new TMemberGrade();


            output.memberGrade.ucn = new TUCN();
            output.memberGrade.discounts = new List<TDiscount>();
            output.attributes = new List<TMemberAttribute>();
            output.gradeAndPay = new List<TMemGradeAndRangePay>();

            string msg = string.Empty;

            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            if (!secretKey(skey, mr.signDate, mr.secret))
            {
                output.retCode.code = "-7031";
                output.retCode.message = "秘钥错误";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            using (CRMEntities db = new CRMEntities())
            {

                var query_mem = from u in db.V_U_TM_Mem_Info
                                join c in db.TM_Mem_Card on u.MemberID equals c.MemberID
                                join l in db.V_M_TM_SYS_BaseData_customerlevel on u.CustomerLevel equals l.CustomerLevelBase
                                where c.Lock == false && c.Active
                                select new { u, c, l };
                switch (mr.memberAccessType)
                {
                    case 0:
                        query_mem = query_mem.Where(i => i.c.CardNo == mr.memberAccessCode);
                        break;
                    case 1:
                        query_mem = query_mem.Where(i => i.u.Mobile == mr.memberAccessCode);
                        break;
                    case 2:
                        query_mem = query_mem.Where(i => i.u.MemberID == mr.memberAccessCode);
                        break;
                    case 3:
                        query_mem = query_mem.Where(i => i.u.CertificateNo == mr.memberAccessCode);
                        break;
                    default:
                        query_mem = null;
                        break;
                }
                var query = query_mem.FirstOrDefault();
                if (query == null)
                {
                    var c = db.TM_Card_CardNo.Where(p => p.CardNo == mr.memberAccessCode).FirstOrDefault();


                    output.retCode.code = "-001";
                    if (c != null)
                    {
                        switch (c.Status)
                        {
                            case "-1":
                                output.retCode.message = "此卡已挂失";
                                break;
                            case "-2":
                                output.retCode.message = "此卡已冻结";
                                break;
                            case "-3":
                                output.retCode.message = "此卡已作废";
                                break;
                            case "1":
                                output.retCode.message = "此卡未激活";
                                break;
                            case "2":
                                output.retCode.code = "51";
                                output.retCode.message = "没找到会员";
                                break;
                            default:
                                output.retCode.message = "没找到会员";
                                break;
                        }
                    }
                    else
                    {
                        output.retCode.message = "没找到会员";
                    }
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
                else
                {
                    output.time = mr.time;
                    output.tranTime = mr.tranTime;
                    output.tranId = mr.tranId;
                    output.operatorStr = mr.operatorStr;

                    output.code = query.u.MemberID;
                    output.name = query.u.CustomerName;
                    //output.memberId =   会员外部号
                    if (query.u.Birthday != null) { output.birthday = (DateTime)query.u.Birthday; }
                    //output.memorialDay =  会员纪念日
                    //output.occupation =   职业
                    //output.specialty = 特长
                    //output.nationality = 民族
                    //output.presenterCode = 推荐人ID
                    //output.presenterName = 推荐人姓名
                    output.memberGradeName = query.l.CustomerLevelNameBase;
                    output.cellPhone = query.u.Mobile;
                    //output.fixedPhone = 固定电话
                    output.email = query.u.CustomerEmail;
                    //output.postCode = 邮编
                    //output.qq = qq号
                    output.address = query.u.Address1;
                    //output.status = 会员状态
                    output.paperType = query.u.CertificateType;
                    output.paperCode = query.u.CertificateNo;
                    //output.remark = 备注
                    output.sex = query.u.Gender;
                    //output.wedding = 婚否;
                    //output.orgCode = 创建组织代码
                    //output.orgName = 创建组织名称
                    //output.nationCode = 国家代码
                    //output.nationCode = 国家名称
                    output.provinceCode = query.u.ProvinceCodeExt;
                    output.provinceName = query.u.Province;
                    output.cityCode = query.u.CityCodeExt;
                    output.cityName = query.u.City;
                    output.districtCode = query.u.DistrictCodeExt;
                    output.districtName = query.u.District;
                    //output.region = 区域代码
                    //output.source = 来源
                    output.isMember = true;
                    output.memberGrade = new TMemberGrade();
                    output.memberGrade.ucn = new TUCN(); // query.x.d.MemberGrade; //会员等级
                    output.memberGrade.ucn.code = query.l.CustomerLevelBase;
                    output.memberGrade.ucn.name = query.l.CustomerLevelNameBase;
                    output.memberGrade.function = "";
                    output.memberGrade.discounts = new List<TDiscount>();

                    //output.login = 登录名
                    //output.lastLoginTime = 最后登录时间
                    //output.referrer = 会员上线标识
                    //output.attributes = 会员扩展属性
                    //output.activityHeat = 活动热度
                    //output.memberSMSReceive = 接受活动信息
                    //output.memberSourceCode = 会员来源代码
                    //output.memberSourceName = 会员来源名称

                    if (query.u.RegisterDate != null) { output.birthday = (DateTime)query.u.RegisterDate; }
                    //output.childBirthday = 孩子生日
                    //output.memorialDayRemark = 纪念日说明
                    //output.memGradePay = 会员等级消费金额
                    //output.memGradeValidate = 会员等级有效期
                    //output.gradeAndPay = 会员即将达到的等级以及所需金额
                    output.cardNo = query.c.CardNo;
                    //output.memberGradeCode = 会员等级代码
                    output.retCode.code = "96960";
                    output.retCode.message = "OK";
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
            }
        }
        #endregion

        #region 查询费用
        /// <summary>
        /// 查询费用
        /// </summary>
        /// <param name="input">传入参数</param>
        /// <returns></returns>
        public string QueryFee(string input)
        {
            //返回数据
            string msg = string.Empty;
            //处理传入的参数
            var mr = JsonHelper.Deserialize<QueryFeeInput>(input);

            if (mr == null)
            {
                msg = resultJson(-1, "入参错误");
                return msg;
            }
            if (!secretKey(skey, mr.signDate, mr.secret))
            {
                msg = resultJson(-7031, "秘钥错误");
                return msg;
            }
            using (CRMEntities db = new CRMEntities())
            {
                #region 根据门店代码获取数据
                var storeCodeQuery = db.V_M_TM_SYS_BaseData_store.Where(t => t.StoreCode == mr.storeCode).Select(t =>
                    new
                    {
                        t.ChannelCodeStore,
                        t.ChannelNameStore
                    }
                ).FirstOrDefault();

                if (storeCodeQuery == null)
                {
                    msg = resultJson(-1, "门店代码:" + mr.storeCode + "没有相应数据！");
                    return msg;
                }

                #endregion

                //根据门店代码获取补发卡数据
                var cardCosQuery = db.TM_JPOS_CardCost.Where(p => p.IsEnable == true).Select(t =>
                    new
                    {
                        t.ID,
                        t.CardType,
                        t.CardAmount,
                        t.CardCost,
                        t.CompanyCode,
                        t.IsEnable,
                        t.AddedDate,
                        t.AddedUser
                    }
                ).FirstOrDefault();
                QueryFeeOutput output = new QueryFeeOutput();
                output.retCode = new TRetCode();
                if (cardCosQuery == null)
                {
                    output.retCode.code = "-1";
                    output.retCode.message = "没有相应的补发卡数据!";
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
                output.time = mr.time;//请求响应时间
                output.retCode.code = "96960";//响应码
                output.retCode.message = "OK";//响应信息
                output.tranId = mr.tranId;//交易流水号
                output.tranTime = mr.tranTime;//交易时间
                output.operatorStr = mr.operatorStr;//操作员
                output.fee = Convert.ToDecimal(cardCosQuery.CardCost.ToString());//费用
                #region 费用类型 可选值:SaleCard("办卡"), ReportLoss("挂失"), RelatCard("卡关联"), ReissueCard("补发")
                output.feeType = mr.feeType;
                #endregion

                output.accountId = mr.accountId;//账户识别码值
                output.accountType = mr.accountType;//账户识别码类型

                msg = JsonHelper.Serialize(output);
            }


            return msg;
        }
        #endregion

        #region 卡补发
        /// <summary>
        /// 卡补发
        /// </summary>
        /// <param name="input">传入参数</param>
        /// <returns></returns>
        public string ReissureCard(string input)
        {
            //返回数据
            string msg = string.Empty;
            //处理传入的参数
            var mr = JsonHelper.Deserialize<ReissureCardInput>(input);
            if (mr == null)
            {
                msg = resultJson(-1, "入参错误");
                return msg;
            }
            if (mr.cardInfo == null)
            {
                msg = resultJson(-1, "卡信息为空");
                return msg;
            }
            if (!secretKey(skey, mr.signDate, mr.secret))
            {
                msg = resultJson(-7031, "秘钥错误");
                return msg;
            }
            using (CRMEntities db = new CRMEntities())
            {

                //#region 根据门店代码获取数据
                //var storeCodeQuery = db.V_M_TM_SYS_BaseData_store.Where(t => t.StoreCode == mr.storeCode).Select(t =>
                //    new
                //    {
                //        t.ChannelCodeStore,
                //        t.ChannelNameStore
                //    }
                //).FirstOrDefault();
                //if (storeCodeQuery == null)
                //{
                //    msg = resultJson(-1, "门店代码:" + mr.storeCode + "没有相应门店数据！");
                //    return msg;
                //}
                //#endregion
                ////根据门店代码获取补发卡数据
                //var cardCosQuery = db.TM_JPOS_CardCost.Where(t => t.CompanyCode == storeCodeQuery.ChannelCodeStore).Select(t =>
                //    new
                //    {
                //        t.CardAmount,
                //        t.CardCost
                //    }
                //).FirstOrDefault();
                //if (cardCosQuery == null)
                //{
                //    msg = resultJson(-1, "门店代码:" + mr.storeCode + "没有相应补发卡数据！");
                //    return msg;
                //}
                //根据卡信息cardInfo.cardNumber获取是否有可用实体卡
                var cardQuery = db.TM_Mem_Card.Where(t => t.CardNo == mr.cardInfo.cardNumber).Select(t => new
                {
                    t.MemberID,
                    t.CardType,
                    t.Lock,
                    t.IsReissured
                }).FirstOrDefault();

                if (cardQuery == null)//
                {
                    var mem = db.TM_Mem_Card.Where(p => p.CardNo == mr.accountAccessCode).FirstOrDefault();
                    mem.Status = -3;
                    var oldcard = db.TM_Card_CardNo.Where(p => p.CardNo == mr.accountAccessCode).FirstOrDefault();
                    oldcard.Status = "-3";
                    var cardno = db.TM_Card_CardNo.Where(i => i.CardNo == mr.cardInfo.cardNumber && i.CardType == "1").FirstOrDefault();
                    cardno.IsSalesStatus = 1; cardno.Status = "1";
                    if (cardno == null || cardno.IsUsed == true)
                    {
                        msg = resultJson(-1, "卡库中没有可用的卡");
                        return msg;
                    }
                    string guid = Guid.NewGuid().ToString("N");
                    var rule = new TM_JPOS_CardReissue
                    {
                        id = guid,
                        time = mr.time,
                        operatorStr = mr.operatorStr,
                        storeCode = mr.storeCode,
                        tranId = mr.tranId,
                        tranTime = mr.tranTime,
                        posNo = mr.posNo,
                        accountType = Convert.ToInt32(mr.accountType),
                        accountAccessCode = mr.accountAccessCode,
                        xid = mr.xid,
                        CardInfo = JsonHelper.Serialize(mr.cardInfo),
                        isCancel = true,
                        cardNumber = mr.cardInfo.cardNumber,
                        referenceNo = mr.reissueTranId,
                        reissueType = mr.reissueType.ToString(),
                        reissueValue = mr.fee
                    };
                    db.TM_JPOS_CardReissue.Add(rule);


                    var card = new TM_Mem_Card()
                    {
                        CardNo = mr.cardInfo.cardNumber,
                        MemberID = mem.MemberID,
                        CardType = "1",
                        Active = true,
                        Lock = false,
                        AddedDate = DateTime.Now,
                        AddedUser = "Jpos",
                        IsReissured = true,
                        Status = 4

                    };
                    db.TM_Mem_Card.Add(card);
                    db.SaveChanges();
                    ReissureCardOutput cutput = new ReissureCardOutput();
                    cutput.retCode = new TRetCode();

                    cutput.time = mr.time;
                    cutput.retCode.code = "96960";
                    cutput.retCode.message = "OK";
                    cutput.tranId = mr.tranId;
                    cutput.tranTime = mr.tranTime;
                    cutput.operatorStr = mr.operatorStr;
                    cutput.accountType = mr.accountType;
                    cutput.accountAccessCode = mr.accountAccessCode;
                    cutput.cardInfo = mr.cardInfo;
                    cutput.xid = mr.xid;
                    cutput.reissueType = mr.reissueType;
                    cutput.fee = mr.fee;
                    cutput.reissueTranId = mr.reissueTranId;
                    msg = JsonHelper.Serialize(cutput);



                }
                else
                {
                    msg = resultJson(-1, "卡号已存在！");
                    return msg;
                }

            }


            return msg;
        }
        #endregion

        #region 卡补发撤销 ReissureCardCancel

        /// <summary>
        /// 卡补发撤销
        /// </summary>
        /// <param name="input">传入参数</param>
        /// <returns></returns>
        public string ReissureCardCancel(string input)
        {
            //返回数据
            string msg = string.Empty;
            //处理传入的参数
            var mr = JsonHelper.Deserialize<ReissureCardCancelInput>(input);

            if (mr == null)
            {
                msg = resultJson(-1, "入参错误");
                return msg;
            }
            if (!secretKey(skey, mr.signDate, mr.secret))
            {
                msg = resultJson(-7031, "秘钥错误");
                return msg;
            }
            using (CRMEntities db = new CRMEntities())
            {
                //1、根据订单号查询该订单是否已补发
                var tradeSalesQuery = db.V_M_TM_Mem_Trade_sales.Where(t => t.TradeCode == mr.oldTranId).Select(t =>
                new
                {
                    t.MemberID,
                    t.IsReissured//是否补发
                }
            ).FirstOrDefault();



                if (tradeSalesQuery != null)
                {
                    #region revert tm_card_cardno  --wy
                    var cardno = (from a in db.TM_Card_CardNo
                                  join b in db.TM_Mem_Card on a.CardNo equals b.CardNo
                                  where b.MemberID == tradeSalesQuery.MemberID
                                  select new
                                  {
                                      a.CardNo
                                  }).ToList();
                    if (cardno != null)
                    {
                        var query = db.TM_Card_CardNo.Where(i => i.CardNo == cardno[0].CardNo).FirstOrDefault();
                        query.IsUsed = false;
                    }
                    else
                    {
                        msg = resultJson(-1, "库中没有要撤销的卡");
                        return msg;
                    }
                    #endregion
                    //2、根据【tradeSalesQuery.MemberID】获取数据
                    var cardQuery = db.TM_Mem_Card.Where(t => t.MemberID == tradeSalesQuery.MemberID).FirstOrDefault();
                    if (cardQuery != null)
                    {
                        #region 3、根据【mr.oldTranId】删除TM_Mem_Card中的数据
                        db.TM_Mem_Card.Remove(cardQuery);
                        int number = db.SaveChanges();
                        #endregion
                        //var number=db.Database.ExecuteSqlCommand(
                        //    "DELETE FROM TM_Mem_Card where CardNo=@CardNo",
                        //    new SqlParameter("@CardNo", mr.oldTranId));

                        ReissureCardCancelOutput output = new ReissureCardCancelOutput();
                        output.retCode = new TRetCode();

                        if (number > 0)
                        {


                            #region 4、更新V_M_TM_Mem_Trade_sales中的IsReissured值
                            //V_M_TM_Mem_Trade_sales oldTradeSales = db.V_M_TM_Mem_Trade_sales.FirstOrDefault(t => t.TradeCode == mr.reissueTranId);

                            db.Database.ExecuteSqlCommand(
                                "update V_M_TM_Mem_Trade_sales set IsReissured=@IsReissured where MemberID=@MemberID",
                                new SqlParameter("@IsReissured", true),
                                new SqlParameter("@MemberID", tradeSalesQuery.MemberID));
                            #endregion

                            #region 2、返回TM_Card_CardNo中的IsSalesStatus值
                            int IsSalesStatus = 0;
                            db.Database.ExecuteSqlCommand(
                                "update TM_Card_CardNo set IsSalesStatus=@IsSalesStatus where CardNo=@CardNo",
                                new SqlParameter("@IsReissured", IsSalesStatus),
                                new SqlParameter("@CardNo", cardno[0].CardNo));
                            #endregion


                            #region 5、返回数据
                            output.time = mr.time;
                            output.retCode.code = "96960";
                            output.retCode.message = "OK";
                            output.tranId = mr.tranId;
                            output.tranTime = mr.tranTime;
                            output.operatorStr = mr.operatorStr;
                            output.oldTranId = mr.oldTranId;
                            output.xid = mr.xid;
                            msg = JsonHelper.Serialize(output);
                            #endregion
                        }
                        else
                        {
                            output.retCode.code = "-1";
                            output.retCode.message = "卡补发撤销失败！";
                            msg = JsonHelper.Serialize(output);
                        }

                    }
                    else
                    {
                        msg = resultJson(-1, "撤销交易号:【" + mr.oldTranId + "】没有相应数据！");
                        return msg;
                    }
                }
                else
                {
                    msg = resultJson(-1, "没有卡补发撤销信息！");
                    return msg;
                }
            }

            return msg;
        }
        #endregion

        #region  SendSMS
        public string SendSMS(string input)
        {
            var mr = JsonHelper.Deserialize<SendSMSInput>(input);
            SendSMSOutput output = new SendSMSOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }


            if (!secretKey(skey, mr.signDate, mr.secret))
            {
                output.retCode.code = "-7031";
                output.retCode.message = "秘钥错误";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            using (CRMEntities db = new CRMEntities())
            {
                var query = db.V_U_TM_Mem_Info.Where(i => i.Mobile == mr.mobileNums).FirstOrDefault();

                if (query == null)
                {
                    output.retCode.code = "51";
                    output.retCode.message = "没找到会员";
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }

                TM_Sys_SMSSendingQueue sms = new TM_Sys_SMSSendingQueue();
                sms.Mobile = mr.mobileNums;
                sms.Message = mr.content;
                sms.MemberID = query.MemberID;
                sms.AddedDate = DateTime.Now;
                sms.IsSent = false;
                sms.AddedUser = "JPos";

                db.TM_Sys_SMSSendingQueue.Add(sms);

                db.SaveChanges();




            }


            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  GetAllMemberGrades
        public string GetAllMemberGrades(string input)
        {
            var mr = JsonHelper.Deserialize<GetAllMemberGradesInput>(input);
            GetAllMemberGradesOutput output = new GetAllMemberGradesOutput();
            string msg = string.Empty;

            output.retCode = new TRetCode();
            output.memberGrades = new List<TMemberGrade>();

            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            if (!secretKey(skey, mr.signDate, mr.secret))
            {
                output.retCode.code = "-7031";
                output.retCode.message = "秘钥错误";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            using (CRMEntities db = new CRMEntities())
            {
                var levels = db.V_M_TM_SYS_BaseData_customerlevel;
                if (levels != null)
                {
                    foreach (var level in levels)
                    {
                        TMemberGrade grade = new TMemberGrade();
                        grade.ucn = new TUCN();
                        grade.ucn.code = level.CustomerLevelBase;
                        grade.ucn.name = level.CustomerLevelNameBase;
                        grade.function = "";
                        grade.discounts = new List<TDiscount>();

                        output.memberGrades.Add(grade);
                    }
                }

            }

            output.time = mr.time;
            output.tranId = mr.tranId;
            output.tranTime = mr.tranTime;
            output.operatorStr = mr.operatorStr;



            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  QueryMemberSource
        public string QueryMemberSource(string input)
        {
            var mr = JsonHelper.Deserialize<QueryMemberSourceInput>(input);
            QueryMemberSourceOutput output = new QueryMemberSourceOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            output.memberSourceRecs = new List<TMemberSourceRec>();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            if (!secretKey(skey, mr.signDate, mr.secret))
            {
                output.retCode.code = "-7031";
                output.retCode.message = "秘钥错误";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            using (CRMEntities db = new CRMEntities())
            {
                var levels = db.TD_SYS_BizOption.Where(i => i.OptionType == "CustomerSource");
                if (levels != null)
                {
                    foreach (var level in levels)
                    {
                        TMemberSourceRec source = new TMemberSourceRec();
                        source.code = level.OptionValue;
                        source.name = level.OptionText;
                        source.remark = "";
                        source.isDefault = true;

                        output.memberSourceRecs.Add(source);
                    }
                }

            }

            output.time = mr.time;
            output.tranId = mr.tranId;
            output.tranTime = mr.tranTime;
            output.operatorStr = mr.operatorStr;


            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  GetMemberAttributeOptions
        public string GetMemberAttributeOptions(string input)
        {
            var mr = JsonHelper.Deserialize<GetMemberAttributeOptionsInput>(input);
            GetMemberAttributeOptionsOutput output = new GetMemberAttributeOptionsOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            output.memberAttributeOptions = new List<TMemberAttributeOptions>();
            output.memberExtAttribute = new List<TMemberAttributeOptions>();

            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }


            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  SaveMember
        public string SaveMember(string input)
        {
            var mr = JsonHelper.Deserialize<SaveMemberInput>(input);

            if (mr.sex == "male")
            {
                mr.sex = "男";
            }

            if (mr.sex == "female")
            {
                mr.sex = "女";
            }

            if (mr.paperType == "IDCARD")
            {
                mr.paperType = "1";
            }

            if (mr.paperType == "OTHERPAPER")
            {
                mr.paperType = "4";
            }



            SaveMemberOutput output = new SaveMemberOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();

            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            if (!secretKey(skey, mr.signDate, mr.secret))
            {
                output.retCode.code = "-7031";
                output.retCode.message = "秘钥错误";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_Mem_Ext.Where(i => i.MemberID == mr.code).FirstOrDefault();
                string mid = Guid.NewGuid().ToString("N");
                var member2 = db.V_U_TM_Mem_Info.Where(p => p.Mobile == mr.cellPhone).FirstOrDefault();

                var RegexStr = new Regex(@"^1\d{10}$");



                if (member2 != null)
                {
                    output.retCode.code = "-7033";
                    output.retCode.message = "手机号已经存在";
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
                else if (!RegexStr.IsMatch(mr.cellPhone))
                {
                    output.retCode.code = "-7034";
                    output.retCode.message = "手机号格式不正确";
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }





                if (!string.IsNullOrEmpty(mr.cardNum))
                {
                    var member1 = db.TM_Mem_Card.Where(p => p.MemberID == mr.memberId && p.Active == true).FirstOrDefault();
                    if (member1 != null)
                    {
                        if (member1.CardNo != mr.cardNum)
                        {
                            output.retCode.code = "-7032";
                            output.retCode.message = "会员卡号异常";
                            msg = JsonHelper.Serialize(output);
                            return msg;
                        }
                    }
                    else
                    {
                        var cardNo = db.TM_Card_CardNo.Where(p => p.CardNo == mr.cardNum).FirstOrDefault();
                        if (cardNo == null)
                        {
                            output.retCode.code = "-7050";
                            output.retCode.message = "卡号未在卡库中";
                            msg = JsonHelper.Serialize(output);
                            return msg;
                        }
                        else if (cardNo.IsUsed == true)
                        {
                            output.retCode.code = "-7051";
                            output.retCode.message = "卡号已经被使用";
                            msg = JsonHelper.Serialize(output);
                            return msg;
                        }
                        else if (cardNo.Status != "2")
                        {
                            output.retCode.code = "-7052";
                            output.retCode.message = "该卡目前不能使用";
                            msg = JsonHelper.Serialize(output);
                            return msg;
                        }

                        else if (cardNo.StoreCode != mr.storeCode)
                        {
                            output.retCode.code = "-7053";
                            output.retCode.message = "该卡门店编号与卡库中不符";
                            msg = JsonHelper.Serialize(output);
                            return msg;
                        }
                        else
                        {
                            if (query == null)
                            {
                                TM_Mem_Card card = new TM_Mem_Card()
                                {
                                    CardNo = mr.cardNum,
                                    MemberID = mid,
                                    CardType = "1",
                                    Active = true,
                                    Lock = false,
                                    AddedDate = DateTime.Now,
                                    AddedUser = "JPos",
                                    IsReissured = false,
                                    Status = 3
                                };
                                db.TM_Mem_Card.Add(card);
                            }
                        }
                    }
                }



                //var query = db.TM_Mem_Ext.Where(i => i.MemberID == mr.code).FirstOrDefault();
                if (query != null)
                {
                    Log4netHelper.WriteInfoLog("333333333333");
                    query.Str_Attr_2 = mr.cardNum;
                    query.Str_Attr_3 = mr.name;
                    query.Str_Attr_4 = mr.cellPhone;
                    query.Str_Attr_6 = mr.email;
                    query.Str_Attr_7 = mr.sex;
                    query.Str_Attr_8 = mr.paperType;//证件类型
                    query.Str_Attr_9 = mr.paperCode;//证件号
                    query.Str_Attr_11 = mr.provinceName;//省
                    query.Str_Attr_12 = mr.cityName;//市
                    query.Str_Attr_13 = mr.districtName;//区
                    query.Str_Attr_18 = mr.address;//现居地址
                    //Str_Attr_15 = meminfo.MemType,//会员类型
                    //Str_Attr_5 = meminfo.StoreCode,//注册门店
                    query.Str_Attr_20 = mr.provinceCode;//省代码
                    query.Str_Attr_21 = mr.cityCode;//市代码
                    //Str_Attr_34 = isOldMem == true ? "1" : "0", //是否老会员
                    query.Str_Attr_22 = mr.districtCode;//区代码
                    //Str_Attr_33 = meminfo.Wechat,//微信号
                    //Str_Attr_30 = meminfo.Region,//所属区域
                    //Str_Attr_31 = meminfo.Brand,//所属品牌
                    //Str_Attr_10 = meminfo.Department.ToString(),//业务部门ID
                    query.Date_Attr_2 = mr.birthday;//生日
                    query.Str_Attr_5 = mr.orgCode;//注册门店
                    query.Str_Attr_53 = mr.orgCode; //门店名称 通过门店代码关联
                    query.Str_Attr_52 = db.V_M_TM_SYS_BaseData_store.Where(i => i.StoreCode == mr.orgCode).Select(j => j.ChannelCodeStore).FirstOrDefault();//代理商 通过门店代码关联
                    var entry = db.Entry<TM_Mem_Ext>(query);
                    entry.State = EntityState.Modified;
                    db.TM_Mem_Master.Where(p => p.MemberID == query.MemberID).FirstOrDefault().Str_Key_1 = mr.cellPhone;
                    db.SaveChanges();

                }
                else
                {
                    TM_Mem_Ext ext = new TM_Mem_Ext()
                    {
                        MemberID = mid,
                        //Str_Attr_1 = isOldMem == true ? "0" : "1",//会员状态
                        Str_Attr_1 = "1",
                        Str_Attr_2 = mr.cardNum,//会员卡号
                        Str_Attr_3 = mr.name,//会员姓名
                        Str_Attr_4 = mr.cellPhone,//手机号
                        Str_Attr_6 = mr.email,//邮箱
                        Str_Attr_7 = mr.sex,//性别
                        Str_Attr_8 = mr.paperType,//证件类型
                        Str_Attr_9 = mr.paperCode,//证件号
                        Str_Attr_11 = mr.provinceName,//省
                        Str_Attr_12 = mr.cityName,//市
                        Str_Attr_13 = mr.districtName,//区
                        Str_Attr_18 = mr.address,//现居地址
                        //Str_Attr_15 = meminfo.MemType,//会员类型
                        Str_Attr_5 = mr.orgCode,//注册门店
                        Str_Attr_53 = mr.orgCode, //门店名称 通过门店代码关联
                        Str_Attr_52 = db.V_M_TM_SYS_BaseData_store.Where(i => i.StoreCode == mr.orgCode).Select(j => j.ChannelCodeStore).FirstOrDefault(),//代理商 通过门店代码关联


                        Str_Attr_20 = mr.provinceCode,//省代码
                        Str_Attr_21 = mr.cityCode,//市代码
                        //Str_Attr_34 = isOldMem == true ? "1" : "0", //是否老会员
                        Str_Attr_22 = mr.districtCode,//区代码
                        //Str_Attr_33 = meminfo.Wechat,//微信号
                        //Str_Attr_30 = meminfo.Region,//所属区域
                        //Str_Attr_31 = meminfo.Brand,//所属品牌
                        //Str_Attr_10 = meminfo.Department.ToString(),//业务部门ID
                        Date_Attr_2 = mr.birthday,//生日
                        AddedDate = DateTime.Now,
                        AddedUser = "JPos",
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = "JPos",
                    };
                    db.TM_Mem_Ext.Add(ext);


                    TM_Mem_Master master = new TM_Mem_Master()
                    {
                        MemberID = mid,
                        MemberGrade = 1,
                        MemberLevel = "001",
                        DataGroupID = 1,
                        Str_Key_1 = mr.cellPhone,

                        //Str_Key_3 = pass,
                        AddedDate = DateTime.Now,
                        AddedUser = "JPos",
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = "JPos",
                    };
                    db.TM_Mem_Master.Add(master);

                    TM_Loy_MemExt memext = new TM_Loy_MemExt()
                    {
                        MemberID = mid,
                        AddedDate = DateTime.Now,
                        AddedUser = "JPos",
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = "JPos",
                        //Dec_Attr_23 = meminfo.TotalGold,
                    };
                    db.TM_Loy_MemExt.Add(memext);

                    TM_Mem_Account account = new TM_Mem_Account();
                    account.AccountID = ToolsHelper.GuidtoString(Guid.NewGuid());
                    account.MemberID = mid;
                    account.AccountType = "3";
                    account.AddedDate = DateTime.Now;
                    account.AddedUser = "JPos";
                    account.ModifiedDate = DateTime.Now;
                    account.ModifiedUser = "JPos";
                    db.TM_Mem_Account.Add(account);





                    db.SaveChanges();
                }

            }



            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  BatchSaleCardPrepare
        public string BatchSaleCardPrepare(string input)
        {
            TRetCode retCode = new TRetCode();
            var mr = JsonHelper.Deserialize<BatchSaleCardPrepareInput>(input);

            BatchSaleCardPrepareOutput output = new BatchSaleCardPrepareOutput();

            output.retCode = new TRetCode();
            output.cardInfo = new List<TCardInfoRec>();

            string msg = string.Empty;

            if (mr == null)
            {
                retCode.code = "-1";
                retCode.message = "操作失败";
                output.retCode = retCode;
                msg = JsonHelper.Serialize(output);
                return msg;
            }



            if (!secretKey(skey, mr.signDate, mr.secret))
            {
                retCode.code = "-7031";
                retCode.message = "秘钥错误";
                output.retCode = retCode;
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_JPOS_CardSalesPrepare.Where(p => p.tranId == mr.tranId);
                if (query.Count() > 0)
                {
                    retCode.code = "304";
                    retCode.message = "交易执行中";
                    output.retCode = retCode;
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
                foreach (var item in mr.cardInfo)
                {
                    var queryC = db.TM_Card_CardNo.Where(p => p.CardNo == item.cardNumber).FirstOrDefault();
                    if (queryC == null)
                    {
                        retCode.code = "107";
                        retCode.message = "卡不存在";
                        output.retCode = retCode;
                        msg = JsonHelper.Serialize(output);
                        return msg;
                    }
                    if (queryC.IsSalesStatus == 1)
                    {
                        retCode.code = "103";
                        retCode.message = "卡已发售";
                        output.retCode = retCode;
                        msg = JsonHelper.Serialize(output);
                        return msg;
                    }
                }


                TM_JPOS_CardSalesPrepare tc = new TM_JPOS_CardSalesPrepare();
                tc.AddedDate = DateTime.Now;
                tc.cardInfo = JsonHelper.Serialize(mr.cardInfo);
                tc.id = Guid.NewGuid().ToString("N");
                tc.isConfirm = false;
                tc.operatorStr = mr.operatorStr;
                tc.posNo = mr.posNo;
                tc.storeCode = mr.storeCode;
                tc.time = mr.time;
                tc.tranId = mr.tranId;
                tc.tranTime = mr.tranTime;
                tc.xid = mr.Xid;
                db.TM_JPOS_CardSalesPrepare.Add(tc);
                db.SaveChanges();
            }
            output.time = mr.time;
            output.tranId = mr.tranId;
            output.tranTime = mr.tranTime;
            output.operatorStr = mr.operatorStr;
            output.Xid = mr.Xid;
            output.payTotal = 0;
            output.feeToal = 0;
            output.retCode = retCode;
            retCode.code = "96960";
            retCode.message = "OK";
            msg = JsonHelper.Serialize(output);

            return msg;
        }
        #endregion

        #region  BatchSaleCardConfirm
        public string BatchSaleCardConfirm(string input)
        {
            var mr = JsonHelper.Deserialize<BatchSaleCardConfirmInput>(input);
            BatchSaleCardConfirmOutput output = new BatchSaleCardConfirmOutput();
            TRetCode retCode = new TRetCode();

            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_JPOS_CardSalesPrepare.Where(p => p.tranId == mr.prepareOwnerTranId && p.isCancel == false && p.isConfirm == false).FirstOrDefault();
                if (query == null)
                    Log4netHelper.WriteErrorLog("BatchSaleCardConfirm " + mr.prepareOwnerTranId + "不存在或状态异常");
                else
                {
                    query.isConfirm = true;
                    query.confirmtradId = mr.tranId;
                    query.ConfirmDate = DateTime.Now;

                    var cardInfo = JsonHelper.Deserialize<List<TCardInfoRec>>(query.cardInfo);
                    foreach (var item in cardInfo)
                    {
                        var c = db.TM_Card_CardNo.Where(p => p.CardNo == item.cardNumber).FirstOrDefault();
                        if (c == null)
                            Log4netHelper.WriteErrorLog("BatchSaleCardConfirm" + item.cardNumber + "卡不存在");
                        c.IsSalesStatus = 1; c.Status = "2";
                    }
                    db.SaveChanges();
                }
            }
            retCode.code = "96960";
            retCode.message = "OK";
            output.retCode = retCode;

            var msg = JsonHelper.Serialize(output);

            return msg;
        }
        #endregion

        #region  BatchSaleCardPrepareCancel
        public string BatchSaleCardPrepareCancel(string input)
        {
            var mr = JsonHelper.Deserialize<BatchSaleCardPrepareCancelInput>(input);
            BatchSaleCardPrepareCancelOutput output = new BatchSaleCardPrepareCancelOutput();
            string msg = string.Empty;
            TRetCode retCode = new TRetCode();

            if (mr == null)
            {
                retCode.code = "-1";
                retCode.message = "操作失败";
                output.retCode = retCode;
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_JPOS_CardSalesPrepare.Where(p => p.tranId == mr.oldTranId && p.isCancel == false && p.isConfirm == false).FirstOrDefault();
                if (query == null)
                    Log4netHelper.WriteErrorLog("BatchSaleCardPrepareCancel " + mr.oldTranId + "不存在或状态异常");
                else
                {
                    query.isCancel = true;
                    query.canceltradId = mr.tranId;
                    query.ConfirmDate = DateTime.Now;
                    db.SaveChanges();
                }
            }


            retCode.code = "96960";
            retCode.message = "OK";
            output.retCode = retCode;
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  ScoreSave
        public string ScoreSave(string input)
        {
            decimal totalScore = 0;//记录总积分

            var mr = JsonHelper.Deserialize<ScoreSaveInput>(input);
            ScoreSaveOutput output = new ScoreSaveOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            if (mr.accountType < 0
                || mr.accountType > 6
                || string.IsNullOrEmpty(mr.accountAccessCode))
            {
                output.retCode.code = "-7037";
                output.retCode.message = "RequiredFieldError";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            if (!secretKey(skey, mr.signDate, mr.secret))
            {
                output.retCode.code = "-7031";
                output.retCode.message = "WrongSecret";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            using (CRMEntities db = new CRMEntities())
            {
                TE_MEM_ScoreSave score = new TE_MEM_ScoreSave();
                score.ID = Guid.NewGuid().ToString("N");
                score.Time = mr.time;
                score.OperatorStr = mr.operatorStr;
                score.StoreCode = mr.storeCode;
                score.TranId = mr.tranId;
                score.TranTime = mr.tranTime;
                score.PosNo = mr.posNo;
                score.AccountType = mr.accountType;
                score.CardType = mr.cardType;
                score.Media = mr.media;
                score.Xid = mr.xid;
                score.Scores = JsonHelper.Serialize(mr.scores);
                foreach (var s in mr.scores)
                {
                    totalScore += s.score;
                }
                score.TotalScores = totalScore;
                score.Status = false;
                score.AccountAccessCode = mr.accountAccessCode;
                score.OldXid = mr.oldXid;
                db.TE_MEM_ScoreSave.Add(score);
                db.SaveChanges();
                db.Database.ExecuteSqlCommand("exec sp_Loy_AccountPointUpdate");
            }
            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  SaleCard
        public string SaleCard(string input)
        {
            var mr = JsonHelper.Deserialize<SaleCardInput>(input);
            SaleCardOutput output = new SaleCardOutput();
            string msg = string.Empty;
            TRetCode retCode = new TRetCode();
            if (mr == null)
            {
                retCode.code = "-1";
                retCode.message = "操作失败";
                output.retCode = retCode;
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            using (CRMEntities db = new CRMEntities())
            {
                var cardNo = db.TM_Card_CardNo.Where(i => i.CardNo == mr.cardInfo.cardNumber).FirstOrDefault();
                if (cardNo == null)
                {
                    retCode.code = "107";
                    retCode.message = "卡不存在";
                    output.retCode = retCode;
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
                if (cardNo.IsUsed)
                {
                    retCode.code = "9903";
                    retCode.message = "卡已被使用";
                    output.retCode = retCode;
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
                var card = db.TM_Mem_Card.Where(i => i.CardNo == mr.cardInfo.cardNumber).FirstOrDefault();
                if (card != null)
                {
                    retCode.code = "9900";
                    retCode.message = "已存在该卡号";
                    output.retCode = retCode;
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
                string storecode = cardNo.StoreCode;
                if (storecode == null || storecode != mr.storeCode)
                {
                    retCode.code = "9901";
                    retCode.message = "门店代码不匹配";
                    output.retCode = retCode;
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
                cardNo.IsUsed = true;
                TM_Mem_Card newCard = new TM_Mem_Card();
                newCard.CardNo = mr.cardInfo.cardNumber;
                newCard.MemberID = mr.cardInfo.cardHolder;
                newCard.CardType = "1";
                newCard.Active = true;
                newCard.Lock = false;
                newCard.Status = 3;
                newCard.AddedDate = DateTime.Now;
                newCard.AddedUser = "JPOS";
                db.TM_Mem_Card.Add(newCard);
                db.SaveChanges();
            }
            retCode.code = "96960";
            retCode.message = "OK";
            output.retCode = retCode;
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  ChangeCardPassword
        public string ChangeCardPassword(string input)
        {
            var mr = JsonHelper.Deserialize<ChangeCardPasswordInput>(input);
            ChangeCardPasswordOutput output = new ChangeCardPasswordOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            if (!secretKey(skey, mr.signDate, mr.secret))
            {
                output.retCode.code = "-7031";
                output.retCode.message = "秘钥错误";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_JPOS_PasswordPrepare.Where(i => i.CardNumber == mr.cardInfo.cardNumber && i.IsConfirm == false).FirstOrDefault();
                if (query != null)
                {
                    query.IsConfirm = true;
                    var master = db.TM_Mem_Master.Where(i => i.MemberID == mr.cardInfo.cardHolder).FirstOrDefault();
                    if (master == null)
                    {
                        output.retCode.code = "51";
                        output.retCode.message = "没找到会员";
                        msg = JsonHelper.Serialize(output);
                        return msg;
                    }
                    else
                    {
                        if (mr.oldPassword != master.Str_Key_3)
                        {
                            output.retCode.code = "12";
                            output.retCode.message = "密码错误";
                            msg = JsonHelper.Serialize(output);
                            return msg;
                        }
                        else
                        {
                            master.Str_Key_3 = mr.newPassword;
                        }

                    }

                }
                db.SaveChanges();
            }

            output.time = mr.time;
            output.tranId = mr.tranId;
            output.tranTime = mr.tranTime;
            output.operatorStr = mr.operatorStr;

            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  ChangePasswordPrepare
        public string ChangePasswordPrepare(string input)
        {
            var mr = JsonHelper.Deserialize<ChangePasswordPrepareInput>(input);
            ChangePasswordPrepareOutput output = new ChangePasswordPrepareOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            if (!secretKey(skey, mr.signDate, mr.secret))
            {
                output.retCode.code = "-7031";
                output.retCode.message = "秘钥错误";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            string mid = Guid.NewGuid().ToString("N");



            using (CRMEntities db = new CRMEntities())
            {
                var query = db.TM_JPOS_PasswordPrepare.Where(i => i.CardNumber == mr.cardInfo.cardNumber && i.IsConfirm == false);
                if (query.Count() > 0)
                {
                    output.retCode.code = "-7027";
                    output.retCode.message = "卡密码prepare重复";
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }


                TM_JPOS_PasswordPrepare pass = new TM_JPOS_PasswordPrepare();
                pass.Id = mid;
                pass.Time = mr.time;
                pass.OperatorStr = mr.operatorStr;
                pass.storeCode = mr.storeCode;
                pass.TranId = mr.tranId;
                pass.TranTime = mr.tranTime;
                pass.PosNo = mr.posNo;
                pass.CardInfo = JsonHelper.Serialize(mr.cardInfo);
                pass.NewPassword = mr.newPassword;
                pass.OldPassword = mr.oldPassword;
                pass.IsConfirm = false;
                pass.CardNumber = mr.cardInfo.cardNumber;
                pass.CardHolder = mr.cardInfo.cardHolder;
                db.TM_JPOS_PasswordPrepare.Add(pass);

                db.SaveChanges();
            }






            output.time = mr.time;
            output.tranId = mr.tranId;
            output.tranTime = mr.tranTime;
            output.operatorStr = mr.operatorStr;

            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  DepositPrepare
        public string DepositPrepare(string input)
        {
            var mr = JsonHelper.Deserialize<DepositPrepareInput>(input);
            DepositPrepareOutput output = new DepositPrepareOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            output.cardInfo = new TCardInfoRec();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }


            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  Deposit
        public string Deposit(string input)
        {
            var mr = JsonHelper.Deserialize<DepositInput>(input);
            DepositOutput output = new DepositOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            output.cardInfo = new TCardInfoRec();
            output.deposits = new List<TDepositRec>();

            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }


            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  QueryCardScore
        public string QueryCardScore(string input)
        {
            var mr = JsonHelper.Deserialize<QueryCardScoreInput>(input);
            QueryCardScoreOutput output = new QueryCardScoreOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            output.scores = new List<TScoreRec>();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            if (mr.accountType < 0
               || mr.accountType > 6
               || string.IsNullOrEmpty(mr.accountAccessCode) || string.IsNullOrEmpty(mr.storeCode))
            {
                output.retCode.code = "-7030";
                output.retCode.message = "必填项为空或有误";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            using (CRMEntities db = new CRMEntities())
            {
                var query_con = from a in db.TM_Mem_Card
                                join b in db.V_U_TM_Mem_Info on a.MemberID equals b.MemberID
                                join c in db.TM_Mem_Account on a.MemberID equals c.MemberID
                                select new
                                {
                                    a.CardNo,
                                    b.Mobile,
                                    b.MemberID,
                                    b.CertificateNo,
                                    b.CustomerName,
                                    c.Value1,
                                    c.AccountID,
                                };
                var query_t = query_con;

                switch (mr.accountType)
                {
                    case 0:
                        query_t = query_con.Where(p => p.CardNo == mr.accountAccessCode);
                        break;
                    case 1:
                        query_t = query_con.Where(p => p.Mobile == mr.accountAccessCode);
                        break;
                    case 2:
                        query_t = query_con.Where(p => p.MemberID == mr.accountAccessCode);
                        break;
                    case 3:
                        query_t = query_con.Where(p => p.CertificateNo == mr.accountAccessCode);
                        break;
                    default:
                        query_t = null;
                        break;
                }

                if (query_t != null)
                {
                    var query = query_t.FirstOrDefault();

                    if (query != null)
                    {
                        Log4netHelper.WriteInfoLog(JsonHelper.Serialize(query));
                        TScoreRec detail = new TScoreRec();
                        detail.tranId = mr.tranId;
                        detail.scoreSubject = "";
                        detail.scoreType = "001";
                        detail.type = "001";
                        detail.scoreTypeName = "婷美积分";
                        detail.score = query.Value1;
                        detail.scoreSource = "";
                        detail.scoreRemark = "";
                        detail.total = 0;
                        detail.shareWay = 0;
                        detail.multiple = 0;

                        output.scores.Add(detail);


                        output.cardNumber = query.CardNo;
                        output.memberId = query.MemberID;
                        output.memberName = query.CustomerName;
                        output.telphone = query.Mobile;
                        output.accountType = mr.accountType;
                        output.currScore = query.Value1;
                    }
                }
            }
            output.tranId = mr.tranId;
            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  QueryPrizeGoodsList
        public string QueryPrizeGoodsList(string input)
        {
            var mr = JsonHelper.Deserialize<QueryPrizeGoodsListInput>(input);
            QueryPrizeGoodsListOutput output = new QueryPrizeGoodsListOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            output.prizeGoodsList = new List<TPrizeGoodsRec>();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            using (var db = new CRMEntities())
            {
                var dt = DateTime.Now;
                var query = from d in db.TM_JPOS_ExchangeGoodsDetail
                            join g in db.TD_Sys_Product on d.GoodsID equals g.Gid
                            join t in db.TM_JPOS_ExchangeGoods on d.ExchangeID equals t.ExchangeID
                            where t.StartDate <= dt && t.EndDate == null ? true : t.EndDate >= dt
                            select new { d, g, t };
                foreach (var item in query.ToList())
                {
                    TPrizeGoodsRec tg = new TPrizeGoodsRec();
                    tg.gdgid = item.d.GoodsID;
                    tg.minUnit = 1;
                    tg.qpc = 1;
                    tg.goodsCode = item.d.GoodsCode;
                    tg.goodsName = item.g.GoodsName;
                    tg.maxCount = Convert.ToInt32(item.d.MaxCounts);
                    tg.count = Convert.ToInt32(item.d.MinCounts);
                    tg.deductiblePrice = item.d.DiscountValue == null ? 0 : Convert.ToDecimal(item.d.DiscountValue);
                    List<TScoreRec> sl = new List<TScoreRec>();
                    TScoreRec tr = new TScoreRec();
                    TPrizeRuleRec trr = new TPrizeRuleRec();
                    trr.ruleUuid = item.d.ExchangeID.Trim();
                    trr.maxPrizeTimes = Convert.ToInt32(item.d.MaxCounts);
                    trr.sharePercent = 0;
                    trr.isTotalPrize = item.d.DiscountValue == 0 ? true : false;
                    tr.score = item.d.Integral;
                    tr.tranId = mr.tranId;
                    tr.scoreSubject = "";
                    tr.scoreTypeName = "婷美积分";
                    tr.scoreType = "001";
                    tr.type = "001";
                    tr.scoreSource = "JPOS";
                    tr.scoreRemark = "";
                    tr.total = 0;
                    tr.shareWay = 1;
                    tr.multiple = 0;
                    sl.Add(tr);
                    tg.scores = sl;
                    tg.rule = trr;
                    output.prizeGoodsList.Add(tg);

                }
            }
            output.tranId = mr.tranId;
            output.tranTime = mr.tranTime;
            output.prizeType_key = 0;
            output.accountAccessCode = mr.accountAccessCode;
            output.accountAccessCodeType = mr.accountAccessCodeType;
            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  QueryPrizeGoods
        public string QueryPrizeGoods(string input)
        {
            var mr = JsonHelper.Deserialize<QueryPrizeGoodsInput>(input);
            QueryPrizeGoodsOutput output = new QueryPrizeGoodsOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            output.prizeGoods = new TPrizeGoodsRec();
            output.prizeGoods.scores = new List<TScoreRec>();
            output.prizeGoods.rule = new TPrizeRuleRec();

            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }
            using (var db = new CRMEntities())
            {
                var dt = DateTime.Now;
                var query = (from d in db.TM_JPOS_ExchangeGoodsDetail
                             join g in db.TD_Sys_Product on d.GoodsID equals g.Gid
                             join t in db.TM_JPOS_ExchangeGoods on d.ExchangeID equals t.ExchangeID
                             where t.StartDate <= dt && t.EndDate == null ? true : t.EndDate >= dt
                             select new { d, g, t }).FirstOrDefault();
                if (query == null)
                {
                    output.retCode.code = "-1";
                    output.retCode.message = "商品不存在";
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
                TPrizeGoodsRec tg = new TPrizeGoodsRec();
                tg.gdgid = query.d.GoodsID;
                tg.qpc = 0;
                tg.goodsCode = query.d.GoodsCode;
                tg.goodsName = query.g.GoodsName;
                tg.maxCount = Convert.ToInt32(query.d.MaxCounts);
                tg.count = Convert.ToInt32(query.d.MinCounts);
                tg.deductiblePrice = query.d.DiscountValue == null ? 0 : Convert.ToDecimal(query.d.DiscountValue);
                output.prizeGoods = tg;
                TScoreRec tr = new TScoreRec();
                tr.score = query.d.Integral;
                tr.scoreType = "001";
                tr.type = "001";
                output.prizeGoods.scores.Add(tr);
            }
            output.tranId = mr.tranId;
            output.tranTime = mr.tranTime;
            output.operatorStr = mr.operatorStr;
            output.accountAccessCode = mr.accountAccessCode;
            output.accountAccessCodeType = mr.accountAccessCodeType;
            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  QueryMbrJoinRuleCount
        public string QueryMbrJoinRuleCount(string input)
        {
            var mr = JsonHelper.Deserialize<QueryMbrJoinRuleCountInput>(input);
            QueryMbrJoinRuleCountOutput output = new QueryMbrJoinRuleCountOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }
            using (var db = new CRMEntities())
            {
                var query_con = from a in db.TM_Mem_Card
                                join b in db.V_U_TM_Mem_Info on a.MemberID equals b.MemberID
                                select new
                                {
                                    a,
                                    b,
                                };
                switch (mr.accountAccessCodeType)
                {
                    case 0:
                        query_con = query_con.Where(p => p.a.CardNo == mr.accountAccessCode);
                        break;
                    case 1:
                        query_con = query_con.Where(p => p.b.Mobile == mr.accountAccessCode && p.a.Active && p.a.Lock == false);
                        break;
                    case 2:
                        query_con = query_con.Where(p => p.b.MemberID == mr.accountAccessCode);
                        break;
                    case 3:
                        query_con = query_con.Where(p => p.b.CertificateNo == mr.accountAccessCode);
                        break;
                    default:
                        query_con = null;
                        break;
                }
                if (query_con == null || query_con.FirstOrDefault() == null)
                {
                    output.retCode.code = "-1";
                    output.retCode.message = "会员不存在";
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }

                var querypromotion = db.TM_POS_MemberPromotion.Where(p => p.MemberID == query_con.FirstOrDefault().b.MemberID && p.PromotionID == mr.ruleUuid).FirstOrDefault();
                if (querypromotion == null)
                {
                    TM_POS_MemberPromotion tp = new TM_POS_MemberPromotion();
                    tp.PromotionID = mr.ruleUuid;
                    tp.MemberID = query_con.FirstOrDefault().b.MemberID;
                    tp.AddedDate = DateTime.Now;
                    tp.MemberCode = query_con.FirstOrDefault().a.CardNo;
                    tp.ModifiedDate = DateTime.Now;
                    tp.Counts = 0;
                    db.TM_POS_MemberPromotion.Add(tp);
                    output.count = 0;
                }
                else
                {
                    //querypromotion.Counts += 1;
                    output.count = querypromotion.Counts;
                }
                db.SaveChanges();
            }
            output.tranId = mr.tranId;
            output.tranTime = mr.tranTime;
            output.operatorStr = mr.operatorStr;
            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  QueryMemberGetPresentCounts
        public string QueryMemberGetPresentCounts(string input)
        {
            var mr = JsonHelper.Deserialize<QueryMemberGetPresentCountsInput>(input);
            QueryMemberGetPresentCountsOutput output = new QueryMemberGetPresentCountsOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }
            using (var db = new CRMEntities())
            {
                var query_con = from a in db.TM_Mem_Card
                                join b in db.V_U_TM_Mem_Info on a.MemberID equals b.MemberID
                                select new
                                {
                                    a,
                                    b,
                                };
                switch (mr.accountAccessCodeType)
                {
                    case 0:
                        query_con = query_con.Where(p => p.a.CardNo == mr.accountAccessCode);
                        break;
                    case 1:
                        query_con = query_con.Where(p => p.b.Mobile == mr.accountAccessCode && p.a.Active && p.a.Lock == false);
                        break;
                    case 2:
                        query_con = query_con.Where(p => p.b.MemberID == mr.accountAccessCode);
                        break;
                    case 3:
                        query_con = query_con.Where(p => p.b.CertificateNo == mr.accountAccessCode);
                        break;
                    default:
                        query_con = null;
                        break;
                }
                if (query_con == null || query_con.FirstOrDefault() == null)
                {
                    output.retCode.code = "-1";
                    output.retCode.message = "会员不存在";
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }

                var querypromotion = db.TM_POS_MemberPromotion.Where(p => p.MemberID == query_con.FirstOrDefault().b.MemberID && p.PromotionID == mr.ruleUuid).FirstOrDefault();
                if (querypromotion == null)
                {
                    TM_POS_MemberPromotion tp = new TM_POS_MemberPromotion();
                    tp.PromotionID = mr.ruleUuid;
                    tp.MemberID = query_con.FirstOrDefault().b.MemberID;
                    tp.AddedDate = DateTime.Now;
                    tp.MemberCode = query_con.FirstOrDefault().a.CardNo;
                    tp.ModifiedDate = DateTime.Now;
                    tp.Counts = 0;
                    db.TM_POS_MemberPromotion.Add(tp);
                    output.count = 0;
                }
                else
                {
                    querypromotion.Counts += 1;
                    output.count = querypromotion.Counts;
                }
                db.SaveChanges();

            }
            output.tranId = mr.tranId;
            output.tranTime = mr.tranTime;
            output.operatorStr = mr.operatorStr;
            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  Prize
        public string Prize(string input)
        {
            var mr = JsonHelper.Deserialize<PrizeInput>(input);
            PrizeOutput output = new PrizeOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            output.prizeGoodsList = new List<TPrizeGoodsRec>();
            output.externalScores = new List<TScoreRec>();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }
            using (var db = new CRMEntities())
            {
                decimal total = 0;
                foreach (var item in mr.prizeGoodsList)
                {
                    foreach (var s in item.scores)
                    {
                        if (s.score > 0)
                            total += s.score;
                    }

                }
                if (total <= 0)
                {
                    output.retCode.code = "-1";
                    output.retCode.message = "入参积分异常";
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
                var query_mem = from u in db.V_U_TM_Mem_Info
                                join c in db.TM_Mem_Card on u.MemberID equals c.MemberID
                                select new { u, c };
                switch (mr.accountAccessCodeType)
                {
                    case 0:
                        query_mem = query_mem.Where(p => p.c.CardNo == mr.accountAccessCode);
                        break;
                    case 1:
                        query_mem = query_mem.Where(p => p.u.Mobile == mr.accountAccessCode);
                        break;
                    case 2:
                        query_mem = query_mem.Where(p => p.u.MemberID == mr.accountAccessCode);
                        break;
                    case 3:
                        query_mem = query_mem.Where(p => p.u.CertificateNo == mr.accountAccessCode);
                        break;
                    default:
                        query_mem = null;
                        break;
                }
                if (query_mem.FirstOrDefault() == null)
                {
                    output.retCode.code = "-1";
                    output.retCode.message = "会员不存在";
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
                var account = db.TM_Mem_Account.Where(p => p.MemberID == query_mem.FirstOrDefault().u.MemberID).FirstOrDefault();
                if (total > account.Value1)
                {
                    output.retCode.code = "-1";
                    output.retCode.message = "会员积分不足";
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
                V_M_TM_Mem_Trade_exchange tm = new V_M_TM_Mem_Trade_exchange();
                tm.TradeCode = mr.tranId;
                tm.posNoExchange = mr.posNo;
                tm.StatusExchange = "0";
                tm.IntegralExchange = total;
                tm.MemberID = query_mem.FirstOrDefault().u.MemberID;
                tm.StoreCodeExchange = mr.storeCode;
                tm.DataGroupID = 1;
                tm.AddedDate = DateTime.Now;
                tm.AddedUser = "JPOS";
                tm.ModifiedDate = DateTime.Now;
                tm.ModifiedUser = "JPOS";
                tm.NeedLoyCompute = false;
                tm.ExchangeDate = mr.tranTime;
                var tid = db.AddViewRow<V_M_TM_Mem_Trade_exchange, TM_Mem_Trade>(tm);
                db.SaveChanges();

                foreach (var item in mr.prizeGoodsList)
                {
                    V_M_TM_Mem_TradeDetail_exchange_exchangeDetail tmm = new V_M_TM_Mem_TradeDetail_exchange_exchangeDetail();
                    tmm.TradeID = tid.TradeID;
                    tmm.GoodsIDExchangeDetail = item.gdgid;
                    tmm.MaxCountsExchangeDetail = item.maxCount;
                    tmm.CountsExchangeDetail = item.count;
                    db.AddViewRow<V_M_TM_Mem_TradeDetail_exchange_exchangeDetail, TM_Mem_TradeDetail>(tmm);
                }
                db.SaveChanges();
                try
                {
                    db.sp_Loy_DeductMemGoldCoinSingle(query_mem.FirstOrDefault().u.MemberID, "3", total, tid.TradeID.ToString(), null, "1", "deduct");
                }
                catch (Exception ex)
                {
                    Log4netHelper.WriteErrorLog("Prize-GoldCoin Error" + ex.ToString());
                    output.retCode.code = "-1";
                    output.retCode.message = "扣除积分异常";
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
            }
            output.tranId = mr.tranId;
            output.tranTime = mr.tranTime;
            output.operatorStr = mr.operatorStr;
            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  PrizeCancel
        public string PrizeCancel(string input)
        {
            var mr = JsonHelper.Deserialize<PrizeCancelInput>(input);
            PrizeCancelOutput output = new PrizeCancelOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }


            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  ScorePrize
        public string ScorePrize(string input)
        {
            var mr = JsonHelper.Deserialize<ScorePrizeInput>(input);
            ScorePrizeOutput output = new ScorePrizeOutput();
            TRetCode retCode = new TRetCode();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            if (mr == null)
            {
                retCode.code = "-1";
                retCode.message = "操作失败";
                output.retCode = retCode;
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            using (var db = new CRMEntities())
            {
                var query_mem = from u in db.V_U_TM_Mem_Info
                                join c in db.TM_Mem_Card on u.MemberID equals c.MemberID
                                where c.Active == true && c.Lock == false
                                select new { u, c };

                if (mr.cardInfo != null)
                    query_mem = query_mem.Where(p => p.c.CardNo == mr.cardInfo.cardNumber);

                switch (mr.accountType)
                {
                    case 0:
                        query_mem = query_mem.Where(p => p.c.CardNo == mr.accountAccessCode);
                        break;
                    case 1:
                        query_mem = query_mem.Where(p => p.u.Mobile == mr.accountAccessCode);
                        break;
                    case 2:
                        query_mem = query_mem.Where(p => p.u.MemberID == mr.accountAccessCode);
                        break;
                    case 3:
                        query_mem = query_mem.Where(p => p.u.CertificateNo == mr.accountAccessCode);
                        break;
                    default:
                        break;
                }
                if (query_mem.Count() == 0)
                {
                    retCode.code = "-1";
                    retCode.message = "会员不存在";
                    output.retCode = retCode;
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
                decimal oldV = 0;
                decimal newV = 0;
                decimal tValue = 0;
                foreach (var item in mr.scores)
                {
                    tValue += item.score;
                }
                if (tValue > 0)
                {
                    var account = db.TM_Mem_Account.Where(p => p.MemberID == query_mem.FirstOrDefault().u.MemberID).FirstOrDefault();
                    oldV = account.Value1; output.oldScore = oldV;
                    if (account.Value1 < tValue)
                    {
                        retCode.code = "-1";
                        retCode.message = "积分不足";
                        output.retCode = retCode;
                        msg = JsonHelper.Serialize(output);
                        return msg;
                    }
                    Log4netHelper.WriteInfoLog("S1");
                    var r = db.sp_Loy_DeductMemGoldCoinSingle(query_mem.FirstOrDefault().u.MemberID, account.AccountType, tValue, mr.tranId, null, "1", "deduct");
                    Log4netHelper.WriteInfoLog("S2");
                    newV = account.Value1;
                }

                output.currScore = newV;
            }

            retCode.code = "96960";
            retCode.message = "OK";
            output.retCode = retCode;
            output.tranId = mr.tranId;
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  mbrPay
        public string mbrPay(string input)
        {
            var mr = JsonHelper.Deserialize<mbrPayInput>(input);
            mbrPayOutput output = new mbrPayOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }


            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  ScorePrizeCancel
        public string ScorePrizeCancel(string input)
        {
            var mr = JsonHelper.Deserialize<ScorePrizeCancelInput>(input);
            ScorePrizeCancelOutput output = new ScorePrizeCancelOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            output.scores = new List<TScoreRec>();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }


            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  QueryVoucher
        public string QueryVoucher(string input)
        {
            var mr = JsonHelper.Deserialize<QueryVoucherInput>(input);
            QueryVoucherOutput output = new QueryVoucherOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            var vouchers = new List<TQueryVoucherRec>();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }
            using (CRMEntities db = new CRMEntities())
            {
                DateTime dt = DateTime.Now;
                var query = (from a in db.TM_Mem_CouponPool
                             join b in db.TM_JPOS_CouponUseRule on a.TempletID equals b.ID
                             where a.StartDate <= dt && a.EndDate > dt && a.BatchNo == mr.storeCode
                             select new
                             {
                                 a.CouponCode,
                                 a.Enable,
                                 b.CouponValue
                             }).ToList();


                foreach (var item in query)
                {
                    TQueryVoucherRec m = new TQueryVoucherRec();
                    m.voucherNo = item.CouponCode;
                    m.parValue = Convert.ToDecimal(item.CouponValue);
                    m.voucherFunc = 1;
                    if (item.Enable == true)
                        m.voucherStatus = "active";
                    vouchers.Add(m);
                }
            }
            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  VoucherAbolish
        public string VoucherAbolish(string input)
        {
            var mr = JsonHelper.Deserialize<VoucherAbolishInput>(input);
            VoucherAbolishOutput output = new VoucherAbolishOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }
            using (CRMEntities db = new CRMEntities())
            {
                var coupon = db.TM_Mem_CouponPool.Where(p => p.CouponCode == mr.voucherNo).FirstOrDefault();
                if (coupon == null)
                {
                    output.retCode.code = "-1";
                    output.retCode.message = "券号：" + mr.voucherNo + "不存在";
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
                coupon.Enable = false;
                db.SaveChanges();
            }

            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  VoucherActivate
        public string VoucherActivate(string input)
        {
            var mr = JsonHelper.Deserialize<VoucherActivateInput>(input);
            VoucherActivateOutput output = new VoucherActivateOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }
            using (CRMEntities db = new CRMEntities())
            {
                var coupon = db.TM_Mem_CouponPool.Where(p => p.CouponCode == mr.voucherNo).FirstOrDefault();
                if (coupon == null)
                {
                    output.retCode.code = "-1";
                    output.retCode.message = "券号：" + mr.voucherNo + "不存在";
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
                coupon.Enable = true;
                db.SaveChanges();
            }

            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  VoucherPrint
        public string VoucherPrint(string input)
        {
            var mr = JsonHelper.Deserialize<VoucherPrintInput>(input);
            VoucherPrintOutput output = new VoucherPrintOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }



            output.retCode.code = "96960";
            output.retCode.message = "OK";
            output.voucherNo = mr.voucherNo;
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  VoucherUse
        public string VoucherUse(string input)
        {
            var mr = JsonHelper.Deserialize<VoucherUseInput>(input);
            VoucherUseOutput output = new VoucherUseOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }
            using (CRMEntities db = new CRMEntities())
            {
                var coupon = db.TM_Mem_CouponPool.Where(p => p.CouponCode == mr.voucherNo).FirstOrDefault();
                if (coupon == null)
                {
                    output.retCode.code = "-1";
                    output.retCode.message = "券不存在";
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
                if (coupon.IsUsed == false)
                {
                    output.retCode.code = "-1";
                    output.retCode.message = "券已被使用";
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
                coupon.IsUsed = true;
                coupon.ReferenceNo = mr.xid;
                db.SaveChanges();
            }
            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  LocalData
        public string LocalData(string input)
        {
            var mr = JsonHelper.Deserialize<LocalDataInput>(input);
            LocalDataOutput output = new LocalDataOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }
            string url = "";
            using (var db = new CRMEntities())
            {
                var query = (from c in db.TM_JPOS_CouponList
                             join p in db.TM_JPOS_CouponPool on c.CouponListID equals p.ListId
                             where c.BeginDate <= DateTime.Now && c.EndDate >= DateTime.Now && p.IsDownLoad == false && p.StoreCode == mr.storeCode
                             select new { c, p }).FirstOrDefault();
                if (query != null)
                {
                    output.url = query.p.FilePath + "?termId=" + query.p.StoreCode + "&filename=" + query.p.FileName + "&sessionId=" + Guid.NewGuid().ToString("N");
                }
            }
            output.tranId = mr.tranId;
            output.tranTime = mr.tranTime;
            output.dataType = "storeIncreExclusiveData";
            output.time = mr.time;
            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        #region  LocalDataDone
        public string LocalDataDone(string input)
        {
            var mr = JsonHelper.Deserialize<LocalDataDoneInput>(input);
            LocalDataDoneOutput output = new LocalDataDoneOutput();
            string msg = string.Empty;
            output.retCode = new TRetCode();
            if (mr == null)
            {
                output.retCode.code = "-1";
                output.retCode.message = "操作失败";
                msg = JsonHelper.Serialize(output);
                return msg;
            }

            using (var db = new CRMEntities())
            {
                var query = db.TM_JPOS_CouponPool.Where(p => p.FileName == mr.fileName).FirstOrDefault();
                if (query == null)
                {
                    output.retCode.code = "-001";
                    output.retCode.message = "文件没有找到";
                    msg = JsonHelper.Serialize(output);
                    return msg;
                }
                query.IsDownLoad = true;
                db.SaveChanges();


            }


            output.retCode.code = "96960";
            output.retCode.message = "OK";
            msg = JsonHelper.Serialize(output);
            return msg;
        }
        #endregion

        /**********************************************************************/
        private static string resultJson(int status, string msg)
        {
            ResultJsonOutput jo = new ResultJsonOutput();
            jo.Status = status;
            jo.Message = msg;
            return JsonHelper.Serialize(jo);
        }

        private static bool secretKey(string key, string signdate, string secret)
        {
            if (ToolsHelper.MD5(ToolsHelper.DateTimeToStamp(Convert.ToDateTime(signdate)).ToString() + key) == secret)
                return true;
            else
                return false;

        }
    }
}
