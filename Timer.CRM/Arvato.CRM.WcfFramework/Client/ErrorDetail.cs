using System.Runtime.Serialization;

namespace Arvato.CRM.WcfFramework.ClientProxy
{
	/// <summary>
	/// 用于描述从WCF服务中返回的错误信息
	/// </summary>
	[DataContract]
	public class ErrorDetail
	{
		private string _errorCode;
		private string _errorMessage;
		private string _errorStackTrace;
		private byte[] exceptionData;

		/// <summary>
		/// 错误代码
		/// </summary>
		[DataMember]
		public string ErrorCode
		{
			get { return _errorCode; }
			set { _errorCode = value; }
		}

		/// <summary>
		/// 错误简述
		/// </summary>
		[DataMember]
		public string ErrorMessage
		{
			get { return _errorMessage; }
			set { _errorMessage = value; }
		}

		/// <summary>
		/// 堆栈详细信息
		/// </summary>
		[DataMember]
		public string ErrorStackTrace
		{
			get { return _errorStackTrace; }
			set { _errorStackTrace = value; }
		}

		/// <summary>
		/// 异常信息的二进制序列化数据
		/// </summary>
		[DataMember]
		public byte[] ExceptionData
		{
			get { return this.exceptionData; }
			set { this.exceptionData = value; }
		}
	}
}
