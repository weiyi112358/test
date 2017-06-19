using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model.Config
{
    public class ActivityElement : ConfigurationElement
    {
        [ConfigurationProperty("workflowTemplateList")]
        public string WorkflowTemplateList
        {
            get
            {
                return (string)this["workflowTemplateList"];
            }
            set
            {
                this["workflowTemplateList"] = value;
            }
        }
    }
}
