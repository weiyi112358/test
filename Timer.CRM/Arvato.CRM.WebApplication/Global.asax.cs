using Arvato.CRM.Utility;
using Arvato.CRM.WebApplication.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Arvato.CRM.WebApplication
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            Log.WriteFormatLog("CRM_Portal_Error", ex.Message);
            var httpContext = ((MvcApplication)sender).Context;

            httpContext.ClearError();
            httpContext.Response.Clear();
            Server.ClearError();
            
            if ((new HttpRequestWrapper(httpContext.Request)).IsAjaxRequest())
            {
                httpContext.Response.Clear();
                httpContext.Response.ContentType = "application/json; charset=utf-8";
                httpContext.Response.StatusCode = 500;
                httpContext.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(
                   new Result(false, ex.Message)));
                httpContext.Response.Flush();
                return;
            }

            var controller = new ErrorController();
            controller.ViewBag.ErrorMsg = ex.Message;
            var routeData = new RouteData();
            routeData.Values["controller"] = "/Error";
            routeData.Values["action"] = "Index";


            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }
    }
}