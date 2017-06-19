using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Model.Config
{
    public class ProjectConfigElement : ConfigurationSection
    {
        [ConfigurationProperty("activity")]
        public ActivityElement Activity
        {
            get
            {
                return (ActivityElement)this["activity"];
            }
            set
            {
                this["activity"] = value;
            }
        }
    }
}
