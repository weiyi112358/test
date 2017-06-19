using System;

namespace Arvato.CRM.WcfFramework.Attributes
{
    /// <summary>
    /// bll是否注册为wcf服务。默认不加标记为注册
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterAttribute : Attribute
    {
        private bool registered = true;

        public RegisterAttribute(bool registered)
        {
            this.registered = registered;
        }

        /// <summary>
        /// 是否注册为wcf服务
        /// </summary>
        public bool Registered
        {
            get { return registered; }
        }
    }
}
