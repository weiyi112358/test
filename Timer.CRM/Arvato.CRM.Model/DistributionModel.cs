using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
   public class ApplyCard
   {     
       public string AcceptingUnit { get; set; }
       public string ArriveTime { get; set; }
       public string Code { get; set; }
       public string Purpose { get; set; }
       public string ApplyNumber { get; set; }
       public string Identity { get; set; }
    }

   public class ExecuteVerify
   {
       public string Id { get; set; }
       public string Status { get; set; }
   }

   public class CardOutTitleInfo
   {
       public string AcceptingUnit { get; set; }
       public string OddIdNo { get; set; }
       public string Create { get; set; }
       public string AcceptingShoppe { get; set; }
       public string SendingUnit { get; set; }
       public string Remark { get; set; }
       public string OddId { get; set; }
       public string BoxNo { get; set; }
       public string CardTypeCode { get; set; }
       public string Purpose { get; set; }
       public string CardNumIn { get; set; }
       public string IsOddId { get; set; }
       public string ApplyId { get; set; }
   }

   public class CardOutBranchInfo
   {
       public string OddIdNo { get; set; }
       public string Create { get; set; }
       public string AcceptingShoppe { get; set; }
       public string SendingUnit { get; set; }
       public string Remark { get; set; }
       public string OddId { get; set; }
       public string BoxNo { get; set; }
       public string CardTypeCode { get; set; }
       public string Purpose { get; set; }
       public string CardNumIn { get; set; }
       public string IsOddId { get; set; }
   }
   public class CardRepealInfo
   {
       public string AcceptingUnit { get; set; }
       public string ExecuteMsg { get; set; }
       public string Create { get; set; }
       public string AcceptingShoppe { get; set; }
       public string SendingUnit { get; set; }
       public string Remark { get; set; }
       public string OddId { get; set; }
       public string BoxNo { get; set; }
       public string CardTypeCode { get; set; }
       public string Purpose { get; set; }
       public string CardNumIn { get; set; }
       public int CanReturnCardNumber { get; set; }
       public string SelReturnUnit { get; set; }
   }
   public class QueryBoxInfo
   {
       public string BoxNo { get; set; }
       public string PurposeId { get; set; }
       public string Purpose { get; set; }
       public int CardTypeCode { get; set; }
       public string CardTypeName { get; set; }
       public int CardNumIn { get; set; }
       public string BeginCardNo { get; set; }
       public string EndCardNo { get; set; }
       public string AcceptingUnit { get; set; }
       public int CanReturnCardNumber { get; set; }   
   }

   public class ApplyDetailInfo
   {
       public string CardTypeName { get; set; }
       public string Purpose { get; set; }
       public int? ApplyNumber { get; set; }
       public int? ApproveNumber { get; set; }
       public string StoreName { get; set; }
   }

   public class CardTitleOutBoxInfo
   {
       public string BoxNo { get; set; }
       public string BeginCardNo { get; set; }
       public string EndCardNo { get; set; }
       public string StoreName { get; set; }
   }

   public class CardOutTitleInfoNew
   {
       public string Company { get; set; }
       public string CreateBy { get; set; }
       public string Store { get; set; }
       public string SendUnit { get; set; }
       public string Remark { get; set; }
       public string ApplyOddId { get; set; }
       public string BoxNo { get; set; }
       public string CardTypeCode { get; set; }
       public string Purpose { get; set; }
       public string CardNumIn { get; set; }
       public string IsOddId { get; set; }
   }

   public class ApplyCardToExcel
   {
       public string ExecuteStatus { get; set; }
       public string Status { get; set; }
       //public string AcceptingUnit { get; set; }
       public string ApplyNumber { get; set; }
       public string Channel { get; set; }
       public string ApproveNumber { get; set; }
       public string DeliverNumber { get; set; }
       public string CreateBy { get; set; }
       public string CreateTime { get; set; }
       public string OddIdNo { get; set; }
   }

   public class CardOutTitleListToExcel
   {
       public string OddIdNo { get; set; }
       public string BoxNumber { get; set; }
       public string SendingUnit { get; set; }
       public string StatusName { get; set; }
       public string CreateTime { get; set; }   
   }

   public class CardOutBranchListToExcel
   {
       public string OddIdNo { get; set; }
       public string BoxNumber { get; set; } 
       public string StatusName { get; set; }
       public string CreateTime { get; set; }
       public string SendingUnit { get; set; }
   }


   public class CardRepealBranchListToExcel
   {
       public string OddIdNo { get; set; }
       public string BoxNumber { get; set; }
       public string AcceptingName { get; set; }
       public string StatusName { get; set; }
       public string CreateTime { get; set; }
       public string SendingUnitName { get; set; }
       public string CardNumber { get; set; }
       public string CreateBy { get; set; }
   }

   public class CardRepealStoreListToExcel
   {
       public string OddIdNo { get; set; }
       public string BoxNumber { get; set; }
       public string AcceptingName { get; set; }
       public string StatusName { get; set; }
       public string CreateTime { get; set; }
       public string SendingUnitName { get; set; }
       public string CardNumber { get; set; }
       public string CreateBy { get; set; }
   }

   public class CardRepealTitleListToExcel
   {
       public string OddIdNo { get; set; }
       public string BoxNumber { get; set; }
       public string AcceptingUnitName { get; set; }
       public string StatusName { get; set; }
       public string CreateTime { get; set; }
       public string SendingUnit { get; set; }
       public string CardNumber { get; set; }
       public string CreateBy { get; set; }
   }

   public class RepealBoxNo
   {
       public string BoxNo { get; set; }
   }

   public class CardTitleOutQuery
   {
       public string OddId { get; set; }
       public string Status { get; set; }
       public string StatusName { get; set; }
       public string SendingUnit { get; set; }
       public string AcceptUnitCode { get; set; }
       public string BoxNumber { get; set; }
       public string CreateBy { get; set; }
       public string CreateTime { get; set; }
       public string OddIdNo { get; set; }
   }
}