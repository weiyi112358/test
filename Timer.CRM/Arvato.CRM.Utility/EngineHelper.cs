using Arvato.CRM.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arvato.CRM.Utility
{
    public static class EngineHelper
    {
        public static string ConvertTimeString(string timeStr)
        {
            string str = timeStr.Trim();
            if (str.Substring(str.Length - 2, 2) == "PM")
            {
                string rst = (int.Parse(str.Substring(0, 2)) + 12).ToString() + str.Substring(2, str.Length - 4).Trim() + ":00";
                if (rst.Substring(0, 2) == "24") return "00" + rst.Substring(2, 6);
                else return rst;
            }
            else
            {
                return str.Substring(0, str.Length - 2).Trim() + ":00";
            }
        }

        public static bool CompareDateTimeIgnoreMillisecond(DateTime? oneTime, DateTime? anotherTime)
        {
            try
            {
                if (oneTime.HasValue)
                {
                    if (anotherTime.HasValue)
                    {
                        return oneTime.Value.ToString("yyyy-MM-dd HH:mm:ss").Equals(anotherTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (anotherTime.HasValue)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static int GetDayOfWeek(DateTime curDatetime)
        {
            switch (curDatetime.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return 1;
                case DayOfWeek.Tuesday:
                    return 2;
                case DayOfWeek.Wednesday:
                    return 3;
                case DayOfWeek.Thursday:
                    return 4;
                case DayOfWeek.Friday:
                    return 5;
                case DayOfWeek.Saturday:
                    return 6;
                case DayOfWeek.Sunday:
                    return 7;
                default:
                    return -1;
            }
        }

        public static int LastDayOfMonth(int year, int month)
        {
            return new DateTime(year, month, 1).AddMonths(1).AddDays(-1).Day;
        }

        public static string ReplaceDate(Match m)
        {
            return "'" + m.Value + "'";
        }


        public static TD_SYS_FieldAlias GetAlias(List<TD_SYS_FieldAlias> fieldAliasList, string matchStr)
        {
            TD_SYS_FieldAlias alias = null;
            foreach (var fl in fieldAliasList)
            {
                //全字匹配
                var index = matchStr.IndexOf(fl.FieldAlias);
                if (index > -1)
                {
                    if (index > 0)
                    {
                        if (matchStr.Substring(index - 1, 1) != " ") continue;
                    }
                    if (index + fl.FieldAlias.Length < matchStr.Length)
                    {
                        if (matchStr.Substring(index + fl.FieldAlias.Length, 1) != " ") continue;
                    }
                    alias = fl;
                    break;
                }
            }
            return alias;
        }


        //计算维度
        private static void computeDimension(CRMEntities db, string sqlForGetMemberList, DateTime curDatetime)
        {
            //拼接当前时间字符串
            string curTime = "'" + curDatetime.ToString("yyyyMMdd") + "'";
            //计算维度
            var query = from d in db.TD_SYS_FieldAlias
                        select new
                        {
                            d.ComputeScript,
                            d.FieldAlias,
                            d.FieldName
                        };

            var dcl = query.ToList();

            foreach (var dc in dcl)
            {

                string rrr = dc.ComputeScript.Replace("[Attr]", dc.FieldName).Replace("[MemberList]", sqlForGetMemberList).Replace("[DatetimeNow]", curTime).Replace("[Switch]", sqlForGetMemberList == "" ? "1" : "0");

                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandTimeout = 360;
                cmd.CommandText = rrr;
                cmd.CommandType = System.Data.CommandType.Text;
                if (db.Database.Connection.State == System.Data.ConnectionState.Closed) db.Database.Connection.Open();
                cmd.ExecuteNonQuery();

            }

        }

        public static string ConvertExpressionBaseByMember(List<TD_SYS_FieldAlias> fieldAliasList, string leftValue, string expression, string rightValue, DateTime curDatetime, string memberSourceTable, string where, string r, int j = 0)
        {
            //寻找别名
            var lv = GetAlias(fieldAliasList, leftValue);
            var rv = GetAlias(fieldAliasList, rightValue);

            //替换实际表名字段名
            if (lv.IsDynamicAlias.Value)
            {
                leftValue = leftValue.Replace(lv.FieldAlias, "[" + lv.TableName + "].[" + lv.FieldName + j.ToString() + "]");
            }
            else
            {
                leftValue = leftValue.Replace(lv.FieldAlias, "[" + lv.TableName + "].[" + lv.FieldName + "]");
            }
            if (rv != null) rightValue = rightValue.Replace(rv.FieldAlias, "[" + rv.TableName + "].[" + rv.FieldName + "]");

            //定义表达式
            var ep = "";

            var strArr = new string[] { "1", "2" };
            var dateArr = new string[] { "5", "6" };
            var strRv = (strArr.Contains(lv.FieldType) && rv == null) ? "'" + rightValue + "'" : rightValue;

            if (dateArr.Contains(lv.FieldType))
            {
                MatchEvaluator evaluator = new MatchEvaluator(ReplaceDate);
                string tmpStrRv = Regex.Replace(rightValue, @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}", evaluator);
                if (strRv == tmpStrRv) strRv = Regex.Replace(rightValue, @"\d{4}-\d{2}-\d{2}", evaluator);
                else strRv = tmpStrRv;
            }

            //当前日期时间的特殊处理
            strRv = strRv.Replace("DateNow", "'" + curDatetime.ToString("yyyy-MM-dd") + "'");
            strRv = strRv.Replace("DatetimeNow", "'" + curDatetime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'");

            //动态维度处理
            if (lv.IsDynamicAlias == true)
            {
                strRv = strRv.Split('|')[0];
            }

            //拼接表达式
            switch (expression)
            {
                case "=":
                    ep = leftValue + " = " + strRv;
                    break;
                case "<>":
                    ep = leftValue + " <> " + strRv;
                    break;
                case ">":
                    ep = leftValue + " > " + strRv;
                    break;
                case "<":
                    ep = leftValue + " < " + strRv;
                    break;
                case ">=":
                    ep = leftValue + " >= " + strRv;
                    break;
                case "<=":
                    ep = leftValue + " <= " + strRv;
                    break;
                case "like":
                    ep = leftValue + " like '%' + " + strRv + " + '%'";
                    break;
                case "notlike":
                    ep = leftValue + " not like '%' + " + strRv + " + '%'";
                    break;
                case "likebegin":
                    ep = leftValue + " like " + strRv + " + '%'";
                    break;
                case "likeend":
                    ep = leftValue + " like '%' + " + strRv;
                    break;
                default:
                    ep = leftValue + " = " + strRv;
                    break;
            }


            if (string.IsNullOrEmpty(lv.AliasType))
            {
                return r + " " + ep;
            }
            if (hasTypeDef(where, lv.AliasType, lv.AliasKey, lv.AliasSubKey))
            {
                string tmp = string.Format("[{0}{1}{2}]", lv.AliasType, lv.AliasKey, lv.AliasSubKey == null ? "" : lv.AliasSubKey);
                return where.Replace(tmp, r + " " + ep + " " + tmp + " ");
            }
            else//第一次出现
            {
                if (lv.AliasType == "MemberSubExt")
                {
                    return where + " " + r + " " + string.Format("exists(select 1 from TM_Mem_SubExt with(nolock) where {2}.MemberID = TM_Mem_SubExt.MemberID and TM_Mem_SubExt.ExtType = '{1}' and {0} [MemberSubExt{1}])", ep, lv.AliasKey, memberSourceTable);
                }
                else if (lv.AliasType == "MemberTrade")
                {
                    return where + " " + r + " " + string.Format("exists(select 1 from TM_Mem_Trade with(nolock) where {2}.MemberID = TM_Mem_Trade.MemberID and TM_Mem_Trade.TradeType = '{1}' and {0} [MemberTrade{1}])", ep, lv.AliasKey, memberSourceTable);
                }
                else if (lv.AliasType == "MemberTradeDetail")
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(string.Format("exists(select 1 from TM_Mem_Trade with(nolock) inner join TM_Mem_TradeDetail with(nolock) on TM_Mem_Trade.TradeID = TM_Mem_TradeDetail.TradeID and TM_Mem_Trade.TradeType = '{0}' ", lv.AliasKey));
                    sb.AppendFormat("where TM_Mem_Trade.MemberID = {2}.MemberID and TM_Mem_TradeDetail.TradeDetailType = '{0}' and {1} [MemberTradeDetail{3}{0}] [MemberTrade{3}])", lv.AliasSubKey, ep, memberSourceTable, lv.AliasKey);
                    return where + " " + r + " " + sb.ToString();
                }
            }
            return where + " " + r + " " + ep;
        }

        public static string ConvertExpressionBaseByMember(List<TD_SYS_FieldAlias> fieldAliasList, string leftValue, string expression, string rightValue, DateTime curDatetime, string memberSourceTable, string where, string r, bool isloyalty, int j = 0)
        {
            //寻找别名
            var lv = GetAlias(fieldAliasList, leftValue);
            var rv = GetAlias(fieldAliasList, rightValue);

            //替换实际表名字段名
            if (lv.IsDynamicAlias.Value)
            {
                leftValue = leftValue.Replace(lv.FieldAlias, "[" + lv.TableName + "].[" + lv.FieldName + j.ToString() + "]");
            }
            else
            {
                leftValue = leftValue.Replace(lv.FieldAlias, "[" + lv.TableName + "].[" + lv.FieldName + "]");
            }
            if (rv != null) rightValue = rightValue.Replace(rv.FieldAlias, "[" + rv.TableName + "].[" + rv.FieldName + "]");

            //定义表达式
            var ep = "";

            var strArr = new string[] { "1", "2" };
            var dateArr = new string[] { "5", "6" };
            var strRv = (strArr.Contains(lv.FieldType) && rv == null) ? "'" + rightValue + "'" : rightValue;

            if (dateArr.Contains(lv.FieldType))
            {
                MatchEvaluator evaluator = new MatchEvaluator(ReplaceDate);
                string tmpStrRv = Regex.Replace(rightValue, @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}", evaluator);
                if (strRv == tmpStrRv) strRv = Regex.Replace(rightValue, @"\d{4}-\d{2}-\d{2}", evaluator);
                else strRv = tmpStrRv;
            }

            //当前日期时间的特殊处理
            strRv = strRv.Replace("DateNow", "'" + curDatetime.ToString("yyyy-MM-dd") + "'");
            strRv = strRv.Replace("DatetimeNow", "'" + curDatetime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'");

            //动态维度处理
            if (lv.IsDynamicAlias == true)
            {
                strRv = strRv.Split('|')[0];
            }

            //拼接表达式
            switch (expression)
            {
                case "=":
                    ep = leftValue + " = " + strRv;
                    break;
                case "<>":
                    ep = leftValue + " <> " + strRv;
                    break;
                case ">":
                    ep = leftValue + " > " + strRv;
                    break;
                case "<":
                    ep = leftValue + " < " + strRv;
                    break;
                case ">=":
                    ep = leftValue + " >= " + strRv;
                    break;
                case "<=":
                    ep = leftValue + " <= " + strRv;
                    break;
                case "like":
                    ep = leftValue + " like '%' + " + strRv + " + '%'";
                    break;
                case "notlike":
                    ep = leftValue + " not like '%' + " + strRv + " + '%'";
                    break;
                case "likebegin":
                    ep = leftValue + " like " + strRv + " + '%'";
                    break;
                case "likeend":
                    ep = leftValue + " like '%' + " + strRv;
                    break;
                default:
                    ep = leftValue + " = " + strRv;
                    break;
            }


            if (string.IsNullOrEmpty(lv.AliasType))
            {
                return r + " " + ep;
            }
            if (hasTypeDef(where, lv.AliasType, lv.AliasKey, lv.AliasSubKey))
            {
                string tmp = string.Format("[{0}{1}{2}]", lv.AliasType, lv.AliasKey, lv.AliasSubKey == null ? "" : lv.AliasSubKey);
                return where.Replace(tmp, r + " " + ep + " " + tmp + " ");
            }
            else//第一次出现
            {
                if (lv.AliasType == "MemberSubExt")
                {
                    return where + " " + r + " " + string.Format("exists(select 1 from TM_Mem_SubExt with(nolock) where {2}.MemberID = TM_Mem_SubExt.MemberID and TM_Mem_SubExt.ExtType = '{1}' and {0} [MemberSubExt{1}]", ep, lv.AliasKey, memberSourceTable);
                }
                else if (lv.AliasType == "MemberTrade")
                {
                    return where + " " + r + " " + string.Format("exists(select 1 from TM_Mem_Trade with(nolock) where {2}.MemberID = TM_Mem_Trade.MemberID and TM_Mem_Trade.TradeType = '{1}' and {0} [MemberTrade{1}]", ep, lv.AliasKey, memberSourceTable);
                }
                else if (lv.AliasType == "MemberTradeDetail")
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(string.Format("exists(select 1 from TM_Mem_Trade with(nolock) inner join TM_Mem_TradeDetail with(nolock) on TM_Mem_Trade.TradeID = TM_Mem_TradeDetail.TradeID and TM_Mem_Trade.TradeType = '{0}' ", lv.AliasKey));
                    sb.AppendFormat("where TM_Mem_Trade.MemberID = {2}.MemberID and TM_Mem_TradeDetail.TradeDetailType = '{0}' and {1} [MemberTradeDetail{3}{0}] [MemberTrade{3}]", lv.AliasSubKey, ep, memberSourceTable, lv.AliasKey);
                    return where + " " + r + " " + sb.ToString();
                }
            }
            if (r == "")
                return where + " (" + r + " " + ep;
            else
                return where + " " + r + " " + ep;
        }


        //判断是否已经处理过某个类型的脚本定义
        private static bool hasTypeDef(string where, string aliasType, string aliasKey, string aliasSubKey)
        {
            switch (aliasType)
            {
                case "MemberSubExt":
                    if (where.Contains("TM_Mem_SubExt with(nolock)") && where.Contains(string.Format("TM_Mem_SubExt.ExtType = '{0}'", aliasKey))) return true;
                    break;
                case "MemberTrade":
                    if (where.Contains("TM_Mem_Trade with(nolock)") && where.Contains(string.Format("TM_Mem_Trade.TradeType = '{0}'", aliasKey))) return true;
                    break;
                case "MemberTradeDetail":
                    if (where.Contains("TM_Mem_TradeDetail with(nolock)") && where.Contains(string.Format("TM_Mem_Trade.TradeType = '{0}'", aliasKey)) && where.Contains(string.Format("TM_Mem_TradeDetail.TradeDetailType = '{0}'", aliasSubKey))) return true;
                    break;
                default:
                    return false;
            }

            return false;
        }

        public static string ConvertExpressionBaseByTradeDetail(List<TD_SYS_FieldAlias> fieldAliasList, string leftValue, string expression, string rightValue, DateTime curDatetime, string memberSourceTable)
        {
            //寻找别名
            var lv = GetAlias(fieldAliasList, leftValue);
            var rv = GetAlias(fieldAliasList, rightValue);

            //替换实际表名字段名
            leftValue = leftValue.Replace(lv.FieldAlias, "[" + lv.TableName + "].[" + lv.FieldName + "]");
            if (rv != null) rightValue = rightValue.Replace(rv.FieldAlias, "[" + rv.TableName + "].[" + rv.FieldName + "]");

            //定义表达式
            var ep = "";

            var strArr = new string[] { "1", "2" };
            var dateArr = new string[] { "5", "6" };
            var strRv = (strArr.Contains(lv.FieldType) && rv == null) ? "'" + rightValue + "'" : rightValue;

            if (dateArr.Contains(lv.FieldType))
            {
                MatchEvaluator evaluator = new MatchEvaluator(ReplaceDate);
                string tmpStrRv = Regex.Replace(rightValue, @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}", evaluator);
                if (strRv == tmpStrRv) strRv = Regex.Replace(rightValue, @"\d{4}-\d{2}-\d{2}", evaluator);
                else strRv = tmpStrRv;
            }

            //当前日期时间的特殊处理
            strRv = strRv.Replace("DateNow", "'" + curDatetime.ToString("yyyy-MM-dd") + "'");
            strRv = strRv.Replace("DatetimeNow", "'" + curDatetime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'");

            //拼接表达式
            switch (expression)
            {
                case "=":
                    ep = leftValue + " = " + strRv;
                    break;
                case "<>":
                    ep = leftValue + " <> " + strRv;
                    break;
                case ">":
                    ep = leftValue + " > " + strRv;
                    break;
                case "<":
                    ep = leftValue + " < " + strRv;
                    break;
                case ">=":
                    ep = leftValue + " >= " + strRv;
                    break;
                case "<=":
                    ep = leftValue + " <= " + strRv;
                    break;
                case "like":
                    ep = leftValue + " like '%' + " + strRv + " + '%'";
                    break;
                case "notlike":
                    ep = leftValue + " not like '%' + " + strRv + " + '%'";
                    break;
                case "likebegin":
                    ep = leftValue + " like " + strRv + " + '%'";
                    break;
                case "likeend":
                    ep = leftValue + " like '%' + " + strRv;
                    break;
                default:
                    ep = leftValue + " = " + strRv;
                    break;
            }


            if (string.IsNullOrEmpty(lv.AliasType))
            {
                return ep;
            }
            else if (lv.AliasType == "MemberSubExt")
            {
                return string.Format("exists(select 1 from TM_Mem_SubExt with(nolock) where {2}.MemberID = TM_Mem_SubExt.MemberID and TM_Mem_SubExt.ExtType = '{1}' and {0})", ep, lv.AliasKey, memberSourceTable);
            }
            return ep;
        }

        public static string GetDBFieldDefine(string fieldType)
        {
            switch (fieldType)
            {
                case "1":
                    return " nvarchar(20) ";
                case "2":
                    return " nvarchar(100) ";
                case "3":
                    return " int ";
                case "4":
                    return " bit ";
                case "5":
                    return " datetime ";
                case "6":
                    return " date ";
                case "7":
                    return " decimal(18,2) ";
                case "8":
                    return " decimal(18,4) ";
                default:
                    return "";
            }
        }
    }
}
