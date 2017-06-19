using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using Arvato.CRM.Model;

namespace Arvato.CRM.WebApplication.Controllers
{
    public class AuthController : Controller
    {
        //
        // GET: /Auth/

        #region 我的信息维护
        public ActionResult MyProfile()
        {
            var rst = Submit("Auth", "GetUserProfileById", false, Auth.UserID);
            return View(JsonHelper.Deserialize<UserProfileModel>(rst.Obj[0].ToString()));
        }
        public JsonResult SubmitMyProfile(UserProfileModel model)
        {
            var userId = Auth.UserID;//提交人的UserID
            model.UserID = userId;
            var result = Submit("Auth", "UpdateUserProfile", false, new object[] { model, userId });
            return Json(result);
        }
        #endregion

        #region 用户管理
        public ActionResult Users()
        {
            var result = Submit("Service", "GetDataGroupsByParentId", false, Auth.DataGroupID);
            ViewBag.DataGroups = result.Obj[0];
            return View();
        }

        public ActionResult SubUsers()
        {
            var result = Submit("Service", "GetDataGroupsByParentId", false, Auth.DataGroupID);
            ViewBag.DataGroups = result.Obj[0];
            return View();
        }

        public JsonResult GetUsers(string userName, int userState, int searchGroupId)
        {
            var dts = Request.CreateDataTableParameter();
            var userId = Auth.UserID;
            var rst = Submit("Auth", "GetUsers", false, new object[] { JsonHelper.Serialize(dts), Auth.DataGroupID,searchGroupId, userName, userState, userId });
            return Json(rst.Obj[0].ToString());
        }

        public JsonResult GetSubUsers(string userName, int userState, int searchGroupId)
        {
            var dts = Request.CreateDataTableParameter();
            var userId = Auth.UserID;
            var rst = Submit("Auth", "GetSubUsers", false, new object[] { JsonHelper.Serialize(dts), Auth.DataGroupID, searchGroupId, userName, userState, userId });
            return Json(rst.Obj[0].ToString());
        }

        public ActionResult ChangePassword()
        {
            var rst = Submit("Auth", "GetUserProfileById", false, Auth.UserID);
            return View(JsonHelper.Deserialize<UserProfileModel>(rst.Obj[0].ToString()));
        }
        public JsonResult SubmitChangePassword(ChangePasswordModel model)
        {
            var userId = Auth.UserID;//提交人的UserID
            model.UserID = userId;
            model.OldPassword = HttpUtility.HtmlDecode(model.OldPassword);
            model.NewPassword = HttpUtility.HtmlDecode(model.NewPassword);
            model.ConfirmPassword = HttpUtility.HtmlDecode(model.ConfirmPassword);
            var result = Submit("Auth", "ChangePassword", false, new object[] { model, userId });
            return Json(result);
        }


        public JsonResult ResetPassword(int userId, string password)
        {
            password = HttpUtility.HtmlDecode(password);
            var modifiedBy = Auth.UserID;
            var result = Submit("Auth", "ResetPassword", false, new object[] { userId, password, modifiedBy });
            return Json(result);
        }

        public JsonResult ActiveUser(int userId, bool enable)
        {
            var modifiedBy = Auth.UserID;
            var result = Submit("Auth", "ActiveUser", false, new object[] { userId, enable, modifiedBy });
            return Json(result);
        }

        public JsonResult EditUser(int userId)
        {
            var result = Submit("Auth", "GetUserProfileById", false, userId);
            return Json(result);
        }

        public JsonResult SubmitUserEdit(UserProfileModel model)
        {
            var userId = Auth.UserID;//提交人的UserID
            var result = Submit("Auth", "UpdateUserProfile", false, new object[] { model, userId });
            return Json(result);
        }

        public JsonResult SubmitUserAdd(string userJsonStr)
        {
            userJsonStr = HttpUtility.HtmlDecode(userJsonStr);
            var userId = Auth.UserID;//提交人的UserID
            var result = Submit("Auth", "CreateUser", false, new object[] { userJsonStr, userId });
            return Json(result);
        }

        public JsonResult SubmitChangeRoles(int userId, List<int> pageRoles, List<int> dataRoles)
        {
            pageRoles = pageRoles ?? new List<int>();
            dataRoles = dataRoles ?? new List<int>();
            var roles = pageRoles.Union(dataRoles);
            var result = Submit("Auth", "ChangeUserRoles", false, new object[] { userId, JsonHelper.Serialize(roles) });
            return Json(result);
        }

        public JsonResult SubmitChangeBusi(int userId, List<int> pageRoles)
        {
            pageRoles = pageRoles ?? new List<int>();
            var result = Submit("Auth", "ChangeUserBusi", false, new object[] { userId, JsonHelper.Serialize(pageRoles) });
            return Json(result);
        }
        /// <summary>
        /// 根据数据群组查找其和其下级的所有角色
        /// </summary>
        /// <param name="dataGroupId">数据群组ID</param>
        /// <param name="roleType">角色类型（page或data）</param>
        /// <returns></returns>
        public JsonResult GetAllRolesByDataGroupId(int? dataGroupId, string roleType)
        {
            var result = Submit("Service", "GetAllRolesByDataGroupId", false, new object[] { dataGroupId, roleType });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 根据其创建用户查找其子用户的角色
        /// </summary>
        /// <param name="dataGroupID">数据群组ID</param>
        /// <param name="roleType">角色类型（page或data或""）</param>
        /// <param name="addeduser">该用户创建者id</param>
        /// <returns></returns>
        public JsonResult GetRolesByAddedUser(int? dataGroupId, string roleType, string addedUser)
        {
            var result = Submit("Service", "GetRolesByAddedUser", false, new object[] { dataGroupId, roleType,addedUser });
            return Json(result.Obj[0]);
        }
        



        /// <summary>
        /// 根据用户ID查找角色
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public JsonResult GetRolesByUserId(int userId)
        {
            var result = Submit("Service", "GetRolesByUserId", false, userId);
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 根据用户ID查找角色
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public JsonResult GetSubRolesByUserId(int userId)
        {
            var result = Submit("Service", "GetSubRolesByUserId", false, userId);
            return Json(result.Obj[0]);
        }

        public JsonResult GetBusiByUserId(int userId)
        {
            var result = Submit("Service", "GetBusiByUserId", false, userId);
            return Json(result.Obj[0]);
        }
        /// <summary>
        /// 根据用户ID加载角色
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public JsonResult LoadRolesByUserId(int userId)
        {
            var result = Submit("Service", "LoadRolesByUserId", false, userId);
            return Json(result.Obj[0]);
        }
        #endregion

        #region 角色管理
        /// <summary>
        /// 角色管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Roles()
        {
            var groups = Submit("Service", "GetDataGroupsExceptRootByParentId", false, Auth.DataGroupID);
            ViewBag.DataGroups = groups.Obj[0];
            var dataLimitTypes = Submit("Service", "GetBizOptionsByType", false, new object[] { "DataLimitType", true });
            ViewBag.DataLimitTypes = dataLimitTypes.Obj[0];
            var pages = Submit("Auth", "GetAllPages", false, new object[] { true, null });
            ViewBag.AllPages = pages.Obj[0];
            return View();
        }

        /// <summary>
        /// 分页获取角色
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <param name="roleType">角色类型（page或data）</param>
        /// <returns></returns>
        public JsonResult GetRolesByPage(string roleName, string roleType, string roleState,int searchGroupId)
        {
            var dts = Request.CreateDataTableParameter();
            var dataGroupId = Auth.DataGroupID;
            var result = Submit("Auth", "GetRolesByPage", false, new object[] { JsonHelper.Serialize(dts), roleName, roleType, roleState, searchGroupId,dataGroupId });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 根据条件查询页面元素
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="pageIds">页面ID列表</param>
        /// <returns></returns>
        public JsonResult GetElementsByCondition(int? roleId, List<int> pageIds)
        {
            pageIds = pageIds ?? new List<int>();
            var dts = Request.CreateDataTableParameter();
            var result = Submit("Auth", "GetElementsByCondition", false, new object[] { JsonHelper.Serialize(dts), roleId, JsonHelper.Serialize(pageIds.ToList()) });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 根据ID获取角色信息
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public JsonResult GetRoleInfoById(int roleId)
        {
            var result = Submit("Auth", "GetRoleInfoById", false, new object[] { roleId });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 保存角色信息
        /// </summary>
        /// <param name="roleJsonStr">角色对象序列化</param>
        /// <param name="pageIds">页面ID列表</param>
        /// <param name="elementsJsonStr">页面元素对象序列化</param>
        /// <returns></returns>
        public JsonResult SubmitPageRoleEdit(string roleJsonStr, List<int> pageIds, string elementsJsonStr)
        {
            var result = Submit("Auth", "SavePageRole", false, new object[] { roleJsonStr, JsonHelper.Serialize(pageIds), elementsJsonStr, Auth.UserID });
            return Json(result);
        }

        /// <summary>
        /// 根据条件查BaseData
        /// </summary>
        /// <param name="dataGroupId">数据群组ID</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public JsonResult GetBaseDataByCondition(int? dataGroupId, string type)
        {
            var groupId = dataGroupId == null ? -1 : (int)dataGroupId;
            var result = Submit("Auth", "GetBaseDataByCondition", false, new object[] { dataGroupId, type });
            return Json(result.Obj[0]);
        }

        /// <summary>
        /// 提交数据角色
        /// </summary>
        /// <param name="roleJsonStr">角色对象序列化</param>
        /// <param name="limitType">维度</param>
        /// <param name="limitValues">维度值</param>
        /// <param name="pageIds">指定页面</param>
        /// <returns></returns>
        public JsonResult SubmitDataRoleEdit(string roleJsonStr, string limitType, List<string> limitValues, List<int> pageIds)
        {
            var result = Submit("Auth", "SaveDataRole", false, new object[] { roleJsonStr, limitType, JsonHelper.Serialize(limitValues), JsonHelper.Serialize(pageIds), Auth.UserID });
            return Json(result);
        }

        /// <summary>
        /// 根据ID获取数据角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public JsonResult GetDataRoleInfoById(int roleId)
        {
            var result = Submit("Auth", "GetDataRoleInfoById", false, new object[] { roleId });
            return Json(result);
        }

        /// <summary>
        /// 根绝ID删除角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public JsonResult DeleteRoleById(int roleId)
        {
            var result = Submit("Auth", "DeleteRoleById", false, new object[] { roleId });
            return Json(result);
        }
        #endregion
    }
}
