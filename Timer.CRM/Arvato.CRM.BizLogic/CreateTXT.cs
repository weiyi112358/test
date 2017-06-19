using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arvato.CRM.Model;
using System.Web;
using Arvato.CRM.Utility;
using System.IO;

namespace Arvato.CRM.BizLogic
{
    public static class CreateTXT
    {
        private static Random rnd = new Random();

        public static string WriteTxt(CreatTxtModel model,string mapPath)
        {
            string info = JsonHelper.Serialize(model);
            string path = "/Upload/TXT";
            string newfilename = GetUniquelyName() + ".txt";          
            string fullPath=mapPath+"/"+newfilename;
            if (!Directory.Exists(mapPath))
            {
                Directory.CreateDirectory(mapPath);
            }          
            if (!File.Exists(fullPath))
            {
                FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);               
                sw.Write(info);
                sw.Close();
            }           
            return path+"/"+newfilename;
        }

        public static void GetHDLogo(string mapPath,string filePath)
        {
            
        
        
        
        }



        /// <summary>
        /// 获取一个不重复的文件名
        /// </summary>
        /// <returns></returns>
        public static string GetUniquelyName()
        {
            const int RANDOM_MAX_VALUE = 1000;
            string strTemp, strYear, strMonth, strDay, strHour, strMinute, strSecond, strMillisecond;
            DateTime dt = DateTime.Now;
            int rndNumber = rnd.Next(RANDOM_MAX_VALUE);
            strYear = dt.Year.ToString();
            strMonth = (dt.Month > 9) ? dt.Month.ToString() : "0" + dt.Month.ToString();
            strDay = (dt.Day > 9) ? dt.Day.ToString() : "0" + dt.Day.ToString();
            strHour = (dt.Hour > 9) ? dt.Hour.ToString() : "0" + dt.Hour.ToString();
            strMinute = (dt.Minute > 9) ? dt.Minute.ToString() : "0" + dt.Minute.ToString();
            strSecond = (dt.Second > 9) ? dt.Second.ToString() : "0" + dt.Second.ToString();
            strMillisecond = dt.Millisecond.ToString();
            strTemp = strYear + strMonth + strDay + "_" + strHour + strMinute + strSecond + "_" + strMillisecond + "_" + rndNumber.ToString();
            return strTemp;
        }
     
    }


}
