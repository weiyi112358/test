using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{

    public class SysClassModel
    {
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassType { get; set; }
        public int? RoleID { get; set; }
        public int? UserID { get; set; }
        public short Sort { get; set; }
        public string AddedUser { get; set; }
        public DateTime? AddedDate { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int DataGroupID { get; set; }
    }
}
