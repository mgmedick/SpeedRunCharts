using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SpeedrunComSharp.Model;

namespace SpeedRunApp.Model.Data
{
    public class SpeedRunRecord : SpeedRun
    {
        //public SpeedRunRecord()
        //{
        //}

        //public SpeedRunRecordDTO(Record record) : base((Run)record)
        //{
        //    Rank = record.Rank;
        //}

        public int Rank { get; set; }
    }
}
