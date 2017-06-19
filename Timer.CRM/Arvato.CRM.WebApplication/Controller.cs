using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arvato.CRM.BizContract;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Arvato.CRM.WcfFramework.ClientProxy;
using Arvato.CRM.Utility.Datatables;

namespace Arvato.CRM.WebApplication
{
    public class Controller : System.Web.Mvc.Controller
    {
        /// <summary>
        /// 当前用户信息
        /// </summary>
        protected AuthModel Auth
        {
            get
            {
                var _auth = JsonHelper.Deserialize<AuthModel>((Session[Utility.AppConst.SESSION_AUTH] ?? "").ToString());
                if (_auth == null) throw new Exception(Utility.AppConst.MSG_SESSION_LOST);
                return _auth;
            }
            set
            {
                Session[Utility.AppConst.SESSION_AUTH] = value;
            }
        }

        /// <summary>
        /// 提交业务层处理方法
        /// </summary>
        /// <param name="domain">业务层类名</param>
        /// <param name="action">业务层方法名</param>
        /// <param name="needSession">是否需要携带Session一起提交</param>
        /// <param name="args">参数数组</param>
        /// <returns>Result类实例</returns>
        protected Result Submit(string domain, string action, bool needSession, params object[] args)
        {
            using (var svc = new ServiceManager<ICommonContract>())
            {
                if (!needSession)
                {
                     return Utility.JsonHelper.Deserialize<Result>(svc.Service.CallFunction(domain, action, args));
                }
                else
                {   
                    var rst = Utility.JsonHelper.Deserialize<Result>(svc.Service.CallFunctionWithSession(domain, action, JsonHelper.Serialize(getSessions()), args));
                    foreach (var s in rst.Sessions)
                    {
                        Session[s.Key] = s.Value;
                    }
                    return rst;
                }
            }
        }

        protected JsonResult GenerateModelList(DatatablesSourceVsPage result, DatatablesParameter model)
        {
            if (result.iTotalRecords > 0)
            {
                return Json(new
                {
                    iDisplayStart = result.iDisplayStart,
                    iDisplayLength = result.iDisplayLength,
                    iTotalRecords = result.iTotalRecords,
                    iTotalDisplayRecords = result.iTotalDisplayRecords,
                    aaData = result.aaData
                });

            }
            else
            {
                return DefaultNullModelList(model);
            }
        }

        protected JsonResult GenerateAssignedModelList<T>(DatatablesSourceVsPage result, DatatablesParameter model)
        {
            if (result.iTotalRecords > 0)
            {
                return Json(new
                {
                    iDisplayStart = result.iDisplayStart,
                    iDisplayLength = result.iDisplayLength,
                    iTotalRecords = result.iTotalRecords,
                    iTotalDisplayRecords = result.iTotalDisplayRecords,
                    aaData = JsonHelper.Deserialize<T>(JsonHelper.Serialize(result.aaData))
                });

            }
            else
            {
                return DefaultNullModelList(model);
            }
        }

        protected JsonResult DefaultNullModelList(DatatablesParameter model)
        {
            return Json(new
            {
                iDisplayStart = model.iDisplayStart,
                iDisplayLength = model.iDisplayLength,
                iTotalRecords = 0,
                iTotalDisplayRecords = 0,
                aaData = ""
            });
        }

        private Dictionary<string, object> getSessions()
        {
            var d = new Dictionary<string, object>();
            foreach (var s in HttpContext.Session.Keys)
            {
                d.Add(s.ToString(), HttpContext.Session[s.ToString()]);
            }
            return d;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception.Message == Utility.AppConst.MSG_SESSION_LOST)
            {
                Response.Redirect(Utility.AppConst.URL_ENTRY);
            }
            base.OnException(filterContext);
        }

        /// <summary>
        /// 重载Json方法
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="contentEncoding"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new NewJsonResult(data, contentType, contentEncoding, behavior);
        }
    }

    /// <summary>
    /// 自定义JsonResult
    /// </summary>
    public class NewJsonResult : JsonResult
    {
        public NewJsonResult(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            this.Data = data;
            this.ContentType = contentType;
            this.ContentEncoding = contentEncoding;
            this.JsonRequestBehavior = behavior;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet && String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet.");
            }

            HttpResponseBase response = context.HttpContext.Response;
            if (context.HttpContext.Request.UserAgent.ToLower().Contains("msie"))
            {
                response.ContentType = "text/json";
            }
            else
            {
                response.ContentType = ContentType ?? "application/json";
            }
            response.ContentEncoding = ContentEncoding ?? System.Text.Encoding.UTF8;
            response.Status = "200 OK";
            response.StatusCode = 200;

            if (Data != null)
            {
                if (Data.GetType() == typeof(string)) response.Write(Data.ToString());
                else
                {
                    var settings = new Newtonsoft.Json.JsonSerializerSettings();
                    settings.DateFormatString = "yyyy-MM-dd";
                    response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(Data, settings));
                }
            }
        }
    }
}