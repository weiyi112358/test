using System;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;

namespace Arvato.CRM.WcfFramework.ClientProxy
{
	/// <summary>
	/// 通道扩展，用于存储压缩有关的数据
	/// </summary>
	public class CustomerChannel : IChannelInitializer, IExtension<IContextChannel>
	{
		#region IChannelInitializer Members
		void IChannelInitializer.Initialize(IClientChannel channel)
		{
			channel.Extensions.Add(this);
		}
		#endregion

		#region IExtension<IContextChannel> Members
		void IExtension<IContextChannel>.Attach(IContextChannel owner) { }
		void IExtension<IContextChannel>.Detach(IContextChannel owner) { }
		#endregion

		// This is where you will save the returned data
		/// <summary>
		/// 压缩时间
		/// </summary>
		public long CompressTime { get; set; }

		/// <summary>
		/// 解压时间
		/// </summary>
		public long DecompressTime { get; set; }

		/// <summary>
		/// 数据长度（压缩前大小）
		/// </summary>
		public int DataLength { get; set; }

		/// <summary>
		/// 传输长度（压缩后大小）
		/// </summary>
		public int TransferLength { get; set; }

		/// <summary>
		/// 请求开始时间
		/// </summary>
		public DateTime StartTime { get; set; }

		/// <summary>
		/// 完成请求时间
		/// </summary>
		public DateTime EndTime { get; set; }

		/// <summary>
		/// 接口名
		/// </summary>
		public string ConstractName { get; set; }

		/// <summary>
		/// 方法名
		/// </summary>
		public string ActionName { get; set; }
	}
}
