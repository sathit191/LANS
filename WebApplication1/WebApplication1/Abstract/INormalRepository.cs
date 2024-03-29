﻿using System;
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
    }
}
