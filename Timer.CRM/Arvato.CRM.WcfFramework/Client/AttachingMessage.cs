using System;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Arvato.CRM.WcfFramework.ClientProxy
{
	/// <summary>
	/// 添加扩展行为到WCF
	/// </summary>
	public class AttachingMessage : BehaviorExtensionElement, IEndpointBehavior
	{
		/// <summary>
		/// BehaviorType is <see cref="AttachingMessage"/>
		/// </summary>
		public override Type BehaviorType
		{
			get { return typeof(AttachingMessage); }
		}

		/// <summary>
		/// Create a new <see cref="AttachingMessage"/> object
		/// </summary>
		/// <returns></returns>
		protected override object CreateBehavior()
		{
			return new AttachingMessage();
		}

		#region IEndpointBehavior Members

		void IEndpointBehavior.AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
		{
		}

		void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
			clientRuntime.MessageInspectors.Add(new ClientMessageInspector());
			CustomerChannel channel = new CustomerChannel();
			channel.ConstractName = clientRuntime.ContractClientType.FullName;
			clientRuntime.ChannelInitializers.Add(channel);
		}

		void IEndpointBehavior.ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
		{
		}

		void IEndpointBehavior.Validate(ServiceEndpoint endpoint)
		{
		}

		#endregion
	}
}
