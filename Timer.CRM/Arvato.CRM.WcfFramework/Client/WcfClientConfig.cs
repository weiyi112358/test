using System.Collections.Generic;

namespace Arvato.CRM.WcfFramework.ClientProxy
{
	/// <summary>
	/// WCF服务参数提供者
	/// </summary>
	public class WcfClientConfig
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		public WcfClientConfig() { }

		private int _sendOrRevTimeOut = 60;
		private int _maxMessage = 65536;
		private bool _isBasicHttpBinding;
		private int minCompressSize = 102400;
		private string baseAddress;

		/// <summary>
		/// 接收和发送等待的时间
		/// </summary>
		public int SendOrRevTimeOut
		{
			get { return _sendOrRevTimeOut; }
			set { _sendOrRevTimeOut = value; }
		}

		/// <summary>
		/// 最大消息处理数量
		/// </summary>
		public int MaxMessage
		{
			get { return _maxMessage; }
			set { _maxMessage = value; }
		}

		/// <summary>
		/// 识别是httpbinding 还是 wshttpbinding
		/// </summary>
		public bool IsBasicHttpBinding
		{
			get { return _isBasicHttpBinding; }
			set { _isBasicHttpBinding = value; }
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
		/// Wcf服务基地址
		/// </summary>
		public string BaseAddress
		{
			set { baseAddress = value; }
			get { return baseAddress; }
		}
	}
}
