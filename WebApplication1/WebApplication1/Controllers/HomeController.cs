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

            List<FTSetup> lstFTSetup = (List<FTSetup>)repository.fTSetups;
            List<FTWip> lstFTWips = (List<FTWip>)repository.FTWips;
             List<FTWip> lstFTWipOut = new List<FTWip>();
            //List<FTWip> lstFTWipOut = lstFTWips.Where(x => !lstFTSetup.Where(p => p.DeviceName == x.DeviceName).Any()).ToList();


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
                List<FTWip> lstFTWipsAuto1 = lstFTWips.Where(x => x.JobName == "AUTO" + J).OrderBy(x => x.Lot_no).ToList();

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


                foreach (var deviceName in machineDevicesList)
                {

                    List<FTSetup> McDevice = lstFTSetupAuto1.Where(x => x.DeviceName == deviceName.DeviceName).ToList(); //กรอก Device
                    List<FTWip> WipDevice = lstFTWipsAuto1.Where(x => x.DeviceName == deviceName.DeviceName).ToList();

                    int count = 1;
                    foreach (var item in WipDevice)
                    {
                        int sequence = count % McDevice.Count;
                        if (sequence != 0)
                        {
                            item.Sequence = sequence;
                        }
                        else
                        {
                            item.Sequence = McDevice.Count;
                        }
                        count++;
                    }


                    int countMc = 1;
                    int lotOverShows = 0 ;
                    foreach (var mcData in McDevice.ToArray()) //เอาลอ็อตเข้าQue
                    {
                        List<FTWip> lstFTWipsAuto = new List<FTWip>();
                        for (int i = 2; i <= 10; i++)
                        {
                            List<FTWip> lstFTWipsAuto1OnMc = WipDevice.Where(x => x.Sequence == countMc).ToList();
                            foreach (var lotSequence in lstFTWipsAuto1OnMc)
                            {
                                lstFTWipsAuto.Add(lotSequence);
                            }
                            for (int k = 0; k < 9 - lstFTWipsAuto1OnMc.Count; k++)
                            {
                                FTWip fTWip = new FTWip();
                                fTWip.DeviceName = "";
                                fTWip.Lot_no = "";
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
                item.Production_LotDevice = onMc.Device;
                DateTime? production_Date = null;
                if (onMc.ProcessState == FTSetup.State.Run)
                {

                    if (item.Flow == "AUTO1")
                    {
                        production_Date = onMc.Updated_time.Value + onMc.timeAuto1;
                    }
                    else if (item.Flow == "AUTO2")
                    {
                        production_Date = onMc.Updated_time.Value + onMc.timeAuto2;
                    }
                    else if (item.Flow == "AUTO3")
                    {
                        production_Date = onMc.Updated_time.Value + onMc.timeAuto3;
                    }
                    else if (item.Flow == "AUTO4")
                    {
                        production_Date = onMc.Updated_time.Value + onMc.timeAuto4;
                    }
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

            // Group the FTWip by DeviceName
            var DeviceList = from Group in lstFTWipOut
                                group Group by new {Group.JobName,Group.DeviceName} into list

                                select new FTWipOutPlan { Flow = list.Key.JobName,  DeviceName =list.Key.DeviceName, Count = list.Count() , SumKpcs = lstFTWipOut.Sum(x=>x.Kpcs)};

            var List2 = lstFTWipOut.GroupBy(x => x.JobName, y => y.DeviceName).ToList();


            List<FTWipOutPlan> outPlans = new List<FTWipOutPlan>();
            
            foreach (var item in DeviceList)
            {
                outPlans.Add(item);
            }

            List<Flow> flows = new List<Flow>();
            var DeviceGroup = lstFTWipOut.Select(p => new { p.DeviceName }).Distinct().ToList();

            foreach (var item in DeviceGroup)
            {
                // List<FTWip> WipDevice = lstFTWipsAuto1.Where(x => x.DeviceName == deviceName.DeviceName).ToList();
                string name = item.DeviceName.ToString();
                int A1 = 0;
                int A2 = 0;
                int A3 = 0;
                int A4 = 0;
                var addflow = new Flow { Name = name, A1 = A1, A2 = A2, A3 = A3, A4 = A4 };
                flows.Add(addflow);

                var lstDevice = DeviceList.Where(p => p.DeviceName == item.DeviceName).ToList();

                foreach (var list in lstDevice)
                {
                    var row = flows.Where(p => p.Name == list.DeviceName).SingleOrDefault();

                    if(list.Flow == "AUTO1")
                    {
                        row.A1 = list.Count;
                    }
                    else if(list.Flow == "AUTO2")
                    {
                        row.A2 = list.Count;
                    }
                    else if (list.Flow == "AUTO3")
                    {
                        row.A3 = list.Count;
                    }
                    else if (list.Flow == "AUTO4")
                    {
                        row.A4 = list.Count;
                    }
                }

                var addData = flows.Where(p => p.Name == item.DeviceName).SingleOrDefault();

                //string data = "[49.9, 71.5, 106.4, 129.2]";


            }

            var list2 = outPlans.Select(p=> new { p.Flow}).Distinct().ToList();

            ViewBag.lstFTWipOut = outPlans;

            string command = "";

            foreach (var item in flows)
            {
                command += "{";
                command += "name: '" + item.Name + "',";
                command += "data: [" + item.A1 + "," + item.A2 + "," + item.A3 + "," + item.A4 + "]},";
            }

            ViewBag.lstFlow = command;

            return View();

        }
        

    }
}