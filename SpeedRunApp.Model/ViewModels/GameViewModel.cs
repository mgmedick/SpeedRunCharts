using SpeedRunApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunCommon;

namespace SpeedRunApp.Model.ViewModels
{
    public class GameViewModel
    {
        public GameViewModel(GameView game)
        {
            ID = game.ID;
            Name = game.Name;
            JapaneseName = game.JapaneseName;
            YearOfRelease = game.YearOfRelease;
            CoverImageUri = game.CoverImageUrl;

            if (!string.IsNullOrWhiteSpace(game.CategoryTypes))
            {
                CategoryTypes = new List<IDNamePair>();
                foreach (var categoryType in game.CategoryTypes.Split("^^"))
                {
                    var values = categoryType.Split("||", 2);
                    CategoryTypes.Add(new IDNamePair { ID = values[0], Name = values[1] });
                }
            }

            if (!string.IsNullOrWhiteSpace(game.Categories))
            {
                Categories = new List<Category>();
                foreach (var category in game.Categories.Split("^^"))
                {
                    var values = category.Split("||", 3);
                    Categories.Add(new Category { ID = values[0], Name = values[1], CategoryTypeID = Convert.ToInt32((string)values[2]) });
                }
            }

            if (!string.IsNullOrWhiteSpace(game.Levels))
            {
                Levels = new List<IDNamePair>();
                foreach (var level in game.Levels.Split("^^"))
                {
                    var values = level.Split("||", 2);
                    Levels.Add(new IDNamePair { ID = values[0], Name = values[1] });
                }
            }

            if (!string.IsNullOrWhiteSpace(game.Variables))
            {
                Variables = new List<Variable>();
                foreach (var variable in game.Variables.Split("^^"))
                {
                    var values = variable.Split("||", 6);
                    var variableDisplay = new Variable { ID = values[0], Name = values[1], IsSubCategory = Convert.ToBoolean(values[2]), ScopeTypeID = Convert.ToInt32((string)values[3]), CategoryID = values[4], LevelID = values[5], };
                    variableDisplay.VariableValues = game.VariableValues?.Split("^^").Where(i => i.Split("||")[2] == variableDisplay.ID).Select(i => new VariableValue { ID = i.Split("||")[0], Name = i.Split("||")[1] });
                    Variables.Add(variableDisplay);
                }
            }

            if (!string.IsNullOrWhiteSpace(game.Platforms))
            {
                Platforms = new List<IDNamePair>();
                foreach (var platform in game.Platforms.Split("^^"))
                {
                    var values = platform.Split("||", 2);
                    Platforms.Add(new IDNamePair { ID = values[0], Name = values[1] });
                }
            }

            if (!string.IsNullOrWhiteSpace(game.Moderators))
            {
                Moderators = new List<IDNamePair>();
                foreach (var moderator in game.Moderators.Split("^^"))
                {
                    var values = moderator.Split("||", 2);
                    Moderators.Add(new IDNamePair { ID = values[0], Name = values[1] });
                }
            }
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string JapaneseName { get; set; }
        public string CoverImageUri { get; set; }
        public int? YearOfRelease { get; set; }
        public List<IDNamePair> CategoryTypes { get; set; }
        public List<Category> Categories { get; set; }
        public List<IDNamePair> Levels { get; set; }
        public List<Variable> Variables { get; set; }
        public List<IDNamePair> Platforms { get; set; }
        public List<IDNamePair> Moderators { get; set; }
        public string PlatformsString
        {
            get
            {
                return string.Join(", ", Platforms.Select(i => i.Name));
            }
        }
    }
}
