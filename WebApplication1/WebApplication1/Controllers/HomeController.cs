﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Abstract;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private INormalRepository repository;

        public HomeController(INormalRepository repo)
        {
            repository = repo;
        }
        // GET: Home
        public class Group<T1,T2>
        {
            public string Key { get; set; }
            public string Key1 { get; set; }
            public int Values { get; set; }
        }
        public ActionResult Index3()
        {
            //DATA-----------------------------------------------------------------------------------------------
            List<FTSetup> lstFTSetup = (List<FTSetup>)repository.fTSetups;
            List<FTWip> lstFTWips = (List<FTWip>)repository.FTWips;
            List<Accumulator_Plan> lstAccumulator_Plans = (List<Accumulator_Plan>)repository.Plan;
            //---------------------------------------------------------------------------------------------------
            //Main table------------------------------------------------------------------------------------------
             List<FTWip> lstFTWipOut = new List<FTWip>();
            
            var ColorList = lstFTWips.Select(x => new { x.DeviceName }).Distinct().ToList();
            int countColor = 1;
            foreach (var deviceName in ColorList)
            {
                List<FTWip> WipDevice = lstFTWips.Where(x => x.DeviceName == deviceName.DeviceName).ToList();
                List<FTSetup> ColorOnMc = lstFTSetup.Where(x => x.DeviceName == deviceName.DeviceName).ToList();

                foreach (var item in WipDevice)
                {
                    item.S_Color = "A" + countColor;

                }
                foreach (var item in ColorOnMc)
                {
                    item.Mc_Color = "A" + countColor;
                }
                countColor++;
            }

            for (int J = 1; J <= 4; J++)
            {
                List<FTSetup> lstFTSetupAuto1 = lstFTSetup.Where(x => x.Flow == "AUTO" + J).OrderBy(x => x.MCNo).ToList(); //หาเครื่องกรอก Auto
                List<FTWip> lstFTWipsAuto1 = lstFTWips.Where(x => x.JobName == "AUTO" + J && x.Lot_State == FTWip.LotState.Wip).OrderBy(x => x.Lot_no).ToList();

                var machineDevicesList = lstFTSetupAuto1.Select(x => new { x.DeviceName }).Distinct().ToList();


                var wipLotOutPlan = lstFTWipsAuto1.Where(x => !machineDevicesList.Where(y => y.DeviceName == x.DeviceName).Any()).ToList();

                foreach (var item in wipLotOutPlan)
                {
                    lstFTWipOut.Add(item);

                }

                //// Group the FTWip by DeviceName
                //var DeviceGrouped = from deviceGroup in lstFTWipOut
                //                   group deviceGroup by deviceGroup.DeviceName into device
                //                   select new Group<string, FTWip> { Key = device.Key, Values = device };

                var machineNoList = (List<string>)lstFTSetupAuto1.Select(x => x.MCNo).Distinct().ToList();
                //GetData
                List<FTMachineSchedulerSetup> machineManualSetupList = (List<FTMachineSchedulerSetup>)repository.FTSchedulerSetup(machineNoList);
                ////SetData Machine Device Now
                //foreach (var machineManualSetup in machineManualSetupList)
                //{
                //    var FtMachine = lstFTSetupAuto1.Where(x => x.MCNo == machineManualSetup.MachineNo).Select(x => new { x.DeviceName }).ToList();
                //    if (FtMachine.Count > 0)
                //    {
                //        machineManualSetup.DeviceNow = FtMachine.FirstOrDefault().DeviceName;
                //    }
                
                //}



                foreach (var deviceName in machineDevicesList)
                {
                    List<FTSetup> McDevice = lstFTSetupAuto1.Where(x => x.DeviceName == deviceName.DeviceName).ToList(); //กรอก Device
                    List<FTWip> WipDevice = lstFTWipsAuto1.Where(x => x.DeviceName == deviceName.DeviceName).ToList();
                    List<FTMachineSchedulerSetup> machineSetupList = McDevice.Select(x => new FTMachineSchedulerSetup { MachineNo = x.MCNo, DeviceNow = x.DeviceName,MachienDisable = false }).ToList();
                    //ทำต่อ
                    var machineManualSetupSittingList = machineManualSetupList.Where(x => McDevice.Where(y => y.MCNo == x.MachineNo).Any()).ToList();
                    int countX = 1;
                    int countY = 2;
                    foreach (var item in WipDevice)
                    {
                        //var machineSetup = machineManualSetupList.Where(x => x.Sequence == countY && x.DeviceNow == deviceName.DeviceName).ToList();

                        var machineManualSetupListTmp = machineManualSetupSittingList.Where(y => y.DeviceNow == deviceName.DeviceName && y.Sequence <= countY).ToList();
                        foreach (var mcSetup in machineManualSetupListTmp)
                        {
                            mcSetup.MachienDisable = true;
                        }
                        var machineManualSetupDisable = machineManualSetupListTmp.Where(y => y.MachienDisable == true).ToList();
                        var mcinfo = machineSetupList.Where(x => machineManualSetupDisable.Where(y=> y.MachineNo != x.MachineNo).Any() || machineManualSetupDisable.Count == 0).ToList();

                        if (mcinfo.Count != 0)
                        {
                            int sequence = (countX + 1) % mcinfo.Count;

                            item.MachineWip = mcinfo[sequence].MachineNo;

                            if (sequence == mcinfo.Count -1)
                                countY++;
                            ////int sequence = countX % McDevice.Count;
                            //if (sequence != 0)
                            //{
                            //    item.Sequence = sequence;
                            //}
                            //else
                            //{
                            //    item.Sequence = McDevice.Count;
                            //    countY++;
                            //}
                            countX++;
                        }
                      
                    }


                    int countMc = 1;
                    int lotOverShows = 0 ;
                    foreach (var mcData in McDevice.ToArray()) //เอาลอ็อตเข้าQue
                    {
                        List<FTWip> lstFTWipsAuto = new List<FTWip>();
                        for (int i = 2; i <= 10; i++)
                        {
                            //List<FTWip> lstFTWipsAuto1OnMc = WipDevice.Where(x => x.Sequence == countMc).ToList();
                            List<FTWip> lstFTWipsAuto1OnMc = WipDevice.Where(x => x.MachineWip == mcData.MCNo).ToList();
                            foreach (var lotSequence in lstFTWipsAuto1OnMc)
                            {
                                lstFTWipsAuto.Add(lotSequence);
                            }
                            for (int k = 0; k < 9 - lstFTWipsAuto1OnMc.Count; k++)
                            {
                                string device = "";
                                string lotno = "";
                                if (k == 0)
                                {
                                    device = machineManualSetupList.Where(x => x.MachineNo == mcData.MCNo).Select(x => x.DeviceChange).FirstOrDefault();
                                    if(device != null)
                                    {
                                        lotno = "Type Change";
                                    }
                                    else
                                    {
                                        lotno = "Set Type Change";
                                        
                                    }
                                }
                                FTWip fTWip = new FTWip();
                                fTWip.FTDevice = device;
                                fTWip.Lot_no = lotno;
                                fTWip.S_Color = "TypeChange";
                                lstFTWipsAuto.Add(fTWip);
                            }
                            if(lstFTWipsAuto1OnMc.Count()-4 > 0)
                            {
                                lotOverShows = lstFTWipsAuto1OnMc.Count() - 4;
                            }

                        }
                        mcData.LotQueue = lstFTWipsAuto;
                        mcData.OverPlan = lotOverShows;
                        //mcData.LotOutPlan = WipOtherDevice;
                        countMc++;
                    }
                }
                var result = lstFTWipsAuto1.Where(x => !machineDevicesList.Where(y => y.DeviceName == x.DeviceName).Any()).ToList();
            } //lot wip add to Que

            List<LotFTinMc> lotFTinMcs = (List<LotFTinMc>)repository.LotFTinMcs;

            foreach (var item in lstFTSetup)
            {
                LotFTinMc onMc = lotFTinMcs.Where(x => x.MCName == item.MCNo).FirstOrDefault();
                if (onMc == null)
                    continue;
                item.Production_LotNo = onMc.LotNo;
                item.Production_LotDevice = onMc.FTDevice;
                DateTime? production_Date = null;
                if (onMc.ProcessState == FTSetup.State.Run)
                {

                    //if (item.Flow == "AUTO1")
                    //{
                        TimeSpan StandardTime = new TimeSpan(0, (int)onMc.StandardTime, 0);
                        production_Date = onMc.Updated_time.Value + StandardTime;

                        var data = lstFTWips.Where(p => p.Lot_no == onMc.LotNo).Select(p => new { p.Lot_no, p.Kpcs, p.Qty_Production, p.StandardTime });
                        float time = data.FirstOrDefault().StandardTime * (float)data.FirstOrDefault().Qty_Production;

                        item.countDown = (new TimeSpan(0, (int)time, 0)).ToString().Substring(0, 5);
                    //}
                    //else if (item.Flow == "AUTO2")
                    //{
                    //    production_Date = onMc.Updated_time.Value + onMc.timeAuto2;

                    //    var data = lstFTWips.Where(p => p.Lot_no == onMc.LotNo).Select(p => new { p.Lot_no, p.Kpcs, p.Qty_Production, p.StandardTime });
                    //    float xx = data.FirstOrDefault().StandardTime * (float)data.FirstOrDefault().Qty_Production;
                    //    float hours = xx;
                    //    float minminute = (xx - hours) * 60;
                    //    item.countDown = hours.ToString("00") + ":" + minminute.ToString("00");
                    //}
                    //else if (item.Flow == "AUTO3")
                    //{
                    //    production_Date = onMc.Updated_time.Value + onMc.timeAuto3;

                    //    var data = lstFTWips.Where(p => p.Lot_no == onMc.LotNo).Select(p => new { p.Lot_no, p.Kpcs, p.Qty_Production, p.StandardTime });
                    //    float xx = data.FirstOrDefault().StandardTime * (float)data.FirstOrDefault().Qty_Production;
                    //    float hours = xx;
                    //    float minminute = (xx - hours) * 60;
                    //    item.countDown = hours.ToString("00") + ":" + minminute.ToString("00");
                    //}
                    //else if (item.Flow == "AUTO4")
                    //{
                    //    production_Date = onMc.Updated_time.Value + onMc.timeAuto4;

                    //    var data = lstFTWips.Where(p => p.Lot_no == onMc.LotNo).Select(p => new { p.Lot_no, p.Kpcs, p.Qty_Production, p.StandardTime });
                    //    float xx = data.FirstOrDefault().StandardTime * (float)data.FirstOrDefault().Qty_Production;
                    //    float hours = xx;
                    //    float minminute = (xx - hours) * 60;
                    //    item.countDown = hours.ToString("00") + ":" + minminute.ToString("00");
                    //}
                    if (production_Date.HasValue)
                    {
                        item.Production_Date = production_Date.Value;
                        item.Production_Time = production_Date.Value;
                        
                    }

                    
                }

                item.Status = onMc.ProcessState;
            }

            //ViewData["listA"] = lstFTSetup;
            ViewBag.ftSetup = lstFTSetup;
            //-----------------------------------------------------------------------------------------------------
            //chart------------------------------------------------------------------------------------------------
            // Group the FTWip by DeviceName
            var DeviceList = from Group in lstFTWips// lstFTWipOut
                             group Group by new { Group.JobName, Group.DeviceName, Group.FTDevice } into list
            
                             select new FTWipOutPlan
                             {
                                 Flow = list.Key.JobName,
                                 FTDevice = list.Key.FTDevice,
                                 DeviceName = list.Key.DeviceName,
                                 Count = list.Count(),
                                 SumKpcs = list.Sum(p=>p.Kpcs)
                             };

            List<Flow> flows = new List<Flow>();

            var DeviceGroup = lstFTWips.Select(p => new { p.DeviceName, p.FTDevice, p.S_Color }).Distinct().ToList();

            foreach (var item in DeviceGroup)
            {
                // List<FTWip> WipDevice = lstFTWipsAuto1.Where(x => x.DeviceName == deviceName.DeviceName).ToList();
                string name = item.FTDevice.ToString();

                string color = item.S_Color;

                var addflow = new Flow { Name = name, A1 = 0, A2 = 0, A3 = 0, A4 = 0, FL = 0,
                    A1_Kpcs = 0, A2_Kpcs = 0, A3_Kpcs = 0, A4_Kpcs = 0, FL_Kpcs = 0, Color = color };
                flows.Add(addflow);

                var lstDevice = DeviceList.Where(p => p.DeviceName == item.DeviceName).ToList();

                foreach (var list in lstDevice)
                {
                    var row = flows.Where(p => p.Name == list.FTDevice).SingleOrDefault();

                    if(list.Flow == "AUTO1")
                    {
                        row.A1 = list.Count;
                        row.A1_Kpcs = (float)list.SumKpcs / 1000 ;
                        
                    }
                    else if(list.Flow == "AUTO2")
                    {
                        row.A2 = list.Count;
                        row.A2_Kpcs = (float)list.SumKpcs / 1000;
                    }
                    else if (list.Flow == "AUTO3")
                    {
                        row.A3 = list.Count;
                        row.A3_Kpcs = (float)list.SumKpcs / 1000;
                    }
                    else if (list.Flow == "AUTO4")
                    {
                        row.A4 = list.Count;
                        row.A4_Kpcs = (float)list.SumKpcs / 1000;
                    }
                    else if(list.Flow.Substring(0,2)== "FL")
                    {
                        row.FL = list.Count;
                        row.FL_Kpcs = (float)list.SumKpcs / 1000;
                    }
                }

                var addData = flows.Where(p => p.Name == item.DeviceName).SingleOrDefault();

                //string data = "[49.9, 71.5, 106.4, 129.2]";


            }
            
            //var list2 = outPlans.Select(p=> new { p.Flow}).Distinct().ToList();

            //ViewBag.lstFTWipOut = outPlans;

            string command = "";
            foreach (var item in flows)
            {
                command += "{";
                command += "name: '" + item.Name + "',";
                command += "data: [" + item.FL + "," + item.A1 + "," + item.A2 + "," + item.A3 + "," + item.A4 + "]},";
            }
            string commandKpcs = "";
            foreach (var item in flows)
            {
                commandKpcs += "{";
                commandKpcs += "name: '" + item.Name + "',";
                commandKpcs += "data: [" + item.FL_Kpcs.ToString("00") + "," + item.A1_Kpcs.ToString("00") + "," + item.A2_Kpcs.ToString("00") + "," + 
                    item.A3_Kpcs.ToString("00") + "," + item.A4_Kpcs.ToString("00") + "]},";
            }

            ViewBag.lstFlow = command;
            ViewBag.chartKpcs = commandKpcs;
            //ViewBag.chartcolor = commandColor;
            //-----------------------------------------------------------------------------------------------------
            //table Denpyo-----------------------------------------------------------------------------------------
            List<FTDenpyo_Calculate> fTDenpyo_Calculates = new List<FTDenpyo_Calculate>();
            
            foreach (var item in DeviceGroup)
            {
                var listDenpyo = lstFTWips.Where(p => p.DeviceName == item.DeviceName);
                var listPlan = lstAccumulator_Plans.Where(p => p.DeviceName == item.DeviceName);
                
                if (listDenpyo.Count() != 0)
                {
                    var calculate = new FTDenpyo_Calculate
                    {
                        PKGName = listDenpyo.FirstOrDefault().PKGName,
                        DeviceName = item.DeviceName,
                        FTDevice = item.FTDevice,
                        TextColor = listDenpyo.FirstOrDefault().S_Color,
                        A1_Calculate = 0,
                        A1_Lot = 0,
                        A2_Calculate = 0,
                        A2_Lot = 0,
                        A3_Calculate = 0,
                        A3_Lot = 0,
                        A4_Calculate = 0,
                        A4_Lot = 0

                        
                        

                    };
                    if (listPlan.Count() != 0)
                    {
                        if (item.DeviceName == listPlan.FirstOrDefault().DeviceName)
                        {
                            calculate.Plan_today = (float)listPlan.FirstOrDefault().Kpcs_PlanT / 1000;
                            calculate.Result_today = (float)listPlan.FirstOrDefault().Kpcs_ResultT / 1000;
                            calculate.Calulate_today = (int)(listPlan.FirstOrDefault().Kpcs_ResultT - listPlan.FirstOrDefault().Kpcs_PlanT) / 1000;

                            calculate.Plan_yesterday = (float)listPlan.FirstOrDefault().Kpcs_PlanY / 1000;
                            calculate.Result_yesterday = (float)listPlan.FirstOrDefault().Kpcs_ResultY / 1000;
                            calculate.Calulate_yesterday = (int)(listPlan.FirstOrDefault().Kpcs_ResultY - listPlan.FirstOrDefault().Kpcs_PlanY) / 1000;
                        }
                    }
                    fTDenpyo_Calculates.Add(calculate);
                }

                DateTime dateS = DateTime.Now;
                DateTime dateE = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 8, 0, 0);
                float countdownHours = (float)((dateE - dateS).TotalHours);

                foreach (var row in listDenpyo)
                {
                    var selectrow = fTDenpyo_Calculates.Where(p => p.PKGName == row.PKGName && p.DeviceName == row.DeviceName).SingleOrDefault();
                    

                    if (row.JobId == "106")
                    {
                        selectrow.A1_Lot++;
                        selectrow.A1_Calculate += row.StandardTime / 60;
                    }
                    else if (row.JobId == "108")
                    {
                        selectrow.A2_Lot++;
                        selectrow.A2_Calculate += row.StandardTime / 60;
                    }
                    else if (row.JobId == "110")
                    {
                        selectrow.A3_Lot++;
                        selectrow.A3_Calculate += row.StandardTime / 60;
                    }
                    else if (row.JobId == "119")
                    {
                        selectrow.A4_Lot++;
                        selectrow.A4_Calculate += row.StandardTime / 60;
                    }
                    else if(row.JobId == "87" || row.JobId == "88" || row.JobId == "278" )
                    {
                        selectrow.FL_Lot++;
                        selectrow.FL_Calculate += row.StandardTime / 60;
                    }

                    if (row.JobId == "119" && row.MachineWip != null && countdownHours > row.StandardTime / 60) //WIP Plan 
                    {
                        selectrow.Result_today += (float)row.Kpcs / 1000;
                        selectrow.Calulate_today = selectrow.Result_today - selectrow.Plan_today;
                        countdownHours -= row.A4;
                    }else if(row.JobId == "119" && row.Lot_State == FTWip.LotState.Start && countdownHours > row.StandardTime / 60) //ON Mc 
                    {
                        selectrow.Result_today += (float)row.Kpcs / 1000;
                        selectrow.Calulate_today = selectrow.Result_today - selectrow.Plan_today;
                        countdownHours -= row.A4;
                    }
                }
                //foreach (var device in DeviceGroup)
                //{
                //    foreach (var row in listDenpyo)
                //    {
                        
                //    }

            }
            ViewBag.Denpyo_Calculates = fTDenpyo_Calculates;
            //-----------------------------------------------------------------------------------------------------
            return View();

        }
        public ActionResult SaveTypeChange(string McNo,int Sequence,string Device ,string DeviceChange)
        {
            // List<FTSetup> lstFTSetup = (List<FTSetup>)repository.fTSetups;
            List<string> lstMcNo = new List<string>();
            lstMcNo.Add(McNo);
            List<FTMachineSchedulerSetup> lstschedulerSetups = (List<FTMachineSchedulerSetup>)repository.FTSchedulerSetup(lstMcNo);
            if (lstschedulerSetups.Count() > 0)
            {
                repository.UpdateData(McNo, Sequence, Device, DeviceChange);
            }
            else
            {
                repository.SaveUpdate(McNo, Sequence, Device, DeviceChange);
                
            }
            return RedirectToAction("Index3");
        }

        public ActionResult CancelTypeChange(string McNo)
        {
            
            List<string> lstMcNo = new List<string>();
            lstMcNo.Add(McNo);
            List<FTMachineSchedulerSetup> lstschedulerSetups = (List<FTMachineSchedulerSetup>)repository.FTSchedulerSetup(lstMcNo);
            if (lstschedulerSetups.Count() > 0)
            {
                repository.CencelTc(McNo);
            }
            else
            {
            }
            return RedirectToAction("Index3");
        }
    }
}