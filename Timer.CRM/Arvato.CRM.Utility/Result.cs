using System;
using System.Configuration;
using System.Collections.Generic;


namespace Arvato.CRM.Utility
{
    [Serializable]
    public struct Result
    {
        public bool IsPass;
        public string MSG;
        public List<object> Obj;
        public Dictionary<string, object> Sessions;

        public Result(bool isPass) : this(isPass, null) { }

        public Result(bool isPass, string msg) : this(isPass, msg, null) { }

        public Result(bool isPass, string msg, List<object> obj) : this(isPass, msg, obj, new Dictionary<string, object>()) { }
        public Result(bool isPass, string msg, object obj) : this(isPass, msg, new List<object> { obj }, new Dictionary<string, object>()) { }

        public Result(bool isPass, string msg, List<object> obj, Dictionary<string, object> sessions)
        {
            IsPass = isPass;
            MSG = msg;
            Obj = obj;
            Sessions = sessions;
        }

    }
}
