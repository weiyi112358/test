using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Arvato.CRM.EF
{
    partial class CRMEntities
    {
        protected TransactionScope scope;

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <returns>返回当前事务</returns>
        public TransactionScope BeginTransaction()
        {
            return scope = scope ?? new TransactionScope();
        }

        public TransactionScope BeginTransaction(int hours, int minutes, int seconds)
        {
            return scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(hours, minutes, seconds));
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public void Rollback()
        {
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            scope.Complete();
            scope.Dispose();
        }


        /// <summary>
        /// 添加视图记录
        /// </summary>
        /// <typeparam name="T1">视图实体类</typeparam>
        /// <typeparam name="T2">视图对应基础表类</typeparam>
        /// <param name="viewRow">视图实体类实例</param>
        /// <returns>视图对应基础表类实例</returns>
        public T2 AddViewRow<T1, T2>(T1 viewRow) where T2 : class,new()
        {
            T2 t2 = new T2();
            var listAlias = this.TD_SYS_FieldAlias.ToList();
            var listFixU = this.TD_SYS_FilterMapping.Where(o => o.Type == "Union").ToList();
            var listFixF = this.TD_SYS_FilterMapping.Where(o => o.Type == "Filter").ToList();
            string key = "";
            string subkey = "";
            string tablename = "";
            foreach (var la in listAlias)
            {
                if (typeof(T1).GetProperty(la.FieldAlias) != null)
                {
                    PropertyInfo property = typeof(T1).GetProperty(la.FieldAlias);
                    object o = property.GetValue(viewRow, null);
                    if (key == "")
                    {
                        key = la.AliasKey;
                        subkey = la.AliasSubKey;
                        tablename = la.TableName;
                    }
                    t2.GetType().GetProperty(la.FieldName).SetValue(t2, o, null);
                }
            }
            foreach (var lf in listFixU)
            {
                if (lf.AliasKey != null && lf.TableName == tablename && typeof(T1).GetProperty(lf.AliasKey) != null)
                {
                    PropertyInfo property = typeof(T1).GetProperty(lf.AliasKey);
                    object o = property.GetValue(viewRow, null);
                    t2.GetType().GetProperty(lf.AliasKey).SetValue(t2, o, null);
                }
            }

            foreach (var lf in listFixF)
            {
                if (lf.AliasKeyFilter != null && lf.TableName == tablename && typeof(T1).GetProperty(lf.AliasKeyFilter) != null)
                {
                    t2.GetType().GetProperty(lf.AliasKeyFilter).SetValue(t2, key, null);
                }
                if (lf.AliasSubKeyFilter != null && lf.TableName == tablename && typeof(T1).GetProperty(lf.AliasSubKeyFilter) != null)
                {
                    t2.GetType().GetProperty(lf.AliasSubKeyFilter).SetValue(t2, subkey, null);
                }
            }

            if (typeof(T1).GetProperty("DataGroupID") != null)
            {
                PropertyInfo property = typeof(T1).GetProperty("DataGroupID");
                object o = property.GetValue(viewRow, null);
                t2.GetType().GetProperty("DataGroupID").SetValue(t2, o, null);
            }

            var entry = this.Entry<T2>(t2);
            entry.State = EntityState.Added;
            return t2;
        }

        /// <summary>
        /// 删除视图记录
        /// </summary>
        /// <typeparam name="T1">视图实体类</typeparam>
        /// <typeparam name="T2">视图对应基础表类</typeparam>
        /// <param name="viewRow">视图实体类实例</param>
        /// <returns>视图对应基础表类实例</returns>
        public T2 DeleteViewRow<T1, T2>(T1 viewRow) where T2 : class,new()
        {
            T2 t2 = new T2();
            var listAlias = this.TD_SYS_FieldAlias.ToList();
            var listFix = this.TD_SYS_FilterMapping.Where(o => o.Type == "Union").ToList();
            string key = "";
            string subkey = "";
            string tablename = "";
            foreach (var la in listAlias)
            {
                if (typeof(T1).GetProperty(la.FieldAlias) != null)
                {
                    if (key == "")
                    {
                        key = la.AliasKey;
                        subkey = la.AliasSubKey;
                        tablename = la.TableName;
                        continue;
                    }
                }
            }
            foreach (var lf in listFix)
            {
                if (lf.AliasKey != null && lf.TableName == tablename && typeof(T1).GetProperty(lf.AliasKey) != null)
                {
                    PropertyInfo property = typeof(T1).GetProperty(lf.AliasKey);
                    object o = property.GetValue(viewRow, null);
                    t2.GetType().GetProperty(lf.AliasKey).SetValue(t2, o, null);
                }
            }

            var entry = this.Entry<T2>(t2);
            entry.State = EntityState.Deleted;
            return t2;
        }

        /// <summary>
        /// 更新视图记录
        /// </summary>
        /// <typeparam name="T1">视图实体类</typeparam>
        /// <typeparam name="T2">视图对应基础表类</typeparam>
        /// <param name="viewRow">视图实体类实例</param>
        /// <returns>视图对应基础表类实例</returns>
        public T2 UpdateViewRow<T1, T2>(T1 viewRow) where T2 : class,new()
        {
            T2 t2 = new T2();
            var listAlias = this.TD_SYS_FieldAlias.ToList();
            var listFixU = this.TD_SYS_FilterMapping.Where(o => o.Type == "Union").ToList();
            var listFixF = this.TD_SYS_FilterMapping.Where(o => o.Type == "Filter").ToList();
            string key = "";
            string subkey = "";
            string tablename = "";
            foreach (var la in listAlias)
            {
                if (typeof(T1).GetProperty(la.FieldAlias) != null)
                {
                    PropertyInfo property = typeof(T1).GetProperty(la.FieldAlias);
                    object o = property.GetValue(viewRow, null);
                    if (key == "")
                    {
                        key = la.AliasKey;
                        subkey = la.AliasSubKey;
                        tablename = la.TableName;
                    }
                    t2.GetType().GetProperty(la.FieldName).SetValue(t2, o, null);
                }
            }
            foreach (var lf in listFixU)
            {
                if (lf.AliasKey != null && lf.TableName == tablename && typeof(T1).GetProperty(lf.AliasKey) != null)
                {
                    PropertyInfo property = typeof(T1).GetProperty(lf.AliasKey);
                    object o = property.GetValue(viewRow, null);
                    t2.GetType().GetProperty(lf.AliasKey).SetValue(t2, o, null);
                }
            }

            foreach (var lf in listFixF)
            {
                if (lf.AliasKeyFilter != null && lf.TableName == tablename && typeof(T1).GetProperty(lf.AliasKeyFilter) != null)
                {
                    t2.GetType().GetProperty(lf.AliasKeyFilter).SetValue(t2, key, null);
                }
                if (lf.AliasSubKeyFilter != null && lf.TableName == tablename && typeof(T1).GetProperty(lf.AliasSubKeyFilter) != null)
                {
                    t2.GetType().GetProperty(lf.AliasSubKeyFilter).SetValue(t2, subkey, null);
                }
            }

            if (typeof(T1).GetProperty("DataGroupID") != null)
            {
                PropertyInfo property = typeof(T1).GetProperty("DataGroupID");
                object o = property.GetValue(viewRow, null);
                t2.GetType().GetProperty("DataGroupID").SetValue(t2, o, null);
            }

            var entry = this.Entry<T2>(t2);

            entry.State = EntityState.Modified;
            return t2;
        }

        /// <summary>
        /// 资源释放（重载）
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (scope != null) { scope.Dispose(); scope = null; }
            base.Dispose(disposing);
        }
    }
}
