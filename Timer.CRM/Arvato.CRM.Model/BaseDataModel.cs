using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    //维度/会员属性
    public class FieldAliasModel
    {
        public int? AliasID { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string FieldAlias { get; set; }
        public string AliasType { get; set; }
        public string AliasKey { get; set; }
        public string FieldDesc { get; set; }
        public string DictTableName { get; set; }
        public string DictTableType { get; set; }
        public bool IsFilterBySubdivision { get; set; }
        public bool IsFilterByLoyRule { get; set; }
        public bool IsFilterByLoyActionLeft { get; set; }
        public bool IsFilterByLoyActionRight { get; set; }
        public bool IsCommunicationTemplet { get; set; }
        public string ControlType { get; set; }
        public string Reg { get; set; }
        public System.DateTime AddedDate { get; set; }
        public string ComputeScript { get; set; }
        public Nullable<short> RunType { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string DataLimitType { get; set; }
        public string AliasSubKey { get; set; }
        public int? ParameterCount { get; set; }
        public List<FieldAliasParameter> FieldPara { get; set; }
        public bool IsDynamic { get; set; }
    }

    public class FieldAliasParameter
    {
        public int? ParaID { get; set; }
        public int? AliasID { get; set; }
        public string ParaIndex { get; set; }
        public string Reg { get; set; }
        public string FieldType { get; set; }
        public string ControlType { get; set; }
        public string DictTableName { get; set; }
        public string DictTableType { get; set; }
        public string ParameterName { get; set; }
        public int? UIIndex { get; set; }
        public bool IsRequired { get; set; }

    }

    //品牌
    public class Brand
    {
        public long? BaseDataID { get; set; }
        public int? DataGroupID { get; set; }
        public string BaseDataType { get; set; }
        public string BrandName { get; set; }
        public string BrandCode { get; set; }
        public string EnableBrand { get; set; }
    }

    /// <summary>
    /// 区域
    /// </summary>
    public class Area
    {
        public long? BaseDataID { get; set; }
        public int? DataGroupID { get; set; }
        public string BaseDataType { get; set; }
        public string AreaNameBase { get; set; }
        public string AreaCodeBase { get; set; }
    }

    public class Vehicle
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Grade { get; set; }
        public string ParentCode { get; set; }
        public string Sort { get; set; }
    }
    //渠道
    public class ChannelModel
    {
        public long? BaseDataID { get; set; }
        public int? DataGroupID { get; set; }
        public string BaseDataType { get; set; }
        public string ChannelName { get; set; }
        public string ChannelCode { get; set; }
        //public string ChannelIsEnableBase { get; set; }
    }

    //条目模型
    public class ItemModel
    {
        public long? BaseDataID { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public string ItemDesc { get; set; }
        public Nullable<bool> ItemEnable { get; set; }
        public Nullable<System.DateTime> ItemAddedTime { get; set; }
        public int DataGroupID { get; set; }
        public string BaseDataType { get; set; }
    }

    //门店模型
    public class StoreModel
    {
        public long? BaseDataID { get; set; }
        public int? DataGroupID { get; set; }
        public string BaseDataType { get; set; }
        public string StoreFullName { get; set; }
        public string StoreName { get; set; }
        public string StoreCode { get; set; }
        public string StoreAddress { get; set; }
        public string StoreBrandCode { get; set; }
        public string StoreCodeSale { get; set; }
        public string AreaCodeStore { get; set; }
        public string AreaNameStore { get; set; }
        public string ProvinceCodeStore { get; set; }
        public string ProvinceStore { get; set; }
        public string CityCodeStore { get; set; }
        public string CityStore { get; set; }
        public string ChannelCodeStore { get; set; }
        public string ChannelNameStore { get; set; }
        public string ChannelTypeCodeStore { get; set; }
        public string ChannerTypeNameStore { get; set; }
        public string StoreType { get; set; }
        public string StoreTel { get; set; }
        public string Printer { get; set; }
        public string SerialCode { get; set; }
        public string StoreCode_IPOS { get; set; }
        public string BrandStore { get; set; }
    }

    //企业模型
    public class CorpModel
    {
        public long? BaseDataID { get; set; }
        public int? DataGroupID { get; set; }
        public string BaseDataType { get; set; }
        public string CorpName { get; set; }
        public string CorpContract { get; set; }
        public string CorpPhoneNo { get; set; }
        public string CorpAddress { get; set; }
    }
    //门店设定模型
    public class StoreSetting
    {
        public long? BaseDataID { get; set; }
        public int? DataGroupID { get; set; }
        public string BaseDataType { get; set; }
        public string StoreSettingStoreCode { get; set; }
        public decimal OrderMaxPoint { get; set; }
        public decimal PointCashPec { get; set; }
    }
    //业务部门
    public class BsiDpt
    {
        public int? ID { get; set; }
        public string BisDptName { get; set; }
        public int? ParentGrade { get; set; }
        public int? ParentBisDpt { get; set; }
    }
    //维修类型模型
    public class RepairModel
    {
        public int? ID { get; set; }
        public List<ActLimit> StoreCode { get; set; }
        public string RepairTypeName { get; set; }
        public bool IsApplyToLoyPoint { get; set; }
        public bool IsApplyToLoyDimension { get; set; }
        public bool IsApplyToLoyStatus { get; set; }
    }
    public partial class Package
    {
        public int PackageID { get; set; }
        public int DataGroupID { get; set; }
        public string PackageName { get; set; }
        public string PackageDesc { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> AppendQty { get; set; }
        public string AppendUnit { get; set; }
        public Nullable<decimal> Price1 { get; set; }
        public Nullable<decimal> Price2 { get; set; }
        public string Proportion { get; set; }
        public Nullable<decimal> MaxSetPrice { get; set; }
        public string PriceRelation { get; set; }
        public System.DateTime AddedDate { get; set; }
        public string AddedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public bool Enable { get; set; }
    }

    public partial class PackageDetail
    {
        public int PackageDetailID { get; set; }
        public int PackageID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public int Qty { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> AppendQty { get; set; }
        public string AppendUnit { get; set; }
        public string IDOSPackageMapping { get; set; }

        public string Proportion { get; set; }
        public Nullable<decimal> MaxSetPrice { get; set; }
    }

    public partial class SysOption
    {
        public int AliasDegeeID { get; set; }
        public string BasicContent { get; set; }
        public string ValueDegeeType { get; set; }
        public int AliasID { get; set; }
        public int AliasID2 { get; set; }
        public int Sort { get; set; }
    }

    public partial class DegreeOption
    {
        public int DegreeModelID { get; set; }
        public string BasicContent { get; set; }
        public string Name { get; set; }
    }

    public class limitType
    {
        public string LimitType { get; set; }
        public string LimitValue { get; set; }
    }

    public class Campaign
    {
        public long ID { get; set; }
        public int DataGroupID { get; set; }
        public string BaseDataType { get; set; }
        public string CampType { get; set; }
        public string AliaskeyBase { get; set; }
        public string AliasSubkeyBase { get; set; }
        public string CampAttrName { get; set; }
        public int CampGrade { get; set; }
    }

    /// <summary>
    /// 供应商
    /// </summary>
    public class Supplier
    {
        public long? BaseDateID { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string SupplierPhone { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierLastUpdateUser { get; set; }
        public string SupplierFax { get; set; }
        public string SupplierContactPerson { get; set; }
        public string SupplierCreateUser { get; set; }
        public string SupplierDescription { get; set; }
        public string SupplierStatus { get; set; }
        public string SupplierFullName { get; set; }
        public DateTime? SupplierCreateTime { get; set; }
    }

    public class StoreMaintenanceToExcel
    {
        public long? BaseDataID { get; set; }
        public int? DataGroupID { get; set; }
        public string BaseDataType { get; set; }
        public string StoreFullName { get; set; }
        public string StoreName { get; set; }
        public string StoreCode { get; set; }
        public string StoreAddress { get; set; }
        public string StoreBrandCode { get; set; }
        public string StoreCodeSale { get; set; }
        public string AreaCodeStore { get; set; }
        public string AreaNameStore { get; set; }
        public string ProvinceCodeStore { get; set; }
        public string ProvinceStore { get; set; }
        public string CityCodeStore { get; set; }
        public string CityStore { get; set; }
        public string ChannelCodeStore { get; set; }
        public string ChannelNameStore { get; set; }
        public string ChannelTypeCodeStore { get; set; }
        public string ChannerTypeNameStore { get; set; }
        public string StoreType { get; set; }
        public string StoreTel { get; set; }
        public string Printer { get; set; }
        public string SerialCode { get; set; }
        public string StoreCode_IPOS { get; set; }
        public string BrandStore { get; set; }
    }
}
