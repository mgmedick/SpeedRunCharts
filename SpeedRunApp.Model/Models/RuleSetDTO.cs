using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using SpeedrunComSharp.Common;
using SpeedrunComSharp.Model;

namespace SpeedRunApp.Model
{
    public class RulesetDTO
    {
        public RulesetDTO(Ruleset ruleset)
        {
            ShowMilliseconds = ruleset.ShowMilliseconds;
            RequiresVerification = ruleset.RequiresVerification;
            RequiresVideo = ruleset.RequiresVideo;
            TimingMethods = ruleset.TimingMethods;
            DefaultTimingMethod = ruleset.DefaultTimingMethod;
            EmulatorsAllowed = ruleset.EmulatorsAllowed;
        }

        public bool ShowMilliseconds { get; set; }
        public bool RequiresVerification { get; set; }
        public bool RequiresVideo { get; set; }
        public ReadOnlyCollection<TimingMethod> TimingMethods { get; set; }
        public TimingMethod DefaultTimingMethod { get; set; }
        public bool EmulatorsAllowed { get; set; }
    }
}
