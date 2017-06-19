using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Utility
{
    public class KnownTypeHelper
    {
        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            return Assembly.Load("Arvato.CRM.Model").GetTypes();
        }
    }
}
