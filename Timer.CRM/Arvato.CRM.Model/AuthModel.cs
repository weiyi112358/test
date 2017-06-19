using Arvato.CRM.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    [Serializable]
    public class AuthModel
    {
        public int UserID { get; set; }
        public string UserDisplayName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? DataGroupID { get; set; }
        public string Language { get; set; }
        public int? CurPageID { get; set; }
        public List<Pages> Pages { get; set; }
        public List<int> RoleIDs { get; set; }
        public List<PageElementModel> PageElements { get; set; }
        public string DefaultPath { get; set; }
        public bool? EnableDashBoard { get; set; }
        public string StoreCode { get; set; }
        public string BrandCode { get; set; }
        public string AreaCode { get; set; }
        public int DataGroupGrade { get; set; }
        public List<string> StoreCodes { get; set; }
        public List<string> Brands { get; set; }
        public List<int> GroupIDs { get; set; }
        public List<LimitData> LimitData { get; set; }
    }


    public class LimitData
    {
        public string LimitType { get; set; }
        public string LimitValue { get; set; }
    }


    [Serializable]
    public class Pages
    {
        public int PageID { get; set; }
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string DispName { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public string Nav { get; set; }
        public short MenuSort { get; set; }
        public short PageSort { get; set; }
        public bool Visable { get; set; }
    }

    [Serializable]
    public class UserProfileModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public int? DataGroupID { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }

    [Serializable]
    public class ChangePasswordModel
    {
        public int UserID { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// 页面元素
    /// </summary>
    [Serializable]
    public class PageElementModel
    {
        public int RoleID { get; set; }
        public int PageID { get; set; }
        public string PageName { get; set; }
        public string ElementText { get; set; }
        public string ElementKey { get; set; }
        public string SettingProp { get; set; }
        public string SettingCss { get; set; }
        public string SettedCss { get; set; }
    }
}
