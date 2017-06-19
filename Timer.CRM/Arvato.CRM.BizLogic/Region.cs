using Arvato.CRM.EF;
using Arvato.CRM.Utility;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.BizLogic
{
    public static class Region
    {
        /// <summary>
        /// 根据父级ID获取省市区信息
        /// </summary>
        /// <param name="pID">父级ID</param>
        /// <returns></returns>
        public static Result GetRegionByPID(int pid, int grade)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var data = from r in db.TD_SYS_Region
                           where r.ParentRegionID == SqlFunctions.StringConvert((double)pid).Trim() && r.RegionGrade == grade && r.Enable == true
                           select r;
                return new Result(true, "", data.Distinct().ToList());
            }
        }

        public static Result InputRegionByCertNo(int regionId)
        {
            using (CRMEntities db = new CRMEntities())
            {
                var data = from r in db.TD_SYS_Region
                           where r.RegionID == SqlFunctions.StringConvert((double)regionId).Trim() && r.RegionGrade == 3 && r.Enable == true
                           select r;
                return new Result(true, "", data.Distinct().ToList());
            }
        
        }
    }
}
