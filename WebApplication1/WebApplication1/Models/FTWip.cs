using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class FTWip
    {
        public string MCName { get; set; }
        public string PKName { get; set; }
        public string Lot_no { get; set; }
        public string JobName { get; set; }
        public string Updated_at { get; set; }
        public string DeviceName { get; set; }
        public int Sequence { get; set; }

        public string S_Color { get; set; }

        public int Kpcs { get; set; }

        public string MachineWip { get; set; }
    }
}