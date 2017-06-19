using System;

namespace Arvato.CRM.Utility.WorkFlow
{
    /// <summary>
    /// 执行活动数据
    /// </summary>
    public class ExecuteArgs : EventArgs
    {
        /// <summary>
        /// 执行是否成功
        /// </summary>
        public bool Success { set; get; }

        public ExecuteArgs()
        {
            Success = true;
        }
    }
}
