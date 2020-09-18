using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.Helpers;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunRecordViewModel : SpeedRunViewModel
    {
        public SpeedRunRecordViewModel(SpeedRunRecord record) : base((SpeedRun)record)
        {
            Rank = record.Rank;
        }

        public int? Rank { get; set; }

        public string RankString {
            get
            {
                return Rank?.ToOrdinalString();
            }
        }
    }
}
