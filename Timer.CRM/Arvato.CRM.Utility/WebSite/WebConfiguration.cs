using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace System.Web.Mvc
{
    [Serializable]
    [XmlRoot("webConfig")]
    public class WebConfiguration
    {
        static WebConfiguration _default;
        internal static WebConfiguration Default
        {
            get { return _default = _default ?? ConfigurationManager.GetSection("webConfig") as WebConfiguration; }
        }

        [XmlElement("webSite")]
        public WebSite WebSite { set; get; }

        public WebConfiguration()
        {
            WebSite = new WebSite();
        }

        public static WebConfiguration Load(string filepath = "~/webConfig.config")
        {
            var file = Arvato.CRM.Util.MapPath(filepath);
            var di = Directory.GetParent(file);
            if (di.Exists) di.Create();
            using (StreamReader txtReader = new StreamReader(file))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(WebConfiguration));
                return xmlSerializer.Deserialize(txtReader) as WebConfiguration;
            }
        }

        /// <summary>
        /// 序列化存储
        /// </summary>
        /// <param name="filepath"></param>
        public void Save(string filepath = "~/webConfig.config")
        {
            var file = Arvato.CRM.Util.MapPath(filepath);
            var di = Directory.GetParent(file);
            if (di.Exists) di.Create();
            using (StreamWriter textWriter = new StreamWriter(file))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(WebConfiguration));
                xmlSerializer.Serialize(textWriter, this);
                textWriter.Flush();
            }
        }
    }
}
