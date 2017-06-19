using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Security;
namespace Arvato.CRM
{
    /// <summary>
    /// 通用工具类
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// 获取会员端IP
        /// </summary>
        /// <returns></returns>
        public static string ClientIp()
        {
            var ip = HttpContext.Current.Request.UserHostAddress;
            if (ip == "::1") ip = "1.1.1.1";
            // string ip = string.Empty;
            //if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"])) ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //if (string.IsNullOrEmpty(ip)) ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return ip;
        }
        /// <summary>
        /// Server.PapPath()重写
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string MapPath(string path)
        {
            var currentDir = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~", currentDir);
            path = Path.GetFullPath(path);
            return path;
        }

        /// <summary>
        /// Serializes the specified object to a JSON string.
        /// </summary>
        /// <param name="data">The object to serialize.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string Serialize(object data)
        {
            var setting = new JsonSerializerSettings();
            setting.DateFormatString = "yyyy-MM-dd HH:mm";
            return JsonConvert.SerializeObject(data, setting);
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="data">The JSON to deserialize.</param>
        /// <returns>The deserialized object from the Json string</returns>
        public static T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        private static System.Security.Cryptography.MD5 md5 = new MD5CryptoServiceProvider();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string MD5(string source, Encoding encode = null)
        {
            encode = encode ?? Encoding.UTF8;
            byte[] s = md5.ComputeHash(encode.GetBytes(source));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                sBuilder.Append(s[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// 获取月份的最后一日
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns></returns>
        public static int LastDayOfMonth(int year, int month)
        {
            return new DateTime(year, month, 1).AddMonths(1).AddDays(-1).Day;
        }

        /// <summary>
        /// 将A对象转换为B对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(object src)
        {
            var type = typeof(T);
            var tgt = Activator.CreateInstance<T>();
            var props = type.GetProperties().Where(p => p.CanWrite);
            var sprops = src.GetType().GetProperties().Where(p => p.CanRead);
            foreach (var prop in props)
            {
                var sprop = sprops.FirstOrDefault(p => p.Name == prop.Name);
                if (sprop != null)
                {
                    var toval = sprop.GetValue(src, null);
                    if (toval != null)
                    {
                        //var tval = Convert.ChangeType(toval, prop.PropertyType);
                        prop.SetValue(tgt, toval);
                    }
                }
            }
            return tgt;
        }
    }
}
