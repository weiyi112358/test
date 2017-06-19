using System;
using System.IO;
using System.Xml;

namespace Arvato.CRM.WcfFramework.Server
{
    /// <summary>
    /// Wcf配置
    /// </summary>
    public class WcfServiceConfig
    {
        private string _hostAddress;
        private string _serviceName;
        private bool _needWSHttpBinding;
        private string _portWSHttpBinding;
        private bool _needTCP;
        private string _portTCP;
        private bool _needBasicHttpBinding;
        private string _portBasicHttpBinding;
        private int _maxMessage = ushort.MaxValue;
        private int _timeOut = 10;
        private int maxConnections;
        private int maxCalls;
        private int maxInstances;
        private int listenBacklog;
        private int minCompressSize = 102400;
        private string dependency;

        /// <summary>
        /// WCF HOST  主机地址
        /// </summary>
        public string HostAddress
        {
            get { return _hostAddress; }
            set { _hostAddress = value; }
        }

        /// <summary>
        /// WCF 服务名称
        /// </summary>
        public string ServiceName
        {
            get { return _serviceName; }
            set { _serviceName = value; }
        }

        /// <summary>
        /// 是否需要WSHttpBinding
        /// </summary>
        public bool NeedWSHttpBinding
        {
            get { return _needWSHttpBinding & !_needBasicHttpBinding; }
            set { _needWSHttpBinding = value; }
        }

        /// <summary>
        /// WSHttpBinding绑定端口
        /// </summary>
        public string PortWSHttpBinding
        {
            get { return _portWSHttpBinding; }
            set { _portWSHttpBinding = value; }
        }

        /// <summary>
        /// 是否需要TCP
        /// </summary>
        public bool NeedTCP
        {
            get { return _needTCP; }
            set { _needTCP = value; }
        }

        /// <summary>
        /// TCP绑定端口
        /// </summary>
        public string PortTCP
        {
            get { return _portTCP; }
            set { _portTCP = value; }
        }

        /// <summary>
        /// 是否需要BasicHttpBinding
        /// </summary>
        public bool NeedBasicHttpBinding
        {
            get { return _needBasicHttpBinding; }
            set { _needBasicHttpBinding = value; }
        }

        /// <summary>
        /// BasicHttpBinding绑定端口
        /// </summary>
        public string PortBasicHttpBinding
        {
            get { return _portBasicHttpBinding; }
            set { _portBasicHttpBinding = value; }
        }

        /// <summary>
        /// 最大传输量
        /// </summary>
        public int MaxMessage
        {
            get { return _maxMessage; }
            set { _maxMessage = value; }
        }

        /// <summary>
        /// 接收和发送数据的时间间隔，即过期关闭连接的时间
        /// </summary>
        public int TimeOut
        {
            get { return _timeOut; }
            set { _timeOut = value; }
        }

        public int MaxConnections
        {
            get { return this.maxConnections; }
            set { this.maxConnections = value; }
        }

        public int MaxCalls
        {
            get { return this.maxCalls; }
            set { this.maxCalls = value; }
        }

        public int MaxInstances
        {
            get { return this.maxInstances; }
            set { this.maxInstances = value; }
        }

        public int ListenBacklog
        {
            get { return this.listenBacklog; }
            set { this.listenBacklog = value; }
        }

        /// <summary>
        /// 压缩传输阀值，小于此值，则压缩传输，默认值：102400字节
        /// </summary>
        public int MinCompressSize
        {
            get { return minCompressSize; }
            set { minCompressSize = value; }
        }

        /// <summary>
        /// Wcf程序集存放路径
        /// </summary>
        public string Dependency
        {
            set { dependency = value; }
            get { return dependency; }
        }

        public WcfServiceConfig() { }

        /// <summary>
        /// 通过配置的XML加载WCF配置信息
        /// </summary>
        /// <param name="configXmlFilePath"></param>
        public WcfServiceConfig(string configXmlFilePath)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(configXmlFilePath);

            #region hostaddr
            HostAddress = xDoc.SelectSingleNode("//SHB.WCFService/HostAddress").InnerText.Trim();
            if (string.IsNullOrEmpty(HostAddress)) HostAddress = "localhost";
            #endregion

            #region ServiceName
            ServiceName = xDoc.SelectSingleNode("//SHB.WCFService/ServiceName").InnerText.Trim();
            if (!string.IsNullOrEmpty(ServiceName) && !ServiceName.StartsWith("/")) ServiceName = "/" + ServiceName;
            #endregion

            #region WSHttpBinding
            if (xDoc.SelectSingleNode("//SHB.WCFService/WSHttpBinding").InnerText == "1") NeedWSHttpBinding = true;
            PortWSHttpBinding = xDoc.SelectSingleNode("//SHB.WCFService/WSHttpBinding/@port").Value.Trim();
            #endregion

            #region TCP
            if (xDoc.SelectSingleNode("//SHB.WCFService/TCP").InnerText == "1") NeedTCP = true;
            PortTCP = xDoc.SelectSingleNode("//SHB.WCFService/TCP/@port").Value.Trim();

            if (xDoc.SelectSingleNode("//SHB.WCFService/TCP/@maxConnections").Value != "0") MaxConnections = Convert.ToInt32(xDoc.SelectSingleNode("//SHB.WCFService/TCP/@maxConnections").Value);

            if (xDoc.SelectSingleNode("//SHB.WCFService/TCP/@maxCalls").Value != "0") MaxCalls = Convert.ToInt32(xDoc.SelectSingleNode("//SHB.WCFService/TCP/@maxCalls").Value);

            if (xDoc.SelectSingleNode("//SHB.WCFService/TCP/@maxInstances").Value != "0") MaxInstances = Convert.ToInt32(xDoc.SelectSingleNode("//SHB.WCFService/TCP/@maxInstances").Value);

            if (xDoc.SelectSingleNode("//SHB.WCFService/TCP/@listenBacklog").Value != "0") ListenBacklog = Convert.ToInt32(xDoc.SelectSingleNode("//SHB.WCFService/TCP/@listenBacklog").Value);
            #endregion

            #region BasicHttpBinding
            if (xDoc.SelectSingleNode("//SHB.WCFService/BasicHttpBinding").InnerText == "1") NeedBasicHttpBinding = true;
            PortBasicHttpBinding = xDoc.SelectSingleNode("//SHB.WCFService/BasicHttpBinding/@port").Value.Trim();
            #endregion

            #region MaxMessage
            MaxMessage = Convert.ToInt32(xDoc.SelectSingleNode("//SHB.WCFService/MaxMessage").InnerText.Trim());
            #endregion

            #region TimeOut
            TimeOut = Convert.ToInt32(xDoc.SelectSingleNode("//SHB.WCFService/SendOrRevTimeOut").InnerText.Trim());
            #endregion

            #region MinCompressSize
            MinCompressSize = Convert.ToInt32(xDoc.SelectSingleNode("//SHB.WCFService/MinCompressSize").InnerText.Trim());
            #endregion

            #region Dependency
            var path = AppDomain.CurrentDomain.BaseDirectory;
            if (xDoc.SelectSingleNode("//SHB.WCFService/Dependency") != null) Dependency = xDoc.SelectSingleNode("//SHB.WCFService/Dependency").InnerText.Trim();
            else Dependency = "~/";
            Dependency = Path.GetFullPath(Dependency.Replace("~/", path));
            #endregion
        }
    }
}
