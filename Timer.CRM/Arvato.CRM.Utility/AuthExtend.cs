using Arvato.CRM.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Utility
{
    public static class AuthExtend
    {
        private static Expression<Func<T, bool>> getExpression<T, T1, T2>(Expression<Func<T, bool>> pred, List<T1> limitList, List<T2> limitAliasList)
        {
            foreach (dynamic t1 in limitList)
            {
                foreach (dynamic t2 in limitAliasList)
                {
                    if (typeof(T).GetProperty((string)t2.FieldAlias) != null && (string)t2.DataLimitType == (string)t1.RangeType)
                    {
                        var parameter = Expression.Parameter(typeof(T), "obj");
                        var leftValue = Expression.Property(parameter, typeof(T).GetProperty((string)t2.FieldAlias));
                        var rightValue = Expression.Constant((string)t1.RangeValue);
                        var filter = Expression.Equal(leftValue, rightValue);
                        var lambdaExpression = Expression.Lambda<Func<T, bool>>(filter, parameter);
                        pred = pred.Or(lambdaExpression);
                    }
                }

            }
            return pred;
        }

        private static Expression<Func<T, bool>> getExpression<T>(Expression<Func<T, bool>> pred, int dataGroupId)
        {
            if (typeof(T).GetProperty("DataGroupID") != null)
            {
                var parameter = Expression.Parameter(typeof(T), "obj");
                var leftValue = Expression.Property(parameter, typeof(T).GetProperty("DataGroupID"));
                var rightValue = Expression.Constant(dataGroupId);
                var filter = Expression.Equal(leftValue, rightValue);
                var lambdaExpression = Expression.Lambda<Func<T, bool>>(filter, parameter);
                pred = pred.ToString() != PredicateBuilder.False<T>().ToString() ? pred.And(lambdaExpression) : lambdaExpression;
            }
            return pred;
        }

        public static IQueryable<T> FilterDataByAuth<T>(this IQueryable<T> query, List<int> roleid, int pageid, int dataGroupId, bool isFilterDataGroupID = true)
        {
            var pred = PredicateBuilder.False<T>();
            //if (query.Count() == 0) return query.Where(pred);
            List<string> rlist = roleid.Select(r => r.ToString()).ToList();
            using (var db = new CRMEntities())
            {
                var limitAliasList = (from f in db.TD_SYS_FieldAlias
                                      where f.DataLimitType != null
                                      select new
                                      {
                                          f.FieldAlias,
                                          f.DataLimitType
                                      }).ToList();
                //数据隔离部分的设置转化为表达式
                var queryLimit = from l in db.TM_AUTH_DataLimit.AsNoTracking()
                                 where l.HierarchyType == "role" && rlist.Contains(l.HierarchyValue) && (l.PageID == 9999 || l.PageID == pageid)
                                 select new
                                 {
                                     l.RangeType,
                                     l.RangeValue
                                 };
                foreach (var g in queryLimit.GroupBy(o => o.RangeType))
                {
                    var gList = g.Distinct().ToList();
                    pred = getExpression(pred, gList, limitAliasList);
                }

            }

            var pred1 = PredicateBuilder.False<T>();
            if (isFilterDataGroupID) pred1 = getExpression(pred1, dataGroupId);


            bool limit = false;
            if (PredicateBuilder.False<T>().ToString() != pred.ToString())
            {
                query = query.Where(pred);
                limit = true;
            }
            if (PredicateBuilder.False<T>().ToString() != pred1.ToString())
            {
                query = query.Where(pred1);
                limit = true;
            }
            if (limit) return query;
            else return query.Where(PredicateBuilder.False<T>());
        }
    }
}
