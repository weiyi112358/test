using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model.Interface
{
    public class ResultJsonOutput
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }

    #region 卡信息查询
    public class QueryCardInput
    {
        #region 字段说明
        //time	Date	请求响应时间，格式：yyyy-MM-dd HH:mm:ss
        //operator	String	操作员
        //storeCode	String	门店代码
        //tranId	String	交易流水号
        //tranTime	Date	交易时间，格式：yyyy-MM-dd HH:mm:ss
        //posNo	String	pos机号
        //cardInfo	TCardInfoRec
        //卡信息
        //queryType	String	查询业务类型，默认为普通查询，可以选：queryCard，saleCard，oldCardReissue，newCardReissue，该字段控制校验卡的状态信息。选queryCard时，校验卡状态是否是可用状态；选saleCard时，校验卡是否为带发卡；选oldCardReissue时，验证旧卡信息；选newCardReissue时，验证新卡信息。
        //needMemberInfo	boolean	是否返回会员信息,默认否
        //accountType	int	账户识别码类型可选0~6，其中：0：卡号，1：手机号，2：会员号，3：身份证号，4：会员外部号，5：良品随视一维码，6：鼎立云会员码
        //accountAccessCode	String	帐户标识代码
        //sendPassword	String	是否发送消费密码 0---不发送 1---发送
        //tranType	String	交易类型
        //payPassword	String	支付密码
        //checkPayPassword	boolean	是否验证密码，默认否
        //verifyCVN	boolean	默认否
        #endregion

        public DateTime? time { get; set; }
        [JsonProperty("operator")]
        public string operatorStr { get; set; }
        public string storeCode { get; set; }
        public string tranId { get; set; }
        public DateTime? tranTime { get; set; }
        public string posNo { get; set; }
        public TCardInfoRec cardinfo { get; set; }
        public string queryType { get; set; }
        public Boolean needMemberInfo { get; set; }
        public int accountType { get; set; }
        public string accountAccessCode { get; set; }
        public string sendPassword { get; set; }
        public string tranType { get; set; }
        public string payPassword { get; set; }
        public Boolean checkPayPassword { get; set; }
        public Boolean verifyCVN { get; set; }
        public string signDate { get; set; }
        public string secret { get; set; }
    }

    public class QueryCardOutput
    {
        #region 字段说明
        //time	Date	请求响应时间，格式：yyyy-MM-dd HH:mm:ss
        //retCode	TRetCode
        //响应码
        //tranId	String	交易流水号
        //tranTime	Date	交易时间
        //operator	String	操作员
        //cardInfo	TCardInfoRec
        //卡信息
        //isBirthdate	boolean	不是会员生日
        //isMemorialDay	boolean	不是会员纪念日
        //memberType	String	会员类型代码
        //memberTypeName	String	会员类型名称
        //accountType	int	账户识别码类型
        //mobilePhoneNum	String	手机号
        //memberId	String	会员ID
        //memberName	String	会员名称
        //desaccountUuid	String	帐户Uuid
        //accountAccessCode	String	帐户标识代码
        //discount	BigDecimal	售卡折扣，查询类型为CQueryType_forSale（saleCard）时必须返回，默认为1
        //memberInfo	TQueryMemberResponse
        //会员信息，needMemberInfo为TRUE时，必须返回
        //discountRate	BigDecimal	折扣率，返回后台在卡类型设置的折扣率，默认为0
        //isSendPassword	boolean	是否重新发送验证码，默认是
        //verifyCVN	boolean	默认否
        //birthday	Date	
        //favAmt	BigDecimal	本期优惠金额，默认为0
        //lastMbrUpGradeTime	Date	
        //mbrRegisterTime	Date	
        //isModifyAccountAccessInfo	boolean	账户识别信息是否修改，账户识别码或账户识别类型有修改，都返回true
        //extCardInfo	List<TExtCardRec>
        //扩展卡信息
        #endregion

        public DateTime? time { get; set; }
        public TRetCode retCode { get; set; }
        public string tranId { get; set; }
        public DateTime? tranTime { get; set; }
        [JsonProperty("operator")]
        public string operatorStr { get; set; }
        public TCardInfoRec cardInfo { get; set; }
        public Boolean isBirthdate { get; set; }
        public Boolean isMemorialDay { get; set; }
        public string memberType { get; set; }
        public string memberTypeName { get; set; }
        public int accountType { get; set; }
        public string mobilePhoneNum { get; set; }
        public string memberId { get; set; }
        public string memberName { get; set; }
        public string desaccountUuid { get; set; }
        public string accountAccessCode { get; set; }
        public decimal discount { get; set; }
        public TQueryMemberResponse memberInfo { get; set; }
        public decimal discountRate { get; set; }
        public Boolean isSendPassword { get; set; }
        public Boolean verifyCVN { get; set; }
        public DateTime? birthday { get; set; }
        public decimal favAmt { get; set; }
        public DateTime? lastMbrUpGradeTime { get; set; }
        public DateTime? mbrRegisterTime { get; set; }
        public Boolean isModifyAccountAccessInfo { get; set; }
        public List<TExtCardRec> extCardInfo { get; set; }
    }

    #endregion

    #region 卡挂失查询
    public class QueryLossCardInput
    {
        #region 字段说明
        //time	Date	请求响应时间，格式：yyyy-MM-dd HH:mm:ss
        //operator	String	操作员
        //storeCode	String	门店代码
        //tranId	String	交易流水号
        //tranTime	Date	交易时间，格式：yyyy-MM-dd HH:mm:ss
        //posNo	String	pos机号
        //accountType	int	账户识别码类型可选0~6，其中：0：卡号，1：手机号，2：会员号，3：身份证号，4：会员外部号，5：良品随视一维码，6：鼎立云会员码
        //accountAccessCode	String	帐户标识代码
        #endregion

        public DateTime? time { get; set; }
        [JsonProperty("operator")]
        public string operatorStr { get; set; }
        public string storeCode { get; set; }
        public string tranId { get; set; }
        public DateTime? tranTime { get; set; }
        public string posNo { get; set; }
        public int accountType { get; set; }
        public string accountAccessCode { get; set; }
        public string signDate { get; set; }
        public string secret { get; set; }
    }

    public class QueryLossCardOutput
    {
        #region 字段说明
        //time	Date	请求响应时间，格式：yyyy-MM-dd HH:mm:ss
        //retCode	TRetCode 响应码
        //tranId	String	交易流水号
        //tranTime	Date	交易时间
        //operator	String	操作员
        //accountType	int	账户识别码类型可选0~6，其中：0：卡号，1：手机号，2：会员号，3：身份证号，4：会员外部号，5：良品随视一维码，6：鼎立云会员码
        //accountAccessCode	String	帐户标识代码
        //lossCardInfos	List<TLossCardInfoRec> 卡信息
        #endregion

        public DateTime? time { get; set; }
        public TRetCode retCode { get; set; }
        public string tranId { get; set; }
        public DateTime? tranTime { get; set; }
        [JsonProperty("operator")]
        public string operatorStr { get; set; }
        public int accountType { get; set; }
        public string accountAccessCode { get; set; }
        public List<TLossCardInfoRec> lossCardInfos { get; set; }
    }

    #endregion

    #region 卡挂失prepare
    public class LossCardPrepareInput
    {
        #region 字段说明
        //time	Date	请求响应时间，格式：yyyy-MM-dd HH:mm:ss
        //operator	String	操作员
        //storeCode	String	门店代码
        //tranId	String	交易流水号
        //tranTime	Date	交易时间，格式：yyyy-MM-dd HH:mm:ss
        //posNo	String	pos机号
        //accountType	int	账户识别码类型可选0~6，其中：0：卡号，1：手机号，2：会员号，3：身份证号，4：会员外部号，5：良品随视一维码，6：鼎立云会员码
        //accountAccessCode	String	帐户标识代码
        //mobileCheckPsw	String	手机校验码
        //xid	String	外部交易号
        //lossCardInfos	List<TLossCardInfoRec> 卡信息
        #endregion

        public DateTime? time { get; set; }
        [JsonProperty("operator")]
        public string operatorStr { get; set; }
        public string storeCode { get; set; }
        public string tranId { get; set; }
        public DateTime? tranTime { get; set; }
        public string posNo { get; set; }
        public int accountType { get; set; }
        public string accountAccessCode { get; set; }
        public string mobileCheckPsw { get; set; }
        public string xid { get; set; }
        public List<TLossCardInfoRec> lossCardInfos { get; set; }
        public string signDate { get; set; }
        public string secret { get; set; }


    }

    public class LossCardPrepareOutput
    {
        #region 字段说明
        //time	Date	请求响应时间，格式：yyyy-MM-dd HH:mm:ss
        //retCode	TRetCode 响应码
        //tranId	String	交易流水号
        //tranTime	Date	交易时间
        //operator	String	操作员
        //accountType	int	账户识别码类型可选0~6，其中：0：卡号，1：手机号，2：会员号，3：身份证号，4：会员外部号，5：良品随视一维码，6：鼎立云会员码
        //accountAccessCode	String	帐户标识代码
        //mobileCheckPsw	String	手机校验码
        //xid	String	外部交易号
        //lossCardInfos	List<TLossCardInfoRec> 卡信息
        #endregion

        public DateTime? time { get; set; }
        public TRetCode retCode { get; set; }
        public string tranId { get; set; }
        public DateTime? tranTime { get; set; }
        [JsonProperty("operator")]
        public string operatorStr { get; set; }
        public int accountType { get; set; }
        public string accountAccessCode { get; set; }
        public string mobileCheckPsw { get; set; }
        public string xid { get; set; }
        public List<TLossCardInfoRec> lossCardInfos { get; set; }
    }

    #endregion

    #region 卡挂失confirm
    public class LossCardConfirmInput
    {
        #region 字段说明
        //time	Date	请求响应时间，格式：yyyy-MM-dd HH:mm:ss
        //operator	String	操作员
        //storeCode	String	门店代码
        //tranId	String	交易流水号
        //tranTime	Date	交易时间，格式：yyyy-MM-dd HH:mm:ss
        //posNo	String	pos机号
        //accountType	int	账户识别码类型可选0~6，其中：0：卡号，1：手机号，2：会员号，3：身份证号，4：会员外部号，5：良品随视一维码，6：鼎立云会员码
        //accountAccessCode	String	帐户标识代码
        //xid	String	外部交易号
        //lossCardInfos	List<TLossCardInfoRec>卡信息
        //remark	String	说明
        #endregion

        public DateTime? time { get; set; }
        [JsonProperty("operator")]
        public string operatorStr { get; set; }
        public string storeCode { get; set; }
        public string tranId { get; set; }
        public DateTime? tranTime { get; set; }
        public string posNo { get; set; }
        public int accountType { get; set; }
        public string accountAccessCode { get; set; }
        public string xid { get; set; }
        public List<TLossCardInfoRec> lossCardInfos { get; set; }
        public string remark { get; set; }
        public string signDate { get; set; }
        public string secret { get; set; }

    }

    public class LossCardConfirmOutput
    {
        #region 字段说明
        //time	Date	请求响应时间，格式：yyyy-MM-dd HH:mm:ss
        //retCode	TRetCode 响应码
        //tranId	String	交易流水号
        //tranTime	Date	交易时间
        //operator	String	操作员
        //xid	String	外部交易号
        //lossCardInfos	List<TLossCardInfoRec> 卡信息
        #endregion

        public DateTime? time { get; set; }
        public TRetCode retCode { get; set; }
        public string tranId { get; set; }
        public DateTime? tranTime { get; set; }
        [JsonProperty("operator")]
        public string operatorStr { get; set; }
        public string xid { get; set; }
        public List<TLossCardInfoRec> lossCardInfos { get; set; }
    }

    #endregion

    #region 查询会员
    public class QueryMemberInput
    {
        #region 字段说明
        //time	Date	请求响应时间，格式：yyyy-MM-dd HH:mm:ss
        //operatorStr	String	操作员
        //storeCode	String	门店代码
        //tranId	String	交易流水号
        //tranTime	Date	交易时间，格式：yyyy-MM-dd HH:mm:ss
        //posNo	String	pos机号
        //validateCode	String	验证码
        //memberAccessType	int	账户识别码类型可选0~6，其中：0：卡号，1：手机号，2：会员号，3：身份证号，4：会员外部号，5：良品随视一维码，6：鼎立云会员码
        //memberAccessCode	String	会员识别代码
        //appName	String	应用名
        #endregion

        public DateTime? time { get; set; }
        [JsonProperty("operator")]
        public string operatorStr { get; set; }
        public string storeCode { get; set; }
        public string tranId { get; set; }
        public DateTime? tranTime { get; set; }
        public string posNo { get; set; }
        public string validateCode { get; set; }
        public int memberAccessType { get; set; }
        public string memberAccessCode { get; set; }
        public string appName { get; set; }
        public string signDate { get; set; }
        public string secret { get; set; }
    }

    public class QueryMemberOutput
    {
        #region 字段说明
        //time	Date	请求响应时间，格式：yyyy-MM-dd HH:mm:ss
        //retCode	TRetCode
        //响应码
        //tranId	String	交易流水号
        //tranTime	Date	交易时间
        //operator	String	操作员
        //code	String	会员ID
        //name	String	会员姓名
        //memberId	String	会员外部号
        //nameSpell	String	会员
        //birthday	Date	会员生日
        //memorialDay	Date	会员纪念日
        //occupation	String	职业
        //specialty	String	特长
        //nationality	String	民族
        //presenterCode	String	推荐人ID
        //presenterName	String	推荐人姓名
        //memberGradeName	String	会员等级名称
        //cellPhone	String	手机号
        //fixedPhone	String	固定电话
        //email	String	email
        //postCode	String	邮编
        //qq	String	qq号
        //address	String	详细地址
        //status	String	会员状态, NORMAL("正常"), FROZEN("已冻结"), DELETED("已删除")
        //paperType	String	证件类型, IDCARD("身份证"), PASS("护照"),MILITARYCARD("军人证"), OTHERPAPER("其它证件")
        //paperCode	String	证件号
        //remark	String	备注
        //sex	String	性别, male("男"), female("女")
        //wedding	String	婚否, Y("是"), N("否"), K("保密")
        //orgCode	String	创建组织代码
        //orgName	String	创建组织名称
        //nationCode	String	国家代码
        //nationName	String	国家名称
        //provinceCode	String	省/市代码
        //provinceName	String	省/市名称
        //cityCode	String	市/区代码
        //cityName	String	市/区名称
        //districtCode	String	区县代码
        //districtName	String	区县名称
        //region	String	区域代码
        //source	String	来源
        //isMember	boolean	是否是会员
        //memberGrade	TMemberGrade
        //会员等级
        //login	String	登录名
        //lastLoginTime	Date	最后登录时间
        //referrer	String	会员上线标识
        //attributes	List<TMemberAttribute>
        //会员扩展属性，与后台会员扩展资料对应，key = 会员扩展资料代码 + "_" + 会员扩展资料属性代码
        //activityHeat	int	活动热度
        //memberSMSReceive	boolean	接受活动信息，默认不接受
        //memberSourceCode	String	会员来源代码
        //memberSourceName	String	会员来源名称
        //registerDate	Date	注册时间
        //childBirthday	Date	孩子生日
        //memorialDayRemark	String	纪念日说明
        //memGradePay	BigDecimal	会员等级消费金额
        //memGradeValidate	Date	会员等级有效期
        //memGradeMessage	String	等级提示消息
        //gradeAndPay	List<TMemGradeAndRangePay>
        //会员即将达到的等级以及在达到此等级需要的金额
        //cardNo	String	会员卡号
        #endregion

        public DateTime? time { get; set; }
        public TRetCode retCode { get; set; }
        public string tranId { get; set; }
        public DateTime? tranTime { get; set; }
        [JsonProperty("operator")]
        public string operatorStr { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string memberId { get; set; }
        public string nameSpell { get; set; }
        public DateTime? birthday { get; set; }
        public DateTime? memorialDay { get; set; }
        public string occupation { get; set; }
        public string specialty { get; set; }
        public string nationality { get; set; }
        public string presenterCode { get; set; }
        public string presenterName { get; set; }
        public string memberGradeName { get; set; }
        public string cellPhone { get; set; }
        public string fixedPhone { get; set; }
        public string email { get; set; }
        public string postCode { get; set; }
        public string qq { get; set; }
        public string address { get; set; }
        public string status { get; set; }
        public string paperType { get; set; }
        public string paperCode { get; set; }
        public string remark { get; set; }
        public string sex { get; set; }
        public string wedding { get; set; }
        public string orgCode { get; set; }
        public string orgName { get; set; }
        public string nationCode { get; set; }
        public string nationName { get; set; }
        public string provinceCode { get; set; }
        public string provinceName { get; set; }
        public string cityCode { get; set; }
        public string cityName { get; set; }
        public string districtCode { get; set; }
        public string districtName { get; set; }
        public string region { get; set; }
        public string source { get; set; }
        public Boolean isMember { get; set; }
        public TMemberGrade memberGrade { get; set; }
        public string login { get; set; }
        public DateTime lastLoginTime { get; set; }
        public string referrer { get; set; }
        public List<TMemberAttribute> attributes { get; set; }
        public int activityHeat { get; set; }
        public Boolean memberSMSReceive { get; set; }
        public string memberSourceCode { get; set; }
        public string memberSourceName { get; set; }
        public DateTime? registerDate { get; set; }
        public DateTime? childBirthday { get; set; }
        public string memorialDayRemark { get; set; }
        public decimal memGradePay { get; set; }
        public DateTime? memGradeValidate { get; set; }
        public string memGradeMessage { get; set; }
        public List<TMemGradeAndRangePay> gradeAndPay { get; set; }
        public string cardNo { get; set; }
        public string memberGradeCode { get; set; }
        public string signDate { get; set; }
        public string secret { get; set; }
    }

    public class TMemberGrade
    {
        #region 字段说明
        //ucn	TUCN
        //function	String	功能类型 第一位 会员价 第二位 折扣价 第三位 储值 第四位 积分 第五位 充值卡 第六位会员电子消费卡 第七位 提货
        //discounts	List<TDiscount>
        #endregion

        public TUCN ucn { get; set; }
        public string function { get; set; }
        public List<TDiscount> discounts { get; set; }
    }

    public class TMemberAttribute
    {
        #region 字段说明
        //key	String	
        //value	String	当为多选值时，值之间以;隔开如：看电影;旅游度假;自驾;看书;上网;运动;
        #endregion

        public string key { get; set; }
        public string value { get; set; }
    }

    public class TMemGradeAndRangePay
    {
        public string code { get; set; }
        public string name { get; set; }
        public decimal occur { get; set; }
        public string message { get; set; }
    }

    public class TUCN
    {
        public string code { get; set; }
        public string name { get; set; }
        public string uuid { get; set; }
    }

    public class TDiscount
    {
        #region 字段说明
        //discountType	int	0-封顶折扣, 1-折上折
        //discountRate	BigDecimal	默认1
        #endregion

        public int discountType { get; set; }
        public decimal discountRate { get; set; }
    }

    #endregion

    #region 查询费用
    public class QueryFeeInput
    {
        public string signDate { get; set; }
        public string secret { get; set; }

        #region 字段说明
        //time	Date	请求响应时间，格式：yyyy-MM-dd HH:mm:ss
        //operator	String	操作员
        //storeCode	String	门店代码
        //tranId	String	交易流水号
        //tranTime	Date	交易时间，格式：yyyy-MM-dd HH:mm:ss
        //posNo	String	pos机号
        //feeType	String	费用类型，可选值:SaleCard("办卡"), ReportLoss("挂失"), RelatCard("卡关联"), ReissueCard("补                              发")。
        //accountId	String	账户识别码值
        //accountType	int	账户识别码类型
        #endregion

        public DateTime? time { get; set; }
        [JsonProperty("operator")]
        public string operatorStr { get; set; }
        public string storeCode { get; set; }
        public string tranId { get; set; }
        public DateTime? tranTime { get; set; }
        public string posNo { get; set; }
        public string feeType { get; set; }
        public string accountId { get; set; }
        public int accountType { get; set; }

    }


    public class QueryFeeOutput
    {
        #region 字段说明
        //time	Date	请求响应时间，格式：yyyy-MM-dd HH:mm:ss
        //retCode	TRetCode 响应码
        //tranId	String	交易流水号
        //tranTime	Date	交易时间
        //operator	String	操作员
        //fee	BigDecimal	费用
        //feeType	String	费用类型（入参直接返回）
        //accountId	String	账户识别码值
        //accountType	int	账户识别码类型
        #endregion

        public DateTime? time { get; set; }
        public TRetCode retCode { get; set; }
        public string tranId { get; set; }
        public DateTime? tranTime { get; set; }
        [JsonProperty("operator")]
        public string operatorStr { get; set; }
        public decimal fee { get; set; }//BigDecimal
        public string feeType { get; set; }
        public string accountId { get; set; }
        public int accountType { get; set; }

    }
    #endregion

    #region  卡补发

    public class ReissureCardInput : CommonInput
    {
        #region 字段说明
        //accountType	int	账户识别码类型 （精确查询）
        //accountAccessCode	String	帐户识别码
        //cardInfo	TCardInfoRec 卡信息
        //xid	String	外部交易号（这个操作的记录ID）
        //reissueType	int	补发类型，REISSUE_BY_FEE=0，REISSUE_BY_TRAN=1，默认0
        //fee	BigDecimal	费用
        //reissueTranId	String	补发用交易号（订单号，一个订单补发一次）
        //authCode	String	验证码
        #endregion

        public int accountType { get; set; }
        public string accountAccessCode { get; set; }
        public TCardInfoRec cardInfo { get; set; }
        public string xid { get; set; }
        public int reissueType { get; set; }
        public decimal fee { get; set; }
        public string reissueTranId { get; set; }
        public string authCode { get; set; }

    }


    public class ReissureCardOutput : CommonOutput
    {
        #region 字段说明
        //accountType	int	账户识别码类型可选0~6，其中：0：卡号，1：手机号，2：会员号，3：身份证号，4：会员外部号，5：良                            品随视一维码，6：鼎立云会员码
        //accountAccessCode	String	帐户识别码
        //cardInfo	TCardInfoRec 卡信息
        //xid	String	外部交易号
        //reissueType	int	补发类型，REISSUE_BY_FEE=0（费用），REISSUE_BY_TRAN=1（订单），默认0
        //fee	BigDecimal	费用
        //reissueTranId	String	补发用交易号
        #endregion

        public int accountType { get; set; }
        public string accountAccessCode { get; set; }
        public TCardInfoRec cardInfo { get; set; }
        public string xid { get; set; }
        public int reissueType { get; set; }
        public decimal fee { get; set; }
        public string reissueTranId { get; set; }
    }


    #endregion


    #region 8卡补发撤销
    public class ReissureCardCancelInput : CommonInput
    {
        public string oldTranId { get; set; }
        public string xid { get; set; }
    }

    public class ReissureCardCancelOutput : CommonOutput
    {
        public string oldTranId { get; set; }
        public string xid { get; set; }

    }
    #endregion

    #region 9发送短信
    public class SendSMSInput : CommonInput
    {
        public string mobileNums { get; set; }
        public Boolean sendMobileSecurityCode { get; set; }
        public string content { get; set; }
        public string tranType { get; set; }
    }


    public class SendSMSOutput : CommonOutput
    {
        public int sendAgainWaitingTime { get; set; }
        public string serialNo { get; set; }

    }
    #endregion

    #region 10获取所有会员等级
    public class GetAllMemberGradesInput : CommonInput
    {

    }

    public class GetAllMemberGradesOutput : CommonOutput
    {
        public string accountAccessCode { get; set; }
        public int accountAccessCodeType { get; set; }
        public List<TMemberGrade> memberGrades { get; set; }
    }
    #endregion


    #region 11查询会员来源
    public class QueryMemberSourceInput : CommonInput
    {

    }

    public class QueryMemberSourceOutput : CommonOutput
    {
        public List<TMemberSourceRec> memberSourceRecs { get; set; }
    }


    #endregion


    #region 12获取会员属性

    public class GetMemberAttributeOptionsInput : CommonInput
    {

    }

    public class GetMemberAttributeOptionsOutput : CommonOutput
    {
        public List<TMemberAttributeOptions> memberAttributeOptions { get; set; }
        public List<TMemberAttributeOptions> memberExtAttribute { get; set; }
    }


    #endregion

    #region 13保存会员
    public class SaveMemberInput : CommonInput
    {
        public string code { get; set; }
        public string name { get; set; }
        public string memberId { get; set; }
        public string nameSpell { get; set; }
        public DateTime? birthday { get; set; }
        public DateTime? memorialDay { get; set; }
        public string occupation { get; set; }
        public string specialty { get; set; }
        public string nationality { get; set; }
        public string presenterCode { get; set; }
        public string presenterName { get; set; }
        public string memberGradeCode { get; set; }
        public string memberGradeName { get; set; }
        public string cellPhone { get; set; }
        public string fixedPhone { get; set; }
        public string email { get; set; }
        public string postCode { get; set; }
        public string qq { get; set; }
        public string address { get; set; }
        public string paperType { get; set; }
        public string paperCode { get; set; }
        public string remark { get; set; }
        public string sex { get; set; }
        public string wedding { get; set; }
        public string orgCode { get; set; }
        public string orgName { get; set; }
        public string nationCode { get; set; }
        public string nationName { get; set; }
        public string provinceCode { get; set; }
        public string provinceName { get; set; }
        public string cityCode { get; set; }
        public string cityName { get; set; }
        public string districtCode { get; set; }
        public string districtName { get; set; }
        public string region { get; set; }
        public string cardNum { get; set; }
        public string validateCode { get; set; }
        public string source { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public List<TMemberAttribute> attributes { get; set; }
        public int activityHeat { get; set; }
        public Boolean memberSMSReveive { get; set; }
        public string memberSourceCode { get; set; }
        public string memberSourceName { get; set; }
        public DateTime? registerDate { get; set; }
        public DateTime? childBirthday { get; set; }
        public string memorialDayRemark { get; set; }
        public string yearOfBirth { get; set; }
        public string monthOfBirth { get; set; }
        public string dayOfBirth { get; set; }
    }


    public class SaveMemberOutput : CommonOutput
    {
        public string code { get; set; }

    }

    #endregion

    #region 14批量发售prepare
    public class BatchSaleCardPrepareInput : CommonInput
    {
        public List<TCardInfoRec> cardInfo { get; set; }
        public string Xid { get; set; }
    }

    public class BatchSaleCardPrepareOutput : CommonOutput
    {
        public List<TCardInfoRec> cardInfo { get; set; }
        public string Xid { get; set; }
        public decimal payTotal { get; set; }
        public decimal feeToal { get; set; }
    }
    #endregion


    #region  15批量发售 confirm
    public class BatchSaleCardConfirmInput : CommonInput
    {
        public List<TSalePayRec> payRec { get; set; }
        public string prepareOwnerTranId { get; set; }
        public decimal feeTotal { get; set; }
        public decimal consumeCost { get; set; }
    }

    public class BatchSaleCardConfirmOutput : CommonOutput
    {



    }




    #endregion

    #region 16批量发售prepare 取消

    public class BatchSaleCardPrepareCancelInput : CommonInput
    {
        public string oldTranId { get; set; }
    }

    public class BatchSaleCardPrepareCancelOutput : CommonOutput
    {

    }

    #endregion


    #region 17保存积分
    public class ScoreSaveInput : CommonInput
    {
        public int accountType { get; set; }
        public string cardType { get; set; }
        public string media { get; set; }
        public string xid { get; set; }
        public List<TScoreRec> scores { get; set; }
        public string accountAccessCode { get; set; }
        public string oldXid { get; set; }
    }

    public class ScoreSaveOutput : CommonOutput
    {
        public decimal currScore { get; set; }
        public decimal oldScore { get; set; }
        public string xid { get; set; }
    }

    #endregion


    #region 18前台办卡
    public class SaleCardInput : CommonInput
    {
        public string mobileSecurityCode { get; set; }
        public TCardInfoRec cardInfo { get; set; }
        public decimal fee { get; set; }
        public Boolean isUseSaleCard { get; set; }
        public Boolean needMobileVerify { get; set; }
        public List<TMemberAttribute> memberAttribute { get; set; }
    }

    public class SaleCardOutput : CommonOutput
    {
        public decimal fee { get; set; }
        public Boolean pwdonoffflag { get; set; }
    }
    #endregion



    #region 19修改卡密码
    public class ChangeCardPasswordInput : CommonInput
    {
        public TCardInfoRec cardInfo { get; set; }
        public string newPassword { get; set; }
        public string oldPassword { get; set; }

    }


    public class ChangeCardPasswordOutput : CommonOutput
    {

    }
    #endregion


    #region  20修改卡密码prepare
    public class ChangePasswordPrepareInput : CommonInput
    {
        public TCardInfoRec cardInfo { get; set; }
        public string newPassword { get; set; }
        public string oldPassword { get; set; }
    }

    public class ChangePasswordPrepareOutput : CommonOutput
    {

    }
    #endregion



    #region  21充值prepare
    public class DepositPrepareInput : CommonInput
    {
        public string xid { get; set; }
        public TCardInfoRec cardInfo { get; set; }
        public string mobileNum { get; set; }
        public int accountAccessType { get; set; }
        public decimal total { get; set; }
        public decimal oldBalance { get; set; }
        public string tranTypeExt { get; set; }

    }

    public class DepositPrepareOutput : CommonOutput
    {
        public string xid { get; set; }
        public TCardInfoRec cardInfo { get; set; }
        public string mobileNum { get; set; }
        public int accountAccessType { get; set; }
        public decimal total { get; set; }
        public decimal oldBalance { get; set; }
        public decimal newBalance { get; set; }
        public string tranTypeExt { get; set; }


    }



    #endregion


    #region 22充值
    public class DepositInput : CommonInput
    {
        public string tranType { get; set; }
        public string depositType { get; set; }
        public TCardInfoRec cardInfo { get; set; }
        public string xid { get; set; }
        public Boolean transacted { get; set; }
        public List<TDepositRec> deposits { get; set; }
        public decimal total { get; set; }
        public string memberid { get; set; }
        public string telphone { get; set; }
        public decimal maxTotal { get; set; }
        public int accountType { get; set; }
    }


    public class DepositOutput : CommonOutput
    {
        public string tranType { get; set; }
        public string depositType { get; set; }
        public TCardInfoRec cardInfo { get; set; }
        public string xid { get; set; }
        //public Boolean transacted { get; set; }
        public List<TDepositRec> deposits { get; set; }
        public decimal total { get; set; }
        //public string memberid { get; set; }
        public string mobileNum { get; set; }
        //public decimal maxTotal { get; set; }
        public int accountType { get; set; }
        public decimal newBalance { get; set; }


    }

    #endregion


    #region 23卡关联（不要）

    #endregion


    #region 24查询卡积分
    public class QueryCardScoreInput : CommonInput
    {
        public string accountAccessCode { get; set; }
        public int accountType { get; set; }
    }


    public class QueryCardScoreOutput : CommonOutput
    {
        public string cardNumber { get; set; }
        public string memberId { get; set; }
        public string memberName { get; set; }
        public string telphone { get; set; }
        public int accountType { get; set; }
        public decimal currScore { get; set; }
        public List<TScoreRec> scores { get; set; }
    }


    #endregion


    #region  25取得兑奖商品列表
    public class QueryPrizeGoodsListInput : CommonInput
    {
        public string accountAccessCode { get; set; }
        public int accountAccessCodeType { get; set; }
        public int prizeType { get; set; }
    }


    public class QueryPrizeGoodsListOutput : CommonOutput
    {
        public string accountAccessCode { get; set; }
        public int accountAccessCodeType { get; set; }
        public int prizeType_key { get; set; }
        public List<TPrizeGoodsRec> prizeGoodsList { get; set; }
    }

    #endregion


    #region 26取得兑奖商品

    public class QueryPrizeGoodsInput : CommonInput
    {
        public string accountAccessCode { get; set; }
        public int accountAccessCodeType { get; set; }
        public int prizeType { get; set; }
        public string gdgid { get; set; }
        public decimal qpc { get; set; }
    }

    public class QueryPrizeGoodsOutput : CommonOutput
    {
        public string accountAccessCode { get; set; }
        public int accountAccessCodeType { get; set; }
        public int prizeType { get; set; }
        public string gdgid { get; set; }
        public decimal qpc { get; set; }
        public TPrizeGoodsRec prizeGoods { get; set; }
    }
    #endregion

    #region 补充接口1查询会员参与活动次数
    public class QueryMbrJoinRuleCountInput : CommonInput
    {
        public string accountAccessCode { get; set; }
        public int accountAccessCodeType { get; set; }
        public string ruleUuid { get; set; }
    }

    public class QueryMbrJoinRuleCountOutput : CommonOutput
    {
        public int count { get; set; }
        public TPrizeGoodsRec prizeGoods { get; set; }
    }
    #endregion

    #region 补充接口2查询会员领用礼品次数
    public class QueryMemberGetPresentCountsInput : CommonInput
    {
        public string accountAccessCode { get; set; }
        public int accountAccessCodeType { get; set; }
        public string ruleUuid { get; set; }
    }

    public class QueryMemberGetPresentCountsOutput : CommonOutput
    {
        public int count { get; set; }
        public TPrizeGoodsRec prizeGoods { get; set; }
    }
    #endregion

    #region  27兑奖
    public class PrizeInput : CommonInput
    {
        public string xid { get; set; }
        public string accountAccessCode { get; set; }
        public int accountAccessCodeType { get; set; }
        public string password { get; set; }
        public int verifyPassword { get; set; }
        public int prizeType { get; set; }
        public List<TPrizeGoodsRec> prizeGoodsList { get; set; }
        public List<TScoreRec> externalScores { get; set; }

    }

    public class PrizeOutput : CommonOutput
    {
        public string xid { get; set; }
        public string accountAccessCode { get; set; }
        public int accountAccessCodeType { get; set; }
        public int prizeType { get; set; }
        public string password { get; set; }
        public int verifyPassword { get; set; }
        public List<TPrizeGoodsRec> prizeGoodsList { get; set; }
        public List<TScoreRec> externalScores { get; set; }
    }

    #endregion


    #region 28兑奖撤换
    public class PrizeCancelInput : CommonInput
    {
        public string accountAccessCode { get; set; }
        public int accountAccessCodeType { get; set; }
        public string xid { get; set; }
        public string oldTranId { get; set; }
    }

    public class PrizeCancelOutput : CommonOutput
    {
        public string accountAccessCode { get; set; }
        public int accountAccessCodeType { get; set; }
        public string xid { get; set; }
        public string oldTranId { get; set; }
    }


    #endregion

    #region 29会员消费
    public class mbrPayInput : CommonInput
    {
        public List<TScoreRec> scores { get; set; }

    }

    public class mbrPayOutput : CommonOutput
    {

    }


    #endregion


    #region 30积分兑奖
    public class ScorePrizeInput : CommonInput
    {
        public TCardInfoRec cardInfo { get; set; }
        public string xid { get; set; }
        public string accountAccessCode { get; set; }
        public int accountType { get; set; }
        public List<TScoreRec> scores { get; set; }
        public string password { get; set; }
        public Boolean usingTotalPay { get; set; }
        public string mobileSecurityCode { get; set; }

    }

    public class ScorePrizeOutput : CommonOutput
    {
        public string flowNo { get; set; }
        public string result { get; set; }
        public string msg { get; set; }
        public decimal currScore { get; set; }
        public decimal oldScore { get; set; }
    }

    #endregion


    #region 31积分兑奖撤销
    public class ScorePrizeCancelInput : CommonInput
    {
        public string xid { get; set; }
        public string oldTranId { get; set; }
        public string accountAccessCode { get; set; }
        public int accountAccessCodeType { get; set; }
        public List<TScoreRec> scores { get; set; }

    }

    public class ScorePrizeCancelOutput : CommonOutput
    {
        public decimal currScore { get; set; }
        public List<TScoreRec> scores { get; set; }
    }

    #endregion


    #region 32购物券查询
    public class QueryVoucherInput : CommonInput
    {
        public List<TQueryVoucherRec> vouchers { get; set; }
    }
    public class QueryVoucherOutput : CommonOutput
    {
        public List<TQueryVoucherRec> vouchers { get; set; }
    }
    #endregion


    #region 33购物券作废
    public class VoucherAbolishInput : CommonInput
    {
        public string voucherNo { get; set; }
        public string xid { get; set; }
    }

    public class VoucherAbolishOutput : CommonOutput
    {
        public string voucherNo { get; set; }


    }

    #endregion


    #region 34购物券激活
    public class VoucherActivateInput : CommonInput
    {
        public string voucherNo { get; set; }
        public string xid { get; set; }
        public decimal occur { get; set; }

    }

    public class VoucherActivateOutput : CommonOutput
    {
        public string voucherNo { get; set; }

    }

    #endregion


    #region 35购物券打印
    public class VoucherPrintInput : CommonInput
    {
        public string voucherNo { get; set; }
        public string xid { get; set; }
    }

    public class VoucherPrintOutput : CommonOutput
    {
        public string voucherNo { get; set; }
    }


    #endregion


    #region 36购物券使用
    public class VoucherUseInput : CommonInput
    {
        public string voucherNo { get; set; }
        public string xid { get; set; }
        public decimal occur { get; set; }
    }

    public class VoucherUseOutput : CommonOutput
    {
        public string voucherNo { get; set; }

    }



    #endregion


    #region  37购物券下载
    public class LocalDataInput : CommonInput
    {
        public DateTime? lastUpdated { get; set; }
        public string dataType { get; set; }
        public DateTime? dataTime { get; set; }
        public string fileName { get; set; }
    }

    public class LocalDataOutput : CommonOutput
    {
        public string url { get; set; }
        public DateTime? dataTime { get; set; }
        public string dataType { get; set; }
    }


    #endregion



    #region  38购物券下载完成
    public class LocalDataDoneInput : CommonInput
    {
        public string dataType { get; set; }
        public DateTime? dataTime { get; set; }
        public string fileName { get; set; }
    }

    public class LocalDataDoneOutput : CommonOutput
    {


    }

    #endregion




    #region 附录Model
    public class TRetCode
    {
        /// <summary>
        /// 传值请参考响应码对照表
        /// </summary>
        public string code { get; set; }
        public string message { get; set; }
    }

    public class TCardInfoRec
    {
        #region 字段说明
        //cardNumber	String	卡号
        //media	String	卡介质
        //cardType	String	卡类型
        //cardTypeName	String	卡类型名称
        //balance	BigDecimal	余额
        //score	BigDecimal	积分
        //validDate	Date	到效期
        //cardFunction	String	卡功能
        //cardHolder	String	持卡人
        //enterpriseId	String	企业标识
        //randomNumber	String	随机数
        //cardVersion	String	卡版本
        //unSyncTotal	BigDecimal	待同步金额
        //trackTwoData	String	磁卡二轨数据
        //cost	BigDecimal	卡成本
        //lastAction	String	最后操作
        //lastActionTime	Date	最后操作时间
        //limitBalance	BigDecimal	账户最大金额
        //depositCount	int	充值计数
        //consumeCount	int	消费计数
        //noChipBalance	boolean	不用芯片余额，默认否
        //CVN	String	CVN码
        //accountFunction	String	账户功能
        //usepwd	boolean	是否使用卡支付密码, 默认不用
        //saleDepCode	String	卡发售门店代码
        //saleDepName	String	卡发售门店名称
        //lstUpdTime	Date	最后交易时间
        //saleTime	Date	售卡时间
        #endregion

        public string cardNumber { get; set; }
        public string media { get; set; }
        public string cardType { get; set; }
        public string cardTypeName { get; set; }
        public decimal balance { get; set; }
        public decimal score { get; set; }
        public DateTime validDate { get; set; }
        public string cardFunction { get; set; }
        public string cardHolder { get; set; }
        public string enterpriseId { get; set; }
        public string randomNumber { get; set; }
        public string cardVersion { get; set; }
        public decimal unSyncTotal { get; set; }
        public string trackTwoData { get; set; }
        public decimal cost { get; set; }
        public string lastAction { get; set; }
        public DateTime lastActionTime { get; set; }
        public decimal limitBalance { get; set; }
        public int depositCount { get; set; }
        public int consumeCount { get; set; }
        public Boolean noChipBalance { get; set; }
        public string CVN { get; set; }
        public string accountFunction { get; set; }
        public Boolean usepwd { get; set; }
        public string saleDepCode { get; set; }
        public string saleDepName { get; set; }
        public DateTime lstUpdTime { get; set; }
        public DateTime saleTime { get; set; }
    }



    public class TQueryMemberResponse
    {
        public string code { get; set; }
        public string name { get; set; }
        public string memberId { get; set; }
        public string nameSpell { get; set; }
        public DateTime birthday { get; set; }
        public DateTime memorialDay { get; set; }
        public string occupation { get; set; }
        public string specialty { get; set; }
        public string nationality { get; set; }
        public string presenterCode { get; set; }
        public string presenterName { get; set; }
        public string memberGradeName { get; set; }
        public string cellPhone { get; set; }
        public string fixedPhone { get; set; }
        public string email { get; set; }
        public string postCode { get; set; }
        public string qq { get; set; }
        public string address { get; set; }
        public string status { get; set; }
        public string paperType { get; set; }
        public string paperCode { get; set; }
        public string remark { get; set; }
        public string sex { get; set; }
        public string wedding { get; set; }
        public string orgCode { get; set; }
        public string orgName { get; set; }
        public string nationCode { get; set; }
        public string nationName { get; set; }
        public string provinceCode { get; set; }
        public string provinceName { get; set; }
        public string cityCode { get; set; }
        public string cityName { get; set; }
        public string districtCode { get; set; }
        public string districtName { get; set; }
        public string region { get; set; }
        public string source { get; set; }
        public Boolean isMember { get; set; }
        public TMemberGrade memberGrade { get; set; }
        public string login { get; set; }
        public DateTime lastLoginTime { get; set; }
        public string referrer { get; set; }
        public List<TMemberAttribute> attributes { get; set; }
        public int activityHeat { get; set; }
        public Boolean memberSMSReceive { get; set; }
        public string memberSourceCode { get; set; }
        public string memberSourceName { get; set; }
        public DateTime registerDate { get; set; }
        public DateTime childBirthday { get; set; }
        public string memorialDayRemark { get; set; }
        public decimal memGradePay { get; set; }
        public DateTime memGradeValidate { get; set; }
        public string memGradeMessage { get; set; }
        public List<TMemGradeAndRangePay> gradeAndPay { get; set; }
        public string cardNo { get; set; }
        public string memberGradeCode { get; set; }
    }

    public class TExtCardRec
    {
        public string openid { get; set; }
    }

    public class TLossCardInfoRec
    {
        #region 字段说明
        //memberName	String	会员代码
        //memberCode	String	费用
        //fee	BigDecimal	卡状态
        //cardStatus	String	卡信息
        //cardInfo	TCardInfoRec 卡信息
        #endregion

        public string memberName { get; set; }
        public string memberCode { get; set; }
        public decimal fee { get; set; }
        public string cardStatus { get; set; }
        public TCardInfoRec cardInfo { get; set; }
    }

    public class TMemberSourceRec
    {
        #region 字段说明
        //code	String	代码
        //name	String	名称
        //remark	String	说明
        //isDefault	boolean	系统默认
        #endregion

        public string code { get; set; }
        public string name { get; set; }
        public string remark { get; set; }
        public Boolean isDefault { get; set; }
    }

    public class TSalePayRec
    {
        #region 字段说明
        //payModeCode	String	付款方式代码
        //payModeName	String	付款方式名称
        //payMoney	BigDecimal	付款金额
        //payChannel	String	付款渠道
        #endregion

        public string payModeCode { get; set; }
        public string payModeName { get; set; }
        public decimal payMoney { get; set; }
        public string payChannel { get; set; }
    }

    public class TScoreRec
    {
        #region 字段说明
        //tranId	String	交易流水号
        //scoreSubject	String	积分科目
        //scoreType	String	积分类别代码
        //scoreTypeName	String	积分类别名称
        //score	BigDecimal	积分数
        //scoreSource	String	积分来源
        //scoreRemark	String	积分备注
        //total	BigDecimal	产生积分对应的金额
        //shareWay	int	0---门店，1---总部
        //multiple	BigDecimal	积分加速倍数 0表示未加速，1表示不加速，大于1表示加速的倍数
        #endregion

        public string tranId { get; set; }
        public string scoreSubject { get; set; }
        public string scoreType { get; set; }
        public string type { get; set; }
        public string scoreTypeName { get; set; }
        public decimal score { get; set; }
        public string scoreSource { get; set; }
        public string scoreRemark { get; set; }
        public decimal total { get; set; }
        public int shareWay { get; set; }
        public decimal multiple { get; set; }
    }

    //public class TScoreRec1
    //{
    //    #region 字段说明
    //    //tranId	String	交易流水号
    //    //scoreSubject	String	积分科目
    //    //scoreType	String	积分类别代码
    //    //scoreTypeName	String	积分类别名称
    //    //score	BigDecimal	积分数
    //    //scoreSource	String	积分来源
    //    //scoreRemark	String	积分备注
    //    //total	BigDecimal	产生积分对应的金额
    //    //shareWay	int	0---门店，1---总部
    //    //multiple	BigDecimal	积分加速倍数 0表示未加速，1表示不加速，大于1表示加速的倍数
    //    #endregion

    //    public string tranId { get; set; }
    //    public string scoreSubject { get; set; }
    //    public string scoreType { get; set; }
    //    public string scoreTypeName { get; set; }
    //    public decimal score { get; set; }
    //    public string scoreSource { get; set; }
    //    public string scoreRemark { get; set; }
    //    public decimal total { get; set; }
    //    public int shareWay { get; set; }
    //    public decimal multiple { get; set; }
    //}


    public class TDepositRec
    {
        #region 字段说明
        //tranId	String	交易流水号
        //retCode	TRetCode 响应码，默认TRetCode.OK
        //cardType	String	卡类型
        //cardNumber	String	卡号
        //cardPassword	String	卡密码
        //total	BigDecimal	充值金额
        //oldBalance	BigDecimal	原余额
        //newBalance	BigDecimal	新余额
        #endregion

        public string tranId { get; set; }
        public TRetCode retCode { get; set; }
        public string cardType { get; set; }
        public string cardNumber { get; set; }
        public string cardPassword { get; set; }
        public decimal total { get; set; }
        public decimal oldBalance { get; set; }
        public decimal newBalance { get; set; }
    }

    public class TPrizeGoodsRec
    {
        #region 字段说明
        //gdgid	String	商品gid
        //qpc	BigDecimal	商品规格，默认0
        //goodsCode	String	商品代码
        //goodsName	String	商品名称
        //maxCount	BigDecimal	最大兑奖数
        //count	BigDecimal	兑奖数
        //deductiblePrice	BigDecimal	抵扣价，默认0
        //scores	List<TScoreRec> 积分明细
        //rule	TPrizeRuleRec 兑奖规则
        //minUnit	Integer	最小兑换单元，该值jpos没传
        #endregion

        public string gdgid { get; set; }
        public int qpc { get; set; }
        public string goodsCode { get; set; }
        public string goodsName { get; set; }
        public int maxCount { get; set; }
        public int count { get; set; }
        public decimal deductiblePrice { get; set; }
        public List<TScoreRec> scores { get; set; }
        public TPrizeRuleRec rule { get; set; }
        public int minUnit { get; set; }
    }

    public class TPrizeRuleRec
    {
        #region 字段说明
        //ruleUuid	String	兑奖规则的uuid
        //maxPrizeTimes	int	最大兑奖次数
        //sharePercent	BigDecimal	分摊比例, 指门店所占的比例，默认0
        //isTotalPrize	boolean	是否总额兑奖
        #endregion

        public string ruleUuid { get; set; }
        public int maxPrizeTimes { get; set; }
        public int sharePercent { get; set; }
        public Boolean isTotalPrize { get; set; }
    }

    public class TMemberAttributeOptions
    {
        #region 字段说明
        //filedName	String	字段属性名，对应TMemberAttribute.key
        //caption	String	字段显示名
        //type	String	字段类型，值范围：Enum\List\String\Date
        //required	boolean	是否必填，默认否
        //options	String	可选值，例如type=Enum\List则options=[{"key":"1-25","value":"1-25"},{"key":"26-40","value":"26-40"}]；例如type=String\Date则options=""
        //defaultValue	String	默认值
        #endregion

        public string filedName { get; set; }
        public string caption { get; set; }
        public string type { get; set; }
        public Boolean required { get; set; }
        public string options { get; set; }
        public string defaultValue { get; set; }
    }

    public class TQueryVoucherRec
    {
        #region 字段说明
        //voucherNo	String	券号
        //voucherStatus	String	券状态，从CVoucherStatus中赋值
        //parValue	BigDecimal	面额
        //voucherFunc	int	券功能 1-现金券， 2-折扣券
        #endregion

        public string voucherNo { get; set; }
        public string voucherStatus { get; set; }
        public decimal parValue { get; set; }
        public int voucherFunc { get; set; }
    }

    public class TVoucherInfoResponse
    {
        public List<TVoucherInfoRec> vouchers { get; set; }
    }

    public class TVoucherInfoRec
    {
        #region 字段说明
        //voucherNo	String	券号
        //voucherName	String	券名称
        //purchaseRemark	String	限购说明
        //voucherRemark	String	购物券备注
        //parValue	BigDecimal	面额
        //beginDate	Date	起始时间
        //endDate	Date	截至时间
        //imageName	String	图片名称
        #endregion

        public string voucherNo { get; set; }
        public string voucherName { get; set; }
        public string purchaseRemark { get; set; }
        public string voucherRemark { get; set; }
        public decimal parValue { get; set; }
        public DateTime beginDate { get; set; }
        public DateTime endDate { get; set; }
        public string imageName { get; set; }
    }

    #endregion


    #region 基类
    public class CommonInput
    {
        public DateTime? time { get; set; }
        [JsonProperty("operator")]
        public string operatorStr { get; set; }
        public string storeCode { get; set; }
        public string tranId { get; set; }
        public DateTime? tranTime { get; set; }
        public string posNo { get; set; }
        public string signDate { get; set; }
        public string secret { get; set; }
    }

    public class CommonOutput
    {
        public DateTime? time { get; set; }
        public TRetCode retCode { get; set; }
        public string tranId { get; set; }
        public DateTime? tranTime { get; set; }
        [JsonProperty("operator")]
        public string operatorStr { get; set; }

    }

    #endregion

}
