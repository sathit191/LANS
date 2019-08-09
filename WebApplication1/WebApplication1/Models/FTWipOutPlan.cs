using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class FTWipOutPlan
    {
        public string Flow { get; set; }
        public string DeviceName { get; set; }
        public int Count { get; set; }
        public string FTDevice { get; set; }
        public int SumKpcs { get; set; }
    }

    public class Flow
    {
        public string Name { get; set; }
        public int A1 { get; set; }
        public int A2 { get; set; }
        public int A3 { get; set; }
        public int A4 { get; set; }
        public int FL { get; set; }
        public float A1_Kpcs { get; set; }
        public float A2_Kpcs { get; set; }
        public float A3_Kpcs { get; set; }
        public float A4_Kpcs { get; set; }
        public float A1_Hour { get; set; }
        public float A2_Hour { get; set; }
        public float A3_Hour { get; set; }
        public float A4_Hour { get; set; }
        public float FL_Kpcs { get; set; }
        public string Color { get; set; }
        public int[] Data { get; set; }
    }
}