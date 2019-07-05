using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class FTDenpyo
    {
        public string PKGName { get; set; }
        public string DeviceName { get; set; }
        public string JobId { get; set; }
        public float A1 { get; set; }
        public float A2 { get; set; }
        public float A3 { get; set; }
        public float A4 { get; set; }
        //public int[] Data { get; set; }
    }

    public class FTDenpyo_Calculate
    {
        public string PKGName { get; set; }
        public string DeviceName { get; set; }
        public string TextColor { get; set; }
        public string FTDevice { get; set; }
        public float A1_Calculate { get; set; }
        public float A2_Calculate { get; set; }
        public float A3_Calculate { get; set; }
        public float A4_Calculate { get; set; }
        public int A1_Lot { get; set; }
        public int A2_Lot { get; set; }
        public int A3_Lot { get; set; }
        public int A4_Lot { get; set; }
        public float Plan_today { get; set; }
        public float Result_today { get; set; }
        public float Calulate_today { get; set; }
        public float Plan_yesterday { get; set; }
        public float Result_yesterday { get; set; }
        public float Calulate_yesterday { get; set; }
    }
}