using Arvato.CRM.EF;
using Arvato.CRM.Model;
using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.MemberSubdivisionLogic
{
    public static class MemberSubdivision
    {

        public static Result Run(DateTime curDatetime)
        {
#if DEBUG
            string lastScript = "";
#endif
            var tmpTime = curDatetime;
            //查找可执行的会员细分
            using (CRMEntities db = new CRMEntities())
            {
                db.BeginTransaction(0, 40, 0);
                try
                {
                    var msList = db.TM_Mem_Subdivision.Where(o => o.Enable && o.SubDevDataType == "1");
                    foreach (var ms in msList.ToList())
                    {
                        var schedule = Utility.JsonHelper.Deserialize<Schedule>(ms.Schedule);
                        var curSubdivision = db.TM_Mem_SubdivisionInstance.Where(o => o.SubdivisionID == ms.SubdivisionID).OrderByDescending(o => o.LastComputerDate).FirstOrDefault();
                        var lct = curSubdivision == null ? new DateTime() : curSubdivision.LastComputerDate;//上次计算时间
                        var runFlag = false;
                        if (schedule.type == "immediately" && curSubdivision == null)//立即执行
                        {
                            runFlag = true;
                        }
                        else if (schedule.type == "appointed" && curSubdivision == null && curDatetime >= DateTime.Parse(schedule.ap))//指定时间执行
                        {
                            runFlag = true;
                        }
                        else if (schedule.type == "cycle")//周期执行
                        {
                            DateTime planDate = DateTime.Parse(curDatetime.ToString("yyyy-MM-dd") + " " + Utility.EngineHelper.ConvertTimeString(schedule.ap));
                            switch (schedule.cycle)
                            {
                                case "daily":
                                    if (lct < planDate && planDate <= curDatetime) runFlag = true;
                                    break;
                                case "weekly":
                                    if (Utility.EngineHelper.GetDayOfWeek(curDatetime).ToString() == schedule.d)
                                    {
                                        if (lct < planDate && planDate <= curDatetime) runFlag = true;
                                    }
                                    break;
                                case "monthly":
                                    switch (schedule.d)
                                    {
                                        case "1st":
                                            if (curDatetime.Day == 1)
                                            {
                                                if (lct < planDate && planDate <= curDatetime) runFlag = true;
                                            }
                                            break;
                                        case "last":
                                            if (Utility.EngineHelper.LastDayOfMonth(curDatetime.Year, curDatetime.Month) == curDatetime.Day)
                                            {
                                                if (lct < planDate && planDate <= curDatetime) runFlag = true;
                                            }
                                            break;
                                        default:
                                            if (curDatetime.Day.ToString() == schedule.d)
                                            {
                                                if (lct < planDate && planDate <= curDatetime) runFlag = true;
                                            }
                                            break;
                                    }
                                    break;
                            }
                        }

                        //执行细分
                        if (runFlag)
                        {
                            //创建动态表
                            var gid = Guid.NewGuid();
                            var si = new TM_Mem_SubdivisionInstance
                            {
                                SubdivisionInstanceID = gid,
                                SubdivisionID = ms.SubdivisionID,
                                LastComputerDate = DateTime.Now,
                                AddedDate = DateTime.Now,
                                TableName = string.Format("TM_Mem_SubdivideResult_{0}", gid)
                            };

                            var sb = new StringBuilder();

                            //反序列化条件
                            Ral r = Utility.JsonHelper.Deserialize<Ral>(ms.Condition);

                            //取出所有别名字段
                            List<TD_SYS_FieldAlias> l = db.TD_SYS_FieldAlias.ToList();

                            //执行所用到动态维度的脚本
                            string dropDynamicSql = createDynamicDimension(curDatetime, db, r, l);
                            computeDynamicDimension(curDatetime, db, r, l);

                            //拼接执行会员细分
                            sb.AppendLine(string.Format("CREATE TABLE [TM_Mem_SubdivideResult_{0}](", si.SubdivisionInstanceID));
                            sb.AppendLine("[MSRID] [bigint] IDENTITY(1,1) NOT NULL,");
                            sb.AppendLine("[MemberID] [char](32) NOT NULL,");
                            sb.AppendLine("[ParentMemberID] [char](32) NULL,");
                            sb.AppendLine("[DataGroupID] [int] NOT NULL,");
                            sb.AppendLine("[MemberGrade] [smallint] NOT NULL,");
                            sb.AppendLine(string.Format("CONSTRAINT [PK_TM_Mem_SubdivideResult_{0}] PRIMARY KEY CLUSTERED ", si.SubdivisionInstanceID));
                            sb.AppendLine("(");
                            sb.AppendLine("[MSRID] ASC");
                            sb.AppendLine(")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                            sb.AppendLine(") ON [PRIMARY]");
                           
                            db.Database.ExecuteSqlCommand(sb.ToString());
                            
                            //生成条件语句
                            string where = " where TM_Mem_Master.DataGroupID = " + ms.DataGroupID.ToString() + " ";
                            if (r != null)
                            {
                                where += " and ";
                                bool f1 = true;
                                List<string> tmp = new List<string>();
                                int j = 0;
                                foreach (var sr in r.Rfl)
                                {
                                    if (!f1) where += r.r;
                                    f1 = false;
                                    where += " ( ";
                                    bool f2 = true;
                                    string myWhere = "";
                                    var newsrfl = SortRfl(sr.srfl, l);
                                    foreach (var e in newsrfl)
                                    {
                                        var lv = EngineHelper.GetAlias(l, e.l);
                                        tmp.Add(string.Format("[{0}{1}{2}]", lv.AliasType, lv.AliasKey, lv.AliasSubKey == null ? "" : lv.AliasSubKey));
                                        //交易子表补上交易头
                                        if (lv.AliasType == "MemberTradeDetail") tmp.Add(string.Format("[MemberTrade{0}]", lv.AliasKey));
                                        if (f2)
                                        {
                                            f2 = false;
                                            myWhere = Utility.EngineHelper.ConvertExpressionBaseByMember(l, e.l, e.e, e.r, curDatetime, "TM_Mem_Master", myWhere, "", j);
                                        }
                                        else
                                        {
                                            myWhere = Utility.EngineHelper.ConvertExpressionBaseByMember(l, e.l, e.e, e.r, curDatetime, "TM_Mem_Master", myWhere, sr.r, j);
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

                            //抽取数据插入动态表
                            sb.Clear();
                            sb.AppendLine("insert into " + string.Format("[TM_Mem_SubdivideResult_{0}]", gid) + " ([MemberID],[ParentMemberID],[DataGroupID],[MemberGrade] )");
                            sb.AppendLine("select TM_Mem_Master.MemberID,TM_Mem_Master.ParentMemberID,TM_Mem_Master.DataGroupID,TM_Mem_Master.MemberGrade from TM_Mem_Master with(nolock)");
                            sb.AppendLine(" join TM_Mem_Ext with(nolock) on TM_Mem_Master.MemberID = TM_Mem_Ext.MemberID");
                            sb.AppendLine(" left join TM_Loy_MemExt with(nolock) on TM_Mem_Master.MemberID = TM_Loy_MemExt.MemberID");
                            sb.AppendLine(" left join TE_Mem_DynamicDimension with(nolock) on TM_Mem_Master.MemberID = TE_Mem_DynamicDimension.MemberID");

                            sb.AppendLine(where);

                            sb.AppendLine(dropDynamicSql);

                            string getCount = "select Count(*) from " + string.Format("[TM_Mem_SubdivideResult_{0}]", gid);
                            

#if DEBUG
                            lastScript = sb.ToString();
#endif

                            var cmd = db.Database.Connection.CreateCommand();
                            cmd.CommandTimeout = 240;
                            cmd.CommandText = sb.ToString();
                            cmd.CommandType = System.Data.CommandType.Text;
                            if (db.Database.Connection.State == System.Data.ConnectionState.Closed) db.Database.Connection.Open();
                            Log4netHelper.WriteInfoLog("SQL:" + sb.ToString());
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = getCount;

                            si.MemberCount = (int)cmd.ExecuteScalar();
                            si.ComputerTime = (int)(DateTime.Now - tmpTime).TotalSeconds;

                            tmpTime = DateTime.Now;
                            db.Entry(si).State = System.Data.EntityState.Added;

                            ms.CurSubdivisionInstanceID = gid;
                            db.Entry(ms).State = System.Data.EntityState.Modified;
                        }

                    }
                    db.SaveChanges();
                    db.Commit();
                }
                catch (Exception e)
                {
                    return new Result(false, e.Message);
                }
            }

            return new Result(true);

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

        //创建动态维度
        private static string createDynamicDimension(DateTime curDatetime, CRMEntities db, Ral r, List<TD_SYS_FieldAlias> listAlias)
        {
            StringBuilder dropDynamicSql = new StringBuilder();
            string curTime = "'" + curDatetime.ToString("yyyyMMdd") + "'";
            int j = 0;
            //循环每个条件组
            foreach (var rl in r.Rfl)
            {
                //循环每个条件组的条件
                var newsrfl = SortRfl(rl.srfl, listAlias);
                foreach (var rlr in newsrfl)
                {
                    //找到别名字段
                    var dc = Utility.EngineHelper.GetAlias(listAlias, rlr.l);
                    //判断是否动态字段
                    if (dc.ParameterCount != null && dc.ComputeScript != null && dc.IsDynamicAlias ==true )//&& dc.ControlType == "dynamic") --delete by ryan 20151211
                    {
                        dropDynamicSql.AppendLine("if exists(SELECT 1 FROM SYSCOLUMNS WHERE ID=OBJECT_ID('[TE_Mem_DynamicDimension]') and Name='" + dc.FieldName + j.ToString() + "')");
                        dropDynamicSql.AppendLine("alter table [TE_Mem_DynamicDimension] drop column [" + dc.FieldName + j.ToString() + "]");
                        StringBuilder rrr = new StringBuilder();
                        //Error 创建的字段类型没有根据配置类型创建
                        rrr.AppendLine("insert into TE_Mem_DynamicDimension (MemberID) select t1.MemberID from TM_Mem_Master t1 left join TE_Mem_DynamicDimension t2 on t1.MemberID=t2.MemberID where t2.MemberID is null");//修改TE表插入数据时间点
                        rrr.AppendLine("if not exists(SELECT 1 FROM SYSCOLUMNS WHERE ID=OBJECT_ID('[TE_Mem_DynamicDimension]') and Name='" + dc.FieldName + j.ToString() + "')");
                        rrr.AppendLine("alter table [TE_Mem_DynamicDimension] add [" + dc.FieldName + j.ToString() + "] " + EngineHelper.GetDBFieldDefine(dc.FieldType));
                        var cmd = db.Database.Connection.CreateCommand();
                        cmd.CommandTimeout = 360;
                        cmd.CommandText = rrr.ToString();
                        cmd.CommandType = System.Data.CommandType.Text;
                        if (db.Database.Connection.State == System.Data.ConnectionState.Closed) db.Database.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    j++;
                }
                j++;
            }
            return dropDynamicSql.ToString();
        }


        //计算动态维度
        private static void computeDynamicDimension(DateTime curDatetime, CRMEntities db, Ral r, List<TD_SYS_FieldAlias> listAlias)
        {
            string curTime = "'" + curDatetime.ToString("yyyyMMdd") + "'";
            int j = 0;
            //循环每个条件组
            foreach (var rl in r.Rfl)
            {
                //循环每个条件组的条件
                var newsrfl = SortRfl(rl.srfl, listAlias);
                foreach (var rlr in newsrfl)
                {
                    //找到别名字段
                    var dc = Utility.EngineHelper.GetAlias(listAlias, rlr.l);
                    //判断是否动态字段
                    if (dc.ParameterCount != null && dc.ComputeScript != null && dc.IsDynamicAlias ==true)
                    {
                        StringBuilder rrr = new StringBuilder();
                        rrr.AppendLine(dc.ComputeScript.Replace("[Attr]", dc.FieldName + j.ToString()).Replace("[MemberList]", "(Select MemberID From TM_Mem_Master)").Replace("[DatetimeNow]", curTime).Replace("[Switch]", "0"));

                        var pl = rlr.r.Split('|');
                        for (int i = 1; i < pl.Length; i++)
                        {
                            rrr = rrr.Replace("[Parameter" + i.ToString() + "]", "'" + pl[i] + "'");
                        }

                        var cmd = db.Database.Connection.CreateCommand();
                        cmd.CommandTimeout = 1800;
                        cmd.CommandText = rrr.ToString();
                        cmd.CommandType = System.Data.CommandType.Text;
                        if (db.Database.Connection.State == System.Data.ConnectionState.Closed) db.Database.Connection.Open();
                        Log4netHelper.WriteInfoLog(dc.FieldDesc+"SQL:" + rrr.ToString());
                        cmd.ExecuteNonQuery();
                    }
                    j++;
                }
                j++;
            }
        }


        //计算维度
        public static Result ComputeAllDimension(DateTime lastComputeTime, DateTime planComputeTime, DateTime curDatetime, int timeout_min)
        {
            if (lastComputeTime < planComputeTime && planComputeTime <= curDatetime)
            {
                //计算所有会员的维度
                try
                {
                    using (CRMEntities db = new CRMEntities())
                    {
                        computeDimension(db, "(select  MemberID from TM_Mem_Master) ", curDatetime);
                    }
                }
                catch (Exception e)
                {
                    return new Result(false, e.Message);
                }
                return new Result(true);

            }
            return new Result(true, "it's not time ");
        }

        public static void ComputeDimension(List<string> memberIDs, DateTime curDatetime, List<int> dimensions, int timeout_min = 240)
        {
            string sqlMemberIDs = "select MemberID from TM_Mem_Master where MemberID in('";
            sqlMemberIDs += string.Join("','", memberIDs);
            sqlMemberIDs += "')";
            ComputeDimension(sqlMemberIDs, curDatetime, dimensions, timeout_min);
        }

        public static void ComputeDimension(string sqlForGetMemberList, DateTime curDatetime, List<int> dimensions, int timeout_min = 240)
        {
            using (CRMEntities db = new CRMEntities())
            {
                //拼接当前时间字符串
                string curTime = "'" + curDatetime.ToString("yyyyMMdd") + "'";
                //计算维度
                var query = from d in db.TD_SYS_FieldAlias
                            orderby d.ComputeSort
                            where d.ComputeScript != null &&  dimensions.Contains(d.AliasID) && d.IsDynamicAlias != true
                            select new
                            {
                                d.ComputeScript,
                                d.FieldAlias,
                                d.FieldName
                            };
                var dcl = query.ToList();
                //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                foreach (var dc in dcl)
                {
                    try
                    {
                        //stopwatch.Start();
                        //Log4netHelper.WriteInfoLog("执行开始 :" + dc.FieldAlias + "开始：" + stopwatch.ElapsedMilliseconds / 1000);
                        string rrr = dc.ComputeScript.Replace("[Attr]", dc.FieldName).Replace("[MemberList]", sqlForGetMemberList).Replace("[DatetimeNow]", curTime).Replace("[Switch]", sqlForGetMemberList == "" ? "1" : "0");

                        var cmd = db.Database.Connection.CreateCommand();
                        cmd.CommandTimeout = timeout_min;
                        cmd.CommandText = rrr;
                        cmd.CommandType = System.Data.CommandType.Text;
                        if (db.Database.Connection.State == System.Data.ConnectionState.Closed) db.Database.Connection.Open();
                        cmd.ExecuteNonQuery();
                        //Log4netHelper.WriteInfoLog("执行结束:" + dc.FieldAlias + "结束：" + stopwatch.ElapsedMilliseconds / 1000);
                        //stopwatch.Stop();

                    }
                    catch (Exception ex)
                    {
                        Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
                    }


                }
            }
        }

        private static void computeDimension(CRMEntities db, string sqlForGetMemberList, DateTime curDatetime, int timeout_min = 240)
        {
            //拼接当前时间字符串
            string curTime = "'" + curDatetime.ToString("yyyyMMdd") + "'";
            //计算维度
            var query = from d in db.TD_SYS_FieldAlias
                        orderby d.ComputeSort
                        where d.ComputeScript != null &&  d.IsDynamicAlias != true
                        select new
                        {
                            d.ComputeScript,
                            d.FieldAlias,
                            d.FieldName
                        };
            var dcl = query.ToList();
            //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            foreach (var dc in dcl)
            {
                try
                {
                    //stopwatch.Start();
                    //Console.WriteLine("执行开始 :" + dc.FieldAlias + "开始：" + stopwatch.ElapsedMilliseconds / 1000);
                    string rrr = dc.ComputeScript.Replace("[Attr]", dc.FieldName).Replace("[MemberList]", sqlForGetMemberList).Replace("[DatetimeNow]", curTime).Replace("[Switch]", sqlForGetMemberList == "" ? "1" : "0");

                    var cmd = db.Database.Connection.CreateCommand();
                    cmd.CommandTimeout = timeout_min;
                    cmd.CommandText = rrr;
                    cmd.CommandType = System.Data.CommandType.Text;
                    if (db.Database.Connection.State == System.Data.ConnectionState.Closed) db.Database.Connection.Open();
                    cmd.ExecuteNonQuery();
                    //Console.WriteLine("执行结束:" + dc.FieldAlias + "结束：" + stopwatch.ElapsedMilliseconds / 1000);
                    //stopwatch.Stop();

                }
                catch (Exception ex)
                {
                    Log4netHelper.WriteErrorLog(ex.Message + ex.InnerException);
                }


            }

        }

    }
}
