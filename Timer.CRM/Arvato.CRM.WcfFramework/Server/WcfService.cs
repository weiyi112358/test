using Arvato.CRM.WcfFramework.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace Arvato.CRM.WcfFramework.Server
{
    public class WcfService
    {
        static WcfService _instance;

        /// <summary>
        /// 服务主机列表
        /// </summary>
        List<ServiceHost> _hosts;

        /// <summary>
        /// Wcf配置
        /// </summary>
        WcfServiceConfig _config;


        #region 消息通知
        /// <summary>
        /// Wcf启动日志
        /// arg1：TraceLevel表示错误级别，只用到Error，Warm，Info
        /// arg2：日志内容
        /// </summary>
        public event Action<object, TraceLevel, Exception> LunchNotify;

        private void OnLunchNotify(TraceLevel level, Exception exception)
        {
            if (LunchNotify != null) LunchNotify(this, level, exception);
        }

        private void OnLunchNotify(Exception exception)
        {
            if (LunchNotify != null) LunchNotify(this, TraceLevel.Error, exception);
        }

        private void OnLunchNotify(string message)
        {
            if (LunchNotify != null) LunchNotify(this, TraceLevel.Info, new Exception(message));
        }
        #endregion

        private WcfService()
        {
            _hosts = new List<ServiceHost>();
            _config = new WcfServiceConfig(getConfigXmlFilePath());
        }

        /// <summary>
        /// 通用ReaderQuotas
        /// </summary>
        /// <returns></returns>
        private XmlDictionaryReaderQuotas generateReaderQuotas()
        {
            XmlDictionaryReaderQuotas quotas = new XmlDictionaryReaderQuotas();
            quotas.MaxStringContentLength = int.MaxValue;
            quotas.MaxArrayLength = int.MaxValue;
            quotas.MaxBytesPerRead = int.MaxValue;
            quotas.MaxNameTableCharCount = int.MaxValue;
            return quotas;
        }

        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <returns></returns>
        private string getConfigXmlFilePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WCFService.config");
        }

        /// <summary>
        /// 字符串类型的协议类型转换到Binding对象
        /// </summary>
        /// <param name="binding">配置文件中的配置类型</param>
        private Binding getBinding(WcfBinding binding)
        {
            //TimeSpan ts = TimeSpan.FromMinutes(_config.TimeOut);
            TimeSpan ts = TimeSpan.FromSeconds(_config.TimeOut);
            switch (binding)
            {
                case WcfBinding.WSHttpBinding:
                    WSHttpBinding httpBinding = new WSHttpBinding();
                    httpBinding.Security.Mode = SecurityMode.None;
                    httpBinding.ReaderQuotas = generateReaderQuotas();
                    httpBinding.MaxReceivedMessageSize = int.MaxValue;
                    httpBinding.MaxBufferPoolSize = long.MaxValue;
                    httpBinding.ReceiveTimeout = ts;
                    httpBinding.SendTimeout = ts;

                    //trans
                    httpBinding.TransactionFlow = true;
                    httpBinding.ReliableSession.Enabled = true;

                    return httpBinding;
                case WcfBinding.TCP:
                    NetTcpBinding netTcpBinding = new NetTcpBinding();
                    netTcpBinding.Security.Mode = SecurityMode.None;
                    netTcpBinding.ReaderQuotas = generateReaderQuotas();
                    netTcpBinding.MaxReceivedMessageSize = int.MaxValue;
                    netTcpBinding.MaxBufferPoolSize = long.MaxValue;
                    netTcpBinding.ReceiveTimeout = ts;
                    netTcpBinding.SendTimeout = ts;
                    netTcpBinding.MaxBufferSize = int.MaxValue;
                    netTcpBinding.TransferMode = TransferMode.Streamed;
                    netTcpBinding.MaxConnections = _config.MaxConnections;
                    netTcpBinding.ListenBacklog = _config.ListenBacklog;
                    netTcpBinding.ReliableSession.InactivityTimeout = ts;
                    return netTcpBinding;
                case WcfBinding.BasicHttpBinding:
                    BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
                    basicHttpBinding.Security.Mode = BasicHttpSecurityMode.None;
                    basicHttpBinding.ReaderQuotas = generateReaderQuotas();
                    basicHttpBinding.MaxReceivedMessageSize = int.MaxValue;
                    basicHttpBinding.MaxBufferPoolSize = long.MaxValue;
                    basicHttpBinding.ReceiveTimeout = ts;
                    basicHttpBinding.SendTimeout = ts;
                    basicHttpBinding.MaxBufferSize = int.MaxValue;
                    basicHttpBinding.TransferMode = TransferMode.Streamed;
                    return basicHttpBinding;
            }
            return null;
        }

        /// <summary>
        /// 获取要注册的Wcf服务类
        /// </summary>
        /// <param name="dllPath">程序集文件目录</param>
        /// <returns></returns>
        private List<ServiceHost> getServices(string dllPath)
        {
            if (!Directory.Exists(dllPath)) Directory.CreateDirectory(dllPath);
            string[] files = Directory.GetFiles(dllPath, "*.dll", SearchOption.AllDirectories);
            OnLunchNotify("共搜索到dll文件：" + files.Length + "个");

            return getServices(dllPath, files);
        }

        /// <summary>
        /// 获取要注册的Wcf服务类
        /// </summary>
        /// <param name="files">程序集文件路径</param>
        /// <returns></returns>
        private List<ServiceHost> getServices(string dllPath, string[] files)
        {
            List<ServiceHost> hosts = new List<ServiceHost>();
            AppDomain.CurrentDomain.AppendPrivatePath(dllPath);
            foreach (string file in files)
            {
                //过滤掉框架程序集
                string fileName = Path.GetFileName(file);

                if (fileName.Contains(".UI.") ||
                    fileName.Contains(".Utility.") ||
                    fileName.StartsWith("Castle", StringComparison.CurrentCultureIgnoreCase) ||
                    fileName.StartsWith("Protobuf", StringComparison.CurrentCultureIgnoreCase) ||
                    fileName.StartsWith("NLog", StringComparison.CurrentCultureIgnoreCase) ||
                    fileName.StartsWith("EntityFramework", StringComparison.CurrentCultureIgnoreCase)) continue;

                try
                {
                    Type[] types = Assembly.LoadFile(file).GetTypes();

                    #region 遍历程序集中的类
                    foreach (var t in types)
                    {
                        if (t.IsClass && isRegistered(t))
                        {
                            var interfaces = t.GetInterfaces();
                            foreach (var i in interfaces)
                            {
                                try
                                {
                                    if (isContract(i) && !existContract(i)) hosts.Add(buildServiceHost(t, i));
                                }
                                catch (Exception ex)
                                {
                                    OnLunchNotify(ex);
                                }
                            }
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    OnLunchNotify(ex);
                }
            }
            return hosts;
        }

        List<Type> contracts = new List<Type>();

        /// <summary>
        /// 判断该契约是否已经注册
        /// </summary>
        /// <param name="contract">契约</param>
        /// <returns></returns>
        private bool existContract(Type contract)
        {
            if (contracts.Contains(contract)) return true;
            contracts.Add(contract);
            return false;
        }
        /// <summary>
        /// 根据类型信息创建wcf service host
        /// </summary>
        /// <param name="service">wcf实现类</param>
        /// <param name="contract">实现的wcf接口</param>
        /// <returns></returns>
        private ServiceHost buildServiceHost(Type service, Type contract)
        {
            //对同一个contract接口，生成连接点，如果是WSDL，暴露一次描述
            ServiceMetadataBehavior metadataBehaviorWSHttpBinding = null;
            ServiceMetadataBehavior metadataBehaviorBasicHttpBinding = null;
            ServiceMetadataBehavior metadataBehaviorTcpBinding = null;

            #region 准备服务,URI和metadataBehavior
            List<Uri> listUri = new List<Uri>();
            if (_config.NeedTCP)
            {
                string addressExposed = string.Format("net.tcp://{0}:{1}{2}/{3}",
                    _config.HostAddress,
                    _config.PortTCP,
                    _config.ServiceName,
                    contract.FullName);
                listUri.Add(new Uri(addressExposed));
                metadataBehaviorTcpBinding = new ServiceMetadataBehavior();

            }
            if (_config.NeedBasicHttpBinding)
            {
                string addressExposed = string.Format("http://{0}:{1}{2}/{3}",
                    _config.HostAddress,
                    _config.PortBasicHttpBinding,
                    _config.ServiceName,
                    contract.FullName);
                listUri.Add(new Uri(addressExposed));
                metadataBehaviorBasicHttpBinding = new ServiceMetadataBehavior();
                metadataBehaviorBasicHttpBinding.HttpGetEnabled = true;
                metadataBehaviorBasicHttpBinding.HttpGetUrl = new Uri(addressExposed);
            }

            if (_config.NeedWSHttpBinding)
            {
                string addressExposed = string.Format("http://{0}:{1}{2}/{3}",
                    _config.HostAddress,
                    _config.PortWSHttpBinding,
                    _config.ServiceName,
                    contract.FullName);
                listUri.Add(new Uri(addressExposed));
                metadataBehaviorWSHttpBinding = new ServiceMetadataBehavior();
                metadataBehaviorWSHttpBinding.HttpGetEnabled = true;
                metadataBehaviorWSHttpBinding.HttpGetUrl = new Uri(addressExposed);

            }
            #endregion

            #region 生成servviceHost实例
            ServiceHost serviceHost = new ServiceHost(service, listUri.ToArray());
            //serviceHost.Faulted += (sender, arg) =>
            //{
            //    serviceHost = buildServiceHost(service, contract);
            //    serviceHost.Open();
            //};

            ServiceThrottlingBehavior stb = new ServiceThrottlingBehavior();
            stb.MaxConcurrentSessions = _config.MaxConnections;
            stb.MaxConcurrentCalls = _config.MaxCalls;
            if (_config.MaxInstances != 0) stb.MaxConcurrentInstances = _config.MaxInstances;
            serviceHost.Description.Behaviors.Add(stb);
            #endregion

            #region 修改ServiceDebugBehavior属性
            var sdb = serviceHost.Description.Behaviors.FirstOrDefault(p => p is ServiceDebugBehavior) as ServiceDebugBehavior;
            if (sdb == null)
            {
                sdb = new ServiceDebugBehavior();
                serviceHost.Description.Behaviors.Add(sdb);
            }
            sdb.IncludeExceptionDetailInFaults = true;
            #endregion 修改ServiceDebugBehavior属性

            #region 暴露WSDL
            addBehavior(serviceHost, metadataBehaviorBasicHttpBinding);
            addBehavior(serviceHost, metadataBehaviorWSHttpBinding);
            addBehavior(serviceHost, metadataBehaviorTcpBinding);
            #endregion

            #region 添加接口方法特性
            if (_config.NeedTCP)
            {
                NetTcpBinding binding = getBinding(WcfBinding.TCP) as NetTcpBinding;
                binding.ListenBacklog = _config.ListenBacklog;
                ServiceEndpoint endpoint = serviceHost.AddServiceEndpoint(contract, binding, "");
                if (contract.GetCustomAttributes(typeof(ProtobufAttribute), false).Any())
                {
                    endpoint.Behaviors.Add(new ProtoBuf.ServiceModel.ProtoEndpointBehavior());
                    OnLunchNotify("Protobuf enabled: " + contract);
                }
                addDescription(endpoint);
            }
            if (_config.NeedBasicHttpBinding)
            {
                ServiceEndpoint endpoint = serviceHost.AddServiceEndpoint(contract, getBinding(WcfBinding.BasicHttpBinding), "");
                addDescription(endpoint);
            }
            if (_config.NeedWSHttpBinding)
            {
                WSHttpBinding binding = getBinding(WcfBinding.WSHttpBinding) as WSHttpBinding;
                ServiceEndpoint endpoint = serviceHost.AddServiceEndpoint(contract, binding, "");
                addDescription(endpoint);
            }
            #endregion

            return serviceHost;
        }

        /// <summary>
        /// 为Wcf服务添加Behavior
        /// </summary>
        /// <param name="serviceHost">Wcf服务</param>
        /// <param name="serviceMetadataBehavior">要添加的Behavior</param>
        private void addBehavior(ServiceHost serviceHost, ServiceMetadataBehavior serviceMetadataBehavior)
        {
            if (null != serviceMetadataBehavior)
            {
                if (serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
                {
                    serviceHost.Description.Behaviors.Add(serviceMetadataBehavior);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceEndpoint"></param>
        private void addDescription(ServiceEndpoint serviceEndpoint)
        {
            ContractDescription cd = serviceEndpoint.Contract;
            //逐一加载设置接口定义内的具体方法operDesc
            foreach (OperationDescription operDesc in cd.Operations)
            {
                // Find the serializer behavior.
                DataContractSerializerOperationBehavior serializerBehavior = operDesc.Behaviors.Find<DataContractSerializerOperationBehavior>();
                //serializerBehavior.DataContractSurrogate = new SHBDataContractSurrogate();
                // If the serializer is not found, create one and add it.
                if (serializerBehavior == null)
                {
                    serializerBehavior = new DataContractSerializerOperationBehavior(operDesc);
                    //serializerBehavior.DataContractSurrogate = new SHBDataContractSurrogate();
                    operDesc.Behaviors.Add(serializerBehavior);
                }
                // Change the settings of the behavior.    
                serializerBehavior.MaxItemsInObjectGraph = int.MaxValue;
                serializerBehavior.IgnoreExtensionDataObject = true;
            }
        }

        /// <summary>
        /// 检查以个类是否需要注册为WCF服务
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        private bool isRegistered(Type service)
        {
            object[] customAttributies = service.GetCustomAttributes(false);
            foreach (object att in customAttributies)
            {
                RegisterAttribute attr = att as RegisterAttribute;
                if (attr != null) return attr.Registered;

            }
            return true;
        }

        /// <summary>
        /// 检查一个类定义中是否有WCF contract行为类型定义,如果有返回类型,否则返回null
        /// </summary>
        /// <param name="type">待检查的类型定义</param>
        /// <returns></returns>
        private bool isContract(Type contact)
        {
            object[] customAttributies = contact.GetCustomAttributes(false);
            foreach (object attr in customAttributies)
            {
                ServiceContractAttribute serviceContractAttribute = attr as ServiceContractAttribute;
                if (null != serviceContractAttribute) return true; //it is not wcf contract
            }
            return false;
        }

        /// <summary>
        /// 单一实例
        /// </summary>
        /// <returns></returns>
        public static WcfService GetInstance()
        {
            return _instance = _instance ?? new WcfService();
        }

        /// <summary>
        /// 启动服务
        /// 查找当前程序域的目录下的所有 dll 文件中的类，
        /// 如果类是一个实现了契约接口的类，则创建并启动一个 WCF 服务
        /// </summary>
        public void Start()
        {
            OnLunchNotify("Wcf配置文件路径为：" + getConfigXmlFilePath());

            StringBuilder serviceList = new StringBuilder();
            string hostLocation = _config.Dependency;
            OnLunchNotify("Host所在的路径为：" + hostLocation);

            _hosts.AddRange(getServices(hostLocation));
            OnLunchNotify("构建Service Host数量：" + _hosts.Count);

            foreach (ServiceHost host in _hosts)
            {
                string hostInfo = host.Description.ServiceType.AssemblyQualifiedName;
                try
                {
                    host.Open();
                    OnLunchNotify("启动成功：" + hostInfo);
#if INF
                    StringBuilder sb = new StringBuilder();
                    foreach (var uri in host.BaseAddresses)
                    {
                        OnLunchNotify(uri.AbsoluteUri);
                        sb.AppendLine(uri.AbsoluteUri);
                    }
                    OnLunchNotify(sb.ToString());
#endif

#if DEBUG
                    ChannelDispatcher dispatcher = host.ChannelDispatchers[0] as ChannelDispatcher;
                    ServiceThrottle serviceThrottl = dispatcher.ServiceThrottle;
					Console.WriteLine("MaxConcurrentCalls = " + serviceThrottl.MaxConcurrentCalls);
					Console.WriteLine("MaxSessions = " + serviceThrottl.MaxConcurrentSessions);
					Console.WriteLine("MaxInstances = " + serviceThrottl.MaxConcurrentInstances);

					NetTcpBinding tcpBinding = host.Description.Endpoints[0].Binding as NetTcpBinding;
					Console.WriteLine("NetTcpBinding.ListenBacklog = " + tcpBinding.ListenBacklog);
					Console.WriteLine();
#endif
                }
                catch (Exception ex)
                {
                    OnLunchNotify(ex);
                }
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            foreach (var host in _hosts)
            {
                if (host.State != CommunicationState.Opened) continue;
                try
                {
                    host.Close();
                }
                catch (Exception ex)
                {
                    OnLunchNotify(ex);
                }
            }
        }
    }
}
