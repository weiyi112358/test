using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Arvato.CRM.CommunicateEngine
{
    public class QiXingSMS
    {
        public string SPPId { get; set; }
        public string SPPwd { get; set; }
        public string SPUrl { get; set; }
        public QiXingSMS(string spid, string sppsw, string spurl)
        {
            SPPId = spid;
            SPPwd = sppsw;
            SPUrl = spurl;
        }

        public string SendSms(string Mobile, string Content)
        {
            string strhex = encodeHexStr(15, Content);
            StringBuilder sbTemp = new StringBuilder();
            sbTemp.Append("command=MT_REQUEST&spid=" + SPPId + "&sppassword=" + SPPwd + "&da=" + Mobile + "&dc=15&sm=" + strhex);
            byte[] bTemp = Encoding.ASCII.GetBytes(sbTemp.ToString());
            return doPostRequest(SPUrl, bTemp);
        }
        /// <summary>
        /// 批量发相同内容的短信
        /// </summary>
        /// <param name="MobileLst"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        public string SendSMSBatch(List<string> MobileLst, string Content)
        {
            try
            {
                TimerSMSWebService.wmgwSoapClient ws = new TimerSMSWebService.wmgwSoapClient();
                //LiNingSMSWebService.wmgwSoapClient ws = new LiNingSMSWebService.wmgwSoapClient();
                var res = ws.MongateCsSpMultixMtSend(SPPId, SPPwd, "");
                return res;
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("调用WebService出错" + ex.ToString());
                return "false";
            }

        }

        /// <summary>
        /// 批量发不同内容的短信
        /// </summary>
        /// <param name="sms"></param>
        /// <returns></returns>
        public string SendSMSBatchX(string sms)
        {
            try
            {
                //LiNingSMSWebService.wmgwSoapClient ws = new LiNingSMSWebService.wmgwSoapClient();
                TimerSMSWebService.wmgwSoapClient ws = new TimerSMSWebService.wmgwSoapClient();
                var res = ws.MongateCsSpMultixMtSend(SPPId, SPPwd, sms);
                return res;
            }
            catch (Exception ex)
            {
                Log4netHelper.WriteErrorLog("调用WebService出错" + ex.ToString());
                return "false";
            }
        }

        #region Function
        //字符编码成HEX
        private static String encodeHexStr(int dataCoding, String realStr)
        {
            string strhex = "";
            try
            {
                Byte[] bytSource = null;
                if (dataCoding == 15)
                {
                    bytSource = Encoding.GetEncoding("GBK").GetBytes(realStr);
                }
                else if (dataCoding == 8)
                {
                    bytSource = Encoding.BigEndianUnicode.GetBytes(realStr);
                }
                else
                {
                    bytSource = Encoding.ASCII.GetBytes(realStr);
                }
                for (int i = 0; i < bytSource.Length; i++)
                {
                    strhex = strhex + bytSource[i].ToString("X2");

                }
            }
            catch (System.Exception err)
            {
                Log.ServiceLog("SmsLog Decode", System.Diagnostics.EventLogEntryType.Error, err.ToString());
            }
            return strhex;
        }
        //hex编码还原成字符
        private static String decodeHexStr(int dataCoding, String hexStr)
        {
            String strReturn = "";
            try
            {
                int len = hexStr.Length / 2;
                byte[] bytSrc = new byte[len];
                for (int i = 0; i < len; i++)
                {
                    string s = hexStr.Substring(i * 2, 2);
                    bytSrc[i] = Byte.Parse(s, System.Globalization.NumberStyles.AllowHexSpecifier);
                }

                if (dataCoding == 15)
                {
                    strReturn = Encoding.GetEncoding("GBK").GetString(bytSrc);
                }
                else if (dataCoding == 8)
                {
                    strReturn = Encoding.BigEndianUnicode.GetString(bytSrc);
                }
                else
                {
                    strReturn = System.Text.ASCIIEncoding.ASCII.GetString(bytSrc);
                }
            }
            catch (System.Exception err)
            {
                Log.ServiceLog("SmsLog Decode", System.Diagnostics.EventLogEntryType.Error, err.ToString());
            }
            return strReturn;
        }



        //推送短信  
        private static String doPostRequest(string url, byte[] bData)
        {
            System.Net.HttpWebRequest hwRequest;
            System.Net.HttpWebResponse hwResponse;

            string strResult = string.Empty;
            try
            {
                hwRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                hwRequest.Timeout = 5000;
                hwRequest.Method = "POST";
                hwRequest.ContentType = "application/x-www-form-urlencoded";
                hwRequest.ContentLength = bData.Length;

                System.IO.Stream smWrite = hwRequest.GetRequestStream();
                smWrite.Write(bData, 0, bData.Length);
                smWrite.Close();
            }
            catch (System.Exception err)
            {
                Log.ServiceLog("SmsLog Request", System.Diagnostics.EventLogEntryType.Error, err.ToString());
                return strResult;
            }

            //get response
            try
            {
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.ASCII);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch (System.Exception err)
            {
                Log.ServiceLog("SmsLog Response", System.Diagnostics.EventLogEntryType.Error, err.ToString());
            }

            return strResult;
        }
        //GET方式发送得结果
        private static String doGetRequest(string url)
        {
            HttpWebRequest hwRequest;
            HttpWebResponse hwResponse;

            string strResult = string.Empty;
            try
            {
                hwRequest = (System.Net.HttpWebRequest)WebRequest.Create(url);
                hwRequest.Timeout = 5000;
                hwRequest.Method = "GET";
                hwRequest.ContentType = "application/x-www-form-urlencoded";
            }
            catch (System.Exception err)
            {
                //WriteErrLog(err.ToString());
                return strResult;
            }

            //get response
            try
            {
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.ASCII);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch (System.Exception err)
            {
                //WriteErrLog(err.ToString());
            }

            return strResult;
        }

        private static void WriteErrLog(string strErr)
        {
            Log.ServiceLog("Send Result", strErr);
            System.Diagnostics.Trace.WriteLine(strErr);
        }

        #endregion
    }
}
