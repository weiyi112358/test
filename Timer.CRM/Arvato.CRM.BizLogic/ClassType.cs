using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.BizLogic
{
    public static class ClassType
    {

        /// <summary>
        /// 获取DataGroup数据列表
        /// </summary>
        /// <returns></returns>
        public static Result GetDataGroups(string user)
        {
            TM_AUTH_User auth_user = JsonHelper.Deserialize<TM_AUTH_User>(user);
            using (var db = new CRMEntities())
            {
                var groups = db.TM_SYS_DataGroup.Where(g => g.DataGroupGrade == auth_user.DataGroupID).ToList();
                return new Result(true, "", groups);
            }
        }

        /// <summary>
        /// 获取DataGroup数据列表
        /// </summary>
        /// <returns></returns>
        public static Result GetRoles(string dataGroupID)
        {
            using (var db = new CRMEntities())
            {
                var roles = from t in db.TM_AUTH_Role
                            where t.Enable == true
                            select t;
                if (dataGroupID != "")
                {
                    List<int> id = new List<int>();
                    string[] ss = dataGroupID.Split(',');
                    foreach (string item in ss)
                    {
                        id.Add(Convert.ToInt32(item));
                    }
                    roles = from t in db.TM_AUTH_Role
                                where t.Enable == true && id.Contains(t.DataGroupID.Value)
                                select t;
                }
                

                return new Result(true, "", roles.ToList());
            }
        }

        /// <summary>
        /// 获取DataGroup数据列表
        /// </summary>
        /// <returns></returns>
        public static Result GetUsers(string dataGroupID)
        {
            using (var db = new CRMEntities())
            {
                var users = from t in db.TM_AUTH_User
                            where t.Enable == true
                            select t;
                if (dataGroupID != "") 
                {
                    List<int> id = new List<int>();
                    string[] ss = dataGroupID.Split(',');
                    foreach (string item in ss)
                    {
                        id.Add(Convert.ToInt32(item));
                    }
                    users = from t in db.TM_AUTH_User
                                where t.Enable == true && id.Contains(t.DataGroupID.Value)
                                select t;
                }
                
                return new Result(true, "", users.ToList());
            }
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="classID"></param>
        /// <returns></returns>
        public static Result GetClassByID(int? classID)
        {
            using (var db = new CRMEntities())
            {
                var users = db.TM_SYS_Class.Where(g => g.ClassID == classID).FirstOrDefault();
                return new Result(true, "", users);
            }
        }

        /// <summary>
        /// 根据主键ID删除KPI数据
        /// </summary>
        /// <param name="aliasId"></param>
        /// <returns></returns>
        public static Result DeleteSubdivisonClassById(int classID)
        {
            try
            {
                using (CRMEntities db = new CRMEntities())
                {
                    var query = db.TM_SYS_Class.Where(p => p.ClassID == classID).FirstOrDefault();
                    if (query != null)
                    {
                        if (query.UserID.HasValue)
                        {
                            var querySubdivisionCount = db.TM_Mem_Subdivision.Where(
                                s => s.SubdivisionType.Trim().Equals(SqlFunctions.StringConvert((double)classID).Trim()) && s.AddedUser.Trim().Equals(SqlFunctions.StringConvert((double)query.UserID.Value).Trim())).Count();
                            if (querySubdivisionCount > 0)
                            {
                                return new Result(false, "已关联会员细分,无法删除细分类型!");
                            }
                        }
                        else if (query.RoleID.HasValue)
                        {
                            var querySubdivisionCount = from s in db.TM_Mem_Subdivision
                                                        join ur in db.TR_AUTH_UserRole on new { userID = s.AddedUser.Trim(), roleID = query.RoleID.Value } equals new { userID = SqlFunctions.StringConvert((double)ur.UserID).Trim(), roleID = ur.RoleID }
                                                        join r in db.TM_AUTH_Role on ur.RoleID equals r.RoleID
                                                        where s.SubdivisionType.Trim().Equals(SqlFunctions.StringConvert((double)classID).Trim())
                                                        select s;
                            if (querySubdivisionCount.Count() > 0)
                            {
                                return new Result(false, "已关联会员细分,无法删除细分类型!");
                            }
                        }

                        db.TM_SYS_Class.Remove(query);
                        db.SaveChanges();
                        return new Result(true, "删除成功");
                    }
                    return new Result(false, "查找不到要删除的客服细分类型");
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        /// <summary>
        /// 修改会员细分自定义设置
        /// </summary>
        /// <param name="user"></param>
        /// <param name="className"></param>
        /// <param name="classID"></param>
        /// <returns></returns>
        public static Result UpdateSubdivisonClassById(string currentUserID, int classID, string className, string classType)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    var query = db.TM_SYS_Class.Where(p => p.ClassID == classID).FirstOrDefault();
                    if (query != null)
                    {
                        if (query.RoleID != null)
                        {
                            //判断同一role下和role所对应的user下是否有相同的ClassName
                            var check = (from t in db.TM_SYS_Class
                                         where t.RoleID == query.RoleID
                                            && t.ClassName.Trim().Equals(className.Trim())
                                            && t.ClassType.Trim().Equals(classType.Trim())
                                         select t)
                                        .Union
                                        (from t in db.TM_SYS_Class
                                         join t1 in db.TR_AUTH_UserRole on t.UserID equals t1.UserID
                                         where t1.RoleID == query.RoleID
                                            && t.ClassName.Trim().Equals(className.Trim())
                                            && t.ClassType.Trim().Equals(classType.Trim())
                                         select t);
                            if (check.Count() > 0)
                            {
                                return new Result(false, "更新失败,同一角色下已存在相同名称");
                            }
                            query.ClassName = className;
                            query.ModifiedDate = DateTime.Now;
                            query.ModifiedUser = currentUserID;
                        }
                        else if (query.UserID != null)
                        {
                            //判断同一role下和role所对应的user下是否有相同的ClassName
                            var check = (from t in db.TM_SYS_Class
                                         where t.UserID == query.UserID
                                            && t.ClassName.Trim().Equals(className.Trim())
                                            && t.ClassType.Trim().Equals(classType.Trim())
                                         select t)
                                        .Union
                                        (from t in db.TM_SYS_Class
                                         join t1 in db.TR_AUTH_UserRole on t.RoleID equals t1.RoleID
                                         where t1.UserID == query.UserID
                                            && t.ClassName.Trim().Equals(className.Trim())
                                            && t.ClassType.Trim().Equals(classType.Trim())
                                         select t);
                            if (check.Count() > 0)
                            {
                                return new Result(false, "更新失败,同一角色下已存在相同名称");
                            }
                            query.ClassName = className;
                            query.ModifiedDate = DateTime.Now;
                            query.ModifiedUser = currentUserID;
                        }
                        else
                        {
                            return new Result(false, "更新的会员细分类型的数据权限信息不完整");
                        }
                    }
                    else
                    {
                        return new Result(false, "查找不到要更新的会员细分类型");
                    }

                    db.SaveChanges();
                    return new Result(true, "更新会员细分类型成功");
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        /// <summary>
        /// 添加会员细分自定义设置
        /// </summary>
        /// <param name="user"></param>
        /// <param name="classType"></param>
        /// <param name="dataGroupID"></param>
        /// <param name="roleID"></param>
        /// <param name="userID"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static Result SaveSubdivisonClass(string currentUserID, string className, string classType, string roleID, string userID)
        {
            try
            {
                using (var db = new CRMEntities())
                {
                    if (!string.IsNullOrEmpty(roleID))
                    {
                        string[] roles = roleID.Substring(0, roleID.Length - 1).Split('|');
                        foreach (string item in roles)
                        {
                            string[] role = item.Split(',');
                            int r = Convert.ToInt32(role[0]);
                            //判断同一role下和role所对应的user下是否有相同的ClassName
                            var check = (from t in db.TM_SYS_Class
                                         where t.RoleID == r
                                            && t.ClassName.Trim().Equals(className.Trim())
                                            && t.ClassType.Trim().Equals(classType.Trim())
                                         select t)
                                        .Union
                                        (from t in db.TM_SYS_Class
                                         join t1 in db.TR_AUTH_UserRole on t.UserID equals t1.UserID
                                         where t1.RoleID == r
                                             && t.ClassName.Trim().Equals(className.Trim())
                                             && t.ClassType.Trim().Equals(classType.Trim())
                                         select t);

                            if (check.Count() > 0)
                            {
                                return new Result(false, "新增会员细分类型失败,同一角色已存在相同名称");
                            }
                            TM_SYS_Class classT = new TM_SYS_Class()
                            {
                                ClassName = className,
                                ClassType = classType,
                                DataGroupID = Convert.ToInt32(role[1]),
                                RoleID = Convert.ToInt32(role[0]),
                                AddedUser = currentUserID,
                                AddedDate = DateTime.Now
                            };
                            db.TM_SYS_Class.Add(classT);
                        }
                    }
                    else if (!string.IsNullOrEmpty(userID))
                    {
                        string[] users = userID.Substring(0, userID.Length - 1).Split('|');
                        foreach (string item in users)
                        {
                            string[] userid = item.Split(',');
                            int r = Convert.ToInt32(userid[0]);
                            //判断同一role下和role所对应的user下是否有相同的ClassName
                            var check = (from t in db.TM_SYS_Class
                                         where t.UserID == r
                                             && t.ClassName.Trim().Equals(className.Trim())
                                             && t.ClassType.Trim().Equals(classType.Trim())
                                         select t)
                                        .Union
                                        (from t in db.TM_SYS_Class
                                         join t1 in db.TR_AUTH_UserRole on t.RoleID equals t1.RoleID
                                         where t1.UserID == r
                                            && t.ClassName.Trim().Equals(className.Trim())
                                            && t.ClassType.Trim().Equals(classType.Trim())
                                         select t);
                            if (check.Count() > 0)
                            {
                                return new Result(false, "新增会员细分类型失败,同一角色已存在相同名称");
                            }
                            TM_SYS_Class classT = new TM_SYS_Class()
                            {
                                ClassName = className,
                                ClassType = classType,
                                DataGroupID = Convert.ToInt32(userid[1]),
                                UserID = Convert.ToInt32(userid[0]),
                                AddedUser = currentUserID,
                                AddedDate = DateTime.Now
                            };
                            db.TM_SYS_Class.Add(classT);
                        }
                    }
                    else
                    {
                        return new Result(false, "请填写细分类型的权限组");
                    }

                    db.SaveChanges();
                    return new Result(true, "新增会员细分类型成功");
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }
    }
}
