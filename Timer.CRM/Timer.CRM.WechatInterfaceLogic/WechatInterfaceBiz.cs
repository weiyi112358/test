using Arvato.CRM.EF;
using Arvato.CRM.Model.Interface;
using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data.Objects.SqlClient;
using System.Data.SqlClient;

namespace Timer.CRM.WechatInterfaceLogic
{
    public class WechatInterfaceBiz
    {
        private string skey = ConfigurationSettings.AppSettings["skey"];

        #region 微信会员入会
        public string AddWeChatMember(string input)
        {
            var mr = JsonHelper.Deserialize<WeChatMemberInput>(input);
            if (mr == null)
                return resultJson(-1, "入参错误");
            if (!secretKey(skey, mr.signdate, mr.secret))
            {
                return resultJson(-7031, "秘钥错误");
            }
            using (CRMEntities db = new CRMEntities())
            {
                var openIdQuery = db.V_U_TM_Mem_Info.Where(p => p.MemberOpenID == mr.FOpenId).FirstOrDefault();
                if (openIdQuery != null)
                    return resultJson(-1, "该微信号已绑定");
                //查找手机号手否存在
                var mobileQuery = db.V_U_TM_Mem_Info.Where(x => x.Mobile == mr.FPhoneNum).FirstOrDefault();
                if (mobileQuery != null && (mobileQuery.MemberOpenID.Length > 5))
                    return resultJson(-1, "手机号已绑定");
                SqlParameter param1 = new SqlParameter("@MemberCardNo", System.Data.SqlDbType.BigInt);
                param1.Direction = System.Data.ParameterDirection.Output;
                int rows = db.Database.ExecuteSqlCommand("exec sp_CRM_ECMemberCardCreate @MemberCardNo out", param1);
                string cardNo = param1.Value.ToString();
                WeChatMemberOutput output = new WeChatMemberOutput();

                var guid = Guid.NewGuid().ToString("N");
                var guidAccout1 = Guid.NewGuid().ToString("N");
                var guidAccout2 = Guid.NewGuid().ToString("N");

                TM_Mem_Card card = new TM_Mem_Card();
                //老卡绑定
                if (mobileQuery != null && string.IsNullOrEmpty(mobileQuery.MemberOpenID))
                {
                    var master = db.TM_Mem_Master.Where(p => p.MemberID == mobileQuery.MemberID).FirstOrDefault();
                    master.Str_Key_4 = mr.FOpenId;
                    var memext = db.TM_Mem_Ext.Where(p => p.MemberID == mobileQuery.MemberID).FirstOrDefault();
                    memext.Str_Attr_2 = cardNo;//卡号
                    memext.Str_Attr_3 = mr.FName;//姓名
                    memext.Str_Attr_4 = mr.FPhoneNum;//手机号
                    memext.Str_Attr_7 = mr.FSex;//性别
                    memext.ModifiedDate = DateTime.Now;//创建时间
                    //TM_Mem_Card card = new TM_Mem_Card();
                    card.CardNo = cardNo;//电子卡号
                    card.MemberID = guid;
                    card.CardType = "2";//1=实体卡;2=电子卡号类型
                    card.Active = true;
                    card.Lock = false;
                    card.AddedUser = "WeChat";
                    card.AddedDate = DateTime.Now;
                    card.Status = '3';
                    db.TM_Mem_Card.Add(card);


                    db.SaveChanges();
                    output.Status = 1;
                    output.ErrorMsg = "成功";
                    output.FCardNum = cardNo;
                    return JsonHelper.Serialize(output);
                }


                card.CardNo = cardNo;//电子卡号
                card.MemberID = guid;
                card.CardType = "2";//1=实体卡;2=电子卡号类型
                card.Active = true;
                card.Lock = false;
                card.AddedUser = "WeChat";
                card.AddedDate = DateTime.Now;
                db.TM_Mem_Card.Add(card);


                TM_Mem_Master insertMaster = new TM_Mem_Master();
                insertMaster.MemberID = guid;//会员编号
                insertMaster.MemberLevel = "000";//会员等级
                insertMaster.DataGroupID = 1;//会员层级
                insertMaster.Str_Key_1 = mr.FPhoneNum;//会员手机号
                insertMaster.Str_Key_3 = ToolsHelper.MD5(mr.FPhoneNum.Substring(5, 6));//会员密码
                insertMaster.MemberGrade = 1;
                insertMaster.AddedDate = DateTime.Now;//创建时间
                insertMaster.AddedUser = "WeChat";
                insertMaster.ModifiedDate = DateTime.Now;
                insertMaster.ModifiedUser = "WeChat";
                insertMaster.Str_Key_4 = mr.FOpenId;//openid

                db.TM_Mem_Master.Add(insertMaster);


                TM_Mem_Ext insertExt = new TM_Mem_Ext();
                insertExt.Date_Attr_2 = mr.FBirthDay;
                insertExt.Str_Attr_1 = "1";
                insertExt.MemberID = guid;
                insertExt.Str_Attr_2 = cardNo;//卡号
                insertExt.Str_Attr_3 = mr.FName;//姓名
                insertExt.Str_Attr_4 = mr.FPhoneNum;//手机号
                insertExt.Str_Attr_7 = mr.FSex;//性别
                insertExt.AddedDate = DateTime.Now;//创建时间
                insertExt.AddedUser = "WeChat";
                insertExt.ModifiedDate = DateTime.Now;
                insertExt.ModifiedUser = "WeChat";
                db.TM_Mem_Ext.Add(insertExt);

                TM_Loy_MemExt insertMemExt = new TM_Loy_MemExt();
                insertMemExt.MemberID = guid;
                insertMemExt.AddedDate = DateTime.Now;//创建时间
                insertMemExt.AddedUser = "WeChat";
                insertMemExt.ModifiedDate = DateTime.Now;
                insertMemExt.ModifiedUser = "WeChat";
                db.TM_Loy_MemExt.Add(insertMemExt);




                TM_Mem_Account insertAccout_point = new TM_Mem_Account();
                insertAccout_point.AccountID = guidAccout2;
                insertAccout_point.MemberID = guid;
                insertAccout_point.AccountType = "3";
                insertAccout_point.Value1 = 0;
                insertAccout_point.Value2 = 0;
                insertAccout_point.Value3 = 0;
                insertAccout_point.NoInvoiceAccount = 0;
                insertAccout_point.NoBackAccount = 0;
                insertAccout_point.AddedDate = DateTime.Now;//创建时间
                insertAccout_point.AddedUser = "WeChat";
                insertAccout_point.ModifiedDate = DateTime.Now;
                insertAccout_point.ModifiedUser = "WeChat";
                db.TM_Mem_Account.Add(insertAccout_point);





                V_M_TM_Mem_SubExt_openid insertOpenid = new V_M_TM_Mem_SubExt_openid();
                insertOpenid.ExtType = "openid";
                insertOpenid.MemberID = guid;
                insertOpenid.OpenIDSubMember = mr.FOpenId;
                insertOpenid.OpenIDSource = "";
                db.AddViewRow<V_M_TM_Mem_SubExt_openid, TM_Mem_SubExt>(insertOpenid);




                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {

                    //foreach (var item in ex.EntityValidationErrors.ToList())
                    //{
                    //    foreach (var v in item.ValidationErrors.ToList())
                    //    {
                    //        Console.WriteLine(v.ErrorMessage + v.PropertyName);
                    //    }
                    //}

                    return "";
                }



                output.Status = 1;
                output.ErrorMsg = "成功";
                output.FCardNum = cardNo;
                return JsonHelper.Serialize(output);

            }


        }
        #endregion

        #region 绑定会员
        public string BindConsultant(string input)
        {
            var mr = JsonHelper.Deserialize<BindMemberInput>(input);
            if (mr == null)
                return resultJson(-1, "入参错误");
            if (!secretKey(skey, mr.signdate, mr.secret))
            {
                return resultJson(-7031, "秘钥错误");
            }

            using (CRMEntities db = new CRMEntities())
            {
                BindMemberOutput output = new BindMemberOutput();
                var queryMember = db.V_U_TM_Mem_Info.Where(i => i.MemberOpenID == mr.OpenId).FirstOrDefault();
                if (queryMember == null)
                {
                    output.Status = -1;
                    output.ErrorMsg = "未找到该会员";
                    return JsonHelper.Serialize(output);
                }
                var c = db.TD_SYS_WeChatConsultant.Where(p => p.LoginName == mr.LoginName).FirstOrDefault();
                if (c == null)
                {
                    output.Status = -1;
                    output.ErrorMsg = "专属顾问不存在";
                    return JsonHelper.Serialize(output);
                }


                var mem = db.TM_Mem_Ext.Where(p => p.MemberID == queryMember.MemberID).FirstOrDefault();
                mem.Str_Attr_33 = mr.LoginName;
                mem.Str_Attr_37 = c.UserName;
                var store = db.V_M_TM_SYS_BaseData_store.Where(p => p.StoreCode == c.StoreCode).FirstOrDefault();
                mem.Str_Attr_38 = store == null ? "" : store.StoreName;
                mem.Str_Attr_39 = c.StoreCode;

                db.SaveChanges();






                output.Status = 1;
                output.ErrorMsg = "成功";

                return JsonHelper.Serialize(output);



            }


            return resultJson(1, "入会成功");
        }
        #endregion

        #region 我的积分
        public string QueryPoints(string input)
        {
            var mr = JsonHelper.Deserialize<QueryPointsInput>(input);
            if (mr == null)
                return resultJson(-1, "入参错误");
            if (!secretKey(skey, mr.signdate, mr.secret))
            {
                return resultJson(-7031, "秘钥错误");
            }
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from a in db.TM_Mem_Account
                             join b in db.V_U_TM_Mem_Info on a.MemberID equals b.MemberID
                             where b.MemberCardNo == mr.FCardNum
                             select new
                             {
                                 a.Value1



                             }).FirstOrDefault();

                QueryPointsOutput output = new QueryPointsOutput();
                output.FAmount = query.Value1;
                output.FDeductAmount = 0;
                output.FFastExpire = 0;
                output.FExpireTime = DateTime.Now.AddYears(100);

                output.Status = 1;
                output.ErrorMsg = "成功";

                return JsonHelper.Serialize(output);


            }


        }
        #endregion

        #region 积分明细
        public string QueryPointsDetail(string input)
        {
            var mr = JsonHelper.Deserialize<QueryPointsDetailInput>(input);
            if (mr == null)
                return resultJson(-1, "入参错误");
            if (!secretKey(skey, mr.signdate, mr.secret))
            {
                return resultJson(-7031, "秘钥错误");
            }
            using (CRMEntities db = new CRMEntities())
            {
                //var query = (from a in db.V_U_TM_Mem_Info
                //             join cd in db.TM_Mem_Card on a.MemberID equals cd.MemberID
                //             join b in db.TM_Mem_Account on a.MemberID equals b.MemberID
                //             join c in db.TM_Mem_AccountDetail on b.AccountID equals c.AccountID
                //             where cd.CardNo == mr.FCardNum
                //             select new PointDetail
                //             {
                //                 FProdDate = c.AddedDate,

                //                 FType = c.AccountDetailType,
                //                 FUseIntegral = c.DetailValue,
                //             }
                //    ).ToList();

                var card = db.TM_Mem_Card.Where(p => p.CardNo == mr.FCardNum && !p.Lock && p.Active).FirstOrDefault();
                if (card == null)
                {
                    return resultJson(-7031, "会员卡号不存在");
                }


                var query = (from tl in db.TL_Mem_AccountChange
                             join ad in db.TM_Mem_AccountDetail on tl.AccountDetailID equals ad.AccountDetailID
                             join td in db.V_M_TM_Mem_Trade_sales on tl.JPOSNumber equals td.TradeCode into tdtmp
                             from tdd in tdtmp.DefaultIfEmpty()
                             join op in db.TD_SYS_BizOption.Where(p => p.OptionType == "AccountChangeType") on tl.AccountChangeType equals op.OptionValue
                             join s in db.V_M_TM_SYS_BaseData_store on tdd.ConsumeStoreCode equals s.StoreCode into stemp
                             from st in stemp.DefaultIfEmpty()
                             where tl.ChangeValue != 0 && tl.MemberID == card.MemberID
                             select new PointDetail
                             {
                                 FFastExpire = 0,
                                 FIntegralAmount = tl.ChangeValue,
                                 FProdDate = tl.AddedDate,
                                 FProdOrg = st.StoreName,
                                 FExpendAmount = tdd.RealAmtSales ?? 0,
                                 FType = op.OptionText,
                                 FUseIntegral = tl.ChangeValue,
                                 FFastDate = ad.SpecialDate2
                             }).ToList();

                QueryPointsDetailOutput output = new QueryPointsDetailOutput();
                output.PointDetailClass = query;
                output.Status = 1;
                output.ErrorMsg = "成功";
                return JsonHelper.Serialize(output);
            }
        }
        #endregion

        #region 我的等级
        public string QueryGrade(string input)
        {
            var mr = JsonHelper.Deserialize<QueryGradeInput>(input);
            if (mr == null)
                return resultJson(-1, "入参错误");
            if (!secretKey(skey, mr.signdate, mr.secret))
            {
                return resultJson(-7031, "秘钥错误");
            }
            using (CRMEntities db = new CRMEntities())
            {

                var mem = db.TM_Mem_Card.Where(p => p.CardType == "2" && p.CardNo == mr.FCardNum).FirstOrDefault();
                if (mem == null) return resultJson(-1, "卡号不存在");

                var cl = db.V_U_TM_Mem_Info.Where(p => p.MemberID == mem.MemberID).FirstOrDefault();
                var query = (from m in db.V_U_TM_Mem_Info
                             join l in db.V_M_TM_SYS_BaseData_customerlevel on m.CustomerLevel equals l.CustomerLevelBase
                             where m.MemberID == mem.MemberID
                             select new
                             {
                                 m.ConsumptionYearly,
                                 l.CustomerLevelBase,
                                 l.LevelUpInt,
                                 m.MemberLevelEndDate,
                             }).FirstOrDefault();
                QueryGradeOutput output = new QueryGradeOutput();
                switch (query.CustomerLevelBase)
                {
                    case "000":
                        output.FGradeType = "1";
                        break;
                    case "001":
                        output.FGradeType = "2";
                        break;
                    case "002":
                        output.FGradeType = "3";
                        break;
                    case "003":
                        output.FGradeType = "4";
                        break;
                    case "004":
                        output.FGradeType = "5";
                        break;
                    default:
                        output.FGradeType = query.CustomerLevelBase;
                        break;
                }



                output.FRebate = "";
                output.FGradeAmount = Convert.ToDecimal(query.LevelUpInt);
                output.FGradeDate = DateTime.Now.AddYears(1);
                output.FDifferAmount = Convert.ToDecimal(query.LevelUpInt) - Convert.ToDecimal(query.ConsumptionYearly == null ? 0 : query.ConsumptionYearly);

                output.Status = 1;
                output.ErrorMsg = "成功";

                return JsonHelper.Serialize(output);


            }


        }
        #endregion

        #region 成员同步
        public string QueryUser(string input)
        {
            var mr = JsonHelper.Deserialize<QueryUserInput>(input);
            if (mr == null)
                return resultJson(-1, "入参错误");
            if (!secretKey(skey, mr.signdate, mr.secret))
            {
                return resultJson(-7031, "秘钥错误");
            }
            using (CRMEntities db = new CRMEntities())
            {


                var query = db.TD_SYS_WeChatConsultant.ToList();
                List<User> Users = new List<User>();
                foreach (var item in query.ToList())
                {
                    User user = new User();
                    user.LoginName = item.LoginName;
                    user.UserName = item.UserName;
                    user.StoreCode = item.StoreCode;
                    user.FUpdateDate = Convert.ToDateTime(item.FUpdateDate).ToString("yyyy-MM-dd HH:ss:mm");
                    user.FType = item.FType;
                    user.FEable = true;
                    user.FEmail = item.FEmail;
                    user.FPhone = item.FPhone;
                    user.FWeixinID = item.FWeixinID;
                    user.FSex = item.FSex == true ? "男" : "女";
                    user.FPosition = item.FPosition;
                    user.FTell = item.FTell;
                    Users.Add(user);
                }
                QueryUserOutPut output = new QueryUserOutPut();
                output.Users = Users;
                output.Status = 1;
                output.ErrorMsg = "成功";

                return JsonHelper.Serialize(output);


            }
        }
        #endregion

        #region 组织架构同步
        /// <summary>
        /// 组织架构同步
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string QueryOrganization(string input)
        {
            var mr = JsonHelper.Deserialize<QueryOrganizationInput>(input);
            if (mr == null)
                return resultJson(-1, "入参错误");
            if (!secretKey(skey, mr.signdate, mr.secret))
            {
                return resultJson(-7031, "秘钥错误");
            }
            using (CRMEntities db = new CRMEntities())
            {

                var query_store = from s in db.V_M_TM_SYS_BaseData_store
                                  select new
                                  {
                                      BaseDataID = s.BaseDataID,
                                      StoreCode = s.StoreCode,
                                      StoreName = s.StoreName,
                                      ProvinceCodeStore = s.ProvinceStore,
                                      CityCodeStore = s.CityStore,
                                      StoreAddress = s.StoreAddress,
                                      ChannelCodeStore = s.ChannelCodeStore,
                                      ChannelNameStore = s.ChannelNameStore,
                                      UpdateTime = s.ModifiedDate
                                  };

                var query_company = from p in db.V_M_TM_SYS_BaseData_company
                                    select new
                                    {
                                        BaseDataID = p.BaseDataID,
                                        StoreCode = p.CompanyCode,
                                        StoreName = p.CompanyName,
                                        ProvinceCodeStore = p.CompanyProvinceName,
                                        CityCodeStore = "",
                                        StoreAddress = "",
                                        UpdateTime = p.ModifiedDate

                                    };
                var query_channel = from c in db.V_M_TM_SYS_BaseData_channel
                                    select new
                                    {
                                        BaseDataID = c.BaseDataID,
                                        StoreCode = c.ChannelCodeBase,
                                        StoreName = c.ChannelNameBase,
                                        ProvinceCodeStore = "",
                                        CityCodeStore = "",
                                        StoreAddress = "",
                                        UpdateTime = c.ModifiedDate
                                    };


                QueryOrganizationOutPut output = new QueryOrganizationOutPut();
                List<Obj> objs = new List<Obj>();
                foreach (var q in query_store)
                {
                    Obj obj = new Obj();
                    obj.GID = q.BaseDataID.ToString();
                    obj.CODE = "1-" + q.StoreCode;
                    obj.STORENAME = q.StoreName;
                    obj.PROVINCE = q.ProvinceCodeStore;
                    obj.CITY = q.CityCodeStore;
                    obj.ADDRESS = q.StoreAddress;
                    obj.ParentCode = "2-" + q.ChannelCodeStore;
                    obj.IsStore = true;
                    obj.UpdateTime = Convert.ToDateTime(q.UpdateTime);
                    obj.Enable = true;
                    objs.Add(obj);
                }
                foreach (var q in query_channel)
                {
                    Obj obj = new Obj();
                    obj.GID = q.BaseDataID.ToString();
                    obj.CODE = "2-" + q.StoreCode;
                    obj.STORENAME = q.StoreName;
                    obj.PROVINCE = q.ProvinceCodeStore;
                    obj.CITY = q.CityCodeStore;
                    obj.ADDRESS = q.StoreAddress;
                    obj.IsStore = false;
                    obj.ParentCode = "3-01";
                    obj.UpdateTime = Convert.ToDateTime(q.UpdateTime);
                    obj.Enable = true;
                    objs.Add(obj);
                }
                objs.Add(new Obj { GID = "00", CODE = "00", STORENAME = "总部", IsStore = false, ParentCode = "0", Enable = true });
                objs.Add(new Obj { GID = "3-01", CODE = "3-01", STORENAME = "南区", IsStore = false, ParentCode = "00", Enable = true });
                objs.Add(new Obj { GID = "3-02", CODE = "3-02", STORENAME = "北区", IsStore = false, ParentCode = "00", Enable = true });
                objs.Add(new Obj { GID = "3-03", CODE = "3-03", STORENAME = "华东区", IsStore = false, ParentCode = "00", Enable = true });
                output.Objs = objs;
                output.Status = 1;
                output.ErrorMsg = "成功";

                return JsonHelper.Serialize(output);
            }
        }
        #endregion

        #region 会员查询接口
        /// <summary>
        /// 会员查询接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string QueryMember(string input)
        {
            var mr = JsonHelper.Deserialize<WXQueryMemberInput>(input);
            if (mr == null)
                return resultJson(-1, "入参错误");
            if (!secretKey(skey, mr.signdate, mr.secret))
            {
                return resultJson(-7031, "秘钥错误");
            }
            if (string.IsNullOrWhiteSpace(mr.FPhoneNum))
                return resultJson(-1, "手机号不能为空");
            using (CRMEntities db = new CRMEntities())
            {

                if (mr.FPhoneNum.Length > 11)
                {
                    var query = (from u in db.V_U_TM_Mem_Info
                                 join c in db.TM_Mem_Card on u.MemberID equals c.MemberID
                                 where c.CardType == "2" && u.MemberOpenID == mr.FPhoneNum
                                 select new
                                 {
                                     u.MemberID,
                                     u.MemberOpenID,
                                     u.CustomerMobile,
                                     u.CustomerName,
                                     u.Gender,
                                     u.Birthday,
                                     c.CardNo
                                 }).FirstOrDefault();



                    WXQueryMemberOutput output = new WXQueryMemberOutput();
                    if (query == null)
                    {
                        output.Status = 2;
                        output.ErrorMsg = "会员不存在";

                    }
                    if (query != null && !string.IsNullOrEmpty(query.MemberOpenID))
                    {
                        output.FName = query.CustomerName;
                        output.FSex = query.Gender;
                        output.FBirthDay = query.Birthday == null ? "" : query.Birthday.Value.ToString("yyyy-MM-dd");
                        output.FPhoneNum = query.CustomerMobile;
                        output.FCardNum = query.CardNo;
                        output.Status = -1;
                        output.ErrorMsg = "会员已绑定微信";

                    }
                    return JsonHelper.Serialize(output);
                }
                else
                {


                    var query = (from u in db.V_U_TM_Mem_Info
                                 join c in db.TM_Mem_Card on u.MemberID equals c.MemberID
                                 where c.CardType == "2" && u.Mobile == mr.FPhoneNum
                                 select new
                                 {
                                     u.MemberID,
                                     u.MemberOpenID,
                                     u.CustomerMobile,
                                     u.CustomerName,
                                     u.Gender,
                                     u.Birthday,
                                     c.CardNo
                                 }).FirstOrDefault();



                    WXQueryMemberOutput output = new WXQueryMemberOutput();
                    if (query == null)
                    {
                        output.Status = 2;
                        output.ErrorMsg = "会员不存在";

                    }
                    //会员已经存在并没有openId数据
                    if (query != null && string.IsNullOrEmpty(query.MemberOpenID))
                    {
                        output.FName = query.CustomerName;
                        output.FSex = query.Gender;
                        output.FBirthDay = query.Birthday == null ? "" : query.Birthday.Value.ToString("yyyy-MM-dd");
                        output.FPhoneNum = query.CustomerMobile;
                        output.FCardNum = query.CardNo;
                        output.Status = 1;
                        output.ErrorMsg = "老会员,未绑定微信";
                    }
                    if (query != null && !string.IsNullOrEmpty(query.MemberOpenID))
                    {
                        output.FName = query.CustomerName;
                        output.FSex = query.Gender;
                        output.FBirthDay = query.Birthday == null ? "" : query.Birthday.Value.ToString("yyyy-MM-dd");
                        output.FPhoneNum = query.CustomerMobile;
                        output.FCardNum = query.CardNo;
                        output.Status = -1;
                        output.ErrorMsg = "会员已绑定微信";

                    }
                    return JsonHelper.Serialize(output);
                }
            
            }
        }
        #endregion

        #region 更新会员
        /// <summary>
        /// 更新会员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string UpdateMember(string input)
        {
            var mr = JsonHelper.Deserialize<UpdateMemberInput>(input);
            if (mr == null)
                return resultJson(-1, "入参错误");
            if (!secretKey(skey, mr.signdate, mr.secret))
            {
                return resultJson(-7031, "秘钥错误");
            }
            if (string.IsNullOrWhiteSpace(mr.FCardNum))
                return resultJson(-1, "卡号不能为空");
            if (string.IsNullOrWhiteSpace(mr.openId))
                return resultJson(-1, "openId不能为空");
            using (CRMEntities db = new CRMEntities())
            {
                UpdateMemberOutput output = new UpdateMemberOutput();
                var query = from u in db.V_S_TM_Mem_Ext
                            join c in db.TM_Mem_Card
                            on u.MemberID equals c.MemberID
                            where c.CardNo == mr.FCardNum && c.Active == true
                            select u;

                var cardmem = db.TM_Mem_Card.Where(p => p.CardNo == mr.FCardNum && p.Active == true && p.Lock == false).FirstOrDefault();
                if (cardmem == null)
                {
                    output.Status = -1;
                    output.ErrorMsg = "卡号不存在";
                    return JsonHelper.Serialize(output);
                }
                var memmaster = db.TM_Mem_Master.Where(p => p.MemberID == cardmem.MemberID);
                var memext = db.TM_Mem_Ext.Where(p => p.MemberID == cardmem.MemberID).FirstOrDefault();
                if (memext == null || memmaster.FirstOrDefault() == null)
                {
                    output.Status = -1;
                    output.ErrorMsg = "会员信息异常：" + cardmem.MemberID;
                    return JsonHelper.Serialize(output);
                }
                if (memmaster.Where(p => p.Str_Key_4 == mr.openId).FirstOrDefault() == null)
                {
                    memmaster.FirstOrDefault().Str_Key_4 = mr.openId;
                }
                var oldmem = db.V_U_TM_Mem_Info.Where(p => p.Mobile == mr.FPhoneNum && p.MemberID != cardmem.MemberID).FirstOrDefault();
                if (oldmem != null)
                {
                    output.Status = -1;
                    output.ErrorMsg = "手机号已有重复";
                    return JsonHelper.Serialize(output);
                }
                memmaster.FirstOrDefault().Str_Key_1 = mr.FPhoneNum;//手机号
                memext.Str_Attr_4 = mr.FPhoneNum; //手机号
                if (!string.IsNullOrEmpty(mr.FBirthDay))
                    memext.Date_Attr_2 = Convert.ToDateTime(mr.FBirthDay);
                if (!string.IsNullOrEmpty(mr.FName))
                    memext.Str_Attr_3 = mr.FName;
                if (!string.IsNullOrEmpty(mr.FSex))
                    memext.Str_Attr_7 = mr.FSex;
                db.SaveChanges();
                output.Status = 1;
                return JsonHelper.Serialize(output);

            }

        }
        #endregion

        #region 短信接口
        /// <summary>
        /// 增加短信记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string AddSMS(string input)
        {
            var mr = JsonHelper.Deserialize<AddSMSInput>(input);
            if (mr == null)
                return resultJson(-1, "入参错误");
            if (!secretKey(skey, mr.signdate, mr.secret))
            {
                return resultJson(-7031, "秘钥错误");
            }
            StringBuilder errMessage = new StringBuilder();
            bool requiredstate = false;
            if (string.IsNullOrWhiteSpace(mr.Mobile))
            {
                requiredstate = true;
                errMessage.Append("手机号不能为空  ");
            }
            if (string.IsNullOrWhiteSpace(mr.Message))
            {
                requiredstate = true;
                errMessage.Append("短信内容不能为空");
            }
            if (requiredstate)
            {
                return resultJson(-1, errMessage.ToString());
            }
            using (CRMEntities db = new CRMEntities())
            {

                TL_Sys_SMSReceivedList SMS = new TL_Sys_SMSReceivedList();
                SMS.Mobile = mr.Mobile;
                SMS.Message = mr.Message;
                SMS.ReceivedTime = DateTime.Now;
                try
                {
                    db.TL_Sys_SMSReceivedList.Add(SMS);
                    db.SaveChanges();
                    AddSMSOutput output = new AddSMSOutput();
                    output.Status = 1;
                    output.ErrorMsg = "成功";
                    return JsonHelper.Serialize(output);
                }
                catch (Exception ex)
                {
                    AddSMSOutput output = new AddSMSOutput();
                    output.Status = -1;
                    output.ErrorMsg = "失败";
                    if (ex.InnerException != null)
                    {
                        output.ErrorMsg = "失败:" + ex.InnerException.Message;
                    }
                    return JsonHelper.Serialize(output);
                }
            }
        }
        #endregion

        #region 会员订单查询接口
        public string QueryMemberTrades(string input)
        {
            var mr = JsonHelper.Deserialize<QueryMemberTradesInput>(input);
            if (mr == null)
                return resultJson(-1, "入参错误");
            //if (!secretKey(skey, mr.signdate, mr.secret))
            //{
            //    return resultJson(-7031, "秘钥错误");
            //}
            using (CRMEntities db = new CRMEntities())
            {
                var orders = (from a in db.V_M_TM_Mem_Trade_sales
                              join b in db.TM_Mem_Card on a.MemberID equals b.MemberID
                              where b.CardNo == mr.FCardNum && b.Active == true
                              orderby a.ListDateSales descending
                              select new Order
                              {
                                  OrderCode = a.TradeCode,
                                  OrderDate = a.ListDateSales,
                                  StoreCode = a.ConsumeStoreCode,
                                  StoreName = db.V_M_TM_SYS_BaseData_store.Where(i => i.StoreCode == a.ConsumeStoreCode).FirstOrDefault().StoreName,
                                  Integral = a.ScoreSales,
                                  Amounts = a.RealAmtSales,


                              }).Take(mr.Counts).ToList();

                var details = from a in db.V_M_TM_Mem_TradeDetail_sales_product
                              join b in db.V_M_TM_Mem_Trade_sales on a.TradeID equals b.TradeID
                              join c in db.TM_Mem_Card on b.MemberID equals c.MemberID
                              join d in db.TD_Sys_Product on a.GdCode equals d.GoodsCode into dtmp
                              from dt in dtmp.DefaultIfEmpty()
                              select new
                              {
                                  dt.GoodsName,
                                  a.StdTotalSalesProduct,
                                  b.TradeCode,
                              };


                foreach (var order in orders)
                {

                    var detail2 = details.Where(i => i.TradeCode == order.OrderCode).ToList();
                    order.OrderDetail = new List<OrderDetail>();
                    foreach (var detail in detail2)
                    {

                        var d = new OrderDetail();
                        d.GoodsName = detail.GoodsName;
                        d.GoodsAmount = detail.StdTotalSalesProduct;

                        order.OrderDetail.Add(d);
                    }
                }





                QueryMemberTradesOutput output = new QueryMemberTradesOutput();

                output.Status = 1;
                output.ErrorMsg = "成功";
                output.Order = orders;

                return JsonHelper.Serialize(output);


            }


        }
        #endregion


        private static string resultJson(int status, string msg)
        {
            CommonJsonOutput jo = new CommonJsonOutput();
            jo.Status = status;
            jo.ErrorMsg = msg;
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
