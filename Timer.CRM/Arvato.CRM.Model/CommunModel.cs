using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
    #region 推送实时签到记录
    /// <summary>
    /// 推送实时签到记录输入参数
    /// </summary>
    public class WeChatMemberSignInput
    {
        public string memberOpenID { get; set; }
        public string activeName { get; set; }
        public string activeCode { get; set; }
        public DateTime signDate { get; set; }
    }
    /// <summary>
    /// 推送实时签到记录返回参数
    /// </summary>
    public class WeChatMemberSignOutput
    {
        public int status { get; set; }
        public string errorMsg { get; set; }
    }

    #endregion

    #region 消息模板接口
    //消息模板返回值（主）
    public class MessageTemplateOutput
    {
        public string sign { get; set; }
        public ContentDetail content { get; set; }
    }
    //content详细（从）
    public class ContentDetail
    {
        public string touser { get; set; }
        public string template_id { get; set; }
        public string url { get; set; }
        public string topcolor { get; set; }
        public DataDetail data { get; set; }
    }
    //data详细
    public class DataDetail
    {
        public KeyWordDeatil first { get; set; }
        public KeyWordDeatil keyword1 { get; set; }
        public KeyWordDeatil keyword2 { get; set; }
        public KeyWordDeatil keyword3 { get; set; }
        public KeyWordDeatil remark { get; set; }
    }

    /// <summary>
    /// 会员卡号 
    /// </summary>
    public class KeyWordDeatil
    {
        public string value { get; set; }
        public string color { get; set; }
    }


    #endregion

    #region 微信群转发接口
    /// <summary>
    /// 微信群转发
    /// </summary>
    public class WechatGroupForWardingOutput
    {
        public string sign { get; set; }
        public ContentsDetail content { get; set; }
    }
    public class ContentsDetail
    {
        public string[] touser { get; set; }
        public MpnewsDetail mpnews { get; set; }
        public string msgtype { get; set; }
    }
    public class MpnewsDetail
    {
        public string media_id { get; set; }
    }
    #endregion
}
