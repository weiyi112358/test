using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Arvato.CRM.BizContract;

namespace Arvato.CRM.BizCommon
{
    public class BizCommon : ICommonContract
    {
        private const string AssemblyName = "Arvato.CRM.BizLogic";
        private const string AssemblyPreName = "Arvato.CRM.BizLogic.";
        public string CallFunctionWithSession(string domain, string action, string session, params object[] objs)
        {
            Type type = Assembly.Load(AssemblyName).GetType(AssemblyPreName + domain);
            List<object> o = new List<object>();
            o.Add(session);
            o.AddRange(objs);
            return Utility.JsonHelper.Serialize(type.InvokeMember(action, BindingFlags.Default | BindingFlags.InvokeMethod, null, null, o.ToArray()));
        }

        public string CallFunction(string domain, string action, params object[] objs)
        {
            Type type = Assembly.Load(AssemblyName).GetType(AssemblyPreName + domain);
            return Utility.JsonHelper.Serialize(type.InvokeMember(action, BindingFlags.Default | BindingFlags.InvokeMethod, null, null, objs));
        }
    }
}
