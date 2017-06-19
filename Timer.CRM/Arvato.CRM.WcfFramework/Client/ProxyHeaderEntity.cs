using System.Runtime.Serialization;

namespace Arvato.CRM.WcfFramework.ClientProxy
{
	/// <summary>
	/// 在WCF头信息中传递的上下文
	/// </summary>
	[DataContract]
	public class ProxyHeaderEntity
	{
		/// <summary>
		/// 会员端的登录用户帐号
		/// </summary>
		[DataMember]
		public string UserAccount = "Anonymouse";

		/// <summary>
		/// 会员端的机器名
		/// </summary>
		[DataMember]
		public string ClientMachineName;

		/// <summary>
		/// 会员端的IP地址
		/// </summary>
		[DataMember]
		public string ClientIP;

		/// <summary>
		/// 会员端的操所系统语言版本，如：zh-CN,en-US
		/// </summary>
		[DataMember]
		public string ClientCulture;

		/// <summary>
		/// 当前连接的服务器（NLB时可识别出当此访问的服务器）
		/// </summary>
		[DataMember]
		public string CurrentConnectedServer;

		/// <summary>
		/// 业务系统自定义数据
		/// </summary>
		[DataMember]
		public string CustomData;

		/// <summary>
		/// 当次远程调用的Session Guid,由WCF服务框架在服务端处理。不做序列化，在会员端无法使用
		/// </summary>
		public string SessionGuid;
	}
}
