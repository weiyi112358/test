using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    public class MemberInfo
    {
        public string Token { get; set; }
        public string MemState { get; set; }
        public string MemType { get; set; }
        public string MemName { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string CertType { get; set; }
        public string CertNo { get; set; }
        public string StoreCode { get; set; }
        public string Region { get; set; }
        public string Brand { get; set; }
        public string CustomerNo { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        public string Wechat { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string ProvinceCode { get; set; }
        public string CityCode { get; set; }
        public string DistrictCode { get; set; }
        public decimal? RechargeGold { get; set; }
        public decimal? SendGold { get; set; }
        public decimal? TotalGold { get; set; }
        public decimal? NoInvoiceGold { get; set; }//未开票金额
        public string CustomerLevel { get; set; }
        public int Department { get; set; }//业务部门
        public string Corp { get; set; }
        public string PosNo { get; set; }
        public string SkinType { get; set; }
        public string SkinCareType { get; set; }
        public string MakeupFrequency { get; set; }
        public string MarriageStatus { get; set; }
        public string HasChild { get; set; }
        public string Job { get; set; }
        public string MonthlyIncome { get; set; }
        public string KnowingFrom { get; set; }
        public string NeededMessage { get; set; }


    }

    public class VechileInfo
    {

        public long VechileId { get; set; }
        public string MemberID { get; set; }
        public string VechileNo { get; set; }
        public string VechileBrand { get; set; }
        public string VechileSerice { get; set; }
        public string VechileType { get; set; }
        public string VechileColor { get; set; }
        public string VechileInner { get; set; }
        public string Mile { get; set; }
        public string VinNo { get; set; }
        public string BuyDate { get; set; }
        public string IsTransfer { get; set; }
    }

    public class TradeInfo
    {
        public string TradeTypeA { get; set; }
        public string StoreCode { get; set; }
        public int Vehicle { get; set; }
        public string TradeTypeB { get; set; }
        public string NotGoldAmountSales { get; set; }
        public string HoursAmountSales { get; set; }
        public string PartsAmountSales { get; set; }
        public string Amount { get; set; }
        public string StandardAmountSales { get; set; }
        public string GoldAmountSales { get; set; }
        public string ListDateSales { get; set; }
        public string MemberId { get; set; }
        public string TradeCode { get; set; }
        public string PayType { get; set; }
        public string Remark { get; set; }
        public string Sales { get; set; }
        public string Created { get; set; }
        public string[] AvailableCoupon { get; set; }
    }
    public class InvoiceInfo
    {
        public string InvoiceNo { get; set; }
        public string InvoiceId { get; set; }
        public string MemberId { get; set; }
        public decimal InvoiceCash { get; set; }
        public string StoreCode { get; set; }
        public string MemType { get; set; }
        public string Telephone { get; set; }
        public string InvoiceDrawer { get; set; }

        public string InvoiceType { get; set; }
        public string CorpName { get; set; }
        public string IdentyNo { get; set; }
        public string CreditCode { get; set; }
        public string CorpMobile { get; set; }
        public string Address { get; set; }
        public string Bank { get; set; }
        public string BankNo { get; set; }
    }

    public class ActInfo
    {
        public string MemberId { get; set; }
        public decimal? NoInvoiceAct { get; set; }//未开票金额
        public decimal? InvoiceAct { get; set; }//已开票金额
        public decimal? TotalAct { get; set; }//累计充值金额
        public decimal? RechargeAct { get; set; }//充值金币
        public decimal? SendAct { get; set; }//赠送金币
    }

    public class TradeSales
    {
        public long TradeID { get; set; }
        public string MemberId { get; set; }
        public string TradeCode { get; set; }
        public string TradeTypeA { get; set; }
        public string TradeTypeB { get; set; }
        public string TradeTypeACode { get; set; }
        public string TradeTypeBCode { get; set; }
        public Decimal? NotGoldAmountSales { get; set; }
        public Decimal? HoursAmountSales { get; set; }
        public Decimal? PartsAmountSales { get; set; }
        public Decimal? Amount { get; set; }
        public Decimal? StandardAmountSales { get; set; }
        public Decimal? GoldAmountSales { get; set; }
        public DateTime? ListDateSales { get; set; }
        public string MemberCardNo { get; set; }
        public string CustomerName { get; set; }
        public string Mobile { get; set; }
        public string StatusSales { get; set; }
        public string StatusSalesText { get; set; }
        public string PlateNumVehicle { get; set; }
        public string VINVehicle { get; set; }
        public string StoreCodeSales { get; set; }
    }
}
