namespace Arvato.CRM.Utility.WorkFlow
{

    /// <summary>
    /// 活动状态
    /// </summary>
    public enum ActivityState
    {
        /// <summary>
        /// 已放弃
        /// </summary>
        Abandon = -2,
        /// <summary>
        /// 已暂停
        /// </summary>
        Paused = -1,
        /// <summary>
        /// 初始
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 等待执行
        /// </summary>
        Waiting = 1,
        /// <summary>
        /// 执行中
        /// </summary>
        Running = 2,
        /// <summary>
        /// 执行完成
        /// </summary>
        Finished = 3,
        /// <summary>
        /// 已过期
        /// </summary>
        Expired = 4,
    }
}
