using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Accumulator_Plan
    {
        public string DeviceName { get; set; }
        public string FTDevice { get; set; }
        public int Kpcs_PlanT { get; set; }
        public int Kpcs_ResultT { get; set; }
        public int Kpcs_PlanY { get; set; }
        public int Kpcs_ResultY { get; set; }


    }
    public class Accumulator_Result
    {
        public string DeviceName { get; set; }
        public int Kpcs { get; set; }

    }
}