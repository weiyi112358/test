using System;

namespace Arvato.CRM.WcfFramework.Attributes
{
    /// <summary>
    /// 用于标志WCF ServiceContract接口是否采用Protobuf做为序列化器 【hmliu】
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class ProtobufAttribute : Attribute
    {
    }
}
