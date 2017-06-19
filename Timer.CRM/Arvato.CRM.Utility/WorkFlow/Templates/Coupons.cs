using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Utility.WorkFlow.Templates
{
    /// <summary>
    /// 优惠券基类
    /// </summary>
    public class Coupon
    {
        /// <summary>
        /// 是否为公共优惠券
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// 是否是异业券
        /// </summary>
        public bool IsOthers { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDate { set; get; }

        /// <summary>
        /// 结束时间
        /// </summary>
        private DateTime? _EndDate;
        public DateTime? EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value != null ? value.Value.Date.AddDays(1).AddSeconds(-1) : value; }
        }


        /// <summary>
        /// 最大可用数量
        /// </summary>
        public int? MaxAvaiCount { set; get; }

        /// <summary>
        /// 获取渠道
        /// </summary>
        public string GetChannel { set; get; }

        /// <summary>
        /// 偏移量
        /// </summary>
        public int? OffNumber { set; get; }

        /// <summary>
        /// 便宜单位
        /// </summary>
        public string Unit { set; get; }
    }

    /// <summary>
    /// 现金券模板
    /// 格式：
    ///{"price":"100","ispublic":false,"offNumber":"3","unit":"day","startdate":"2015-10-13","enddate":"2015-10-30"}
    /// </summary>
    public class VoucherCoupon : Coupon
    {
        /// <summary>
        /// 抵用现金
        /// </summary>
        public decimal Price { set; get; }
    }

    /// <summary>
    /// 折扣券模板
    /// 格式：
    ///  {"discount":"0.8","ispublic":false,"offNumber":"1","unit":"day","startdate":"2015-10-06","enddate":"2015-10-30"}
    /// </summary>
    public class DiscountCoupon : Coupon
    {
        /// <summary>
        /// 折扣率(0.01~1.00)
        /// </summary>
        public decimal Discount { set; get; }
    }

    /// <summary>
    /// 满减卷模板
    /// 格式：
    /// {"reach":"100","reduce":"10","ispublic":false,"offNumber":"1","unit":"day","startdate":"2015-10-06","enddate":"2015-10-30"}
    /// </summary>
    public class ReduceCoupon : Coupon
    {
        /// <summary>
        /// 最低消费金额
        /// </summary>
        public decimal Reach { set; get; }

        /// <summary>
        /// 减免金额
        /// </summary>
        public decimal Reduce { set; get; }
    }
}
