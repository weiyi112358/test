using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Arvato.CRM.Utility;

namespace Arvato.CRM.BizContract
{
    [ServiceContract]
    [ServiceKnownType("GetKnownTypes", typeof(KnownTypeHelper))]
    public interface ICommonContract
    {
        [OperationContract]
        string CallFunctionWithSession(string domain, string action, string session, params object[] objs);
        //string CallFunctionWithSession(string domain, string action, Dictionary<string, object> session, params object[] objs);

        [OperationContract]
        string CallFunction(string domain, string action, params object[] objs);

    }
}
