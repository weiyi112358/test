using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Utility
{
    public static class ToolsHelper
    {
        private static System.Security.Cryptography.MD5 md5 = new MD5CryptoServiceProvider();

        #region GUIDtoString
        public static string GuidtoString(Guid guid)
        {
            return guid.ToString().Replace("-","");
        }
        #endregion

        #region
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
        #endregion

        public static T MargeObject<T>(T o1, T o2) where T : new()
        {
            T t1 = new T();
            var properties = typeof(T).GetProperties();
            foreach (var p in properties)
            {
                object pValue = p.GetValue(o2, null);
                object pValue2 = p.GetValue(o1);
                var c = p.DeclaringType;
                if (pValue != null)
                {
                    p.SetValue(t1, pValue, null);
                }
                else
                {
                    p.SetValue(t1, pValue2, null);
                }
            }
            return t1;
        }

        #region DateTime和时间戳的转换

        //   DateTime时间格式转换为Unix时间戳格式
        public static int DateTimeToStamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        // 时间戳转为C#格式时间
        public static DateTime StampToDateTime(string timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);

            return dateTimeStart.Add(toNow);
        }
        #endregion
    }
}
