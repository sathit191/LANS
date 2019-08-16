using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class LimitFlow
    {
        public string Name { get; set; }
        public string PKG { get; set; }
        public string Flow { get; set; }
        public int IsAlarmed { get; set; }
    }
}