namespace Arvato.CRM.WcfFramework.ClientProxy
{
	/// <summary>
	/// 用于描述WCF Host 服务器的基本信息
	/// </summary>
	public class ServiceInfo
	{
		/// <summary>
		/// 服务器显示名称
		/// </summary>
		public string DisplayName { get; set; }

		/// <summary>
		/// 服务器基地址
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// EMP配置文件名
		/// </summary>
		public string EmpConfigFile { get; set; }

		/// <summary>
		/// 返回文本形式：DisplayName[Address]
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("{0}", DisplayName);
		}
	}
}
