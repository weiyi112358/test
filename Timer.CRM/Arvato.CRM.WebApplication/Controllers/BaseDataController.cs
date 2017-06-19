using Arvato.CRM.Model;
using Arvato.CRM.Model.Config;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Arvato.CRM.WebApplication.Controllers
{
    public class BaseDataController : Controller
    {
        //
        // GET: /BaseData/

        #region DataGroup数据群组

        /// <summary>
        /// 数据群组视图
        /// </summary>
        /// <returns></returns>
        public ActionResult DataGroup()
        {
            return View();
        }

        /// <summary>
        /// 获取数据群组信息（分页）
        /// </summary>
        /// <param name="groupGrade"></param>
        /// <param name="groupName"></param>
        /// <param name="addDate"></param>
        /// <returns></returns>
        public JsonResult GetDataGroup(int? groupGrade, string groupName, DateTime? addDate)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetDataGroup", false, new object[] { groupGrade, groupName, addDate, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 获取数据群组信息（树图）
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDataGroupList()
        {
            var result = Submit("BaseData", "GetDataGroupList", false);
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 根据主键ID获取单条数据群组信息
        /// </summary>
        /// <param name="dataGroupID"></param>
        /// <returns></returns>
        public JsonResult GetDataGroupById(int dataGroupID)
        {
            var result = Submit("BaseData", "GetDataGroupById", false, new object[] { dataGroupID });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 根据主键ID删除单条数据群组信息
        /// </summary>
        /// <param name="dataGroupID"></param>
        /// <returns></returns>
        public JsonResult DeleteDataGroupById(int dataGroupID)
        {
            var result = Submit("BaseData", "DeleteDataGroupById", false, new object[] { dataGroupID });
            return Json(result);
        }

        /// <summary>
        /// 增加或者更新群组信息
        /// </summary>
        /// <param name="dataGroupName"></param>
        /// <param name="dataGroupGrade"></param>
        /// <param name="pDataGroupID"></param>
        /// <param name="dgId"></param>
        /// <returns></returns>
        public JsonResult AddOrUpdateDataGroup(string dataGroupName, int dataGroupGrade, int? pDataGroupID, int? dgId)
        {
            var result = Submit("BaseData", "AddOrUpdateDataGroup", false, new object[] { dataGroupName, dataGroupGrade, pDataGroupID, dgId });
            return Json(result);
        }

        /// <summary>
        /// 获取群组等级列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDataGroupGrade()
        {
            var result = Submit("BaseData", "GetDataGroupGrade", false);
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 获取所属父群组列表
        /// </summary>
        /// <param name="dataGroupId"></param>
        /// <returns></returns>
        public JsonResult GetParentDataGroup(int? dataGroupGrade)
        {
            var result = Submit("BaseData", "GetParentDataGroup", false, new object[] { dataGroupGrade });
            return Json(result.Obj[0].ToString());
        }
        #endregion

        #region 维度/会员属性

        /// <summary>
        /// 维度视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Dimension()
        {
            return View();
        }

        /// <summary>
        /// 属性视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Attribute()
        {
            return View();
        }

        /// <summary>
        /// 获取维度数据列表
        /// </summary>
        /// <param name="fieldDesc"></param>
        /// <param name="fieldAlias"></param>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        public JsonResult GetDimensionList(string fieldDesc, string fieldAlias, string fieldType)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetDimensionList", false, new object[] { fieldDesc, fieldAlias, fieldType, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 根据主键ID删除维度数据
        /// </summary>
        /// <param name="aliasId"></param>
        /// <returns></returns>
        public JsonResult DeleteDimensionById(int aliasId)
        {
            var result = Submit("BaseData", "DeleteDimensionById", false, new object[] { aliasId });
            return Json(result);
        }

        /// <summary>
        /// 根据主键ID获取维度数据
        /// </summary>
        /// <param name="aliasId"></param>
        /// <returns></returns>
        public JsonResult GetDimensionById(int aliasId)
        {
            var result = Submit("BaseData", "GetDimensionById", false, new object[] { aliasId });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 增加或者修改维度数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult AddorUpdateDimensionData(FieldAliasModel model)
        {
            model.Reg = HttpUtility.HtmlDecode(model.Reg);
            model.ComputeScript = HttpUtility.HtmlDecode(model.ComputeScript);
            var result = Submit("BaseData", "AddorUpdateDimensionData", false, new object[] { model });
            return Json(result);
        }

        /// <summary>
        /// 获取系统属性列表
        /// </summary>
        /// <param name="fieldDesc"></param>
        /// <param name="fieldAlias"></param>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        public JsonResult GetAttributeList(string fieldDesc, string fieldAlias, string fieldType)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetAttributeList", false, new object[] { fieldDesc, fieldAlias, fieldType, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        public JsonResult GetCampaignAttributeList(string fieldDesc, string fieldAlias, string fieldType)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetCampaignAttributeList", false, new object[] { fieldDesc, fieldAlias, fieldType, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 增加或者修改属性数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult AddorUpdateAttributeData(FieldAliasModel model)
        {
            model.Reg = HttpUtility.HtmlDecode(model.Reg);
            var result = Submit("BaseData", "AddorUpdateAttributeData", false, new object[] { model });
            return Json(result);
        }

        /// <summary>
        /// 获取动态参数列表
        /// </summary>
        /// <param name="aliasID"></param>
        /// <returns></returns>
        public JsonResult GetParaList(int aliasID)
        {
            var result = Submit("BaseData", "GetParaList", false, new object[] { aliasID });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 获取字段类型数据列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetFieldTypeList()
        {
            var result = Submit("Service", "GetFieldTypeList", false);
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 获取会员属性关系列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRelationshipList()
        {
            var result = Submit("Service", "GetRelationshipList", false);
            return Json(result.Obj[0].ToString());
        }

        /// <summary>GetCampaginOptionDataList
        /// 获取option表关于系统属性的数据
        /// </summary>
        /// <param name="optType">字段类型</param>
        /// <returns></returns>
        public JsonResult GetOptionDataList(string optType)
        {
            var result = Submit("Service", "GetOptionDataList", false, new object[] { optType });
            return Json(result.Obj[0].ToString());
        }
        public JsonResult GetCampaginOptionDataList(string optType)
        {
            var result = Submit("BaseData", "GetCampaginOptionDataList", false, new object[] { optType });
            return Json(result.Obj[0].ToString());
        }

        public JsonResult GetMemVehicle(string mid)
        {
            var result = Submit("Member360", "GetMemVehicle", false, new object[] { mid });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 获取运行类型列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRunTypeList()
        {
            var result = Submit("Service", "GetRunTypeList", false);
            return Json(result.Obj[0].ToString());
        }
        #endregion

        #region 条目管理
        public ActionResult Item()
        {
            return View();
        }

        /// <summary>
        /// 获取条目数据
        /// </summary>
        /// <param name="itemCode"></param>
        /// <param name="itemName"></param>
        /// <param name="itemClass"></param>
        /// <returns></returns>
        public JsonResult GetItemData(long? itemCode, string itemName, string itemClass, bool itemEnable)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetItemData", false, new object[] { itemCode, itemName, itemClass, itemEnable, Auth.DataGroupID, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 获取条目类型
        /// </summary>
        /// <returns></returns>
        public JsonResult GetItemTypeList()
        {
            string optType = "ItemType";
            var result = Submit("Service", "GetOptionDataList", false, new object[] { optType });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 新增条目
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemName"></param>
        /// <param name="itemType"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public JsonResult AddItemData(long? itemId, string itemName, string itemType, string remark, bool enable)
        {
            var dataGroupID = (int)Auth.DataGroupID;
            itemName = HttpUtility.HtmlDecode(itemName);
            remark = HttpUtility.HtmlDecode(remark);
            var result = Submit("BaseData", "AddItemData", false, new object[] { itemId, itemName, itemType, remark, enable, dataGroupID });
            return Json(result);
        }
        /// <summary>
        /// 根据id获取单挑条目信息
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public JsonResult GetItemById(long itemId)
        {
            var result = Submit("BaseData", "GetItemById", false, new object[] { itemId });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 修改条目
        /// </summary>
        /// <param name="remark"></param>
        /// <returns></returns>
        public JsonResult UpdateItemData(ItemModel model)
        {

            model.ItemDesc = HttpUtility.HtmlDecode(model.ItemDesc);
            var result = Submit("BaseData", "UpdateItemData", false, new object[] { model });
            return Json(result);
        }

        /// <summary>
        /// 删除条目
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public JsonResult DeleteItemById(long itemId)
        {
            var result = Submit("BaseData", "DeleteItemById", false, new object[] { itemId });
            return Json(result);
        }
        /// <summary>
        /// 激活条目
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public JsonResult ActiveItemById(long itemId)
        {
            var result = Submit("BaseData", "ActiveItemById", false, new object[] { itemId });
            return Json(result);
        }
        /// <summary>
        /// 禁用条目
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public JsonResult InActiveItemById(long itemId)
        {
            var result = Submit("BaseData", "InActiveItemById", false, new object[] { itemId });
            return Json(result);
        }
        #endregion

        #region 门店管理
        public ActionResult Store()
        {
            return View();
        }
        /// <summary>
        /// 获取门店数据
        /// </summary>
        /// <param name="storeCode"></param>
        /// <param name="storeName"></param>
        /// <returns></returns>
        public JsonResult GetStoreData(string storeCode, string storeName, int? datagroupId)
        {
            var dts = Request.CreateDataTableParameter();
            var groupId = Auth.DataGroupID;
            var result = Submit("BaseData", "GetStoreData", false, new object[] { storeCode, storeName, datagroupId, groupId, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 增加门店信息
        /// </summary>
        /// <param name="storeCode"></param>
        /// <param name="storeName"></param>
        /// <param name="storeClass"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public JsonResult AddStoreData(string storeCode, string storeName, int storeDataGroup, string address, string storeBrand, string storeCodeSale, string printer, string code, string storeFullName)
        {
            var dataGroupID = (int)Auth.DataGroupID;
            storeFullName = HttpUtility.HtmlDecode(storeFullName);
            storeName = HttpUtility.HtmlDecode(storeName);
            address = HttpUtility.HtmlDecode(address);
            printer = HttpUtility.HtmlDecode(printer);
            var result = Submit("BaseData", "AddStoreData", false, new object[] { storeCode, storeName, storeDataGroup, address, storeBrand, storeCodeSale, printer, dataGroupID, code, storeFullName });
            return Json(result);
        }
        /// <summary>
        /// 根据Id获取门店信息
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public JsonResult GetStoreById(long storeId)
        {
            var result = Submit("BaseData", "GetStoreById", false, new object[] { storeId });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 根据id删除门店信息
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public JsonResult DeleteStoreById(long storeId)
        {
            var result = Submit("BaseData", "DeleteStoreById", false, new object[] { storeId });
            return Json(result);
        }
        /// <summary>
        /// 修改门店信息
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public JsonResult UpdateStoreData(StoreModel model)
        {
            model.StoreFullName = HttpUtility.HtmlDecode(model.StoreFullName);
            model.StoreName = HttpUtility.HtmlDecode(model.StoreName);
            model.StoreAddress = HttpUtility.HtmlDecode(model.StoreAddress);

            model.Printer = HttpUtility.HtmlDecode(model.Printer);
            var result = Submit("BaseData", "UpdateStoreData", false, new object[] { model });
            return Json(result);
        }
        /// <summary>
        /// 获取门店所属群组列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetStroeGroupList()
        {
            var dataGroupID = (int)Auth.DataGroupID;
            var result = Submit("BaseData", "GetStroeGroupList", false, new object[] { dataGroupID });
            return Json(result.Obj[0].ToString());
        }
        #endregion

        #region 品牌维护
        public ActionResult Brand()
        {
            return View();
        }
        /// <summary>
        /// 获取车辆品牌列表（分页）
        /// </summary>
        /// <param name="brandName"></param>
        /// <returns></returns>
        public JsonResult GetBrandData(string brandName, string groupId)
        {
            int? igroupId = 0;
            var dts = Request.CreateDataTableParameter();
            if (string.IsNullOrEmpty(groupId))
                igroupId = Auth.DataGroupID;
            else
                igroupId = Convert.ToInt32(groupId);
            var result = Submit("BaseData", "GetBrandData", false, new object[] { brandName, igroupId, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 添加品牌信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult AddBrandData(Brand model)
        {
            //model.DataGroupID = (int)Auth.DataGroupID;
            var result = Submit("BaseData", "AddBrandData", false, new object[] { model });
            return Json(result);
        }
        /// <summary>
        /// 更新品牌信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult UpdateBrandData(Brand model)
        {
            model.BrandName = HttpUtility.HtmlDecode(model.BrandName);
            var result = Submit("BaseData", "UpdateBrandData", false, new object[] { model });
            return Json(result);
        }
        /// <summary>
        /// 根据id获取品牌信息
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public JsonResult GetBrandById(long brandId)
        {
            var result = Submit("BaseData", "GetBrandById", false, new object[] { brandId });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 根据id删除品牌信息
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public JsonResult DeleteBrandById(long brandId)
        {
            var result = Submit("BaseData", "DeleteBrandById", false, new object[] { brandId });
            return Json(result);
        }
        /// <summary>
        /// 控制是否启用
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="currentEnable"></param>
        /// <returns></returns>
        public JsonResult EnableBrandById(long brandId, string currentEnable)
        {
            var result = Submit("BaseData", "EnableBrandById", false, new object[] { brandId, currentEnable });
            return Json(result);
        }

        #endregion

        #region 区域维护
        public ActionResult Area()
        {
            return View();
        }

        /// <summary>
        /// 获取车辆区域列表（分页）
        /// </summary>
        /// <param name="areaNameBase"></param>
        /// <returns></returns>
        public JsonResult GetAreaData(string areaNameBase, string groupId)
        {
            int? igroupId = 0;
            var dts = Request.CreateDataTableParameter();
            if (string.IsNullOrEmpty(groupId))
                igroupId = Auth.DataGroupID;
            else
                igroupId = Convert.ToInt32(groupId);
            var result = Submit("BaseData", "GetAreaData", false, new object[] { areaNameBase, igroupId, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 添加区域信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult AddAreaData(Area model)
        {
            //model.DataGroupID = (int)Auth.DataGroupID;
            var result = Submit("BaseData", "AddAreaData", false, new object[] { model });
            return Json(result);
        }
        /// <summary>
        /// 更新区域信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult UpdateAreaData(Area model)
        {
            model.AreaNameBase = HttpUtility.HtmlDecode(model.AreaNameBase);
            var result = Submit("BaseData", "UpdateAreaData", false, new object[] { model });
            return Json(result);
        }
        /// <summary>
        /// 根据id获取区域信息
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public JsonResult GetAreaById(long areaId)
        {
            var result = Submit("BaseData", "GetAreaById", false, new object[] { areaId });
            return Json(result.Obj[0].ToString());
        }
        #endregion

        //#region 车辆车型维护
        public ActionResult VehicleType()
        {
            return View();
        }
        ///// <summary>
        ///// 获取车辆车型列表（分页）
        ///// </summary>
        ///// <param name="typeName"></param>
        ///// <returns></returns>
        //public JsonResult GetVehicleTypeData(string typeName, string brandId)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var groupId = Auth.DataGroupID;
        //    var result = Submit("BaseData", "GetVehicleTypeData", false, new object[] { typeName, groupId, brandId, JsonHelper.Serialize(dts) });
        //    return Json(result.Obj[0].ToString());
        //}

        ///// <summary>
        ///// 根据id获取车型信息
        ///// </summary>
        ///// <param name="typeId"></param>
        ///// <returns></returns>
        //public JsonResult GetVehicleTypeById(long typeId)
        //{
        //    var result = Submit("BaseData", "GetVehicleTypeById", false, new object[] { typeId });
        //    return Json(result.Obj[0].ToString());
        //}
        ///// <summary>
        ///// 根据id删除车型信息
        ///// </summary>
        ///// <param name="typeId"></param>
        ///// <returns></returns>
        //public JsonResult DeleteVehicleTypeById(long typeId)
        //{
        //    var result = Submit("BaseData", "DeleteVehicleTypeById", false, new object[] { typeId });
        //    return Json(result);
        //}
        //#endregion

        //#region 车辆车系维护
        public ActionResult VehicleSeries()
        {
            return View();
        }
        ///// <summary>
        ///// 获取车辆车型列表（分页）
        ///// </summary>
        ///// <param name="SeriesName"></param>
        ///// <returns></returns>
        //public JsonResult GetVehicleSeriesData(string seriesName, string brandId, string typeId)
        //{
        //    var dts = Request.CreateDataTableParameter();
        //    var groupId = Auth.DataGroupID;
        //    var result = Submit("BaseData", "GetVehicleSeriesData", false, new object[] { seriesName, groupId, brandId, typeId, JsonHelper.Serialize(dts) });
        //    return Json(result.Obj[0].ToString());
        //}

        ///// <summary>
        ///// 根据id获取车型信息
        ///// </summary>
        ///// <param name="seriesId"></param>
        ///// <returns></returns>
        //public JsonResult GetVehicleSeriesById(long seriesId)
        //{
        //    var result = Submit("BaseData", "GetVehicleSeriesById", false, new object[] { seriesId });
        //    return Json(result.Obj[0].ToString());
        //}
        ///// <summary>
        ///// 根据id删除车型信息
        ///// </summary>
        ///// <param name="seriesId"></param>
        ///// <returns></returns>
        //public JsonResult DeleteVehicleSeriesById(long seriesId)
        //{
        //    var result = Submit("BaseData", "DeleteVehicleSeriesById", false, new object[] { seriesId });
        //    return Json(result);
        //}

        //#endregion

        #region 车款维护
        public ActionResult Vehicle()
        {
            return View();
        }
        /// <summary>
        /// 获取车辆管理数据
        /// </summary>
        /// <param name="vehicleBrand"></param>
        /// <param name="vehicleSeries"></param>
        /// <param name="vehicleType"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public JsonResult GetVehicleData(string vehicleBrand, string vehicleSeries, string vehicleType)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetVehicleData", false, new object[] { vehicleBrand, vehicleSeries, vehicleType, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }



        #endregion

        #region 车辆基本信息维护（品牌、车系、车型）
        public ActionResult VehicleInfo()
        {
            return View();
        }
        /// <summary>
        /// 加载车辆品牌信息
        /// </summary>
        /// <param name="brandName"></param>
        /// <returns></returns>
        public JsonResult GetVehicleBrandInfo(string brandName)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetVehicleBrandInfo", false, new object[] { brandName, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 加载车辆车系信息
        /// </summary>
        /// <param name="vehicleBrand"></param>
        /// <param name="seriesName"></param>
        /// <returns></returns>
        public JsonResult GetVehicleSeriesInfo(string vehicleBrand, string seriesName)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetVehicleSeriesInfo", false, new object[] { vehicleBrand, seriesName, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 加载车型信息
        /// </summary>
        /// <param name="vehicleBrand"></param>
        /// <param name="vehicleSeries"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public JsonResult GetVehicleTypeInfo(string vehicleBrand, string vehicleSeries, string typeName)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetVehicleTypeInfo", false, new object[] { vehicleBrand, vehicleSeries, typeName, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 获取车辆品牌列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetVehicleBrandList()
        {
            var result = Submit("BaseData", "GetVehicleBrandList", false, null);
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 获取车系列表
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public JsonResult GetVehicleSeriesList(string brandId)
        {
            var result = Submit("BaseData", "GetVehicleSeriesList", false, new object[] { brandId });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 获取统计车系列表
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public JsonResult GetVehicleSiblingList(string code)
        {
            var result = Submit("BaseData", "GetVehicleSiblingList", false, new object[] { code });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 获取车型列表
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public JsonResult GetVehicleLevelList(string typeId)
        {
            var result = Submit("BaseData", "GetVehicleLevelList", false, new object[] { typeId });
            return Json(result.Obj[0].ToString());
        }


        /// <summary>
        /// 根据id删除车型信息
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public JsonResult DeleteVehicleById(int vehicleId)
        {
            var result = Submit("BaseData", "DeleteVehicleById", false, new object[] { vehicleId });
            return Json(result);
        }

        /// <summary>
        /// 添加品牌信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult AddVehicleData(Vehicle vehicle)
        {
            var result = Submit("BaseData", "AddVehicleData", false, new object[] { vehicle });
            return Json(result);
        }
        /// <summary>
        /// 更新品牌信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult UpdateVehicleData(Vehicle vehicle)
        {
            var result = Submit("BaseData", "UpdateVehicleData", false, new object[] { vehicle });
            return Json(result);
        }
        /// <summary>
        /// 根据id获取车型信息
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public JsonResult GetVehicleById(int vehicleId)
        {
            var result = Submit("BaseData", "GetVehicleById", false, new object[] { vehicleId });
            return Json(result.Obj[0].ToString());
        }
        #endregion

        #region 企业信息管理
        public ActionResult Corp()
        {
            return View();
        }
        /// <summary>
        /// 获取企业信息（分页）
        /// </summary>
        /// <param name="corpName"></param>
        /// <returns></returns>
        public JsonResult GetCorpData(string corpName)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetCorpData", false, new object[] { corpName, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 增加企业信息
        /// </summary>
        /// <param name="corpName"></param>
        /// <param name="corpContract"></param>
        /// <param name="corpPhone"></param>
        /// <param name="corpAddress"></param>
        /// <returns></returns>
        public JsonResult AddCorpData(CorpModel model)
        {
            model.DataGroupID = Auth.DataGroupID;
            model.CorpName = HttpUtility.HtmlDecode(model.CorpName);
            model.CorpAddress = HttpUtility.HtmlDecode(model.CorpAddress);
            var result = Submit("BaseData", "AddCorpData", false, new object[] { model });
            return Json(result);
        }
        /// <summary>
        /// 编辑企业信息
        /// </summary>
        /// <param name="corpId"></param>
        /// <param name="corpName"></param>
        /// <param name="corpContract"></param>
        /// <param name="corpPhone"></param>
        /// <param name="corpAddress"></param>
        /// <returns></returns>
        public JsonResult UpdateCorpData(CorpModel model)
        {
            model.CorpName = HttpUtility.HtmlDecode(model.CorpName);
            model.CorpAddress = HttpUtility.HtmlDecode(model.CorpAddress);
            var result = Submit("BaseData", "UpdateCorpData", false, new object[] { model });
            return Json(result);
        }
        /// <summary>
        /// 根据ID获取企业信息
        /// </summary>
        /// <param name="corpId"></param>
        /// <returns></returns>
        public JsonResult GetCorpById(long corpId)
        {
            var result = Submit("BaseData", "GetCorpById", false, new object[] { corpId });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 根据ID删除企业信息
        /// </summary>
        /// <param name="corpId"></param>
        /// <returns></returns>
        public JsonResult DeleteCorpById(long corpId)
        {
            var result = Submit("BaseData", "DeleteCorpById", false, new object[] { corpId });
            return Json(result);
        }
        #endregion

        #region 门店设定
        public ActionResult StoreSetting()
        {
            return View();
        }
        /// <summary>
        /// 获取门店设定数据列表（分页）
        /// </summary>
        /// <param name="storeCode"></param>
        /// <returns></returns>
        public JsonResult GetStoreSettingData(string storeCode)
        {
            var dts = Request.CreateDataTableParameter();
            var groupId = Auth.DataGroupID;
            var result = Submit("BaseData", "GetStoreSettingData", false, new object[] { storeCode, groupId, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 添加门店设定信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult AddStoreSettingData(StoreSetting model)
        {
            model.DataGroupID = (int)Auth.DataGroupID;
            var result = Submit("BaseData", "AddStoreSettingData", false, new object[] { model });
            return Json(result);
        }
        /// <summary>
        /// 更新门店设定信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult UpdateStoreSettingData(StoreSetting model)
        {
            var result = Submit("BaseData", "UpdateStoreSettingData", false, new object[] { model });
            return Json(result);
        }
        /// <summary>
        /// 根据id获取门店设定信息
        /// </summary>
        /// <param name="settingId"></param>
        /// <returns></returns>
        public JsonResult GetStoreSettingById(long settingId)
        {
            var result = Submit("BaseData", "GetStoreSettingById", false, new object[] { settingId });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 根据id删除门店设定信息
        /// </summary>
        /// <param name="settingId"></param>
        /// <returns></returns>
        public JsonResult DeleteStoreSettingById(long settingId)
        {
            var result = Submit("BaseData", "DeleteStoreSettingById", false, new object[] { settingId });
            return Json(result);
        }
        /// <summary>
        /// 获取门店列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetStoreList()
        {
            var dataGroupID = (int)Auth.DataGroupID;
            var result = Submit("BaseData", "GetStoreList", false, new object[] { dataGroupID });
            return Json(result.Obj[0].ToString());
        }
        #endregion

        #region 套餐管理
        public ActionResult Package()
        {
            return View();
        }
        /// <summary>
        /// 获取套餐列表
        /// </summary>
        /// <param name="pName"></param>
        /// <returns></returns>
        public JsonResult GetPackageList(string pName, string enable)
        {
            var dts = Request.CreateDataTableParameter();
            var groupId = Auth.DataGroupID;
            var result = Submit("BaseData", "GetPackageList", false, new object[] { pName, enable, groupId, Auth.UserID, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 增加套餐信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult AddPackageData(Package model, List<ActLimit> limit)
        {
            model.PackageName = HttpUtility.HtmlDecode(model.PackageName);
            model.PackageDesc = HttpUtility.HtmlDecode(model.PackageDesc);
            model.DataGroupID = (int)Auth.DataGroupID;
            model.AddedUser = Auth.UserID.ToString();
            var result = Submit("BaseData", "AddPackageData", false, new object[] { model, JsonHelper.Serialize(limit) });
            return Json(result);
        }
        /// <summary>
        /// 更新套餐信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult UpdatePackageData(Package model, List<ActLimit> limit)
        {
            model.PackageName = HttpUtility.HtmlDecode(model.PackageName);
            model.PackageDesc = HttpUtility.HtmlDecode(model.PackageDesc);
            model.ModifiedUser = Auth.UserID.ToString();
            var result = Submit("BaseData", "UpdatePackageData", false, new object[] { model, JsonHelper.Serialize(limit) });
            return Json(result);
        }
        /// <summary>
        /// 根据id获取单条套餐信息
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public JsonResult GetPackageById(int? packageId)
        {
            var result = Submit("BaseData", "GetPackageById", false, new object[] { packageId });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 根据id删除套餐信息
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public JsonResult DeletePackageById(int packageId)
        {
            var result = Submit("BaseData", "DeletePackageById", false, new object[] { packageId });
            return Json(result);
        }
        /// <summary>
        /// 获取单位列表
        /// </summary>
        /// <param name="optType"></param>
        /// <returns></returns>
        public JsonResult GetUnitList(string optType)
        {
            var result = Submit("Service", "GetOptionDataList", false, new object[] { optType });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 获取某套餐明细列表
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public JsonResult GetPackageDetailList(int packageId)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetPackageDetailList", false, new object[] { packageId, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        public JsonResult GetPackageDetailList1(int packageId)
        {
            var result = Submit("BaseData", "GetPackageDetailList1", false, new object[] { packageId });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 根据id获取套餐明细信息
        /// </summary>
        /// <param name="packageDetailId"></param>
        /// <returns></returns>
        public JsonResult GetPackageDetailById(int packageDetailId)
        {
            var result = Submit("BaseData", "GetPackageDetailById", false, new object[] { packageDetailId });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 根据id删除套餐信息
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public JsonResult DeletePackageDetailById(int packageDetailId)
        {
            var result = Submit("BaseData", "DeletePackageDetailById", false, new object[] { packageDetailId });
            return Json(result);
        }
        /// <summary>
        /// 添加套餐明细信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public JsonResult AddPackageDetailData(PackageDetail model)
        {
            model.ItemDesc = HttpUtility.HtmlDecode(model.ItemDesc);
            var result = Submit("BaseData", "AddPackageDetailData", false, new object[] { model });
            return Json(result);
        }
        /// <summary>
        /// 修改套餐明细信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public JsonResult UpdatePackageDetailData(PackageDetail model)
        {
            model.ItemDesc = HttpUtility.HtmlDecode(model.ItemDesc);
            var result = Submit("BaseData", "UpdatePackageDetailData", false, new object[] { model });
            return Json(result);
        }
        /// <summary>
        /// 获取套餐条目列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPackageItemList()
        {
            string optType = "package";
            int dataGroupId = (int)Auth.DataGroupID;
            var result = Submit("TmpCommunication", "GetItemList", false, new object[] { dataGroupId, optType });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 激活套餐
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public JsonResult ActivePackageById(int packageId)
        {
            var result = Submit("BaseData", "ActivePackageById", false, new object[] { packageId });
            return Json(result);
        }
        /// <summary>
        /// 禁用套餐
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public JsonResult InActivePackageById(int packageId)
        {
            var result = Submit("BaseData", "InActivePackageById", false, new object[] { packageId });
            return Json(result);
        }
        /// <summary>
        /// 获取IDOS套餐列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetIDOSPackageList(string[] stores)
        {
            var groupId = Auth.DataGroupID;
            var result = Submit("BaseData", "GetIDOSPackageList", false, new object[] { groupId, Auth.UserID, JsonHelper.Serialize(stores) });
            return Json(result.Obj[0].ToString());
        }
        #endregion

        #region 公共优惠券
        public ActionResult PublicCoupon()
        {
            return View();
        }


        /// <summary>
        /// 增加门店信息
        /// </summary>
        /// <param name="storeCode"></param>
        /// <param name="storeName"></param>
        /// <param name="storeClass"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public JsonResult AddPublicCouponData(string prefix, string couponLength, string couponName, string couponCounts, int templetID)
        {
            var dataGroupID = (int)Auth.DataGroupID;
            couponCounts = HttpUtility.HtmlDecode(couponCounts);
            couponName = HttpUtility.HtmlDecode(couponName);
            prefix = HttpUtility.HtmlDecode(prefix);
            couponLength = HttpUtility.HtmlDecode(couponLength);
            DateTime dt = DateTime.Now;
            var result = Submit("BaseData", "AddPublicCouponData", false, new object[] { prefix, couponLength, couponName, couponCounts, dt, templetID });
            return Json(result);
        }

        /// <summary>
        /// 获取优惠券列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCouponList()
        {
            int dataGroupId = (int)Auth.DataGroupID;
            var result = Submit("BaseData", "GetCouponList", false, new object[] { dataGroupId, Auth.UserID });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 获取不记名优惠券数据
        /// </summary>
        /// <param name="storeCode"></param>
        /// <param name="storeName"></param>
        /// <returns></returns>
        public JsonResult GetCouponData(string batchNo, string templetID, string status, string templetName)
        {
            var dts = Request.CreateDataTableParameter();
            var dataGroupID = Auth.DataGroupID;
            var result = Submit("BaseData", "GetCouponData", false, new object[] { JsonHelper.Serialize(dts), dataGroupID, batchNo, templetID, templetName, status, Auth.UserID });
            return Json(result.Obj[0].ToString());
        }


        /// <summary>
        /// 获取不记名优惠券详细数据
        /// </summary>
        /// <param name="storeCode"></param>
        /// <param name="storeName"></param>
        /// <returns></returns>
        public JsonResult GetCouponDetailData(string batchNo)
        {
            var dts = Request.CreateDataTableParameter();
            var dataGroupID = Auth.DataGroupID;
            var result = Submit("BaseData", "GetCouponDetailData", false, new object[] { JsonHelper.Serialize(dts), batchNo, dataGroupID });
            return Json(result.Obj[0].ToString());
        }

        [HttpPost]
        public FileResult ExportPublicCoupon(string exprBatchNo)
        {
            List<ExcelColumnFormat<PublicCoupon>> mainSheetFormats = new List<ExcelColumnFormat<PublicCoupon>>{
                    //new ExcelColumnFormat<PublicCoupon>{ColumnName="券编号", FuncGetValue=p=>p.CouponCode},
                    new ExcelColumnFormat<PublicCoupon>{ColumnName="券代码", FuncGetValue=p=>p.CouponCode},
                    new ExcelColumnFormat<PublicCoupon>{ColumnName="手机号码", FuncGetValue=p=>p.Mobile},
                    new ExcelColumnFormat<PublicCoupon>{ColumnName="使用状态", FuncGetValue=p=>p.IsUsed==true?"已用":"未用"}, 
                    new ExcelColumnFormat<PublicCoupon>{ColumnName="所属模板", FuncGetValue=p=>p.TempletName}
            };
            try
            {
                var result = Submit("BaseData", "ExportPublicCoupon", false, new object[] { exprBatchNo, (int)Auth.DataGroupID });


                if (result.IsPass == false)
                {
                    List<PublicCoupon> list = new List<PublicCoupon>();
                    var workBook = ExcelHelper.DataToExcel(list, mainSheetFormats);
                    var stream = ExcelHelper.GetStream(workBook);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(stream, "application/vnd.ms-excel", result.MSG + DateTime.Now.ToShortDateString() + ".xls");
                }
                else
                {
                    List<PublicCoupon> list = JsonHelper.Deserialize<List<PublicCoupon>>(result.Obj[0].ToString());
                    HSSFWorkbook workBook = ExcelHelper.DataToExcel(list, mainSheetFormats, null, null);
                    var stream = ExcelHelper.GetStream(workBook);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(stream, "application/vnd.ms-excel", "公共优惠券导出" + DateTime.Now.ToShortDateString() + ".xls");
                }
            }
            catch (Exception e)
            {
                MemoryStream strm = new MemoryStream();
                return File(strm, "application/vnd.ms-excel", "公共优惠券导出异常_" + e.Message + DateTime.Now.ToShortDateString() + ".xls");
            }

        }

        //[HttpPost]
        //public int Forbidden(string exprBatchNo, bool status) 
        //{
        //    var result = Submit("BaseData", "ForbiddenCoupon", false, new object[] { exprBatchNo, status });
        //    return 0;
        //}

        /// <summary>
        /// 激活规则
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public JsonResult ActiveCoupon(string batchNo)
        {
            var result = Submit("BaseData", "ActiveCoupon", false, new object[] { batchNo });
            return Json(result);
        }
        /// <summary>
        /// 禁用规则
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public JsonResult InActiveCoupon(string batchNo)
        {
            var result = Submit("BaseData", "InActiveCoupon", false, new object[] { batchNo });
            return Json(result);
        }



        #endregion

        #region 异业券管理

        public ActionResult DiscrepantIndustryCoupon()
        {
            return View();
        }

        /// <summary>
        /// 获取异业优惠券模板列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDiscrepantIndustryCouponTemplet()
        {
            var result = Submit("BaseData", "GetDiscrepantIndustryCouponTemplet", false, new object[] { Auth.DataGroupID });
            return Json(result.Obj[0].ToString());
        }

        public JsonResult GetDiscrepantIndustryCouponList(string batchNo, string templetID)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetDiscrepantIndustryCoupon", false, new object[] { batchNo, templetID, Auth.DataGroupID, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        [HttpPost]
        public string UploadDiscrepantIndustryCoupon()
        {
            try
            {
                var file = Request.Files[0];
                int FileLen = file.ContentLength;
                byte[] input = new byte[FileLen];

                System.IO.Stream MyStream = file.InputStream;
                MyStream.Read(input, 0, FileLen);
                DataTable dt = ExcelHelper.ExcelToDataTable(MyStream);

                List<string> couponList = new List<string>();
                foreach (DataRow r in dt.Rows)
                {
                    if (r["异业券编号"] != null)
                    {
                        if (!string.IsNullOrEmpty(r["异业券编号"].ToString()))
                        {
                            couponList.Add(r["异业券编号"].ToString().Trim());
                        }
                    }
                }
                if (couponList != null && couponList.Count > 0)
                {
                    return JsonHelper.Serialize(couponList);
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public JsonResult BatchInsertDiscrepantIndustryCouponPool(string templet, string couponList)
        {
            try
            {
                var result = Submit("BaseData", "BatchInsertDiscrepantIndustryCouponPool", false, new object[] { templet, couponList });
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new Result(false, ex.Message));
            }
        }

        #endregion

        #region 维修类型维护
        public ActionResult RepairType()
        {
            return View();
        }
        /// <summary>
        /// 获取维修类型数据
        /// </summary>
        /// <param name="storeCode"></param>
        /// <returns></returns>
        public JsonResult GetRepairTypeData(string repairName, string storeCode)
        {
            var dts = Request.CreateDataTableParameter();
            var groupId = Auth.DataGroupID;
            var result = Submit("BaseData", "GetRepairTypeData", false, new object[] { repairName, storeCode, groupId, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 添加维修类型数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult AddRepairTypeData(RepairModel model)
        {
            model.RepairTypeName = HttpUtility.HtmlDecode(model.RepairTypeName);
            var result = Submit("BaseData", "AddRepairTypeData", false, new object[] { model });
            return Json(result);
        }
        /// <summary>
        /// 更新维修类型数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult UpdateRepairTypeData(RepairModel model)
        {
            var result = Submit("BaseData", "UpdateRepairTypeData", false, new object[] { model });
            return Json(result);
        }
        /// <summary>
        /// 根据id获取维修类型数据
        /// </summary>
        /// <param name="repairTypeId"></param>
        /// <returns></returns>
        public JsonResult GetRepairTypeById(int repairTypeId)
        {
            var result = Submit("BaseData", "GetRepairTypeById", false, new object[] { repairTypeId });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 根据id删除维修类型数据
        /// </summary>
        /// <param name="repairTypeId"></param>
        /// <returns></returns>
        public JsonResult DeleteRepairTypeById(int repairTypeId)
        {
            var result = Submit("BaseData", "DeleteRepairTypeById", false, new object[] { repairTypeId });
            return Json(result);
        }
        #endregion

        #region 获取公共数据

        public JsonResult GetBizOptionsByType(string optionType, bool? enable)
        {
            var result = Submit("Service", "GetBizOptionsByType", false, new object[] { optionType, enable });
            return Json(result.Obj[0].ToString());
        }

        public JsonResult GetDataLimitTypes(bool? enable)
        {
            var result = Submit("Service", "GetDataLimitTypes", false, new object[] { enable });
            return Json(result.Obj[0].ToString());
        }

        public JsonResult GetBizOptionsByTypeAndDataGroup(string optionType, int? dataGroupId, bool? enable)
        {
            var result = Submit("Service", "GetBizOptionsByTypeAndDataGroup", false, new object[] { optionType, dataGroupId, enable });
            return Json(result.Obj[0].ToString());
        }

        public JsonResult GetRegionByPID(int pid, int grade)
        {
            var info = Submit("Region", "GetRegionByPID", false, new object[] { pid, grade });
            return Json(info);
        }

        public JsonResult InputRegionByCertNo(int regionId)
        {
            var info = Submit("Region", "InputRegionByCertNo", false, new object[] { regionId });
            return Json(info);
        }

        #endregion

        #region kpi管理
        public ActionResult KPIManager()
        {
            return View();
        }

        public JsonResult GetKpiList(string KPIname, string KPItype)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetKpiList", false, new object[] { KPIname, KPItype, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        public JsonResult GetKPIById(int KPIId)
        {
            var result = Submit("BaseData", "GetKPIById", false, new object[] { KPIId });
            return Json(result.Obj[0].ToString());
        }

        public JsonResult DeleteKPIById(int KPIId)
        {
            var result = Submit("BaseData", "DeleteKPIById", false, new object[] { KPIId });
            return Json(result);
        }

        public JsonResult SetKPIEnable(int KPIId, bool Enable)
        {
            var result = Submit("BaseData", "SetKPIEnable", false, new object[] { KPIId, Enable });
            return Json(result);
        }

        public JsonResult AddorUpdateKPI(KPIModel model)
        {
            string user = Session[Utility.AppConst.SESSION_AUTH].ToString();
            model.ComputeScript = HttpUtility.HtmlDecode(model.ComputeScript);
            var result = Submit("BaseData", "AddorUpdateKPI", false, new object[] { model, user });
            return Json(result);
        }

        #endregion

        #region 门店维护
        public ActionResult StoreMaintenance()
        {
            //渠道 
            var chan = Submit("Service", "GetStoreChannels", false);
            ViewBag.Channels = chan.Obj[0];

            ////可用渠道 
            //var enableChannel = Submit("Service", "GetEnableStoreChannels", false);
            //ViewBag.EnableChannels = enableChannel.Obj[0];

            //大区
            var area = Submit("Service", "GetStoreArea", false);
            ViewBag.Areas = area.Obj[0];

            //省份
            var province = Submit("BaseData", "GetProvince", false);
            ViewBag.province = province.Obj[0];

            //品牌 
            var Brands = Submit("Service", "GetBrands", false);
            ViewBag.Brands = Brands.Obj[0];

            return View();
        }

        /// <summary>
        ///  GetStoreData
        /// </summary>
        /// <param name="StoreName"></param>
        /// <param name="ChannelCodeStore"></param>
        /// <param name="AddressStore"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetStoreMaintenanceData(string StoreName, string ChannelCodeStore, string AddressStore, int? groupId,string storeCode)
        {
            int? igroupId = 0;
            var dts = Request.CreateDataTableParameter();
            if (string.IsNullOrEmpty(groupId.ToString()))
                igroupId = Auth.DataGroupID;
            else
                igroupId = Convert.ToInt32(groupId);
            var result = Submit("BaseData", "GetStoreMaintenanceData", false, new object[] { StoreName, ChannelCodeStore, AddressStore, igroupId, storeCode, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        [HttpPost]
        public JsonResult AddStoreMaintenanceData(StoreModel model)
        {
            var result = Submit("BaseData", "AddStoreMaintenanceData", false, new object[] { model });
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetStoreMaintenanceById(long storeId)
        {
            var result = Submit("BaseData", "GetStoreMaintenanceById", false, new object[] { storeId });
            return Json(result.Obj[0].ToString());
        }


        /// <summary>
        /// 更新门店信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateStoreMaintenanceData(StoreModel model)
        {
            var result = Submit("BaseData", "UpdateStoreMaintenanceData", false, new object[] { model });
            return Json(result);
        }

        /// <summary>
        /// 根据id删除门店信息
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteStoreMaintenanceById(long storeId)
        {
            var result = Submit("BaseData", "DeleteStoreMaintenanceById", false, new object[] { storeId });
            return Json(result);
        }

        /// <summary>
        /// 根据省份获取城市
        /// </summary>
        /// <param name="ParentRegionID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCityByProvince(string ProvinceCode)
        {

            var rst = Submit("BaseData", "GetCity", false, new object[] { ProvinceCode });
            return Json(rst.Obj[0].ToString());
        }

        [HttpPost]
        public FileResult StoreMaintenanceToExcel(string hideDrpStoreClass, string hideStoreName, string hideChannelCodeStore, string hideAddressStore, string hideStoreCode)
        {
            var result = Submit("BaseData", "StoreMaintenanceToExcel", false, new object[] { hideDrpStoreClass, hideStoreName, hideChannelCodeStore, hideAddressStore, hideStoreCode });
            var applyList = JsonHelper.Deserialize<List<StoreMaintenanceToExcel>>(result.Obj[0].ToString());
            List<ExcelColumnFormat<StoreMaintenanceToExcel>> excelColumnFormats = new List<ExcelColumnFormat<StoreMaintenanceToExcel>>{
                 new ExcelColumnFormat<StoreMaintenanceToExcel>{ColumnName="门店名称", FuncGetValue=p=>p.StoreName},                       
                   new ExcelColumnFormat<StoreMaintenanceToExcel>{ColumnName="门店地址", FuncGetValue=p=>p.StoreAddress},
                         new ExcelColumnFormat<StoreMaintenanceToExcel>{ColumnName="经销商", FuncGetValue=p=>p.ChannelNameStore},
                   new ExcelColumnFormat<StoreMaintenanceToExcel>{ColumnName="门店代码", FuncGetValue=p=>p.StoreCode},
                     new ExcelColumnFormat<StoreMaintenanceToExcel>{ColumnName="区域名称", FuncGetValue=p=>p.AreaNameStore},
                     new ExcelColumnFormat<StoreMaintenanceToExcel>{ColumnName="门店类型", FuncGetValue=p=>p.StoreType},              
            };
            var wordBook = ExcelHelper.DataToExcel(applyList, excelColumnFormats);
            var stream = ExcelHelper.GetStream(wordBook);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, "门店列表导出" + DateTime.Now.ToShortDateString() + ".xls");
        }
        #endregion

        #region 渠道管理
        public ActionResult Channel()
        {
            return View();
        }
        /// <summary>
        /// 获取渠道数据
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetChannelData(string channelName, int? groupId)
        {
            int? igroupId = 0;
            var dts = Request.CreateDataTableParameter();
            if (string.IsNullOrEmpty(groupId.ToString()))
                igroupId = Auth.DataGroupID;
            else
                igroupId = Convert.ToInt32(groupId);
            var result = Submit("BaseData", "GetChannelData", false, new object[] { channelName, igroupId, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 添加渠道数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddChannelData(ChannelModel model)
        {
            var result = Submit("BaseData", "AddChannelData", false, new object[] { model });
            return Json(result);
        }
        /// <summary>
        /// 修改渠道数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateChannelData(ChannelModel model)
        {
            var result = Submit("BaseData", "UpdateChannelData", false, new object[] { model });
            return Json(result);
        }
        /// <summary>
        /// 根据ID获取渠道数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetChannelById(long channelId)
        {
            var result = Submit("BaseData", "GetChannelById", false, new object[] { channelId });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 根据ID删除渠道
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public JsonResult DeleteChannelById(long channelId)
        {
            var result = Submit("BaseData", "DeleteChannelById", false, new object[] { channelId });
            return Json(result);
        }
        /// <summary>
        /// 禁用/启用渠道
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        public JsonResult UpdateChannelEnableById(long channelId, string isEnable)
        {
            var result = Submit("BaseData", "UpdateChannelEnableById", false, new object[] { channelId, isEnable });
            return Json(result);
        }
        #endregion

        public JsonResult GetActivityWorkflowTemplateList()
        {
            try
            {
                ProjectConfigElement config = (ProjectConfigElement)(WebConfigurationManager.GetSection("ProjectConfig"));
                if (config != null && config.Activity != null && !string.IsNullOrEmpty(config.Activity.WorkflowTemplateList))
                {
                    string[] list = config.Activity.WorkflowTemplateList.Split(',');
                    return Json(list);
                }
                return Json("");
            }
            catch (Exception ex)
            {
                return Json("");
            }
        }

        //public List<string> GetActivityWorkflowTemplateList()
        //{
        //    try
        //    {
        //        ProjectConfigElement config = (ProjectConfigElement)(WebConfigurationManager.GetSection("ProjectConfig"));
        //        if (config != null && config.Activity != null && !string.IsNullOrEmpty(config.Activity.WorkflowTemplateList))
        //        {
        //            string[] list = config.Activity.WorkflowTemplateList.Split(',');
        //            return Json(list);
        //        }
        //        return Json("");
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        #region  基础分类信息维护相关

        //管理员使用分页展示
        public JsonResult GetSysClassListForPage(int? classID, string className, string classType, int? dataGroupID, int? roleID, int? userID)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetSysClassForPage", false, new object[] { classID, className, classType, dataGroupID, roleID,userID,
                JsonHelper.Serialize(pageParamers) });
            return Json(result.Obj[0].ToString());
        }

        //个人账户使用分页展示
        public JsonResult GetSysClassForPage(int? classID, string className, string classType)
        {
            var pageParamers = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetSysClassForPage", false, new object[] { classID, className, classType,Auth.DataGroupID,null,Auth.UserID,
                JsonHelper.Serialize(pageParamers) });
            return Json(result.Obj[0].ToString());
        }

        //管理员使用不分页
        public JsonResult GetSysClassList(int? classID, string className, string classType, int? dataGroupID, int? roleID, int? userID)
        {
            var result = Submit("BaseData", "GetSysClass", false, new object[] { classID, className, classType, dataGroupID, roleID, userID });
            return Json(result.Obj[0].ToString());
        }

        //个人账户使用不分页
        public JsonResult GetSysClass(int? classID, string className, string classType)
        {
            var result = Submit("BaseData", "GetSysClass", false, new object[] { classID, className, classType, Auth.DataGroupID, null, Auth.UserID });
            return Json(result.Obj[0].ToString());
        }


        //个人账户新增基础分类
        public JsonResult InsertSysClass(string classInfo)
        {
            try
            {
                SysClassModel sysClass = JsonHelper.Deserialize<SysClassModel>(classInfo);
                sysClass.AddedUser = Auth.UserID.ToString();
                sysClass.DataGroupID = Auth.DataGroupID.Value;
                sysClass.UserID = Auth.UserID;
                var result = Submit("BaseData", "InsertSysClass", false, new object[] { JsonHelper.Serialize(sysClass) });
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new Result(false, ex.Message, null));
            }
        }

        //个人账户更新基础分类
        public JsonResult UpdateSysClass(string classInfo)
        {
            try
            {
                SysClassModel sysClass = JsonHelper.Deserialize<SysClassModel>(classInfo);
                sysClass.ModifiedUser = Auth.UserID.ToString();
                var result = Submit("BaseData", "UpdateSysClass", false, new object[] { JsonHelper.Serialize(sysClass) });
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new Result(false, ex.Message, null));
            }
        }


        #endregion

        public ActionResult GetCustomerLevel()
        {
            var result = Submit("BaseData", "GetCustomerLevel", false, new object[] { });
            return Json(result);
        }

        public ActionResult VehicleManage()
        {
            return View();
        }


        #region 业务部门管理
        public ActionResult BusinessDepartment()
        {
            return View();
        }

        /// <summary>
        /// 根据主键ID删除单条信息
        /// </summary>
        /// <param name="dataGroupID"></param>
        /// <returns></returns>
        public JsonResult DeleteBsiDptDataById(int tmpId)
        {
            var result = Submit("BaseData", "DeleteBsiDptDataById", false, new object[] { tmpId });
            return Json(result);
        }

        /// <summary>
        /// 增加或者更新信息
        /// </summary>
        /// <param name="templet"></param>
        /// <returns></returns>
        public JsonResult AddOrUpdateBsiDptData(BsiDpt bisdpt)
        {
            var result = Submit("BaseData", "AddOrUpdateBsiDptData", false, new object[] { bisdpt });
            return Json(result);
        }

        /// <summary>
        /// 加载沟通模板树图
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public JsonResult GetBsiDptDataList(string key)
        {
            key = HttpUtility.HtmlDecode(key);
            var result = Submit("BaseData", "GetBsiDptDataList", false, new object[] { key });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 根据主键ID获取单条沟通模板信息
        /// </summary>
        /// <param name="dataGroupID"></param>
        /// <returns></returns>
        public JsonResult GetBsiDptDataById(int tmpId)
        {
            var result = Submit("BaseData", "GetBsiDptDataById", false, new object[] { tmpId });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 获取群组等级列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBsiDptGrade()
        {
            var result = Submit("BaseData", "GetBsiDptGrade", false);
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 获取所属父群组列表
        /// </summary>
        /// <param name="dataGroupId"></param>
        /// <returns></returns>
        public JsonResult GetParentBsiDpt()
        {
            var result = Submit("BaseData", "GetParentBsiDpt", false);
            return Json(result.Obj[0].ToString());
        }

        //根据账号id获取业务部门
        public JsonResult LoadDepartmentByUser()
        {
            int userID = Auth.UserID;
            var result = Submit("BaseData", "LoadDepartmentByUser", false, new object[] { userID });
            return Json(result);
        }



        #endregion



        #region 业务类型

        public ActionResult BusinessType()
        {
            return View();
        }

        public ActionResult GetTypeAList()
        {
            var result = Submit("BaseData", "GetTypeAList", false, new object[] { });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 加载大类信息
        /// </summary>
        /// <param name="brandName"></param>
        /// <returns></returns>
        public JsonResult GetBusinessType(string typeName, string groupId)
        {
            int? igroupId = 0;
            var dts = Request.CreateDataTableParameter();
            if (string.IsNullOrEmpty(groupId))
                igroupId = Auth.DataGroupID;
            else
                igroupId = Convert.ToInt32(groupId);


            var result = Submit("BaseData", "GetBusinessType", false, new object[] { typeName, igroupId, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 根据id获取大类信息
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public JsonResult GetTypeById(int Id)
        {
            var result = Submit("BaseData", "GetTypeById", false, new object[] { Id });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 添加大类信息
        /// </summary>
        /// <param name="TypeCode"></param>
        /// <param name="TypeName"></param>
        /// <returns></returns>
        public JsonResult AddType(string TypeCode, string TypeName, int GroupId)
        {
            var result = Submit("BaseData", "AddType", false, new object[] { TypeCode, TypeName, GroupId });
            return Json(result);
        }


        public JsonResult UpdateType(string TypeCode, string TypeName, int GroupId, int BaseDataId)
        {
            var result = Submit("BaseData", "UpdateType", false, new object[] { TypeCode, TypeName, GroupId, BaseDataId });
            return Json(result);
        }


        public JsonResult DeleteType(int BaseDataId)
        {
            var result = Submit("BaseData", "DeleteType", false, new object[] { BaseDataId });
            return Json(result);
        }


        #endregion

        #region 业务子类型

        public ActionResult BusinessSubType()
        {
            return View();
        }

        public ActionResult GetTypeBList(string typeA)
        {
            var result = Submit("BaseData", "GetTypeBList", false, new object[] { typeA });
            return Json(result.Obj[0].ToString());
        }
        public ActionResult GetTypeBListAccount()
        {
            var result = Submit("BaseData", "GetTypeAListEnabled", false, new object[] { });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 加载小类信息
        /// </summary>
        /// <param name="brandName"></param>
        /// <returns></returns>
        public JsonResult GetBusinessSubType(string subTypeName, string typeName)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetBusinessSubType", false, new object[] { subTypeName, typeName, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        public JsonResult GetSubTypeById(int Id)
        {
            var result = Submit("BaseData", "GetSubTypeById", false, new object[] { Id });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 添加大类信息
        /// </summary>
        /// <param name="TypeCode"></param>
        /// <param name="TypeName"></param>
        /// <returns></returns>
        public JsonResult AddSubType(string TypeCode, string SubTypeCode, string SubTypeName)
        {
            var result = Submit("BaseData", "AddSubType", false, new object[] { TypeCode, SubTypeCode, SubTypeName });
            return Json(result);
        }


        public JsonResult UpdateSubType(string TypeCode, string SubTypeCode, string SubTypeName, int BaseDataId)
        {
            var result = Submit("BaseData", "UpdateSubType", false, new object[] { TypeCode, SubTypeCode, SubTypeName, BaseDataId });
            return Json(result);
        }


        public JsonResult DeleteSubType(int BaseDataId)
        {
            var result = Submit("BaseData", "DeleteSubType", false, new object[] { BaseDataId });
            return Json(result);
        }


        #endregion


        #region 参数配置

        public ActionResult Option()
        {
            return View();
        }

        public ActionResult GetSysOptionTypes()
        {
            var result = Submit("BaseData", "GetSysOptionTypes", false, new object[] { });
            return Json(result);
        }

        public ActionResult GetSysOptionValues(string type, string type2)
        {
            var result = Submit("BaseData", "GetSysOptionValues", false, new object[] { type, type2 });
            return Json(result);
        }

        public ActionResult SaveSysOption(SysOption model)
        {
            string user = Session[AppConst.SESSION_AUTH].ToString();
            var result = Submit("BaseData", "SaveSysOption", false, new object[] { model, user });
            return Json(result);
        }

        public ActionResult DeleteSysOption(int optionId)
        {
            var result = Submit("BaseData", "DeleteSysOption", false, new object[] { optionId });
            return Json(result);
        }

        public ActionResult GetOptionData(string optionType, string optionText)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetOptionData", false, new object[] { optionType, optionText, JsonHelper.Serialize(dts) }).Obj[0];
            return Json(result);
        }

        public ActionResult GetOptionById(int optionId)
        {
            var result = Submit("BaseData", "GetOptionById", false, new object[] { optionId });
            return Json(result);
        }

        #endregion
        #region 价值度模型

        public ActionResult DegreeModel()
        {
            return View();
        }

        public ActionResult GetAliasDegree()
        {
            var result = Submit("BaseData", "GetAliasDegree", false, new object[] { });
            return Json(result);
        }

        public ActionResult GetDegreeModelList(string degreeName)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetDegreeModelList", false, new object[] {degreeName, JsonHelper.Serialize(dts) }).Obj[0];
            return Json(result);
        }

        public ActionResult GetTargetData()
        {
            var result = Submit("BaseData", "GetTargetData", false, new object[] { });
            return Json(result);
        }

        
        public ActionResult SaveModelDegree(DegreeOption model)
        {
            string user = Session[AppConst.SESSION_AUTH].ToString();
            var result = Submit("BaseData", "SaveModelDegree", false, new object[] { model, user });
            return Json(result);
        }

        public ActionResult GetDegreeModelById(int degreeId)
        {
            var result = Submit("BaseData", "GetDegreeModelById", false, new object[] { degreeId });
            return Json(result);
        }

        public ActionResult DeleteDegreeModel(int degreeId)
        {
            var result = Submit("BaseData", "DeleteDegreeModel", false, new object[] { degreeId });
            return Json(result);
        }


        #endregion
        
        
        #region 活动属性配置

        
        public ActionResult CampaignAttribute()
        {
            return View();
        }

        #endregion

        #region 活动类别
        public ActionResult CampaignCatg()
        {
            return View();
        }
        public JsonResult GetCampaignCatgList(string parentCatg)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetCampaignCatgList", false, new object[] { parentCatg,  JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }

        public JsonResult AddCampaignData(Campaign cam)
        {
            var result = Submit("BaseData", "AddCampaignData", false, new object[] { cam });
            return Json(result);
        }

        public JsonResult UpdateCampaignData(Campaign cam)
        {
            var result = Submit("BaseData", "UpdateCampaignData", false, new object[] { cam });
            return Json(result);
        }
        /// <summary>
        /// 根据id获取车型信息
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public JsonResult GetCampaignById(long camid)
        {
            var result = Submit("BaseData", "GetCampaignById", false, new object[] { camid });
            return Json(result.Obj[0].ToString());
        }

        public JsonResult DeleteCampaignById(long camid)
        {
            var result = Submit("BaseData", "DeleteCampaignById", false, new object[] { camid });
            return Json(result);
        }

        public JsonResult GetCampaignParentCatg(int? grade)
        {
            var result = Submit("BaseData", "GetCampaignParentCatg", false, new object[] { grade });
            return Json(result.Obj[0].ToString());
        }
        #endregion

        #region   供应商
        public ViewResult Supplier()
        {
            var province = Submit("BaseData", "GetProvince", false);
            ViewBag.province = province.Obj[0];
            return View();
        }

        //[HttpPost]
        //public JsonResult Supplier(string txtCode, string txtName, string txtPhone, string txtFax, string txtProvince, string txtCity, string txtContactPerson)
        //{
        //    string address = "";
        //    if (!string.IsNullOrWhiteSpace(txtProvince))
        //    {
        //        address += txtProvince;
        //    }
        //    if (!string.IsNullOrWhiteSpace(txtCity))
        //    {
        //        address += txtCity;
        //    }
        //    var result = Submit("BaseData", "AddSupplier", false, new object[] { txtCode, txtName, txtPhone, txtFax, address, txtContactPerson });
        //    return Json(result);
        //}

        /// <summary>
        /// 获取供应商 列表
        /// </summary>
        /// <param name="SupplierCode"></param>
        /// <param name="SupplierName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSupplierData(string SupplierCode, string SupplierName)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetSupplierData", false, new object[] {  JsonHelper.Serialize(dts),SupplierCode, SupplierName });
            return Json(result.Obj[0].ToString());
        }

        [HttpPost]
        public JsonResult AddSupplierData(Supplier model)
        {
            AuthModel authmodel = this.Auth;
            string adduserid = null;
            if (authmodel != null)
            {
                adduserid = authmodel.UserID.ToString();
            }
            var result = Submit("BaseData", "AddSupplierData", false, new object[] { model, adduserid });
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetSupplierByID(long? SupplierId)
        {
            var result = Submit("BaseData", "GetSupplierByID", false, new object[] { SupplierId });
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateSupplier(Supplier model)
        {
            AuthModel authmodel = this.Auth;
            string updateuserid = null;
            if (authmodel != null)
            {
                updateuserid = authmodel.UserID.ToString();
            }
            var result = Submit("BaseData", "UpdateSupplier", false, new object[] { model, updateuserid });
            return Json(result);
        }

        public JsonResult StopChannel(string basedataID)
        {
            var result = Submit("BaseData", "StopChannel", false, new object[] { basedataID,Auth.UserID });
            return Json(result);
        }
        #endregion

        #region 卡片类型维护
        public ActionResult CarTypeManage() {

            return View();
        }

        /// <summary>
        /// 获取表格数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCarTypeData(string CarName, string CarCode)
        {
            var dts = Request.CreateDataTableParameter();
            var result = Submit("BaseData", "GetCarTypeData", false, new object[] { CarName, CarCode, JsonHelper.Serialize(dts) });
            return Json(result.Obj[0].ToString());
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCarTypeById(int id)
        {
            var result = Submit("BaseData", "GetCarTypeById", false, new object[] { id });
            return Json(result.Obj[0]);
        }
        /// <summary>
        /// 修改卡片类型信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="carName"></param>
        /// <param name="carCode"></param>
        /// <param name="status"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JsonResult UpdateCarTypeData(int id, string carName, string carCode)
        {
            string user = Session[Utility.AppConst.SESSION_AUTH].ToString();
            var result = Submit("BaseData", "UpdateCarTypeData", false, new object[] { id, carName, carCode, user });
            return Json(result);
        }
        #endregion
        public JsonResult AddCarTypeData(string carName, string carCode)
        {
            string user = Session[Utility.AppConst.SESSION_AUTH].ToString();
            var result = Submit("BaseData", "AddCarTypeData", false, new object[] { carName, carCode,  user });
            return Json(result);
        }
        public JsonResult DeleteCarType(long Id)
        {
            var result = Submit("BaseData", "DeleteCarType", false, new object[] { Id });
            return Json(result);
        }

        
        
    }
}
