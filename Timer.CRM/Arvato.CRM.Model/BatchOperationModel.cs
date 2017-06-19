using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    public class BatchOperationModel
    {
        public string OperationID { get; set; }
        public Nullable<long> OddNumber { get; set; }
        public string Status { get; set; }
        public string OperationType { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public string AddedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public string Where { get; set; }
        public List<OperationWhere> OperationWhere { get; set; }
        public List<OperationCardModel> OperationCard { get; set; }
        public Nullable<System.DateTime> ModifyTimeBegin { get; set; }
        public Nullable<System.DateTime> ModifyTimeEnd { get; set; }
    }
    public class OperationWhere
    {

        public int sort { get; set; }
        public Nullable<long> beginCard { get; set; }
        public Nullable<long> endCard { get; set; }
        public string CardStatus { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string StoreCode { get; set; }
       
        public string StoreName { get; set; }
        public string StatuStr { get; set; }
        public string CardTypeName { get; set; }
        public string CardType { get; set; }
    }
    public class OperationCardModel
    {
        public string CardNo { get; set; }
        public string Statu { get; set; }
        public string StoreCode { get; set; }
        public Nullable<Int32> IsSalesStatus { get; set; }
        public string Status { get; set; }
        public Nullable<Boolean> IsUsed { get; set; }
        public string CardTypeName { get; set; }
        public string CardType { get; set; }
    }
}
