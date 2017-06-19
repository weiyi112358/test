using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model.Interface
{
    public class CommonJsonOutput
    {
        public int Status { get; set; }
        public string ErrorMsg { get; set; }
    }




    public class WeChatMemberInput
    {
        public string signdate { get; set; }
        public string secret { get; set; }
        //public string openId { get; set; }
        //public string electronicCardNo { get; set; }//电子卡号
        //public string realityCardNo { get; set; }//实体卡号
        public string FPhoneNum { get; set; }//手机号
        public string FName { get; set; }//姓名
        public string FSex { get; set; }//性别
        public DateTime? FBirthDay { get; set; }//生日
        public string FOpenId { get; set; }


        //public string sourceParameter { get; set; }//来源参数
        //public string status { get; set; }

        //public string papersType { get; set; }//证件号码
        //public string papersNo { get; set; }//证件号码
        //public string Email { get; set; }//电子邮箱
        //public string province { get; set; }//省
        //public string city { get; set; }//市
        //public string area { get; set; }//区
        //public string mailAddress { get; set; }//邮寄地址
        //public string zipCode { get; set; }//邮编

        //public int levelId { get; set; }//充值档次ID
        //public decimal rechangeMoney { get; set; }//充值金额
        //public string orderNo { get; set; }//单号
        //public string tradeCode { get; set; }// 订单ID
        //public decimal? settleMoney { get; set; }
        //public string password { get; set; }

    }

    public class WeChatMemberOutput : CommonJsonOutput
    {
        public string FCardNum { get; set; }//会员卡号
        public string FType { get; set; }
        public string FGoldNum { get; set; }

    }

    public class QueryPointsInput
    {
        public string signdate { get; set; }
        public string secret { get; set; }
        public string FCardNum { get; set; }//会员卡号

    }

    public class QueryPointsOutput : CommonJsonOutput
    {

        public decimal FAmount { get; set; }
        public DateTime? FExpireTime { get; set; }
        public decimal FDeductAmount { get; set; }
        public decimal FFastExpire { get; set; }

    }



    public class QueryGradeInput
    {
        public string signdate { get; set; }
        public string secret { get; set; }
        public string FCardNum { get; set; }//会员卡号

    }

    public class QueryGradeOutput : CommonJsonOutput
    {

        public string FGradeType { get; set; }
        public DateTime? FGradeDate { get; set; }
        public string FRebate { get; set; }
        public decimal FGradeAmount { get; set; }
        public decimal FDifferAmount { get; set; }

    }


    public class QueryPointsDetailInput
    {
        public string signdate { get; set; }
        public string secret { get; set; }
        public string FCardNum { get; set; }//会员卡号

    }

    public class QueryPointsDetailOutput : CommonJsonOutput
    {

        public List<PointDetail> PointDetailClass { get; set; }

    }

    public class PointDetail
    {
        public DateTime? FProdDate { get; set; }
        public string FProdOrg { get; set; }
        public string FType { get; set; }

        public decimal? FExpendAmount { get; set; }
        public decimal? FIntegralAmount { get; set; }
        public decimal? FUseIntegral { get; set; }
        public decimal? FFastExpire { get; set; }
        public DateTime? FFastDate { get; set; }


    }

    public class BindMemberInput
    {
        public string signdate { get; set; }
        public string secret { get; set; }
        public string OpenId { get; set; }
        public string LoginName { get; set; }

    }

    public class BindMemberOutput : CommonJsonOutput
    {

        public string FCardNum { get; set; }//会员卡号
        public string FType { get; set; }
        public string FGoldNum { get; set; }

        public string FName { get; set; }//姓名
        public string FSex { get; set; }//性别
        public DateTime? FBirthDay { get; set; }//生日

    }

    public class QueryUserInput
    {
        public string signdate { get; set; }
        public string secret { get; set; }
    }

    public class Store
    {
        public string StoreCode { get; set; }  //门店代码
        public string StoreName { get; set; }
    }

    public class User
    {
        public string LoginName { get; set; }  //登录账户
        public string UserName { get; set; }   //登录名称
        public string StoreCode { get; set; }
        public string FUpdateDate { get; set; }
        public string FType { get; set; }
        public bool FEable { get; set; }
        public string FEmail { get; set; }
        public string FPhone { get; set; }
        public string FWeixinID { get; set; }
        public string FSex { get; set; }
        public string FPosition { get; set; }
        public string FTell { get; set; }
    }

    public class UserStore
    {
        public string LoginName { get; set; }  //登录账户
        public string UserName { get; set; }   //登录名称
        public string StoreCode { get; set; }  //门店代码
        public string StoreName { get; set; }
    }

    public class QueryUserOutPut : CommonJsonOutput
    {
        public List<User> Users { get; set; }
        public QueryUserOutPut()
        {
            Users = new List<User>();
        }
    }

    public class QueryOrganizationInput
    {
        public string signdate { get; set; }
        public string secret { get; set; }
    }

    public class Obj
    {
        /// <summary>
        /// 门店ID
        /// </summary>
        public string GID { get; set; }
        /// <summary>
        /// 门店代码
        /// </summary>
        public string CODE { get; set; }
        /// <summary>
        /// 门店名称
        /// </summary>
        public string STORENAME { get; set; }
        /// <summary>
        /// 所属省
        /// </summary>
        public string PROVINCE { get; set; }
        /// <summary>
        /// 所属市
        /// </summary>
        public string CITY { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string ADDRESS { get; set; }

        public bool IsStore { get; set; }

        public string ParentCode { get; set; }

        public DateTime UpdateTime { get; set; }

        public bool Enable { get; set; }
    }

    public class QueryOrganizationOutPut : CommonJsonOutput
    {
        public List<Obj> Objs { get; set; }
        public QueryOrganizationOutPut()
        {
            Objs = new List<Obj>();
        }
    }

    public class WXQueryMemberInput
    {
        public string signdate { get; set; }
        public string secret { get; set; }
        public string FPhoneNum { get; set; }//手机号
    }

    public class WXQueryMemberOutput : CommonJsonOutput
    {
        public string FName { get; set; }
        public string FSex { get; set; }
        public string FBirthDay { get; set; }
        public string FPhoneNum { get; set; }
        public string FCardNum { get; set; }
    }

    public class UpdateMemberInput
    {
        public string openId { get; set; }
        public string FCardNum { get; set; }
        public string FPhoneNum { get; set; }
        public string FName { get; set; }
        public string FSex { get; set; }
        public string FBirthDay { get; set; }
        public string signdate { get; set; }
        public string secret { get; set; }
    }

    public class UpdateMemberOutput : CommonJsonOutput
    {
    }

    public class AddSMSInput
    {
        public string Mobile { get; set; }
        public string Message { get; set; }
        public string signdate { get; set; }
        public string secret { get; set; }
    }

    public class AddSMSOutput : CommonJsonOutput
    {

    }


    public class QueryMemberTradesInput
    {
        public string FCardNum { get; set; }
        public int Counts { get; set; }
        public string signdate { get; set; }
        public string secret { get; set; }

    }

    public class QueryMemberTradesOutput : CommonJsonOutput
    {
        public List<Order> Order { get; set; }


    }
    public class Order
    {
        public string OrderCode { get; set; }
        public DateTime? OrderDate { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public decimal? Integral { get; set; }
        public decimal? Amounts { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }


    }


    public class OrderDetail
    {
        public string GoodsName { get; set; }
        public decimal? GoodsAmount { get; set; }
        //public string OrderCode { get; set; }


    }
}













