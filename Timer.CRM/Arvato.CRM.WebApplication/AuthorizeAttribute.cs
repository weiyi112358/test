using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Arvato.CRM.WebApplication
{
    /// <summary>
    /// 权限验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private string controller
        {
            get
            {
                 return HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString().ToLower();
            }
        }

        private string action
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString().ToLower();
            }
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null) throw new ArgumentNullException("filterContext");
            if (controller == "home" && action == "login") return;
            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
            {
                // If a child action cache block is active, we need to fail immediately, even if authorization
                // would have succeeded. The reason is that there's no way to hook a callback to rerun
                // authorization before the fragment is served from the cache, so we can't guarantee that this
                // filter will be re-run on subsequent requests.
                throw new InvalidOperationException("AuthorizeAttribute cannot be used within a child action caching block.");
            }

            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(HttpPostAttribute), inherit: true)
                                      || filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
                                      || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);

            if (skipAuthorization) return;
            if (filterContext.HttpContext.Session[Utility.AppConst.SESSION_AUTH] == null) HandleUnauthorizedRequest(filterContext);//用户没有登录
            else if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                return;
            }
            else
            {
                var auth = JsonHelper.Deserialize<AuthModel>((filterContext.HttpContext.Session[AppConst.SESSION_AUTH] ?? "").ToString());
                var path = filterContext.HttpContext.Request.Path.ToLower().TrimStart(new[] { '/', '\\' });
                if (!string.IsNullOrWhiteSpace(path) && !Utility.AppConst.NO_AUTH_PAGES.Contains(path) && !auth.Pages.Any(p => ("/" + path).StartsWith(p.Path.ToLower()))) filterContext.Result = new RedirectResult("/");
                else
                {
                    var pp = auth.Pages.FirstOrDefault(p => ("/" + path).StartsWith(p.Path.ToLower()));
                    if (pp != null)
                    {
                        auth.CurPageID = pp.PageID;
                        filterContext.HttpContext.Session[AppConst.SESSION_AUTH] = JsonHelper.Serialize(auth);
                    }
                }
                
            }
        }

        /// <summary>
        /// 未登录页面
        /// </summary>
        /// <param name="filterContext"></param>
        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var res = new Result(false, "系统登陆超时", new[] { new { Lexp_LoginTimeout = "系统登陆超时" } });
                filterContext.Result = new NewJsonResult(
                    res,
                    filterContext.HttpContext.Request.ContentType,
                    filterContext.HttpContext.Request.ContentEncoding,
                    JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Returns HTTP 401 - see comment in HttpUnauthorizedResult.cs.
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}