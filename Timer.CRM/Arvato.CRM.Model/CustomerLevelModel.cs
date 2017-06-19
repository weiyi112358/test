using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace Arvato.CRM.Model
{
    public class CustomerLevelModel
    {

        public int ID { get; set; }
        [DisplayName("会员等级")]
        [Required]
        public string CustomerLevel { get; set; }
        [DisplayName("等级名称")]
        [Required(ErrorMessage = "等级名称不能为空")]
        public string CustomerLevelName { get; set; }

        [DisplayName("所属品牌")]
        [Required]
        public string BrandCodeCustomerLevel { get; set; }
        [DisplayName("所属品牌名称")]
        [Required(ErrorMessage = "所属品牌名称不能为空")]
        public string BrandNameCustomerLevel { get; set; }
        [Required]
        [DisplayName("折扣")]
        [Range(typeof(decimal), "0.01", "1.00", ErrorMessage = "折扣范围在0.01到1.00之间")]
        public decimal Rate { get; set; }
        public int DataGroupID { get; set; }
        [DisplayName("最大抵用积分")]
        [Required(ErrorMessage = "不能为空")]
        public int MaxIntergral { get; set; }
        [DisplayName("当月储值币可用比率")]
        [Required(ErrorMessage = "不能为空")]
        public decimal? RateMaxUse { get; set; }

        //public string Remark { get; set; }
        //public DateTime AddedDate { get; set; }
        //public string AddedUser { get; set; }
        //public DateTime ModifiedDate { get; set; }
        //public string ModifiedUser { get; set; }
    }
}
