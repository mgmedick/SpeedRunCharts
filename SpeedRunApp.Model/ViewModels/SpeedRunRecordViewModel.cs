using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpeedRunCommon;
using SpeedRunApp.Model;

namespace SpeedRunApp.Model
{
    public class RecordViewModel : SpeedRunViewModel
    {
        public RecordViewModel(SpeedRunRecordDTO record) : base((SpeedRunDTO)record)
        {
            Rank = record.Rank;
        }

        public int Rank { get; set; }
    }
}
