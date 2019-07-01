using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class FTMachineSchedulerSetup
    {
        public int Priority { get; set; }
        public string MachineNo { get; set; }
        public byte Sequence { get; set; }
        public string DeviceChange { get; set; }
        public string DeviceNow { get; set; }
        public DateTime? DateChange { get; set; }
        public DateTime? DateComplete { get; set; }
        public bool MachienDisable { get; set; }

    }
}