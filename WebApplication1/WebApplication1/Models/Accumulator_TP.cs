using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Accumulator_TP
    {
        public string PKG { get; set; }
        public string DeviceName { get; set; }
        public float Kpcs_PlanT { get; set; }
        public float Kpcs_ResultT { get; set; }
        public float Kpcs_PlanY { get; set; }
        public float Kpcs_ResultY { get; set; }
        public float Kpcs_SumT { get; set; }
        public float Kpcs_SumY { get; set; }
    }
    public class Accumulator_QA
    {
        public string DeviceName { get; set; }
        public int Kpcs_PlanT { get; set; }
        public int Kpcs_ResultT { get; set; }
        public int Kpcs_PlanY { get; set; }
        public int Kpcs_ResultY { get; set; }
    }
    public class Calculate_TP
    {
        public string PKGName { get; set; }
        public string DeviceName { get; set; }
        public string TextColor { get; set; }
        public float QALot { get; set; }
        public float TPLot { get; set; }
        public float SumRunTime { get; set; }
        public float Plan_today { get; set; }
        public float Result_today { get; set; }
        public float Calulate_today { get; set; }
        public float Plan_yesterday { get; set; }
        public float Result_yesterday { get; set; }
        public float Calulate_yesterday { get; set; }
        public bool SetOnMcA1 { get; set; }
        public bool SetOnMcA2 { get; set; }
        public bool SetOnMcA3 { get; set; }
        public bool SetOnMcA4 { get; set; }
    }
}