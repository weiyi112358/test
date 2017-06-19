using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

namespace Arvato.CRM.Utility.WorkFlow.Templates
{
    /// <summary>
    /// 问题和答案
    /// </summary>
    public class QuestionAndAnswer
    {
        /// <summary>
        /// 问题列表
        /// </summary>
        [JsonProperty("questions")]
        public List<Question> Questions { set; get; }

        /// <summary>
        /// 答案列表
        /// </summary>
        [JsonProperty("answers")]
        public List<Answer> Answers { set; get; }

        /// <summary>
        /// 计算得分
        /// </summary>
        /// <returns>最小为0</returns>
        public int Calc()
        {
            int score = 0;
            foreach (var que in Questions)
            {
                if (Answers.Any(p => p.No == que.No && p.Key == que.Key))
                {
                    score++;
                }
            }
            return score;
        }
    }

    /// <summary>
    /// 问题
    /// </summary>
    public class Question
    {
        /// <summary>
        /// 编号
        /// </summary>
        [JsonProperty("no")]
        public int No { set; get; }

        /// <summary>
        /// 类型
        /// </summary>
        [JsonProperty("type")]
        public QuestionType Type { set; get; }

        /// <summary>
        /// 题目
        /// </summary>
        [JsonProperty("title")]
        public string Title { set; get; }

        /// <summary>
        /// 答案
        /// </summary>
        [JsonProperty("key")]
        public string Key { set; get; }

        /// <summary>
        /// 问题选项
        /// </summary>
        [JsonProperty("options")]
        public List<QuestionOption> Options { set; get; }
    }

    /// <summary>
    /// 问题选项
    /// </summary>
    public class QuestionOption
    {
        /// <summary>
        /// 编号
        /// </summary>
        [JsonProperty("no")]
        public int No { set; get; }


        /// <summary>
        /// 内容
        /// </summary>
        [JsonProperty("content")]
        public string Content { set; get; }
    }

    /// <summary>
    /// 问题类型
    /// </summary>
    public enum QuestionType
    {
        /// <summary>
        /// 是非题
        /// </summary>
        TF,
        /// <summary>
        /// 单选题
        /// </summary>
        SC,
        /// <summary>
        /// 多选题
        /// </summary>
        MT,
    }

    /// <summary>
    /// 问题答案
    /// </summary>
    public class Answer
    {
        /// <summary>
        /// 编号
        /// </summary>
        [JsonProperty("no")]
        public int No { set; get; }

        /// <summary>
        /// 答案
        /// </summary>
        [JsonProperty("key")]
        public string Key { set; get; }
    }
}
