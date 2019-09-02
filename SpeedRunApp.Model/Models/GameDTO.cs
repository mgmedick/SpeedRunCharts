using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using SpeedrunComSharp.Common;
using System.Linq;
using SpeedrunComSharp.Model;

namespace SpeedRunApp.Model
{
    public class GameDTO
    {
        public GameDTO(Game game)
        {
            ID = game.ID;
            Name = game.Name;
            JapaneseName = game.JapaneseName;
            Abbreviation = game.Abbreviation;
            WebLink = game.WebLink;
            YearOfRelease = game.YearOfRelease;
            Ruleset = new RulesetDTO(game.Ruleset);
            IsRomHack = game.IsRomHack;
            CreationDate = game.CreationDate;
            runs = new Lazy<IEnumerable<RunDTO>>(() => Common.GetRunsFromLazy(game.runs));
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string JapaneseName { get; set; }
        public string Abbreviation { get; set; }
        public Uri WebLink { get; set; }
        public int? YearOfRelease { get; set; }
        public RulesetDTO Ruleset { get; set; }
        public bool IsRomHack { get; set; }
        public DateTime? CreationDate { get; set; }
        private Lazy<IEnumerable<RunDTO>> runs { get; set; }
        public IEnumerable<RunDTO> Runs { get { return runs.Value; } }

    }
}
