using System;
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

                for (int i = 0; i < machineDevicesList.Count(); i++)
                {

                }

                foreach (var deviceName in machineDevicesList)
                {

                    List<FTSetup> McDevice = lstFTSetupAuto1.Where(x => x.DeviceName == deviceName.DeviceName).ToList(); //กรอก Device
                    List<FTWip> WipDevice = lstFTWipsAuto1.Where(x => x.DeviceName == deviceName.DeviceName).ToList();

                    //List<FTWip> WipOtherDevice = lstFTWipsAuto1.Where(x => !DevicesList.Where(y => y.DeviceName == x.DeviceName).Any()).ToList();

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

                        }
                        mcData.LotQueue = lstFTWipsAuto;
                        //mcData.LotOutPlan = WipOtherDevice;
                        countMc++;
                    }
                }
                var result = lstFTWipsAuto1.Where(x => !machineDevicesList.Where(y => y.DeviceName == x.DeviceName).Any()).ToList();
            } //lot wip add to Que


            // lstFTWips.Where(x => x != lstFTSetup.ToList())
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
            ViewBag.lstFTWipOut = lstFTWipOut;
            return View();

            //return View(repository.fTSetups);
        }
        

    }
}