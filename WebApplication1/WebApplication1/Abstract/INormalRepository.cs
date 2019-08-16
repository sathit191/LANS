using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Abstract
{
    public interface INormalRepository
    {
        IEnumerable<FTSetup> fTSetups { get; }
        IEnumerable<LotFTinMc> LotFTinMcs { get; }
        IEnumerable<FTWip> FTWips { get; }
        //IEnumerable<FTDenpyo> Denpyos { get; }
        IEnumerable<FTMachineSchedulerSetup> FTSchedulerSetup(List<int> mcNoList);
        IEnumerable<Accumulator_Plan> Plan { get; }
        IEnumerable<McWIP> McWIP { get; }
        IEnumerable<TPWip> TPWips { get; }
        IEnumerable<TPSetup> TPSetups { get; }
        IEnumerable<TPQAAccumulate> TPQAaccumulates { get; }
        IEnumerable<Accumulator_TP> Accumulators_TP { get; }
        //IEnumerable<Accumulator_QA> Accumulators_QA { get; }
        IEnumerable<TPLotinMc> LotTPinMcs { get; }
        void SaveUpdate(string McNo,int McId, int Sequence, string Device, string DeviceChange);
        void UpdateData(string McNo,int McId, int Sequence, string Device, string DeviceChange);
        void CencelTc(string McNo);
        void InsertOPRate(int McId, float OpRate);
        void UpdateOPRate(int McId, float OpRate);
        void UpdateOPRateGroup(int GroupId, float OpRate);
        void MCSetup(string PKGname);
        void SetTPCalculate(); // test
        IEnumerable<LimitFlow> LimitFlows { get; }
        IEnumerable<BMPMData> BMPMDatas { get; }


    }
}
