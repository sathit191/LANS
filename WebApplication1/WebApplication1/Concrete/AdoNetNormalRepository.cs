﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using WebApplication1.Abstract;
using WebApplication1.Models;

namespace WebApplication1.Concrete
{
    public class AdoNetNormalRepository : INormalRepository
    {
        public IEnumerable<FTSetup> fTSetups
        {
            get
            {
                List<FTSetup> lstFTSetup = new List<FTSetup>();
                var conn = new SqlConnection(Properties.Settings.Default.DBConnect);
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "[StoredProcedureDB].[dbo].[sp_get_scheduler_temp]";
                    cmd.Parameters.Add("@DEBUG", System.Data.SqlDbType.Int).Value = Properties.Settings.Default.Debug;
                    //cmd.CommandText = "SELECT TOP (100) MCNo,LotNo,PackageName,DeviceName,ProgramName,TesterType,TestFlow,TestBoxA" +
                    //    ",TestBoxB,DutcardA,DutcardB,OptionName1,OptionName2,'Wait' as Status FROM [DBx].[dbo].[FTSetupReport] " +
                    //    " where PackageName like 'SSOP-B28W'";
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FTSetup fTSetup = new FTSetup();

                            if (!(reader["MCNo"] is DBNull)) fTSetup.MCNo = reader["MCNo"].ToString().Trim();
                            if (!(reader["PackageName"] is DBNull)) fTSetup.PKName = reader["PackageName"].ToString().Trim();
                            if (!(reader["TesterType"] is DBNull)) fTSetup.TesterType = reader["TesterType"].ToString().Trim();
                            if (!(reader["TestBoxA"] is DBNull))
                            {
                                fTSetup.TestBord = reader["TestBoxA"].ToString();
                            }
                            else if (!(reader["TestBoxB"] is DBNull))
                            {
                                fTSetup.TestBord = reader["TestBoxB"].ToString();
                            }
                            if (!(reader["DutcardA"] is DBNull))
                            {
                                fTSetup.DutName = reader["DutcardA"].ToString();
                            }
                            else if (!(reader["DutcardB"] is DBNull))
                            {
                                fTSetup.DutName = reader["DutcardB"].ToString();
                            }
                            //if (!(reader["TestBoxA"] is DBNull)) fTSetup.TestBord = reader["TestBoxA"].ToString();
                            if (!(reader["TestFlow"] is DBNull)) fTSetup.Flow = reader["TestFlow"].ToString().Trim();
                            if (!(reader["ProgramName"] is DBNull)) fTSetup.TestProgram = reader["ProgramName"].ToString().Trim();
                            if (!(reader["OptionName1"] is DBNull)) fTSetup.Option1 = reader["OptionName1"].ToString().Trim();
                            if (!(reader["OptionName2"] is DBNull)) fTSetup.Option2 = reader["OptionName2"].ToString().Trim();
                            if (!(reader["DeviceName"] is DBNull)) fTSetup.DeviceName = reader["DeviceName"].ToString().Trim();
                            if (!(reader["LOT1"] is DBNull)) fTSetup.Production_LotNo = reader["LOT1"].ToString().Trim();
                            if (!(reader["DEVICE1"] is DBNull)) fTSetup.Production_LotDevice = reader["DEVICE1"].ToString().Trim();

                            for (int i = 2; i <= 10; i++)
                            {
                                FTWip fTWipLot = new FTWip();
                                if (!(reader["LOT" + i] is DBNull)) fTWipLot.Lot_no = reader["LOT" + i].ToString().Trim();
                                if (!(reader["DEVICE" + i] is DBNull)) fTWipLot.DeviceName = reader["DEVICE" + i].ToString().Trim();
                                fTSetup.LotQueue.Add(fTWipLot);
                            }

                            if (!(reader["DelayLot"] is DBNull)) fTSetup.Production_DelayLot = reader["DelayLot"].ToString().Trim();
                            if (!(reader["Lot1Date"] is DBNull)) fTSetup.Production_Date = Convert.ToDateTime(reader["Lot1Date"]);
                            if (!(reader["Lot1Date"] is DBNull)) fTSetup.Production_Time = Convert.ToDateTime(reader["Lot1Date"]);
                            if (!(reader["Lot1Date"] is DBNull)) fTSetup.Production_Datetime = Convert.ToDateTime(reader["Lot1Date"]);
                            lstFTSetup.Add(fTSetup);

                        } conn.Close();
                        // List<FTWip> lstFTWips = FTWips();
                        // List<FTWip> lstFTWipOut = new List<FTWip>();
                        // //List<FTWip> lstFTWipOut = lstFTWips.Where(x => !lstFTSetup.Where(p => p.DeviceName == x.DeviceName).Any()).ToList();


                        // var ColorList = lstFTWips.Select(x => new { x.DeviceName }).Distinct().ToList();
                        // int countColor = 1;
                        // foreach (var deviceName in ColorList)
                        // {
                        //     List<FTWip> WipDevice = lstFTWips.Where(x => x.DeviceName == deviceName.DeviceName).ToList();
                        //     List<FTSetup> ColorOnMc = lstFTSetup.Where(x => x.DeviceName == deviceName.DeviceName).ToList();

                        //     foreach (var item in WipDevice)
                        //     {
                        //         item.S_Color = "A" + countColor;

                        //     }
                        //     foreach (var item in ColorOnMc)
                        //     {
                        //         item.Mc_Color = "A" + countColor;
                        //     }
                        //     countColor++;
                        // }

                        // for (int J = 1; J <= 4; J++)
                        // {
                        //     List<FTSetup> lstFTSetupAuto1 = lstFTSetup.Where(x => x.Flow == "AUTO" +J).OrderBy(x => x.MCNo).ToList(); //หาเครื่องกรอก Auto
                        //     List<FTWip> lstFTWipsAuto1 = lstFTWips.Where(x => x.JobName == "AUTO" +J).OrderBy(x => x.Lot_no).ToList();

                        //     var machineDevicesList = lstFTSetupAuto1.Select(x => new { x.DeviceName }).Distinct().ToList();


                        //     var wipLotOutPlan = lstFTWipsAuto1.Where(x => !machineDevicesList.Where(y => y.DeviceName == x.DeviceName).Any()).ToList();

                        //     foreach (var item in wipLotOutPlan)
                        //     {
                        //         lstFTWipOut.Add(item);
                        //     }

                        //     for (int i = 0; i < machineDevicesList.Count(); i++)
                        //     {

                        //     }

                        //     foreach (var deviceName in machineDevicesList)
                        //     {

                        //         List<FTSetup > McDevice = lstFTSetupAuto1.Where(x => x.DeviceName == deviceName.DeviceName).ToList(); //กรอก Device
                        //         List<FTWip> WipDevice = lstFTWipsAuto1.Where(x => x.DeviceName == deviceName.DeviceName).ToList();

                        //         //List<FTWip> WipOtherDevice = lstFTWipsAuto1.Where(x => !DevicesList.Where(y => y.DeviceName == x.DeviceName).Any()).ToList();

                        //         int count = 1;
                        //         foreach (var item in WipDevice)
                        //         {
                        //             int sequence = count % McDevice.Count;
                        //             if (sequence != 0)
                        //             {
                        //                 item.Sequence = sequence;
                        //             }
                        //             else
                        //             {
                        //                 item.Sequence = McDevice.Count;
                        //             }
                        //             count++;
                        //         }


                        //         int countMc = 1;
                        //         foreach (var mcData in McDevice.ToArray()) //เอาลอ็อตเข้าQue
                        //         {
                        //             List<FTWip> lstFTWipsAuto = new List<FTWip>();
                        //             for (int i = 2; i <= 10; i++)
                        //             {
                        //                 List<FTWip> lstFTWipsAuto1OnMc = WipDevice.Where(x => x.Sequence == countMc).ToList();
                        //                 foreach (var lotSequence in lstFTWipsAuto1OnMc)
                        //                 {
                        //                     lstFTWipsAuto.Add(lotSequence);
                        //                 }
                        //                 for (int k = 0; k < 9 - lstFTWipsAuto1OnMc.Count; k++)
                        //                 {
                        //                     FTWip fTWip = new FTWip();
                        //                     fTWip.DeviceName = "";
                        //                     fTWip.Lot_no = "";
                        //                     lstFTWipsAuto.Add(fTWip);
                        //                 }

                        //             }
                        //             mcData.LotQueue = lstFTWipsAuto;
                        //             //mcData.LotOutPlan = WipOtherDevice;
                        //             countMc++;
                        //         }
                        //     }
                        //     var result = lstFTWipsAuto1.Where(x => !machineDevicesList.Where(y => y.DeviceName == x.DeviceName).Any()).ToList();
                        // } //lot wip add to Que


                        //// lstFTWips.Where(x => x != lstFTSetup.ToList())
                        // List<LotFTinMc> lotFTinMcs = LotFTinMcs();

                        // foreach (var item in lstFTSetup)
                        // {
                        //     LotFTinMc onMc = lotFTinMcs.Where(x => x.MCName == item.MCNo).FirstOrDefault();
                        //     if (onMc == null)
                        //         continue;
                        //     item.Production_LotNo = onMc.LotNo;
                        //     item.Production_LotDevice = onMc.Device;
                        //     DateTime? production_Date = null;
                        //     if (onMc.ProcessState == FTSetup.State.Run)
                        //     {

                        //         if (item.Flow == "AUTO1")
                        //         {
                        //             production_Date = onMc.Updated_time.Value + onMc.timeAuto1;
                        //         }
                        //         else if (item.Flow == "AUTO2")
                        //         {
                        //             production_Date = onMc.Updated_time.Value + onMc.timeAuto2;
                        //         }
                        //         else if (item.Flow == "AUTO3")
                        //         {
                        //             production_Date = onMc.Updated_time.Value + onMc.timeAuto3;
                        //         }
                        //         else if (item.Flow == "AUTO4")
                        //         {
                        //             production_Date = onMc.Updated_time.Value + onMc.timeAuto4;
                        //         }
                        //         if (production_Date.HasValue)
                        //         {
                        //             item.Production_Date = production_Date.Value;
                        //             item.Production_Time = production_Date.Value;
                        //         }
                        //     }

                        //     item.Status = onMc.ProcessState;
                        // }


                        return lstFTSetup;
                    }
                }
            }
        }
        public IEnumerable<FTWip> FTWips
        {

            get
            {


                List<FTWip> lstFTWip = new List<FTWip>();
                var conn = new SqlConnection(Properties.Settings.Default.DBConnect);
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "[StoredProcedureDB].[dbo].[sp_get_scheduler_ft_wip]";
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FTWip ftWip = new FTWip();

                            if (!(reader["MCName"] is DBNull)) ftWip.MCName = reader["MCName"].ToString().Trim();
                            if (!(reader["lot_no"] is DBNull)) ftWip.Lot_no = reader["lot_no"].ToString().Trim();
                            if (!(reader["DeviceName"] is DBNull)) ftWip.DeviceName = reader["DeviceName"].ToString().Trim();
                            if (!(reader["MethodPkgName"] is DBNull)) ftWip.PKName = reader["MethodPkgName"].ToString().Trim();
                            if (!(reader["JobName"] is DBNull)) ftWip.JobName = reader["JobName"].ToString().Trim();
                            if (!(reader["updated_at"] is DBNull)) ftWip.Updated_at = reader["updated_at"].ToString().Trim();
                            if (!(reader["Kpcs"] is DBNull)) ftWip.Kpcs = int.Parse(reader["Kpcs"].ToString().Trim());
                            if (!(reader["state"] is DBNull)) {
                               string state = reader["state"].ToString().Trim();
                                if (state == "0" || state == "100")
                                {
                                    ftWip.Lot_State = FTWip.LotState.Wip;
                                }
                                else if(state == "1" || state == "101")
                                {
                                    ftWip.Lot_State = FTWip.LotState.Setup;
                                }
                                else if (state =="2" || state == "102")
                                {
                                    ftWip.Lot_State = FTWip.LotState.Start;
                                }else
                                {
                                    ftWip.Lot_State = FTWip.LotState.Other;
                                }
                            }
                            lstFTWip.Add(ftWip);
                        }
                        conn.Close();
                    }
                    return lstFTWip;
                }
                //return null;
            }
        }
        public IEnumerable<LotFTinMc> LotFTinMcs
        {

            get
            {
                List<LotFTinMc> lstLotFTinMc = new List<LotFTinMc>();
                var conn = new SqlConnection(Properties.Settings.Default.DBConnect);
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "[StoredProcedureDB].[dbo].[sp_get_scheduler_lot_in_machine]";
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LotFTinMc lotFTinMc = new LotFTinMc();

                            if (!(reader["MCName"] is DBNull)) lotFTinMc.MCName = reader["MCName"].ToString().Trim();
                            if (!(reader["DeviceName"] is DBNull)) lotFTinMc.Device = reader["DeviceName"].ToString().Trim();
                            if (!(reader["lot_no"] is DBNull)) lotFTinMc.LotNo = reader["lot_no"].ToString().Trim();
                            if (!(reader["process_state"] is DBNull))
                            {
                                if (reader["process_state"].ToString() == "1")
                                    lotFTinMc.ProcessState = FTSetup.State.Setup;
                                else if (reader["process_state"].ToString() == "2")
                                    lotFTinMc.ProcessState = FTSetup.State.Run;

                            }
                            if (!(reader["update_time"] is DBNull)) lotFTinMc.Updated_time = Convert.ToDateTime(reader["update_time"]);


                            if (!(reader["timeAuto1"] is DBNull)) lotFTinMc.timeAuto1 = new TimeSpan(
                                int.Parse(reader["timeAuto1"].ToString().Split(' ')[1].Split('.')[0]),
                                int.Parse(reader["timeAuto1"].ToString().Split(' ')[1].Split('.')[1]),
                                 0
                                );
                            if (!(reader["timeAuto2"] is DBNull) && reader["timeAuto2"].ToString() != "") lotFTinMc.timeAuto2 = new TimeSpan(
                                int.Parse(reader["timeAuto2"].ToString().Split(' ')[1].Split('.')[0]),
                                int.Parse(reader["timeAuto2"].ToString().Split(' ')[1].Split('.')[1]),
                                0
                                );
                            if (!(reader["timeAuto3"] is DBNull) && reader["timeAuto3"].ToString() != "") lotFTinMc.timeAuto3 = new TimeSpan(
                                int.Parse(reader["timeAuto3"].ToString().Split(' ')[1].Split('.')[0]),
                                int.Parse(reader["timeAuto3"].ToString().Split(' ')[1].Split('.')[1]),
                                0
                                );
                            if (!(reader["timeAuto4"] is DBNull) && reader["timeAuto4"].ToString() != "") lotFTinMc.timeAuto4 = new TimeSpan(
                                int.Parse(reader["timeAuto4"].ToString().Split(' ')[1].Split('.')[0]),
                                int.Parse(reader["timeAuto4"].ToString().Split(' ')[1].Split('.')[1]),
                                0
                                );
                            lstLotFTinMc.Add(lotFTinMc);
                        }
                        conn.Close();
                    }
                    return lstLotFTinMc;
                }

            }
        }
        public IEnumerable<FTDenpyo> Denpyos
        {
            get
            {
                List<FTDenpyo> fTDenpyos = new List<FTDenpyo>();
                var conn = new SqlConnection(Properties.Settings.Default.DBConnect);
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "[StoredProcedureDB].[dbo].[sp_get_scheduler_ft_time_wip]";
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // FTWip ftWip = new FTWip();
                            FTDenpyo denpyo = new FTDenpyo();
                            if (!(reader["PKG"] is DBNull)) denpyo.PKGName = reader["PKG"].ToString().Trim();
                            if (!(reader["DeviceName"] is DBNull)) denpyo.DeviceName = reader["DeviceName"].ToString().Trim();
                            if (!(reader["job_Id"] is DBNull)) denpyo.JobId = reader["job_Id"].ToString().Trim();
                            if (!(reader["timeAuto1"] is DBNull))
                            { denpyo.A1 = float.Parse(reader["timeAuto1"].ToString().Split(' ')[1]); }
                            else
                            {
                                denpyo.A1 = 0;
                            }
                            if (!(reader["timeAuto2"] is DBNull) && reader["timeAuto2"].ToString() != "")
                            { denpyo.A2 = float.Parse(reader["timeAuto2"].ToString().Split(' ')[1]); }
                            else
                            {
                                denpyo.A2 = 0;
                            }
                            if (!(reader["timeAuto3"] is DBNull) && reader["timeAuto3"].ToString() != "")
                            { denpyo.A3 = float.Parse(reader["timeAuto3"].ToString().Split(' ')[1]); }
                            else
                            {
                                denpyo.A3 = 0;
                            }
                            if (!(reader["timeAuto4"] is DBNull) && reader["timeAuto4"].ToString() != "")
                            { denpyo.A4 = float.Parse(reader["timeAuto4"].ToString().Split(' ')[1]); }
                            else
                            {
                                denpyo.A4 = 0;
                            }
                            fTDenpyos.Add(denpyo);
                        }
                        conn.Close();
                    }
                    return fTDenpyos;
                }
            }
        }

        public IEnumerable<FTMachineSchedulerSetup> FTSchedulerSetup(List<string> mcNoList)
        {


            List<FTMachineSchedulerSetup> ftSchedulerSetupList = new List<FTMachineSchedulerSetup>();
            var conn = new SqlConnection(Properties.Settings.Default.DBConnect);

            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[StoredProcedureDB].[dbo].[sp_get_scheduler_sequence]";


                foreach (var machineNo in mcNoList)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@machine_no", machineNo);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //FTMachineSchedulerSetup ftSchedulerSetup = new FTMachineSchedulerSetup()
                            //{

                            //    Priority = (int)reader["priority"],
                            //    MachineNo = ((string)reader["mc_no"]).Trim(),
                            //    Sequence = (byte)reader["sequence"],
                            //    DeviceChange = ((string)reader["device_set"]).Trim(),
                            //    //   DeviceNow = ((string)reader["device_now"]).Trim(),
                            //    DateChange = (DateTime)reader["device_set_date"],
                            //    MachienDisable = false
                            //};
                            FTMachineSchedulerSetup ftSchedulerSetup = new FTMachineSchedulerSetup();
                            ftSchedulerSetup.Priority = (int)reader["priority"];
                            ftSchedulerSetup.MachineNo = ((string)reader["mc_no"]).Trim();
                            ftSchedulerSetup.Sequence = (byte)reader["sequence"];
                            ftSchedulerSetup.DeviceChange = ((string)reader["device_set"]).Trim();
                            ftSchedulerSetup.DeviceNow = ((string)reader["device_now"]).Trim();
                            ftSchedulerSetup.DateChange = (DateTime)reader["device_set_date"];
                            ftSchedulerSetup.MachienDisable = false;
                            ftSchedulerSetupList.Add(ftSchedulerSetup);
                            //if (!(reader["lot_no"] is DBNull)) ftWip.Lot_no = reader["lot_no"].ToString().Trim();
                            //if (!(reader["DeviceName"] is DBNull)) ftWip.DeviceName = reader["DeviceName"].ToString().Trim();
                            //if (!(reader["MethodPkgName"] is DBNull)) ftWip.PKName = reader["MethodPkgName"].ToString().Trim();
                            //if (!(reader["JobName"] is DBNull)) ftWip.JobName = reader["JobName"].ToString().Trim();
                            //if (!(reader["updated_at"] is DBNull)) ftWip.Updated_at = reader["updated_at"].ToString().Trim();
                            //if (!(reader["Kpcs"] is DBNull)) ftWip.Kpcs = int.Parse(reader["Kpcs"].ToString().Trim());
                            //lstFTWip.Add(ftWip);
                        }

                    }
                }

                conn.Close();
                return ftSchedulerSetupList;
            }
        }
        public void SaveUpdate(string McNo, int Sequence, string Device, string DeviceChange)
        {
            var conn = new SqlConnection(Properties.Settings.Default.DBConnect);
            using (var cmd = conn.CreateCommand())
            {
                //cmd.CommandText = "SELECT * FROM [APCSProDWH].[cac].[wip_monitor_delay_lot_condition_detail] WHERE lot_no = '" + SaveUpdateDelaylotCon.lot + "'";
                cmd.CommandText = "INSERT INTO [DBx].[dbo].[scheduler_setup]([mc_no],[sequence],[device_change],[device_now],[date_change])" +
                    "VALUES(@McNo,@Sequence,@Device_change,@Device_now,GETDATE())";
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    conn.Close();
                    if (reader != null)
                    {
                        conn.Open();
                        using (var cmd2 = conn.CreateCommand())
                        {
                            cmd2.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd2.CommandText = "[StoredProcedureDB].[cac].[sp_set_wip_monitor_delay_lot_condition_table]";

                            cmd2.Parameters.Add("@McNo", System.Data.SqlDbType.Int).Value = McNo;
                            cmd2.Parameters.Add("@Sequence", System.Data.SqlDbType.VarChar).Value = Sequence;
                            cmd2.Parameters.Add("@Device_change", System.Data.SqlDbType.VarChar).Value = DeviceChange;
                            cmd2.Parameters.Add("@Device_now", System.Data.SqlDbType.VarChar).Value = Device;

                            cmd2.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}