using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class LotFTinMc
    {
        public static object State { get; internal set; }
        public string MCName { get; set; }
        public string LotNo { get; set; }
        public string Device { get; set; }
        public string FTDevice { get; set; }
        public DateTime? Updated_time { get; set; } 
        public float StandardTime { get; set; }
        public TimeSpan  timeAuto1 { get; set; }
        public TimeSpan timeAuto2 { get; set; }
        public TimeSpan timeAuto3 { get; set; }
        public TimeSpan timeAuto4 { get; set; }

        public FTSetup.State ProcessState { get; set; }
        



    }
}