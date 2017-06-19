using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Threading;

namespace Arvato.CRM.WcfFramework.ClientProxy
{
	/// <summary>
	/// 添加信息到WCF的Header
	/// </summary>
	public class ClientMessageInspector : IClientMessageInspector
	{
		static ProxyHeaderEntity proxyHeader;

		#region IClientMessageInspector 成员
		/// <summary>
		/// 获取通道里的数据
		/// </summary>
		/// <param name="reply"></param>
		/// <param name="correlationState"></param>
		public void AfterReceiveReply(ref Message reply, object correlationState)
		{
			CustomerChannel channel = (CustomerChannel)correlationState;
			channel.EndTime = DateTime.Now;
			if (System.Configuration.ConfigurationManager.AppSettings["RequestTime"] == "true")
			{
				string msg = string.Format("开始时间：【{0}】,结束时间：【{1}】，时差【{2}】毫秒，类型【{3}】方法【{4}】",
						channel.StartTime, channel.EndTime, (channel.EndTime - channel.StartTime).TotalMilliseconds,
						channel.ConstractName, channel.ActionName);
			}
			//如果通道里有需要的数据
			if (reply.Headers.FindHeader("ProxyHeader", "UINS") > -1)
			{
				//把通道的数据缓存
				proxyHeader = reply.Headers.GetHeader<ProxyHeaderEntity>("ProxyHeader", "UINS");
				proxyHeader.ClientCulture = Thread.CurrentThread.CurrentCulture.Name;
			}
			if (reply.IsFault && reply.Headers.FindHeader("FrameworkWrappedException", "UINS") > -1)
			{
				MessageFault mf = MessageFault.CreateFault(reply, 10240000);
				ErrorDetail ed = mf.GetDetail<ErrorDetail>();
				MemoryStream ms = new MemoryStream(ed.ExceptionData);
				Exception ex = new BinaryFormatter().Deserialize(ms) as Exception;
				throw ex;
			}
		}
		/// <summary>
		/// 附加数据到通道中
		/// </summary>
		/// <param name="request"></param>
		/// <param name="channel"></param>
		/// <returns></returns>
		public object BeforeSendRequest(ref Message request, IClientChannel channel)
		{
			//开始附加信息
			MessageHeader mh = MessageHeader.CreateHeader("ProxyHeader", "UINS", proxyHeader);
			request.Headers.Add(mh);
			request.Headers.Add(MessageHeader.CreateHeader("RequestExceptionDetail", "UINS", true));
			CustomerChannel myhannelExtension = channel.Extensions.Find<CustomerChannel>();
			myhannelExtension.StartTime = DateTime.Now;
			myhannelExtension.ActionName = request.Headers.Action;
			return myhannelExtension;
		}

		#endregion
	}
}
