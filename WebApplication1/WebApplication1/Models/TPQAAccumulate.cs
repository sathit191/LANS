using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class TPQAAccumulate
    {
        public string PKGName { get; set; }
        public string DeviceName { get; set; }
        public string JobName { get; set; }
        public int JobId { get; set; }
        public float SumLots { get; set; }
        public float SumKpcs { get; set; }
        public int State { get; set; }
        public float StandardTime { get; set; }
    }

    public class ChartShow
    {
        public string DeviceName { get; set; }
        public int QALot { get; set; }
        public int TPLot { get; set; }
    }
}