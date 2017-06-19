using Arvato.CRM.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Trigger
{
    public class SMSTrigger : BaseTrigger
    {
        public ExtraSMS Ext;

        public SMSTrigger(ExtraSMS ext)
        {
            TriggerType = Trigger.TriggerType.SMS;
            Ext = ext;
            ExtraData = Utility.JsonHelper.Serialize(ext);
        }

        public override void Start()
        {
            using (CRMEntities db = new CRMEntities())
            {
                //会员数据准备
                //To Do

                //获取模板
                var t = db.TM_Act_CommunicationTemplet.Single(o => o.TempletID == Ext.TempletID);

                //插入短信发送队列
                //To Do

                //Log
                //base.Log(db);
            }

            //根据执行情况判断调用回调函数
            //To Do
        }
    }

    /// <summary>
    /// 短信触发器扩展结构体
    /// </summary>
    public struct ExtraSMS
    {
        public int TempletID { get; set; }//短信模板
        public string SMSContent { get; set; }//短信内容（与模板互斥，优先模板）
    }
}
