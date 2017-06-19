using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Utility
{
    public class Log
    {
        #region 本地文件日志
        private static string ErrFileDir = @"C:\arvatoCRM\Log";

        private bool WriteLog(string name, string strbody)
        {
            strbody = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "  " + strbody.Replace("\r\n", "<br>") + "<br>";
            try
            {
                if (string.IsNullOrEmpty(ErrFileDir)) return false;
                string mm = System.DateTime.Now.ToString(@"yyyyMM");
                string date = System.DateTime.Now.ToString(@"yyyyMMdd");
                string hour = System.DateTime.Now.ToString(@"HH");
                date = mm + @"\" + date + @"\" + hour;
                string fileDir = ErrFileDir.TrimEnd('\\') + @"\" + date + @"\";
                string filepath = fileDir;// +name + ".htm";

                name = name + ".htm";

                return WriteLog(filepath, name, strbody);

            }
            catch
            {
                return false;
            }
        }

        private bool WriteIDOSLog(string name, string tradecode, string strbody)
        {
            strbody = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "  " + strbody.Replace("\r\n", "<br>") + "<br>";
            try
            {
                if (string.IsNullOrEmpty(ErrFileDir)) return false;
                string mm = System.DateTime.Now.ToString(@"yyyyMM");
                string date = System.DateTime.Now.ToString(@"yyyyMMdd");
                string hour = System.DateTime.Now.ToString(@"HH");
                
                string n = "";
                string filename = "";
                if (tradecode == "")
                {
                    n=@"IDOSMemberInterFace";
                    date = mm + @"\" + date + @"\" + n;
                    filename = name + ".htm";
                }
                else
                {
                    n=@"IDOSTradeInterFace";
                    date = mm + @"\" + date + @"\" + n;
                    filename = tradecode + ".htm";
                }
                string fileDir = ErrFileDir.TrimEnd('\\') + @"\" + date + @"\";
                string filepath = fileDir;// +name + ".htm";
                return WriteLog(filepath, filename, strbody);

            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建日志文件主函数
        /// </summary>
        /// <param name="filepath">文件路径＋文件名</param>
        /// <param name="strbody">文件内容</param>
        /// <returns></returns>
        private bool WriteLog(string filepath, string name, string strbody)
        {

            StreamWriter my_writer = null;

            try
            {
                if (Directory.Exists(filepath) == false)
                {
                    Directory.CreateDirectory(filepath);
                }
                //如果文件存在，则自动追加方式写
                my_writer = new StreamWriter(filepath + name, true, System.Text.Encoding.Default);
                my_writer.Write(strbody);
                my_writer.Flush();
                return true;

            }
            catch
            {
                return false;
            }
            finally
            {
                if (my_writer != null)
                    my_writer.Close();
            }
        }

        public Log()
        {
            if (ConfigurationManager.AppSettings["ErrorLogPath"] != null)
                ErrFileDir = ConfigurationManager.AppSettings["ErrorLogPath"];
        }

        public static bool WriteFormatLog(string filename, string strbody, params object[] arg)
        {
            try
            {
                string error = string.Empty;
                if (arg != null && arg.Length > 0)
                    error = string.Format(strbody, arg);
                else
                    error = strbody;
                return new Log().WriteLog(filename, error);
            }
            catch (Exception ex)
            {
                new Log().WriteLog(filename, string.Format("异常{0} 堆栈{1} 日志内容{2} 参数{3}", ex.Message, ex.StackTrace, strbody, arg.Length));
                return false;
            }
        }

        public static bool WriteIDOSFormatLog(string filename, string tradecode, string strbody, params object[] arg)
        {
            try
            {
                string error = string.Empty;
                if (arg != null && arg.Length > 0)
                    error = string.Format(strbody, arg);
                else
                    error = strbody;
                return new Log().WriteIDOSLog(filename, tradecode, error);
            }
            catch (Exception ex)
            {
                new Log().WriteIDOSLog(filename,tradecode, string.Format("异常{0} 堆栈{1} 日志内容{2} 参数{3}", ex.Message, ex.StackTrace, strbody, arg.Length));
                return false;
            }
        }
        #endregion

        #region 服务日志
        public static void ServiceLog(string message)
        {
            ServiceLog("YaXia", EventLogEntryType.Information, message);
        }

        public static void ServiceLog(string name, string message)
        {
            ServiceLog(name, EventLogEntryType.Information, message);
        }

        public static void ServiceLog(string name, EventLogEntryType type, string message)
        {
            if (!EventLog.SourceExists(name)) EventLog.CreateEventSource(name, "Application");
            EventLog.WriteEntry(name, message, type);
        }
        #endregion

    }
}
