using System;
using System.Configuration;
using System.Xml;

namespace Arvato.CRM.WcfFramework.ClientProxy
{
	/// <summary>
	/// 配置文件转化
	/// </summary>
	/// <remarks >
	///   <BusinessService>
	///     <SendOrRevTimeOut>600</SendOrRevTimeOut>
	///     <MaxMessage>104857600</MaxMessage>
	///     <IsBasicHttpBinding>1</IsBasicHttpBinding>
	///     <BaseAddress>http://LocalHost:8731</BaseAddress>
	///     <MinCompressSize>102400</MinCompressSize>
	///     <ServiceBaseAddress>
	///       <OptionalAddress>net.tcp://LocalHost:8732</OptionalAddress>
	///       <OptionalAddress>http://LocalHost:8731</OptionalAddress>
	///       <OptionalAddress>http://192.168.248.189:8731</OptionalAddress>
	///       <OptionalAddress>http://192.168.1.110:8731</OptionalAddress>
	///     </ServiceBaseAddress>
	///   </BusinessService>
	/// </remarks>
	public class WcfClientConfigHandler : IConfigurationSectionHandler
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		public WcfClientConfigHandler() { }

		#region IConfigurationSectionHandler 成员
		/// <summary>
		/// 创建配置节点
		/// </summary>
		/// <param name="parent">父对象</param>
		/// <param name="configContext">上下文对象</param>
		/// <param name="section">配置节点</param>
		/// <returns></returns>
		public object Create(object parent, object configContext, System.Xml.XmlNode section)
		{
			WcfClientConfig clientConfig = new WcfClientConfig();
			clientConfig.MaxMessage = Convert.ToInt32(section.SelectSingleNode("MaxMessage").InnerText);
			clientConfig.SendOrRevTimeOut = Convert.ToInt32(section.SelectSingleNode("SendOrRevTimeOut").InnerText);
			clientConfig.IsBasicHttpBinding = section.SelectSingleNode("IsBasicHttpBinding").InnerText == "1";
			//clientConfig.WellKnowServiceUrl = section.SelectSingleNode("WellKnowServiceUrl").InnerText;
			clientConfig.MinCompressSize = Convert.ToInt32(section.SelectSingleNode("MinCompressSize").InnerText);
			clientConfig.BaseAddress = section.SelectSingleNode("BaseAddress").InnerText;

			//XmlNodeList cultureNodes = section.SelectSingleNode("CultureSupported").SelectNodes("Culture");

			//foreach (XmlNode node in cultureNodes)
			//{
			//	clientConfig.Cutures.Add(node.InnerText);
			//}
			return clientConfig;
		}

		#endregion
	}
}
