using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Utility
{
    public class AppConst
    {
        public const string SESSION_AUTH = "auth";

        public const string URL_ENTRY = "/Home/Login";
        public const string URL_DEFAULT = "/Home/Index";

        public const string MSG_SESSION_LOST = "SessionLost";

        public const string APP_SETTING_SITE_TITLE = "SiteTitle";
        public const string APP_SETTING_HAS_SESSION = "crm_hasSession";
        public const string APP_SETTING_ACTION_KEY = "crm_action";
        public const string APP_SETTING_ACTION = "action";
        public const string APP_SETTING_HAS_SESSION_KEY = "hasSession";

        public const char SPLIT_WORD = '_';
        public const string PREFIX = "crm";

        public const string QUERY_STRING_PAGEID = "PageID";

        public const string HIRERARCHY_TYPE_ROLE = "role";
        public const int DATA_LIMIT_DIRECTION = 1;

        public const string ROLE_TYPE_PAGE = "page";
        public const string ROLE_TYPE_DATA = "data";

        public static string[] NO_AUTH_PAGES = new string[] { "home", "home/index", "home/login", "auth/myprofile", "auth/changepassword" };
    }
}
