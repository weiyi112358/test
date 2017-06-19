using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model
{
  
        public class Mappings
        {
            public List<Mapping> Fields { set; get; }
        }


        public class Mapping
        {
            public string FieldName { set; get; }
            public string MappingExpression1 { set; get; }
            public string MappingExpression2 { set; get; }
            public string MappingExpression3 { set; get; }
        }
    
}
