using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Arvato.CRM.CommunicateEngine
{
    public     class WechatService
    {
        public   string ScretKey { get; set; }

        public   string WechatAPI { get; set; }

        /// <summary>
        /// 群发
        /// </summary>
        /// <param name="wechatContent"></param>
        /// <returns></returns>
        public async Task<string> SendWechatMass(ContentsDetail wechatContent)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                 
                var content = JsonConvert.SerializeObject(wechatContent);
                var md5Content = md5(content);
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("sign", md5Content);
                dic.Add("content", content);
                FormUrlEncodedContent contentForm = new System.Net.Http.FormUrlEncodedContent(dic.ToList());


                var result = await client.PostAsync(WechatAPI, contentForm);

                return await result.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Log.ServiceLog("SendWechatMass Request", System.Diagnostics.EventLogEntryType.Error, ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// 积分
        /// </summary>
        /// <param name="wechatContent"></param>
        /// <returns></returns>
        public async Task<string> SendWechatPoint(ContentDetail wechatContent)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                 
                var content = JsonConvert.SerializeObject(wechatContent);
                var md5Content = md5(content);
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("sign", md5Content);
                dic.Add("content", content);
                FormUrlEncodedContent contentForm = new System.Net.Http.FormUrlEncodedContent(dic.ToList());

                var result = await client.PostAsync(WechatAPI + "/template/send_template", contentForm);

                return await result.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Log.ServiceLog("SendWechatPoint Request", System.Diagnostics.EventLogEntryType.Error, ex.ToString());
                throw;
            }
        }

        private   string md5(string content )
        {
            using (var md5 = MD5.Create())
            {
                var md5data = md5.ComputeHash(Encoding.UTF8.GetBytes(content + ScretKey));
                StringBuilder sBuilder = new StringBuilder();
                foreach (var c in md5data)
                    sBuilder.Append(c.ToString("x2"));
                return sBuilder.ToString();
            }
        }
    }
}
