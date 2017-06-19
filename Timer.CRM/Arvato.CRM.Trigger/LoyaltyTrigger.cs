using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Trigger
{
    public class LoyaltyTrigger : BaseTrigger, IDisposable
    {
        public ExtraLoyalty Ext;
        private bool IsMyDB;
        private static List<TD_SYS_FieldAlias> listAliasBuff;
        private static DateTime lastAliasBuffTime = DateTime.Now;
        private static List<TR_SYS_InterfaceRule> listiRuleBuff;
        private static DateTime lastiRuleBuffTime = DateTime.Now;
        public LoyaltyTrigger(ExtraLoyalty ext)
        {
            TriggerType = Trigger.TriggerType.Loyalty;
            Ext = ext;
            if (Ext.db == null) { Ext.db = new CRMEntities(); IsMyDB = true; } else { IsMyDB = false; }
            //ExtraData = Utility.JsonHelper.Serialize(ext);
        }

        public override void Start()
        {
            //获取可执行规则
            Ext.LoyaltyRuleList = getAvaiRule(this.StartTime, Ext.db);
            Log4netHelper.WriteInfoLog("1");
            Log4netHelper.WriteInfoLog(JsonHelper.Serialize(Ext.LoyaltyRuleList));
            if (Ext.RuleType != null) Ext.LoyaltyRuleList = Ext.LoyaltyRuleList.Where(o => o.RuleType == Ext.RuleType).ToList();
            Ext.LoyaltyRuleList = Ext.LoyaltyRuleList.Where(o => o.DataGroupID == Ext.DataGroupID).ToList();
            //查询会员ID集合
            if (MemberIDs != null)
            {
                MemberScript = "Select MemberID From TM_Mem_Master Where MemberID in (" + string.Join(",", this.MemberIDs) + ")";
            }
            //查询交易信息
            if (Ext.TradeList != null)
            {
                Ext.SearchTradeSQL = "Select TradeID From TM_Mem_Trade Where NeedLoyCompute = 1 and TradeID in (" + string.Join(",", Ext.TradeList.Select(o => o.TradeID)) + ")";
            }
            if (Ext.TradeDetaiList != null)
            {
                Ext.SearchTradeDetailSQL = "Select TradeDetailID From TM_Mem_TradeDetail Where TradeDetailID in (" + string.Join(",", Ext.TradeDetaiList.Select(o => o.TradeDetailID)) + ") and TradeID in (" + Ext.SearchTradeSQL + ")";
            }
            //别名字段信息
            List<TD_SYS_FieldAlias> listAlias;
            if (listAliasBuff == null || (DateTime.Now - lastAliasBuffTime).Hours > 1)
            {
                listAliasBuff = Ext.db.TD_SYS_FieldAlias.ToList();
                lastAliasBuffTime = DateTime.Now;
            }
            listAlias = listAliasBuff;

            List<TR_SYS_InterfaceRule> listRule;
            if (listiRuleBuff == null || (DateTime.Now - lastiRuleBuffTime).Hours > 1)
            {
                listiRuleBuff = Ext.db.TR_SYS_InterfaceRule.ToList();
                lastiRuleBuffTime = DateTime.Now;
            }
            listRule = listiRuleBuff;

            //循环执行规则
            foreach (var rule in Ext.LoyaltyRuleList.OrderBy(o => o.RunIndex))
            {
                if (!String.IsNullOrEmpty(Ext.InterfaceName))
                {
                    var r = listRule.Where(p => p.InterfaceName == Ext.InterfaceName && p.RuleID == rule.RuleID).FirstOrDefault();
                    if (r == null) continue;
                }

                var Remark = "忠诚度发放";
                LoyaltySchedule s = Utility.JsonHelper.Deserialize<LoyaltySchedule>(rule.Schedule);
                if (!string.IsNullOrEmpty(s.Remark)) Remark = s.Remark;
                //规则执行脚本
                StringBuilder ruleSql = new StringBuilder();
                //计算所需的维度          
                string sqlForGetMemberList = this.MemberScript;
                computeDimensionByRule(sqlForGetMemberList, StartTime, Ext.db, rule, listAlias);

                #region 增量规则反算所有订单涉及的权益
                if (rule.RuleType == "1")
                {
                    //var a = this.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    //var b = StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ruleSql.AppendLine("insert into TL_Mem_AccountChange");
                    ruleSql.AppendLine("(");
                    ruleSql.AppendLine("AccountID         ,");
                    ruleSql.AppendLine("MemberID          ,");
                    ruleSql.AppendLine("AccountDetailID   ,");
                    ruleSql.AppendLine("AccountChangeType ,");
                    ruleSql.AppendLine("ChangeValue       ,");
                    ruleSql.AppendLine("ChangeReason      ,");
                    ruleSql.AppendLine("ReferenceNo       ,");
                    ruleSql.AppendLine("HasReverse        ,");
                    ruleSql.AppendLine("AddedDate         ,");
                    ruleSql.AppendLine("AddedUser");
                    ruleSql.AppendLine(")");
                    ruleSql.AppendLine("select ");
                    ruleSql.AppendLine("AccountID ,");
                    ruleSql.AppendLine("MemberID         ,");
                    ruleSql.AppendLine("AccountDetailID   ,");
                    ruleSql.AppendLine("'loy' ,");
                    ruleSql.AppendLine("-1*ChangeValue       ,");
                    ruleSql.AppendLine("'对冲操作'      ,");
                    ruleSql.AppendLine("ReferenceNo       ,");
                    ruleSql.AppendLine("1,");
                    ruleSql.AppendLine("'" + StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "',");
                    ruleSql.AppendLine("'1000'");
                    ruleSql.AppendLine("from (" + Ext.SearchTradeSQL + ") a");
                    ruleSql.AppendLine("inner join TL_Mem_AccountChange b with (nolock)  on cast(a.tradeid as nvarchar(10))=b.ReferenceNo");
                    ruleSql.AppendLine("where b.AccountChangeType ='loy' and HasReverse=0 and b.AddedDate <>'" + StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "'");

                    ruleSql.AppendLine("update TM_Mem_AccountDetail");
                    ruleSql.AppendLine("set DetailValue =DetailValue+temp.ChangeValue,ModifiedDate=getdate(),ModifiedUser='1000'");
                    ruleSql.AppendLine("from");
                    ruleSql.AppendLine("(select b.AccountID,b.AccountDetailID,sum(-1*ChangeValue) ChangeValue");
                    ruleSql.AppendLine(" from  (" + Ext.SearchTradeSQL + ") a");
                    ruleSql.AppendLine("inner join TL_Mem_AccountChange b with (nolock)  on cast(a.tradeid as nvarchar(10))=b.ReferenceNo");
                    ruleSql.AppendLine("where b.AccountChangeType ='loy' and HasReverse=0 and b.AddedDate <>'" + StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                    ruleSql.AppendLine("group by b.AccountID,b.AccountDetailID) temp ");
                    ruleSql.AppendLine("where TM_Mem_AccountDetail.AccountID =temp.AccountID and TM_Mem_AccountDetail.AccountDetailID =temp.AccountDetailID");

                    ruleSql.AppendLine("update TM_Mem_Account");
                    ruleSql.AppendLine("set TM_Mem_Account.Value1 =TM_Mem_Account.Value1+temp.value1,");
                    ruleSql.AppendLine("    TM_Mem_Account.Value2 =TM_Mem_Account.Value2+temp.value2,");
                    ruleSql.AppendLine("	TM_Mem_Account.ModifiedDate=getdate()");
                    ruleSql.AppendLine("from ");
                    ruleSql.AppendLine("(");
                    ruleSql.AppendLine("select AccountID,isnull(value1,0)  value1,isnull(value2,0)  value2,isnull(value3,0)  value3");
                    ruleSql.AppendLine("from");
                    ruleSql.AppendLine("(");
                    ruleSql.AppendLine("select  m.AccountID,n.AccountDetailType,m.ChangeValue");
                    ruleSql.AppendLine("from ");
                    ruleSql.AppendLine("(select b.AccountID,b.AccountDetailID,sum(-1*ChangeValue) ChangeValue");
                    ruleSql.AppendLine(" from (" + Ext.SearchTradeSQL + ") a");
                    ruleSql.AppendLine("inner join TL_Mem_AccountChange b with (nolock) on cast(a.tradeid as nvarchar(10))=b.ReferenceNo");
                    ruleSql.AppendLine("where b.AccountChangeType ='loy' and HasReverse=0 and b.AddedDate <>'" + StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                    ruleSql.AppendLine("group by b.AccountID,b.AccountDetailID) m");
                    ruleSql.AppendLine("inner join  TM_Mem_AccountDetail  n  with (nolock)  on m.AccountID=n.AccountID and m.AccountDetailID=n.AccountDetailID");
                    ruleSql.AppendLine("group by m.AccountID,n.AccountDetailType,m.ChangeValue) t ");
                    ruleSql.AppendLine(" pivot (max(ChangeValue) for AccountDetailType in ([value1],[value2],[value3])) w   ) temp");
                    ruleSql.AppendLine(" where TM_Mem_Account.AccountID=temp.AccountID");

                    ruleSql.AppendLine("update TL_Mem_AccountChange set HasReverse=1 ");
                    ruleSql.AppendLine(" from (" + Ext.SearchTradeSQL + ") a");
                    ruleSql.AppendLine("inner join TL_Mem_AccountChange b with (nolock) on cast(a.tradeid as nvarchar(10))=b.ReferenceNo ");
                    ruleSql.AppendLine("where b.AccountChangeType ='loy' and HasReverse=0 and b.AddedDate <>'" + StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                }
                #endregion

                //反序列化规则用Json数据
                Ral ral = JsonHelper.Deserialize<Ral>(rule.Condition);
                List<Act> actList = JsonHelper.Deserialize<List<Act>>(rule.Action);
                List<ConditionResult> conditionResultList = JsonHelper.Deserialize<List<ConditionResult>>(rule.ConditionResult);

                //会员命中脚本
                string sqlForGetMemberByCondition = filterMembersByCondition(ral, rule, sqlForGetMemberList, StartTime, listAlias, "tmpMemberTable");

                //中间计算临时表名
                string tmpCondition = "tmpCondition" + DateTime.Now.DayOfYear.ToString() + DateTime.Now.ToString("hhmmss");
                string tmpTradeDetail = "tmpTradeDetail" + DateTime.Now.DayOfYear.ToString() + DateTime.Now.ToString("hhmmss");
                string TETable1 = "TETable1" + DateTime.Now.DayOfYear.ToString() + DateTime.Now.ToString("hhmmss");
                string TETable2 = "TETable2" + DateTime.Now.DayOfYear.ToString() + DateTime.Now.ToString("hhmmss");
                string TETable3 = "TETable3" + DateTime.Now.DayOfYear.ToString() + DateTime.Now.ToString("hhmmss");
                string tmpTradeExt = "tmpTradeExt" + DateTime.Now.DayOfYear.ToString() + DateTime.Now.ToString("hhmmss");

                //交易命中脚本
                string sqlForGetTradeDetailByCondition = "";
                string sqlForGetTradeByCondition = "";
                if (conditionResultList.Count != 0)//需要对交易明细汇总
                {
                    //获取交易命中脚本
                    sqlForGetTradeDetailByCondition = filterTradeDetailByCondition(ral, rule, sqlForGetMemberByCondition, Ext.SearchTradeSQL, Ext.SearchTradeDetailSQL, StartTime, listAlias, "tmpMemberTable");
                    //创建交易明细临时表
                    ruleSql.AppendLine("CREATE TABLE [dbo].[" + tmpTradeDetail + "](");
                    ruleSql.AppendLine("	[TradeDetailID] [bigint]  NOT NULL");
                    ruleSql.AppendLine(") ON [PRIMARY]");

                    ruleSql.AppendLine("INSERT INTO [dbo].[" + tmpTradeDetail + "]");
                    ruleSql.AppendLine("           ([TradeDetailID])");
                    ruleSql.AppendLine("");
                    ruleSql.AppendLine("" + sqlForGetTradeDetailByCondition + "");

                    sqlForGetTradeDetailByCondition = "(select TradeDetailID from " + tmpTradeDetail + ")";
                    //中间计算脚本
                    //创建计算规则临时表
                    ruleSql.AppendLine("CREATE TABLE [dbo].[" + tmpCondition + "](");
                    ruleSql.AppendLine("	[FieldAlias] [nvarchar](50) NOT NULL,");
                    ruleSql.AppendLine("	[Maximum] [decimal](18, 2) NULL,");
                    ruleSql.AppendLine("	[Minimum] [decimal](18, 2) NULL,");
                    ruleSql.AppendLine("	[GroupFunc] [nvarchar](10) NOT NULL,");
                    ruleSql.AppendLine("	[OffsetExpression] [nvarchar](10) NULL,");
                    ruleSql.AppendLine("	[OffsetValue] [decimal](18, 2) NULL,");
                    ruleSql.AppendLine("	[IsABS] [bit] NOT NULL,");
                    ruleSql.AppendLine("	[NameCode] [nvarchar](50) NOT NULL");
                    ruleSql.AppendLine(" CONSTRAINT [PK_" + tmpCondition + "] PRIMARY KEY CLUSTERED");
                    ruleSql.AppendLine("(");
                    ruleSql.AppendLine("	[NameCode] ASC");
                    ruleSql.AppendLine(")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                    ruleSql.AppendLine(") ON [PRIMARY]");

                    //插入数据
                    foreach (var cr in conditionResultList)
                    {
                        ruleSql.AppendLine("INSERT INTO [dbo].[" + tmpCondition + "]");
                        ruleSql.AppendLine("           ([FieldAlias]");
                        ruleSql.AppendLine("           ,[Maximum]");
                        ruleSql.AppendLine("           ,[Minimum]");
                        ruleSql.AppendLine("           ,[GroupFunc]");
                        ruleSql.AppendLine("           ,[OffsetExpression]");
                        ruleSql.AppendLine("           ,[OffsetValue]");
                        ruleSql.AppendLine("           ,[IsABS]");
                        ruleSql.AppendLine("           ,[NameCode])");
                        ruleSql.AppendLine("     VALUES");
                        ruleSql.AppendLine("('" + cr.FieldAlias + "'");
                        ruleSql.AppendLine("," + (cr.Maximum.HasValue ? cr.Maximum.ToString() : "null"));
                        ruleSql.AppendLine("," + (cr.Minimum.HasValue ? cr.Minimum.ToString() : "null"));
                        ruleSql.AppendLine(",'" + cr.GroupFunc + "'");
                        ruleSql.AppendLine("," + (string.IsNullOrEmpty(cr.OffsetExpression) ? "null" : ("'" + cr.OffsetExpression + "'")));
                        ruleSql.AppendLine("," + (cr.OffsetValue.HasValue ? cr.OffsetValue.ToString() : "null"));
                        ruleSql.AppendLine("," + (cr.IsABS ? "1" : "0"));
                        ruleSql.AppendLine(",'" + cr.NameCode + "')");
                    }

                    //计算交易的汇总数据

                    ruleSql.AppendLine("exec('select cast('''' as nvarchar(max)) AS SQL into " + TETable1 + "')");
                    ruleSql.AppendLine("declare");
                    ruleSql.AppendLine("@Sqlx nvarchar(max) ,");
                    ruleSql.AppendLine("@Sqly nvarchar(max) ,");
                    ruleSql.AppendLine("@Sqlz nvarchar(max) ,");
                    ruleSql.AppendLine("@Sqlw nvarchar(max) ,");
                    ruleSql.AppendLine("@Sql  nvarchar(max) ,");
                    ruleSql.AppendLine("@ReturnSql  nvarchar(max) ,");
                    ruleSql.AppendLine("@ReturnValue  nvarchar(max)");


                    ruleSql.AppendLine("DECLARE TradeCursor CURSOR");
                    ruleSql.AppendLine("FOR");
                    ruleSql.AppendLine("select a.FieldAlias,a.groupfunc,a.offsetexpression,a.offsetvalue,a.namecode,a.isabs,");
                    ruleSql.AppendLine("  case when b.TableName='TM_Mem_TradeDetail' and b.AliasKey is not null and  b.AliasSubKey is not  null");
                    ruleSql.AppendLine("       then  'V_M_'+B.TableName+'_'+b.AliasKey+'_'+b.AliasSubKey ");
                    ruleSql.AppendLine("	   when b.TableName='TM_Mem_Trade' and b.AliasKey is not null and  b.AliasSubKey is   null ");
                    ruleSql.AppendLine("       then  'V_M_'+B.TableName+'_'+b.AliasKey");
                    ruleSql.AppendLine("  end V_TableName");
                    ruleSql.AppendLine("from  " + tmpCondition + "  a");
                    ruleSql.AppendLine("inner join TD_SYS_FieldAlias b on a.FieldAlias=b.FieldAlias");

                    ruleSql.AppendLine("OPEN TradeCursor");
                    ruleSql.AppendLine("DECLARE @FieldAlias nvarchar(200) ,");
                    ruleSql.AppendLine("		@groupfunc nvarchar(100) ,");
                    ruleSql.AppendLine("        @offsetexpression 	nvarchar(100) ,");
                    ruleSql.AppendLine("		@offsetvalue 	nvarchar(100),");
                    ruleSql.AppendLine("		@namecode nvarchar(100) ,");
                    ruleSql.AppendLine("		@isabs  nvarchar(2) ,");
                    ruleSql.AppendLine("        @V_TableName 	nvarchar(100)");
                    ruleSql.AppendLine("FETCH NEXT FROM  TradeCursor INTO @FieldAlias,@groupfunc,@offsetexpression,@offsetvalue,@namecode,@isabs,@V_TableName");
                    ruleSql.AppendLine("WHILE @@FETCH_STATUS =0	");
                    ruleSql.AppendLine("BEGIN");
                    ruleSql.AppendLine("DECLARE @sql1 nvarchar(max) ,");
                    ruleSql.AppendLine("        @sql_search1 nvarchar(max)='where 1=1 ' ,");
                    ruleSql.AppendLine("		@sql_search2a nvarchar(max)='' ,");
                    ruleSql.AppendLine("		@sql_search2b nvarchar(max)='' ,");
                    ruleSql.AppendLine("		@sql_search3a nvarchar(max)='' ,");
                    ruleSql.AppendLine("		@sql_search3b nvarchar(max)='' ,");
                    ruleSql.AppendLine("		@sql_search4 nvarchar(max)='' ,");
                    ruleSql.AppendLine("		@sql_search5 nvarchar(max)=''");
                    ruleSql.AppendLine("    if (@isabs ='1')");
                    ruleSql.AppendLine("	begin");
                    ruleSql.AppendLine("		set @sql_search2a = @sql_search2a  + 'abs('");
                    ruleSql.AppendLine("		set @sql_search2b = @sql_search2b  + ')'");
                    ruleSql.AppendLine("	end");
                    ruleSql.AppendLine("    if (@groupfunc is not null )");
                    ruleSql.AppendLine("	begin");
                    ruleSql.AppendLine("		set @sql_search3a = @sql_search3a  + @groupfunc+'('");
                    ruleSql.AppendLine("		set @sql_search3b = @sql_search3b  + ')'");
                    ruleSql.AppendLine("	end ");
                    ruleSql.AppendLine("    if (@offsetexpression is not null )");
                    ruleSql.AppendLine("	begin");
                    ruleSql.AppendLine("		set @sql_search4 = @sql_search4+@offsetexpression ");
                    ruleSql.AppendLine("	end");
                    ruleSql.AppendLine("    if (@offsetvalue is not null )");
                    ruleSql.AppendLine("	begin");
                    ruleSql.AppendLine("		set @sql_search5 = @sql_search5+@offsetvalue");
                    ruleSql.AppendLine("	end");

                    ruleSql.AppendLine("set @sql1='");
                    ruleSql.AppendLine("union all");
                    ruleSql.AppendLine("select a.tradeid ,'''''+@namecode+'''''  as type,");
                    ruleSql.AppendLine("'+@sql_search2a+''+@sql_search3a+@FieldAlias+@sql_search3b+@sql_search4+''+@sql_search5+''+@sql_search2b+'  as amt");
                    ruleSql.AppendLine("from '+@V_TableName+'  a");
                    ruleSql.AppendLine("inner join " + sqlForGetTradeDetailByCondition + " b  on a.tradedetailid=b.tradedetailid ");
                    ruleSql.AppendLine("'+@sql_search1+'");
                    ruleSql.AppendLine("group by a.tradeid ");
                    ruleSql.AppendLine("union all");
                    ruleSql.AppendLine("select 0 tradeid,'''''+@namecode+''''' as type, 0 amt");
                    ruleSql.AppendLine("'");

                    ruleSql.AppendLine("exec('update " + TETable1 + " set sql= isnull(sql,'''')'+'+  '''+@sql1+'''')");

                    ruleSql.AppendLine("FETCH NEXT FROM  TradeCursor INTO  @FieldAlias,@groupfunc,@offsetexpression,@offsetvalue,@namecode,@isabs,@V_TableName");
                    ruleSql.AppendLine("END");
                    ruleSql.AppendLine("CLOSE TradeCursor");
                    ruleSql.AppendLine("DEALLOCATE TradeCursor");

                    ruleSql.AppendLine("exec('update " + TETable1 + " set sql= ''select * into " + TETable2 + " from (''+ stuff(sql,1,12,'''') +'') t'' ')");
                    ruleSql.AppendLine("set @sqlx = 'select @ct=sql from " + TETable1 + "'");
                    ruleSql.AppendLine("exec sp_executesql @sqlx,N'@ct nvarchar(max) output',@ReturnSql output ");
                    ruleSql.AppendLine("exec(@ReturnSql)");

                    ruleSql.AppendLine("set @sqly=' select @ct1 = isnull(@ct1 + ''],['' ,'''') + type  from " + TETable2 + "  group by type '");
                    ruleSql.AppendLine("exec sp_executesql @sqly,N'@ct1 nvarchar(max) output',@ReturnValue output ");
                    ruleSql.AppendLine("set @ReturnValue = '[' + @ReturnValue + ']'");

                    ruleSql.AppendLine("update " + TETable2);
                    ruleSql.AppendLine("set amt =");
                    ruleSql.AppendLine("case when amt is not null and temp.maximum is null  and temp.minimum  is null  then amt");
                    ruleSql.AppendLine("     when amt is not null and temp.maximum is not null  and temp.minimum  is not  null  and  amt>temp.maximum  then temp.maximum");
                    ruleSql.AppendLine("	 when amt is not null and temp.maximum is not null  and temp.minimum  is not  null  and  amt<temp.minimum  then temp.minimum");
                    ruleSql.AppendLine("	 when amt is not null and temp.maximum is not null  and temp.minimum  is not  null  and  amt<=temp.maximum  and   amt>=temp.minimum  then amt");
                    ruleSql.AppendLine("	 when amt is not null and temp.maximum is not null  and temp.minimum  is null and  amt>temp.maximum  then temp.maximum");
                    ruleSql.AppendLine("	 when amt is not null and temp.maximum is not null  and temp.minimum  is null and  amt<=temp.maximum  then amt");
                    ruleSql.AppendLine("	 when amt is not null and temp.maximum is  null  and temp.minimum  is not null and  amt>temp.minimum  then amt");
                    ruleSql.AppendLine("	 when amt is not null and temp.maximum is  null  and temp.minimum  is not null and  amt<temp.minimum  then temp.minimum");
                    ruleSql.AppendLine("     when amt is null then  amt  end");
                    ruleSql.AppendLine("from " + tmpCondition + " temp");
                    ruleSql.AppendLine("where TYPE=temp.NameCode");

                    ruleSql.AppendLine("exec ('select * into " + TETable3);
                    ruleSql.AppendLine("       from (select * from " + TETable2 + ") a pivot (max(amt) for type in (' + @ReturnValue + ')) b')");

                    ruleSql.AppendLine("select b.memberid,a.*");
                    ruleSql.AppendLine("into " + tmpTradeExt);
                    ruleSql.AppendLine("from " + TETable3 + "   a");
                    ruleSql.AppendLine("inner join (select c.TradeID,c.memberid");
                    ruleSql.AppendLine("from  " + sqlForGetTradeDetailByCondition + "  a");
                    ruleSql.AppendLine("inner join TM_Mem_TradeDetail b   on a.tradedetailid=b.tradedetailid");
                    ruleSql.AppendLine("inner join TM_Mem_Trade c on b.tradeid=c.tradeid");
                    ruleSql.AppendLine("group by c.TradeID,c.memberid  ) b on a.tradeid=b.TradeID ");
                    ruleSql.AppendLine("");
                    ruleSql.AppendLine("drop table " + tmpTradeDetail + "");
                }

                //执行行为的脚本
                //获取交易命中脚本
                sqlForGetTradeByCondition = filterTradeByCondition(ral, rule, sqlForGetMemberByCondition, Ext.SearchTradeSQL, StartTime, listAlias, "tmpMemberTable1");

                string actionScript = createActionScript_bk(actList, listAlias, conditionResultList, sqlForGetTradeByCondition, sqlForGetMemberByCondition, this.StartTime, conditionResultList.Count != 0 ? tmpTradeExt : "", rule.RuleID, Remark);

                ruleSql.Append(actionScript);

                if (conditionResultList.Count != 0)
                {
                    ruleSql.AppendLine("Drop Table " + tmpCondition);
                    ruleSql.AppendLine("Drop Table " + tmpTradeExt);
                }

                //更新规则上次执行时间
                ruleSql.AppendLine("");
                ruleSql.AppendLine("update [TM_Loy_Rule] set LastExcuteTime = '" + StartTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' where RuleID = " + rule.RuleID.ToString());

                var cmd = Ext.db.Database.Connection.CreateCommand();
                cmd.CommandTimeout = 1800;
                cmd.CommandText = ruleSql.ToString();
                cmd.CommandType = System.Data.CommandType.Text;
                //Log4netHelper.WriteInfoLog(ruleSql.ToString());
                if (Ext.db.Database.Connection.State == System.Data.ConnectionState.Closed) Ext.db.Database.Connection.Open();
                //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                //stopwatch.Start();
                //Log4netHelper.WriteInfoLog("执行开始 :" + rule.RuleName + "开始：" + stopwatch.ElapsedMilliseconds);
                Log4netHelper.WriteInfoLog(ruleSql.ToString());
                cmd.ExecuteNonQuery();
                Log4netHelper.WriteInfoLog(ruleSql.ToString());
                //Ext.db.Database.ExecuteSqlCommand(ruleSql.ToString());
                //Log4netHelper.WriteInfoLog("执行结束 :" + rule.RuleName + "结束：" + stopwatch.ElapsedMilliseconds);
                //计算规则后应计算的维度
                computeAfterRuleDimension(Ext.db, sqlForGetMemberList, this.StartTime, rule.RuleID);

                if (!string.IsNullOrEmpty(rule.ComputeScript))
                {
                    Ext.db.Database.ExecuteSqlCommand(string.Format(rule.ComputeScript, sqlForGetMemberList));
                }

                //Log4netHelper.WriteInfoLog("执行结束 :" + rule.RuleName + "结束：" + stopwatch.ElapsedMilliseconds);
            }
            //销毁自己建立的数据库
            Dispose();

        }

        //创建行为脚本
        private static string createActionScript(List<Act> actList, List<TD_SYS_FieldAlias> listAlias, List<ConditionResult> conditionResultList, string tmpTradeTableName, string getMemberScript, DateTime curDatetime, string tmpTradeExt, int ruleId, string Remark)
        {
            string[] strTypes = new string[] { "1", "2" };
            string[] dateTypes = new string[] { "5", "6" };
            StringBuilder actionSql = new StringBuilder();

            string tmpMember = "tmpMember" + curDatetime.DayOfYear.ToString() + curDatetime.ToString("hhmmss");

            //根据条件把会员数据插入临时表
            actionSql.AppendLine("CREATE TABLE " + tmpMember + "(");
            actionSql.AppendLine("	[MemberID] [char](32) NOT NULL");
            actionSql.AppendLine(") ON [PRIMARY]");
            actionSql.AppendLine("insert into " + tmpMember);
            actionSql.AppendLine(getMemberScript.Substring(1, getMemberScript.Length - 2));

            foreach (var act in actList.OrderBy(o => o.Sort))
            {
                //账户处理
                if (act.LeftValue.ExtName == "Account")
                {
                    #region 账户累加处理
                    string tmpRuleAct = "tmpRuleAct" + act.Sort.ToString() + curDatetime.DayOfYear.ToString() + curDatetime.ToString("hhmmss");
                    TD_SYS_FieldAlias rightValue;

                    if (!string.IsNullOrEmpty(act.RightValueMax) || !string.IsNullOrEmpty(act.RightValueMin))
                    {
                        #region 如果有上下限过滤，则将数据插入临时表
                        //临时表名定义
                        string tmpTrade = "tmpTrade" + act.Sort.ToString() + curDatetime.DayOfYear.ToString() + curDatetime.ToString("hhmmss");

                        //创建临时表
                        actionSql.AppendLine("CREATE TABLE [" + tmpTrade + "](");
                        actionSql.AppendLine("  [Memberid] [nvarchar](50) NULL,");
                        actionSql.AppendLine("	[TradeID] [bigint] NULL,");
                        actionSql.AppendLine("	[ComputeValue] [decimal](18, 4) NULL,");
                        actionSql.AppendLine("	[NodeValue] [decimal](18, 2) NULL,");
                        actionSql.AppendLine("	[LoyAccBeforeValue] [decimal](18, 4) NULL,");
                        actionSql.AppendLine("	[Addeddate] [datetime] NULL,");
                        actionSql.AppendLine("	[Sort] [bigint] NULL");
                        actionSql.AppendLine(") ON [PRIMARY]");

                        //插入临时表数据
                        rightValue = Utility.EngineHelper.GetAlias(listAlias, act.RightValue);
                        TD_SYS_FieldAlias rightValueFilterAlias = Utility.EngineHelper.GetAlias(listAlias, act.RightValueFilterAlias);
                        if (rightValue.TableName == "TM_Mem_TradeDetail")
                        {
                            foreach (var cl in conditionResultList)
                            {
                                actionSql.AppendLine("insert into " + tmpTrade);
                                actionSql.AppendLine("select TM_Mem_Trade.MemberID,TM_Mem_Trade.TradeID,tdd.[" + cl.NameCode + "]," + act.RightValueMin + ",[" + rightValueFilterAlias.TableName + "].[" + rightValueFilterAlias.FieldName + "],TM_Mem_Trade.ModifiedDate,0");
                                actionSql.AppendLine("from TM_Mem_Trade");
                                actionSql.AppendLine("inner join " + tmpTradeTableName + " td on TM_Mem_Trade.TradeID = td.TradeID");
                                actionSql.AppendLine("inner join " + tmpTradeExt + " tdd on TM_Mem_Trade.TradeID = tdd.TradeID ");
                                actionSql.AppendLine("left join TM_Loy_MemExt on TM_Mem_Trade.MemberID = TM_Loy_MemExt.MemberID");
                                actionSql.AppendLine("where TM_Mem_Trade.TradeType = '" + rightValue.AliasKey + "'");
                            }
                        }
                        else
                        {
                            actionSql.AppendLine("insert into " + tmpTrade);
                            actionSql.AppendLine("select TM_Mem_Trade.MemberID,TM_Mem_Trade.TradeID,TM_Mem_Trade.[" + rightValue.FieldName + "]," + act.RightValueMin + ",[" + rightValueFilterAlias.TableName + "].[" + rightValueFilterAlias.FieldName + "],TM_Mem_Trade.ModifiedDate,0");
                            actionSql.AppendLine("from TM_Mem_Trade");
                            actionSql.AppendLine("inner join (" + tmpTradeTableName + ") td on TM_Mem_Trade.TradeID = td.TradeID");
                            actionSql.AppendLine("left join TM_Loy_MemExt on TM_Mem_Trade.MemberID = TM_Loy_MemExt.MemberID");
                            actionSql.AppendLine("where TM_Mem_Trade.TradeType = '" + rightValue.AliasKey + "'");
                        }


                        //切割上下限
                        string TETable1 = "TETable1" + act.Sort.ToString() + curDatetime.DayOfYear.ToString() + curDatetime.ToString("hhmmss");
                        string TETable2 = "TETable2" + act.Sort.ToString() + curDatetime.DayOfYear.ToString() + curDatetime.ToString("hhmmss");
                        string TETable3 = "TETable3" + act.Sort.ToString() + curDatetime.DayOfYear.ToString() + curDatetime.ToString("hhmmss");
                        string OutSplitTable = "OutSplitTable" + act.Sort.ToString() + curDatetime.DayOfYear.ToString() + curDatetime.ToString("hhmmss");

                        //重算初始累计值
                        actionSql.AppendLine("update " + tmpTrade + " set LoyAccBeforeValue=LoyAccBeforeValue-temp.accamt");
                        actionSql.AppendLine("from  (select Memberid,sum(ComputeValue) accamt");
                        actionSql.AppendLine("from " + tmpTrade);
                        actionSql.AppendLine("	   group by Memberid ) temp ");
                        actionSql.AppendLine("where " + tmpTrade + ".Memberid=temp.Memberid");

                        actionSql.AppendLine("select a.*,ROW_NUMBER() OVER (partition by a.Memberid ORDER BY a.addeddate asc) serial ,");
                        actionSql.AppendLine("SUM(ComputeValue) OVER(");
                        actionSql.AppendLine("partition by a.Memberid");
                        actionSql.AppendLine("ORDER BY a.addeddate");
                        actionSql.AppendLine("ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) accvalue ,");
                        actionSql.AppendLine("a.NodeValue-a.LoyAccBeforeValue DiffValue ");
                        actionSql.AppendLine("into " + TETable1);
                        actionSql.AppendLine("from " + tmpTrade + " a ");

                        actionSql.AppendLine("select *");
                        actionSql.AppendLine("into " + TETable2);
                        actionSql.AppendLine("from (");
                        actionSql.AppendLine("select Memberid,TradeID,ComputeValue,NodeValue,LoyAccBeforeValue,AddedDate,serial,accvalue,DiffValue, ComputeValue SplitValue ,0 sort");
                        actionSql.AppendLine("from " + TETable1 + " a");
                        actionSql.AppendLine("where accvalue<=DiffValue ");
                        actionSql.AppendLine("union all");
                        actionSql.AppendLine("select Memberid,TradeID,ComputeValue,NodeValue,LoyAccBeforeValue,AddedDate,serial,accvalue,DiffValue,accvalue-DiffValue   SplitValue ,2 sort");
                        actionSql.AppendLine("from " + TETable1 + " a");
                        actionSql.AppendLine("where accvalue>DiffValue ");
                        actionSql.AppendLine("union all ");
                        actionSql.AppendLine("select Memberid,TradeID,ComputeValue,NodeValue,LoyAccBeforeValue,AddedDate,serial,accvalue,DiffValue,ComputeValue-(accvalue-DiffValue)   SplitValue ,1 sort");
                        actionSql.AppendLine("from " + TETable1 + " a");
                        actionSql.AppendLine("where accvalue>DiffValue ) t ");

                        actionSql.AppendLine("delete from " + TETable2 + " where  SplitValue=0 ");

                        actionSql.AppendLine("select * ");
                        actionSql.AppendLine("into " + TETable3);
                        actionSql.AppendLine("from (");
                        actionSql.AppendLine("select a.* ");
                        actionSql.AppendLine("from " + TETable2 + " a");
                        actionSql.AppendLine("left join  (");
                        actionSql.AppendLine("select  Memberid,TradeID ");
                        actionSql.AppendLine("from " + TETable2 + " a");
                        actionSql.AppendLine("where SplitValue<0 ");
                        actionSql.AppendLine("group by Memberid,TradeID  ) b on a.Memberid=b.Memberid and a.TradeID=b.TradeID");
                        actionSql.AppendLine("where b.Memberid is null and b.TradeID is null ");
                        actionSql.AppendLine("union all");
                        actionSql.AppendLine("select  a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.serial,a.accvalue,a.DiffValue,sum(SplitValue) SplitValue,0 sort");
                        actionSql.AppendLine("from " + TETable2 + " a");
                        actionSql.AppendLine("inner join  (");
                        actionSql.AppendLine("select  Memberid,TradeID ");
                        actionSql.AppendLine("from " + TETable2 + " a");
                        actionSql.AppendLine("where SplitValue<0 ");
                        actionSql.AppendLine("group by Memberid,TradeID  ) b on a.Memberid=b.Memberid and a.TradeID=b.TradeID");
                        actionSql.AppendLine("group by a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.serial,a.accvalue,a.DiffValue ) t ");
                        actionSql.AppendLine("order by memberid ,tradeid ,sort");

                        actionSql.AppendLine("Drop table " + tmpTrade);

                        //将结果输出到初始表并更新砍数
                        actionSql.AppendLine("select a.Memberid,a.TradeID,a.SplitValue as ComputeValue," + act.RightValueMax + " as NodeValue,a.LoyAccBeforeValue,a.AddedDate,Sort");
                        actionSql.AppendLine("into " + tmpTrade);
                        actionSql.AppendLine("from " + TETable3 + " a ");

                        //释放临时表
                        actionSql.AppendLine("Drop table " + TETable1);
                        actionSql.AppendLine("Drop table " + TETable2);
                        actionSql.AppendLine("Drop table " + TETable3);


                        //处理上限
                        actionSql.AppendLine("select a.*,ROW_NUMBER() OVER (partition by a.Memberid ORDER BY a.addeddate asc) serial ,");
                        actionSql.AppendLine("SUM(ComputeValue) OVER(");
                        actionSql.AppendLine("partition by a.Memberid");
                        actionSql.AppendLine("ORDER BY a.addeddate,a.TradeID,Sort");
                        actionSql.AppendLine("ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) accvalue ,");
                        actionSql.AppendLine("a.NodeValue-a.LoyAccBeforeValue DiffValue ");
                        actionSql.AppendLine("into " + TETable1);
                        actionSql.AppendLine("from " + tmpTrade + " a ");

                        actionSql.AppendLine("select *");
                        actionSql.AppendLine("into " + TETable2);
                        actionSql.AppendLine("from (");
                        actionSql.AppendLine("select Memberid,TradeID,ComputeValue,NodeValue,LoyAccBeforeValue,AddedDate,serial,accvalue,DiffValue, ComputeValue SplitValue ,0 sort");
                        actionSql.AppendLine("from " + TETable1 + " a");
                        actionSql.AppendLine("where accvalue<=DiffValue ");
                        actionSql.AppendLine("union all");
                        actionSql.AppendLine("select Memberid,TradeID,ComputeValue,NodeValue,LoyAccBeforeValue,AddedDate,serial,accvalue,DiffValue,accvalue-DiffValue   SplitValue ,2 sort");
                        actionSql.AppendLine("from " + TETable1 + " a");
                        actionSql.AppendLine("where accvalue>DiffValue ");
                        actionSql.AppendLine("union all ");
                        actionSql.AppendLine("select Memberid,TradeID,ComputeValue,NodeValue,LoyAccBeforeValue,AddedDate,serial,accvalue,DiffValue,ComputeValue-(accvalue-DiffValue)   SplitValue ,1 sort");
                        actionSql.AppendLine("from " + TETable1 + " a");
                        actionSql.AppendLine("where accvalue>DiffValue ) t ");

                        actionSql.AppendLine("delete from " + TETable2 + " where  SplitValue=0 ");

                        actionSql.AppendLine("select * ");
                        actionSql.AppendLine("into " + TETable3);
                        actionSql.AppendLine("from (");
                        actionSql.AppendLine("select a.* ");
                        actionSql.AppendLine("from " + TETable2 + " a");
                        actionSql.AppendLine("left join  (");
                        actionSql.AppendLine("select  Memberid,TradeID ");
                        actionSql.AppendLine("from " + TETable2 + " a");
                        actionSql.AppendLine("where SplitValue<0 ");
                        actionSql.AppendLine("group by Memberid,TradeID  ) b on a.Memberid=b.Memberid and a.TradeID=b.TradeID");
                        actionSql.AppendLine("where b.Memberid is null and b.TradeID is null ");
                        actionSql.AppendLine("union all");
                        actionSql.AppendLine("select  a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.serial,a.accvalue,a.DiffValue,sum(SplitValue) SplitValue,0 sort");
                        actionSql.AppendLine("from " + TETable2 + " a");
                        actionSql.AppendLine("inner join  (");
                        actionSql.AppendLine("select  Memberid,TradeID ");
                        actionSql.AppendLine("from " + TETable2 + " a");
                        actionSql.AppendLine("where SplitValue<0 ");
                        actionSql.AppendLine("group by Memberid,TradeID  ) b on a.Memberid=b.Memberid and a.TradeID=b.TradeID");
                        actionSql.AppendLine("group by a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.serial,a.accvalue,a.DiffValue ) t ");
                        actionSql.AppendLine("order by memberid ,tradeid ,sort");

                        //处理最终结果并计算累计
                        actionSql.AppendLine("select a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.SplitValue,");
                        actionSql.AppendLine("a.LoyAccBeforeValue+SUM(SplitValue) OVER(");
                        actionSql.AppendLine("partition by a.Memberid ");
                        actionSql.AppendLine("ORDER BY a.serial  ,sort ");
                        actionSql.AppendLine("ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AccSplitValue ");
                        actionSql.AppendLine("into " + OutSplitTable);
                        actionSql.AppendLine("from " + TETable3 + " a ");

                        //释放临时表
                        actionSql.AppendLine("Drop table " + TETable1);
                        actionSql.AppendLine("Drop table " + TETable2);
                        actionSql.AppendLine("Drop table " + TETable3);

                        //计算账户值
                        if (rightValue == null || rightValue.AliasKey == "")
                        {
                            #region 与交易无关
                            //创建ruleact临时表
                            actionSql.AppendLine("CREATE TABLE [" + tmpRuleAct + "](");
                            actionSql.AppendLine("[Memberid] [char](32) NULL,");
                            actionSql.AppendLine("[AccountID] [char](32) NOT NULL,");
                            actionSql.AppendLine("[FieldName] [varchar](6) NOT NULL,");
                            actionSql.AppendLine("[SpecialDate1] [varchar](10)  NULL,");
                            actionSql.AppendLine("[SpecialDate2] [varchar](10)  NULL,");
                            actionSql.AppendLine("[OffsetExpression] [varchar](1) NOT NULL,");
                            actionSql.AppendLine("[OffsetValue] [varchar](10) NOT NULL,");
                            actionSql.AppendLine("[RuleID] [int] NOT NULL,");
                            actionSql.AppendLine("[ComputeValue] [decimal](18, 4) NOT NULL,");
                            actionSql.AppendLine("[TradeID] [int] NULL");
                            actionSql.AppendLine(") ON [PRIMARY]");
                            actionSql.AppendLine("insert into " + tmpRuleAct);
                            actionSql.AppendLine("select distinct ");
                            actionSql.AppendLine(OutSplitTable + ".Memberid,");
                            actionSql.AppendLine("TM_Mem_Account.AccountID,");
                            if (!string.IsNullOrEmpty(act.FreezeValue))//有冻结
                            {
                                actionSql.AppendLine("'value2' as FieldName,");
                                switch (act.FreezeUnit)
                                {
                                    case "day":
                                        actionSql.AppendLine("cast ('" + curDatetime.AddDays(double.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as date) as SpecialDate1,");
                                        break;
                                    case "month":
                                        actionSql.AppendLine("cast ('" + curDatetime.AddMonths(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as date) as SpecialDate1,");
                                        break;
                                    case "year":
                                        actionSql.AppendLine("cast ('" + curDatetime.AddYears(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as date) as SpecialDate1,");
                                        break;
                                    default:
                                        actionSql.AppendLine("cast(null as date) as SpecialDate1,");
                                        break;
                                }
                                switch (act.AvailabeUnit)
                                {
                                    case "day":
                                        actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        break;
                                    case "month":
                                        if (string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    case "year":
                                        if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    default:
                                        actionSql.AppendLine("cast(null as date) SpecialDate2,");
                                        break;
                                }
                            }
                            else
                            {
                                actionSql.AppendLine("'value1' as FieldName,");
                                actionSql.AppendLine("cast(null as date) as SpecialDate1,");
                                switch (act.AvailabeUnit)
                                {
                                    case "day":
                                        actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(string.IsNullOrEmpty(act.AvailabeValue) ? "0" : act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        break;
                                    case "month":
                                        if (string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    case "year":
                                        if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    default:
                                        actionSql.AppendLine("cast(null as date) as SpecialDate2,");
                                        break;
                                }
                            }
                            actionSql.AppendLine("'" + act.OffsetExpression + "' as OffsetExpression,");
                            actionSql.AppendLine("'" + act.OffsetValue + "' as OffsetValue,");
                            actionSql.AppendLine(ruleId + " as RuleID,");
                            actionSql.AppendLine(OutSplitTable + ".SplitValue as ComputeValue,");
                            actionSql.AppendLine("null as TradeID");
                            //actionSql.AppendLine("into " + tmpRuleAct);
                            actionSql.AppendLine("from " + OutSplitTable + " inner join TM_Mem_Account on " + OutSplitTable + ".Memberid = TM_Mem_Account.MemberID");
                            actionSql.AppendLine("where TM_Mem_Account.AccountType = '" + act.LeftValue.ExtType + "' and ");
                            actionSql.AppendLine("AccSplitValue > " + act.RightValueMin + " and AccSplitValue <= " + act.RightValueMax);
                            #endregion

                        }
                        else
                        {
                            #region 与交易有关
                            //创建ruleact临时表
                            actionSql.AppendLine("CREATE TABLE [" + tmpRuleAct + "](");
                            actionSql.AppendLine("[Memberid] [char](32) NULL,");
                            actionSql.AppendLine("[AccountID] [char](32) NOT NULL,");
                            actionSql.AppendLine("[FieldName] [varchar](6) NOT NULL,");
                            actionSql.AppendLine("[SpecialDate1] [varchar](10)  NULL,");
                            actionSql.AppendLine("[SpecialDate2] [varchar](10)  NULL,");
                            actionSql.AppendLine("[OffsetExpression] [varchar](1) NOT NULL,");
                            actionSql.AppendLine("[OffsetValue] [varchar](10) NOT NULL,");
                            actionSql.AppendLine("[RuleID] [int] NOT NULL,");
                            actionSql.AppendLine("[ComputeValue] [decimal](18, 4) NOT NULL,");
                            actionSql.AppendLine("[TradeID] [int] NULL");
                            actionSql.AppendLine(") ON [PRIMARY]");
                            actionSql.AppendLine("insert into " + tmpRuleAct);
                            actionSql.AppendLine("select");
                            actionSql.AppendLine(OutSplitTable + ".Memberid,");
                            actionSql.AppendLine("TM_Mem_Account.AccountID,");
                            if (!string.IsNullOrEmpty(act.FreezeValue))//有冻结
                            {
                                actionSql.AppendLine("'value2' as FieldName,");
                                switch (act.FreezeUnit)
                                {
                                    case "day":
                                        actionSql.AppendLine("cast ('" + curDatetime.AddDays(double.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as date) as SpecialDate1,");
                                        break;
                                    case "month":
                                        actionSql.AppendLine("cast ('" + curDatetime.AddMonths(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as date) as SpecialDate1,");
                                        break;
                                    case "year":
                                        actionSql.AppendLine("cast ('" + curDatetime.AddYears(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as date) as SpecialDate1,");
                                        break;
                                    default:
                                        actionSql.AppendLine("cast(null as date) as SpecialDate1,");
                                        break;
                                }
                                switch (act.AvailabeUnit)
                                {
                                    case "day":
                                        actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        break;
                                    case "month":
                                        if (string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    case "year":
                                        if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    default:
                                        actionSql.AppendLine("cast(null as date) SpecialDate2,");
                                        break;
                                }
                            }
                            else
                            {
                                actionSql.AppendLine("'value1' as FieldName,");
                                actionSql.AppendLine("cast(null as date) as SpecialDate1,");
                                switch (act.AvailabeUnit)
                                {
                                    case "day":
                                        actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(string.IsNullOrEmpty(act.AvailabeValue) ? "0" : act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        break;
                                    case "month":
                                        if (string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    case "year":
                                        if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    default:
                                        actionSql.AppendLine("cast(null as date) as SpecialDate2,");
                                        break;
                                }
                            }
                            actionSql.AppendLine("'" + act.OffsetExpression + "' as OffsetExpression,");
                            actionSql.AppendLine("'" + act.OffsetValue + "' as OffsetValue,");
                            actionSql.AppendLine(ruleId + " as RuleID,");
                            actionSql.AppendLine(OutSplitTable + ".SplitValue as ComputeValue,");
                            actionSql.AppendLine(OutSplitTable + ".TradeID");
                            //actionSql.AppendLine("into " + tmpRuleAct);
                            actionSql.AppendLine("from " + OutSplitTable + " inner join TM_Mem_Account on " + OutSplitTable + ".Memberid = TM_Mem_Account.MemberID");
                            actionSql.AppendLine("where TM_Mem_Account.AccountType = '" + act.LeftValue.ExtType + "' and ");
                            actionSql.AppendLine("AccSplitValue > " + act.RightValueMin + " and AccSplitValue <= " + act.RightValueMax);
                            #endregion

                        }
                        actionSql.AppendLine("Drop Table " + OutSplitTable);
                        actionSql.AppendLine("Drop Table " + tmpTrade);
                    }
                        #endregion
                    else
                    {
                        #region 无上下限过滤
                        //计算账户值
                        //确定右值
                        ConditionResult? rightConditionResult = getConditionResult(conditionResultList, act.RightValue);
                        rightValue = rightConditionResult == null ? Utility.EngineHelper.GetAlias(listAlias, act.RightValue) : Utility.EngineHelper.GetAlias(listAlias, rightConditionResult.Value.FieldAlias);
                        if (rightValue == null || rightValue.AliasKey == "")
                        {
                            #region 右值处理和交易无关
                            //创建ruleact临时表
                            actionSql.AppendLine("CREATE TABLE [" + tmpRuleAct + "](");
                            actionSql.AppendLine("[Memberid] [char](32) NULL,");
                            actionSql.AppendLine("[AccountID] [char](32) NOT NULL,");
                            actionSql.AppendLine("[FieldName] [varchar](6) NOT NULL,");
                            actionSql.AppendLine("[SpecialDate1] [varchar](10)  NULL,");
                            actionSql.AppendLine("[SpecialDate2] [varchar](10)  NULL,");
                            actionSql.AppendLine("[OffsetExpression] [varchar](1) NOT NULL,");
                            actionSql.AppendLine("[OffsetValue] [varchar](10) NOT NULL,");
                            actionSql.AppendLine("[RuleID] [int] NOT NULL,");
                            actionSql.AppendLine("[ComputeValue] [decimal](18, 4) NOT NULL,");
                            actionSql.AppendLine("[TradeID] [int] NULL");
                            actionSql.AppendLine(") ON [PRIMARY]");
                            actionSql.AppendLine("insert into " + tmpRuleAct);
                            actionSql.AppendLine("select distinct ");
                            actionSql.AppendLine("TM_Mem_Trade.Memberid,");
                            actionSql.AppendLine("TM_Mem_Account.AccountID,");
                            if (!string.IsNullOrEmpty(act.FreezeValue))//有冻结
                            {
                                actionSql.AppendLine("'value2' as FieldName,");
                                switch (act.FreezeUnit)
                                {
                                    case "day":
                                        actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate1,");
                                        break;
                                    case "month":
                                        actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate1,");
                                        break;
                                    case "year":
                                        actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate1,");
                                        break;
                                    default:
                                        actionSql.AppendLine("cast(null as date) SpecialDate1,");
                                        break;
                                }
                                switch (act.AvailabeUnit)
                                {
                                    case "day":
                                        actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        break;
                                    case "month":
                                        if (string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    case "year":
                                        if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    default:
                                        actionSql.AppendLine("cast(null as date) SpecialDate2,");
                                        break;
                                }
                            }
                            else
                            {
                                actionSql.AppendLine("'value1' as FieldName,");
                                actionSql.AppendLine("cast(null as date) as SpecialDate1,");
                                switch (act.AvailabeUnit)
                                {
                                    case "day":
                                        actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(string.IsNullOrEmpty(act.AvailabeValue) ? "0" : act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        break;
                                    case "month":
                                        if (string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    case "year":
                                        if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    default:
                                        actionSql.AppendLine("cast(null as date) as SpecialDate2,");
                                        break;
                                }
                            }
                            actionSql.AppendLine("'" + act.OffsetExpression + "' as OffsetExpression,");
                            actionSql.AppendLine("'" + act.OffsetValue + "' as OffsetValue,");
                            actionSql.AppendLine(ruleId + " as RuleID,");
                            actionSql.AppendLine("isnull(" + (rightValue != null ? rightValue.FieldName : act.RightValue) + " , 0) as ComputeValue,");
                            actionSql.AppendLine("null as TradeID");
                            actionSql.AppendLine("from (" + tmpTradeTableName + ") TradeTable inner join TM_Mem_Trade on TradeTable.TradeID = TM_Mem_Trade.TradeID inner join TM_Mem_Account on TM_Mem_Trade.Memberid = TM_Mem_Account.MemberID ");
                            if (tmpTradeExt != "")
                            {
                                actionSql.AppendLine(" left join " + tmpTradeExt + " on " + tmpTradeExt + ".TradeID = TradeTable.TradeID");
                            }
                            actionSql.AppendLine("where TM_Mem_Account.AccountType = '" + act.LeftValue.ExtType + "' ");
                            #endregion
                        }
                        else
                        {
                            #region 右值处理和交易有关
                            //创建ruleact临时表
                            actionSql.AppendLine("CREATE TABLE [" + tmpRuleAct + "](");
                            actionSql.AppendLine("[Memberid] [char](32) NULL,");
                            actionSql.AppendLine("[AccountID] [char](32) NOT NULL,");
                            actionSql.AppendLine("[FieldName] [varchar](6) NOT NULL,");
                            actionSql.AppendLine("[SpecialDate1] [varchar](10)  NULL,");
                            actionSql.AppendLine("[SpecialDate2] [varchar](10)  NULL,");
                            actionSql.AppendLine("[OffsetExpression] [varchar](1) NOT NULL,");
                            actionSql.AppendLine("[OffsetValue] [varchar](10) NOT NULL,");
                            actionSql.AppendLine("[RuleID] [int] NOT NULL,");
                            actionSql.AppendLine("[ComputeValue] [decimal](18, 4) NOT NULL,");
                            actionSql.AppendLine("[TradeID] [int] NULL");
                            actionSql.AppendLine(") ON [PRIMARY]");

                            if (rightValue.TableName == "TM_Mem_TradeDetail")
                            {
                                foreach (var cl in conditionResultList)
                                {
                                    actionSql.AppendLine("insert into " + tmpRuleAct);
                                    actionSql.AppendLine("select");
                                    actionSql.AppendLine("TM_Mem_Trade.Memberid,");
                                    actionSql.AppendLine("TM_Mem_Account.AccountID,");
                                    if (!string.IsNullOrEmpty(act.FreezeValue))//有冻结
                                    {
                                        actionSql.AppendLine("'value2' as FieldName,");
                                        switch (act.FreezeUnit)
                                        {
                                            case "day":
                                                actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate1,");
                                                break;
                                            case "month":
                                                actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate1,");
                                                break;
                                            case "year":
                                                actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate1,");
                                                break;
                                            default:
                                                actionSql.AppendLine("cast(null as date) SpecialDate1,");
                                                break;
                                        }
                                        switch (act.AvailabeUnit)
                                        {
                                            case "day":
                                                actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                                break;
                                            case "month":
                                                if (string.IsNullOrEmpty(act.OffsetDay))
                                                    actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                                else
                                                    actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                                break;
                                            case "year":
                                                if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                                    actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                                else
                                                    actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                                break;
                                            default:
                                                actionSql.AppendLine("cast(null as date) SpecialDate2,");
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        actionSql.AppendLine("'value1' as FieldName,");
                                        actionSql.AppendLine("cast(null as date) as SpecialDate1,");
                                        switch (act.AvailabeUnit)
                                        {
                                            case "day":
                                                actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(string.IsNullOrEmpty(act.AvailabeValue) ? "0" : act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                                break;
                                            case "month":
                                                if (string.IsNullOrEmpty(act.OffsetDay))
                                                    actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                                else
                                                    actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                                break;
                                            case "year":
                                                if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                                    actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                                else
                                                    actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                                break;
                                            default:
                                                actionSql.AppendLine("cast(null as date) as SpecialDate2,");
                                                break;
                                        }
                                    }
                                    actionSql.AppendLine("'" + act.OffsetExpression + "' as OffsetExpression,");
                                    actionSql.AppendLine("'" + act.OffsetValue + "' as OffsetValue,");
                                    actionSql.AppendLine(ruleId + " as RuleID,");
                                    actionSql.AppendLine("isnull(" + tmpTradeExt + "." + cl.NameCode + " , 0) as ComputeValue,");
                                    actionSql.AppendLine("TradeTable.TradeID");
                                    //actionSql.AppendLine("into " + tmpRuleAct);
                                    actionSql.AppendLine("from (" + tmpTradeTableName + ") TradeTable inner join TM_Mem_Trade on TradeTable.TradeID = TM_Mem_Trade.TradeID inner join TM_Mem_Account on TM_Mem_Trade.Memberid = TM_Mem_Account.MemberID ");
                                    if (tmpTradeExt != "")
                                    {
                                        actionSql.AppendLine(" left join " + tmpTradeExt + " on " + tmpTradeExt + ".TradeID = TradeTable.TradeID");
                                    }
                                    actionSql.AppendLine("where TM_Mem_Account.AccountType = '" + act.LeftValue.ExtType + "' ");
                                }

                            }
                            else
                            {
                                actionSql.AppendLine("insert into " + tmpRuleAct);
                                actionSql.AppendLine("select");
                                actionSql.AppendLine("TM_Mem_Trade.Memberid,");
                                actionSql.AppendLine("TM_Mem_Account.AccountID,");
                                if (!string.IsNullOrEmpty(act.FreezeValue))//有冻结
                                {
                                    actionSql.AppendLine("'value2' as FieldName,");
                                    switch (act.FreezeUnit)
                                    {
                                        case "day":
                                            actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate1,");
                                            break;
                                        case "month":
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate1,");
                                            break;
                                        case "year":
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate1,");
                                            break;
                                        default:
                                            actionSql.AppendLine("cast(null as date) SpecialDate1,");
                                            break;
                                    }
                                    switch (act.AvailabeUnit)
                                    {
                                        case "day":
                                            actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                            break;
                                        case "month":
                                            if (string.IsNullOrEmpty(act.OffsetDay))
                                                actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                            else
                                                actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                            break;
                                        case "year":
                                            if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                                actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                            else
                                                actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                            break;
                                        default:
                                            actionSql.AppendLine("cast(null as date) SpecialDate2,");
                                            break;
                                    }
                                }
                                else
                                {
                                    actionSql.AppendLine("'value1' as FieldName,");
                                    actionSql.AppendLine("cast(null as date) as SpecialDate1,");
                                    switch (act.AvailabeUnit)
                                    {
                                        case "day":
                                            actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(string.IsNullOrEmpty(act.AvailabeValue) ? "0" : act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                            break;
                                        case "month":
                                            if (string.IsNullOrEmpty(act.OffsetDay))
                                                actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                            else
                                                actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                            break;
                                        case "year":
                                            if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                                actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                            else
                                                actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                            break;
                                        default:
                                            actionSql.AppendLine("cast(null as date) as SpecialDate2,");
                                            break;
                                    }
                                }
                                actionSql.AppendLine("'" + act.OffsetExpression + "' as OffsetExpression,");
                                actionSql.AppendLine("'" + act.OffsetValue + "' as OffsetValue,");
                                actionSql.AppendLine(ruleId + " as RuleID,");
                                actionSql.AppendLine("isnull(" + (rightValue != null ? rightValue.FieldName : act.RightValue) + " , 0) as ComputeValue,");
                                actionSql.AppendLine("TradeTable.TradeID");
                                //actionSql.AppendLine("into " + tmpRuleAct);
                                actionSql.AppendLine("from (" + tmpTradeTableName + ") TradeTable inner join TM_Mem_Trade on TradeTable.TradeID = TM_Mem_Trade.TradeID inner join TM_Mem_Account on TM_Mem_Trade.Memberid = TM_Mem_Account.MemberID ");
                                if (tmpTradeExt != "")
                                {
                                    actionSql.AppendLine(" left join " + tmpTradeExt + " on " + tmpTradeExt + ".TradeID = TradeTable.TradeID");
                                }
                                actionSql.AppendLine("where TM_Mem_Account.AccountType = '" + act.LeftValue.ExtType + "' ");
                            }

                            #endregion
                        }


                        #endregion
                    }

                    //限制类型临时表
                    string tmpLimit = "tmpLimit" + act.Sort.ToString() + curDatetime.DayOfYear.ToString() + curDatetime.ToString("hhmmss");
                    actionSql.AppendLine("CREATE TABLE [" + tmpLimit + "](");
                    actionSql.AppendLine("	[Type] [nvarchar](20) NOT NULL");
                    actionSql.AppendLine(") ON [PRIMARY]");
                    foreach (string limit in act.LeftValue.ExtLimitList)
                    {
                        actionSql.AppendLine("insert into " + tmpLimit + " values('" + limit + "')");
                    }

                    //判断右值是否关联交易--计算代码
                    if (rightValue == null || rightValue.AliasKey == "")
                    {
                        #region 交易无关
                        actionSql.AppendLine("                           ---可用积分,冻结积分更新插入                                                                                                                                         ");
                        actionSql.AppendLine("exec ('                                                                                                                                                                                         ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("update TM_Mem_AccountDetail                                                                                                                                                                     ");
                        actionSql.AppendLine("set TM_Mem_AccountDetail.DetailValue         =TM_Mem_AccountDetail.DetailValue+temp.DetailValue        ,                                                                                        ");
                        actionSql.AppendLine("    TM_Mem_AccountDetail.ModifiedDate        =getdate()                                                                                                                                         ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("from  (                                                                                                                                                                                         ");
                        actionSql.AppendLine("	                                                                                                                                                                                              ");
                        actionSql.AppendLine("	      select n.AccountID,n.AccountDetailID,n.AccountDetailType,t.DetailValue                                                                                                                  ");
                        actionSql.AppendLine("		  from                                                                                                                                                                                    ");
                        actionSql.AppendLine("	 --------------本次账户相关内容及限定值                                                                                                                                                       ");
                        actionSql.AppendLine("		  (                                                                                                                                                                                       ");
                        actionSql.AppendLine("	      	select a.AccountID,fieldname AccountDetailType,                                                                                                                                       ");
                        actionSql.AppendLine("			       isnull(a.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(a.SpecialDate2,''2000-01-01'')  SpecialDate2                                                                        ");
                        actionSql.AppendLine("			          , ''0'' vehicle, ''0''  StoreCode, ''0''  StoreBrandCode                                                                                                                    ");
                        actionSql.AppendLine("			from   " + tmpRuleAct + "  a                                                                                                                                                           ");
                        actionSql.AppendLine("			group by a.AccountID,fieldname,isnull(a.SpecialDate1,''2000-01-01''), isnull(a.SpecialDate2,''2000-01-01'')    ) m                                                                    ");
                        actionSql.AppendLine("         inner join                                                                                                                                                                             ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("     ---------------历史账户明细限制值 (left 存在不作限制的账户明细ID)                                                                                                                          ");
                        actionSql.AppendLine("	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,                                                                                                                             ");
                        actionSql.AppendLine("		          isnull(b.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(b.SpecialDate2,''2000-01-01'')  SpecialDate2,                                                                        ");
                        actionSql.AppendLine("		               max(case when Limittype=''vehicle'' then LimitValue else ''0'' end ) vehicle,                                                                                              ");
                        actionSql.AppendLine("			           max(case when Limittype=''store''   then LimitValue else ''0'' end ) StoreCode,                                                                                            ");
                        actionSql.AppendLine("					   max(case when Limittype=''brand''   then LimitValue else ''0'' end ) StoreBrandCode                                                                                        ");
                        actionSql.AppendLine("		   from   " + tmpRuleAct + "  a                                                                                                                                                             ");
                        actionSql.AppendLine("		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType                                                                                       ");
                        actionSql.AppendLine("		   left join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID                                                                                    ");
                        actionSql.AppendLine("		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''2000-01-01''), isnull(b.SpecialDate2,''2000-01-01'') ) n                                            ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2                                             ");
                        actionSql.AppendLine("		 and m.vehicle=n.vehicle  and m.StoreCode=n.StoreCode and  m.StoreBrandCode=n.StoreBrandCode                                                                                              ");
                        actionSql.AppendLine("		                                                                                                                                                                                          ");
                        actionSql.AppendLine("		 inner join                                                                                                                                                                               ");
                        actionSql.AppendLine("	-----------------本次需要更新的账户数据                                                                                                                                                       ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("		  (select AccountID,fieldname AccountDetailType,isnull(SpecialDate1,''2000-01-01'') SpecialDate1, isnull(SpecialDate2,''2000-01-01'')  SpecialDate2,                                      ");
                        actionSql.AppendLine("              sum(case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue)                                                                                               ");
                        actionSql.AppendLine("	                   when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue)                                                                                                 ");
                        actionSql.AppendLine("			           when isnull(OffsetExpression,'''')=''''  then ComputeValue end ) DetailValue,                                                                                              ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("                    getdate() AddedDate,''1000'' AddedUser,getdate() ModifiedDate,''1000'' ModifiedUser                                                                                         ");
                        actionSql.AppendLine("           from   " + tmpRuleAct + "  a                                                                                                                                                       ");
                        actionSql.AppendLine("           group by AccountID,fieldname,isnull(SpecialDate1,''2000-01-01'') , isnull(SpecialDate2,''2000-01-01'')  ) t                                                                          ");
                        actionSql.AppendLine("		on  m.AccountID=t.AccountID                                                                                                                                                               ");
                        actionSql.AppendLine("        and m.AccountDetailType=t.AccountDetailType                                                                                                                                             ");
                        actionSql.AppendLine("		and m.SpecialDate1=t.SpecialDate1                                                                                                                                                         ");
                        actionSql.AppendLine("		and m.SpecialDate2=t.SpecialDate2  ) temp                                                                                                                                                 ");
                        actionSql.AppendLine("where TM_Mem_AccountDetail.AccountDetailID=temp.AccountDetailID                                                                                                                                 ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("--------插入部分                                                                                                                                                                                ");
                        actionSql.AppendLine("insert into TM_Mem_AccountDetail                                                                                                                                                                ");
                        actionSql.AppendLine("(AccountID,AccountDetailType,DetailValue,SpecialDate1,SpecialDate2,AddedDate,AddedUser,ModifiedDate,ModifiedUser)                                                                               ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("	      select m.AccountID,m.AccountDetailType,t.DetailValue,                                                                                                                                   ");
                        actionSql.AppendLine("		         case when t.SpecialDate1=''2000-01-01'' then null else t.SpecialDate1 end SpecialDate1,                                                                                          ");
                        actionSql.AppendLine("				 case when t.SpecialDate2=''2000-01-01'' then null else t.SpecialDate2 end SpecialDate2,                                                                                          ");
                        actionSql.AppendLine("		        t.AddedDate,t.AddedUser,t.ModifiedDate,t.ModifiedUser                                                                                                                             ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("		  from                                                                                                                                                                                    ");
                        actionSql.AppendLine("	 --------------本次账户相关内容及限定值                                                                                                                                                       ");
                        actionSql.AppendLine("		  (                                                                                                                                                                                       ");
                        actionSql.AppendLine("	      	select a.AccountID,fieldname AccountDetailType,                                                                                                                                       ");
                        actionSql.AppendLine("			       isnull(a.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(a.SpecialDate2,''2000-01-01'')  SpecialDate2                                                                        ");
                        actionSql.AppendLine("			          ,''0'' vehicle, ''0''  StoreCode, ''0''  StoreBrandCode                                                                                                                     ");
                        actionSql.AppendLine("			from   " + tmpRuleAct + "  a                                                                                                                                                           ");
                        actionSql.AppendLine("			group by a.AccountID,fieldname,isnull(a.SpecialDate1,''2000-01-01''), isnull(a.SpecialDate2,''2000-01-01'')    ) m                                                                    ");
                        actionSql.AppendLine("         left join                                                                                                                                                                              ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("     ---------------历史账户明细限制值                                                                                                                                                          ");
                        actionSql.AppendLine("	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,                                                                                                                             ");
                        actionSql.AppendLine("		          isnull(b.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(b.SpecialDate2,''2000-01-01'')  SpecialDate2,                                                                        ");
                        actionSql.AppendLine("		               max(case when Limittype=''vehicle'' then LimitValue else ''0'' end ) vehicle,                                                                                              ");
                        actionSql.AppendLine("			           max(case when Limittype=''store''   then LimitValue else ''0'' end ) StoreCode,                                                                                            ");
                        actionSql.AppendLine("					   max(case when Limittype=''brand''   then LimitValue else ''0'' end ) StoreBrandCode                                                                                        ");
                        actionSql.AppendLine("		   from   " + tmpRuleAct + "  a                                                                                                                                                               ");
                        actionSql.AppendLine("		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType                                                                                       ");
                        actionSql.AppendLine("		   left join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID                                                                                    ");
                        actionSql.AppendLine("		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''2000-01-01''), isnull(b.SpecialDate2,''2000-01-01'') ) n                                            ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2                                             ");
                        actionSql.AppendLine("		 and m.vehicle=n.vehicle  and m.StoreCode=n.StoreCode and  m.StoreBrandCode=n.StoreBrandCode                                                                                              ");
                        actionSql.AppendLine("		                                                                                                                                                                                          ");
                        actionSql.AppendLine("		 inner join                                                                                                                                                                               ");
                        actionSql.AppendLine("	-----------------本次需要更新的账户数据                                                                                                                                                       ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("		  (select AccountID,fieldname AccountDetailType,isnull(SpecialDate1,''2000-01-01'') SpecialDate1, isnull(SpecialDate2,''2000-01-01'')  SpecialDate2,                                      ");
                        actionSql.AppendLine("              sum(case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue)                                                                                               ");
                        actionSql.AppendLine("	                   when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue)                                                                                                 ");
                        actionSql.AppendLine("			           when isnull(OffsetExpression,'''')=''''  then ComputeValue end ) DetailValue,                                                                                              ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("                    getdate() AddedDate,''1000'' AddedUser,getdate() ModifiedDate,''1000'' ModifiedUser                                                                                         ");
                        actionSql.AppendLine("           from   " + tmpRuleAct + "  a                                                                                                                                                    ");
                        actionSql.AppendLine("           group by AccountID,fieldname,isnull(SpecialDate1,''2000-01-01'') , isnull(SpecialDate2,''2000-01-01'')  ) t                                                                          ");
                        actionSql.AppendLine("		on  m.AccountID=t.AccountID                                                                                                                                                               ");
                        actionSql.AppendLine("        and m.AccountDetailType=t.AccountDetailType                                                                                                                                             ");
                        actionSql.AppendLine("		and m.SpecialDate1=t.SpecialDate1                                                                                                                                                         ");
                        actionSql.AppendLine("		and m.SpecialDate2=t.SpecialDate2                                                                                                                                                         ");
                        actionSql.AppendLine("    where n.AccountID is null                                                                                                                                                                   ");
                        actionSql.AppendLine("	 and  n.AccountDetailType  is null                                                                                                                                                            ");
                        actionSql.AppendLine("	 and  n.SpecialDate1 is null                                                                                                                                                                  ");
                        actionSql.AppendLine("	 and  n.SpecialDate2 is null                                                                                                                                                                  ");
                        actionSql.AppendLine("     and  n.vehicle is null                                                                                                                                                                     ");
                        actionSql.AppendLine("	 and  n.StoreCode is null                                                                                                                                                                     ");
                        actionSql.AppendLine("	 and  n.StoreBrandCode is null 		   		                                                                                                                                                  ");
                        actionSql.AppendLine("')                                                                                                                                                                                              ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("----不存在账户限制值                                                                                                                                                                            ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("----更新账户表中积分账户的可用积分;                                                                                                                                                             ");
                        actionSql.AppendLine("---删除AccountType=2，通过AccountID判别其为积分或者积点账户                                                                                                                                     ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("exec('                                                                                                                                                                                          ");
                        actionSql.AppendLine("update  TM_Mem_Account set value1=value1+temp.ChangeValue ,ModifiedDate=getdate()                                                                                                               ");
                        actionSql.AppendLine("from ( 	                                                                                                                                                                                      ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("     select AccountID,                                                                                                                                                                          ");
                        actionSql.AppendLine("              sum(case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue)                                                                                               ");
                        actionSql.AppendLine("	                   when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue)                                                                                                 ");
                        actionSql.AppendLine("			            when isnull(OffsetExpression,'''')='''' then ComputeValue end ) ChangeValue,                                                                                              ");
                        actionSql.AppendLine("              getdate() AddedDate,''1000'' AddedUser,getdate() ModifiedDate,''1000'' ModifiedUser                                                                                               ");
                        actionSql.AppendLine("      from   " + tmpRuleAct + "  a                                                                                                                                                               ");
                        actionSql.AppendLine("      where fieldname=''value1''                                                                                                                                                                ");
                        actionSql.AppendLine("	  group by AccountID ) temp                                                                                                                                                                   ");
                        actionSql.AppendLine("where TM_Mem_Account.AccountID=temp.AccountID   and temp.ChangeValue<>0                                                                                                                         ");
                        actionSql.AppendLine("')                                                                                                                                                                                              ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("---更新账户表中积分账户的可用积分                                                                                                                                                               ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("exec('                                                                                                                                                                                          ");
                        actionSql.AppendLine("update  TM_Mem_Account set value2=value2+temp.ChangeValue ,ModifiedDate=getdate()                                                                                                               ");
                        actionSql.AppendLine("from ( 	                                                                                                                                                                                      ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("     select AccountID,                                                                                                                                                                          ");
                        actionSql.AppendLine("              sum(case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue)                                                                                               ");
                        actionSql.AppendLine("	                   when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue)                                                                                                 ");
                        actionSql.AppendLine("			            when isnull(OffsetExpression,'''')='''' then ComputeValue end ) ChangeValue,                                                                                              ");
                        actionSql.AppendLine("              getdate() AddedDate,''1000'' AddedUser,getdate() ModifiedDate,''1000'' ModifiedUser                                                                                               ");
                        actionSql.AppendLine("      from   " + tmpRuleAct + "  a                                                                                                                                                              ");
                        actionSql.AppendLine("      where fieldname=''value2''                                                                                                                                                                ");
                        actionSql.AppendLine("	  group by AccountID ) temp                                                                                                                                                                   ");
                        actionSql.AppendLine("where TM_Mem_Account.AccountID=temp.AccountID   and temp.ChangeValue<>0                                                                                                                         ");
                        actionSql.AppendLine("')                                                                                                                                                                                              ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("-----明细记录值插入                                                                                                                                                                             ");
                        actionSql.AppendLine("                                                                                                                                                                                                ");
                        actionSql.AppendLine("exec('                                                                                                                                                                                          ");
                        actionSql.AppendLine("insert into TL_Mem_AccountChange                                                                                                                                                                ");
                        actionSql.AppendLine("(                                                                                                                                                                                               ");
                        actionSql.AppendLine("AccountID         ,                                                                                                                                                                             ");
                        actionSql.AppendLine("MemberID         ,                                                                                                                                                                             ");
                        actionSql.AppendLine("AccountDetailID   ,                                                                                                                                                                             ");
                        actionSql.AppendLine("AccountChangeType ,                                                                                                                                                                             ");
                        actionSql.AppendLine("ChangeValue       ,                                                                                                                                                                             ");
                        actionSql.AppendLine("ChangeReason      ,                                                                                                                                                                             ");
                        actionSql.AppendLine("ReferenceNo       ,                                                                                                                                                                             ");
                        actionSql.AppendLine("HasReverse        ,                                                                                                                                                                             ");
                        actionSql.AppendLine("RuleID,                                                                                                                                                                                         ");
                        actionSql.AppendLine("AddedDate         ,                                                                                                                                                                             ");
                        actionSql.AppendLine("AddedUser                                                                                                                                                                                       ");
                        actionSql.AppendLine(")                                                                                                                                                                                               ");
                        actionSql.AppendLine("select ttt.AccountID,ttt.MemberID,ttt.AccountDetailID,ttt.AccountChangeType,ttt.ChangeValue,ttt.ChangeReason,ttt.ReferenceNo,ttt.HasReverse,ttt.RuleID,ttt.AddedDate,ttt.AddedUser");
                        actionSql.AppendLine("from ");
                        actionSql.AppendLine("(");
                        actionSql.AppendLine("select                                                                                                                                                                                          ");
                        actionSql.AppendLine("m.AccountID         ,                                                                                                                                                                           ");
                        actionSql.AppendLine("m.MemberID         ,                                                                                                                                                                           ");
                        actionSql.AppendLine("m.AccountDetailID   ,                                                                                                                                                                           ");
                        actionSql.AppendLine("''loy'' AccountChangeType ,                                                                                                                                                                     ");
                        actionSql.AppendLine("(case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue)                                                                                                                ");
                        actionSql.AppendLine("	                  when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue)                                                                                                  ");
                        actionSql.AppendLine("			          when isnull(OffsetExpression,'''')='''' then ComputeValue end )   ChangeValue, ");
                        actionSql.AppendLine("''" + Remark + "'' ChangeReason      , ");
                        actionSql.AppendLine("t.TradeID ReferenceNo       , ");
                        actionSql.AppendLine("0 HasReverse                , ");
                        actionSql.AppendLine("t.RuleID                    , ");
                        actionSql.AppendLine("''" + curDatetime.ToString("yyyy-MM-dd HH:mm:ss") + "'' AddedDate,");
                        actionSql.AppendLine("''1000''   AddedUser ");
                        actionSql.AppendLine("from ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("	     (  select w.* ");
                        actionSql.AppendLine("		  from ");
                        actionSql.AppendLine("	      (select a.AccountID,a.MemberID,fieldname AccountDetailType,t.AccountDetailID, ");
                        actionSql.AppendLine("			       isnull(a.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(a.SpecialDate2,''2000-01-01'')  SpecialDate2 ");
                        actionSql.AppendLine("			          ,''0''   vehicle1,     ''0''  store1,	    ''0''  brand1,isnull(f.store,0) store, isnull(f.brand,0) brand,isnull(f.vehicle,0) vehicle ");
                        actionSql.AppendLine("			from   " + tmpRuleAct + "  a ");
                        actionSql.AppendLine("	          inner join TM_Mem_AccountDetail t on ");
                        actionSql.AppendLine("			      a.AccountID=t.AccountID and a.fieldname=t.AccountDetailType ");
                        actionSql.AppendLine("              and isnull(a.SpecialDate1,''2000-01-01'')=isnull(t.SpecialDate1,''2000-01-01'') ");
                        actionSql.AppendLine("              and isnull(a.SpecialDate2,''2000-01-01'')=isnull(t.SpecialDate2,''2000-01-01'') ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("			left join (select AccountID,AccountDetailID,max(store) store,max(brand) brand,max(vehicle) vehicle ");
                        actionSql.AppendLine("			              from ( ");
                        actionSql.AppendLine("			              select AccountID,AccountDetailID,isnull(store,''0'') store,isnull(brand,''0'') brand,isnull(vehicle,''0'') vehicle ");
                        actionSql.AppendLine("		                  from TM_Mem_AccountLimit pivot( max(limitvalue) for limittype in(store,brand,vehicle)) t  ) t ");
                        actionSql.AppendLine("						  group by  AccountID,AccountDetailID ");
                        actionSql.AppendLine("						  )  f ");
                        actionSql.AppendLine("						  on t.AccountID=f.AccountID and t.AccountDetailID=f.AccountDetailID ");
                        actionSql.AppendLine("			group by a.AccountID,a.MemberID,fieldname,t.AccountDetailID,isnull(a.SpecialDate1,''2000-01-01''), isnull(a.SpecialDate2,''2000-01-01'') ,f.store,f.brand,f.vehicle  )w ");
                        actionSql.AppendLine("			where  store=store1 and brand=brand1 and vehicle=vehicle1 )  m ");
                        actionSql.AppendLine("	     inner join ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("     ---------------历史账户明细限制值 ");
                        actionSql.AppendLine("	      ( select b.AccountID,b.AccountDetailID,b.AccountDetailType, ");
                        actionSql.AppendLine("		          isnull(b.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(b.SpecialDate2,''2000-01-01'')  SpecialDate2, ");
                        actionSql.AppendLine("		               max(case when Limittype=''vehicle'' then LimitValue else ''0'' end ) vehicle, ");
                        actionSql.AppendLine("			           max(case when Limittype=''store''   then LimitValue else ''0'' end ) store, ");
                        actionSql.AppendLine("					   max(case when Limittype=''brand''   then LimitValue else ''0'' end ) brand ");
                        actionSql.AppendLine("		   from   " + tmpRuleAct + "  a ");
                        actionSql.AppendLine("		   inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType ");
                        actionSql.AppendLine("		   left join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID ");
                        actionSql.AppendLine("		   group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''2000-01-01''), isnull(b.SpecialDate2,''2000-01-01'') ) n ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("	     on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2 ");
                        actionSql.AppendLine("		 and m.vehicle=n.vehicle  and m.store=n.store and  m.brand=n.brand ");
                        actionSql.AppendLine("		 inner join   " + tmpRuleAct + "  t ");
                        actionSql.AppendLine("		 on m.AccountID=t.AccountID and m.AccountDetailType=t.fieldname ");
                        actionSql.AppendLine("		  and m.SpecialDate1=isnull(t.SpecialDate1,''2000-01-01'') ");
                        actionSql.AppendLine("		  and m.SpecialDate2=isnull(t.SpecialDate2,''2000-01-01'') ");
                        actionSql.AppendLine(" ) ttt");
                        actionSql.AppendLine("where ttt.ChangeValue <> 0 ')");

                        actionSql.AppendLine("Drop Table " + tmpRuleAct);
                        actionSql.AppendLine("Drop Table " + tmpLimit);
                        #endregion
                    }
                    else
                    {
                        #region 右值处理和交易有关
                        //按照中间表进行执行
                        actionSql.AppendLine("exec ('");
                        actionSql.AppendLine("if exists (select type  from  " + tmpLimit + " where type !='''')");
                        actionSql.AppendLine("begin");
                        actionSql.AppendLine("update TM_Mem_AccountDetail ");
                        actionSql.AppendLine("set TM_Mem_AccountDetail.DetailValue = TM_Mem_AccountDetail.DetailValue + temp.DetailValue,");

                        actionSql.AppendLine("    TM_Mem_AccountDetail.ModifiedDate = getdate()");
                        actionSql.AppendLine("from  (");
                        actionSql.AppendLine("           select n.AccountID,n.AccountDetailID,n.AccountDetailType,t.DetailValue ");
                        actionSql.AppendLine("           from ");
                        actionSql.AppendLine(" --------------本次账户相关内容及限定值");
                        actionSql.AppendLine("            (");
                        actionSql.AppendLine("              select a.AccountID,fieldname AccountDetailType,");
                        actionSql.AppendLine("                     isnull(a.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(a.SpecialDate2,''2000-01-01'')  SpecialDate2");
                        //actionSql.AppendLine("                     ,max(case when type=''vehicle'' then cast(e.MemberSubExtID  as nvarchar(10)) else ''0'' end ) vehicle");
                        //actionSql.AppendLine("                     ,max(case when type=''store'' then StoreCode  else ''0'' end ) StoreCode");
                        //actionSql.AppendLine("                     ,max(case when type=''brand'' then StoreBrandCode  else ''0'' end ) StoreBrandCode ");
                        actionSql.AppendLine("                     from   " + tmpRuleAct + "  a");
                        //actionSql.AppendLine("                     inner join V_M_TM_Mem_Trade_service  b on a.TradeID=b.TradeID");
                        //actionSql.AppendLine("                     inner join V_M_TM_SYS_BaseData_store c on b.StoreCodeservice=c.StoreCode  ");
                        //actionSql.AppendLine("                     inner join V_M_TM_Mem_SubExt_vehicle e on b.CarIdService=e.MemberSubExtID  ");
                        actionSql.AppendLine("                     cross  join  " + tmpLimit + " d ");
                        actionSql.AppendLine("                     group by a.AccountID,fieldname,isnull(a.SpecialDate1,''2000-01-01''), isnull(a.SpecialDate2,''2000-01-01'')    ) m");
                        actionSql.AppendLine("          inner join ");
                        actionSql.AppendLine("---------------历史账户明细限制值    ");
                        actionSql.AppendLine("            ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,");
                        actionSql.AppendLine("                     isnull(b.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(b.SpecialDate2,''2000-01-01'')  SpecialDate2");
                        //actionSql.AppendLine("                     ,max(case when Limittype=''vehicle'' then LimitValue else ''0'' end ) vehicle,");
                        //actionSql.AppendLine("                     max(case when Limittype=''store''   then LimitValue else ''0'' end ) StoreCode,");
                        //actionSql.AppendLine("                     max(case when Limittype=''brand''   then LimitValue else ''0'' end ) StoreBrandCode ");
                        actionSql.AppendLine("                     from " + tmpRuleAct + " a");
                        actionSql.AppendLine("                     inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType");
                        actionSql.AppendLine("                     inner join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID");
                        actionSql.AppendLine("                     group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''2000-01-01''), isnull(b.SpecialDate2,''2000-01-01'') ) n");
                        actionSql.AppendLine("          on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2 ");
                        //actionSql.AppendLine("          and m.vehicle=n.vehicle  and m.StoreCode=n.StoreCode and  m.StoreBrandCode=n.StoreBrandCode");
                        actionSql.AppendLine("          inner join ");
                        actionSql.AppendLine(" -----------------本次需要更新的账户数据 ");
                        actionSql.AppendLine("            ( select AccountID,fieldname AccountDetailType,isnull(SpecialDate1,''2000-01-01'') SpecialDate1, isnull(SpecialDate2,''2000-01-01'')  SpecialDate2,");
                        actionSql.AppendLine("                     sum(case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue) ");
                        actionSql.AppendLine("                     when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue)");
                        actionSql.AppendLine("                     when isnull(OffsetExpression,'''')=''''  then ComputeValue end ) DetailValue,");
                        actionSql.AppendLine("                     getdate() AddedDate,''1000'' AddedUser,getdate() ModifiedDate,''1000'' ModifiedUser");
                        actionSql.AppendLine("                     from " + tmpRuleAct + "  a");
                        actionSql.AppendLine("                     group by AccountID,fieldname,isnull(SpecialDate1,''2000-01-01'') , isnull(SpecialDate2,''2000-01-01'')  ) t ");
                        actionSql.AppendLine("          on m.AccountID=t.AccountID");
                        actionSql.AppendLine("          and m.AccountDetailType=t.AccountDetailType");
                        actionSql.AppendLine("          and m.SpecialDate1=t.SpecialDate1 ");
                        actionSql.AppendLine("          and m.SpecialDate2=t.SpecialDate2  ) temp");
                        actionSql.AppendLine("where TM_Mem_AccountDetail.AccountDetailID=temp.AccountDetailID ");
                        actionSql.AppendLine("-----------------插入部分");
                        actionSql.AppendLine("insert into TM_Mem_AccountDetail ");
                        actionSql.AppendLine("(AccountID,AccountDetailType,DetailValue,SpecialDate1,SpecialDate2,AddedDate,AddedUser,ModifiedDate,ModifiedUser) ");
                        actionSql.AppendLine("               select m.AccountID,m.AccountDetailType,t.DetailValue,");
                        actionSql.AppendLine("                      case when t.SpecialDate1=''2000-01-01'' then null else t.SpecialDate1 end SpecialDate1,");
                        actionSql.AppendLine("                      case when t.SpecialDate2=''2000-01-01'' then null else t.SpecialDate2 end SpecialDate2,");
                        actionSql.AppendLine("                      t.AddedDate,t.AddedUser,t.ModifiedDate,t.ModifiedUser");
                        actionSql.AppendLine("               from ");
                        actionSql.AppendLine("  --------------本次账户相关内容及限定值");
                        actionSql.AppendLine("               (");
                        actionSql.AppendLine("                   select a.AccountID,fieldname AccountDetailType,");
                        actionSql.AppendLine("                        isnull(a.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(a.SpecialDate2,''2000-01-01'')  SpecialDate2");
                        //actionSql.AppendLine("                        ,max(case when type=''vehicle'' then cast(e.MemberSubExtID  as nvarchar(10)) else ''0'' end ) vehicle,");
                        //actionSql.AppendLine("                         max(case when type=''store'' then StoreCode  else ''0'' end ) StoreCode,");
                        //actionSql.AppendLine("                         max(case when type=''brand'' then StoreBrandCode  else ''0'' end ) StoreBrandCode ");
                        actionSql.AppendLine("                   from   " + tmpRuleAct + "  a");
                        //actionSql.AppendLine("                   inner join V_M_TM_Mem_Trade_service  b on a.TradeID=b.TradeID");
                        //actionSql.AppendLine("	                 inner join V_M_TM_SYS_BaseData_store c on b.StoreCodeservice=c.StoreCode ");
                        //actionSql.AppendLine("                   inner join V_M_TM_Mem_SubExt_vehicle e on b.CarIdService=e.MemberSubExtID");
                        actionSql.AppendLine("                   cross join  " + tmpLimit + " d ");
                        actionSql.AppendLine("			         group by a.AccountID,fieldname,isnull(a.SpecialDate1,''2000-01-01''), isnull(a.SpecialDate2,''2000-01-01'')    ) m");
                        actionSql.AppendLine("              left join ");
                        //  ---------------历史账户明细限制值   
                        actionSql.AppendLine("	             ( select b.AccountID,b.AccountDetailID,b.AccountDetailType,");
                        actionSql.AppendLine("	                      isnull(b.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(b.SpecialDate2,''2000-01-01'')  SpecialDate2");
                        //actionSql.AppendLine("                        ,max(case when Limittype=''vehicle'' then LimitValue else ''0'' end ) vehicle,");
                        //actionSql.AppendLine("                        max(case when Limittype=''store''   then LimitValue else ''0'' end ) StoreCode,");
                        //actionSql.AppendLine("                        max(case when Limittype=''brand''   then LimitValue else ''0'' end ) StoreBrandCode ");
                        actionSql.AppendLine("                 from " + tmpRuleAct + " a");
                        actionSql.AppendLine("                 inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType");
                        actionSql.AppendLine("                 inner join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID");
                        actionSql.AppendLine("	               group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''2000-01-01''), isnull(b.SpecialDate2,''2000-01-01'') ) n");
                        actionSql.AppendLine("	              on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2  ");
                        //actionSql.AppendLine("                and m.vehicle=n.vehicle  and m.StoreCode=n.StoreCode and  m.StoreBrandCode=n.StoreBrandCode");
                        actionSql.AppendLine("              inner join ");
                        //-----------------本次需要更新的账户数据 
                        actionSql.AppendLine("               (select AccountID,fieldname AccountDetailType,isnull(SpecialDate1,''2000-01-01'') SpecialDate1, isnull(SpecialDate2,''2000-01-01'')  SpecialDate2,");
                        actionSql.AppendLine("                       sum(case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue) ");
                        actionSql.AppendLine("                                when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue)");
                        actionSql.AppendLine("                                when isnull(OffsetExpression,'''')=''''  then ComputeValue end ) DetailValue,");
                        actionSql.AppendLine("                       getdate() AddedDate,''1000'' AddedUser,getdate() ModifiedDate,''1000'' ModifiedUser ");
                        actionSql.AppendLine("                from " + tmpRuleAct + "  a");
                        actionSql.AppendLine("                group by AccountID,fieldname,isnull(SpecialDate1,''2000-01-01'') , isnull(SpecialDate2,''2000-01-01'')  ) t  ");
                        actionSql.AppendLine("	              on  m.AccountID=t.AccountID ");
                        actionSql.AppendLine("                and m.AccountDetailType=t.AccountDetailType");
                        actionSql.AppendLine("                and m.SpecialDate1=t.SpecialDate1  ");
                        actionSql.AppendLine("                and m.SpecialDate2=t.SpecialDate2 ");
                        actionSql.AppendLine("              where n.AccountID is null ");
                        actionSql.AppendLine("	                   and  n.AccountDetailType  is null");
                        actionSql.AppendLine("			           and  n.SpecialDate1 is null   ");
                        actionSql.AppendLine("                     and  n.SpecialDate2 is null ");
                        //actionSql.AppendLine("                     and  n.vehicle is null ");
                        //actionSql.AppendLine("                     and  n.StoreCode is null ");
                        //actionSql.AppendLine("	                   and  n.StoreBrandCode is null ");
                        actionSql.AppendLine("end");
                        actionSql.AppendLine("if  exists (select type  from  " + tmpLimit + " where type ='''')");
                        actionSql.AppendLine("begin");
                        actionSql.AppendLine("update TM_Mem_AccountDetail          ");
                        actionSql.AppendLine("set TM_Mem_AccountDetail.DetailValue         =TM_Mem_AccountDetail.DetailValue+temp.DetailValue        ,         ");
                        actionSql.AppendLine("    TM_Mem_AccountDetail.ModifiedDate        =getdate()          ");
                        actionSql.AppendLine("from  (       ");
                        actionSql.AppendLine("          select n.AccountID,n.AccountDetailID,n.AccountDetailType,t.DetailValue   ");
                        actionSql.AppendLine("          from     ");
                        actionSql.AppendLine(" --------------本次账户相关内容及限定值");
                        actionSql.AppendLine("          (");
                        actionSql.AppendLine("            select a.AccountID,fieldname AccountDetailType, ");
                        actionSql.AppendLine("                   isnull(a.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(a.SpecialDate2,''2000-01-01'')  SpecialDate2");
                        //actionSql.AppendLine("                   , ''0'' vehicle, ''0''  StoreCode, ''0''  StoreBrandCode ");
                        actionSql.AppendLine("            from   " + tmpRuleAct + "  a ");
                        actionSql.AppendLine("            group by a.AccountID,fieldname,isnull(a.SpecialDate1,''2000-01-01''), isnull(a.SpecialDate2,''2000-01-01'')    ) m");
                        actionSql.AppendLine("          inner join ");
                        actionSql.AppendLine("---------------历史账户明细限制值 (left 存在不作限制的账户明细ID)");
                        actionSql.AppendLine("          ( select b.AccountID,b.AccountDetailID,b.AccountDetailType, ");
                        actionSql.AppendLine("                   isnull(b.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(b.SpecialDate2,''2000-01-01'')  SpecialDate2");
                        //actionSql.AppendLine("                         ,max(case when Limittype=''vehicle'' then LimitValue else ''0'' end ) vehicle,");
                        //actionSql.AppendLine("                         max(case when Limittype=''store''   then LimitValue else ''0'' end ) StoreCode, ");
                        //actionSql.AppendLine("                         max(case when Limittype=''brand''   then LimitValue else ''0'' end ) StoreBrandCode ");
                        actionSql.AppendLine("            from " + tmpRuleAct + " a ");
                        actionSql.AppendLine("            inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType");
                        actionSql.AppendLine("            inner join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID");
                        actionSql.AppendLine("            group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''2000-01-01''), isnull(b.SpecialDate2,''2000-01-01'') ) n");
                        actionSql.AppendLine("          on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2");
                        //actionSql.AppendLine("          and m.vehicle=n.vehicle  and m.StoreCode=n.StoreCode and  m.StoreBrandCode=n.StoreBrandCode");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("          inner join");
                        actionSql.AppendLine("----------------本次需要更新的账户数据 ");
                        actionSql.AppendLine("	          (select AccountID,fieldname AccountDetailType,isnull(SpecialDate1,''2000-01-01'') SpecialDate1, isnull(SpecialDate2,''2000-01-01'')  SpecialDate2,");
                        actionSql.AppendLine("                    sum(case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue)");
                        actionSql.AppendLine("                             when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue)");
                        actionSql.AppendLine("		                       when isnull(OffsetExpression,'''')=''''  then ComputeValue end ) DetailValue,");
                        actionSql.AppendLine("                    getdate() AddedDate,''1000'' AddedUser,getdate() ModifiedDate,''1000'' ModifiedUser");
                        actionSql.AppendLine("             from " + tmpRuleAct + "  a");
                        actionSql.AppendLine("             group by AccountID,fieldname,isnull(SpecialDate1,''2000-01-01'') , isnull(SpecialDate2,''2000-01-01'')  ) t");
                        actionSql.AppendLine("	        on  m.AccountID=t.AccountID");
                        actionSql.AppendLine("          and m.AccountDetailType=t.AccountDetailType");
                        actionSql.AppendLine("	        and m.SpecialDate1=t.SpecialDate1");
                        actionSql.AppendLine("	        and m.SpecialDate2=t.SpecialDate2  ) temp");
                        actionSql.AppendLine("where TM_Mem_AccountDetail.AccountDetailID=temp.AccountDetailID");
                        actionSql.AppendLine("--------插入部分");
                        actionSql.AppendLine("insert into TM_Mem_AccountDetail ");
                        actionSql.AppendLine("(AccountID,AccountDetailType,DetailValue,SpecialDate1,SpecialDate2,AddedDate,AddedUser,ModifiedDate,ModifiedUser) ");
                        actionSql.AppendLine("                                                                                                                                                                                                                    ");
                        actionSql.AppendLine("                                                                                                                                                                                                                    ");
                        actionSql.AppendLine("                select m.AccountID,m.AccountDetailType,t.DetailValue, ");
                        actionSql.AppendLine("    	                 case when t.SpecialDate1=''2000-01-01'' then null else t.SpecialDate1 end SpecialDate1, ");
                        actionSql.AppendLine("    			         case when t.SpecialDate2=''2000-01-01'' then null else t.SpecialDate2 end SpecialDate2, ");
                        actionSql.AppendLine("    	                t.AddedDate,t.AddedUser,t.ModifiedDate,t.ModifiedUser ");
                        actionSql.AppendLine("    	          from ");
                        actionSql.AppendLine("--------------本次账户相关内容及限定值 ");
                        actionSql.AppendLine("    	          ( ");
                        actionSql.AppendLine("          	        select a.AccountID,fieldname AccountDetailType, ");
                        actionSql.AppendLine("    		               isnull(a.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(a.SpecialDate2,''2000-01-01'')  SpecialDate2 ");
                        //actionSql.AppendLine("    		                  ,''0'' vehicle, ''0''  StoreCode, ''0''  StoreBrandCode ");
                        actionSql.AppendLine("    		        from   " + tmpRuleAct + "  a ");
                        actionSql.AppendLine("    		        group by a.AccountID,fieldname,isnull(a.SpecialDate1,''2000-01-01''), isnull(a.SpecialDate2,''2000-01-01'')    ) m ");
                        actionSql.AppendLine("                 left join ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("---------------历史账户明细限制值 ");
                        actionSql.AppendLine("                  ( select b.AccountID,b.AccountDetailID,b.AccountDetailType, ");
                        actionSql.AppendLine("    	                  isnull(b.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(b.SpecialDate2,''2000-01-01'')  SpecialDate2 ");
                        //actionSql.AppendLine("    	                       ,max(case when Limittype=''vehicle'' then LimitValue else ''0'' end ) vehicle, ");
                        //actionSql.AppendLine("    		                   max(case when Limittype=''store''   then LimitValue else ''0'' end ) StoreCode, ");
                        //actionSql.AppendLine("    				           max(case when Limittype=''brand''   then LimitValue else ''0'' end ) StoreBrandCode ");
                        actionSql.AppendLine("    	           from   " + tmpRuleAct + "  a ");
                        actionSql.AppendLine("    	           inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType ");
                        actionSql.AppendLine("    	           inner join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID ");
                        actionSql.AppendLine("    	           group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''2000-01-01''), isnull(b.SpecialDate2,''2000-01-01'') ) n ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("                 on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2 ");
                        //actionSql.AppendLine("    	         and m.vehicle=n.vehicle  and m.StoreCode=n.StoreCode and  m.StoreBrandCode=n.StoreBrandCode ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("    	         inner join ");
                        actionSql.AppendLine("            -----------------本次需要更新的账户数据 ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("    	          (select AccountID,fieldname AccountDetailType,isnull(SpecialDate1,''2000-01-01'') SpecialDate1, isnull(SpecialDate2,''2000-01-01'')  SpecialDate2, ");
                        actionSql.AppendLine("                      sum(case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue) ");
                        actionSql.AppendLine("                               when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue) ");
                        actionSql.AppendLine("    		                   when isnull(OffsetExpression,'''')=''''  then ComputeValue end ) DetailValue, ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("                            getdate() AddedDate,''1000'' AddedUser,getdate() ModifiedDate,''1000'' ModifiedUser ");
                        actionSql.AppendLine("                    from   " + tmpRuleAct + "  a ");
                        actionSql.AppendLine("                   group by AccountID,fieldname,isnull(SpecialDate1,''2000-01-01'') , isnull(SpecialDate2,''2000-01-01'')  ) t ");
                        actionSql.AppendLine("    	        on  m.AccountID=t.AccountID ");
                        actionSql.AppendLine("                and m.AccountDetailType=t.AccountDetailType ");
                        actionSql.AppendLine("    	        and m.SpecialDate1=t.SpecialDate1 ");
                        actionSql.AppendLine("    	        and m.SpecialDate2=t.SpecialDate2 ");
                        actionSql.AppendLine("            where n.AccountID is null ");
                        actionSql.AppendLine("             and  n.AccountDetailType  is null ");
                        actionSql.AppendLine("             and  n.SpecialDate1 is null ");
                        actionSql.AppendLine("             and  n.SpecialDate2 is null ");
                        //actionSql.AppendLine("             and  n.vehicle is null ");
                        //actionSql.AppendLine("             and  n.StoreCode is null ");
                        //actionSql.AppendLine("             and  n.StoreBrandCode is null ");
                        actionSql.AppendLine("end ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine(" ')");

                        actionSql.AppendLine("---更新插入账户限制  TM_Mem_AccountLimit ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("exec ('  insert into TM_Mem_AccountLimit ");
                        actionSql.AppendLine("(AccountID,AccountDetailID,LimitType,LimitValue,AddedDate) ");
                        actionSql.AppendLine("        ---------------行转列，去掉值为0的数据 ");
                        actionSql.AppendLine("                 select AccountID,AccountDetailID, attribute LimitType,value LimitValue ,getdate() ");
                        actionSql.AppendLine("    	         from ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("                 ( select m.AccountID,m.AccountDetailID, ");
                        actionSql.AppendLine("    	                 '''' vehicle , ");
                        actionSql.AppendLine("    			         '''' store , ");
                        actionSql.AppendLine("    			        '''' brand ");
                        actionSql.AppendLine("    	          from ");
                        actionSql.AppendLine("             --------------本次账户相关内容及限定值 ");
                        actionSql.AppendLine("    	          ( ");
                        actionSql.AppendLine("          	        select a.AccountID,fieldname AccountDetailType,t.AccountDetailID, ");
                        actionSql.AppendLine("    		               isnull(a.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(a.SpecialDate2,''2000-01-01'')  SpecialDate2 ");
                        //actionSql.AppendLine("    		                  ,max(case when type=''vehicle'' then cast(e.MemberSubExtID  as nvarchar(10)) else ''0'' end ) vehicle, ");
                        //actionSql.AppendLine("    		                   max(case when type=''store'' then StoreCode  else ''0'' end ) store, ");
                        //actionSql.AppendLine("    				           max(case when type=''brand'' then StoreBrandCode  else ''0'' end ) brand ");
                        actionSql.AppendLine("    		         from   " + tmpRuleAct + "  a ");
                        actionSql.AppendLine("    	            inner join ");
                        actionSql.AppendLine("    		                    ------由于限制类型尚未添加，只能取同accountid,AccountDetailType,SpecialDate1,SpecialDate2 刚刚创建插入的那条数据记录 ");
                        actionSql.AppendLine("    		                    (select x.*  from ");
                        actionSql.AppendLine("    		                    (select x.*,ROW_NUMBER() over(partition by x.accountid,AccountDetailType,x.SpecialDate1,x.SpecialDate2  order by addeddate desc ) serial ");
                        actionSql.AppendLine("    		                    from TM_Mem_AccountDetail x  ");
                        actionSql.AppendLine("                              inner join   " + tmpRuleAct + "  w on  x.AccountID=w.AccountID and w.fieldname=x.AccountDetailType ");
                        actionSql.AppendLine("                               and isnull(x.SpecialDate1,''2000-01-01'')=isnull(w.SpecialDate1,''2000-01-01'')    ");
                        actionSql.AppendLine("                               and isnull(x.SpecialDate2,''2000-01-01'')=isnull(w.SpecialDate2,''2000-01-01'')");
                        actionSql.AppendLine("                               )  x ");
                        actionSql.AppendLine("    					        where serial=1 ) t on ");
                        actionSql.AppendLine("                          a.AccountID=t.AccountID and a.fieldname=t.AccountDetailType ");
                        actionSql.AppendLine("                      and isnull(a.SpecialDate1,''2000-01-01'')=isnull(t.SpecialDate1,''2000-01-01'') ");
                        actionSql.AppendLine("                      and isnull(a.SpecialDate2,''2000-01-01'')=isnull(t.SpecialDate2,''2000-01-01'') ");
                        //actionSql.AppendLine("    		        inner join V_M_TM_Mem_Trade_service  b on a.TradeID=b.TradeID ");
                        //actionSql.AppendLine("                    inner join V_M_TM_SYS_BaseData_store c on b.StoreCodeservice=c.StoreCode ");
                        //actionSql.AppendLine("                    inner join V_M_TM_Mem_SubExt_vehicle e on b.CarIdService=e.MemberSubExtID ");
                        actionSql.AppendLine("    		        cross  join  " + tmpLimit + " d ");
                        actionSql.AppendLine("    		        group by a.AccountID,fieldname,t.AccountDetailID,isnull(a.SpecialDate1,''2000-01-01''), isnull(a.SpecialDate2,''2000-01-01'')    ) m ");
                        actionSql.AppendLine("                 left join ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("             ---------------历史账户明细限制值 ");
                        actionSql.AppendLine("                  ( select b.AccountID,b.AccountDetailID,b.AccountDetailType, ");
                        actionSql.AppendLine("    	                  isnull(b.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(b.SpecialDate2,''2000-01-01'')  SpecialDate2, ");
                        actionSql.AppendLine("    	                       max(case when Limittype=''vehicle'' then LimitValue else ''0'' end ) vehicle, ");
                        actionSql.AppendLine("    		                   max(case when Limittype=''store''   then LimitValue else ''0'' end ) store, ");
                        actionSql.AppendLine("    				           max(case when Limittype=''brand''   then LimitValue else ''0'' end ) brand ");
                        actionSql.AppendLine("    	            from   " + tmpRuleAct + "  a ");
                        actionSql.AppendLine("    	           inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType ");
                        actionSql.AppendLine("    	           inner join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID ");
                        actionSql.AppendLine("    	           group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''2000-01-01''), isnull(b.SpecialDate2,''2000-01-01'') ) n ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("                 on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2 ");
                        //actionSql.AppendLine("    	         and m.vehicle=n.vehicle  and m.store=n.store and  m.brand=n.brand ");
                        actionSql.AppendLine("            where n.AccountID is null ");
                        actionSql.AppendLine("             and  n.AccountDetailType  is null ");
                        actionSql.AppendLine("             and  n.SpecialDate1 is null ");
                        actionSql.AppendLine("             and  n.SpecialDate2 is null ");
                        actionSql.AppendLine("             and  n.vehicle is null ");
                        actionSql.AppendLine("             and  n.store is null ");
                        actionSql.AppendLine("             and  n.brand is null ");
                        actionSql.AppendLine("             ) t ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("         UNPIVOT ");
                        actionSql.AppendLine("          ( ");
                        actionSql.AppendLine("            value FOR attribute IN (store, brand,vehicle) ");
                        actionSql.AppendLine("          ) AS b ");
                        actionSql.AppendLine("        where value<>''0'' ");
                        actionSql.AppendLine("') ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("        ----更新账户表中积分账户的可用积分; ");
                        actionSql.AppendLine("        ---删除AccountType=2，通过AccountID判别其为积分或者积点账户 ");
                        actionSql.AppendLine("exec(' ");

                        actionSql.AppendLine("        update  TM_Mem_Account set value1=value1+temp.ChangeValue ,ModifiedDate=getdate() ");
                        actionSql.AppendLine("        from ( ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("             select AccountID, ");
                        actionSql.AppendLine("                      sum(case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue) ");
                        actionSql.AppendLine("                               when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue) ");
                        actionSql.AppendLine("    		                    when isnull(OffsetExpression,'''')='''' then ComputeValue end ) ChangeValue, ");
                        actionSql.AppendLine("                      getdate() AddedDate,''1000'' AddedUser,getdate() ModifiedDate,''1000'' ModifiedUser ");
                        actionSql.AppendLine("               from   " + tmpRuleAct + "  a ");
                        actionSql.AppendLine("              where fieldname=''value1'' ");
                        actionSql.AppendLine("              group by AccountID ) temp ");
                        actionSql.AppendLine("        where TM_Mem_Account.AccountID=temp.AccountID   and temp.ChangeValue<>0 ");
                        actionSql.AppendLine("') ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("        ---更新账户表中积分账户的可用积分                                                                                                                                                                           ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("exec(' ");
                        actionSql.AppendLine("        update  TM_Mem_Account set value2=value2+temp.ChangeValue ,ModifiedDate=getdate() ");
                        actionSql.AppendLine("        from ( ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("             select AccountID, ");
                        actionSql.AppendLine("                      sum(case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue) ");
                        actionSql.AppendLine("                               when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue) ");
                        actionSql.AppendLine("    		                    when isnull(OffsetExpression,'''')='''' then ComputeValue end ) ChangeValue, ");
                        actionSql.AppendLine("                      getdate() AddedDate,''1000'' AddedUser,getdate() ModifiedDate,''1000'' ModifiedUser ");
                        actionSql.AppendLine("               from   " + tmpRuleAct + "  a ");
                        actionSql.AppendLine("              where fieldname=''value2'' ");
                        actionSql.AppendLine("              group by AccountID ) temp ");
                        actionSql.AppendLine("where TM_Mem_Account.AccountID=temp.AccountID   and temp.ChangeValue<>0  ");
                        actionSql.AppendLine("') ");

                        actionSql.AppendLine("        -----明细记录值插入 ");
                        actionSql.AppendLine("exec('");
                        actionSql.AppendLine("        if exists (select type  from  " + tmpLimit + " where type !='''')   ---非空时 ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("        insert into TL_Mem_AccountChange ");
                        actionSql.AppendLine("        ( ");
                        actionSql.AppendLine("        AccountID         , ");
                        actionSql.AppendLine("        MemberID         , ");
                        actionSql.AppendLine("        AccountDetailID   , ");
                        actionSql.AppendLine("        AccountChangeType , ");
                        actionSql.AppendLine("        ChangeValue       , ");
                        actionSql.AppendLine("        ChangeReason      , ");
                        actionSql.AppendLine("        ReferenceNo       , ");
                        actionSql.AppendLine("        HasReverse        , ");
                        actionSql.AppendLine("        RuleID, ");
                        actionSql.AppendLine("        AddedDate         , ");
                        actionSql.AppendLine("        AddedUser ");
                        actionSql.AppendLine("        ) ");
                        actionSql.AppendLine("select ttt.AccountID,ttt.MemberID,ttt.AccountDetailID,ttt.AccountChangeType,ttt.ChangeValue,ttt.ChangeReason,ttt.ReferenceNo,ttt.HasReverse,ttt.RuleID,ttt.AddedDate,ttt.AddedUser");
                        actionSql.AppendLine("from ");
                        actionSql.AppendLine("(");
                        actionSql.AppendLine("        select ");
                        actionSql.AppendLine("        m.AccountID         , ");
                        actionSql.AppendLine("        m.MemberID         , ");
                        actionSql.AppendLine("        m.AccountDetailID   , ");
                        actionSql.AppendLine("        ''loy'' AccountChangeType , ");
                        actionSql.AppendLine("        (case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue) ");
                        actionSql.AppendLine("                              when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue) ");
                        actionSql.AppendLine("    		                  when isnull(OffsetExpression,'''')='''' then ComputeValue end )   ChangeValue, ");
                        actionSql.AppendLine("        ''" + Remark + "'' ChangeReason      , ");
                        actionSql.AppendLine("        t.TradeID ReferenceNo       , ");
                        actionSql.AppendLine("        0 HasReverse                , ");
                        actionSql.AppendLine("        t.RuleID                    , ");
                        actionSql.AppendLine("''" + curDatetime.ToString("yyyy-MM-dd HH:mm:ss") + "'' AddedDate,");
                        actionSql.AppendLine("        ''1000''   AddedUser ");
                        actionSql.AppendLine("        from ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("                  (  select * ");
                        actionSql.AppendLine("    	          from ( ");
                        actionSql.AppendLine("                  (select a.AccountID,a.MemberID,fieldname AccountDetailType,t.AccountDetailID, ");
                        actionSql.AppendLine("    		               isnull(a.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(a.SpecialDate2,''2000-01-01'')  SpecialDate2 ");
                        //actionSql.AppendLine("    		                  ,max(case when type=''vehicle'' then cast(e.MemberSubExtID  as nvarchar(10)) else ''0'' end ) vehicle1, ");
                        //actionSql.AppendLine("    		                   max(case when type=''store'' then StoreCode  else ''0'' end ) store1, ");
                        //actionSql.AppendLine("    				           max(case when type=''brand'' then StoreBrandCode  else ''0'' end ) brand1, f.store,f.brand,f.vehicle ");
                        actionSql.AppendLine("    		         from   " + tmpRuleAct + "  a ");
                        actionSql.AppendLine("    	            inner join ");
                        actionSql.AppendLine("    	                 TM_Mem_AccountDetail	t on ");
                        actionSql.AppendLine("                          a.AccountID=t.AccountID and a.fieldname=t.AccountDetailType ");
                        actionSql.AppendLine("                      and isnull(a.SpecialDate1,''2000-01-01'')=isnull(t.SpecialDate1,''2000-01-01'') ");
                        actionSql.AppendLine("                      and isnull(a.SpecialDate2,''2000-01-01'')=isnull(t.SpecialDate2,''2000-01-01'') ");
                        //actionSql.AppendLine("    		        inner join V_M_TM_Mem_Trade_service  b on a.TradeID=b.TradeID ");
                        //actionSql.AppendLine("                    inner join V_M_TM_SYS_BaseData_store c on b.StoreCodeservice=c.StoreCode ");
                        //actionSql.AppendLine("                    inner join V_M_TM_Mem_SubExt_vehicle e on b.CarIdService=e.MemberSubExtID ");
                        actionSql.AppendLine("    		        inner join   (select AccountID,AccountDetailID,max(store) store,max(brand) brand,max(vehicle) vehicle ");
                        actionSql.AppendLine("    		                      from ( ");
                        actionSql.AppendLine("    		                      select AccountID,AccountDetailID,isnull(store,''0'') store,isnull(brand,''0'') brand,isnull(vehicle,''0'') vehicle ");
                        actionSql.AppendLine("    	                          from TM_Mem_AccountLimit pivot( max(limitvalue) for limittype in(store,brand,vehicle)) t  ) t ");
                        actionSql.AppendLine("    					          group by  AccountID,AccountDetailID ");
                        actionSql.AppendLine("    					          )  f ");
                        actionSql.AppendLine("    					          on t.AccountID=f.AccountID and t.AccountDetailID=f.AccountDetailID ");
                        actionSql.AppendLine("    		        cross  join " + tmpLimit + "  d ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("    		        group by a.AccountID,a.MemberID,fieldname,t.AccountDetailID,isnull(a.SpecialDate1,''2000-01-01''), isnull(a.SpecialDate2,''2000-01-01'') ,f.store,f.brand,f.vehicle )  ) w ");
                        //actionSql.AppendLine("    		        where store=store1 and brand=brand1 and vehicle=vehicle1");
                        actionSql.AppendLine("                                                                          )  m ");
                        actionSql.AppendLine("                 inner join ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("             ---------------历史账户明细限制值 ");
                        actionSql.AppendLine("                  ( select b.AccountID,b.AccountDetailID,b.AccountDetailType, ");
                        actionSql.AppendLine("    	                  isnull(b.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(b.SpecialDate2,''2000-01-01'')  SpecialDate2 ");
                        //actionSql.AppendLine("    	                       ,max(case when Limittype=''vehicle'' then LimitValue else ''0'' end ) vehicle, ");
                        //actionSql.AppendLine("    		                   max(case when Limittype=''store''   then LimitValue else ''0'' end ) store, ");
                        //actionSql.AppendLine("    				           max(case when Limittype=''brand''   then LimitValue else ''0'' end ) brand ");
                        actionSql.AppendLine("    	            from   " + tmpRuleAct + "  a ");
                        actionSql.AppendLine("    	           inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType ");
                        actionSql.AppendLine("    	           inner join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID ");
                        actionSql.AppendLine("    	           group by b.AccountID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''2000-01-01''), isnull(b.SpecialDate2,''2000-01-01'') ) n ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("                 on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2 ");
                        //actionSql.AppendLine("    	         and m.vehicle=n.vehicle  and m.store=n.store and  m.brand=n.brand ");
                        actionSql.AppendLine("    	         inner join   " + tmpRuleAct + "  t ");
                        actionSql.AppendLine("    	         on m.AccountID=t.AccountID and m.AccountDetailType=t.fieldname ");
                        actionSql.AppendLine("    	          and m.SpecialDate1=isnull(t.SpecialDate1,''2000-01-01'') ");
                        actionSql.AppendLine("    	          and m.SpecialDate2=isnull(t.SpecialDate2,''2000-01-01'') ");
                        actionSql.AppendLine(" )ttt");
                        actionSql.AppendLine(" where ttt.ChangeValue<>0");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("        if not exists (select type  from  " + tmpLimit + " where type !='''')   ---空时                                                                                                                                                 ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("        insert into TL_Mem_AccountChange ");
                        actionSql.AppendLine("        ( ");
                        actionSql.AppendLine("        AccountID         , ");
                        actionSql.AppendLine("        MemberID         , ");
                        actionSql.AppendLine("        AccountDetailID   , ");
                        actionSql.AppendLine("        AccountChangeType , ");
                        actionSql.AppendLine("        ChangeValue       , ");
                        actionSql.AppendLine("        ChangeReason      , ");
                        actionSql.AppendLine("        ReferenceNo       , ");
                        actionSql.AppendLine("        HasReverse        , ");
                        actionSql.AppendLine("        RuleID, ");
                        actionSql.AppendLine("        AddedDate         , ");
                        actionSql.AppendLine("        AddedUser ");
                        actionSql.AppendLine("        ) ");
                        actionSql.AppendLine("select ttt.AccountID,ttt.MemberID,ttt.AccountDetailID,ttt.AccountChangeType,ttt.ChangeValue,ttt.ChangeReason,ttt.ReferenceNo,ttt.HasReverse,ttt.RuleID,ttt.AddedDate,ttt.AddedUser");
                        actionSql.AppendLine("from ");
                        actionSql.AppendLine("(");
                        actionSql.AppendLine("        select ");
                        actionSql.AppendLine("        m.AccountID         , ");
                        actionSql.AppendLine("        m.MemberID         , ");
                        actionSql.AppendLine("        m.AccountDetailID   , ");
                        actionSql.AppendLine("        ''loy'' AccountChangeType , ");
                        actionSql.AppendLine("        (case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue) ");
                        actionSql.AppendLine("                              when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue) ");
                        actionSql.AppendLine("    		                  when isnull(OffsetExpression,'''')='''' then ComputeValue end )   ChangeValue, ");
                        actionSql.AppendLine("        ''" + Remark + "'' ChangeReason      , ");
                        actionSql.AppendLine("        t.TradeID ReferenceNo       , ");
                        actionSql.AppendLine("        0 HasReverse                , ");
                        actionSql.AppendLine("        t.RuleID                    , ");
                        actionSql.AppendLine("''" + curDatetime.ToString("yyyy-MM-dd HH:mm:ss") + "'' AddedDate,");
                        actionSql.AppendLine("        ''1000''   AddedUser ");
                        actionSql.AppendLine("        from                                                                                                                                                                                                        ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("                 (  select w.* ");
                        actionSql.AppendLine("    	          from ");
                        actionSql.AppendLine("                  (select a.AccountID,a.MemberID,fieldname AccountDetailType,t.AccountDetailID, ");
                        actionSql.AppendLine("    		               isnull(a.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(a.SpecialDate2,''2000-01-01'')  SpecialDate2 ");
                        actionSql.AppendLine("    		                  ,''0''   vehicle1,     ''0''  store1,	    ''0''  brand1,isnull(f.store,0) store, isnull(f.brand,0) brand,isnull(f.vehicle,0) vehicle ");
                        actionSql.AppendLine("    		        from   " + tmpRuleAct + "  a ");
                        actionSql.AppendLine("                      inner join TM_Mem_AccountDetail t on ");
                        actionSql.AppendLine("    		              a.AccountID=t.AccountID and a.fieldname=t.AccountDetailType ");
                        actionSql.AppendLine("                      and isnull(a.SpecialDate1,''2000-01-01'')=isnull(t.SpecialDate1,''2000-01-01'') ");
                        actionSql.AppendLine("                      and isnull(a.SpecialDate2,''2000-01-01'')=isnull(t.SpecialDate2,''2000-01-01'') ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("    		        left join (select AccountID,AccountDetailID,max(store) store,max(brand) brand,max(vehicle) vehicle ");
                        actionSql.AppendLine("    		                      from ( ");
                        actionSql.AppendLine("    		                      select AccountID,AccountDetailID,isnull(store,''0'') store,isnull(brand,''0'') brand,isnull(vehicle,''0'') vehicle ");
                        actionSql.AppendLine("    	                          from TM_Mem_AccountLimit pivot( max(limitvalue) for limittype in(store,brand,vehicle)) t  ) t ");
                        actionSql.AppendLine("    					          group by  AccountID,AccountDetailID ");
                        actionSql.AppendLine("    					          )  f ");
                        actionSql.AppendLine("    					          on t.AccountID=f.AccountID and t.AccountDetailID=f.AccountDetailID ");
                        actionSql.AppendLine("    		        group by a.AccountID,a.MemberID,fieldname,t.AccountDetailID,isnull(a.SpecialDate1,''2000-01-01''), isnull(a.SpecialDate2,''2000-01-01'') ,f.store,f.brand,f.vehicle  )w ");
                        actionSql.AppendLine("    		        where  store=store1 and brand=brand1 and vehicle=vehicle1 )  m ");
                        actionSql.AppendLine("                 inner join ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("----------------------------历史账户明细限制值                                                                                                                                                                      ");
                        actionSql.AppendLine("                  ( select b.AccountID,b.AccountDetailID,b.AccountDetailType, ");
                        actionSql.AppendLine("    	                  isnull(b.SpecialDate1,''2000-01-01'') SpecialDate1, isnull(b.SpecialDate2,''2000-01-01'')  SpecialDate2, ");
                        actionSql.AppendLine("    	                       max(case when Limittype=''vehicle'' then LimitValue else ''0'' end ) vehicle,                                                                                                          ");
                        actionSql.AppendLine("    		                   max(case when Limittype=''store''   then LimitValue else ''0'' end ) store, ");
                        actionSql.AppendLine("    				           max(case when Limittype=''brand''   then LimitValue else ''0'' end ) brand ");
                        actionSql.AppendLine("    	           from   " + tmpRuleAct + "  a ");
                        actionSql.AppendLine("    	           inner join TM_Mem_AccountDetail b on a.AccountID=b.AccountID and a.fieldname=b.AccountDetailType ");
                        actionSql.AppendLine("    	           inner join TM_Mem_AccountLimit  c on b.AccountID=c.AccountID and b.AccountDetailID=c.AccountDetailID ");
                        actionSql.AppendLine("    	           group by b.AccountID,a.MemberID,b.AccountDetailID,b.AccountDetailType,isnull(b.SpecialDate1,''2000-01-01''), isnull(b.SpecialDate2,''2000-01-01'') ) n ");
                        actionSql.AppendLine(" ");
                        actionSql.AppendLine("                 on m.AccountID=n.AccountID and m.AccountDetailType=n.AccountDetailType  and m.SpecialDate1=n.SpecialDate1  and m.SpecialDate2=n.SpecialDate2 ");
                        actionSql.AppendLine("    	         and m.vehicle=n.vehicle  and m.store=n.store and  m.brand=n.brand	                                                                                                                                  ");
                        actionSql.AppendLine("    	         inner join   " + tmpRuleAct + "  t ");
                        actionSql.AppendLine("    	         on m.AccountID=t.AccountID and m.AccountDetailType=t.fieldname ");
                        actionSql.AppendLine("    	          and m.SpecialDate1=isnull(t.SpecialDate1,''2000-01-01'') ");
                        actionSql.AppendLine("    	          and m.SpecialDate2=isnull(t.SpecialDate2,''2000-01-01'') ");
                        actionSql.AppendLine(")ttt");
                        actionSql.AppendLine("where ttt.ChangeValue<>0");
                        actionSql.AppendLine("  ') ");

                        actionSql.AppendLine("Drop Table " + tmpRuleAct);
                        actionSql.AppendLine("Drop Table " + tmpLimit);
                        #endregion


                    }
                    #endregion
                }
                else if (act.LeftValue.ExtName == "Account" && act.Expression != "=")
                {
                    #region 账户设置处理
                    string tmpRuleAct = "tmpRuleAct" + act.Sort.ToString() + curDatetime.DayOfYear.ToString() + curDatetime.ToString("hhmmss");



                    #endregion
                }
                else
                {
                    //确定左值
                    TD_SYS_FieldAlias leftValue = Utility.EngineHelper.GetAlias(listAlias, act.LeftValue.ExtName);

                    //确定右值
                    ConditionResult? rightConditionResult = getConditionResult(conditionResultList, act.RightValue);
                    TD_SYS_FieldAlias rightValue = rightConditionResult == null ? Utility.EngineHelper.GetAlias(listAlias, act.RightValue) : Utility.EngineHelper.GetAlias(listAlias, rightConditionResult.Value.FieldAlias);
                    //如果需要记录日志
                    if (!string.IsNullOrEmpty(leftValue.LogScript))
                    {
                        actionSql.AppendLine("");
                        actionSql.AppendFormat(leftValue.LogScript, tmpMember, act.RightValue, curDatetime.ToString("yyyy-MM-dd hh:mm:ss"));
                        actionSql.AppendLine("");
                    }
                    //拼接更新脚本
                    string tmpSql1 = "";

                    if (rightConditionResult == null && rightValue == null && strTypes.Contains(leftValue.FieldType))
                    {
                        //字符串类型
                        tmpSql1 = "Update [{0}] Set [{0}].[{1}]" + (act.Expression == "=" ? " = " : " = [{0}].[{1}] " + act.Expression.Substring(0, 1)) + "'" + act.RightValue + "'";
                    }
                    else if (rightConditionResult == null && rightValue == null && dateTypes.Contains(leftValue.FieldType))
                    {
                        //日期类型
                        if (string.IsNullOrEmpty(act.OffsetExpression))
                        {
                            tmpSql1 = "Update [{0}] Set [{0}].[{1}]" + (act.Expression == "=" ? " = " : " = [{0}].[{1}] " + act.Expression.Substring(0, 1)) + "'" + act.RightValue.Replace("DateNow", curDatetime.ToString("yyyy-MM-dd")).Replace("DatetimeNow", curDatetime.ToString("yyyy-MM-dd hh:mm:ss")) + "'";
                        }
                        else
                        {
                            string tmpDateAddExp = string.Format("DateAdd({0},{2},{1})", act.OffsetUnit, "'" + act.RightValue.Replace("DateNow", curDatetime.ToString("yyyy-MM-dd")).Replace("DatetimeNow", curDatetime.ToString("yyyy-MM-dd hh:mm:ss")) + "'", act.OffsetValue);
                            tmpSql1 = "Update [{0}] Set [{0}].[{1}]" + (act.Expression == "=" ? " = " : " = [{0}].[{1}] " + act.Expression.Substring(0, 1)) + tmpDateAddExp;
                        }
                    }
                    else if (rightConditionResult == null && rightValue == null && leftValue.FieldType == "4")
                    {
                        //布尔型
                        tmpSql1 = "Update [{0}] Set [{0}].[{1}]" + (act.Expression == "=" ? " = " : " = [{0}].[{1}] " + act.Expression.Substring(0, 1)) + act.RightValue;
                    }
                    else
                    {
                        tmpSql1 = "Update [{0}] Set [{0}].[{1}]" + (act.Expression == "=" ? " = " : " = [{0}].[{1}] " + act.Expression.Substring(0, 1)) + (rightConditionResult == null ? act.RightValue.Replace(rightValue.AliasKey, "[" + rightValue.TableName + "].[" + rightValue.FieldName + "]") : act.RightValue.Replace(rightConditionResult.Value.NameCode, "[" + tmpTradeExt + "].[" + rightConditionResult.Value.NameCode + "]")) + (act.OffsetExpression == null ? "" : act.OffsetExpression) + act.OffsetValue;
                    }
                    actionSql.AppendLine(string.Format(tmpSql1, leftValue.TableName, leftValue.FieldName));

                    actionSql.AppendLine(string.Format("Where [{0}].MemberID in (select MemberID from " + tmpMember + ")", leftValue.TableName));


                }
            }

            actionSql.AppendLine("drop table " + tmpMember);

            return actionSql.ToString();
        }


        //创建行为脚本
        private static string createActionScript_bk(List<Act> actList, List<TD_SYS_FieldAlias> listAlias, List<ConditionResult> conditionResultList, string tmpTradeTableName, string getMemberScript, DateTime curDatetime, string tmpTradeExt, int ruleId, string Remark)
        {
            string[] strTypes = new string[] { "1", "2" };
            string[] dateTypes = new string[] { "5", "6" };
            StringBuilder actionSql = new StringBuilder();

            string tmpMember = "tmpMember" + curDatetime.DayOfYear.ToString() + curDatetime.ToString("hhmmss");

            //根据条件把会员数据插入临时表
            actionSql.AppendLine("CREATE TABLE " + tmpMember + "(");
            actionSql.AppendLine("	[MemberID] [char](32) NOT NULL");
            actionSql.AppendLine(") ON [PRIMARY]");
            actionSql.AppendLine("insert into " + tmpMember);
            actionSql.AppendLine(getMemberScript.Substring(1, getMemberScript.Length - 2));

            foreach (var act in actList.OrderBy(o => o.Sort))
            {
                //账户处理
                if (act.LeftValue.ExtName == "Account")
                {
                    #region 账户累加处理
                    string tmpRuleAct = "tmpRuleAct" + act.Sort.ToString() + curDatetime.DayOfYear.ToString() + curDatetime.ToString("hhmmss");
                    TD_SYS_FieldAlias rightValue;

                    if (!string.IsNullOrEmpty(act.RightValueMax) || !string.IsNullOrEmpty(act.RightValueMin))
                    {
                        #region 如果有上下限过滤，则将数据插入临时表
                        //临时表名定义
                        string tmpTrade = "tmpTrade" + act.Sort.ToString() + curDatetime.DayOfYear.ToString() + curDatetime.ToString("hhmmss");

                        //创建临时表
                        actionSql.AppendLine("CREATE TABLE [" + tmpTrade + "](");
                        actionSql.AppendLine("  [Memberid] [nvarchar](50) NULL,");
                        actionSql.AppendLine("	[TradeID] [bigint] NULL,");
                        actionSql.AppendLine("	[ComputeValue] [decimal](18, 4) NULL,");
                        actionSql.AppendLine("	[NodeValue] [decimal](18, 2) NULL,");
                        actionSql.AppendLine("	[LoyAccBeforeValue] [decimal](18, 4) NULL,");
                        actionSql.AppendLine("	[Addeddate] [datetime] NULL,");
                        actionSql.AppendLine("	[Sort] [bigint] NULL");
                        actionSql.AppendLine(") ON [PRIMARY]");

                        //插入临时表数据
                        rightValue = Utility.EngineHelper.GetAlias(listAlias, act.RightValue);
                        TD_SYS_FieldAlias rightValueFilterAlias = Utility.EngineHelper.GetAlias(listAlias, act.RightValueFilterAlias);
                        if (rightValue.TableName == "TM_Mem_TradeDetail")
                        {
                            foreach (var cl in conditionResultList)
                            {
                                actionSql.AppendLine("insert into " + tmpTrade);
                                actionSql.AppendLine("select TM_Mem_Trade.MemberID,TM_Mem_Trade.TradeID,tdd.[" + cl.NameCode + "]," + act.RightValueMin + ",[" + rightValueFilterAlias.TableName + "].[" + rightValueFilterAlias.FieldName + "],TM_Mem_Trade.ModifiedDate,0");
                                actionSql.AppendLine("from TM_Mem_Trade");
                                actionSql.AppendLine("inner join " + tmpTradeTableName + " td on TM_Mem_Trade.TradeID = td.TradeID");
                                actionSql.AppendLine("inner join " + tmpTradeExt + " tdd on TM_Mem_Trade.TradeID = tdd.TradeID ");
                                actionSql.AppendLine("left join TM_Loy_MemExt on TM_Mem_Trade.MemberID = TM_Loy_MemExt.MemberID");
                                actionSql.AppendLine("where TM_Mem_Trade.TradeType = '" + rightValue.AliasKey + "'");
                            }
                        }
                        else
                        {
                            actionSql.AppendLine("insert into " + tmpTrade);
                            actionSql.AppendLine("select TM_Mem_Trade.MemberID,TM_Mem_Trade.TradeID,TM_Mem_Trade.[" + rightValue.FieldName + "]," + act.RightValueMin + ",[" + rightValueFilterAlias.TableName + "].[" + rightValueFilterAlias.FieldName + "],TM_Mem_Trade.ModifiedDate,0");
                            actionSql.AppendLine("from TM_Mem_Trade");
                            actionSql.AppendLine("inner join (" + tmpTradeTableName + ") td on TM_Mem_Trade.TradeID = td.TradeID");
                            actionSql.AppendLine("left join TM_Loy_MemExt on TM_Mem_Trade.MemberID = TM_Loy_MemExt.MemberID");
                            actionSql.AppendLine("where TM_Mem_Trade.TradeType = '" + rightValue.AliasKey + "'");
                        }


                        //切割上下限
                        string TETable1 = "TETable1" + act.Sort.ToString() + curDatetime.DayOfYear.ToString() + curDatetime.ToString("hhmmss");
                        string TETable2 = "TETable2" + act.Sort.ToString() + curDatetime.DayOfYear.ToString() + curDatetime.ToString("hhmmss");
                        string TETable3 = "TETable3" + act.Sort.ToString() + curDatetime.DayOfYear.ToString() + curDatetime.ToString("hhmmss");
                        string OutSplitTable = "OutSplitTable" + act.Sort.ToString() + curDatetime.DayOfYear.ToString() + curDatetime.ToString("hhmmss");

                        //重算初始累计值
                        actionSql.AppendLine("update " + tmpTrade + " set LoyAccBeforeValue=LoyAccBeforeValue-temp.accamt");
                        actionSql.AppendLine("from  (select Memberid,sum(ComputeValue) accamt");
                        actionSql.AppendLine("from " + tmpTrade);
                        actionSql.AppendLine("	   group by Memberid ) temp ");
                        actionSql.AppendLine("where " + tmpTrade + ".Memberid=temp.Memberid");

                        actionSql.AppendLine("select a.*,ROW_NUMBER() OVER (partition by a.Memberid ORDER BY a.addeddate asc) serial ,");
                        actionSql.AppendLine("SUM(ComputeValue) OVER(");
                        actionSql.AppendLine("partition by a.Memberid");
                        actionSql.AppendLine("ORDER BY a.addeddate");
                        actionSql.AppendLine("ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) accvalue ,");
                        actionSql.AppendLine("a.NodeValue-a.LoyAccBeforeValue DiffValue ");
                        actionSql.AppendLine("into " + TETable1);
                        actionSql.AppendLine("from " + tmpTrade + " a ");

                        actionSql.AppendLine("select *");
                        actionSql.AppendLine("into " + TETable2);
                        actionSql.AppendLine("from (");
                        actionSql.AppendLine("select Memberid,TradeID,ComputeValue,NodeValue,LoyAccBeforeValue,AddedDate,serial,accvalue,DiffValue, ComputeValue SplitValue ,0 sort");
                        actionSql.AppendLine("from " + TETable1 + " a");
                        actionSql.AppendLine("where accvalue<=DiffValue ");
                        actionSql.AppendLine("union all");
                        actionSql.AppendLine("select Memberid,TradeID,ComputeValue,NodeValue,LoyAccBeforeValue,AddedDate,serial,accvalue,DiffValue,accvalue-DiffValue   SplitValue ,2 sort");
                        actionSql.AppendLine("from " + TETable1 + " a");
                        actionSql.AppendLine("where accvalue>DiffValue ");
                        actionSql.AppendLine("union all ");
                        actionSql.AppendLine("select Memberid,TradeID,ComputeValue,NodeValue,LoyAccBeforeValue,AddedDate,serial,accvalue,DiffValue,ComputeValue-(accvalue-DiffValue)   SplitValue ,1 sort");
                        actionSql.AppendLine("from " + TETable1 + " a");
                        actionSql.AppendLine("where accvalue>DiffValue ) t ");

                        actionSql.AppendLine("delete from " + TETable2 + " where  SplitValue=0 ");

                        actionSql.AppendLine("select * ");
                        actionSql.AppendLine("into " + TETable3);
                        actionSql.AppendLine("from (");
                        actionSql.AppendLine("select a.* ");
                        actionSql.AppendLine("from " + TETable2 + " a");
                        actionSql.AppendLine("left join  (");
                        actionSql.AppendLine("select  Memberid,TradeID ");
                        actionSql.AppendLine("from " + TETable2 + " a");
                        actionSql.AppendLine("where SplitValue<0 ");
                        actionSql.AppendLine("group by Memberid,TradeID  ) b on a.Memberid=b.Memberid and a.TradeID=b.TradeID");
                        actionSql.AppendLine("where b.Memberid is null and b.TradeID is null ");
                        actionSql.AppendLine("union all");
                        actionSql.AppendLine("select  a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.serial,a.accvalue,a.DiffValue,sum(SplitValue) SplitValue,0 sort");
                        actionSql.AppendLine("from " + TETable2 + " a");
                        actionSql.AppendLine("inner join  (");
                        actionSql.AppendLine("select  Memberid,TradeID ");
                        actionSql.AppendLine("from " + TETable2 + " a");
                        actionSql.AppendLine("where SplitValue<0 ");
                        actionSql.AppendLine("group by Memberid,TradeID  ) b on a.Memberid=b.Memberid and a.TradeID=b.TradeID");
                        actionSql.AppendLine("group by a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.serial,a.accvalue,a.DiffValue ) t ");
                        actionSql.AppendLine("order by memberid ,tradeid ,sort");

                        actionSql.AppendLine("Drop table " + tmpTrade);

                        //将结果输出到初始表并更新砍数
                        actionSql.AppendLine("select a.Memberid,a.TradeID,a.SplitValue as ComputeValue," + act.RightValueMax + " as NodeValue,a.LoyAccBeforeValue,a.AddedDate,Sort");
                        actionSql.AppendLine("into " + tmpTrade);
                        actionSql.AppendLine("from " + TETable3 + " a ");

                        //释放临时表
                        actionSql.AppendLine("Drop table " + TETable1);
                        actionSql.AppendLine("Drop table " + TETable2);
                        actionSql.AppendLine("Drop table " + TETable3);


                        //处理上限
                        actionSql.AppendLine("select a.*,ROW_NUMBER() OVER (partition by a.Memberid ORDER BY a.addeddate asc) serial ,");
                        actionSql.AppendLine("SUM(ComputeValue) OVER(");
                        actionSql.AppendLine("partition by a.Memberid");
                        actionSql.AppendLine("ORDER BY a.addeddate,a.TradeID,Sort");
                        actionSql.AppendLine("ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) accvalue ,");
                        actionSql.AppendLine("a.NodeValue-a.LoyAccBeforeValue DiffValue ");
                        actionSql.AppendLine("into " + TETable1);
                        actionSql.AppendLine("from " + tmpTrade + " a ");

                        actionSql.AppendLine("select *");
                        actionSql.AppendLine("into " + TETable2);
                        actionSql.AppendLine("from (");
                        actionSql.AppendLine("select Memberid,TradeID,ComputeValue,NodeValue,LoyAccBeforeValue,AddedDate,serial,accvalue,DiffValue, ComputeValue SplitValue ,0 sort");
                        actionSql.AppendLine("from " + TETable1 + " a");
                        actionSql.AppendLine("where accvalue<=DiffValue ");
                        actionSql.AppendLine("union all");
                        actionSql.AppendLine("select Memberid,TradeID,ComputeValue,NodeValue,LoyAccBeforeValue,AddedDate,serial,accvalue,DiffValue,accvalue-DiffValue   SplitValue ,2 sort");
                        actionSql.AppendLine("from " + TETable1 + " a");
                        actionSql.AppendLine("where accvalue>DiffValue ");
                        actionSql.AppendLine("union all ");
                        actionSql.AppendLine("select Memberid,TradeID,ComputeValue,NodeValue,LoyAccBeforeValue,AddedDate,serial,accvalue,DiffValue,ComputeValue-(accvalue-DiffValue)   SplitValue ,1 sort");
                        actionSql.AppendLine("from " + TETable1 + " a");
                        actionSql.AppendLine("where accvalue>DiffValue ) t ");

                        actionSql.AppendLine("delete from " + TETable2 + " where  SplitValue=0 ");

                        actionSql.AppendLine("select * ");
                        actionSql.AppendLine("into " + TETable3);
                        actionSql.AppendLine("from (");
                        actionSql.AppendLine("select a.* ");
                        actionSql.AppendLine("from " + TETable2 + " a");
                        actionSql.AppendLine("left join  (");
                        actionSql.AppendLine("select  Memberid,TradeID ");
                        actionSql.AppendLine("from " + TETable2 + " a");
                        actionSql.AppendLine("where SplitValue<0 ");
                        actionSql.AppendLine("group by Memberid,TradeID  ) b on a.Memberid=b.Memberid and a.TradeID=b.TradeID");
                        actionSql.AppendLine("where b.Memberid is null and b.TradeID is null ");
                        actionSql.AppendLine("union all");
                        actionSql.AppendLine("select  a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.serial,a.accvalue,a.DiffValue,sum(SplitValue) SplitValue,0 sort");
                        actionSql.AppendLine("from " + TETable2 + " a");
                        actionSql.AppendLine("inner join  (");
                        actionSql.AppendLine("select  Memberid,TradeID ");
                        actionSql.AppendLine("from " + TETable2 + " a");
                        actionSql.AppendLine("where SplitValue<0 ");
                        actionSql.AppendLine("group by Memberid,TradeID  ) b on a.Memberid=b.Memberid and a.TradeID=b.TradeID");
                        actionSql.AppendLine("group by a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.serial,a.accvalue,a.DiffValue ) t ");
                        actionSql.AppendLine("order by memberid ,tradeid ,sort");

                        //处理最终结果并计算累计
                        actionSql.AppendLine("select a.Memberid,a.TradeID,a.ComputeValue,a.NodeValue,a.LoyAccBeforeValue,a.AddedDate,a.SplitValue,");
                        actionSql.AppendLine("a.LoyAccBeforeValue+SUM(SplitValue) OVER(");
                        actionSql.AppendLine("partition by a.Memberid ");
                        actionSql.AppendLine("ORDER BY a.serial  ,sort ");
                        actionSql.AppendLine("ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AccSplitValue ");
                        actionSql.AppendLine("into " + OutSplitTable);
                        actionSql.AppendLine("from " + TETable3 + " a ");

                        //释放临时表
                        actionSql.AppendLine("Drop table " + TETable1);
                        actionSql.AppendLine("Drop table " + TETable2);
                        actionSql.AppendLine("Drop table " + TETable3);

                        //计算账户值
                        if (rightValue == null || rightValue.AliasKey == "")
                        {
                            #region 与交易无关
                            //创建ruleact临时表
                            actionSql.AppendLine("CREATE TABLE [" + tmpRuleAct + "](");
                            actionSql.AppendLine("[Memberid] [char](32) NULL,");
                            actionSql.AppendLine("[AccountID] [char](32) NOT NULL,");
                            actionSql.AppendLine("[FieldName] [varchar](6) NOT NULL,");
                            actionSql.AppendLine("[SpecialDate1] [varchar](10)  NULL,");
                            actionSql.AppendLine("[SpecialDate2] [varchar](10)  NULL,");
                            actionSql.AppendLine("[OffsetExpression] [varchar](1) NOT NULL,");
                            actionSql.AppendLine("[OffsetValue] [varchar](10) NOT NULL,");
                            actionSql.AppendLine("[RuleID] [int] NOT NULL,");
                            actionSql.AppendLine("[ComputeValue] [decimal](18, 4) NOT NULL,");
                            actionSql.AppendLine("[TradeID] [int] NULL");
                            actionSql.AppendLine(") ON [PRIMARY]");
                            actionSql.AppendLine("insert into " + tmpRuleAct);
                            actionSql.AppendLine("select distinct ");
                            actionSql.AppendLine(OutSplitTable + ".Memberid,");
                            actionSql.AppendLine("TM_Mem_Account.AccountID,");
                            if (!string.IsNullOrEmpty(act.FreezeValue))//有冻结
                            {
                                actionSql.AppendLine("'value2' as FieldName,");
                                switch (act.FreezeUnit)
                                {
                                    case "day":
                                        actionSql.AppendLine("cast ('" + curDatetime.AddDays(double.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as date) as SpecialDate1,");
                                        break;
                                    case "month":
                                        actionSql.AppendLine("cast ('" + curDatetime.AddMonths(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as date) as SpecialDate1,");
                                        break;
                                    case "year":
                                        actionSql.AppendLine("cast ('" + curDatetime.AddYears(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as date) as SpecialDate1,");
                                        break;
                                    default:
                                        actionSql.AppendLine("cast(null as date) as SpecialDate1,");
                                        break;
                                }
                                switch (act.AvailabeUnit)
                                {
                                    case "day":
                                        actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        break;
                                    case "month":
                                        if (string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    case "year":
                                        if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    default:
                                        actionSql.AppendLine("cast(null as date) SpecialDate2,");
                                        break;
                                }
                            }
                            else
                            {
                                actionSql.AppendLine("'value1' as FieldName,");
                                actionSql.AppendLine("cast(null as date) as SpecialDate1,");
                                switch (act.AvailabeUnit)
                                {
                                    case "day":
                                        actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(string.IsNullOrEmpty(act.AvailabeValue) ? "0" : act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        break;
                                    case "month":
                                        if (string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    case "year":
                                        if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    default:
                                        actionSql.AppendLine("cast(null as date) as SpecialDate2,");
                                        break;
                                }
                            }
                            actionSql.AppendLine("'" + act.OffsetExpression + "' as OffsetExpression,");
                            actionSql.AppendLine("'" + act.OffsetValue + "' as OffsetValue,");
                            actionSql.AppendLine(ruleId + " as RuleID,");
                            actionSql.AppendLine(OutSplitTable + ".SplitValue as ComputeValue,");
                            actionSql.AppendLine("null as TradeID");
                            //actionSql.AppendLine("into " + tmpRuleAct);
                            actionSql.AppendLine("from " + OutSplitTable + " inner join TM_Mem_Account on " + OutSplitTable + ".Memberid = TM_Mem_Account.MemberID");
                            actionSql.AppendLine("where TM_Mem_Account.AccountType = '" + act.LeftValue.ExtType + "' and ");
                            actionSql.AppendLine("AccSplitValue > " + act.RightValueMin + " and AccSplitValue <= " + act.RightValueMax);
                            #endregion

                        }
                        else
                        {
                            #region 与交易有关
                            //创建ruleact临时表
                            actionSql.AppendLine("CREATE TABLE [" + tmpRuleAct + "](");
                            actionSql.AppendLine("[Memberid] [char](32) NULL,");
                            actionSql.AppendLine("[AccountID] [char](32) NOT NULL,");
                            actionSql.AppendLine("[FieldName] [varchar](6) NOT NULL,");
                            actionSql.AppendLine("[SpecialDate1] [varchar](10)  NULL,");
                            actionSql.AppendLine("[SpecialDate2] [varchar](10)  NULL,");
                            actionSql.AppendLine("[OffsetExpression] [varchar](1) NOT NULL,");
                            actionSql.AppendLine("[OffsetValue] [varchar](10) NOT NULL,");
                            actionSql.AppendLine("[RuleID] [int] NOT NULL,");
                            actionSql.AppendLine("[ComputeValue] [decimal](18, 4) NOT NULL,");
                            actionSql.AppendLine("[TradeID] [int] NULL");
                            actionSql.AppendLine(") ON [PRIMARY]");
                            actionSql.AppendLine("insert into " + tmpRuleAct);
                            actionSql.AppendLine("select");
                            actionSql.AppendLine(OutSplitTable + ".Memberid,");
                            actionSql.AppendLine("TM_Mem_Account.AccountID,");
                            if (!string.IsNullOrEmpty(act.FreezeValue))//有冻结
                            {
                                actionSql.AppendLine("'value2' as FieldName,");
                                switch (act.FreezeUnit)
                                {
                                    case "day":
                                        actionSql.AppendLine("cast ('" + curDatetime.AddDays(double.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as date) as SpecialDate1,");
                                        break;
                                    case "month":
                                        actionSql.AppendLine("cast ('" + curDatetime.AddMonths(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as date) as SpecialDate1,");
                                        break;
                                    case "year":
                                        actionSql.AppendLine("cast ('" + curDatetime.AddYears(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as date) as SpecialDate1,");
                                        break;
                                    default:
                                        actionSql.AppendLine("cast(null as date) as SpecialDate1,");
                                        break;
                                }
                                switch (act.AvailabeUnit)
                                {
                                    case "day":
                                        actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        break;
                                    case "month":
                                        if (string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    case "year":
                                        if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    default:
                                        actionSql.AppendLine("cast(null as date) SpecialDate2,");
                                        break;
                                }
                            }
                            else
                            {
                                actionSql.AppendLine("'value1' as FieldName,");
                                actionSql.AppendLine("cast(null as date) as SpecialDate1,");
                                switch (act.AvailabeUnit)
                                {
                                    case "day":
                                        actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(string.IsNullOrEmpty(act.AvailabeValue) ? "0" : act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        break;
                                    case "month":
                                        if (string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    case "year":
                                        if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    default:
                                        actionSql.AppendLine("cast(null as date) as SpecialDate2,");
                                        break;
                                }
                            }
                            actionSql.AppendLine("'" + act.OffsetExpression + "' as OffsetExpression,");
                            actionSql.AppendLine("'" + act.OffsetValue + "' as OffsetValue,");
                            actionSql.AppendLine(ruleId + " as RuleID,");
                            actionSql.AppendLine(OutSplitTable + ".SplitValue as ComputeValue,");
                            actionSql.AppendLine(OutSplitTable + ".TradeID");
                            //actionSql.AppendLine("into " + tmpRuleAct);
                            actionSql.AppendLine("from " + OutSplitTable + " inner join TM_Mem_Account on " + OutSplitTable + ".Memberid = TM_Mem_Account.MemberID");
                            actionSql.AppendLine("where TM_Mem_Account.AccountType = '" + act.LeftValue.ExtType + "' and ");
                            actionSql.AppendLine("AccSplitValue > " + act.RightValueMin + " and AccSplitValue <= " + act.RightValueMax);
                            #endregion

                        }
                        actionSql.AppendLine("Drop Table " + OutSplitTable);
                        actionSql.AppendLine("Drop Table " + tmpTrade);
                    }
                        #endregion
                    else
                    {
                        #region 无上下限过滤
                        //计算账户值
                        //确定右值
                        ConditionResult? rightConditionResult = getConditionResult(conditionResultList, act.RightValue);
                        rightValue = rightConditionResult == null ? Utility.EngineHelper.GetAlias(listAlias, act.RightValue) : Utility.EngineHelper.GetAlias(listAlias, rightConditionResult.Value.FieldAlias);
                        if (rightValue == null || rightValue.AliasKey == "")
                        {
                            #region 右值处理和交易无关
                            //创建ruleact临时表
                            actionSql.AppendLine("CREATE TABLE [" + tmpRuleAct + "](");
                            actionSql.AppendLine("[Memberid] [char](32) NULL,");
                            actionSql.AppendLine("[AccountID] [char](32) NOT NULL,");
                            actionSql.AppendLine("[FieldName] [varchar](6) NOT NULL,");
                            actionSql.AppendLine("[SpecialDate1] [varchar](10)  NULL,");
                            actionSql.AppendLine("[SpecialDate2] [varchar](10)  NULL,");
                            actionSql.AppendLine("[OffsetExpression] [varchar](1) NOT NULL,");
                            actionSql.AppendLine("[OffsetValue] [varchar](10) NOT NULL,");
                            actionSql.AppendLine("[RuleID] [int] NOT NULL,");
                            actionSql.AppendLine("[ComputeValue] [decimal](18, 4) NOT NULL,");
                            actionSql.AppendLine("[TradeID] [int] NULL");
                            actionSql.AppendLine(") ON [PRIMARY]");
                            actionSql.AppendLine("insert into " + tmpRuleAct);
                            actionSql.AppendLine("select distinct ");
                            actionSql.AppendLine("TM_Mem_Trade.Memberid,");
                            actionSql.AppendLine("TM_Mem_Account.AccountID,");
                            if (!string.IsNullOrEmpty(act.FreezeValue))//有冻结
                            {
                                actionSql.AppendLine("'value2' as FieldName,");
                                switch (act.FreezeUnit)
                                {
                                    case "day":
                                        actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate1,");
                                        break;
                                    case "month":
                                        actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate1,");
                                        break;
                                    case "year":
                                        actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate1,");
                                        break;
                                    default:
                                        actionSql.AppendLine("cast(null as date) SpecialDate1,");
                                        break;
                                }
                                switch (act.AvailabeUnit)
                                {
                                    case "day":
                                        actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        break;
                                    case "month":
                                        if (string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    case "year":
                                        if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    default:
                                        actionSql.AppendLine("cast(null as date) SpecialDate2,");
                                        break;
                                }
                            }
                            else
                            {
                                actionSql.AppendLine("'value1' as FieldName,");
                                actionSql.AppendLine("cast(null as date) as SpecialDate1,");
                                switch (act.AvailabeUnit)
                                {
                                    case "day":
                                        actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(string.IsNullOrEmpty(act.AvailabeValue) ? "0" : act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        break;
                                    case "month":
                                        if (string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    case "year":
                                        if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                        else
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                        break;
                                    default:
                                        actionSql.AppendLine("cast(null as date) as SpecialDate2,");
                                        break;
                                }
                            }
                            actionSql.AppendLine("'" + act.OffsetExpression + "' as OffsetExpression,");
                            actionSql.AppendLine("'" + act.OffsetValue + "' as OffsetValue,");
                            actionSql.AppendLine(ruleId + " as RuleID,");
                            actionSql.AppendLine("isnull(" + (rightValue != null ? rightValue.FieldName : act.RightValue) + " , 0) as ComputeValue,");
                            actionSql.AppendLine("null as TradeID");
                            actionSql.AppendLine("from (" + tmpTradeTableName + ") TradeTable inner join TM_Mem_Trade on TradeTable.TradeID = TM_Mem_Trade.TradeID inner join TM_Mem_Account on TM_Mem_Trade.Memberid = TM_Mem_Account.MemberID ");
                            if (tmpTradeExt != "")
                            {
                                actionSql.AppendLine(" left join " + tmpTradeExt + " on " + tmpTradeExt + ".TradeID = TradeTable.TradeID");
                            }
                            actionSql.AppendLine("where TM_Mem_Account.AccountType = '" + act.LeftValue.ExtType + "' ");
                            #endregion
                        }
                        else
                        {
                            #region 右值处理和交易有关
                            //创建ruleact临时表
                            actionSql.AppendLine("CREATE TABLE [" + tmpRuleAct + "](");
                            actionSql.AppendLine("[Memberid] [char](32) NULL,");
                            actionSql.AppendLine("[AccountID] [char](32) NOT NULL,");
                            actionSql.AppendLine("[FieldName] [varchar](6) NOT NULL,");
                            actionSql.AppendLine("[SpecialDate1] [varchar](10)  NULL,");
                            actionSql.AppendLine("[SpecialDate2] [varchar](10)  NULL,");
                            actionSql.AppendLine("[OffsetExpression] [varchar](1) NOT NULL,");
                            actionSql.AppendLine("[OffsetValue] [varchar](10) NOT NULL,");
                            actionSql.AppendLine("[RuleID] [int] NOT NULL,");
                            actionSql.AppendLine("[ComputeValue] [decimal](18, 4) NOT NULL,");
                            actionSql.AppendLine("[TradeID] [int] NULL");
                            actionSql.AppendLine(") ON [PRIMARY]");

                            if (rightValue.TableName == "TM_Mem_TradeDetail")
                            {
                                foreach (var cl in conditionResultList)
                                {
                                    actionSql.AppendLine("insert into " + tmpRuleAct);
                                    actionSql.AppendLine("select");
                                    actionSql.AppendLine("TM_Mem_Trade.Memberid,");
                                    actionSql.AppendLine("TM_Mem_Account.AccountID,");
                                    if (!string.IsNullOrEmpty(act.FreezeValue))//有冻结
                                    {
                                        actionSql.AppendLine("'value2' as FieldName,");
                                        switch (act.FreezeUnit)
                                        {
                                            case "day":
                                                actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate1,");
                                                break;
                                            case "month":
                                                actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate1,");
                                                break;
                                            case "year":
                                                actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate1,");
                                                break;
                                            default:
                                                actionSql.AppendLine("cast(null as date) SpecialDate1,");
                                                break;
                                        }
                                        switch (act.AvailabeUnit)
                                        {
                                            case "day":
                                                actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                                break;
                                            case "month":
                                                if (string.IsNullOrEmpty(act.OffsetDay))
                                                    actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                                else
                                                    actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                                break;
                                            case "year":
                                                if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                                    actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                                else
                                                    actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                                break;
                                            default:
                                                actionSql.AppendLine("cast(null as date) SpecialDate2,");
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        actionSql.AppendLine("'value1' as FieldName,");
                                        actionSql.AppendLine("cast(null as date) as SpecialDate1,");
                                        switch (act.AvailabeUnit)
                                        {
                                            case "day":
                                                actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(string.IsNullOrEmpty(act.AvailabeValue) ? "0" : act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                                break;
                                            case "month":
                                                if (string.IsNullOrEmpty(act.OffsetDay))
                                                    actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                                else
                                                    actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                                break;
                                            case "year":
                                                if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                                    actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                                else
                                                    actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                                break;
                                            default:
                                                actionSql.AppendLine("cast(null as date) as SpecialDate2,");
                                                break;
                                        }
                                    }
                                    actionSql.AppendLine("'" + act.OffsetExpression + "' as OffsetExpression,");
                                    actionSql.AppendLine("'" + act.OffsetValue + "' as OffsetValue,");
                                    actionSql.AppendLine(ruleId + " as RuleID,");
                                    actionSql.AppendLine("isnull(" + tmpTradeExt + "." + cl.NameCode + " , 0) as ComputeValue,");
                                    actionSql.AppendLine("TradeTable.TradeID");
                                    //actionSql.AppendLine("into " + tmpRuleAct);
                                    actionSql.AppendLine("from (" + tmpTradeTableName + ") TradeTable inner join TM_Mem_Trade on TradeTable.TradeID = TM_Mem_Trade.TradeID inner join TM_Mem_Account on TM_Mem_Trade.Memberid = TM_Mem_Account.MemberID ");
                                    if (tmpTradeExt != "")
                                    {
                                        actionSql.AppendLine(" left join " + tmpTradeExt + " on " + tmpTradeExt + ".TradeID = TradeTable.TradeID");
                                    }
                                    actionSql.AppendLine("where TM_Mem_Account.AccountType = '" + act.LeftValue.ExtType + "' ");
                                }

                            }
                            else
                            {
                                actionSql.AppendLine("insert into " + tmpRuleAct);
                                actionSql.AppendLine("select");
                                actionSql.AppendLine("TM_Mem_Trade.Memberid,");
                                actionSql.AppendLine("TM_Mem_Account.AccountID,");
                                if (!string.IsNullOrEmpty(act.FreezeValue))//有冻结
                                {
                                    actionSql.AppendLine("'value2' as FieldName,");
                                    switch (act.FreezeUnit)
                                    {
                                        case "day":
                                            actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate1,");
                                            break;
                                        case "month":
                                            actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate1,");
                                            break;
                                        case "year":
                                            actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.FreezeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate1,");
                                            break;
                                        default:
                                            actionSql.AppendLine("cast(null as date) SpecialDate1,");
                                            break;
                                    }
                                    switch (act.AvailabeUnit)
                                    {
                                        case "day":
                                            actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                            break;
                                        case "month":
                                            if (string.IsNullOrEmpty(act.OffsetDay))
                                                actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                            else
                                                actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                            break;
                                        case "year":
                                            if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                                actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                            else
                                                actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                            break;
                                        default:
                                            actionSql.AppendLine("cast(null as date) SpecialDate2,");
                                            break;
                                    }
                                }
                                else
                                {
                                    actionSql.AppendLine("'value1' as FieldName,");
                                    actionSql.AppendLine("cast(null as date) as SpecialDate1,");
                                    switch (act.AvailabeUnit)
                                    {
                                        case "day":
                                            actionSql.AppendLine("'" + curDatetime.AddDays(double.Parse(string.IsNullOrEmpty(act.AvailabeValue) ? "0" : act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                            break;
                                        case "month":
                                            if (string.IsNullOrEmpty(act.OffsetDay))
                                                actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + "' as SpecialDate2,");
                                            else
                                                actionSql.AppendLine("'" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + curDatetime.AddMonths(int.Parse(act.AvailabeValue)).Month.ToString() + "-" + act.OffsetDay + "' as SpecialDate2,");
                                            break;
                                        case "year":
                                            if (string.IsNullOrEmpty(act.OffsetMonth) && string.IsNullOrEmpty(act.OffsetDay))
                                                actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).ToString("yyyy-MM-dd") + " as SpecialDate2,");
                                            else
                                                actionSql.AppendLine("'" + curDatetime.AddYears(int.Parse(act.AvailabeValue)).Year.ToString() + "-" + act.OffsetMonth + "-" + act.OffsetDay + "' as SpecialDate2,");
                                            break;
                                        default:
                                            actionSql.AppendLine("cast(null as date) as SpecialDate2,");
                                            break;
                                    }
                                }
                                actionSql.AppendLine("'" + act.OffsetExpression + "' as OffsetExpression,");
                                actionSql.AppendLine("'" + act.OffsetValue + "' as OffsetValue,");
                                actionSql.AppendLine(ruleId + " as RuleID,");
                                actionSql.AppendLine("isnull(" + (rightValue != null ? rightValue.FieldName : act.RightValue) + " , 0) as ComputeValue,");
                                actionSql.AppendLine("TradeTable.TradeID");
                                //actionSql.AppendLine("into " + tmpRuleAct);
                                actionSql.AppendLine("from (" + tmpTradeTableName + ") TradeTable inner join TM_Mem_Trade on TradeTable.TradeID = TM_Mem_Trade.TradeID inner join TM_Mem_Account on TM_Mem_Trade.Memberid = TM_Mem_Account.MemberID ");
                                if (tmpTradeExt != "")
                                {
                                    actionSql.AppendLine(" left join " + tmpTradeExt + " on " + tmpTradeExt + ".TradeID = TradeTable.TradeID");
                                }
                                actionSql.AppendLine("where TM_Mem_Account.AccountType = '" + act.LeftValue.ExtType + "' ");
                            }

                            #endregion
                        }


                        #endregion
                    }

                    //限制类型临时表
                    string tmpLimit = "tmpLimit" + act.Sort.ToString() + curDatetime.DayOfYear.ToString() + curDatetime.ToString("hhmmss");
                    actionSql.AppendLine("CREATE TABLE [" + tmpLimit + "](");
                    actionSql.AppendLine("TradeID [nvarchar](50) ,");
                    actionSql.AppendLine("[Type]  [nvarchar](20) NOT NULL,");
                    actionSql.AppendLine("Value   [nvarchar](50) NOT NULL");
                    actionSql.AppendLine(") ON [PRIMARY]");
                    //foreach (string limit in act.LeftValue.ExtLimitList)
                    //{
                    //    actionSql.AppendLine("insert into " + tmpLimit + " values('" + limit + "')");
                    //}


                    //判断右值是否关联交易--计算代码
                    if (rightValue == null || rightValue.AliasKey == "")
                    {
                        #region 交易无关
                        actionSql.AppendLine("exec ('");
                        actionSql.AppendLine("if object_id(N''tempdb.dbo.#TE_Mem_AccountDetailIDNoTrade'') is not null");
                        actionSql.AppendLine("   begin");
                        actionSql.AppendLine("      drop table #TE_Mem_AccountDetailIDNoTrade ;");
                        actionSql.AppendLine("   end");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("----------2.2   不存在limit限定值的账户明细若不存在新增插入记录（账户ID，账户类型，有效期起止日期，账户明细无限定记录）");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("               select  replace(newid(),''-'','''') AccountDetailID,m.AccountID,m.fieldname  AccountDetailType,");
                        actionSql.AppendLine("                      case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue)");
                        actionSql.AppendLine("                                          when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue)");
                        actionSql.AppendLine("                                          when isnull(OffsetExpression,'''')=''''  then ComputeValue end");
                        actionSql.AppendLine("			                       DetailValue,");
                        actionSql.AppendLine("                       SpecialDate1,");
                        actionSql.AppendLine("                       SpecialDate2,");
                        actionSql.AppendLine("                       getdate() AddedDate,''1000'' AddedUser,getdate() ModifiedDate,''1000'' ModifiedUser ,");
                        actionSql.AppendLine("                 RuleID,TradeID ,memberid");
                        actionSql.AppendLine("              into #TE_Mem_AccountDetailIDNoTrade");
                        actionSql.AppendLine("               from tmpRuleAct1330080158  m");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("insert into TM_Mem_AccountDetail");
                        actionSql.AppendLine(" (AccountDetailID , AccountID,AccountDetailType,DetailValue,SpecialDate1,SpecialDate2,AddedDate,AddedUser,ModifiedDate,ModifiedUser)");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("select AccountDetailID , AccountID,AccountDetailType,DetailValue,SpecialDate1,SpecialDate2,AddedDate,AddedUser,ModifiedDate,ModifiedUser");
                        actionSql.AppendLine("from #TE_Mem_AccountDetailIDNoTrade");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("---------------------2.3无限定limit的TL_Mem_AccountChange的记录插入");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("        insert into TL_Mem_AccountChange");
                        actionSql.AppendLine("        (");
                        actionSql.AppendLine("        AccountID         ,");
                        actionSql.AppendLine("        MemberID         ,");
                        actionSql.AppendLine("        AccountDetailID   ,");
                        actionSql.AppendLine("        AccountChangeType ,");
                        actionSql.AppendLine("        ChangeValue       ,");
                        actionSql.AppendLine("        ChangeReason      ,");
                        actionSql.AppendLine("        ReferenceNo       ,");
                        actionSql.AppendLine("        HasReverse        ,");
                        actionSql.AppendLine("        RuleID,");
                        actionSql.AppendLine("        AddedDate         ,");
                        actionSql.AppendLine("        AddedUser");
                        actionSql.AppendLine("        )");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("select ttt.AccountID,ttt.MemberID,ttt.AccountDetailID,ttt.AccountChangeType,ttt.ChangeValue,ttt.ChangeReason,");
                        actionSql.AppendLine("       ttt.ReferenceNo,ttt.HasReverse,ttt.RuleID,ttt.AddedDate,ttt.AddedUser");
                        actionSql.AppendLine("from");
                        actionSql.AppendLine("(");
                        actionSql.AppendLine("        select");
                        actionSql.AppendLine("        t.AccountID         ,");
                        actionSql.AppendLine("        t.MemberID         ,");
                        actionSql.AppendLine("        t.AccountDetailID   ,");
                        actionSql.AppendLine("        ''loy'' AccountChangeType ,");
                        actionSql.AppendLine("        DetailValue  ChangeValue,");
                        actionSql.AppendLine("        ''充值现金'' ChangeReason      ,");
                        actionSql.AppendLine("        t.TradeID ReferenceNo       ,");
                        actionSql.AppendLine("        0 HasReverse                ,");
                        actionSql.AppendLine("        t.RuleID                    ,");
                        actionSql.AppendLine("        ''2016-11-25 20:01:58''  AddedDate,");
                        actionSql.AppendLine("        ''1000''   AddedUser");
                        actionSql.AppendLine("        from  #TE_Mem_AccountDetailIDNoTrade t");
                        actionSql.AppendLine(" )ttt");
                        actionSql.AppendLine(" where ttt.ChangeValue<>0");
                        actionSql.AppendLine("");
                        actionSql.AppendLine(" ')");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("----不存在账户限制值");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("----更新账户表中积分账户的可用额度和冻结额度;");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("exec('");
                        actionSql.AppendLine("        update  TM_Mem_Account set value1=value1+temp.ChangeValue ,ModifiedDate=getdate()");
                        actionSql.AppendLine("        from (");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("             select AccountID,");
                        actionSql.AppendLine("                      sum(case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue)");
                        actionSql.AppendLine("                               when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue)");
                        actionSql.AppendLine("                               when isnull(OffsetExpression,'''')='''' then ComputeValue end ) ChangeValue,");
                        actionSql.AppendLine("                      getdate() AddedDate,''1000'' AddedUser,getdate() ModifiedDate,''1000'' ModifiedUser");
                        actionSql.AppendLine("               from   tmpRuleAct1330080158  a");
                        actionSql.AppendLine("              where fieldname=''value1''");
                        actionSql.AppendLine("              group by AccountID ) temp");
                        actionSql.AppendLine("        where TM_Mem_Account.AccountID=temp.AccountID   and temp.ChangeValue<>0");
                        actionSql.AppendLine("')");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("exec('");
                        actionSql.AppendLine("update  TM_Mem_Account set value2=value2+temp.ChangeValue ,ModifiedDate=getdate()");
                        actionSql.AppendLine("        from (");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("             select AccountID,");
                        actionSql.AppendLine("                      sum(case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue)");
                        actionSql.AppendLine("                               when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue)");
                        actionSql.AppendLine("                               when isnull(OffsetExpression,'''')='''' then ComputeValue end ) ChangeValue,");
                        actionSql.AppendLine("                      getdate() AddedDate,''1000'' AddedUser,getdate() ModifiedDate,''1000'' ModifiedUser");
                        actionSql.AppendLine("               from   tmpRuleAct1330080158  a");
                        actionSql.AppendLine("              where fieldname=''value2''");
                        actionSql.AppendLine("              group by AccountID ) temp");
                        actionSql.AppendLine("where TM_Mem_Account.AccountID=temp.AccountID   and temp.ChangeValue<>0");
                        actionSql.AppendLine("')");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("Drop Table tmpRuleAct1330080158");
                        actionSql.AppendLine("Drop Table tmpLimit1330080158");
                        #endregion
                    }
                    else
                    {
                        //忠诚度只能限制交易与交易明细表
                        if (act.LeftValue.ExtLimitList.Count != 0)
                        {
                            var aliaslimit = listAlias.Where(p => !string.IsNullOrEmpty(p.DataLimitType) && (p.TableName == "TM_Mem_Trade" || p.TableName == "TM_Mem_TradeDetail"));

                            foreach (var item in act.LeftValue.ExtLimitList)
                            {
                                aliaslimit = aliaslimit.Where(p => p.DataLimitType == item);
                            }
                            bool limitflag = false;
                            StringBuilder limitSql = new StringBuilder();
                            bool flag = false;
                            foreach (var lim in aliaslimit)
                            {
                                limitflag = true;
                                if (lim.TableName == "TM_Mem_Trade")
                                {
                                    if (flag)
                                    {
                                        limitSql.AppendLine(" union all ( ");
                                    }
                                    else
                                    {
                                        limitSql.AppendLine(" ( ");
                                        flag = true;

                                    }
                                    limitSql.AppendLine("select '" + lim.DataLimitType + "' as Type , " + lim.FieldName + " as Value, t1.TradeID as TradeID from TM_mem_trade t1 ");
                                    limitSql.AppendLine("inner join (" + tmpTradeTableName + ") t2 on t1.tradeid = t2.tradeid ");
                                    limitSql.AppendLine("where t1.TradeType ='" + lim.AliasKey + "'");
                                    limitSql.AppendLine(" )");
                                }
                                if (lim.TableName == "TM_Mem_TradeDetail")
                                {
                                    if (flag)
                                    {
                                        limitSql.AppendLine(" union all ( ");

                                    }
                                    else
                                    {
                                        limitSql.AppendLine(" ( ");
                                        flag = true;

                                    }
                                    limitSql.AppendLine("select '" + lim.DataLimitType + "' as Type , t3." + lim.FieldName + " as Value, t1.TradeID as TradeID from TM_mem_trade t1 ");
                                    limitSql.AppendLine("inner join (" + tmpTradeTableName + ") t2 on t1.tradeid = t2.tradeid");
                                    limitSql.AppendLine("inner join TM_mem_tradedetail t3 on t1.tradeid = t2.tradeid");
                                    limitSql.AppendLine("where t1.TradeType ='" + lim.AliasKey + "' and t3.TradeDetailType ='" + lim.AliasSubKey + "'");
                                    limitSql.AppendLine(" )");
                                }
                            }
                            if (limitflag)
                            {
                                actionSql.AppendLine("insert into " + tmpLimit + " (Type,Value,TradeID)");
                                actionSql.AppendLine("select Type,Value,TradeID from (");
                                actionSql.AppendLine(limitSql + ")lim");
                            }
                        }
                        #region 右值处理和交易有关
                        //按照中间表进行执行
                        actionSql.AppendLine("exec ('");
                        actionSql.AppendLine("--------------------------------- 一.存在limit限定值--------------------------------------------------------");
                        actionSql.AppendLine("if exists (select type  from  " + tmpLimit + " where type !='''')");
                        actionSql.AppendLine("begin");
                        actionSql.AppendLine(" -----------------------------------1.2需要新增TM_Mem_AccountDetail 的值数据---------------------------------");
                        actionSql.AppendLine("-----------------1.2.1若不存在历史账户明细限制值（限定值类型相同，限定值的值一样）插入部分");
                        actionSql.AppendLine("    if object_id(N''tempdb.dbo.#TE_Mem_AccountDetailIDLimit'') is not null");
                        actionSql.AppendLine("    begin");
                        actionSql.AppendLine("        drop table #TE_Mem_AccountDetailIDLimit ;");
                        actionSql.AppendLine("    end");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("                select  replace(newid(),''-'','''') AccountDetailID,m.AccountID,m.fieldname  AccountDetailType,");
                        actionSql.AppendLine("                        case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue)");
                        actionSql.AppendLine("                                            when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue)");
                        actionSql.AppendLine("                                            when isnull(OffsetExpression,'''')=''''  then ComputeValue end");
                        actionSql.AppendLine("							                DetailValue,");
                        actionSql.AppendLine("                        SpecialDate1,");
                        actionSql.AppendLine("                        SpecialDate2,");
                        //actionSql.AppendLine("                        getdate() AddedDate,");
                        actionSql.AppendLine("                        ''" + curDatetime.ToString("yyyy-MM-dd HH:mm:ss") + "'' AddedDate,");
                        actionSql.AppendLine("                        ''1000'' AddedUser,");
                        actionSql.AppendLine("                        getdate() ModifiedDate,''1000'' ModifiedUser ,");
                        actionSql.AppendLine("		                RuleID,TradeID ,MemberID");
                        actionSql.AppendLine("                into #TE_Mem_AccountDetailIDLimit");
                        actionSql.AppendLine("                from " + tmpRuleAct + "  m");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("    insert into TM_Mem_AccountDetail");
                        actionSql.AppendLine("     (AccountDetailID , AccountID,AccountDetailType,DetailValue,SpecialDate1,SpecialDate2,AddedDate,AddedUser,ModifiedDate,ModifiedUser)    ");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("    select AccountDetailID , AccountID,AccountDetailType,DetailValue,SpecialDate1,SpecialDate2,AddedDate,AddedUser,ModifiedDate,ModifiedUser");
                        actionSql.AppendLine("    from #TE_Mem_AccountDetailIDLimit");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("    -----------------1.2.2  积分变动记录新增（新增accountdetailid）");
                        actionSql.AppendLine("            insert into TL_Mem_AccountChange");
                        actionSql.AppendLine("            (");
                        actionSql.AppendLine("            AccountID         ,");
                        actionSql.AppendLine("            MemberID         ,");
                        actionSql.AppendLine("            AccountDetailID   ,");
                        actionSql.AppendLine("            AccountChangeType ,");
                        actionSql.AppendLine("            ChangeValue       ,");
                        actionSql.AppendLine("            ChangeReason      ,");
                        actionSql.AppendLine("            ReferenceNo       ,");
                        actionSql.AppendLine("            HasReverse        ,");
                        actionSql.AppendLine("            RuleID,");
                        actionSql.AppendLine("            AddedDate         ,");
                        actionSql.AppendLine("            AddedUser");
                        actionSql.AppendLine("            )");
                        actionSql.AppendLine("    select ttt.AccountID,ttt.MemberID,ttt.AccountDetailID,ttt.AccountChangeType,ttt.ChangeValue,ttt.ChangeReason,");
                        actionSql.AppendLine("           ttt.ReferenceNo,ttt.HasReverse,ttt.RuleID,ttt.AddedDate,ttt.AddedUser");
                        actionSql.AppendLine("    from");
                        actionSql.AppendLine("    (");
                        actionSql.AppendLine("            select");
                        actionSql.AppendLine("            t.AccountID         ,");
                        actionSql.AppendLine("            t.MemberID         ,");
                        actionSql.AppendLine("            t.AccountDetailID   ,");
                        actionSql.AppendLine("            ''loy'' AccountChangeType ,");
                        actionSql.AppendLine("            DetailValue  ChangeValue,");
                        actionSql.AppendLine("            ''" + Remark + "'' ChangeReason      , ");
                        actionSql.AppendLine("            t.TradeID ReferenceNo       ,");
                        actionSql.AppendLine("            0 HasReverse                ,");
                        actionSql.AppendLine("            t.RuleID                    ,");
                        actionSql.AppendLine("            ''" + curDatetime.ToString("yyyy-MM-dd HH:mm:ss") + "'' AddedDate,");
                        actionSql.AppendLine("            ''1000''   AddedUser");
                        actionSql.AppendLine("            from  #TE_Mem_AccountDetailIDLimit t");
                        actionSql.AppendLine("     )ttt");
                        actionSql.AppendLine("     where ttt.ChangeValue<>0");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("    -----------------1.3若不存在历史账户明细限制值（限定值类型相同，限定值的值一样）插入部分");
                        actionSql.AppendLine("                  insert into TM_Mem_AccountLimit");
                        actionSql.AppendLine("                              (AccountID,AccountDetailID,LimitType,LimitValue,AddedDate)");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("                  select m.AccountID,m.AccountDetailID,n.Type LimitType,n.Value LimitValue,getdate()");
                        actionSql.AppendLine("                  from  #TE_Mem_AccountDetailIDLimit  m");
                        actionSql.AppendLine("                  inner join " + tmpLimit + " n  on m.tradeid=n.tradeid");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("    end");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("    --------------------------------------------- 二.不存在limit限定值--------------------------------------------------------");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("        if  not  exists (select top 1    type  from  " + tmpLimit + " )");
                        actionSql.AppendLine("    begin");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("    if object_id(N''tempdb.dbo.#TE_Mem_AccountDetailIDNoLimit'') is not null");
                        actionSql.AppendLine("       begin");
                        actionSql.AppendLine("          drop table #TE_Mem_AccountDetailIDNoLimit ;");
                        actionSql.AppendLine("       end");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("    ----------2.2   不存在limit限定值的账户明细若不存在新增插入记录（账户ID，账户类型，有效期起止日期，账户明细无限定记录）");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("                   select  replace(newid(),''-'','''') AccountDetailID,m.AccountID,m.fieldname  AccountDetailType,");
                        actionSql.AppendLine("                           case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue)");
                        actionSql.AppendLine("                                              when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue)");
                        actionSql.AppendLine("                                              when isnull(OffsetExpression,'''')=''''  then ComputeValue end");
                        actionSql.AppendLine("							                   DetailValue,");
                        actionSql.AppendLine("                           SpecialDate1,");
                        actionSql.AppendLine("                           SpecialDate2,");
                        actionSql.AppendLine("                           getdate() AddedDate,''1000'' AddedUser,getdate() ModifiedDate,''1000'' ModifiedUser ,");
                        actionSql.AppendLine("		                   RuleID,TradeID ,memberid");
                        actionSql.AppendLine("                   into #TE_Mem_AccountDetailIDNoLimit");
                        actionSql.AppendLine("                   from " + tmpRuleAct + "  m");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("    insert into TM_Mem_AccountDetail");
                        actionSql.AppendLine("     (AccountDetailID , AccountID,AccountDetailType,DetailValue,SpecialDate1,SpecialDate2,AddedDate,AddedUser,ModifiedDate,ModifiedUser)");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("    select AccountDetailID , AccountID,AccountDetailType,DetailValue,SpecialDate1,SpecialDate2,AddedDate,AddedUser,ModifiedDate,ModifiedUser");
                        actionSql.AppendLine("    from #TE_Mem_AccountDetailIDNoLimit");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("    ---------------------2.3无限定limit的TL_Mem_AccountChange的记录插入");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("            insert into TL_Mem_AccountChange");
                        actionSql.AppendLine("            (");
                        actionSql.AppendLine("            AccountID         ,");
                        actionSql.AppendLine("            MemberID         ,");
                        actionSql.AppendLine("            AccountDetailID   ,");
                        actionSql.AppendLine("            AccountChangeType ,");
                        actionSql.AppendLine("            ChangeValue       ,");
                        actionSql.AppendLine("            ChangeReason      ,");
                        actionSql.AppendLine("            ReferenceNo       ,");
                        actionSql.AppendLine("            HasReverse        ,");
                        actionSql.AppendLine("            RuleID,");
                        actionSql.AppendLine("            AddedDate         ,");
                        actionSql.AppendLine("            AddedUser");
                        actionSql.AppendLine("            )");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("    select ttt.AccountID,ttt.MemberID,ttt.AccountDetailID,ttt.AccountChangeType,ttt.ChangeValue,ttt.ChangeReason,");
                        actionSql.AppendLine("           ttt.ReferenceNo,ttt.HasReverse,ttt.RuleID,ttt.AddedDate,ttt.AddedUser");
                        actionSql.AppendLine("    from");
                        actionSql.AppendLine("    (");
                        actionSql.AppendLine("            select");
                        actionSql.AppendLine("            t.AccountID         ,");
                        actionSql.AppendLine("            t.MemberID         ,");
                        actionSql.AppendLine("            t.AccountDetailID   ,");
                        actionSql.AppendLine("            ''loy'' AccountChangeType ,");
                        actionSql.AppendLine("            DetailValue  ChangeValue,");
                        actionSql.AppendLine("            ''" + Remark + "'' ChangeReason      , ");
                        actionSql.AppendLine("            t.TradeID ReferenceNo       ,");
                        actionSql.AppendLine("            0 HasReverse                ,");
                        actionSql.AppendLine("            t.RuleID                    ,");
                        //actionSql.AppendLine("            ''2016-11-25 20:01:58''  AddedDate,");
                        actionSql.AppendLine("            ''" + curDatetime.ToString("yyyy-MM-dd HH:mm:ss") + "'' AddedDate,");
                        actionSql.AppendLine("            ''1000''   AddedUser");
                        actionSql.AppendLine("            from  #TE_Mem_AccountDetailIDNoLimit t");
                        actionSql.AppendLine("     )ttt");
                        actionSql.AppendLine("     where ttt.ChangeValue<>0");
                        actionSql.AppendLine("    end");
                        actionSql.AppendLine("     ')");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("    ------------------------------------三.更新账户表中积分账户的可用积分-------------------------------------------------------");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("    ---------------------3.1可用额度的更新（至于是现金，积分，积点账户通过accountid判别）");
                        actionSql.AppendLine("    exec('");
                        actionSql.AppendLine("            update  TM_Mem_Account set value1=value1+temp.ChangeValue ,ModifiedDate=getdate()");
                        actionSql.AppendLine("            from (");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("                 select AccountID,");
                        actionSql.AppendLine("                          sum(case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue)");
                        actionSql.AppendLine("                                   when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue)");
                        actionSql.AppendLine("                                    when isnull(OffsetExpression,'''')='''' then ComputeValue end ) ChangeValue,");
                        actionSql.AppendLine("                          getdate() AddedDate,''1000'' AddedUser,getdate() ModifiedDate,''1000'' ModifiedUser");
                        actionSql.AppendLine("                   from   " + tmpRuleAct + "  a");
                        actionSql.AppendLine("                  where fieldname=''value1''");
                        actionSql.AppendLine("                  group by AccountID ) temp");
                        actionSql.AppendLine("            where TM_Mem_Account.AccountID=temp.AccountID   and temp.ChangeValue<>0");
                        actionSql.AppendLine("    ')");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("    -------------------3.2 冻结额度的更新");
                        actionSql.AppendLine("    exec('");
                        actionSql.AppendLine("            update  TM_Mem_Account set value2=value2+temp.ChangeValue ,ModifiedDate=getdate()");
                        actionSql.AppendLine("            from (");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("                 select AccountID,");
                        actionSql.AppendLine("                          sum(case when isnull(OffsetExpression,'''')=''*'' then  ComputeValue* (OffsetValue)");
                        actionSql.AppendLine("                                   when isnull(OffsetExpression,'''')=''+'' then  ComputeValue+ (OffsetValue)");
                        actionSql.AppendLine("                                    when isnull(OffsetExpression,'''')='''' then ComputeValue end ) ChangeValue,");
                        actionSql.AppendLine("                          getdate() AddedDate,''1000'' AddedUser,getdate() ModifiedDate,''1000'' ModifiedUser");
                        actionSql.AppendLine("                   from   " + tmpRuleAct + "  a");
                        actionSql.AppendLine("                  where fieldname=''value2''");
                        actionSql.AppendLine("                  group by AccountID ) temp");
                        actionSql.AppendLine("    where TM_Mem_Account.AccountID=temp.AccountID   and temp.ChangeValue<>0");
                        actionSql.AppendLine("    ')");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("");
                        actionSql.AppendLine("    Drop Table " + tmpRuleAct);
                        actionSql.AppendLine("    Drop Table " + tmpLimit);
                        #endregion


                    }
                    #endregion
                }
                else if (act.LeftValue.ExtName == "Account" && act.Expression != "=")
                {
                    #region 账户设置处理
                    string tmpRuleAct = "tmpRuleAct" + act.Sort.ToString() + curDatetime.DayOfYear.ToString() + curDatetime.ToString("hhmmss");



                    #endregion
                }
                else
                {
                    //确定左值
                    TD_SYS_FieldAlias leftValue = Utility.EngineHelper.GetAlias(listAlias, act.LeftValue.ExtName);

                    //确定右值
                    ConditionResult? rightConditionResult = getConditionResult(conditionResultList, act.RightValue);
                    TD_SYS_FieldAlias rightValue = rightConditionResult == null ? Utility.EngineHelper.GetAlias(listAlias, act.RightValue) : Utility.EngineHelper.GetAlias(listAlias, rightConditionResult.Value.FieldAlias);
                    //如果需要记录日志
                    if (!string.IsNullOrEmpty(leftValue.LogScript))
                    {
                        actionSql.AppendLine("");
                        actionSql.AppendFormat(leftValue.LogScript, tmpMember, act.RightValue, curDatetime.ToString("yyyy-MM-dd hh:mm:ss"));
                        actionSql.AppendLine("");
                    }
                    //拼接更新脚本
                    string tmpSql1 = "";

                    if (rightConditionResult == null && rightValue == null && strTypes.Contains(leftValue.FieldType))
                    {
                        //字符串类型
                        tmpSql1 = "Update [{0}] Set [{0}].[{1}]" + (act.Expression == "=" ? " = " : " = [{0}].[{1}] " + act.Expression.Substring(0, 1)) + "'" + act.RightValue + "'";
                    }
                    else if (rightConditionResult == null && rightValue == null && dateTypes.Contains(leftValue.FieldType))
                    {
                        //日期类型
                        if (string.IsNullOrEmpty(act.OffsetExpression))
                        {
                            tmpSql1 = "Update [{0}] Set [{0}].[{1}]" + (act.Expression == "=" ? " = " : " = [{0}].[{1}] " + act.Expression.Substring(0, 1)) + "'" + act.RightValue.Replace("DateNow", curDatetime.ToString("yyyy-MM-dd")).Replace("DatetimeNow", curDatetime.ToString("yyyy-MM-dd hh:mm:ss")) + "'";
                        }
                        else
                        {
                            string tmpDateAddExp = string.Format("DateAdd({0},{2},{1})", act.OffsetUnit, "'" + act.RightValue.Replace("DateNow", curDatetime.ToString("yyyy-MM-dd")).Replace("DatetimeNow", curDatetime.ToString("yyyy-MM-dd hh:mm:ss")) + "'", act.OffsetValue);
                            tmpSql1 = "Update [{0}] Set [{0}].[{1}]" + (act.Expression == "=" ? " = " : " = [{0}].[{1}] " + act.Expression.Substring(0, 1)) + tmpDateAddExp;
                        }
                    }
                    else if (rightConditionResult == null && rightValue == null && leftValue.FieldType == "4")
                    {
                        //布尔型
                        tmpSql1 = "Update [{0}] Set [{0}].[{1}]" + (act.Expression == "=" ? " = " : " = [{0}].[{1}] " + act.Expression.Substring(0, 1)) + act.RightValue;
                    }
                    else
                    {
                        tmpSql1 = "Update [{0}] Set [{0}].[{1}]" + (act.Expression == "=" ? " = " : " = [{0}].[{1}] " + act.Expression.Substring(0, 1)) + (rightConditionResult == null ? act.RightValue.Replace(rightValue.AliasKey, "[" + rightValue.TableName + "].[" + rightValue.FieldName + "]") : act.RightValue.Replace(rightConditionResult.Value.NameCode, "[" + tmpTradeExt + "].[" + rightConditionResult.Value.NameCode + "]")) + (act.OffsetExpression == null ? "" : act.OffsetExpression) + act.OffsetValue;
                    }
                    actionSql.AppendLine(string.Format(tmpSql1, leftValue.TableName, leftValue.FieldName));

                    actionSql.AppendLine(string.Format("Where [{0}].MemberID in (select MemberID from " + tmpMember + ")", leftValue.TableName));


                }
            }

            actionSql.AppendLine("drop table " + tmpMember);

            return actionSql.ToString();
        }


        private static ConditionResult? getConditionResult(List<ConditionResult> conditionResultList, string matchStr)
        {
            ConditionResult conditionResult;
            foreach (var fl in conditionResultList)
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
                    conditionResult = fl;
                    return conditionResult;
                }
            }
            return null;
        }

        //根据规则中的条件过滤交易
        private static string filterTradeDetailByCondition(Ral ral, TM_Loy_Rule rule, string sqlGetMember, string sqlGetTrade, string sqlGetTradeDetail, DateTime curDatetime, List<TD_SYS_FieldAlias> listAlias, string memberSourceTable)
        {
            //生成条件语句
            string where = "where ";
            if (ral != null)
            {
                bool f1 = true;
                List<string> tmp = new List<string>();
                int j = 0;
                foreach (var sr in ral.Rfl)
                {
                    if (!f1) where += ral.r;
                    f1 = false;
                    where += " ( ";
                    bool f2 = true;
                    string myWhere = "";
                    var newsrfl = SortRfl(sr.srfl, listAlias);
                    foreach (var e in newsrfl)
                    {
                        var lv = EngineHelper.GetAlias(listAlias, e.l);
                        tmp.Add(string.Format("[{0}{1}{2}]", lv.AliasType, lv.AliasKey, lv.AliasSubKey == null ? "" : lv.AliasSubKey));
                        //交易子表补上交易头
                        if (lv.AliasType == "MemberTradeDetail") tmp.Add(string.Format("[MemberTrade{0}]", lv.AliasKey));
                        if (f2)
                        {
                            f2 = false;
                            myWhere = Utility.EngineHelper.ConvertExpressionBaseByMember(listAlias, e.l, e.e, e.r, curDatetime, "TM_Mem_Master", myWhere, "", j);
                        }
                        else
                        {
                            myWhere = Utility.EngineHelper.ConvertExpressionBaseByMember(listAlias, e.l, e.e, e.r, curDatetime, "TM_Mem_Master", myWhere, sr.r, j);
                        }
                        j++;
                    }
                    where += myWhere + " ) ";
                    j++;
                }
                foreach (var t in tmp)
                {
                    where = where.Replace(t, "");
                }
            }
            else
            {
                where += " 1 = 1 ";
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select TM_Mem_TradeDetail.TradeDetailID ");
            sb.AppendLine("from TM_Mem_TradeDetail inner join TM_Mem_Trade on TM_Mem_TradeDetail.TradeID = TM_Mem_Trade.TradeID");
            sb.AppendLine("inner join ({0}) {1} on TM_Mem_Trade.MemberID = {1}.MemberID");
            sb.AppendLine("inner join TM_Mem_Master on TM_Mem_Master.MemberID = {1}.MemberID");
            sb.AppendLine("inner join TM_Mem_Ext on TM_Mem_Trade.MemberID = TM_Mem_Ext.MemberID");
            sb.AppendLine("inner join TM_Loy_MemExt on TM_Mem_Trade.MemberID = TM_Loy_MemExt.MemberID");
            sb.AppendLine("{2} and TM_Mem_TradeDetail.TradeID in ({3}) and TM_Mem_TradeDetail.TradeDetailID in ({4}) ");

            return string.Format(sb.ToString(), sqlGetMember, memberSourceTable, where, sqlGetTrade, sqlGetTradeDetail);
        }

        //根据规则中的条件过滤交易
        private static string filterTradeByCondition(Ral ral, TM_Loy_Rule rule, string sqlGetMember, string sqlGetTrade, DateTime curDatetime, List<TD_SYS_FieldAlias> listAlias, string memberSourceTable)
        {
            //生成条件语句
            string where = "where ";
            if (ral != null)
            {
                bool f1 = true;
                List<string> tmp = new List<string>();
                int j = 0;
                foreach (var sr in ral.Rfl)
                {
                    if (!f1) where += ral.r;
                    f1 = false;
                    where += " ( ";
                    bool f2 = true;
                    string myWhere = "";
                    var newsrfl = SortRfl(sr.srfl, listAlias);
                    foreach (var e in newsrfl)
                    {
                        var lv = EngineHelper.GetAlias(listAlias, e.l);
                        tmp.Add(string.Format("[{0}{1}{2}]", lv.AliasType, lv.AliasKey, lv.AliasSubKey == null ? "" : lv.AliasSubKey));
                        //交易子表补上交易头
                        if (lv.AliasType == "MemberTradeDetail") tmp.Add(string.Format("[MemberTrade{0}]", lv.AliasKey));
                        if (f2)
                        {
                            f2 = false;
                            myWhere = Utility.EngineHelper.ConvertExpressionBaseByMember(listAlias, e.l, e.e, e.r, curDatetime, "TM_Mem_Master", myWhere, "", true, j);
                        }
                        else
                        {
                            myWhere = Utility.EngineHelper.ConvertExpressionBaseByMember(listAlias, e.l, e.e, e.r, curDatetime, "TM_Mem_Master", myWhere, sr.r, true, j);
                        }
                        j++;
                    }
                    where += myWhere;
                    j++;
                }
                foreach (var t in tmp)
                {
                    where = where.Replace(t, "");
                }
            }
            else
            {
                where += " 1 = 1 ";
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("(select TM_Mem_Trade.TradeID ");
            sb.AppendLine("from TM_Mem_Trade");
            sb.AppendLine("inner join ({0}) {1} on TM_Mem_Trade.MemberID = {1}.MemberID");
            sb.AppendLine("inner join TM_Mem_Master on TM_Mem_Master.MemberID = {1}.MemberID");
            sb.AppendLine("inner join TM_Mem_Ext on TM_Mem_Trade.MemberID = TM_Mem_Ext.MemberID");
            sb.AppendLine("inner join TM_Loy_MemExt on TM_Mem_Trade.MemberID = TM_Loy_MemExt.MemberID");
            sb.AppendLine("{2} and TM_Mem_Trade.TradeID in ({3}) ) and TM_Mem_Trade.TradeID in ({3}) ))");

            return string.Format(sb.ToString(), sqlGetMember, memberSourceTable, where, sqlGetTrade);
        }

        //按照从交易明细、交易主表、会员子表进行排序
        public static List<Exp> SortRfl(List<Exp> srfl, List<TD_SYS_FieldAlias> list)
        {
            List<Exp> tmp = new List<Exp>();
            var query = from rf in srfl
                        join l in list on Utility.EngineHelper.GetAlias(list, rf.l) equals l
                        select new
                        {
                            rf.l,
                            rf.e,
                            rf.r,
                            l.AliasType
                        };

            tmp.AddRange(query.Where(o => o.AliasType == "MemberTradeDetail").Select(t => new Exp()
            {
                l = t.l,
                e = t.e,
                r = t.r
            }).ToList());
            tmp.AddRange(query.Where(o => o.AliasType == "MemberTrade").Select(t => new Exp()
            {
                l = t.l,
                e = t.e,
                r = t.r
            }).ToList());
            tmp.AddRange(query.Where(o => o.AliasType == "MemberSubExt").Select(t => new Exp()
            {
                l = t.l,
                e = t.e,
                r = t.r
            }).ToList());
            tmp.AddRange(query.Where(o => o.AliasType != "MemberSubExt" && o.AliasType != "MemberTrade" && o.AliasType != "MemberTradeDetail").Select(t => new Exp()
            {
                l = t.l,
                e = t.e,
                r = t.r
            }).ToList());

            return tmp;
        }

        //根据规则中的条件过滤出要执行的会员集合
        private static string filterMembersByCondition(Ral ral, TM_Loy_Rule rule, string sqlGetMember, DateTime curDatetime, List<TD_SYS_FieldAlias> listAlias, string memberSourceTable)
        {
            //生成条件语句
            string where = "where ";
            if (ral != null)
            {
                bool f1 = true;
                List<string> tmp = new List<string>();
                int j = 0;
                foreach (var sr in ral.Rfl)
                {
                    if (!f1) where += ral.r;
                    f1 = false;
                    where += " ( ";
                    bool f2 = true;
                    string myWhere = "";
                    var newsrfl = SortRfl(sr.srfl, listAlias);
                    foreach (var e in newsrfl)
                    {
                        var lv = EngineHelper.GetAlias(listAlias, e.l);
                        tmp.Add(string.Format("[{0}{1}{2}]", lv.AliasType, lv.AliasKey, lv.AliasSubKey == null ? "" : lv.AliasSubKey));
                        //交易子表补上交易头
                        if (lv.AliasType == "MemberTradeDetail") tmp.Add(string.Format("[MemberTrade{0}]", lv.AliasKey));
                        if (f2)
                        {
                            f2 = false;
                            myWhere = Utility.EngineHelper.ConvertExpressionBaseByMember(listAlias, e.l, e.e, e.r, curDatetime, memberSourceTable, myWhere, "", j);
                        }
                        else
                        {
                            myWhere = Utility.EngineHelper.ConvertExpressionBaseByMember(listAlias, e.l, e.e, e.r, curDatetime, memberSourceTable, myWhere, sr.r, j);
                        }
                        j++;
                    }
                    where += myWhere + " ) ";
                    j++;
                }
                foreach (var t in tmp)
                {
                    where = where.Replace(t, "");
                }
            }
            else
            {
                where += " 1 = 1 ";
            }
            //return string.Format("(select {1}.MemberID from ({0}) {1}  left join TM_Loy_MemExt on {1}.MemberID = TM_Loy_MemExt.MemberID left join TM_Mem_Ext on {1}.MemberID = TM_Mem_Ext.MemberID {2})", sqlGetMember, memberSourceTable, where);
            //By Ryan 20160115  
            return string.Format("(select {1}.MemberID from ({0}) {1} inner join TM_Mem_Master on {1}.MemberID = TM_Mem_Master.MemberID left join TM_Loy_MemExt on {1}.MemberID = TM_Loy_MemExt.MemberID left join TM_Mem_Ext on {1}.MemberID = TM_Mem_Ext.MemberID {2})", sqlGetMember, memberSourceTable, where);
        }

        //计算维度
        private static void computeAfterRuleDimension(CRMEntities db, string sqlForGetMemberList, DateTime curDatetime, int ruleID)
        {
            //拼接当前时间字符串
            string curTime = "'" + curDatetime.ToString("yyyyMMdd") + "'";
            //计算维度
            var query = from d in db.TD_SYS_FieldAlias
                        join e in db.TL_CRM_AliasAfterRule on d.AliasID equals e.AliasID
                        orderby d.ComputeSort
                        where d.ComputeScript != null && e.RuleID == ruleID
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

        private static void computeDimensionByRule(string sqlForGetMemberList, DateTime curDatetime, CRMEntities db, TM_Loy_Rule rule, List<TD_SYS_FieldAlias> listAlias)
        {
            List<string> ds = new List<string>();
            foreach (var l in listAlias)
            {
                //全字匹配
                var index = rule.Condition.IndexOf(l.FieldAlias);
                if (index > -1)
                {
                    if (index > 1)
                    {
                        if (rule.Condition.Substring(index - 1, 1) != "\"") continue;
                    }
                    if (index + l.FieldAlias.Length < rule.Condition.Length)
                    {
                        if (rule.Condition.Substring(index + l.FieldAlias.Length, 1) != " " && rule.Condition.Substring(index + l.FieldAlias.Length, 1) != "\"") continue;
                    }
                    else
                    {
                        continue;
                    }
                    if (!ds.Contains(l.FieldAlias)) ds.Add(l.FieldAlias);
                }
            }

            foreach (var l in listAlias)
            {
                //全字匹配
                var index = rule.Action.IndexOf(l.FieldAlias);
                if (index > -1)
                {
                    if (index > 1)
                    {
                        if (rule.Action.Substring(index - 1, 1) != "\"") continue;
                    }
                    if (index + l.FieldAlias.Length < rule.Action.Length)
                    {
                        if (rule.Action.Substring(index + l.FieldAlias.Length, 1) != " " && rule.Action.Substring(index + l.FieldAlias.Length, 1) != "\"") continue;
                    }
                    else
                    {
                        continue;
                    }
                    if (!ds.Contains(l.FieldAlias)) ds.Add(l.FieldAlias);
                }
            }

            //拼接当前时间字符串
            string curTime = "'" + curDatetime.ToString("yyyyMMdd") + "'";

            //计算维度
            var query = from d in listAlias
                        where d.IsFilterByLoyRule && ds.Contains(d.FieldAlias) && d.ComputeScript != null
                        select new
                        {
                            d.ComputeScript,
                            d.FieldAlias,
                            d.FieldName
                        };

            var dcl = query.ToList();
            foreach (var dc in dcl)
            {
                string rrr = dc.ComputeScript.Replace("[Attr]", dc.FieldName).Replace("[MemberList]", "(" + sqlForGetMemberList + ")").Replace("[DatetimeNow]", curTime).Replace("[Switch]", sqlForGetMemberList == "" ? "1" : "0");

                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandTimeout = 360;
                cmd.CommandText = rrr;
                cmd.CommandType = System.Data.CommandType.Text;
                if (db.Database.Connection.State == System.Data.ConnectionState.Closed) db.Database.Connection.Open();
                cmd.ExecuteNonQuery();

            }
        }

        //获取可执行的规则
        private static List<TM_Loy_Rule> getAvaiRule(DateTime curDatetime, CRMEntities db)
        {
            string sql = "select * from TM_Loy_Rule where Enable = 1 and StartDate <= '{0}' and ( EndDate is null or EndDate >= '{1}') order by RunIndex";
            var ruleList = db.Database.SqlQuery<TM_Loy_Rule>(string.Format(sql, curDatetime.ToString("yyyy-MM-dd HH:mm:ss.fff"), curDatetime.ToString("yyyy-MM-dd"))).ToList();
            var avaiRuleList = new List<TM_Loy_Rule>();
            foreach (var rule in ruleList)
            {
                //判断是否到了执行时间
                if (rule.RuleRunType == "2" && !isTiming(rule, curDatetime)) continue;
                var avaiRule = rule;
                avaiRuleList.Add(avaiRule);
            }
            return avaiRuleList;
        }

        //判断执行时间是否到了
        public static bool isTiming(TM_Loy_Rule rule, DateTime curDatetime)
        {
            //Mapping日程
            LoyaltySchedule s = Utility.JsonHelper.Deserialize<LoyaltySchedule>(rule.Schedule);

            string curDate = curDatetime.ToString("yyyy-MM-dd");
            string curTime = curDatetime.ToString("HH:mm:ss.fff");
            string space = " ";
            DateTime planExcuteTime = new DateTime(1900, 1, 1);
            switch (s.SubType)
            {
                case "day":
                    planExcuteTime = DateTime.Parse(curDate + space + EngineHelper.ConvertTimeString(s.Time));
                    break;
                case "week":
                    var nowDayOfWeek = EngineHelper.GetDayOfWeek(curDatetime);
                    var diff = int.Parse(s.Date) - nowDayOfWeek;
                    planExcuteTime = DateTime.Parse(curDatetime.AddDays(diff).ToString("yyyy-MM-dd") + space + EngineHelper.ConvertTimeString(s.Time));
                    break;
                case "month":
                    switch (s.Date)
                    {
                        case "lst":
                            planExcuteTime = DateTime.Parse(new DateTime(curDatetime.Year, curDatetime.Month, EngineHelper.LastDayOfMonth(curDatetime.Year, curDatetime.Month)).ToString("yyyy-MM-dd") + space + EngineHelper.ConvertTimeString(s.Time));
                            break;
                        default:
                            planExcuteTime = DateTime.Parse(new DateTime(curDatetime.Year, curDatetime.Month, int.Parse(s.Date)).ToString("yyyy-MM-dd") + space + EngineHelper.ConvertTimeString(s.Time));
                            break;
                    }
                    break;
                case "year":
                    switch (s.Date)
                    {
                        case "lst":
                            planExcuteTime = DateTime.Parse(curDatetime.Year.ToString() + "-12-31" + space + EngineHelper.ConvertTimeString(s.Time));
                            break;
                        case "1st":
                            planExcuteTime = DateTime.Parse(curDatetime.Year.ToString() + "-01-01" + space + EngineHelper.ConvertTimeString(s.Time));
                            break;
                    }
                    break;
            }
            //未执行过或上次执行时间小于计划执行时间
            if (rule.LastExcuteTime == null || rule.LastExcuteTime.Value < planExcuteTime)
            {
                //当前日期和计划执行日期不在同一天的
                if (curDate != planExcuteTime.ToString("yyyy-MM-dd")) return false;
                //当前时间大于等于计划执行时间
                if (planExcuteTime <= curDatetime) return true;
                else return false;
            }
            return false;

        }

        public void Dispose()
        {
            if (IsMyDB) Ext.db.Dispose();
        }
    }

    public struct ExtraLoyalty
    {
        public string InterfaceName { get; set; }
        public CRMEntities db { get; set; }
        public List<TM_Mem_Trade> TradeList { get; set; }
        public string SearchTradeSQL { get; set; }
        public List<TM_Mem_TradeDetail> TradeDetaiList { get; set; }
        public string SearchTradeDetailSQL { get; set; }
        public List<TM_Loy_Rule> LoyaltyRuleList { get; set; }
        public string RuleType { get; set; }
        public int DataGroupID { get; set; }
    }

}
