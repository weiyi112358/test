using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Arvato.CRM.Utility.Datatables;
using System.Data;

namespace Arvato.CRM.BizLogic
{
    public static class Auth
    {
        public static Result GetUsers(string dp, int? curDataGroupId, int searchGroupId, string userName, int userState, int userId)
        {
            curDataGroupId = curDataGroupId ?? -1;
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var query = (from u in db.TM_AUTH_User
                             join d in db.V_Sys_DataGroupRelation on u.DataGroupID equals d.SubDataGroupID
                             where d.DataGroupID == curDataGroupId && (searchGroupId > 0 ? u.DataGroupID == searchGroupId : true)
                             select u).ToList();
                //门店过滤一下

                //List<string> brand = new List<string>();
                //var store = Service.GetStoreNameByUserID(userId,ref brand);//获取门店
                //List<TM_AUTH_User> list = new List<TM_AUTH_User>(); ;
                //if (!string.IsNullOrEmpty(store)&&brand.Count<=0)
                //{
                //    if (query.Count > 0)
                //    {
                //        var storeCode1 = store.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //        foreach (var item in query)
                //        {
                //            var s = Service.GetStoreNameByUserID(item.UserID,ref brand);
                //            if (!string.IsNullOrEmpty(s))
                //            {
                //                var storeCode2 = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //                //判断两个数组中数据是否有相同的，
                //                var q = from a in storeCode1 join b in storeCode2 on a equals b select a;
                //                if (q.Count() > 0)
                //                {
                //                    list.Add(item);
                //                }
                //            }
                //            //else {
                //            //    list.Add(item);
                //            //}
                //        }
                //        query = list;
                //    }
                //}
                //else
                //{
                //    query = (from u in query
                //             join d in db.V_Sys_DataGroupRelation on u.DataGroupID equals d.SubDataGroupID
                //             where d.DataGroupID == curDataGroupId
                //             select u).ToList();
                //}
                if (!string.IsNullOrEmpty(userName))
                    query = query.Where(o => o.UserName.Contains(userName)).ToList();
                if (userState == 1)
                    query = query.Where(o => o.Enable == true).ToList();
                if (userState == 2)
                    query = query.Where(o => o.Enable == false).ToList();

                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }


        public static Result GetSubUsers(string dp, int? curDataGroupId, int searchGroupId, string userName, int userState, int userId)
        {
            curDataGroupId = curDataGroupId ?? -1;
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            string strUserId = userId.ToString();
            using (CRMEntities db = new CRMEntities())
            {

                var query = (from u in db.TM_AUTH_User
                             join d in db.V_Sys_DataGroupRelation on u.DataGroupID equals d.SubDataGroupID
                             where d.DataGroupID == curDataGroupId && (searchGroupId > 0 ? u.DataGroupID == searchGroupId : true) && u.AddedUser == strUserId
                             select u).ToList();

                if (!string.IsNullOrEmpty(userName))
                    query = query.Where(o => o.UserName.Contains(userName)).ToList();
                if (userState == 1)
                    query = query.Where(o => o.Enable == true).ToList();
                if (userState == 2)
                    query = query.Where(o => o.Enable == false).ToList();
                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }


        /// <summary>
        /// 校验登录用户
        /// </summary>
        /// <param name="sessions">Session列表</param>
        /// <param name="username">登录名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static Result CheckUser(string sessions, string username, string pwd)
        {
            try
            {
                var u = new AuthModel();
                using (CRMEntities db = new CRMEntities())
                {
                    var user = db.TM_AUTH_User.FirstOrDefault(o => o.LoginName == username);
                    if (user != null)
                    {
                        if (user.Password != Utility.ToolsHelper.MD5(pwd)) return new Result(false, "密码错误");
                        if (!user.Enable) return new Result(false, "用户已禁用");
                    }
                    else
                    {
                        return new Result(false, "用户名不存在");
                    }
                    u.UserID = user.UserID;
                    u.UserDisplayName = user.UserName;
                    u.Username = user.LoginName;
                    u.DataGroupID = user.DataGroupID;

                    //角色
                    var roles = db.TR_AUTH_UserRole.Where(o => o.UserID == u.UserID).Select(s => s.RoleID).ToList();
                    u.RoleIDs = roles;

                    //菜单
                    var queryMenu = (from page in db.TD_SYS_Page
                                     join rp in db.TR_AUTH_RolePage on page.PageID equals rp.PageID
                                     join ur in db.TR_AUTH_UserRole on rp.RoleID equals ur.RoleID
                                     join menu in db.TD_SYS_Menu on page.MenuID equals menu.MenuID
                                     where ur.UserID == user.UserID && page.Enable
                                     select new Pages
                                     {
                                         PageID = page.PageID,
                                         MenuID = page.MenuID,
                                         MenuName = menu.MenuName,
                                         DispName = page.PageDesc,
                                         Path = page.Path,
                                         Type = menu.Type,
                                         Nav = menu.Nav,
                                         MenuSort = menu.Sort,
                                         PageSort = page.Sort
                                     }).Distinct().OrderBy(o => o.MenuSort).ThenBy(o => o.PageSort);
                    u.Pages = queryMenu.ToList();

                    var roleID = db.TM_AUTH_Role.Where(p => p.RoleType == "page" && u.RoleIDs.Contains(p.RoleID)).FirstOrDefault();

                    if (roleID != null)
                    {
                        var queryDefaultUrl = (from p in db.TM_AUTH_Role
                                               where p.RoleID == roleID.RoleID
                                               select new
                                               {
                                                   p.DefaultPath,
                                                   p.EnableDashBoard
                                               }).FirstOrDefault();
                        u.DefaultPath = queryDefaultUrl.DefaultPath;
                        u.EnableDashBoard = queryDefaultUrl.EnableDashBoard;
                    }
                    else
                    {
                        u.DefaultPath = "/Home/Index";
                        u.EnableDashBoard = true;
                    }
                    

                    //页面元素权限
                    var queryElement = from e in db.TM_AUTH_RolePageElementSettings
                                       join rp in db.TR_AUTH_RolePage on new { RoleID = e.RoleID, PageID = e.PageID } equals new { RoleID = rp.RoleID, PageID = rp.PageID }
                                       join r in db.TM_AUTH_Role on e.RoleID equals r.RoleID
                                       join ur in db.TR_AUTH_UserRole on e.RoleID equals ur.RoleID
                                       where ur.UserID == user.UserID
                                       select new PageElementModel
                                       {
                                           ElementKey = e.ElementKey,
                                           PageID = e.PageID,
                                           RoleID = e.RoleID,
                                           SettingCss = e.SettingCss,
                                           SettingProp = e.SettingProp
                                       };

                    List<string> rids = new List<string>();
                    foreach (var item in u.RoleIDs)
                    {
                        rids.Add(item.ToString());
                    }
                    var dataLimit = (from d in db.TM_AUTH_DataLimit
                                     where d.HierarchyType == "role" && rids.Contains(d.HierarchyValue)
                                     select d).ToList();
                    string storeCode = "";
                    foreach (var item in dataLimit)
                    {
                        storeCode += item.RangeValue + ",";
                    }
                    if (storeCode != "")
                        storeCode = storeCode.Substring(0, storeCode.Length - 1);
                    u.PageElements = queryElement.ToList();
                    
                    u.StoreCode = storeCode;
                    var dict = new Dictionary<string, object>();
                    dict.Add(Utility.AppConst.SESSION_AUTH, u);
                    return new Result(true, "", null, dict);
                }
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }

        }


        //public static Result CheckUser(string sessions, string username, string pwd, string groupType, string groupID, string storeCode)
        //{
        //    var u = new AuthModel();
        //    using (CRMEntities db = new CRMEntities())
        //    {
        //        var user = db.TM_AUTH_User.FirstOrDefault(o => o.LoginName == username);
        //        if (user != null)
        //        {
        //            //用户角色关联门店列表
        //            var userRoleList = db.TR_AUTH_UserRole.Where(p => p.UserID == user.UserID).ToList();
        //            var dataLimit = db.TM_AUTH_DataLimit.Where(p => p.HierarchyType == "role").ToList();
        //            var lmtStoreList = (from r in userRoleList
        //                                join a in dataLimit.Where(p => p.RangeType == "store")
        //                        on r.RoleID.ToString() equals a.HierarchyValue
        //                                select a.RangeValue).Distinct().ToList();
        //            var lmtBrandList = (from r in userRoleList
        //                                join a in dataLimit.Where(p => p.RangeType == "brand")
        //                                on r.RoleID.ToString() equals a.HierarchyValue
        //                                select a.RangeValue).Distinct().ToList();
        //            var lmtList = (from r in userRoleList
        //                           join a in dataLimit
        //                           on r.RoleID.ToString() equals a.HierarchyValue
        //                           select new { a.RangeValue ,a.RangeType}).Distinct().ToList();

        //            if (!string.IsNullOrWhiteSpace(groupID))
        //            {
        //                var brandInfo = db.V_M_TM_SYS_BaseData_brand.Where(p => p.BrandCodeBase == groupID).FirstOrDefault();
        //                if (brandInfo != null)
        //                {
        //                    groupID = brandInfo.BrandCodeBase;
        //                }
        //            }



        //            if (user.Password != Utility.ToolsHelper.MD5(pwd)) return new Result(false, "密码错误");
        //            if (!user.Enable) return new Result(false, "用户已禁用");
        //            //总部
        //            if (groupType == "1" && user.DataGroupID != 1)
        //            {
        //                return new Result(false, "输入的用户名不是 总部 账号");
        //            }
        //            else if (groupType == "2" && !lmtBrandList.Any())
        //            {
        //                return new Result(false, "输入的用户名不属于 区域/品牌 级别，请重新选择分类");
        //            }
        //            else if (groupType == "2" && string.IsNullOrEmpty(groupID))//&& !lmtBrandList.Contains(groupID)
        //            {
        //                return new Result(false, "输入的用户名选择的 区域/品牌 不正确");
        //            }
        //            else if (groupType == "3" && !lmtStoreList.Any())
        //            {
        //                return new Result(false, "输入的用户名不属于门店级别，请重新选择分类");
        //            }
        //            else if (groupType == "3" && !lmtStoreList.Contains(storeCode))
        //            {
        //                return new Result(false, "输入的用户名不是所选门店的用户");
        //            }


        //            List<LimitData> LimitData = new List<LimitData>();

        //            if (!string.IsNullOrEmpty(storeCode))
        //            {
        //                var storeInfo = db.V_M_TM_SYS_BaseData_store.Where(p => p.StoreCode == storeCode).FirstOrDefault();
        //                if (storeInfo != null)
        //                {
        //                    u.BrandCode = storeInfo.BrandCodeStore;
        //                    u.AreaCode = storeInfo.AreaCodeStore;
        //                }
        //            }
        //            else
        //            {
        //                foreach (var item in lmtList)
        //                {
        //                    LimitData model = new LimitData()
        //                    {
        //                        LimitType = item.RangeType,
        //                        LimitValue = item.RangeValue
        //                    };
        //                    LimitData.Add(model);
        //                }
        //                u.LimitData = LimitData;
        //            }
        //        }
        //        else
        //        {
        //            return new Result(false, "用户名不存在");
        //        }
        //        u.UserID = user.UserID;
        //        u.UserDisplayName = user.UserName;
        //        u.Username = user.LoginName;
        //        u.DataGroupID = user.DataGroupID;
        //        u.StoreCode = storeCode;

        //        var qGroup = db.TM_SYS_DataGroup.Where(o => o.DataGroupID == user.DataGroupID).FirstOrDefault();
        //        if (qGroup != null)
        //            u.DataGroupGrade = qGroup.DataGroupGrade;
        //        //角色
        //        var roles = db.TR_AUTH_UserRole.Where(o => o.UserID == u.UserID).Select(s => s.RoleID).ToList();
        //        u.RoleIDs = roles;

        //        //菜单
        //        var queryMenu = (from page in db.TD_SYS_Page
        //                         join rp in db.TR_AUTH_RolePage on page.PageID equals rp.PageID
        //                         join ur in db.TR_AUTH_UserRole on rp.RoleID equals ur.RoleID
        //                         join menu in db.TD_SYS_Menu on page.MenuID equals menu.MenuID
        //                         where ur.UserID == user.UserID && page.Enable
        //                         select new Pages
        //                         {
        //                             PageID = page.PageID,
        //                             MenuID = page.MenuID,
        //                             MenuName = menu.MenuName,
        //                             DispName = page.PageDesc,
        //                             Path = page.Path,
        //                             Type = menu.Type,
        //                             Nav = menu.Nav,
        //                             MenuSort = menu.Sort,
        //                             PageSort = page.Sort,
        //                             Visable = page.Visable
        //                         }).Distinct().OrderBy(o => o.MenuSort).ThenBy(o => o.PageSort);
        //        u.Pages = queryMenu.ToList();

        //        //页面元素权限
        //        var queryElement = from e in db.TM_AUTH_RolePageElementSettings
        //                           join rp in db.TR_AUTH_RolePage on new { RoleID = e.RoleID, PageID = e.PageID } equals new { RoleID = rp.RoleID, PageID = rp.PageID }
        //                           join r in db.TM_AUTH_Role on e.RoleID equals r.RoleID
        //                           join ur in db.TR_AUTH_UserRole on e.RoleID equals ur.RoleID
        //                           where ur.UserID == user.UserID
        //                           select new PageElementModel
        //                           {
        //                               ElementKey = e.ElementKey,
        //                               PageID = e.PageID,
        //                               RoleID = e.RoleID,
        //                               SettingCss = e.SettingCss,
        //                               SettingProp = e.SettingProp
        //                           };
        //        u.PageElements = queryElement.ToList();
        //        //获取角色权限门店
        //        if (u.RoleIDs != null)
        //        {
        //            List<string> roleStrIds = new List<string>();
        //            foreach (int i in u.RoleIDs)
        //            {
        //                roleStrIds.Add(i.ToString());
        //            }
        //            List<string> storeLimits = new List<string>();
        //            var qDataLimit = db.TM_AUTH_DataLimit.Where(o => o.HierarchyType == "role" && roleStrIds.Contains(o.HierarchyValue) && o.RangeType == "store").ToList();
        //            if (qDataLimit != null)
        //            {
        //                for (int i = 0; i < qDataLimit.Count; i++)
        //                {
        //                    storeLimits.Add(qDataLimit[i].RangeValue);
        //                }
        //            }
        //            var qDataLimit2 = (from a in db.TM_SYS_BaseData.Where(p => p.BaseDataType == "store")
        //                               join b in db.TM_AUTH_DataLimit.Where(o => o.HierarchyType == "role" && roleStrIds.Contains(o.HierarchyValue) && o.RangeType == "brand")
        //                                         on a.Str_Attr_11 equals b.RangeValue
        //                               select a).ToList();
        //            if (qDataLimit2 != null)
        //            {
        //                for (int i = 0; i < qDataLimit2.Count; i++)
        //                {
        //                    storeLimits.Add(qDataLimit2[i].Str_Attr_2);
        //                }
        //            }

        //            u.StoreCodes = storeLimits.Distinct().ToList();
        //        }

        //        //获取角色权限群组
        //        if (u.RoleIDs != null)
        //        {
        //            List<string> roleStrIds = new List<string>();
        //            foreach (int i in u.RoleIDs)
        //            {
        //                roleStrIds.Add(i.ToString());
        //            }
        //            List<int> GroupIDs = new List<int>();
        //            List<string> Brands = new List<string>();
        //            var qDataLimit2 = (from b in db.TM_AUTH_DataLimit.Where(o => o.HierarchyType == "role" && roleStrIds.Contains(o.HierarchyValue) && o.RangeType == "brand")
        //                               select b).ToList();
        //            if (qDataLimit2 != null)
        //            {
        //                var BrandGroup = db.TM_SYS_BaseData.Where(p => p.BaseDataType == "brand").ToList();
        //                foreach (var group in qDataLimit2)
        //                {
        //                    var q = BrandGroup.Where(p => p.Str_Attr_2 == group.RangeValue).FirstOrDefault();
        //                    if (q != null)
        //                    {
        //                        GroupIDs.Add(q.DataGroupID);
        //                        Brands.Add(q.Str_Attr_2);
        //                    }
        //                }
        //            }
        //            ////把自身的群组也添加进去
        //            //GroupIDs.Add(user.DataGroupID ?? 0);
        //            u.GroupIDs = GroupIDs;
        //            u.Brands = Brands;
        //        }
        //        var roleID = u.RoleIDs[0];
        //        var queryDefaultUrl = (from p in db.TM_AUTH_Role
        //                               where p.RoleID == roleID
        //                               select new
        //                               {
        //                                   p.DefaultPath,
        //                                   p.EnableDashBoard
        //                               }).FirstOrDefault();
        //        u.DefaultPath = queryDefaultUrl.DefaultPath;
        //        u.EnableDashBoard = queryDefaultUrl.EnableDashBoard;



        //        var dict = new Dictionary<string, object>();
        //        dict.Add(Utility.AppConst.SESSION_AUTH, u);
        //        return new Result(true, "", null, dict);
        //    }
        //}

        /// <summary>
        /// 根据ID查找用户基本信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public static Result GetUserProfileById(long userId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var user = (from a in db.TM_AUTH_User
                            where a.UserID == userId
                            select new UserProfileModel
                            {
                                UserID = a.UserID,
                                UserName = a.UserName,
                                LoginName = a.LoginName,
                                DataGroupID = a.DataGroupID,
                                Mobile = a.Mobile,
                                Email = a.Email
                            }).FirstOrDefault();
                if (user != null)
                {
                    return new Result(true, "修改成功", user);
                }
                else
                {
                    return new Result(false, "没有查询到用户");
                }
            }
        }

        /// <summary>
        /// 更新用户基本信息
        /// </summary>
        /// <param name="model">用户基本信息</param>
        /// <param name="modifiedBy">修改人ID</param>
        /// <returns></returns>
        public static Result UpdateUserProfile(UserProfileModel model, int modifiedBy)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var user = db.TM_AUTH_User.Find(model.UserID);
                if (user == null)
                {
                    return new Result(false, "此用户不存在");
                }

                //修改用户信息
                try
                {
                    user.UserName = model.UserName;
                    user.Mobile = model.Mobile;
                    user.Email = model.Email;
                    user.ModifiedUser = modifiedBy.ToString();
                    user.ModifiedDate = DateTime.Now;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return new Result(true, "用户信息修改成功");
                }
                catch (Exception ex)
                {
                    return new Result(false, ex.Message);
                }
            }
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="model">密码信息</param>
        /// <param name="modifiedBy">修改人ID</param>
        /// <returns></returns>
        public static Result ChangePassword(ChangePasswordModel model, int modifiedBy)
        {
            if (string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrEmpty(model.ConfirmPassword))
            {
                return new Result(false, "新密码不能为空");
            }
            if (!model.NewPassword.Equals(model.ConfirmPassword))
            {
                return new Result(false, "新密码输入不一致");
            }
            using (CRMEntities db = new CRMEntities())
            {
                var user = db.TM_AUTH_User.Find(model.UserID);
                if (user == null)
                {
                    return new Result(false, "此用户不存在");
                }
                if (!Utility.ToolsHelper.MD5(model.OldPassword).Equals(user.Password))
                {
                    return new Result(false, "原密码错误");
                }

                //修改用户密码
                try
                {
                    user.Password = Utility.ToolsHelper.MD5(model.NewPassword);
                    user.ModifiedUser = modifiedBy.ToString();
                    user.ModifiedDate = DateTime.Now;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return new Result(true);
                }
                catch (Exception ex)
                {
                    return new Result(false, ex.Message);
                }
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="password">新密码</param>
        /// <param name="modifiedBy">修改人ID</param>
        /// <returns></returns>
        public static Result ResetPassword(int userId, string password, int modifiedBy)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var user = db.TM_AUTH_User.Find(userId);
                if (user == null)
                {
                    return new Result(false, "此用户不存在");
                }

                //修改用户启用状态
                try
                {
                    user.Password = Utility.ToolsHelper.MD5(password);
                    user.ModifiedUser = modifiedBy.ToString();
                    user.ModifiedDate = DateTime.Now;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return new Result(true, "密码重置成功");
                }
                catch (Exception ex)
                {
                    return new Result(false, ex.Message);
                }
            }
        }

        /// <summary>
        /// 修改用户启用状态
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="enable">是否启用用户</param>
        /// <param name="modifiedBy">修改人ID</param>
        /// <returns></returns>
        public static Result ActiveUser(int userId, bool enable, int modifiedBy)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var user = db.TM_AUTH_User.Find(userId);
                if (user == null)
                {
                    return new Result(false, "此用户不存在");
                }

                //修改用户启用状态
                try
                {
                    user.Enable = enable;
                    user.ModifiedUser = modifiedBy.ToString();
                    user.ModifiedDate = DateTime.Now;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return new Result(true, enable ? "启用成功" : "禁用成功");
                }
                catch (Exception ex)
                {
                    return new Result(false, ex.Message);
                }
            }
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userJsonStr">序列化的用户</param>
        /// <param name="modifiedBy">修改人</param>
        /// <returns></returns>
        public static Result CreateUser(string userJsonStr, int modifiedBy)
        {
            using (CRMEntities db = new CRMEntities())
            {
                try
                {

                    
                    var user = JsonHelper.Deserialize<TM_AUTH_User>(userJsonStr);

                    if (string.IsNullOrEmpty(user.LoginName))
                    {
                        return new Result(false, "用户登录名不能为空");
                    }
                    var count = db.TM_AUTH_User.Where(o => o.LoginName.Equals(user.LoginName)).Count();
                    if (count > 0)
                    {
                        return new Result(false, "用户登录名已存在");
                    }

                    

                    user.Password = Utility.ToolsHelper.MD5(user.Password);
                    user.Enable = true;
                    user.AddedUser = modifiedBy.ToString();
                    user.AddedDate = DateTime.Now;
                    user.ModifiedUser = modifiedBy.ToString();
                    user.ModifiedDate = DateTime.Now;
                    db.TM_AUTH_User.Add(user);
                    db.SaveChanges();

                    var user_latter = db.TM_AUTH_User.Where(o => o.LoginName.Equals(user.LoginName));
                    var deps = db.TR_AUTH_UserBusinessDepartment.Where(p => p.UserID.Equals(modifiedBy));
                    foreach (var dep in deps.ToList())
                    {
                        var dep_new = new TR_AUTH_UserBusinessDepartment();
                        dep_new.UserID = user_latter.ToList()[0].UserID;
                        dep_new.DepartmentID = dep.DepartmentID;
                        db.TR_AUTH_UserBusinessDepartment.Add(dep_new);
                        
                    }

                    db.SaveChanges();


                    return new Result(true, "用户添加成功");
                }
                catch (Exception ex)
                {
                    return new Result(false, ex.Message);
                }
            }
        }


        public static Result ChangeUserRoles(int userId, string rolesStr)
        {
            using (var db = new CRMEntities())
            {
                try
                {
                    var roles = JsonHelper.Deserialize<List<int>>(rolesStr);
                    var exit = db.TR_AUTH_UserRole.Where(o => o.UserID == userId).ToList();
                    foreach (var e in exit)
                    {
                        if (!roles.Contains(e.RoleID))
                        {
                            db.TR_AUTH_UserRole.Remove(e);
                        }
                    }
                    foreach (var r in roles)
                    {
                        if (exit.Where(o => o.RoleID == r).Count() < 1)
                        {
                            db.TR_AUTH_UserRole.Add(new TR_AUTH_UserRole { UserID = userId, RoleID = r });
                        }
                    }
                    db.SaveChanges();
                    return new Result(true, "用户角色保存成功");
                }
                catch (Exception ex)
                {
                    return new Result(false, ex.Message);
                }
            }
        }
        public static Result ChangeUserBusi(int userId, string busisStr)
        {
            using (var db = new CRMEntities())
            {
                try
                {
                    var busis = JsonHelper.Deserialize<List<int>>(busisStr);
                    var exit = db.TR_AUTH_UserBusinessDepartment.Where(o => o.UserID == userId).ToList();
                    foreach (var e in exit)
                    {
                        if (!busis.Contains(e.DepartmentID))
                        {
                            db.TR_AUTH_UserBusinessDepartment.Remove(e);
                        }
                    }
                    foreach (var r in busis)
                    {
                        if (exit.Where(o => o.DepartmentID == r).Count() < 1)
                        {
                            db.TR_AUTH_UserBusinessDepartment.Add(new TR_AUTH_UserBusinessDepartment { UserID = userId, DepartmentID = r });
                        }
                    }
                    db.SaveChanges();
                    return new Result(true, "业务部门分配成功");
                }
                catch (Exception ex)
                {
                    return new Result(false, ex.Message);
                }
            }
        }
        #region 角色管理
        public static Result GetRolesByPage(string dp, string roleName, string roleType, string roleState, int searchGroupId, int dataGroupId)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.TM_AUTH_Role
                            join btemp in db.TM_SYS_DataGroup on a.DataGroupID equals btemp.DataGroupID into bt
                            from b in bt.DefaultIfEmpty()
                            join c in db.V_Sys_DataGroupRelation on a.DataGroupID equals c.SubDataGroupID into cb
                            from ccg in cb.DefaultIfEmpty()
                            where ccg.DataGroupID == dataGroupId && (searchGroupId > 0 ? a.DataGroupID == searchGroupId : true)
                            select new
                            {
                                a.AddedDate,
                                a.AddedUser,
                                a.DataGroupID,
                                b.DataGroupName,
                                a.ModifiedDate,
                                a.ModifiedUser,
                                a.RoleID,
                                a.RoleName,
                                a.RoleType,
                                a.Enable
                            };
                if (!string.IsNullOrEmpty(roleName))
                    query = query.Where(o => o.RoleName.Contains(roleName));
                if (!string.IsNullOrEmpty(roleType))
                {
                    query = query.Where(o => o.RoleType == roleType);
                }
                if (roleState == "1")
                {
                    query = query.Where(o => o.Enable == true);
                }
                else if (roleState == "2")
                {
                    query = query.Where(o => o.Enable == false);
                }

                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }

        /// <summary>
        /// 跟据条件查找页面元素
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="roleId">角色ID</param>
        /// <param name="pageIds">页面ID列表</param>
        /// <returns></returns>
        public static Result GetElementsByCondition(string dp, int roleId, string pageIdsStr)
        {
            DatatablesParameter myDp = JsonHelper.Deserialize<DatatablesParameter>(dp);
            var pageIds = JsonHelper.Deserialize<List<int>>(pageIdsStr);
            using (CRMEntities db = new CRMEntities())
            {
                var query = from pe in db.TD_SYS_PageElement
                            join p in db.TD_SYS_Page on pe.PageID equals p.PageID
                            join r in
                                (from r in db.TM_AUTH_Role
                                 join e in db.TM_AUTH_RolePageElementSettings on r.RoleID equals e.RoleID
                                 where (roleId == null ? true : e.RoleID == roleId)
                                 select new { PageID = e.PageID, RoleID = r.RoleID, RoleName = r.RoleName, ElementKey = e.ElementKey, SettingProp = e.SettingProp, SettingCss = e.SettingCss })
                            on new { pe.PageID, pe.ElementKey } equals new { r.PageID, r.ElementKey }
                            into rp
                            from r in rp.DefaultIfEmpty()
                            where pageIds.Contains(pe.PageID) && p.Enable == true
                            orderby p.Sort
                            select new
                            {
                                RoleID = r.RoleID == null ? -1 : r.RoleID,
                                PageID = p.PageID,
                                PageName = p.PageDesc,
                                ElementText = pe.ElementText == null ? "" : pe.ElementText,
                                ElementKey = pe.ElementKey,
                                SettingProp = r.SettingProp,
                                SettingCss = pe.SettingCss == null ? "" : pe.SettingCss,
                                SettedCss = r.SettingCss == null ? "" : r.SettingCss
                            };

                return new Result(true, "", new List<object> { query.ToDataTableSourceVsPage(myDp) });
            }
        }

        /// <summary>
        /// 获取所有页面列表
        /// </summary>
        /// <param name="enable">是否可用</param>
        /// <param name="visible">是否显示</param>
        /// <returns></returns>
        public static Result GetAllPages(bool? enable, bool? visible)
        {
            using (var db = new CRMEntities())
            {
                var result = from a in db.TD_SYS_Page
                             select a;
                if (enable.HasValue)
                {
                    result = result.Where(o => o.Enable == enable);
                }
                if (visible.HasValue)
                {
                    result = result.Where(o => o.Visable == visible);
                }
                return new Result(true, "", result.ToList());
            }
        }

        /// <summary>
        /// 根据ID获取角色基本信息
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public static Result GetRoleInfoById(int roleId)
        {
            using (var db = new CRMEntities())
            {
                var result = from a in db.TM_AUTH_Role
                             where a.RoleID == roleId
                             select new
                             {
                                 a.RoleID,
                                 a.RoleName,
                                 a.RoleType,
                                 a.DataGroupID,
                                 a.Enable,
                                 PageIds = db.TR_AUTH_RolePage.Where(o => o.RoleID == roleId).Select(o => o.PageID),
                                 a.EnableDashBoard,
                                 a.DefaultPath
                             };
                return new Result(true, "", result.FirstOrDefault());
            }
        }

        /// <summary>
        /// 保存页面角色
        /// </summary>
        /// <param name="roleJsonStr">角色对象序列化</param>
        /// <param name="pageIdsStr">页面ID列表序列化</param>
        /// <param name="elementsJsonStr">页面元素列表序列化</param>
        /// <param name="modifiedBy">操作人ID</param>
        /// <returns></returns>
        public static Result SavePageRole(string roleJsonStr, string pageIdsStr, string elementsJsonStr, int modifiedBy)
        {
            try
            {
                List<int> pageIds = JsonHelper.Deserialize<List<int>>(pageIdsStr);
                var role = JsonHelper.Deserialize<TM_AUTH_Role>(roleJsonStr);
                List<TM_AUTH_RolePageElementSettings> elements = null;
                if (pageIds != null)
                {
                    elements = JsonHelper.Deserialize<List<TM_AUTH_RolePageElementSettings>>(elementsJsonStr);
                }
                if (role != null)
                {
                    using (var db = new CRMEntities())
                    {
                        if (role.RoleID == 0)//新增角色
                        {
                            role.RoleType = Utility.AppConst.ROLE_TYPE_PAGE;
                            role.AddedDate = DateTime.Now;
                            role.AddedUser = modifiedBy.ToString();
                            role.ModifiedDate = DateTime.Now;
                            role.ModifiedUser = modifiedBy.ToString();
                            role.EnableDashBoard = role.EnableDashBoard;
                            role.DefaultPath = role.DefaultPath;
                            db.TM_AUTH_Role.Add(role);
                        }
                        else//修改角色信息
                        {
                            var model = db.TM_AUTH_Role.Where(o => o.RoleID == role.RoleID).FirstOrDefault();
                            if (model == null)
                            {
                                return new Result(false, "角色不存在，请刷新后重试");
                            }
                            model.DataGroupID = role.DataGroupID;
                            model.Enable = role.Enable;
                            model.ModifiedDate = DateTime.Now;
                            model.ModifiedUser = modifiedBy.ToString();
                            model.RoleID = role.RoleID;
                            model.RoleName = role.RoleName;
                            model.RoleType = Utility.AppConst.ROLE_TYPE_PAGE;
                            model.EnableDashBoard = role.EnableDashBoard;
                            model.DefaultPath = role.DefaultPath;
                            db.Entry(model).State = EntityState.Modified;
                        }
                        db.SaveChanges();

                        int roleId = role.RoleID;
                        //删除原有角色页面关系
                        var rps = db.TR_AUTH_RolePage.Where(o => o.RoleID == roleId).ToList();
                        if (rps != null && rps.Count > 0)
                        {
                            foreach (var rp in rps)
                            {
                                db.TR_AUTH_RolePage.Remove(rp);
                            }
                        }
                        //新建角色页面关系
                        if (pageIds != null && pageIds.Count > 0)
                        {
                            foreach (var pid in pageIds)
                            {
                                var rolePage = new TR_AUTH_RolePage();
                                rolePage.RoleID = roleId;
                                rolePage.PageID = pid;
                                db.TR_AUTH_RolePage.Add(rolePage);
                            }
                        }
                        db.SaveChanges();

                        //删除原有角色页面元素关系
                        var res = db.TM_AUTH_RolePageElementSettings.Where(o => o.RoleID == roleId).ToList();
                        if (res != null && res.Count > 0)
                        {
                            foreach (var re in res)
                            {
                                db.TM_AUTH_RolePageElementSettings.Remove(re);
                            }
                        }
                        //新建角色页面元素关系
                        if (elements != null && elements.Count > 0)
                        {
                            foreach (var element in elements)
                            {
                                element.RoleID = roleId;
                                db.TM_AUTH_RolePageElementSettings.Add(element);
                            }
                        }
                        db.SaveChanges();

                        return new Result(true, "角色保存成功");
                    }
                }
                else
                {
                    return new Result(false, "角色对象不能为空");
                }
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        /// <summary>
        /// 根据条件获取BaseData
        /// </summary>
        /// <param name="dataGroupId">数据群组ID</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static Result GetBaseDataByCondition(int dataGroupId, string type)
        {
            using (var db = new CRMEntities())
            {
                var t = from s in db.V_M_TM_SYS_BaseData_store
                        select s;
                var query = from a in db.TM_SYS_BaseData
                            join b in db.V_Sys_DataGroupRelation on a.DataGroupID equals b.SubDataGroupID
                            where a.BaseDataType.Equals(type) && b.DataGroupID == dataGroupId
                            select a;
                return new Result(true, "", query.ToList());
            }
        }

        /// <summary>
        /// 保存数据角色
        /// </summary>
        /// <param name="roleJsonStr">数据角色对象序列化</param>
        /// <param name="limitType">维度</param>
        /// <param name="limitValuesStr">维度值列表序列化</param>
        /// <param name="pageIdsStr">指定页面ID列表序列化</param>
        /// <param name="modifiedBy">操作人ID</param>
        /// <returns></returns>
        public static Result SaveDataRole(string roleJsonStr, string limitType, string limitValuesStr, string pageIdsStr, int modifiedBy)
        {
            try
            {
                List<int> pageIds = JsonHelper.Deserialize<List<int>>(pageIdsStr);
                List<string> limitValues = JsonHelper.Deserialize<List<string>>(limitValuesStr);
                var role = JsonHelper.Deserialize<TM_AUTH_Role>(roleJsonStr);
                if (role != null)
                {
                    using (var db = new CRMEntities())
                    {
                        if (role.RoleID == 0)//新增角色
                        {
                            role.RoleType = Utility.AppConst.ROLE_TYPE_DATA;
                            role.AddedDate = DateTime.Now;
                            role.AddedUser = modifiedBy.ToString();
                            role.ModifiedDate = DateTime.Now;
                            role.ModifiedUser = modifiedBy.ToString();
                            db.TM_AUTH_Role.Add(role);
                        }
                        else//修改角色信息
                        {
                            var model = db.TM_AUTH_Role.Where(o => o.RoleID == role.RoleID).FirstOrDefault();
                            if (model == null)
                            {
                                return new Result(false, "角色不存在，请刷新后重试");
                            }
                            model.DataGroupID = role.DataGroupID;
                            model.Enable = role.Enable;
                            model.ModifiedDate = DateTime.Now;
                            model.ModifiedUser = modifiedBy.ToString();
                            model.RoleID = role.RoleID;
                            model.RoleName = role.RoleName;
                            model.RoleType = Utility.AppConst.ROLE_TYPE_DATA;
                            db.Entry(model).State = EntityState.Modified;
                        }
                        db.SaveChanges();

                        string roleId = role.RoleID.ToString();
                        //删除原有DataLimit数据
                        var dls = db.TM_AUTH_DataLimit.Where(o => o.HierarchyType.Equals(Utility.AppConst.HIRERARCHY_TYPE_ROLE) && o.HierarchyValue.Equals(roleId)).ToList();
                        if (dls != null && dls.Count > 0)
                        {
                            foreach (var dl in dls)
                            {
                                db.TM_AUTH_DataLimit.Remove(dl);
                            }
                        }
                        //新建DataLimit数据
                        if (limitValues != null && limitValues.Count > 0)
                        {
                            if (pageIds == null || pageIds.Count < 1)
                            {
                                pageIds = new List<int> { 9999 };
                            }
                            foreach (var value in limitValues)
                            {
                                foreach (var pageId in pageIds)
                                {
                                    var limit = new TM_AUTH_DataLimit();
                                    limit.HierarchyType = Utility.AppConst.HIRERARCHY_TYPE_ROLE;
                                    limit.HierarchyValue = roleId;
                                    limit.RangeType = limitType;
                                    limit.RangeValue = value;
                                    limit.PageID = pageId;
                                    limit.Direction = Utility.AppConst.DATA_LIMIT_DIRECTION;
                                    limit.AddedDate = DateTime.Now;
                                    limit.AddedUser = modifiedBy.ToString();
                                    limit.ModifiedDate = DateTime.Now;
                                    limit.ModifiedUser = modifiedBy.ToString();
                                    db.TM_AUTH_DataLimit.Add(limit);
                                }
                            }
                        }
                        db.SaveChanges();

                        return new Result(true, "角色保存成功");
                    }
                }
                else
                {
                    return new Result(false, "角色对象不能为空");
                }
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        /// <summary>
        /// 根据ID获取数据角色信息
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public static Result GetDataRoleInfoById(int roleId)
        {
            using (var db = new CRMEntities())
            {
                string roleIdStr = roleId.ToString();
                var role = db.TM_AUTH_Role.Where(o => o.RoleID == roleId).FirstOrDefault();
                if (role != null)
                {
                    var limits = db.TM_AUTH_DataLimit.Where(o => o.HierarchyType.Equals(Utility.AppConst.HIRERARCHY_TYPE_ROLE) && o.HierarchyValue == roleIdStr).ToList();
                    string limitType = limits.Select(o => o.RangeType).Distinct().FirstOrDefault();
                    List<string> limitValues = limits.Select(o => o.RangeValue).Distinct().ToList();
                    List<int> pageIds = limits.Select(o => o.PageID).Distinct().ToList();
                    //如果维度的值为空，则设为默认值
                    var options = GetBaseDataByCondition(!role.DataGroupID.HasValue ? 0 : role.DataGroupID.Value, limitType);
                    return new Result(true, "", new List<object> { role, limitType ?? "", limitValues ?? new List<string>(), pageIds ?? new List<int>(), options.Obj[0] });
                }
                else
                {
                    return new Result(false, "没有找到此角色信息，请刷新后重试");
                }
            }
        }

        /// <summary>
        /// 根绝ID删除角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public static Result DeleteRoleById(int roleId)
        {
            using (var db = new CRMEntities())
            {
                try
                {
                    //判断角色是否存在
                    var role = db.TM_AUTH_Role.Where(o => o.RoleID == roleId).FirstOrDefault();
                    if (role == null)
                    {
                        return new Result(false, "此角色不存在，请刷新后重试");
                    }
                    //判断是否有关联用户
                    var count = db.TR_AUTH_UserRole.Where(o => o.RoleID == roleId).Count();
                    if (count > 0)
                    {
                        return new Result(false, "此角色有关联用户，不能删除");
                    }
                    //删除页面角色关联数据
                    if (role.RoleType.Equals(Utility.AppConst.ROLE_TYPE_PAGE))
                    {
                        var rpes = db.TM_AUTH_RolePageElementSettings.Where(o => o.RoleID == roleId).ToList();
                        foreach (var rpe in rpes)
                        {
                            db.TM_AUTH_RolePageElementSettings.Remove(rpe);
                        }
                        var rps = db.TR_AUTH_RolePage.Where(o => o.RoleID == roleId).ToList();
                        foreach (var rp in rps)
                        {
                            db.TR_AUTH_RolePage.Remove(rp);
                        }
                    }
                    //删除数据角色关联数据
                    else if (role.RoleType.Equals(Utility.AppConst.ROLE_TYPE_DATA))
                    {
                        var roleIdStr = roleId.ToString();
                        var limits = db.TM_AUTH_DataLimit.Where(o => o.HierarchyType == Utility.AppConst.HIRERARCHY_TYPE_ROLE && o.HierarchyValue == roleIdStr).ToList();
                        foreach (var limit in limits)
                        {
                            db.TM_AUTH_DataLimit.Remove(limit);
                        }
                    }
                    //删除角色
                    db.TM_AUTH_Role.Remove(role);
                    db.SaveChanges();
                    return new Result(true, "角色删除成功");
                }
                catch (Exception ex)
                {
                    return new Result(false, ex.Message);
                }
            }
        }
        #endregion
    }
}
