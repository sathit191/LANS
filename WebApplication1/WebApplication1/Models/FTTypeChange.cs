using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class FTTypeChange
    {
        public int priority { get; set; }
        public string MCNo { get; set; }
        public int sequence { get; set; }
        public string device_change { get; set; }
        public string device_now { get; set; }
        public DateTime date_change { get; set; }
        public DateTime date_complete { get; set; }
    }
}