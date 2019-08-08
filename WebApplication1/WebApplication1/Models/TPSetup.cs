using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class TPSetup
    {
        public TPSetup()
        {
            LotQueue = new List<TPWip>();
        }
        public string PKGName { get; set; }
        public string MCNo { get; set; }
        public int McId { get; set; }
        public string MCType { get; set; }
        public string DeviceName { get; set; }
        public string Production_LotNo { get; set; }
        public string Production_LotDevice { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? Production_Date { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Production_Time { get; set; }
        public List<TPWip> LotQueue { get; set; }
        public State Status { get; set; }
        public enum State
        {
            Wait = 0,
            Setup = 1,
            Run = 2,
            Ready = 3
        }
    }
}