using Arvato.CRM.EF;
using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Arvato.CRM.Trigger
{
    public abstract class BaseTrigger
    {
        public delegate void CallBackMethod(Result rst);

        /// <summary>
        /// 创建者
        /// </summary>
        public string AddedUser { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime AddedDate { get; set; }

        /// <summary>
        /// 触发器类型
        /// </summary>
        public TriggerType TriggerType { get; set; }

        /// <summary>
        /// 会员编号列表
        /// </summary>
        public List<string> MemberIDs { get; set; }

        /// <summary>
        /// 查找会员脚本
        /// </summary>
        public string MemberScript { get; set; }

        /// <summary>
        /// 启动时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 扩展数据
        /// </summary>
        public string ExtraData { get; set; }

        /// <summary>
        /// 成功回调事件
        /// </summary>
        public CallBackMethod Callback { get; set; }

        /// <summary>
        /// 执行触发器实例
        /// </summary>
        public abstract void Start();

        ///// <summary>
        ///// 记录执行
        ///// </summary>
        ///// <returns></returns>
        //protected void Log(CRMEntities db)
        //{
        //    var tr = new TL_SYS_Trigger
        //    {
        //        AddedDate = DateTime.Now,
        //        TriggerType = this.TriggerType.ToString(),
        //        Callback = this.Callback.ToString(),
        //        StartTime = this.StartTime,
        //        AddedUser = this.AddedUser,
        //        ExtraData = this.ExtraData,
        //        MemberIDs = string.Join(",", this.MemberIDs),
        //        MemberScript = this.MemberScript
        //    };
        //    db.TL_SYS_Trigger.Add(tr);
        //    db.SaveChanges();
        //}
    }

    public enum TriggerType
    {
        SMS = 1,
        EDM = 2,
        Account = 3,
        Loyalty = 4
    }
}
