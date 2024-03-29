﻿using System;
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

        public int SumKpcs { get; set; }
    }

    public class Flow
    {
        public string Name { get; set; }
        public int A1 { get; set; }
        public int A2 { get; set; }
        public int A3 { get; set; }
        public int A4 { get; set; }
        public int[] Data { get; set; }
    }
}