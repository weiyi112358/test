﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Arvato.CRM.Model
{
    public class KPIResultModel
    {
        public long KPIResultID { get; set; }
        public long KPIID { get; set; }
        public string KPIName { get; set; }
        public string TargetValueType { get; set; }
        public string TargetValue { get; set; }
        public string CurrentValue { get; set; }
        public string Unit { get; set; }
        public string KPIType { get; set; }
        public string KPITypeValue { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public DateTime ComputeTime { get; set; }
        public Nullable<int> IntValue1 { get; set; }
        public Nullable<decimal> DecValue1 { get; set; }
        public string StrValue1 { get; set; }
    }
}