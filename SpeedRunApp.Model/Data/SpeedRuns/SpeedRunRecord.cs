using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class SpeedRunRecord : SpeedRun
    {
        //public SpeedRunRecord()
        //{
        //}

        //public SpeedRunRecord(SpeedRun run, int rank) : base(run)
        //{
        //    Rank = rank;
        //}

        public int? Rank { get; set; }
    }
}
