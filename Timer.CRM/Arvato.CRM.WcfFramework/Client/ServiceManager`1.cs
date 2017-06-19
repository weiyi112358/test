using Arvato.CRM.WcfFramework.Attributes;
using System;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;

namespace Arvato.CRM.WcfFramework.ClientProxy
{
    /// <summary>
    /// 简化开发者调用WCF的工作，此类继承IDisposable,在使用时
    /// 要包含在using块里，或者调用Close方法。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceManager<T> : IDisposable
    {
        //被调用的服务的发布地址
        private string _address;

        //被调用的服务点
        ServiceEndpoint _serviceEndpoint;

        //本地和服务端的通道，T是要调用的服务接口的type.
        ChannelFactory<T> _factory;

        //要调用的服务接口，如IUser
        T _service;

        WcfClientConfig _config = null;

        /// <summary>
        /// 初始化被调用的服务点对象
        /// </summary>  
        public ServiceManager() : this("") { }

        /// <summary>
        /// 初始化被调用的服务点对象
        /// </summary>
        /// <param name="hostMachineAddress">指定特定的连接主机</param>
        public ServiceManager(string hostMachineAddress)
        {
            if (null == _config) _config = getConfig();
            ContractDescription contractDescription = ContractDescription.GetContract(typeof(T));

            //判断是否是特定的连接主机
            if (string.IsNullOrEmpty(hostMachineAddress))
            {
                _address = string.Format("{0}/{1}", _config.BaseAddress, typeof(T).FullName);
            }
            else
            {
                string address = _config.BaseAddress;
                string address1 = string.Empty;
                int machinPositon = 0;

                machinPositon = address.IndexOf("://", StringComparison.OrdinalIgnoreCase) + 3;
                address1 = address.Substring(0, machinPositon);
                address1 += hostMachineAddress;
                int portPositon = 0;
                portPositon = address.IndexOf(":", machinPositon + 1, StringComparison.OrdinalIgnoreCase);
                if (portPositon < 0) portPositon = address.IndexOf("/", machinPositon + 1, StringComparison.OrdinalIgnoreCase);
                address1 += address.Substring(portPositon);
                _address = string.Format("{0}/{1}", address1, typeof(T).FullName);
            }

            TimeSpan ts = TimeSpan.FromSeconds(_config.SendOrRevTimeOut);
            if (_address.Contains("net.tcp:"))
            {
                NetTcpBinding binding = new NetTcpBinding();
                
                binding.ReaderQuotas = generateReaderQuotas();
                binding.SendTimeout = ts;
                binding.ReceiveTimeout = ts;
                binding.MaxReceivedMessageSize = int.MaxValue;
                binding.MaxBufferPoolSize = long.MaxValue;
                binding.MaxBufferSize = int.MaxValue;
                binding.Security.Mode = SecurityMode.None;
                binding.TransferMode = TransferMode.Streamed;
                binding.MaxConnections = 1000;
                binding.ReliableSession.InactivityTimeout = ts;
                binding.ListenBacklog = 1000;
                _serviceEndpoint = new ServiceEndpoint(contractDescription, binding, new EndpointAddress(_address));
                if (contractDescription.ContractType.GetCustomAttributes(typeof(ProtobufAttribute), false).Any()) _serviceEndpoint.Behaviors.Add(new ProtoBuf.ServiceModel.ProtoEndpointBehavior());
            }
            else
            {
                if (_config.IsBasicHttpBinding)
                {
                    BasicHttpBinding binding = new BasicHttpBinding();
                    binding.ReaderQuotas = generateReaderQuotas();
                    binding.SendTimeout = ts;
                    binding.ReceiveTimeout = ts;
                    binding.MaxReceivedMessageSize = int.MaxValue;
                    binding.MaxBufferPoolSize = long.MaxValue;
                    binding.MaxBufferSize = int.MaxValue;
                    binding.Security.Mode = BasicHttpSecurityMode.None;
                    binding.TransferMode = TransferMode.Streamed;
                    _serviceEndpoint = new ServiceEndpoint(contractDescription, binding, new EndpointAddress(_address));
                }
                else
                {
                    WSHttpBinding binding = new WSHttpBinding();
                    binding.ReaderQuotas = generateReaderQuotas();
                    binding.SendTimeout = ts;
                    binding.ReceiveTimeout = ts;
                    binding.MaxReceivedMessageSize = int.MaxValue;
                    binding.MaxBufferPoolSize = long.MaxValue;
                    binding.Security.Mode = SecurityMode.None;
                    
                    //trans
                    binding.TransactionFlow = true;
                    binding.ReliableSession.Enabled = true;

                    _serviceEndpoint = new ServiceEndpoint(contractDescription, binding, new EndpointAddress(_address));
                }
            }
            // Find the ContractDescription of the operation to find.
            foreach (OperationDescription operDesc in contractDescription.Operations)
            {
                // Find the serializer behavior.
                DataContractSerializerOperationBehavior serializerBehavior = operDesc.Behaviors.Find<DataContractSerializerOperationBehavior>();
                // If the serializer is not found, create one and add iserializerBehavior.DataContractSurrogate = new SHBDataContractSurrogate();.
                //serializerBehavior.DataContractSurrogate = new SHBDataContractSurrogate();
                if (serializerBehavior == null)
                {
                    serializerBehavior = new DataContractSerializerOperationBehavior(operDesc);
                    operDesc.Behaviors.Add(serializerBehavior);
                }
                // Change the settings of the behavior.
                serializerBehavior.MaxItemsInObjectGraph = int.MaxValue;
                serializerBehavior.IgnoreExtensionDataObject = true;
            }
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
            quotas.MaxDepth = _config.MaxMessage;
            quotas.MaxNameTableCharCount = _config.MaxMessage;
            return quotas;
        }

        /// <summary>
        /// 创建用户调用服务的接口
        /// </summary>
        /// <returns>返回一个服务对象，一般以接口形式返回</returns>
        private void initService()
        {
            _factory = new ChannelFactory<T>(_serviceEndpoint);
            //添加可以传输自定义信息的扩展，这个扩展在会员端隐式完成的
            if (_factory.Endpoint.Behaviors.Find<AttachingMessage>() == null)
            {
                _factory.Endpoint.Behaviors.Add(new AttachingMessage());
            }
            _service = _factory.CreateChannel();
            IContextChannel contextChannel = (IContextChannel)_service;
            TimeSpan ts = TimeSpan.FromSeconds(_config.SendOrRevTimeOut);
            contextChannel.OperationTimeout = ts;
            //方便进行代理转移
            //_service = ServiceProxyFactory.Create<T>(_serviceEndpoint, _wcfClientConfig.SendOrRevTimeOut);
        }

        /// <summary>
        /// 设置服务地址
        /// </summary>
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                _serviceEndpoint.Address = new EndpointAddress(_address);
            }
        }

        /// <summary>
        /// 获得服务接口
        /// </summary>
        public T Service
        {
            get
            {
                if (null == _service) initService();
                return _service;
            }
        }

        /// <summary>
        /// 释放代理
        /// 如果会员端被包含在using块里，不需要调用；反之，需要。
        /// </summary>
        public void Close()
        {
            if (_factory.State != CommunicationState.Closed && _factory.State != CommunicationState.Closing)
            {
                try
                {
                    _factory.Close();
                }
                catch
                {
                    //避免在通道错误时，拿不到正确的错误信息
                }
            }
        }

        /// <summary>
        /// 打开代理
        /// </summary>
        public void Open()
        {
            if (_factory.State != CommunicationState.Closed)
            {
                try
                {
                    _factory.Open();
                }
                catch
                {

                }
            }
        }

        private WcfClientConfig getConfig()
        {
            return ConfigurationManager.GetSection("BusinessService") as WcfClientConfig;
        }

        #region IDisposable 成员
        /// <summary>
        /// 释放对象
        /// </summary>
        public void Dispose()
        {
            if (null != _factory) Close();
        }
        #endregion
    }
}
