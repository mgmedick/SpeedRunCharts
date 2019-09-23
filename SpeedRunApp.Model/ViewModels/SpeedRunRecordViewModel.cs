using SpeedRunCommon;

namespace SpeedRunApp.Model
{
    public class SpeedRunRecordViewModel : SpeedRunViewModel
    {
        public SpeedRunRecordViewModel(SpeedRunRecordDTO record) : base((SpeedRunDTO)record)
        {
            RankString = record.Rank.ToOrdinalString();
        }

        public string RankString { get; set; }
    }
}
