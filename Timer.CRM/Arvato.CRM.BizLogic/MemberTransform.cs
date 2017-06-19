using Arvato.CRM.EF;
using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arvato.CRM.Utility.Datatables;
using System.Data.Entity.Infrastructure;

namespace Arvato.CRM.BizLogic
{
    public class MemberTransform
    {
        public static Result GetMemList(string Corp, string RegisterStoreCode, string dp)
        {
            using (CRMEntities db = new CRMEntities())
            {
                DatatablesParameter pageInfo = JsonHelper.Deserialize<DatatablesParameter>(dp);
                var query = from a in db.V_S_TM_Mem_Ext
                            join c in db.V_M_TM_SYS_BaseData_store on a.RegisterStoreCode equals c.StoreCode
                            into ac
                            from ca in ac.DefaultIfEmpty()
                            join d in db.V_M_TM_SYS_BaseData_company on a.Corp equals d.CompanyCode
                            into ad
                            from da in ad.DefaultIfEmpty()
                            select new
                            {
                                a.MemberID,
                                a.MemberCardNo,
                                a.CustomerName,
                                a.CustomerMobile,
                                a.RegisterStoreCode,
                                a.Corp,
                                ca.StoreName,
                                da.CompanyName
                            };
                if (!string.IsNullOrWhiteSpace(Corp))
                {
                    query = query.Where(p => p.Corp == Corp);
                }
                if (!string.IsNullOrWhiteSpace(RegisterStoreCode))
                {
                    query = query.Where(p => p.RegisterStoreCode == RegisterStoreCode);
                }
                var res = query.ToDataTableSourceVsPage(pageInfo);
                return new Result(true, "", new List<object> { res });
            }
        }

        public static Result GetMemListID(string Corp, string RegisterStoreCode)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var query = from a in db.V_S_TM_Mem_Ext
                            join c in db.V_M_TM_SYS_BaseData_store on a.RegisterStoreCode equals c.StoreCode
                            into ac
                            from ca in ac.DefaultIfEmpty()
                            join d in db.V_M_TM_SYS_BaseData_company on a.Corp equals d.CompanyCode
                            into ad
                            from da in ad.DefaultIfEmpty()
                            select new
                            {
                                a.MemberID,
                                a.MemberCardNo,
                                a.CustomerName,
                                a.CustomerMobile,
                                a.RegisterStoreCode,
                                a.Corp,
                                ca.StoreName,
                                da.CompanyName
                            };
                if (!string.IsNullOrWhiteSpace(Corp))
                {
                    query = query.Where(p => p.Corp == Corp);
                }
                if (!string.IsNullOrWhiteSpace(RegisterStoreCode))
                {
                    query = query.Where(p => p.RegisterStoreCode == RegisterStoreCode);
                }
                var res = query.Select(p => p.MemberID).ToList();
                var resu=string.Join(",", res);
                return new Result(true, "", new { MemberIDs = resu+"," });
            }
        }

        public static Result GetStoreByCompany(string companycode)
        {
            using (CRMEntities db = new CRMEntities())
            {


                var query = db.V_M_TM_SYS_BaseData_store.Where(p => p.ChannelCodeStore == companycode).Select(p => new { p.StoreCode, p.StoreName });

                return new Result(true, "", new List<object> { query.ToList() });
            }
        }
        public static Result SaveTransform(string Corp, string RegisterStoreCode, string MemId,int userid)
        {
            using (CRMEntities db = new CRMEntities())
            {
                string[] memids = MemId.Substring(0, MemId.Length - 1).Split(',');
                var query = db.TM_Mem_Ext.Where(a => memids.Contains(a.MemberID)).ToList();
                int j=query.Count / 10;
                int k = 1;
                int i = 0;
                List<dynamic> msgjson = new List<dynamic>();
                foreach (var item in query)
                {
                    
                    i++;
                    if (k <= j)
                    {
                        var a = new
                        {
                            MemberID = item.MemberID,
                            OldStore=item.Str_Attr_5,
                            OldCompany=item.Str_Attr_40,
                            NewStore=RegisterStoreCode,
                            NewCompany=Corp

                        };
                        msgjson.Add(a);
                        item.Str_Attr_5 = RegisterStoreCode;
                        item.Str_Attr_40 = Corp;
                        if(i==10)
                        {
                            k++;
                            TL_Sys_Log log = new TL_Sys_Log();
                            log.LogType = "cardTransform";
                            log.LogInfo = JsonHelper.Serialize(msgjson);
                            log.AddedDate = DateTime.Now;
                            log.AddedUser = userid.ToString();
                            db.TL_Sys_Log.Add(log);
                            i = 0;
                            msgjson.Clear();
                        }
                    }
                    else
                    {
                        var a = new
                        {
                            MemberID = item.MemberID,
                            OldStore = item.Str_Attr_5,
                            OldCompany = item.Str_Attr_40,
                            NewStore = RegisterStoreCode,
                            NewCompany = Corp

                        };
                        msgjson.Add(a);
                        item.Str_Attr_5 = RegisterStoreCode;
                        item.Str_Attr_40 = Corp;
                        
                        if (i == query.Count%10)
                        {
                            TL_Sys_Log log = new TL_Sys_Log();
                            log.LogType = "cardTransform";
                            log.LogInfo = JsonHelper.Serialize(msgjson);
                            log.AddedDate = DateTime.Now;
                            log.AddedUser = userid.ToString();
                            db.TL_Sys_Log.Add(log);
                            
                            i = 0;
                            msgjson.Clear();
                        }
                    }
                }
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
                return new Result(true, "");
            }
        }

       
    }
}
