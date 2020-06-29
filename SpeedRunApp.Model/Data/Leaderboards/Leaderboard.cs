using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SpeedRunApp.Model;
using SpeedRunCommon;

namespace SpeedRunApp.Model.Data
{
    public class Leaderboard
    {
        public Uri WebLink { get; set; }
        public EmulatorsFilter EmulatorFilter { get; set; }
        public bool VideoOnlyFilter { get; set; }
        public TimingMethod TimingMethodFilter { get; set; }
        public IEnumerable<VariableValueMapping> VariableValueMappingFilters { get; set; }
        public IEnumerable<SpeedRunRecord> Records { get; set; }
        public string GameID { get; set; }
        public string CategoryID { get; set; }
        public string LevelID { get; set; }
        public string PlatformFilterID { get; set; }
        public string RegionFilterID { get; set; }
        public IEnumerable<Player> Players { get; set; }
        public IEnumerable<Region> UsedRegions { get; set; }
        public IEnumerable<Platform> UsedPlatforms { get; set; }
        public IEnumerable<Variable> ApplicableVariables { get; set; }

        //embeds
        public Game Game { get; set; }
        public Category Category { get; set; }
        public Level Level { get; set; }
        public Platform PlatformFilter { get; set; }
        public Region RegionFilter { get; set; }

        //public string GameID { get; set; }
        //public Lazy<Game> game { get; set; }
        //public Game Game { get { return game.Value; } }
        //public string CategoryID { get; set; }
        //public Lazy<Category> category { get; set; }
        //public Category Category { get { return category.Value; } }
        //public string LevelID { get; set; }
        //public Lazy<Level> level { get; set; }
        //public Level Level { get { return level.Value; } }
        //public string PlatformIDOfFilter { get; set; }
        //public Lazy<Platform> platformFilter { get; set; }
        //public Platform PlatformFilter { get { return platformFilter.Value; } }
        //public string RegionIDOfFilter { get; set; }
        //public Lazy<Region> regionFilter { get; set; }
        //public Region RegionFilter { get { return regionFilter.Value; } }

        //public Leaderboard() { }
    }
}
