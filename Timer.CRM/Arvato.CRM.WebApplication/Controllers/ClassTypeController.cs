using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Arvato.CRM.WebApplication.Controllers
{
    public class ClassTypeController : Controller
    {
        //
        // GET: /ClassType/

        public ActionResult Index()
        {
            //判断用户是否是管理员
            bool isAdmin = Auth.RoleIDs.Contains(100);
            ViewBag.Role = isAdmin.ToString();
            //用户的DataGroupID
            ViewBag.dataGroupID = Auth.DataGroupID.Value;
            return View();
        }

        /// <summary>
        /// 获取option表关于系统属性的数据
        /// </summary>
        /// <param name="optType">字段类型</param>
        /// <returns></returns>
        public JsonResult GetOptionDataList(string optType)
        {
            var result = Submit("Service", "GetOptionDataList", false, new object[] { optType });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 获取DataGroup表的数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDataGroupList()
        {
            string user = Session[Utility.AppConst.SESSION_AUTH].ToString();
            var result = Submit("ClassType", "GetDataGroups", false, new object[] { user });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllRolesByDataGroupId(List<int> GroupId)
        {
            string dataGroupId = "";
            if (GroupId!=null&&GroupId.Count > 0) 
            {
                foreach (var item in GroupId)
                {
                    dataGroupId += item + ",";
                }
                dataGroupId = dataGroupId.Substring(0, dataGroupId.Length - 1);
            }
            
            var result = Submit("ClassType", "GetRoles", false, new object[] { dataGroupId });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 获取User数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUsersList(List<int> GroupId)
        {
            string dataGroupId = "";
            if (GroupId != null&&GroupId.Count > 0) 
            {
                foreach (var item in GroupId)
                {
                    dataGroupId += item + ",";
                }
                dataGroupId = dataGroupId.Substring(0, dataGroupId.Length - 1);
            }
           
            var result = Submit("ClassType", "GetUsers", false, new object[] { dataGroupId });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 获取Class数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetClassByID(int? classId)
        {
            var result = Submit("ClassType", "GetClassByID", false, new object[] { classId });
            return Json(result.Obj[0].ToString());
        }

        /// <summary>
        /// 删除Class
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteClassByID(int? classId)
        {
            var result = Submit("ClassType", "DeleteSubdivisonClassById", false, new object[] { classId });
            return Json(result);
        }

        /// <summary>
        /// 保存会员细分的自定义类型 
        /// </summary>
        /// <returns></returns>
        public JsonResult AddClass(string classType, List<string> roles, List<string> users, string className)
        {
            string roleID = "";
            string userID = "";
            if (roles != null && roles.Count > 0)
            {
                foreach (string item in roles)
                {
                    roleID += item + "|";
                }
            }
            if (users != null && users.Count > 0)
            {
                foreach (string item in users)
                {
                    userID += item + "|";
                }

            }

            var result = Submit("ClassType", "SaveSubdivisonClass", false, new object[] { Auth.UserID.ToString().Trim(), className, classType, roleID, userID });
            return Json(result);
        }

        /// <summary>
        /// 修改会员细分的自定义类型
        /// </summary>
        /// <param name="className"></param>
        /// <param name="classID"></param>
        /// <returns></returns>
        public JsonResult UpdateClass(int classID, string className, string classType)
        {
            var result = Submit("ClassType", "UpdateSubdivisonClassById", false, new object[] { Auth.UserID.ToString().Trim(), classID, className, classType });
            return Json(result);
        }

    }
}
