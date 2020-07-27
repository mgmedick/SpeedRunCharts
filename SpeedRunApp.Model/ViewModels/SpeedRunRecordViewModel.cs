using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.Helpers;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunRecordViewModel : SpeedRunViewModel
    {
        public SpeedRunRecordViewModel(SpeedRunRecord record) : base((SpeedRun)record)
        {
            RankString = record.Rank.ToOrdinalString();
        }

        public string RankString { get; set; }
    }
}
