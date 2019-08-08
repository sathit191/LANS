using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class TPWip
    {
        public string Mcname { get; set; }
        public string LotNo { get; set; }
        public string DeviceName { get; set; }
        public string PKGName { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int Kpcs { get; set; }
        public float QtyProduction { get; set; }
        public int State { get; set; }
        public float StandareTime { get; set; }
        public DateTime UpdateAt { get; set; }
        public string McWip { get; set; }
    }
}