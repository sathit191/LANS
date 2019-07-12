using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class FTSetup
    {
        public FTSetup()
        {
            LotQueue = new List<FTWip>();
        }
        public string PKName { get; set; }
        public string MCNo { get; set; }
        public int McId { get; set; }
        public string TesterType { get; set; }
        public string TestBord { get; set; }
        public string DutName { get; set; }
        public string DeviceName { get; set; }
        public string Flow { get; set; }
        public string TestProgram { get; set; }
        public string Temp { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public State Status { get; set; }
        public string Production_LotNo { get; set; }
        public string Production_LotDevice { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? Production_Date { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Production_Time { get; set; }
        public DateTime? Production_Datetime { get; set; }
        public string Production_DelayLot { get; set; }
        public string countDown { get; set; }
        public string WipCountup { get; set; }
        public List<FTWip> LotQueue { get; set; }
        public int? OverPlan { get; set; }
        public string Mc_Color { get; set; }
        public DateTime? STime { get; set; }
        public DateTime? ETime { get; set; }
        public enum State
        {
            Wait = 0,
            Setup = 1,
            Run = 2,
            Ready = 3
        }
        

    }

   
}