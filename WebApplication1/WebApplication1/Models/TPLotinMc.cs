using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class TPLotinMc
    {
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public string DeviceName { get; set; }
        public string McName { get; set; }
        //public int ProcessState { get; set; }
        public DateTime UpDateTime { get; set; }
        public int StandardTime { get; set; }
        public TPSetup.State ProcessState { get; set; }

    }
}