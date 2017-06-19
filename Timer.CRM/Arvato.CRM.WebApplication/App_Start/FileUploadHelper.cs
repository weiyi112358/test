using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Arvato.CRM.WebApplication
{
    public static class FileUploadHelper
    {
        private static Random rnd = new Random();
        public static string UpLoadFile(string imagePath, HttpPostedFileBase hfb, ref string filePath)
        {
            var file = hfb;
            int fileLen = file.ContentLength;
            string filename = file.FileName;
            string newfilename = string.Empty;
            string fileex = System.IO.Path.GetExtension(filename); ;//文件扩展名 
            if (filename.LastIndexOf("\\") > -1)
            {
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);
            }
            string path = "/Upload/" + imagePath.ToString();
            string mapPath = System.Web.HttpContext.Current.Server.MapPath(path);
            if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
            {
                System.IO.Directory.CreateDirectory(mapPath);
            }
            newfilename = GetUniquelyName() + fileex;

            filePath = path + "/" + newfilename;//返回到前台的ImgUrl
            mapPath = mapPath + "/" + newfilename;
            file.SaveAs(mapPath);
            return path;
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